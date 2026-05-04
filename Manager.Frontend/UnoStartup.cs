using System.Text.Json;
using Manager.Frontend.Models;
using Manager.Persistence;
using Manager.Services;
using Manager.Startup;
using Manager.Startup.Modules;
using Microsoft.Extensions.Configuration;

namespace Manager.Frontend;

internal class UnoStartup : ModularStartup<IApplicationBuilder>
{
    private IServiceCollection Services { get; set; }
    public IServiceProvider ServiceProvider { get; private set; }
    public IHost? Host { get; private set; }

    private readonly IConfiguration configuration;
    private readonly ConfigurationService configurationService;

    public UnoStartup()
    {
        configurationService = new ConfigurationService();
        configuration = configurationService.BuildConfiguration();

        AddModule(new LoggingStartupModule(new[]
        {
            LogTarget.CONSOLE,
            LogTarget.FILE,
        }, configurationService.GetApplicationDataPath()));

        AddModule(new DatabaseContextStartupModule<ManagerDatabaseContext>(
            configurationService.ConfigureDatabaseOptions));
    }

    protected override void ConfigureApplication(IApplicationBuilder app)
    {
        app.Configure(host => host
#if DEBUG
            // Switch to Development environment when running in DEBUG
            .UseEnvironment(Environments.Development)
#endif
            .UseConfiguration(configure: ConfigureConfigurationSource).UseSerialization(ConfigureSerialization));
    }

    private void ConfigureSerialization(HostBuilderContext builderContext, IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(new JsonSerializerOptions {IncludeFields = true,});
    }

    private IHostBuilder ConfigureConfigurationSource(IConfigBuilder configBuilder)
    {
        return configBuilder.EmbeddedSource<App>().Section<AppConfig>();
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddSingleton(configurationService);
    }
}
