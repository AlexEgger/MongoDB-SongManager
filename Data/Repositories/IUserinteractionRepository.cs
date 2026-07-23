using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines data access operations for user interactions and song ratings.
/// </summary>
public interface IUserInteractionRepository : IRepository<UserSongInteraction>
{
    /// <summary>
    /// Retrieves a specific user interaction record associated with a user and a song.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="songId">The unique identifier of the song.</param>
    /// <returns>The interaction record if present; otherwise, <c>null</c>.</returns>
    UserSongInteraction? GetInteraction (string userId, string songId);

    /// <summary>
    /// Retrieves all song interaction records marked as favorites for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of user interaction records.</returns>
    IEnumerable<UserSongInteraction> GetFavoritesByUserId (string userId);

    /// <summary>
    /// Retrieves all user interactions linked to a specific song.
    /// </summary>
    /// <param name="songId">The unique identifier of the song.</param>
    /// <returns>A collection of user interaction records.</returns>
    IEnumerable<UserSongInteraction> GetInteractionsBySongId (string songId);
}