using System.Security.Claims;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.API.Dtos;
using Server.API.Models;
using Server.BLL.Interfaces;
using Server.BLL.Services;
using Server.DAL.Interfaces.Repositories;
using Server.DAL.Models;

namespace Server.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class TrackController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string ClientId;
    private readonly IUserService _userService;
    private readonly ILikeTracksRepository _likeTracksRepository;
    private readonly ITrackHistoryRepository _trackHistoryRepository;
    private readonly ITrackService _trackService;
    
    public TrackController(HttpClient httpClient, IConfiguration configuration, IUserService userService, ILikeTracksRepository likeTracksRepository, ITrackService trackService, ITrackHistoryRepository trackHistoryRepository)
    {
        _httpClient = httpClient;
        ClientId = configuration["JamendoClientId"];
        _userService = userService;
        _likeTracksRepository = likeTracksRepository;
        _trackService = trackService;
        _trackHistoryRepository = trackHistoryRepository;
    }

    [HttpGet("likeTracks")]
    public async  Task<List<Joint.Data.Models.Track>> GetLikesTrack()
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var user = _userService.ReadByEmail(claim.Value);

        var track = await _likeTracksRepository.GetLikeTracks(user.Id);

        return track;
    }
    
    
    [HttpGet("newTracks")]
    public List<Joint.Data.Models.Track> GetNewTracks(int? skip, int? take)
    {
        var res = _httpClient.GetAsync($"tracks/?client_id={ClientId}&offset={skip}&limit={take}&format=json&order=releasedate_desc").Result;
        res.EnsureSuccessStatusCode();
        var tracksDto = res.Content.ReadFromJsonAsync<TracksDto>().Result;

        return tracksDto.results.Select(i => new Joint.Data.Models.Track
        {
            Id = i.id,
            Name = i.name,
            Duration = i.duration,
            ArtistName = i.artist_name,
            AlbumName = i.album_name,
            AlbumId = string.IsNullOrEmpty(i.album_id) ? 0 : int.Parse(i.album_id),
            AudioUrl = i.audio,
            AudioDownload = i.audiodownload_allowed ? i.audiodownload : null,
            AlbumImage = i.album_image,
            Image = i.image,

        }).ToList();

    }

    [HttpGet("trackByTags")]
    public List<Joint.Data.Models.Track> GetTracksByTags(int? skip, int? take, string tags)
    {
        var res = _httpClient.GetAsync($"tracks/?client_id={ClientId}&offset={skip}&limit={take}&format=json&tags={tags}").Result;
        res.EnsureSuccessStatusCode();
        var tracksDto = res.Content.ReadFromJsonAsync<TracksDto>().Result;

        return tracksDto.results.Select(i => new Joint.Data.Models.Track
        {
            Id = i.id,
            Name = i.name,
            Duration = i.duration,
            ArtistName = i.artist_name,
            AlbumName = i.album_name,
            AlbumId = string.IsNullOrEmpty(i.album_id) ? 0 : int.Parse(i.album_id),
            AudioUrl = i.audio,
            AudioDownload = i.audiodownload_allowed ? i.audiodownload : null,
            AlbumImage = i.album_image,
            Image = i.image,

        }).ToList();

    }
    
    [HttpGet("trackByQuery")]
    public List<Joint.Data.Models.Track> GetTracksByQuery(int? skip, int? take, string query)
    {
        var res = _httpClient.GetAsync($"tracks/?client_id={ClientId}&offset={skip}&limit={take}&format=json&search={query}").Result;
        res.EnsureSuccessStatusCode();
        var tracksDto = res.Content.ReadFromJsonAsync<TracksDto>().Result;

        return tracksDto.results.Select(i => new Joint.Data.Models.Track
        {
            Id = i.id,
            Name = i.name,
            Duration = i.duration,
            ArtistName = i.artist_name,
            AlbumName = i.album_name,
            AlbumId = string.IsNullOrEmpty(i.album_id) ? 0 : int.Parse(i.album_id),
            AudioUrl = i.audio,
            AudioDownload = i.audiodownload_allowed ? i.audiodownload : null,
            AlbumImage = i.album_image,
            Image = i.image,

        }).ToList();

    }
    
    [HttpPost("likeTracks")]
    public async Task<ActionResult> AddLikeTrack([FromBody] Track track)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var user = _userService.ReadByEmail(claim.Value);
        
        _trackService.SaveLikeTrack(user,track);

        return Ok();
    }
    
    [HttpPost("history")]
    public async Task<ActionResult> SaveHistory([FromBody] Track track)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var user = _userService.ReadByEmail(claim.Value);

        var tarckExsist = _trackService.Read(i => i.Id == track.Id).FirstOrDefault();

        if (tarckExsist == null)
        {
            _trackService.Save(track);
            
        }
        
        
        await _trackHistoryRepository.CreateAsync(new TrackHistory
        {
            Id = 0,
            UserId = user.Id,
            User = user,
            TrackId = track.Id,
            Track = track,
            Date = DateTime.Now
        });
        await _trackHistoryRepository.SaveAsync();

        return Ok();
    }

    [HttpGet("history")]
    public async Task<List<TrackHistory>> GetHistory()
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var user = _userService.ReadByEmail(claim.Value);
        var res = await _trackHistoryRepository.GetOrderedFavoriteHistoryTracks();  
        return res;
    }
    
    
    [HttpDelete("likeTracks")]
    public async Task<ActionResult> DelLikeTrack([FromBody] Track track)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var user = _userService.ReadByEmail(claim.Value);
        
        var exist = await _likeTracksRepository.Read(i => i.UserId == user.Id && i.TrackId == track.Id).FirstOrDefaultAsync();

        if (exist != null)
        {
            _likeTracksRepository.Delete(exist);
            await _likeTracksRepository.SaveAsync();
        }
        
        return Ok();
    }

}