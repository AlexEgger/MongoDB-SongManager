using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Services.DTOs;
using MongoDB_SongManager.Views;
using SongManager.Views;

namespace MongoDB_SongManager.Presenters
{
    /// <summary>
    /// Presenter responsible for binding song and artist data using DTOs, managing multi-criteria filters, 
    /// handling song/artist lifecycle events, and managing user-specific interactions (ratings and notes).
    /// </summary>
    public class SongsPresenter
    {
        private readonly ISongsView _view;
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ISonglistRepository _songlistRepository;
        private readonly IUserSongInteractionRepository _userInteractionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDtoService _dtoService;
        private readonly ICsvService _csvService;

        private List<Song> _allSongs = new();
        private List<Artist> _allArtists = new();
        private List<Songlist> _allSonglists = new();
        private Dictionary<string, string> _artistNames = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SongsPresenter"/> class.
        /// </summary>
        /// <param name="view">The songs view contract.</param>
        /// <param name="songRepository">Repository for song data access.</param>
        /// <param name="artistRepository">Repository for artist data access.</param>
        /// <param name="songlistRepository">Repository for songlist data access.</param>
        /// <param name="userInteractionRepository">Repository for user song interactions.</param>
        /// <param name="currentUserService">Service tracking the current user state.</param>
        /// <param name="dtoService">Service for DTO mappings.</param>
        /// <param name="csvService">Service providing CSV import and export capabilities.</param>
        public SongsPresenter (
            ISongsView view,
            ISongRepository songRepository,
            IArtistRepository artistRepository,
            ISonglistRepository songlistRepository,
            IUserSongInteractionRepository userInteractionRepository,
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
            // Song & Artist Events
            _view.SearchTextChanged += (s, e) => ApplyFilters();
            _view.SongSelectionChanged += OnSongSelectionChanged;
            _view.AddSongClicked += OnAddSongClicked;
            _view.AddArtistClicked += OnAddArtistClicked;
            _view.EditSongClicked += OnEditSongClicked;
            _view.DeleteSongClicked += OnDeleteSongClicked;
            _view.EditArtistClicked += OnEditArtistClicked;
            _view.DeleteArtistClicked += OnDeleteArtistClicked;

            // User Interaction Events (Ratings & Notes)
            _view.SaveInteractionClicked += OnSaveInteractionClicked;

            // CSV Export / Import Events
            _view.ExportCsvClicked += OnExportCsvClicked;
            _view.ImportCsvClicked += OnImportCsvClicked;

            // Songlist / Sidebar Mode Events
            _view.SonglistSelectionChanged += (s, e) => ApplyFilters();
            _view.RenameSonglistClicked += OnRenameSonglistClicked;
            _view.DeleteSonglistClicked += OnDeleteSonglistClicked;
            _view.AddSongToSonglistClicked += OnAddSongToSonglistClicked;
            _view.RemoveSongFromSonglistClicked += OnRemoveSongFromSonglistClicked;

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
        /// Fetches all active artists and populates the artist lookup dictionary and collection.
        /// </summary>
        private void LoadArtists ()
        {
            _allArtists = _artistRepository.GetAll().ToList();
            _artistNames = _allArtists.ToDictionary(a => a.Id, a => a.Name);
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
            var songlistDtos = _dtoService.MapToSonglistDtos(_allSonglists).ToList();
            _view.DisplaySonglists(songlistDtos, _currentUserService.CurrentUserId);
        }

        /// <summary>
        /// Applies active playlist, artist mode, and search string filters concurrently, safely fetching 
        /// user interactions only when a valid user ID is set.
        /// </summary>
        private void ApplyFilters ()
        {
            // Check if user selected the "All Artists" view mode in the sidebar
            if (_view.IsArtistModeActive)
            {
                IEnumerable<Artist> filteredArtists = _allArtists;

                string search = _view.SearchTerm;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    filteredArtists = filteredArtists.Where(a => a.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                var artistDtos = _dtoService.MapToArtistDtos(filteredArtists);
                _view.DisplayArtists(artistDtos);
                _view.DisplaySongDetails(null);
                _view.DisplayUserInteraction(null);
                return;
            }

            IEnumerable<Song> filtered = _allSongs;

            // 1. Filter by selected setlist/playlist DTO
            var selectedListDto = _view.SelectedSonglist;
            if (selectedListDto != null)
            {
                var songIdsInList = new HashSet<string>(selectedListDto.SongIds ?? new List<string>());
                filtered = filtered.Where(s => songIdsInList.Contains(s.Id));
            }

            // 2. Filter by search query (song title or artist name)
            string searchQuery = _view.SearchTerm;
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                filtered = filtered.Where(s =>
                {
                    string artistName = _artistNames.TryGetValue(s.ArtistId ?? string.Empty, out var name) ? name : string.Empty;
                    return s.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                           artistName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);
                });
            }

            // 3. Fetch user-isolated interactions only if a valid CurrentUserId is active.
            string currentUserId = _currentUserService.CurrentUserId;
            var userInteractions = new Dictionary<string, UserSongInteraction>();

            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                userInteractions = _userInteractionRepository
                    .Find(x => x.UserId == currentUserId)
                    .ToDictionary(i => i.SongId);
            }

