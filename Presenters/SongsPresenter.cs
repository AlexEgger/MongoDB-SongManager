using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Services.DTOs;
using MongoDB_SongManager.Views;
using SongManager.Views;

namespace MongoDB_SongManager.Presenters
{
    /// <summary>
    /// Presenter responsible for binding song data using DTOs, managing multi-criteria filters, and handling song lifecycle events.
    /// </summary>
    public class SongsPresenter
    {
        private readonly ISongsView _view;
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ISonglistRepository _songlistRepository;
        private readonly IUserInteractionRepository _userInteractionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDtoService _dtoService;
        private readonly ICsvService _csvService;

        private List<Song> _allSongs = new();
        private List<Songlist> _allSonglists = new();
        private Dictionary<string, string> _artistNames = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SongsPresenter"/> class.
        /// </summary>
        /// <param name="view">The songs view contract.</param>
        /// <param name="songRepository">Repository for song data access.</param>
        /// <param name="artistRepository">Repository for artist data access.</param>
        /// <param name="songlistRepository">Repository for songlist data access.</param>
        /// <param name="userInteractionRepository">Repository for user interactions and favorites.</param>
        /// <param name="currentUserService">Service tracking the current user state.</param>
        /// <param name="dtoService">Service for DTO mappings.</param>
        /// <param name="csvService">Service providing CSV import and export capabilities.</param>
        public SongsPresenter (
            ISongsView view,
            ISongRepository songRepository,
            IArtistRepository artistRepository,
            ISonglistRepository songlistRepository,
            IUserInteractionRepository userInteractionRepository,
            ICurrentUserService currentUserService,
            IDtoService dtoService,
            ICsvService csvService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _songRepository = songRepository ?? throw new ArgumentNullException(nameof(songRepository));
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
            _songlistRepository = songlistRepository ?? throw new ArgumentNullException(nameof(songlistRepository));
            _userInteractionRepository = userInteractionRepository ?? throw new ArgumentNullException(nameof(userInteractionRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _dtoService = dtoService ?? throw new ArgumentNullException(nameof(dtoService));
            _csvService = csvService ?? throw new ArgumentNullException(nameof(csvService));

            WireUpEvents();
            LoadInitialData();
        }

        /// <summary>
        /// Subscribes to UI view events and service state changes.
        /// </summary>
        private void WireUpEvents ()
        {
            // Song Events
            _view.SearchTextChanged += (s, e) => ApplyFilters();
            _view.FilterFavoritesClicked += (s, e) => ApplyFilters();
            _view.SongSelectionChanged += OnSongSelectionChanged;
            _view.AddSongClicked += OnAddSongClicked;
            _view.AddArtistClicked += OnAddArtistClicked;
            _view.EditSongClicked += OnEditSongClicked;
            _view.DeleteSongClicked += OnDeleteSongClicked;

            // CSV Export / Import Events
            _view.ExportCsvClicked += OnExportCsvClicked;
            _view.ImportCsvClicked += OnImportCsvClicked;

            // Songlist Events
            _view.SonglistSelectionChanged += (s, e) => ApplyFilters();
            _view.CreateSonglistClicked += OnCreateSonglistClicked;
            _view.RenameSonglistClicked += OnRenameSonglistClicked;

            // Current User Changed
            _currentUserService.CurrentUserChanged += (s, e) =>
            {
                LoadSonglists();
                ApplyFilters();
            };
        }

        /// <summary>
        /// Loads initial data collections from database repositories.
        /// </summary>
        public void LoadInitialData ()
        {
            LoadArtists();
            LoadSongs();
            LoadSonglists();
            ApplyFilters();
        }

        /// <summary>
        /// Fetches all active artists and populates the artist lookup dictionary.
        /// </summary>
        private void LoadArtists ()
        {
            var artists = _artistRepository.GetAll();
            _artistNames = artists.ToDictionary(a => a.Id, a => a.Name);
        }

        /// <summary>
        /// Fetches all active songs from the database.
        /// </summary>
        private void LoadSongs ()
        {
            _allSongs = _songRepository.GetAll().ToList();
        }

        /// <summary>
        /// Fetches all songlists and updates the view sidebar with DTOs.
        /// </summary>
        private void LoadSonglists ()
        {
            _allSonglists = _songlistRepository.GetAll().ToList();

            var songlistDtos = _allSonglists.Select(sl => new SonglistDto
            {
                Id = sl.Id,
                Name = sl.Name,
                CreatorId = sl.CreatorId,
                SongIds = sl.SongIds
            }).ToList();

            _view.DisplaySonglists(songlistDtos, _currentUserService.CurrentUserId);
        }

        /// <summary>
        /// Applies active playlist, favorite status, and search string filters concurrently.
        /// </summary>
        private void ApplyFilters ()
        {
            IEnumerable<Song> filtered = _allSongs;

            // 1. Filter by selected setlist/playlist DTO
            var selectedListDto = _view.SelectedSonglist;
            if (selectedListDto != null)
            {
                var songIdsInList = new HashSet<string>(selectedListDto.SongIds ?? new List<string>());
                filtered = filtered.Where(s => songIdsInList.Contains(s.Id));
            }

            // 2. Filter by favorite status via UserSongInteraction records
            if (_view.IsFavoritesFilterActive)
            {
                string currentUserId = _currentUserService.CurrentUserId;

                var favoriteSongIds = _userInteractionRepository
                    .GetFavoritesByUserId(currentUserId)
                    .Select(interaction => interaction.SongId)
                    .ToHashSet();

                filtered = filtered.Where(s => favoriteSongIds.Contains(s.Id));
            }

            // 3. Filter by search query (song title or artist name)
            string search = _view.SearchTerm;
            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(s =>
                {
                    string artistName = _artistNames.TryGetValue(s.ArtistId ?? string.Empty, out var name) ? name : string.Empty;
                    return s.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                           artistName.Contains(search, StringComparison.OrdinalIgnoreCase);
                });
            }

            // Transform domain models to presentation DTOs via DtoService
            var songDtos = _dtoService.MapToSongDisplayDtos(filtered, _artistNames);
            _view.DisplaySongs(songDtos);
        }

        /// <summary>
        /// Displays detailed song information when selection changes using presentation DTOs.
        /// </summary>
        private void OnSongSelectionChanged (object? sender, EventArgs e)
        {
            var selectedDto = _view.SelectedSong;
            _view.DisplaySongDetails(selectedDto);
        }

        /// <summary>
        /// Opens a dialog to create a new artist and refreshes state upon confirmation.
        /// </summary>
        private void OnAddArtistClicked (object? sender, EventArgs e)
        {
            using var dialog = new ArtistDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _artistRepository.Insert(dialog.Artist);
                LoadArtists();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Opens a dialog to create a new song using <see cref="SongDto"/> and refreshes state upon confirmation.
        /// </summary>
        private void OnAddSongClicked (object? sender, EventArgs e)
        {
            var artistDtos = GetArtistDtos();
            using var dialog = new SongDialog(null, artistDtos);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var createdDto = dialog.SongDto;
                var songEntity = MapToSongEntity(createdDto);

                _songRepository.Insert(songEntity);
                LoadSongs();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Opens a dialog to edit the selected song using <see cref="SongDto"/> and updates the database.
        /// </summary>
        private void OnEditSongClicked (object? sender, EventArgs e)
        {
            var selectedDisplayDto = _view.SelectedSong;
            if (selectedDisplayDto == null) return;

            var selectedSong = _allSongs.FirstOrDefault(s => s.Id == selectedDisplayDto.Id);
            if (selectedSong == null) return;

            var songDtoToEdit = MapToSongDto(selectedSong);
            var artistDtos = GetArtistDtos();

            using var dialog = new SongDialog(songDtoToEdit, artistDtos);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var updatedDto = dialog.SongDto;
                var updatedEntity = MapToSongEntity(updatedDto);

                _songRepository.Update(updatedEntity);
                LoadSongs();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Prompts confirmation before soft-deleting the selected song.
        /// </summary>
        private void OnDeleteSongClicked (object? sender, EventArgs e)
        {
            var selectedDto = _view.SelectedSong;
            if (selectedDto == null) return;

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete the song '{selectedDto.Title}'?",
                "Delete Song",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _songRepository.Delete(selectedDto.Id);
                LoadSongs();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Handles CSV export by exporting all currently filtered songs from the view.
        /// </summary>
        private void OnExportCsvClicked (object? sender, EventArgs e)
        {
            var songsToExport = GetCurrentlyFilteredSongs().ToList();

            if (!songsToExport.Any())
            {
                MessageBox.Show("There are no songs in the current view to export.", "Export Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string defaultFileName = _view.SelectedSonglist != null
                ? $"{_view.SelectedSonglist.Name}_export.csv"
                : "songs_export.csv";

            string? filePath = null;

            // Dedicated STA thread for WinForms file dialogs
            Thread staThread = new Thread(() =>
            {
                using var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = defaultFileName
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveDialog.FileName;
                }
            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            if (!string.IsNullOrEmpty(filePath))
            {
                ExecuteExportAsync(songsToExport, filePath);
            }
        }

        /// <summary>
        /// Retrieves the current list of filtered songs matching active view criteria.
        /// </summary>
        private IEnumerable<Song> GetCurrentlyFilteredSongs ()
        {
            IEnumerable<Song> filtered = _allSongs;

            var selectedListDto = _view.SelectedSonglist;
            if (selectedListDto != null)
            {
                var songIdsInList = new HashSet<string>(selectedListDto.SongIds ?? new List<string>());
                filtered = filtered.Where(s => songIdsInList.Contains(s.Id));
            }

            if (_view.IsFavoritesFilterActive)
            {
                string currentUserId = _currentUserService.CurrentUserId;
                var favoriteSongIds = _userInteractionRepository
                    .GetFavoritesByUserId(currentUserId)
                    .Select(i => i.SongId)
                    .ToHashSet();

                filtered = filtered.Where(s => favoriteSongIds.Contains(s.Id));
            }

            string search = _view.SearchTerm;
            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(s =>
                {
                    string artistName = _artistNames.TryGetValue(s.ArtistId ?? string.Empty, out var name) ? name : string.Empty;
                    return s.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                           artistName.Contains(search, StringComparison.OrdinalIgnoreCase);
                });
            }

            return filtered;
        }

        /// <summary>
        /// Asynchronously exports songs to the designated CSV file path.
        /// </summary>
        private async void ExecuteExportAsync (List<Song> songsToExport, string filePath)
        {
            try
            {
                var artists = _artistRepository.GetAll().ToList();
                await _csvService.ExportSongsAsync(songsToExport, artists, filePath);
                MessageBox.Show($"{songsToExport.Count} songs successfully exported to CSV.", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting CSV: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles CSV import, offering options to group songs into a playlist or import them directly.
        /// </summary>
        private void OnImportCsvClicked (object? sender, EventArgs e)
        {
            string? filePath = null;

            Thread staThread = new Thread(() =>
            {
                using var openDialog = new OpenFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
                };

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openDialog.FileName;
                }
            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            if (string.IsNullOrEmpty(filePath)) return;

            var result = MessageBox.Show(
                "Do you want to create a new playlist for the imported songs?\n\n" +
                "• Click [Yes] to enter a playlist name and group them.\n" +
                "• Click [No] to ONLY import missing songs into your library.\n" +
                "• Click [Cancel] to abort.",
                "Import Option",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            string? playlistName = null;

            if (result == DialogResult.Yes)
            {
                string defaultListName = $"Imported - {Path.GetFileNameWithoutExtension(filePath)}";
                playlistName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter a name for the new playlist:",
                    "Create Playlist on Import",
                    defaultListName);

                if (string.IsNullOrWhiteSpace(playlistName)) return;
            }

            ExecuteImportAsync(filePath, playlistName);
        }

        /// <summary>
        /// Asynchronously parses CSV data, registers new artists, persists songs, and optionally builds a new playlist.
        /// </summary>
        private async void ExecuteImportAsync (string filePath, string? playlistName)
        {
            try
            {
                var importedData = await _csvService.ImportSongsAsync(filePath);
                var importedSongIds = new List<string>();

                foreach (var (importedSong, artistName) in importedData)
                {
                    string? artistId = null;

                    if (!string.IsNullOrWhiteSpace(artistName))
                    {
                        var existingArtist = _artistRepository.GetAll()
                            .FirstOrDefault(a => string.Equals(a.Name, artistName, StringComparison.OrdinalIgnoreCase));

                        if (existingArtist != null)
                        {
                            artistId = existingArtist.Id;
                        }
                        else
                        {
                            var newArtist = new Artist { Name = artistName };
                            _artistRepository.Insert(newArtist);
                            artistId = newArtist.Id;
                        }

                        importedSong.ArtistId = artistId;
                    }

                    var existingSong = _allSongs.FirstOrDefault(s =>
                        string.Equals(s.Title, importedSong.Title, StringComparison.OrdinalIgnoreCase) &&
                        s.ArtistId == artistId);

                    if (existingSong != null)
                    {
                        importedSongIds.Add(existingSong.Id);
                    }
                    else
                    {
                        _songRepository.Insert(importedSong);
                        importedSongIds.Add(importedSong.Id);
                    }
                }

                if (!string.IsNullOrWhiteSpace(playlistName))
                {
                    var newPlaylist = new Songlist
                    {
                        Name = playlistName,
                        CreatorId = _currentUserService.CurrentUserId,
                        SongIds = importedSongIds.Distinct().ToList()
                    };

                    _songlistRepository.Insert(newPlaylist);
                }

                LoadArtists();
                LoadSongs();
                LoadSonglists();
                ApplyFilters();

                string successMessage = !string.IsNullOrWhiteSpace(playlistName)
                    ? $"Successfully imported songs and created playlist '{playlistName}'."
                    : "Songs successfully imported into library.";

                MessageBox.Show(successMessage, "Import Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing CSV: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Prompts user input and inserts a new songlist.
        /// </summary>
        private void OnCreateSonglistClicked (object? sender, EventArgs e)
        {
            string listName = Microsoft.VisualBasic.Interaction.InputBox("Name of the new list:", "New Songlist", "New Setlist");
            if (!string.IsNullOrWhiteSpace(listName))
            {
                var newList = new Songlist
                {
                    Name = listName,
                    CreatorId = _currentUserService.CurrentUserId,
                    SongIds = new List<string>()
                };

                _songlistRepository.Insert(newList);
                LoadSonglists();
            }
        }

        /// <summary>
        /// Handles renaming the selected songlist if authorized.
        /// </summary>
        private void OnRenameSonglistClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;

            if (selectedListDto == null)
            {
                MessageBox.Show("Please select a playlist to rename.", "Rename Playlist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId)
            {
                MessageBox.Show("You can only rename playlists created by you.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var entityToUpdate = _allSonglists.FirstOrDefault(sl => sl.Id == selectedListDto.Id);
            if (entityToUpdate == null) return;

            string newListName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter a new name for the playlist:",
                "Rename Playlist",
                selectedListDto.Name);

            if (!string.IsNullOrWhiteSpace(newListName) && newListName != selectedListDto.Name)
            {
                entityToUpdate.Name = newListName;
                _songlistRepository.Update(entityToUpdate);
                LoadSonglists();
            }
        }

        #region Helper Mapping Methods

        /// <summary>
        /// Retrieves active artists mapped to <see cref="ArtistDto"/> instances.
        /// </summary>
        private IEnumerable<ArtistDto> GetArtistDtos ()
        {
            return _artistRepository.GetAll().Select(a => new ArtistDto
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
        }

        /// <summary>
        /// Maps a <see cref="Song"/> entity to a <see cref="SongDto"/> transfer object.
        /// </summary>
        private static SongDto MapToSongDto (Song song)
        {
            return new SongDto
            {
                Id = song.Id,
                Title = song.Title,
                ArtistId = song.ArtistId,
                Tempo = song.Tempo,
                ChordsUrl = song.ChordsUrl,
                YoutubeUrl = song.YoutubeUrl,
                Liederbuchnummer = song.Liederbuchnummer,
                Liederbuchseite = song.Liederbuchseite
            };
        }

        /// <summary>
        /// Maps a <see cref="SongDto"/> transfer object back to a <see cref="Song"/> domain entity.
        /// </summary>
        private static Song MapToSongEntity (SongDto dto)
        {
            return new Song
            {
                Id = dto.Id,
                Title = dto.Title,
                ArtistId = dto.ArtistId,
                Tempo = dto.Tempo,
                ChordsUrl = dto.ChordsUrl,
                YoutubeUrl = dto.YoutubeUrl,
                Liederbuchnummer = dto.Liederbuchnummer,
                Liederbuchseite = dto.Liederbuchseite
            };
        }

        #endregion
    }
}