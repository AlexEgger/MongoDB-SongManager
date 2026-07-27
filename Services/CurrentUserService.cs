using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Service implementation responsible for tracking the currently active user DTO and notifying listeners upon change.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private UserDto? _currentUser;

        /// <inheritdoc />
        public UserDto? CurrentUser => _currentUser;

        /// <inheritdoc />
        public string CurrentUserId => _currentUser?.Id ?? string.Empty;

        /// <inheritdoc />
        public event EventHandler? CurrentUserChanged;

        /// <inheritdoc />
        public void SetCurrentUser (UserDto? user)
        {
            // Avoid redundant updates if the user identity hasn't changed
            if (_currentUser?.Id != user?.Id)
            {
                _currentUser = user;
                CurrentUserChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}