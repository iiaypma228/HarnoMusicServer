using Joint.Data.Constants;
using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Server.DAL.Interfaces.Repositories;

namespace Server.DAL.Repositories;

public class PlayListRepository : Repository<PlayList>, IPlayListRepository
{
    public PlayListRepository(ServerContext context) : base(context)
    {
    }

    public async Task<List<PlayList>> GetFilledAllPlayList()
    {
        if (await context.PlayListTracks.AnyAsync())
        {
            return await context.PlayLists
                .GroupJoin(context.PlayListTracks, p => p.Id, t => t.PlayListId, (p, t) => new {p, t})
                .SelectMany(o => o.t.DefaultIfEmpty(), (p, t) => new { p.p, t })
                .GroupJoin(context.Tracks, p => p.t.TrackId, tr => tr.Id, (p ,tr) =>  new {p.p, p.t, tr} )    
                .SelectMany(o => o.tr.DefaultIfEmpty(), (p, tr) => new { p.p, p.t, tr })
                .GroupBy(i => i.t.PlayListId)
                //.Where(i => i.Any() || i.FirstOrDefault() == null)
                .Select(i => new Joint.Data.Models.PlayList
                {
                    Id = i.FirstOrDefault() != null ? i.FirstOrDefault().p.Id! : 0,
                    Name = i.FirstOrDefault() != null ? i.FirstOrDefault().p.Name : null,
                    UserId =  i.FirstOrDefault() != null ? i.FirstOrDefault().p.UserId : 0,
                    User = i.FirstOrDefault() != null ? i.FirstOrDefault().p.User : null,
                    ImagePath = i.FirstOrDefault() != null ? i.FirstOrDefault().p.ImagePath : "",
                    ImagePathResourceType = i.FirstOrDefault() != null ? i.FirstOrDefault().p.ImagePathResourceType : 0,
                    IsPrivate = i.FirstOrDefault() != null ? i.FirstOrDefault().p.IsPrivate : false,
                    Tracks = i.Select(i => i.tr).ToList()
                })
                .Where(i => i.Id != 0)
                .ToListAsync();
        }

        return (await context.PlayLists.ToListAsync()).Select(i =>
        {
            i.Tracks = new List<Track>();
            return i;
        }).ToList();
    }
}