using Manager.Models.Entity.Model;
using Manager.Persistence.Core;
using Manager.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manager.Persistence.Configuration;

public class PlaysetModConfiguration : EntityConfiguration<PlaysetMod>
{
    public PlaysetModConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<PlaysetMod> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => (Playset) x.Playset).WithMany(x => (ICollection<PlaysetMod>) x.PlaysetMods)
            .HasForeignKey(x => x.PlaysetId);

        builder.HasOne(x => (Mod) x.Mod).WithMany(x => (ICollection<PlaysetMod>) x.PlaysetMods)
            .HasForeignKey(x => x.ModId);

        builder.HasIndex(x => new {x.PlaysetId, x.ModId,}).IsUnique();
        builder.HasIndex(x => new {x.PlaysetId, x.Priority,}).IsUnique();
    }
}
