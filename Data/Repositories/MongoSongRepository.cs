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
}