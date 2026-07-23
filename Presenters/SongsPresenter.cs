using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Views;
using SongManager.Views;

namespace MongoDB_SongManager.Presenters;

/// <summary>
/// Presenter component handling business logic, data presentation, dialog interaction, and UI event binding for songs.
/// </summary>
public class SongsPresenter
{
    private readonly ISongsView _view;
    private readonly IRepository<Song> _songRepository;
    private readonly IRepository<Artist> _artistRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SongsPresenter"/> class.
    /// </summary>
    /// <param name="view">The view contract instance.</param>
    /// <param name="songRepository">The MongoDB repository for songs.</param>
    /// <param name="artistRepository">The MongoDB repository for artists.</param>
    public SongsPresenter (
        ISongsView view,
        IRepository<Song> songRepository,
        IRepository<Artist> artistRepository)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _songRepository = songRepository ?? throw new ArgumentNullException(nameof(songRepository));
        _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));

        // Subscribe to view events
        _view.SearchTextChanged += OnSearchTextChanged;
        _view.SongSelectionChanged += OnSongSelectionChanged;
        _view.AddSongClicked += OnAddSongClicked;
        _view.EditSongClicked += OnEditSongClicked;
        _view.DeleteSongClicked += OnDeleteSongClicked;

        LoadAllSongs();
    }

    /// <summary>
    /// Fetches all active songs and artists from MongoDB and updates the view display.
    /// </summary>
    public void LoadAllSongs ()
    {
        var songs = _songRepository.GetAll().ToList();
        var artistLookup = GetArtistLookup();

        _view.DisplaySongs(songs, artistLookup);
    }

    /// <summary>
    /// Handles search text input changes and updates the filtered list in the view.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">Event arguments.</param>
    private void OnSearchTextChanged (object? sender, EventArgs e)
    {
        string search = _view.SearchTerm?.Trim().ToLower() ?? string.Empty;
        var artistLookup = GetArtistLookup();

        if (string.IsNullOrEmpty(search))
        {
            LoadAllSongs();
            return;
        }

        // Filter songs by title
        var filteredSongs = _songRepository.Find(s =>
            s.Title.ToLower().Contains(search)
        ).ToList();

        _view.DisplaySongs(filteredSongs, artistLookup);
    }

    /// <summary>
    /// Handles selection of a song row in the view and displays detailed information.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">Event arguments.</param>
    private void OnSongSelectionChanged (object? sender, EventArgs e)
    {
        var selectedSong = _view.SelectedSong;
        if (selectedSong == null) return;

        string? artistName = null;
        if (!string.IsNullOrEmpty(selectedSong.ArtistId))
        {
            var artist = _artistRepository.GetById(selectedSong.ArtistId);
            artistName = artist?.Name;
        }

        _view.DisplaySongDetails(selectedSong, artistName);
    }

    /// <summary>
    /// Opens the dialog to create a new song and persists it upon confirmation.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">Event arguments.</param>
    private void OnAddSongClicked (object? sender, EventArgs e)
    {
        var artists = _artistRepository.GetAll();
        using var dialog = new SongDialog(null, artists);

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _songRepository.Insert(dialog.Song);
            LoadAllSongs();
        }
    }

    /// <summary>
    /// Opens the dialog to edit the currently selected song and updates it upon confirmation.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">Event arguments.</param>
    private void OnEditSongClicked (object? sender, EventArgs e)
    {
        var selectedSong = _view.SelectedSong;
        if (selectedSong == null) return;

        var artists = _artistRepository.GetAll();
        using var dialog = new SongDialog(selectedSong, artists);

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _songRepository.Update(dialog.Song);
            LoadAllSongs();
        }
    }

    /// <summary>
    /// Handles soft-deletion of the currently selected song in the view.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">Event arguments.</param>
    private void OnDeleteSongClicked (object? sender, EventArgs e)
    {
        var selectedSong = _view.SelectedSong;
        if (selectedSong != null && !string.IsNullOrEmpty(selectedSong.Id))
        {
            _songRepository.Delete(selectedSong.Id);
            LoadAllSongs();
        }
    }

    /// <summary>
    /// Helper method to retrieve a dictionary mapping Artist IDs to Artist Names.
    /// </summary>
    /// <returns>A dictionary containing ArtistId as key and Artist Name as value.</returns>
    private IReadOnlyDictionary<string, string> GetArtistLookup ()
    {
        return _artistRepository.GetAll()
            .ToDictionary(a => a.Id, a => a.Name);
    }
}