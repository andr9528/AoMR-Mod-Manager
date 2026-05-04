namespace Manager.Abstractions.Persistence;

public interface IEntity : ISearchable
{
    /// <summary>
    /// Exchange my type to fit with the type needed for concurrency control in the database.
    /// Most databases can use a byte array for this purpose, but you can change it to something else if needed.
    /// The important thing is that it should be able to detect changes to the entity and prevent concurrent updates from overwriting each other.
    /// </summary>
    byte[] Version { get; set; }

    DateTime CreatedDateTime { get; set; }
    DateTime UpdatedDateTime { get; set; }
}
