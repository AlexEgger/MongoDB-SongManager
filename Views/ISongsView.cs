using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Defines the view contract for displaying and interacting with songs, song lists, and song details in the UI using DTOs.
    /// </summary>
    public interface ISongsView
    {
        #region Properties

        /// <summary>
        /// Gets the current search term entered by the user.
        /// </summary>
        string SearchTerm { get; }

        /// <summary>
        /// Gets the currently selected song display DTO in the UI data grid.
        /// </summary>
        SongDisplayDto? SelectedSong { get; }

        /// <summary>
        /// Gets the currently selected song list DTO in the UI list box.
        /// </summary>
        SonglistDto? SelectedSonglist { get; }

        #endregion

        #region Song Events

        /// <summary>
        /// Occurs when the user changes the search input text.
        /// </summary>
        event EventHandler SearchTextChanged;

        /// <summary>
        /// Occurs when the user selects a different song in the grid.
        /// </summary>
        event EventHandler SongSelectionChanged;

        /// <summary>
        /// Occurs when the user requests to create a new song.
        /// </summary>
        event EventHandler AddSongClicked;

        /// <summary>
        /// Occurs when the user requests to edit the selected song.
        /// </summary>
        event EventHandler EditSongClicked;

        /// <summary>
        /// Occurs when the user requests to delete the selected song.
        /// </summary>
        event EventHandler DeleteSongClicked;

        /// <summary>
        /// Occurs when the user requests to create a new artist.
        /// </summary>
        event EventHandler AddArtistClicked;

        #endregion

        #region User Interaction Events

        /// <summary>
        /// Occurs when the user requests to save personal ratings and notes for the selected song.
        /// </summary>
        event EventHandler SaveInteractionClicked;

        #endregion

        #region Songlist Events

        /// <summary>
        /// Occurs when the user selects a different song list from the list box.
        /// </summary>
        event EventHandler SonglistSelectionChanged;

        /// <summary>
        /// Occurs when the user requests to create a new song list.
        /// </summary>
        event EventHandler CreateSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to delete the selected song list.
        /// </summary>
        event EventHandler DeleteSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to add the selected song to the active song list.
        /// </summary>
        event EventHandler AddSongToSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to remove the selected song from the active song list.
        /// </summary>
        event EventHandler RemoveSongFromSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to rename the selected song list.
        /// </summary>
        event EventHandler RenameSonglistClicked;

        #endregion

        #region CSV Events

        /// <summary>
        /// Occurs when the user requests to export songs to a CSV file.
        /// </summary>
        event EventHandler ExportCsvClicked;

        /// <summary>
        /// Occurs when the user requests to import songs from a CSV file.
        /// </summary>
        event EventHandler ImportCsvClicked;

        #endregion

        #region Display Methods & Input Retrieval

        /// <summary>
        /// Binds the presentation song DTOs to the main UI data grid.
        /// </summary>
        /// <param name="songs">The collection of song display DTOs to render.</param>
        void DisplaySongs (IEnumerable<SongDisplayDto> songs);

        /// <summary>
        /// Displays detailed metadata for a single selected song in the details panel using its presentation DTO.
        /// </summary>
        /// <param name="song">The song display DTO containing metadata, or null to clear panel.</param>
        void DisplaySongDetails (SongDisplayDto? song);

        /// <summary>
        /// Displays the active user's personal interaction (ratings & notes) for the selected song.
        /// </summary>
        /// <param name="interaction">The user song interaction DTO, or null if none exists.</param>
        void DisplayUserInteraction (UserSongInteractionDto? interaction);

        /// <summary>
        /// Binds the collection of available song list DTOs to the sidebar list box.
        /// </summary>
        /// <param name="songlists">The collection of song list DTOs to display.</param>
        /// <param name="currentUserId">The ID of the active user for sorting/visual cues.</param>
        void DisplaySonglists (IEnumerable<SonglistDto> songlists, string currentUserId);

        /// <summary>
        /// Opens the interaction dialog and retrieves the user-entered ratings and notes input DTO.
        /// </summary>
        /// <returns>A <see cref="SaveUserSongInteractionDto"/> containing the input, or null if cancelled.</returns>
        SaveUserSongInteractionDto? GetUserInteractionInput ();

        #endregion
    }
}