using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.DAL.Models;

public class PlayListHistoryConfiguration : IEntityTypeConfiguration<PlayListHistory>
{
    public void Configure(EntityTypeBuilder<PlayListHistory> builder)
    {
        builder.ToTable(nameof(Tables.PlayListHistory));
        builder.HasKey(i => i.Id);
        builder.HasOne<PlayList>().WithMany().HasForeignKey(i => i.PlayListId);
        builder.Ignore(i => i.PlayList);
    }
}