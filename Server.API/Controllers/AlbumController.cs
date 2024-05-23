using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.Dtos;
using Server.API.Models;

namespace Server.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string ClientId;
    
    public AlbumController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        ClientId = configuration["JamendoClientId"];
    }
    
    [HttpGet("newAlbums")]
    public List<Album> GetNewAlbums(int? skip, int? take)
    {
        var res = _httpClient.GetAsync($"albums/?client_id={ClientId}&offset={skip}&limit={take}&format=json&order=releasedate_desc&imagesize=100").Result;
        res.EnsureSuccessStatusCode();
        var albumsDto = res.Content.ReadFromJsonAsync<AlbumsDto>().Result;

        return albumsDto.results.Select(i => new Album
        {
            Id = i.id,
            Name = i.name,
            ArtistId = i.artist_id,
            ArtistName = i.artist_name,
            ImageUrl = i.image
        }).ToList();

    }
}