using Manager.Abstractions.Entity.Searchable;

namespace Manager.Models.Entity.Searchable;

public sealed class SearchablePlayset : ISearchablePlayset
{
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Id { get; set; }
}
