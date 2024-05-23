using Joint.Data.Models;
using Server.DAL.Interfaces.Repositories;

namespace Server.DAL.Repositories;

public class PlayListTracksRepository : Repository<PlayListTracks>, IPlayListTracksRepository
{
    public PlayListTracksRepository(ServerContext context) : base(context)
    {
    }
}