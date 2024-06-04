using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Server.DAL.Interfaces.Repositories;
using Server.DAL.Models;

namespace Server.DAL.Repositories;

public class TrackHistoryRepository : Repository<TrackHistory>, ITrackHistoryRepository
{
    public TrackHistoryRepository(ServerContext context) : base(context)
    {
    }

    public async Task<List<TrackHistory>> GetOrderedFavoriteHistoryTracks()
    {
        var result = this.context.TrackHistories
            .GroupJoin(this.context.Tracks, th => th.TrackId, t => t.Id,
                (t, th) => new { t, th })
            .SelectMany(o => o.th.DefaultIfEmpty(), (o, cl) => new { o.t, cl })
            .GroupJoin(this.context.Users, th => th.t.UserId, u => u.Id,
                (th, u) => new { th.t, th.cl, u })
            .SelectMany(o => o.u.DefaultIfEmpty(), (o, u) => new { o.t, o.cl, u })
            
            ;

        return await result.Select(i => new TrackHistory
        {
            Id = i.t.Id,
            TrackId = i.cl.Id,
            Track = i.cl,
            Date = i.t.Date
        }).OrderByDescending(i => i.Date).ToListAsync();
    }

    public async Task<List<PlayListHistory>> GetOrderedFavoriteHistoryPlayList()
    {
        var result = this.context.PlayListHistories
            .GroupJoin(this.context.PlayLists, th => th.PlayListId, t => t.Id,
                (t, th) => new { t, th })
            .SelectMany(o => o.th.DefaultIfEmpty(), (o, cl) => new { o.t, cl });

        return await result.Select(i => new PlayListHistory
        {
            Id = i.t.Id,
            PlayList = i.cl,
            PlayListId = i.cl.Id,
            Date = i.t.Date,
            TimeListening = i.t.TimeListening,
        }).OrderByDescending(i => i.Date).ToListAsync();
    }
}