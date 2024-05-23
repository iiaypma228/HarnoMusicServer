using Joint.Data.Models;
using Server.DAL.Models;

namespace Server.DAL.Interfaces.Repositories;

public interface ITrackHistoryRepository : IRepository<TrackHistory>
{
    Task<List<TrackHistory>> GetOrderedFavoriteHistoryTracks();
    
    Task<List<PlayListHistory>> GetOrderedFavoriteHistoryPlayList();
}