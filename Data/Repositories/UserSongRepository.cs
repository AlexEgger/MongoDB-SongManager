using MongoDB.Driver;
using MongoDB_SongManager.Data;
using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;

/// <summary>
/// Implements interaction repository operations in MongoDB.
/// </summary>
public class MongoUserInteractionRepository : MongoRepository<UserSongInteraction>, IUserInteractionRepository
{
    public MongoUserInteractionRepository (MongoDbContext context) : base(context.UserInteractions)
    {
    }

    public async Task<UserSongInteraction?> GetInteractionAsync (string userId, string songId)
    {
        var filter = Builders<UserSongInteraction>.Filter.And(
            Builders<UserSongInteraction>.Filter.Eq(x => x.UserId, userId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.SongId, songId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsDeleted, false)
        );

        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<UserSongInteraction>> GetFavoritesByUserIdAsync (string userId)
    {
        var filter = Builders<UserSongInteraction>.Filter.And(
            Builders<UserSongInteraction>.Filter.Eq(x => x.UserId, userId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsFavorite, true),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsDeleted, false)
        );

        return await Collection.Find(filter).ToListAsync();
    }

    public async Task<List<UserSongInteraction>> GetInteractionsBySongIdAsync (string songId)
    {
        var filter = Builders<UserSongInteraction>.Filter.And(
            Builders<UserSongInteraction>.Filter.Eq(x => x.SongId, songId),
            Builders<UserSongInteraction>.Filter.Eq(x => x.IsDeleted, false)
        );

        return await Collection.Find(filter).ToListAsync();
    }
}