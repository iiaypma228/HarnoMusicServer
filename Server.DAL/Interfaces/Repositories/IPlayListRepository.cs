using Joint.Data.Models;

namespace Server.DAL.Interfaces.Repositories;

public interface IPlayListRepository : IRepository<PlayList>
{
    Task<List<PlayList>> GetFilledAllPlayList();
}