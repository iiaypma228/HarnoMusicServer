using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.DAL.Models;

public class TrackHistoryConfiguration : IEntityTypeConfiguration<TrackHistory>
{
    public void Configure(EntityTypeBuilder<TrackHistory> builder)
    {
        builder.ToTable(nameof(Tables.TrackHistory));
        builder.HasKey(i => i.Id);
        builder.HasOne<Track>().WithMany().HasForeignKey(i => i.TrackId);
        builder.Ignore(i => i.Track);
    }
}