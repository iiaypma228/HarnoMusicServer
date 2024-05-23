namespace Joint.Data.Models;

public class PlayListTracks
{
    public int Id { get; set; }
    public int PlayListId { get; set; }
    
    public PlayList? PlayList { get; set; }
    
    public int TrackId { get; set; }
    
    public Track? Track { get; set; }
    
    public int PositionTrack { get; set; }
}