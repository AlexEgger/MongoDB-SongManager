using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views;

/// <summary>
/// Defines the view contract for displaying and interacting with songs, song lists, and song details in the UI.
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
    /// Gets the currently selected song list in the UI list box.
    /// </summary>
    Songlist? SelectedSonglist { get; }

    /// <summary>
    /// Gets a value indicating whether the favorite songs filter is active.
    /// </summary>
    bool IsFavoritesFilterActive { get; }

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
    /// Occurs when the user toggles the favorites filter button.
    /// </summary>
    event EventHandler FilterFavoritesClicked;

    /// <summary>
    /// Occurs when the user toggles the favorite status of the selected song.
    /// </summary>
    event EventHandler ToggleFavoriteClicked;

    /// <summary>
    /// Occurs when the user requests to create a new artist.
    /// </summary>
    event EventHandler AddArtistClicked;

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

    #endregion

    #region Display Methods

    /// <summary>
    /// Binds the presentation song DTOs to the main UI data grid.
    /// </summary>
    /// <param name="songs">The collection of song display DTOs to render.</param>
    void DisplaySongs (IEnumerable<SongDisplayDto> songs);

    /// <summary>
    /// Displays detailed metadata for a single selected song in the details panel.
    /// </summary>
    /// <param name="song">The song entity to display.</param>
    /// <param name="artistName">The resolved name of the artist, if available.</param>
    void DisplaySongDetails (Song? song, string? artistName);

    /// <summary>
    /// Binds the collection of available song lists to the sidebar list box.
    /// </summary>
    /// <param name="songlists">The list of song list entities to display.</param>
    /// <param name="currentUserId">The ID of the active user for sorting/visual cues.</param>
    void DisplaySonglists (IEnumerable<Songlist> songlists, string currentUserId);

    #endregion
}