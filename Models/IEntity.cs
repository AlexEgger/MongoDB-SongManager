namespace MongoDB_SongManager.Models;

/// <summary>
/// Defines the core contract for entities stored in MongoDB with soft-delete support.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets the unique document identifier.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is soft-deleted.
    /// </summary>
    bool IsDeleted { get; set; }
}