            // Transform domain models to presentation DTOs via DtoService
            var songDtos = _dtoService.MapToSongDisplayDtos(filtered, _artistNames, userInteractions);
            _view.DisplaySongs(songDtos);
        }

        /// <summary>
        /// Displays detailed song information and the active user's interaction when song selection changes.
        /// </summary>
        private void OnSongSelectionChanged (object? sender, EventArgs e)
        {
            if (_view.IsArtistModeActive)
            {
                _view.DisplaySongDetails(null);
                _view.DisplayUserInteraction(null);
                return;
            }

            var selectedDto = _view.SelectedSong;
            if (selectedDto == null)
            {
                _view.DisplaySongDetails(null);
                _view.DisplayUserInteraction(null);
                return;
            }

            _view.DisplaySongDetails(selectedDto);

            // Load user-isolated interaction for the selected song
            var interaction = _userInteractionRepository.GetInteraction(_currentUserService.CurrentUserId, selectedDto.Id);
            var interactionDto = _dtoService.MapToInteractionDto(interaction);

            _view.DisplayUserInteraction(interactionDto);
        }

        /// <summary>
        /// Saves or updates the user-isolated interaction (ratings and notes) for the currently selected song.
        /// </summary>
        private void OnSaveInteractionClicked (object? sender, EventArgs e)
        {
            var selectedSong = _view.SelectedSong;
            if (selectedSong == null) return;

            var interactionInput = _view.GetUserInteractionInput();
            if (interactionInput == null) return;

            string currentUserId = _currentUserService.CurrentUserId;
            interactionInput.SongId = selectedSong.Id;

            var existingEntity = _userInteractionRepository.GetInteraction(currentUserId, selectedSong.Id);
            var interactionEntity = _dtoService.MapToInteractionEntity(interactionInput, currentUserId);

            if (interactionEntity == null) return;

            if (existingEntity != null)
            {
                interactionEntity.Id = existingEntity.Id;
                _userInteractionRepository.Update(interactionEntity);
            }
            else
            {
                _userInteractionRepository.Insert(interactionEntity);
            }

            ApplyFilters();

            // Refresh selected song details view to show updated interaction state
            var updatedInteraction = _userInteractionRepository.GetInteraction(currentUserId, selectedSong.Id);
            _view.DisplayUserInteraction(_dtoService.MapToInteractionDto(updatedInteraction));
        }

