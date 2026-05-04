using Manager.Abstractions.Dto;

namespace Manager.Models.Dto;

public sealed record PlaysetDto : IPlaysetDto
{
    public IReadOnlyList<object> Actions { get; init; } = [];

    public IReadOnlyList<IModDto> Mods { get; init; } = [];
}
