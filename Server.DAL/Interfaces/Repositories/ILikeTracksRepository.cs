using Joint.Data.Models;

namespace Server.DAL.Interfaces.Repositories;

public interface ILikeTracksRepository : IRepository<LikeTracks>
{
    Task<List<Track>> GetLikeTracks(int userId);
}