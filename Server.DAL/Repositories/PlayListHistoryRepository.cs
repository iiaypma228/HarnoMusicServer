using Joint.Data.Models;
using Server.DAL.Interfaces.Repositories;
using Server.DAL.Models;

namespace Server.DAL.Repositories;

public class PlayListHistoryRepository : Repository<PlayListHistory>, IPlayListHistoryRepository
{
    public PlayListHistoryRepository(ServerContext context) : base(context)
    {
    }
}