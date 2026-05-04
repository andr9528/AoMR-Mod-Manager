using Manager.Models.Entity.Model;
using Manager.Persistence.Configuration;
using Manager.Persistence.Core;
using Manager.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Manager.Persistence;

public class ManagerDatabaseContext : BaseDatabaseContext<ManagerDatabaseContext>
{
    public virtual DbSet<Playset> Playsets { get; set; }

    public virtual DbSet<Mod> Mods { get; set; }

    public virtual DbSet<PlaysetMod> PlaysetMods { get; set; }

    /// <inheritdoc />
    public ManagerDatabaseContext(DbContextOptions<ManagerDatabaseContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PlaysetConfiguration(DatabaseType.SQL_LITE));
        modelBuilder.ApplyConfiguration(new ModConfiguration(DatabaseType.SQL_LITE));
        modelBuilder.ApplyConfiguration(new PlaysetModConfiguration(DatabaseType.SQL_LITE));
    }
}
