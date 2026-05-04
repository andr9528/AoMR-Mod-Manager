namespace Manager.Abstractions.Dto;

public interface IPlaysetDto
{
    public IReadOnlyList<object> Actions { get; init; }

    public IReadOnlyList<IModDto> Mods { get; init; }
}
