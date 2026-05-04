using Manager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Manager.Persistence;

public sealed class ManagerDbDesignTimeContextFactory : IDesignTimeDbContextFactory<ManagerDatabaseContext>
{
    public ManagerDatabaseContext CreateDbContext(string[] args)
    {
        var configurationService = new ConfigurationService();

        var optionsBuilder = new DbContextOptionsBuilder<ManagerDatabaseContext>();

        configurationService.ConfigureDatabaseOptions(optionsBuilder);

        return new ManagerDatabaseContext(optionsBuilder.Options);
    }
}
