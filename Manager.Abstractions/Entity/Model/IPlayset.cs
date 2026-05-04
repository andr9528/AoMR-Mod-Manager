using Manager.Abstractions.Entity.Searchable;
using Manager.Abstractions.Persistence;

namespace Manager.Abstractions.Entity.Model;

public interface IPlayset : ISearchablePlayset, IEntity
{
    ICollection<IPlaysetMod> PlaysetMods { get; set; }
}
