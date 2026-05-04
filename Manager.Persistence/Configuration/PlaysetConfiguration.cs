using Manager.Models.Entity.Model;
using Manager.Persistence.Core;
using Manager.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manager.Persistence.Configuration;

public class PlaysetConfiguration : EntityConfiguration<Playset>
{
    public PlaysetConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Playset> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
