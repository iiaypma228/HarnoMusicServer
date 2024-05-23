using Joint.Data.Constants;

namespace Joint.Data.Models;

public class PlayList
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int UserId { get; set; }
    
    public User? User { get; set; }
    
    public string ImagePath { get; set; }
    
    public ResourceType ImagePathResourceType { get; set; }
    
    public bool IsPrivate { get; set; }
    
    public List<Track>? Tracks { get; set; }
     
}