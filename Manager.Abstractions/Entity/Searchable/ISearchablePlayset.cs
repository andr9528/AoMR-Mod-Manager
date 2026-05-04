using Manager.Abstractions.Persistence;

namespace Manager.Abstractions.Entity.Searchable;

public interface ISearchablePlayset : ISearchable
{
    string Name { get; set; }
}
