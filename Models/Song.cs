using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB_SongManager.Models;

/// <summary>
/// Represents a song entity containing resource URLs, tempo, and songbook metadata.
/// </summary>
public class Song : IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the song.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the song title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the associated artist identifier.
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ArtistId { get; set; }

    /// <summary>
    /// Gets or sets the song tempo in beats per minute (BPM).
    /// </summary>
    public uint? Tempo { get; set; }

    /// <summary>
    /// Gets or sets the web link to the chords or tabs.
    /// </summary>
    public string? ChordsUrl { get; set; }

    /// <summary>
    /// Gets or sets the web link to a YouTube video.
    /// </summary>
    public string? YoutubeUrl { get; set; }

    /// <summary>
    /// Gets or sets the songbook reference number.
    /// </summary>
    public uint? Liederbuchnummer { get; set; }

    /// <summary>
    /// Gets or sets the songbook page number.
    /// </summary>
    public uint? Liederbuchseite { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the song is soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}