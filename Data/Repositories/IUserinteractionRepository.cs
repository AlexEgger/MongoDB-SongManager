using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines data access operations for user-song interactions and ratings.
/// </summary>
public interface IUserInteractionRepository : IRepository<UserSongInteraction>
{
    Task<UserSongInteraction?> GetInteractionAsync (string userId, string songId);
    Task<List<UserSongInteraction>> GetFavoritesByUserIdAsync (string userId);
    Task<List<UserSongInteraction>> GetInteractionsBySongIdAsync (string songId);
}
