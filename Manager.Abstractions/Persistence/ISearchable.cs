namespace Manager.Abstractions.Persistence;

/// <summary>
/// Interfaces inheriting from this interface are searchable by their Id property, and their own properties, when used in.
/// </summary>
public interface ISearchable
{
    int Id { get; set; }
}