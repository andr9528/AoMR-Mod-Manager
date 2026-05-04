using Manager.Abstractions.Persistence;

namespace Manager.Abstractions.Entity.Searchable;

public interface ISearchablePlaysetMod : ISearchable
{
    int PlaysetId { get; set; }

    int ModId { get; set; }
}
