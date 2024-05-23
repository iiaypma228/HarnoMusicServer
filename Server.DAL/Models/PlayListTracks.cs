using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.DAL.Models;

public class PlayListTracksConfiguration : IEntityTypeConfiguration<PlayListTracks>
{
    public void Configure(EntityTypeBuilder<PlayListTracks> builder)
    {
        builder.ToTable(nameof(Tables.PlayListTracks));
        builder.HasKey(i => i.Id); 
        //builder.HasAlternateKey(i => new { i.PlayListId, i.TrackId });
        //builder.HasNoKey();

        builder.HasOne<PlayList>().WithMany().HasForeignKey(i => i.PlayListId);
        builder.Ignore(i => i.PlayList);
        
        builder.HasOne<Track>().WithMany().HasForeignKey(i => i.TrackId);
        builder.Ignore(i => i.Track);
        
    }
}