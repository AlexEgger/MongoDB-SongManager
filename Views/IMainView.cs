using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Contract defining the interactions, user management capabilities, and view-switching behavior of the main application form.
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
        /// Occurs when the user requests to create a new user profile.
        /// </summary>
        event EventHandler? AddUserClicked;

        /// <summary>
        /// Occurs when the user requests to edit the currently selected user profile.
        /// </summary>
        event EventHandler? EditUserClicked;

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
        /// Selects a user profile in the UI dropdown based on the specified user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to select.</param>
        void SelectUser (string? userId);

        /// <summary>
        /// Displays the given UserControl in the main content container.
        /// </summary>
        /// <param name="view">The UserControl view to load.</param>
        void ShowView (System.Windows.Forms.UserControl view);

        /// <summary>
        /// Prompts the user via a modal dialog to input or modify user details.
        /// </summary>
        /// <param name="userToEdit">The existing user DTO to populate the form with for editing, or null to create a new user.</param>
        /// <returns>The populated <see cref="UserDto"/> if accepted; otherwise, null.</returns>
        UserDto? GetUserInput (UserDto? userToEdit = null);
    }
}