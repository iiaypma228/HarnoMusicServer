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

namespace Server.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class TrackController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string ClientId;
    private readonly IUserService _userService;
    private readonly ILikeTracksRepository _likeTracksRepository;
    private readonly ITrackService _trackService;
    
    public TrackController(HttpClient httpClient, IConfiguration configuration, IUserService userService, ILikeTracksRepository likeTracksRepository, ITrackService trackService)
    {
        _httpClient = httpClient;
        ClientId = configuration["JamendoClientId"];
        _userService = userService;
        _likeTracksRepository = likeTracksRepository;
        _trackService = trackService;
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
            AlbumId = i.album_id,
            AudioUrl = i.audio,
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
            AlbumId = i.album_id,
            AudioUrl = i.audio,
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