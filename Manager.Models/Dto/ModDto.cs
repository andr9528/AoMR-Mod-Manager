using Manager.Abstractions.Dto;
using Newtonsoft.Json;

namespace Manager.Models.Dto;

public sealed record ModDto : IModDto
{
    public string Title { get; init; } = string.Empty;

    public string Author { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Path { get; init; } = string.Empty;

    [JsonProperty(IModDto.JSON_PROPERTY_WORKSHOP_ID_NAME)]
    public long WorkshopId { get; init; }

    public string LastUpdate { get; init; } = "0";

    public string InstallTime { get; init; } = "0";

    public int Priority { get; init; }

    [JsonProperty(IModDto.JSON_PROPERTY_ENABLED_NAME)]
    public bool IsEnabled { get; init; }

    [JsonProperty(IModDto.JSON_PROPERTY_INSTALL_CRC_NAME)]
    public uint? InstallCrc { get; init; }
}
