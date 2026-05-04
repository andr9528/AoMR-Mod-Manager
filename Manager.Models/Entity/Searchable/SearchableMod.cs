using Manager.Abstractions.Entity.Searchable;

namespace Manager.Models.Entity.Searchable;

public sealed class SearchableMod : ISearchableMod
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Id { get; set; }
}
