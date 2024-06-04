using Joint.Data.Models;

namespace Server.DAL.Models;

public class TrackHistory
{
    public int Id { get; set; }
    
    public int TrackId { get; set; }
    
    public int UserId { get; set; }
    
    public User? User { get; set; }
    
    public Track? Track { get; set; }
    
    public DateTime Date { get; set; }
    
}