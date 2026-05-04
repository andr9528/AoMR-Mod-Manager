using Manager.Abstractions.Entity.Model;
using Newtonsoft.Json;

namespace Manager.Models.Entity.Model;

public sealed class Playset : IPlayset
{
    private int id;

    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(Playset)} entity");
    }

    public byte[] Version { get; set; } = [];

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<IPlaysetMod> PlaysetMods { get; set; } = [];

    [JsonConstructor]
    private Playset(int id, List<PlaysetMod> playsetMods)
    {
        this.id = id;
        PlaysetMods = playsetMods.Cast<IPlaysetMod>().ToList();
    }

    public Playset()
    {
    }
}
