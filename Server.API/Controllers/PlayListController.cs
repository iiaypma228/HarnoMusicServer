using System.Security.Claims;
using Joint.Data.Constants;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.BLL.Interfaces;
using Server.DAL.Interfaces.Repositories;

namespace Server.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class PlayListController : ControllerBase
{
    private readonly IPlayListRepository _playListRepository;
    private readonly IPlayListTracksRepository _playListTracksRepository;
    private readonly ITrackService _trackService;
    private readonly IUserService _userService;

    public PlayListController(IPlayListRepository playListRepository, IUserService userService, ITrackService trackService, IPlayListTracksRepository playListTracksRepository)
    {
        _playListRepository = playListRepository;
        _userService = userService;
        _trackService = trackService;
        _playListTracksRepository = playListTracksRepository;
    }

    [HttpGet("myPlaylists")]
    public async  Task<List<PlayList>> GetMyPlayLists()
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var currentUser = _userService.ReadByEmail(claim.Value);
        var res = (await _playListRepository.GetFilledAllPlayList()).Where(i => i.UserId == currentUser.Id).ToList();
        return res;
    }
    
    [HttpPost("newPlayList")]
    public PlayList CreatePlayList(string name)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var currentUser = _userService.ReadByEmail(claim.Value);
        
        var playList = new PlayList
        {
            Id = 0,
            Name = name,
            UserId = currentUser.Id,
            User = currentUser,
            ImagePath = "",
            ImagePathResourceType = ResourceType.Local,
            IsPrivate = false,
            Tracks = new List<Track>()
        };
        _playListRepository.Create(playList);
        _playListRepository.Save();
        return playList;
    }

    [HttpPost("addTrackToPlayList")]
    public ActionResult AddTrackToPlayList([FromBody] Track track, int playListId)
    {
        var tarckExsist = _trackService.Read(i => i.Id == track.Id).FirstOrDefault();

        if (tarckExsist == null)
        {
            _trackService.Save(track);
            
        }
        
        _playListTracksRepository.Create(new PlayListTracks
        {
            Id = 0,
            PlayListId = playListId,
            PlayList = null,
            TrackId = track.Id,
            Track = track,
            PositionTrack = 0
        });
        _playListTracksRepository.Save();
        
        return Ok();
    }
    
    [HttpGet("playlistTracks")]
    public async Task<List<Track>> GetMyPlayLists(int id)
    {
        var playLists = await _playListRepository.GetFilledAllPlayList() ;

        return playLists.First(i => i.Id == id).Tracks;

    }
    
}