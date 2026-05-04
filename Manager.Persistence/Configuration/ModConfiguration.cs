using Manager.Models.Entity.Model;
using Manager.Persistence.Core;
using Manager.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manager.Persistence.Configuration;

public class ModConfiguration : EntityConfiguration<Mod>
{
    public ModConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Mod> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Author).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Path).IsRequired();
        builder.Property(x => x.LastUpdate).IsRequired();
        builder.Property(x => x.InstallTime).IsRequired();

        builder.Ignore(x => x.IsLocalMod);

        builder.HasIndex(x => x.Path).IsUnique();
    }
}
