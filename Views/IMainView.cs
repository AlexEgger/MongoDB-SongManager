using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Contract defining the interactions and view-switching capabilities of the main application form.
    /// </summary>
    public interface IMainView
    {
        /// <summary>
        /// Occurs when the user selects a different user profile in the header dropdown.
        /// </summary>
        event EventHandler? UserSelectionChanged;

        /// <summary>
        /// Occurs when the navigation button for the Songs view is clicked.
        /// </summary>
        event EventHandler? NavSongsClicked;

        /// <summary>
        /// Occurs when the navigation button for the Songlists view is clicked.
        /// </summary>
        event EventHandler? NavSonglistsClicked;

        /// <summary>
        /// Occurs when the navigation button for the Statistics dashboard view is clicked.
        /// </summary>
        event EventHandler? NavStatisticsClicked;

        /// <summary>
        /// Gets the currently selected user profile from the dropdown.
        /// </summary>
        UserDto? SelectedUser { get; }

        /// <summary>
        /// Populates the user selection dropdown with user profiles.
        /// </summary>
        /// <param name="users">The collection of users to display.</param>
        void DisplayUsers (IEnumerable<UserDto> users);

        /// <summary>
        /// Displays the given UserControl in the main content container.
        /// </summary>
        /// <param name="view">The UserControl view to load.</param>
        void ShowView (System.Windows.Forms.UserControl view);
    }
}