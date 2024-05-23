using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.DAL.Models;

public class LikeTracksConfiguration : IEntityTypeConfiguration<LikeTracks>
{
    public void Configure(EntityTypeBuilder<LikeTracks> builder)
    {
        builder.ToTable(nameof(Tables.LikeTracks)).HasKey(i => new {i.UserId, i.TrackId}) ;

        builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);
        builder.Ignore(i => i.User);
        
        builder.HasOne<Track>().WithMany().HasForeignKey(i => i.TrackId);
        builder.Ignore(i => i.Track);
    }
}