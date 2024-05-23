using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Server.DAL.Models;

namespace Server.DAL;

public class ServerContext : DbContext
{
    public ServerContext(DbContextOptions options)
        : base(options)
    {
        //Database.EnsureDeleted();
        if (Database.EnsureCreated())
        {
            Users.Add(new User
            {
                Id = 1,
                Email = "guest",
                Username = "guest",
                Password = "guest"
            });
            this.SaveChanges();
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TrackConfiguration());
        modelBuilder.ApplyConfiguration(new PlayListConfiguration());
        modelBuilder.ApplyConfiguration(new PlayListTracksConfiguration());
        modelBuilder.ApplyConfiguration(new LikeTracksConfiguration());
        modelBuilder.ApplyConfiguration(new TrackHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new PlayListHistoryConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<PlayList> PlayLists { get; set; }
    public DbSet<PlayListTracks> PlayListTracks { get; set; }
    public DbSet<LikeTracks> LikeTracks { get; set; }
    public DbSet<TrackHistory> TrackHistories { get; set; }
    public DbSet<PlayListHistory> PlayListHistories { get; set; }
}