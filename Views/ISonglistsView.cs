using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Defines the contract for the view responsible for managing songlists (setlists) and their song associations.
    /// </summary>
    public interface ISonglistsView
    {
        #region Properties

        /// <summary>
        /// Gets the currently selected <see cref="Songlist"/> object from the view.
        /// </summary>
        Songlist? SelectedSonglist { get; }

        /// <summary>
        /// Gets the currently selected <see cref="Song"/> from the available songs library grid.
        /// </summary>
        Song? SelectedAvailableSong { get; }

        /// <summary>
        /// Gets the currently selected <see cref="Song"/> from the active playlist grid.
        /// </summary>
        Song? SelectedPlaylistSong { get; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user selects a different songlist in the UI.
        /// </summary>
        event EventHandler? SonglistSelectionChanged;

        /// <summary>
        /// Occurs when the user requests to add the selected available song to the active playlist.
        /// </summary>
        event EventHandler? AddSongToPlaylistClicked;

        /// <summary>
        /// Occurs when the user requests to remove the selected song from the active playlist.
        /// </summary>
        event EventHandler? RemoveSongFromPlaylistClicked;

        /// <summary>
        /// Occurs when the user requests to create a new songlist.
        /// </summary>
        event EventHandler? CreateSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to delete the active songlist.
        /// </summary>
        event EventHandler? DeleteSonglistClicked;

        #endregion

        #region Display Methods

        /// <summary>
        /// Displays the collection of available songlists in the sidebar list view.
        /// </summary>
        /// <param name="songlists">The collection of songlist entities to display.</param>
        void DisplaySonglists (IEnumerable<Songlist> songlists);

        /// <summary>
        /// Displays the collection of available songs in the main library grid.
        /// </summary>
        /// <param name="songs">The collection of available song entities.</param>
        /// <param name="artistNames">A dictionary mapping artist IDs to resolved artist names.</param>
        void DisplayAvailableSongs (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames);

        /// <summary>
        /// Displays the collection of songs assigned to the currently selected songlist.
        /// </summary>
        /// <param name="songs">The collection of playlist song entities.</param>
        /// <param name="artistNames">A dictionary mapping artist IDs to resolved artist names.</param>
        void DisplayPlaylistSongs (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames);

        #endregion
    }
}