        /// <summary>
        /// Opens a dialog to create a new artist and refreshes state upon confirmation.
        /// </summary>
        private void OnAddArtistClicked (object? sender, EventArgs e)
        {
            using var dialog = new ArtistEditDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var artistEntity = _dtoService.MapToArtistEntity(dialog.Artist);
                    if (artistEntity != null)
                    {
                        _artistRepository.Insert(artistEntity);
                        LoadArtists();
                        ApplyFilters();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Duplicate Artist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Opens a dialog to edit the selected artist and updates database records.
        /// </summary>
        private void OnEditArtistClicked (object? sender, EventArgs e)
        {
            var selectedArtistDto = _view.SelectedArtist;
            if (selectedArtistDto == null)
            {
                MessageBox.Show("Please select an artist to edit.", "Edit Artist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var artistEntity = _allArtists.FirstOrDefault(a => a.Id == selectedArtistDto.Id);
            if (artistEntity == null) return;

            using var dialog = new ArtistEditDialog(_dtoService.MapToArtistDto(artistEntity));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var updatedEntity = _dtoService.MapToArtistEntity(dialog.Artist);
                    if (updatedEntity != null)
                    {
                        updatedEntity.Id = artistEntity.Id;
                        _artistRepository.Update(updatedEntity);
                        LoadArtists();
                        LoadSongs();
                        ApplyFilters();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Duplicate Artist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Prompts confirmation before deleting the selected artist and handling referencing songs.
        /// </summary>
        private void OnDeleteArtistClicked (object? sender, EventArgs e)
        {
            var selectedArtistDto = _view.SelectedArtist;
            if (selectedArtistDto == null)
            {
                MessageBox.Show("Please select an artist to delete.", "Delete Artist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int dependentSongsCount = _allSongs.Count(s => s.ArtistId == selectedArtistDto.Id);

            if (dependentSongsCount > 0)
            {
                MessageBox.Show(
                    $"Cannot delete artist '{selectedArtistDto.Name}' because {dependentSongsCount} song(s) are still assigned to them. Please reassign or delete those songs first.",
                    "Deletion Blocked",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete the artist '{selectedArtistDto.Name}'?",
                "Delete Artist",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _artistRepository.Delete(selectedArtistDto.Id);
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
                var songEntity = _dtoService.MapToSongEntity(createdDto);

                if (songEntity != null)
                {
                    try
                    {
                        _songRepository.Insert(songEntity);
                        LoadSongs();
                        ApplyFilters();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Duplicate Song", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
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

            var songDtoToEdit = _dtoService.MapToSongDto(selectedSong);
            var artistDtos = GetArtistDtos();

            using var dialog = new SongDialog(songDtoToEdit, artistDtos);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var updatedDto = dialog.SongDto;
                var updatedEntity = _dtoService.MapToSongEntity(updatedDto);

                if (updatedEntity != null)
                {
                    try
                    {
                        _songRepository.Update(updatedEntity);
                        LoadSongs();
                        ApplyFilters();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Duplicate Song", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
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
        /// Handles adding the selected song to the currently selected songlist.
        /// </summary>
        private void OnAddSongToSonglistClicked (object? sender, EventArgs e)
        {
            var selectedSong = _view.SelectedSong;
            var selectedListDto = _view.SelectedSonglist;

            if (selectedSong == null || selectedListDto == null) return;

            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId)
            {
                MessageBox.Show("You can only edit songlists created by you.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var entityToUpdate = _allSonglists.FirstOrDefault(sl => sl.Id == selectedListDto.Id);
            if (entityToUpdate == null) return;

            if (!entityToUpdate.SongIds.Contains(selectedSong.Id))
            {
                entityToUpdate.SongIds.Add(selectedSong.Id);
                _songlistRepository.Update(entityToUpdate);
                LoadSonglists();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Handles removing the selected song from the currently selected songlist.
        /// </summary>
        private void OnRemoveSongFromSonglistClicked (object? sender, EventArgs e)
        {
            var selectedSong = _view.SelectedSong;
            var selectedListDto = _view.SelectedSonglist;

            if (selectedSong == null || selectedListDto == null) return;

            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId)
            {
                MessageBox.Show("You can only edit songlists created by you.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var entityToUpdate = _allSonglists.FirstOrDefault(sl => sl.Id == selectedListDto.Id);
            if (entityToUpdate == null) return;

            if (entityToUpdate.SongIds.Remove(selectedSong.Id))
            {
                _songlistRepository.Update(entityToUpdate);
                LoadSonglists();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Prompts confirmation before soft-deleting the selected songlist.
        /// </summary>
        private void OnDeleteSonglistClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;
            if (selectedListDto == null) return;

            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId)
            {
                MessageBox.Show("You can only delete songlists created by you.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete playlist '{selectedListDto.Name}'?",
                "Delete Songlist",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _songlistRepository.Delete(selectedListDto.Id);
                LoadSonglists();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Handles CSV export by exporting all currently filtered songs from the view.
        /// </summary>
        private void OnExportCsvClicked (object? sender, EventArgs e)
        {
            if (_view.IsArtistModeActive)
            {
                MessageBox.Show("CSV export is only available for songs.", "Export Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                            try
                            {
                                var newArtist = new Artist { Name = artistName };
                                _artistRepository.Insert(newArtist);
                                artistId = newArtist.Id;
                            }
                            catch (InvalidOperationException)
                            {
                                artistId = _artistRepository.GetAll()
                                    .FirstOrDefault(a => string.Equals(a.Name, artistName, StringComparison.OrdinalIgnoreCase))?.Id;
                            }
                        }

                        importedSong.ArtistId = artistId;
                    }

                    try
                    {
                        _songRepository.Insert(importedSong);
                        importedSongIds.Add(importedSong.Id);
                    }
                    catch (InvalidOperationException)
                    {
                        var existingSong = _allSongs.FirstOrDefault(s =>
                            string.Equals(s.Title, importedSong.Title, StringComparison.OrdinalIgnoreCase) &&
                            s.ArtistId == artistId);

                        if (existingSong != null)
                        {
                            importedSongIds.Add(existingSong.Id);
                        }
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

        /// <summary>
        /// Retrieves active artists mapped to <see cref="ArtistDto"/> instances via <see cref="IDtoService"/>.
        /// </summary>
        private IEnumerable<ArtistDto> GetArtistDtos ()
        {
            var artists = _artistRepository.GetAll();
            return _dtoService.MapToArtistDtos(artists);
        }
    }
}