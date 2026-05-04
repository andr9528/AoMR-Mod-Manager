using Manager.Abstractions.Persistence;

namespace Manager.Abstractions.Entity.Searchable;

public interface ISearchableMod : ISearchable
{
    string Title { get; set; }

    string Author { get; set; }

    string Description { get; set; }
}
