using Manager.Abstractions.Entity.Model;
using Newtonsoft.Json;

namespace Manager.Models.Entity.Model;

public sealed class Mod : IMod
{
    private int id;

    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(Mod)} entity");
    }

    public byte[] Version { get; set; } = [];

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public long WorkshopId { get; set; }

    public string LastUpdate { get; set; } = "0";

    public string InstallTime { get; set; } = "0";

    public uint? InstallCrc { get; set; }

    public bool IsLocalMod => Path.StartsWith("local\\", StringComparison.OrdinalIgnoreCase);

    public ICollection<IPlaysetMod> PlaysetMods { get; set; } = [];

    [JsonConstructor]
    private Mod(int id, List<PlaysetMod> playsetMods)
    {
        this.id = id;
        PlaysetMods = playsetMods.Cast<IPlaysetMod>().ToList();
    }

    public Mod()
    {
    }
}
