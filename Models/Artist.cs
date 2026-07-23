using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_SongManager.Models;

/// <summary>
/// Represents a musical artist or band.
/// </summary>
public class Artist : IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the artist.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the artist's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the artist is soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}