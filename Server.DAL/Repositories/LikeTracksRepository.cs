using Joint.Data.Constants;
using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Server.DAL.Interfaces.Repositories;

namespace Server.DAL.Repositories;

public class LikeTracksRepository : Repository<LikeTracks>, ILikeTracksRepository
{
    public LikeTracksRepository(ServerContext context) : base(context)
    {
    }

    public async Task<List<Track>> GetLikeTracks(int userId)
    {
        var res =  await this.context.LikeTracks
            .Where(i => i.UserId == userId)
            .GroupJoin(context.Tracks, p => p.TrackId, t => t.Id, (lt, t) => new { lt, t })
            .SelectMany(o => o.t.DefaultIfEmpty(), (lt, t) => new { lt.lt, t })
            .Select(i => i.t).ToListAsync();
        return res;
    }
}