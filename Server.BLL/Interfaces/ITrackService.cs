using Joint.Data.Models;

namespace Server.BLL.Interfaces;

public interface ITrackService : ICRUDService<Track>
{
    void SaveLikeTrack(User user, Track track);
}