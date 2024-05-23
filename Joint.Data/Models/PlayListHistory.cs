namespace Joint.Data.Models;

public class PlayListHistory
{
    public int Id { get; set; }
    
    public int PlayListId { get; set; }
    
    public PlayList? PlayList { get; set; }
    
    public DateTime Date { get; set; }
    
    public TimeSpan TimeListening { get; set; }
    
}