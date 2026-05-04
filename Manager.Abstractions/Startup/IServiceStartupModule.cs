using Microsoft.Extensions.DependencyInjection;

namespace Manager.Abstractions.Startup;

public interface IServiceStartupModule
{
    void ConfigureServices(IServiceCollection services);
    string Name { get; }
}
