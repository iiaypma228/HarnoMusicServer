namespace Server.API.Models;

public class Album
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ArtistId { get; set; }
    public string ArtistName { get; set; }
    public string ImageUrl { get; set; }
}