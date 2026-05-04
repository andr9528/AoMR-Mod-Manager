using Manager.Abstractions.Startup;
using Manager.Persistence.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manager.Startup.Modules;

public class DatabaseContextStartupModule<TContext> : IServiceStartupModule
    where TContext : BaseDatabaseContext<TContext>
{
    public delegate void SetupOptionsDelegate(DbContextOptionsBuilder options);

    protected readonly bool migrateOnStartup;
    private readonly SetupOptionsDelegate setupOptions;
    protected ILogger<DatabaseContextStartupModule<TContext>>? logger;

    public DatabaseContextStartupModule(SetupOptionsDelegate setup, bool migrateOnStartup = true)
    {
        if (typeof(TContext) is {IsAbstract: true,})
        {
            throw new ArgumentException($"Invalid type argument supplied to '{nameof(TContext)}'");
        }

        this.migrateOnStartup = migrateOnStartup;
        setupOptions = setup ?? throw new ArgumentNullException(nameof(setup));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        logger = services.BuildServiceProvider().GetService<ILogger<DatabaseContextStartupModule<TContext>>>();

        services.AddDbContext<TContext>(options => setupOptions.Invoke(options));

        logger?.LogDebug("Completed Configuration of Database Services.");

        if (!migrateOnStartup)
        {
            return;
        }

        ServiceProvider? provider = services.BuildServiceProvider();
        using var context = provider.GetService<TContext>();
        context?.Database.Migrate();

        logger?.LogDebug("Completed Migration of Database.");
    }

    /// <inheritdoc />
    public string Name => "Database Module";
}
