using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.DAL.Models;

public class PlayListConfiguration : IEntityTypeConfiguration<PlayList>
{
    public void Configure(EntityTypeBuilder<PlayList> builder)
    {
        builder.ToTable(nameof(Tables.PlayList));
        builder.HasKey(i => i.Id);
        builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);
        builder.Ignore(i => i.User);
        builder.Ignore(i => i.Tracks);
    }
}