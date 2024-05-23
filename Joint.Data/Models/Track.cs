using Joint.Data.Constants;

namespace Joint.Data.Models;

public class Track
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public string ArtistName { get; set; }
    public string AlbumName { get; set; }
    public int AlbumId { get; set; }
    public string AudioUrl { get; set; }
    public string AlbumImage { get; set; }
    public string Image { get; set; }
    
}