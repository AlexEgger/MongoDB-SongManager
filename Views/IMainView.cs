using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Represents the contract between the main WinForms top-level view and its presenter.
    /// </summary>
    public interface IMainView
    {
        /// <summary>
        /// Occurs when the user selects a different active user from the header dropdown menu.
        /// </summary>
        event EventHandler UserSelectionChanged;

        /// <summary>
        /// Occurs when the user requests navigation to the songs view.
        /// </summary>
        event EventHandler NavSongsClicked;

        /// <summary>
        /// Occurs when the user requests navigation to the setlists view.
        /// </summary>
        event EventHandler NavSonglistsClicked;

        /// <summary>
        /// Gets the currently selected user DTO from the header dropdown control.
        /// </summary>
        UserDto? SelectedUser { get; }

        /// <summary>
        /// Displays the list of available users in the header dropdown control.
        /// </summary>
        /// <param name="users">The collection of user DTOs to populate.</param>
        void DisplayUsers (IEnumerable<UserDto> users);

        /// <summary>
        /// Renders the specified sub-view control inside the main content container panel.
        /// </summary>
        /// <param name="view">The user control instance to present.</param>
        void ShowView (UserControl view);
    }
}