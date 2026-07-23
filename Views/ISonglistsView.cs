using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// View contract defining interaction capabilities for setlists and playlist song assignments.
    /// </summary>
    public interface ISonglistsView
    {
        // Selected Items
        Songlist? SelectedSonglist { get; }
        SongDisplayDto? SelectedAvailableSong { get; }
        SongDisplayDto? SelectedAssignedSong { get; }

        // Filters & Input States
        bool IsFilterMySonglistsOnlyActive { get; }
        string AvailableSongsSearchTerm { get; }

        // Setlist Events
        event EventHandler? SonglistSelectionChanged;
        event EventHandler? FilterMySonglistsOnlyChanged;
        event EventHandler? CreateSonglistClicked;
        event EventHandler? RenameSonglistClicked;
        event EventHandler? DeleteSonglistClicked;

        // Song Transfer & Ordering Events
        event EventHandler? AddSongToSonglistClicked;
        event EventHandler? RemoveSongFromSonglistClicked;
        event EventHandler? MoveSongUpClicked;
        event EventHandler? MoveSongDownClicked;
        event EventHandler? AvailableSongsSearchTextChanged;

        // UI Display Operations
        void DisplaySonglists (IEnumerable<Songlist> songlists, string currentUserId);
        void DisplayAvailableSongs (IEnumerable<SongDisplayDto> songs);
        void DisplayAssignedSongs (IEnumerable<SongDisplayDto> songs);
        void SetReadOnlyState (bool isReadOnly);
    }
}