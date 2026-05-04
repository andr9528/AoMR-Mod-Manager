using Manager.Abstractions.Startup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Manager.Startup.Modules;

public class LoggingStartupModule : IServiceStartupModule
{
    private const string LOG_PATTERN =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] [{SourceContext}] {Scope} {Message:lj}{NewLine}{Exception}";

    private readonly string? logDirectory;
    private readonly IReadOnlyCollection<LogTarget> logTargets;

    public LoggingStartupModule(IEnumerable<LogTarget> logTargets, string? applicationDataPath = null)
    {
        var enumerable = logTargets.ToList();
        this.logTargets = enumerable.Distinct().ToArray();

        if (!enumerable.Any())
        {
            throw new ArgumentException("At least one log target must be supplied.", nameof(logTargets));
        }

        if (!enumerable.Contains(LogTarget.FILE))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(applicationDataPath))
        {
            throw new ArgumentException(
                $"'{nameof(applicationDataPath)}' must be supplied when using {LogTarget.FILE}.",
                nameof(applicationDataPath));
        }

        logDirectory = Path.Combine(applicationDataPath, "Logs");
    }

    /// <inheritdoc />
    public void ConfigureServices(IServiceCollection services)
    {
        var level = LogEventLevel.Information;
#if DEBUG
        level = LogEventLevel.Debug;
#endif

        ConfigureSerilog();

        services.AddLogging(x =>
        {
            x.ClearProviders();
            x.AddSerilog(Log.Logger);
        });

        WriteFileGap();
    }

    /// <inheritdoc />
    public string Name => "Logging Module";

    private void ConfigureSerilog()
    {
        LoggerConfiguration configuration = new LoggerConfiguration().Enrich.FromLogContext();
        configuration = AddWriteToSegments(configuration, logTargets);
        configuration = ConfigureMinimumLevel(configuration);
        configuration = AddLevelOverwrites(configuration);

        Log.Logger = configuration.CreateLogger();
    }

    private void WriteFileGap()
    {
        if (!logTargets.Contains(LogTarget.FILE))
        {
            return;
        }

        var todayFileName = $"log-{DateTime.Now:yyyyMMdd}.log";
        string fullPath = Path.Combine(logDirectory!, todayFileName);

        if (!File.Exists(fullPath))
        {
            return;
        }

        File.AppendAllText(fullPath, Environment.NewLine);
        File.AppendAllText(fullPath, Environment.NewLine);
        File.AppendAllText(fullPath, Environment.NewLine);
    }

    private LoggerConfiguration ConfigureMinimumLevel(LoggerConfiguration configuration)
    {
        configuration = configuration.MinimumLevel.Debug();

        return configuration;
    }

    private LoggerConfiguration AddWriteToSegments(LoggerConfiguration configuration, IEnumerable<LogTarget> targets)
    {
        var level = LogEventLevel.Information;
#if DEBUG
        level = LogEventLevel.Debug;
#endif

        foreach (LogTarget logTarget in targets)
        {
            configuration = logTarget switch
            {
                LogTarget.CONSOLE => configuration.WriteTo.Console(outputTemplate: LOG_PATTERN),

                LogTarget.BROWSER_CONSOLE => configuration.WriteTo.BrowserConsoleSimple(LOG_PATTERN),
                // Not Supported on BrowserWASM - yet.
                //LogTarget.BROWSER_CONSOLE => configuration.WriteTo.BrowserConsole(outputTemplate: LOG_PATTERN),

                LogTarget.FILE => configuration.WriteTo.File(Path.Combine(logDirectory!, "log-.log"),
                    outputTemplate: LOG_PATTERN, shared: true, flushToDiskInterval: TimeSpan.FromMinutes(1),
                    restrictedToMinimumLevel: level, retainedFileCountLimit: 7, rollingInterval: RollingInterval.Day),

                var _ => throw new ArgumentOutOfRangeException(nameof(targets), logTarget, "Unsupported log target."),
            };
        }

        return configuration;
    }

    /// <summary>
    /// Remove / Add relevant log level overrides for specific usage here.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private LoggerConfiguration AddLevelOverwrites(LoggerConfiguration configuration)
    {
        configuration = configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
        configuration = configuration.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information);

        configuration = configuration.MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning);
        configuration = configuration.MinimumLevel.Override("Microsoft.Extensions.Http", LogEventLevel.Warning);

        return configuration;
    }
}

public enum LogTarget
{
    CONSOLE = 0,
    BROWSER_CONSOLE = 1,
    FILE = 2,
}

public class BrowserConsoleSink : ILogEventSink
{
    private readonly MessageTemplateTextFormatter _formatter;

    public BrowserConsoleSink(string outputTemplate)
    {
        _formatter = new MessageTemplateTextFormatter(outputTemplate);
    }

    public void Emit(LogEvent logEvent)
    {
        using var writer = new StringWriter();
        _formatter.Format(logEvent, writer);

        Console.WriteLine(writer.ToString());
    }
}

public static class BrowserConsoleSinkExtensions
{
    public static LoggerConfiguration BrowserConsoleSimple(
        this LoggerSinkConfiguration sinkConfiguration, string outputTemplate)
    {
        return sinkConfiguration.Sink(new BrowserConsoleSink(outputTemplate));
    }
}
