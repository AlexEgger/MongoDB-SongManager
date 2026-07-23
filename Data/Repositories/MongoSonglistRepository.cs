using MongoDB.Driver;
using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Implements domain-specific repository operations for the <see cref="Songlist"/> collection.
/// </summary>
public class MongoSonglistRepository : MongoRepository<Songlist>, ISonglistRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoSonglistRepository"/> class using the provided database context.
    /// </summary>
    /// <param name="context">The database context providing access to MongoDB collections.</param>
    public MongoSonglistRepository (MongoDbContext context) : base(context.Songlists)
    {
    }

    /// <summary>
    /// Retrieves all active (non-deleted) songlists created by a specific user.
    /// </summary>
    /// <param name="creatorId">The unique ID of the creator user.</param>
    /// <returns>A collection of matching <see cref="Songlist"/> documents.</returns>
    public IEnumerable<Songlist> GetSonglistsByCreatorId (string creatorId)
    {
        var filter = Builders<Songlist>.Filter.And(
            Builders<Songlist>.Filter.Eq(s => s.CreatorId, creatorId),
            Builders<Songlist>.Filter.Eq(s => s.IsDeleted, false)
        );

        return Collection.Find(filter).ToList();
    }
}