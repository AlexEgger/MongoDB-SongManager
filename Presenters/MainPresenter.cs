using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;

namespace MongoDB_SongManager.Presenters;

/// <summary>
/// Controls application presentation logic, orchestrating database interaction and UI updates.
/// </summary>
public class MainPresenter
{
    private readonly IMainView _view;
    private readonly ISongRepository _songRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly ICsvService _csvService;
    private readonly IDtoService _dtoService;

    public MainPresenter (
        IMainView view,
        ISongRepository songRepository,
        IArtistRepository artistRepository,
        ICsvService csvService,
        IDtoService dtoService)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _songRepository = songRepository ?? throw new ArgumentNullException(nameof(songRepository));
        _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
        _csvService = csvService ?? throw new ArgumentNullException(nameof(csvService));
        _dtoService = dtoService ?? throw new ArgumentNullException(nameof(dtoService));

        // Wire event subscriptions
        _view.ViewLoaded += OnViewLoadedAsync;
        _view.SearchRequested += OnSearchRequestedAsync;
        _view.ImportCsvRequested += OnImportCsvRequestedAsync;
    }

    private async void OnViewLoadedAsync (object? sender, EventArgs e)
    {
        await RefreshSongListAsync();
    }

    private async void OnSearchRequestedAsync (object? sender, EventArgs e)
    {
        await RefreshSongListAsync(_view.SearchText);
    }

    private async void OnImportCsvRequestedAsync (object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_view.SelectedCsvPath))
        {
            return;
        }

        try
        {
            var importedEntries = await _csvService.ImportSongsAsync(_view.SelectedCsvPath);
            var existingArtists = _artistRepository.GetAll().ToList();

            foreach (var (song, artistName) in importedEntries)
            {
                if (!string.IsNullOrWhiteSpace(artistName))
                {
                    var artist = existingArtists.FirstOrDefault(a => a.Name.Equals(artistName, StringComparison.OrdinalIgnoreCase));
                    if (artist == null)
                    {
                        artist = new Artist { Name = artistName };
                        _artistRepository.Insert(artist);
                        existingArtists.Add(artist);
                    }

                    song.ArtistId = artist.Id;
                }

                _songRepository.Insert(song);
            }

            _view.ShowSuccessMessage("CSV songs successfully imported.");
            await RefreshSongListAsync();
        }
        catch (Exception ex)
        {
            _view.ShowErrorMessage($"CSV Import failed: {ex.Message}");
        }
    }

    private async Task RefreshSongListAsync (string? filterText = null)
    {
        try
        {
            var songs = _songRepository.GetAll();
            var artists = _artistRepository.GetAll();
            var artistDict = artists.ToDictionary(a => a.Id, a => a.Name);

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                songs = songs.Where(s => s.Title.Contains(filterText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Centralized transformation using IDtoService
            var displayDtos = _dtoService.MapToSongDisplayDtos(songs, artistDict).ToList();

            _view.DisplaySongs(displayDtos);
        }
        catch (Exception ex)
        {
            _view.ShowErrorMessage($"Failed to load songs: {ex.Message}");
        }
    }
}