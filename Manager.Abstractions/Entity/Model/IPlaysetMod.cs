using Manager.Abstractions.Entity.Searchable;
using Manager.Abstractions.Persistence;

namespace Manager.Abstractions.Entity.Model;

public interface IPlaysetMod : ISearchablePlaysetMod, IEntity
{
    IPlayset Playset { get; set; }

    IMod Mod { get; set; }

    int Priority { get; set; }

    bool IsEnabled { get; set; }

    bool IsHidden { get; set; }

    bool IsMissing { get; set; }
}
