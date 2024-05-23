using System.Linq.Expressions;
using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Server.BLL.Interfaces;
using Server.DAL.Interfaces.Repositories;

namespace Server.BLL.Services;

public class TrackService : Service, ITrackService
{
    private readonly ITrackRepository _repository;
    private readonly ILikeTracksRepository _likeTracksRepository;
    
    
    public TrackService(ITrackRepository repository, ILikeTracksRepository likeTracksRepository)
    {
        _repository = repository;
        _likeTracksRepository = likeTracksRepository;
    }

    public void Save(Track item)
    {
        Save(new List<Track>(){item});
    }

    public void Save(IList<Track> items)
    {
        foreach (var item in items)
        {
            var exsist = _repository.Read(i => i.Id == item.Id).FirstOrDefault();

            if (exsist != null)
                _repository.Update(item);
            
            else
                _repository.Create(item);
        }
        _repository.Save();
    }

    public IList<Track> Read()
    {
        return _repository.Read().ToList();
    }

    public IList<Track> Read(Expression<Func<Track, bool>> where)
    {
        return _repository.Read(where).ToList();
    }

    public Track Read(object id)
    {
        return _repository.Read(i => i.Id == (int)id).FirstOrDefault();
    }

    public void Delete(Track item)
    {
        Delete(new List<Track>(){item});
    }

    public void Delete(IList<Track> items)
    {
        foreach (var item in items)
        {
            var exsist = _repository.Read(i => i.Id == item.Id).FirstOrDefault();

            if (exsist != null)
                _repository.Delete(item);

        }
        _repository.Save();
    }

    public void SaveLikeTrack(User user, Track track)
    {
        var exist = this.Read(track.Id);

        if (exist == null)
        {
            this.Save(track);
        }

        var isLiked = _likeTracksRepository.Read(i => i.TrackId == track.Id && user.Id == i.UserId).FirstOrDefault();

        if (isLiked == null)
        {
            this._likeTracksRepository.Create(new LikeTracks
            {
                UserId = user.Id,
                User = user,
                TrackId = track.Id,
                Track = track
            });
            
            _likeTracksRepository.Save();
        }
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}