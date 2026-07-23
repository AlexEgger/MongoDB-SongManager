using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_SongManager.Models;

/// <summary>
/// Represents a playlist created by a user, holding references to songs.
/// </summary>
public class Songlist : IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the songlist.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the playlist name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier of the creator.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public string CreatorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of song identifiers contained in this playlist.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> SongIds { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the songlist is soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}