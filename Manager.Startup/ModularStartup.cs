using Manager.Abstractions.Startup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manager.Startup;

public abstract class ModularStartup<TApplicationBuilder>
{
    private readonly ICollection<IServiceStartupModule> serviceModules;
    private readonly ICollection<IApplicationStartupModule<TApplicationBuilder>> applicationModules;
    private readonly Queue<Action<ILogger>> delayedLogActions = new();

    protected ModularStartup()
    {
        serviceModules = new List<IServiceStartupModule>();
        applicationModules = new List<IApplicationStartupModule<TApplicationBuilder>>();
    }

    public IServiceCollection Services { get; protected set; } = null!;
    public IServiceProvider ServiceProvider { get; protected set; } = null!;

    protected void QueueStartupLog(Action<ILogger> logAction)
    {
        ArgumentNullException.ThrowIfNull(logAction);
        delayedLogActions.Enqueue(logAction);
    }

    public void FlushStartupLogs()
    {
        ArgumentNullException.ThrowIfNull(ServiceProvider);

        var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
        ILogger logger = loggerFactory.CreateLogger(GetType());

        while (delayedLogActions.Count > 0)
        {
            var action = delayedLogActions.Dequeue();

            try
            {
                action(logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "A delayed startup log action failed.");
            }
        }
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    protected virtual void ConfigureApplication(TApplicationBuilder app)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    /// <param name="module"></param>
    /// <exception cref="ArgumentException">
    /// If the provided module does not implement either <see cref="IServiceStartupModule"/> or <see cref="IApplicationStartupModule{TApplicationBuilder}"/>.
    /// Or if the module has already been registered - Fail Fast approach.
    /// </exception>
    protected void AddModule<TModule>(TModule module) where TModule : class
    {
        ArgumentNullException.ThrowIfNull(module);

        var wasRegistered = false;

        if (module is IServiceStartupModule serviceModule && !serviceModules.Contains(serviceModule))
        {
            serviceModules.Add(serviceModule);
            wasRegistered = true;
            QueueStartupLog(logger => logger.LogDebug($"Registered '{serviceModule.Name}' as Service Startup module."));
        }

        if (module is IApplicationStartupModule<TApplicationBuilder> applicationModule &&
            !applicationModules.Contains(applicationModule))
        {
            applicationModules.Add(applicationModule);
            wasRegistered = true;
            QueueStartupLog(logger =>
                logger.LogDebug($"Registered '{applicationModule.Name}' as Application Startup module."));
        }

        if (!wasRegistered)
        {
            throw new ArgumentException(
                $"{typeof(TModule).Name} must implement {nameof(IServiceStartupModule)} " +
                $"and/or {nameof(IApplicationStartupModule<TApplicationBuilder>)}.", nameof(module));
        }
    }

    public void SetupServices(IServiceCollection? services = null)
    {
        QueueStartupLog(logger => logger.LogDebug($"Setting up services..."));

        Services = services ?? new ServiceCollection();

        ConfigureServices(Services);

        foreach (IServiceStartupModule module in serviceModules)
        {
            QueueStartupLog(logger => logger.LogDebug($"Configuring services for: {module.Name}"));
            module.ConfigureServices(Services);
        }

        ServiceProvider = Services.BuildServiceProvider();
    }

    public TApplicationBuilder SetupApplication(TApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        QueueStartupLog(logger => logger.LogDebug($"Setting up application..."));

        ConfigureApplication(app);

        foreach (var module in applicationModules)
        {
            QueueStartupLog(logger => logger.LogDebug($"Configuring Application for: {module.Name}"));
            module.ConfigureApplication(app);
        }

        return app;
    }
}
