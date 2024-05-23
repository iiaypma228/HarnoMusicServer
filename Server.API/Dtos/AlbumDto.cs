namespace Server.API.Dtos;

public class HeadersDto
{
    public string status { get; set; }
    public int code { get; set; }
    public string error_message { get; set; }
    public string warnings { get; set; }
    public int results_count { get; set; }
}

public class AlbumDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string releasedate { get; set; }
    public int artist_id { get; set; }
    public string artist_name { get; set; }
    public string image { get; set; }
    public string zip { get; set; }
    public string shorturl { get; set; }
    public string shareurl { get; set; }
    public bool zip_allowed { get; set; }
}

public class AlbumsDto
{
    public HeadersDto headers { get; set; }
    public List<AlbumDto> results { get; set; }
}
