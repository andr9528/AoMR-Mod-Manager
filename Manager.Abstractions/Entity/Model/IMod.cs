using Manager.Abstractions.Entity.Searchable;
using Manager.Abstractions.Persistence;

namespace Manager.Abstractions.Entity.Model;

public interface IMod : ISearchableMod, IEntity
{
    string Path { get; set; }

    long WorkshopId { get; set; }

    string LastUpdate { get; set; }

    string InstallTime { get; set; }

    uint? InstallCrc { get; set; }

    bool IsLocalMod { get; }

    ICollection<IPlaysetMod> PlaysetMods { get; set; }
}
