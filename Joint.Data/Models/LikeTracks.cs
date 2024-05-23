namespace Joint.Data.Models;

public class LikeTracks
{
    public int UserId { get; set; }
    
    public User? User { get; set; }
    
    public int TrackId { get; set; }
    
    public Track? Track { get; set; }
}