using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Services.DTOs;
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

    public MainPresenter (
        IMainView view,
        ISongRepository songRepository,
        IArtistRepository artistRepository,
        ICsvService csvService)
    {
        _view = view;
        _songRepository = songRepository;
        _artistRepository = artistRepository;
        _csvService = csvService;

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
            var existingArtists = await _artistRepository.GetAllActiveAsync();

            foreach (var (song, artistName) in importedEntries)
            {
                if (!string.IsNullOrWhiteSpace(artistName))
                {
                    var artist = existingArtists.FirstOrDefault(a => a.Name.Equals(artistName, StringComparison.OrdinalIgnoreCase));
                    if (artist == null)
                    {
                        artist = new Artist { Name = artistName };
                        await _artistRepository.CreateAsync(artist);
                        existingArtists.Add(artist);
                    }

                    song.ArtistId = artist.Id;
                }

                await _songRepository.CreateAsync(song);
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
            var songs = await _songRepository.GetAllActiveAsync();
            var artists = await _artistRepository.GetAllActiveAsync();
            var artistDict = artists.ToDictionary(a => a.Id, a => a.Name);

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                songs = songs.Where(s => s.Title.Contains(filterText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var displayDtos = songs.Select(s => new SongDisplayDto
            {
                Id = s.Id,
                Title = s.Title,
                ArtistName = !string.IsNullOrEmpty(s.ArtistId) && artistDict.TryGetValue(s.ArtistId, out var artistName)
                    ? artistName
                    : "Unknown",
                ChordsUrl = s.ChordsUrl,
                YoutubeUrl = s.YoutubeUrl,
                SongbookInfo = s.Liederbuchnummer.HasValue ? $"Book #{s.Liederbuchnummer} / Page {s.Liederbuchseite}" : "-"
            }).ToList();

            _view.DisplaySongs(displayDtos);
        }
        catch (Exception ex)
        {
            _view.ShowErrorMessage($"Failed to load songs: {ex.Message}");
        }
    }
}