using System.Diagnostics.CodeAnalysis;
using Manager.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Manager.Persistence.Core;

public abstract class BaseDatabaseContext<TContext> : DbContext where TContext : DbContext
{
    private readonly DbContextOptions options;

    protected BaseDatabaseContext([NotNull] DbContextOptions<TContext> options) : base(options)
    {
        this.options = options;
    }

    public override int SaveChanges()
    {
        UpdateDatetime();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateDatetime();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateDatetime();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateDatetime();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateDatetime()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e is { Entity: IEntity, State: EntityState.Added or EntityState.Modified, });

        foreach (EntityEntry entityEntry in entries)
        {
            ((IEntity)entityEntry.Entity).UpdatedDateTime = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
                ((IEntity)entityEntry.Entity).CreatedDateTime = DateTime.UtcNow;
        }
    }
}
