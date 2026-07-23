using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB_SongManager.Models.Enums;

namespace MongoDB_SongManager.Models;

/// <summary>
/// Represents a score assigned to a specific rating category.
/// </summary>
public class RatingEntry
{
    /// <summary>
    /// Gets or sets the category of the rating.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public RatingType Category { get; set; }

    /// <summary>
    /// Gets or sets the numerical rating score (e.g., 1 to 10).
    /// </summary>
    public int Value { get; set; }
}