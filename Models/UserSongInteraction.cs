using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_SongManager.Models;

/// <summary>
/// Holds user-specific metadata for a song, such as ratings, notes, and favorite status.
/// </summary>
public class UserSongInteraction : IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the interaction document.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the song identifier.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public string SongId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the song is marked as favorite by the user.
    /// </summary>
    public bool IsFavorite { get; set; } = false;

    /// <summary>
    /// Gets or sets the list of category ratings submitted by the user.
    /// </summary>
    public List<RatingEntry> Ratings { get; set; } = new();

    /// <summary>
    /// Gets or sets optional personal notes or comments about the song performance.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the last modification timestamp in UTC.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets a value indicating whether the interaction record is soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}