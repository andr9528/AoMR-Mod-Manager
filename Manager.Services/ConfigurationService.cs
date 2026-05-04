using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Manager.Services;

/// <summary>
/// Handles application configuration loading, file creation and database option setup.
/// </summary>
public class ConfigurationService
{
    public const string DEFAULT_OFF_PLAYSET_FILE_NAME = "all-off";
    public const string DEFAULT_ON_PLAYSET_FILE_NAME = "all-on";
    public const string MOD_STATUS_FILE_NAME = "myth-mod-status";

    private const string APP_SETTINGS_FILE = "appsettings.json";
    private const string MOD_MANAGER_FOLDER = "Manager";
    private const string PLAYSET_FOLDER = "Playsets";
    private const string DATABASE_FILE_NAME = "manager.db";
    public static string PlaysetsLocation;

    private IConfiguration? configuration;

    public ConfigurationService()
    {
        PlaysetsLocation = Path.Combine(GetExecutionDirectory(), MOD_MANAGER_FOLDER, PLAYSET_FOLDER);
        if (!Directory.Exists(PlaysetsLocation))
        {
            Directory.CreateDirectory(PlaysetsLocation);
        }
    }

    private string GetExecutionDirectory()
    {
        return Environment.CurrentDirectory;
    }

    /// <summary>
    /// Builds and returns the application configuration from the local app data folder.
    /// </summary>
    public IConfiguration BuildConfiguration()
    {
        IConfigurationBuilder configBuilder = new ConfigurationBuilder();

        configBuilder.AddEnvironmentVariables();

        if (!IsRunningInCi())
        {
            EnsureAppSettingsFileExists();

            string fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);

            configBuilder.AddJsonFile(fullAppFilePath, false, true);
        }

        configuration = configBuilder.Build();
        return configuration;
    }

    private bool IsRunningInCi()
    {
        return string.Equals(Environment.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Configures the database options.
    /// </summary>
    public void ConfigureDatabaseOptions(DbContextOptionsBuilder options)
    {
        Directory.CreateDirectory(GetApplicationDataPath());

        string databasePath = Path.Combine(GetApplicationDataPath(), DATABASE_FILE_NAME);

        options.UseSqlite($"Data Source={databasePath}");

#if DEBUG
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
#endif
    }

    /// <summary>
    /// Returns the application data path used for configuration files.
    /// </summary>
    public string GetApplicationDataPath()
    {
        return Path.Combine(Environment.CurrentDirectory, MOD_MANAGER_FOLDER);
    }

    /// <summary>
    /// Ensures the appsettings file exists, creating an empty one if needed.
    /// </summary>
    private void EnsureAppSettingsFileExists()
    {
        string fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);

        if (File.Exists(fullAppFilePath))
        {
            return;
        }

        var template = new { };

        CreateFile(fullAppFilePath, template);
    }

    /// <summary>
    /// Creates a JSON file with the supplied template content.
    /// </summary>
    private void CreateFile(string path, object template)
    {
        string templateContent = JsonSerializer.Serialize(template, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, templateContent);
    }
}
