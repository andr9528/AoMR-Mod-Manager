using Newtonsoft.Json;

namespace Manager.Abstractions.Dto;

public interface IModDto
{
    public const string JSON_PROPERTY_WORKSHOP_ID_NAME = "WorkshopID";
    public const string JSON_PROPERTY_INSTALL_CRC_NAME = "InstallCRC";
    public const string JSON_PROPERTY_ENABLED_NAME = "Enabled";

    public string Title { get; init; }

    public string Author { get; init; }

    public string Description { get; init; }

    public string Path { get; init; }

    [JsonProperty(JSON_PROPERTY_WORKSHOP_ID_NAME)] public long WorkshopId { get; init; }

    public string LastUpdate { get; init; }

    public string InstallTime { get; init; }

    public int Priority { get; init; }

    [JsonProperty(JSON_PROPERTY_ENABLED_NAME)] public bool IsEnabled { get; init; }

    [JsonProperty(JSON_PROPERTY_INSTALL_CRC_NAME)] public uint? InstallCrc { get; init; }
}
