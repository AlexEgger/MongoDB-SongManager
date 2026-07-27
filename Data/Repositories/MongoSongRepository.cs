using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// MongoDB implementation of the <see cref="ISongRepository"/> contract.
/// </summary>
public class MongoSongRepository : MongoRepository<Song>, ISongRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoSongRepository"/> class using the shared database context.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public MongoSongRepository (MongoDbContext context) : base(context.Songs)
    {
    }

    /// <summary>
    /// Inserts a new song document only if an active song with the same title and artist does not already exist.
    /// </summary>
    /// <param name="entity">The song entity to insert.</param>
    /// <exception cref="InvalidOperationException">Thrown when a song with the same title and artist already exists.</exception>
    public override void Insert (Song entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        string normalizedTitle = entity.Title?.Trim().ToLower() ?? string.Empty;
        string? artistId = entity.ArtistId;

        bool exists = Collection.Find(s =>
            !s.IsDeleted &&
            s.ArtistId == artistId &&
            s.Title.ToLower() == normalizedTitle
        ).Any();

        if (exists)
        {
            throw new InvalidOperationException($"A song with the title '{entity.Title}' already exists for this artist.");
        }

        base.Insert(entity);
    }

    /// <summary>
    /// Retrieves all active songs associated with a specific artist ID.
    /// </summary>
    /// <param name="artistId">The unique identifier of the artist.</param>
    /// <returns>A collection of matching song entities.</returns>
    public IEnumerable<Song> GetByArtistId (string artistId)
    {
        return Collection.Find(s => s.ArtistId == artistId && !s.IsDeleted).ToList();
    }

    /// <summary>
    /// Performs a case-insensitive search for songs matching the specified title query string.
    /// </summary>
    /// <param name="titleQuery">The partial or full song title to search for.</param>
    /// <returns>A collection of matching song entities.</returns>
    public IEnumerable<Song> SearchByTitle (string titleQuery)
    {
        if (string.IsNullOrWhiteSpace(titleQuery))
        {
            return GetAll();
        }

        var filter = Builders<Song>.Filter.And(
            Builders<Song>.Filter.Eq(s => s.IsDeleted, false),
            Builders<Song>.Filter.Regex(s => s.Title, new BsonRegularExpression(titleQuery, "i"))
        );

        return Collection.Find(filter).ToList();
    }

    /// <summary>
    /// Updates an existing artist entity while ensuring that no other active artist has the same name (case-insensitive).
    /// </summary>
    /// <param name="entity"> The song entity to update.</param>
    /// <returns> True if the update was successful; otherwise, false.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a song with the same title and artist already exists.</exception>
    public override bool Update (Song entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        string normalizedTitle = entity.Title?.Trim().ToLower() ?? string.Empty;
        string? artistId = entity.ArtistId;

        // Check for duplicates
        bool duplicateExists = Collection.Find(s =>
        !s.IsDeleted &&
        s.Id != entity.Id &&
        s.ArtistId == artistId &&
        s.Title.ToLower() == normalizedTitle
                                ).Any();

        if (duplicateExists)
        {
            throw new InvalidOperationException($"A song with the title '{entity.Title}' already exists for this artist.");
        }

        return base.Update(entity);
    }
}