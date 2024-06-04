namespace Server.API.Dtos;

public class TrackDto
{
    public int id { get; set; }
    public string name { get; set; }
    public int duration { get; set; }
    public string artist_id { get; set; }
    public string artist_name { get; set; }
    public string artist_idstr { get; set; }
    public string album_name { get; set; }
    public string album_id { get; set; }
    public string license_ccurl { get; set; }
    public int position { get; set; }
    public string releasedate { get; set; }
    public string album_image { get; set; }
    public string audio { get; set; }
    public string audiodownload { get; set; }
    public string prourl { get; set; }
    public string shorturl { get; set; }
    public string shareurl { get; set; }
    public string waveform { get; set; }
    public string image { get; set; }
    public bool audiodownload_allowed { get; set; }
}


public class TracksDto
{
    public HeadersDto headers { get; set; }
    public List<TrackDto> results { get; set; }
}