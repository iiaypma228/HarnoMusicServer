using Joint.Data.Models;
using Server.DAL.Interfaces.Repositories;

namespace Server.DAL.Repositories;

public class TrackRepository :  Repository<Track>, ITrackRepository
{
    public TrackRepository(ServerContext context) : base(context)
    {
    }
}