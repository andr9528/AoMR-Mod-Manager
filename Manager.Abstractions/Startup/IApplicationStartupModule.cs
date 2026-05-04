namespace Manager.Abstractions.Startup;

public interface IApplicationStartupModule<TApplicationBuilder>
{
    void ConfigureApplication(TApplicationBuilder app);
    string Name { get; }
}
