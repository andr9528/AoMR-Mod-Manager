using Manager.Abstractions.Entity.Model;

namespace Manager.Models.Entity.Model;

public sealed class PlaysetMod : IPlaysetMod
{
    private int id;

    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(PlaysetMod)} entity");
    }

    public byte[] Version { get; set; } = [];

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public int PlaysetId { get; set; }

    public IPlayset Playset { get; set; }

    public int ModId { get; set; }

    public IMod Mod { get; set; }

    public int Priority { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsHidden { get; set; }

    public bool IsMissing { get; set; }

    public PlaysetMod(int id, Playset playset, Mod mod)
    {
        this.id = id;
        Playset = playset;
        Mod = mod;
    }

    public PlaysetMod()
    {
    }
}
