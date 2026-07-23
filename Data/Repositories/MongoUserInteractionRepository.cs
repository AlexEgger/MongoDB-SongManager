using MongoDB.Driver;
using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Implements interaction repository operations in MongoDB.
/// </summary>
public class MongoUserInteractionRepository : MongoRepository<UserSongInteraction>, IUserInteractionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoUserInteractionRepository"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public MongoUserInteractionRepository (MongoDbContext context) : base(context.UserInteractions)
    {
    }

    /// <summary>
    /// Retrieves a specific user interaction record associated with a user and a song.
    /// </summary>
    public UserSongInteraction? GetInteraction (string userId, string songId)
    {
        var filter = Builders<UserSongInteraction>.Filter.And(
            Builders<UserSongInteraction>.Filter.Eq(x => x.UserId, userId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.SongId, songId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsDeleted, false)
        );

        return Collection.Find(filter).FirstOrDefault();
    }

    /// <summary>
    /// Retrieves all song interaction records marked as favorites for a specific user.
    /// </summary>
    public IEnumerable<UserSongInteraction> GetFavoritesByUserId (string userId)
    {
        var filter = Builders<UserSongInteraction>.Filter.And(
            Builders<UserSongInteraction>.Filter.Eq(x => x.UserId, userId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsFavorite, true),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsDeleted, false)
        );

        return Collection.Find(filter).ToList();
    }

    /// <summary>
    /// Retrieves all user interactions linked to a specific song.
    /// </summary>
    public IEnumerable<UserSongInteraction> GetInteractionsBySongId (string songId)
    {
        var filter = Builders<UserSongInteraction>.Filter.And(
            Builders<UserSongInteraction>.Filter.Eq(x => x.SongId, songId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsDeleted, false)
        );

        return Collection.Find(filter).ToList();
    }
}