using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Contract for managing the state and notification events of the currently active application user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the currently active <see cref="UserDto"/>, or null if no user is selected.
        /// </summary>
        UserDto? CurrentUser { get; }

        /// <summary>
        /// Gets the unique ID of the currently active user, or an empty string if no user is active.
        /// </summary>
        string CurrentUserId { get; }

        /// <summary>
        /// Occurs when the active user changes.
        /// </summary>
        event EventHandler? CurrentUserChanged;

        /// <summary>
        /// Sets the specified user DTO as active and triggers the <see cref="CurrentUserChanged"/> event if the user ID changed.
        /// </summary>
        /// <param name="user">The user DTO instance to set as current active user.</param>
        void SetCurrentUser (UserDto? user);
    }
}