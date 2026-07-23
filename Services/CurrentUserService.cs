using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Singleton/Service responsible for managing the state of the currently active user in the application.
    /// </summary>
    public class CurrentUserService
    {
        private User? _currentUser;

        /// <summary>
        /// Gets the currently active <see cref="User"/>.
        /// </summary>
        public User? CurrentUser => _currentUser;

        /// <summary>
        /// Gets the ID of the currently active user, or an empty string if no user is set.
        /// </summary>
        public string CurrentUserId => _currentUser?.Id ?? string.Empty;

        /// <summary>
        /// Occurs when the active user is changed.
        /// </summary>
        public event EventHandler? CurrentUserChanged;

        /// <summary>
        /// Sets the currently active user and triggers the <see cref="CurrentUserChanged"/> event.
        /// </summary>
        /// <param name="user">The user entity to set as active.</param>
        public void SetCurrentUser (User? user)
        {
            if (_currentUser?.Id != user?.Id)
            {
                _currentUser = user;
                CurrentUserChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}