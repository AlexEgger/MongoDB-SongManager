using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_SongManager.Models;

/// <summary>
/// Represents an application user.
/// </summary>
public class User : IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user is soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}