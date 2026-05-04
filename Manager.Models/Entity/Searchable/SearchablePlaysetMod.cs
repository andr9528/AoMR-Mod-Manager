using Manager.Abstractions.Entity.Searchable;

namespace Manager.Models.Entity.Searchable;

public sealed class SearchablePlaysetMod : ISearchablePlaysetMod
{
    public int PlaysetId { get; set; }

    public int ModId { get; set; }

    /// <inheritdoc />
    public int Id { get; set; }
}
