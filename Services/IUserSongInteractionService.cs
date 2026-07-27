using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Contract for managing user-specific song interactions such as personal notes and ratings.
    /// </summary>
    public interface IUserSongInteractionService
    {
        /// <summary>
        /// Retrieves the personal interaction of a specific user for a given song.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="songId">The unique identifier of the song.</param>
        /// <returns>The interaction DTO if found; otherwise, null.</returns>
        Task<UserSongInteractionDto?> GetInteractionAsync (string userId, string songId);

        /// <summary>
        /// Retrieves all song interactions belonging to a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of interaction DTOs for the specified user.</returns>
        Task<IEnumerable<UserSongInteractionDto>> GetUserInteractionsAsync (string userId);

        /// <summary>
        /// Saves or updates a user's interaction for a specific song.
        /// </summary>
        /// <param name="userId">The unique identifier of the active user.</param>
        /// <param name="dto">The interaction data to persist.</param>
        /// <returns>The saved interaction DTO with updated metadata.</returns>
        Task<UserSongInteractionDto> SaveInteractionAsync (string userId, SaveUserSongInteractionDto dto);

        /// <summary>
        /// Removes a user's interaction for a specific song.
        /// </summary>
        /// <param name="userId">The unique identifier of the active user.</param>
        /// <param name="songId">The unique identifier of the song whose interaction should be deleted.</param>
        /// <returns>A task representing the asynchronous operation, returning true if successful.</returns>
        Task<bool> DeleteInteractionAsync (string userId, string songId);
    }
}