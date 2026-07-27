using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Services.DTOs;
using MongoDB_SongManager.Views;

namespace MongoDB_SongManager.Presenters
{
    /// <summary>
    /// Presenter responsible for managing setlists, song transfers, item reordering, and read-only protection for non-owned setlists.
    /// Uses DTOs for all view interactions and prevents duplicate songs in setlists.
    /// </summary>
    public class SonglistsPresenter
    {
        private readonly ISonglistsView _view;
        private readonly ISonglistRepository _songlistRepository;
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDtoService _dtoService;

        private List<Songlist> _allSonglists = new();
        private List<Song> _allSongs = new();
        private Dictionary<string, string> _artistNames = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SonglistsPresenter"/> class.
        /// </summary>
        /// <param name="view">The songlists view interface.</param>
        /// <param name="songlistRepository">The repository for setlist data operations.</param>
        /// <param name="songRepository">The repository for song data operations.</param>
        /// <param name="artistRepository">The repository for artist data operations.</param>
        /// <param name="currentUserService">The service tracking the active application user.</param>
        /// <param name="dtoService">The service handling data transfer object mappings.</param>
        /// <exception cref="ArgumentNullException">Thrown when any required dependency is null.</exception>
        public SonglistsPresenter (
            ISonglistsView view,
            ISonglistRepository songlistRepository,
            ISongRepository songRepository,
            IArtistRepository artistRepository,
            ICurrentUserService currentUserService,
            IDtoService dtoService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _songlistRepository = songlistRepository ?? throw new ArgumentNullException(nameof(songlistRepository));
            _songRepository = songRepository ?? throw new ArgumentNullException(nameof(songRepository));
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _dtoService = dtoService ?? throw new ArgumentNullException(nameof(dtoService));

            WireUpEvents();
            LoadInitialData();
        }

        /// <summary>
        /// Subscribes to view interactions and user service state changes.
        /// </summary>
        private void WireUpEvents ()
        {
            // Songlist selection & management
            _view.SonglistSelectionChanged += OnSonglistSelectionChanged;
            _view.FilterMySonglistsOnlyChanged += (s, e) => ApplySonglistFilter();
            _view.CreateSonglistClicked += OnCreateSonglistClicked;
            _view.RenameSonglistClicked += OnRenameSonglistClicked;
            _view.DeleteSonglistClicked += OnDeleteSonglistClicked;

            // Song Transfer & Ordering
            _view.AddSongToSonglistClicked += OnAddSongToSonglistClicked;
            _view.RemoveSongFromSonglistClicked += OnRemoveSongFromSonglistClicked;
            _view.MoveSongUpClicked += OnMoveSongUpClicked;
            _view.MoveSongDownClicked += OnMoveSongDownClicked;

            // Available Songs Search
            _view.AvailableSongsSearchTextChanged += (s, e) => RefreshAvailableSongs();

            // React to top-bar current user changes
            _currentUserService.CurrentUserChanged += (s, e) =>
            {
                ApplySonglistFilter();
                UpdateReadOnlyState();
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
        }

        /// <summary>
        /// Fetches all artists and creates a quick lookup dictionary.
        /// </summary>
        private void LoadArtists ()
        {
            var artists = _artistRepository.GetAll();
            _artistNames = artists.ToDictionary(a => a.Id, a => a.Name);
        }

        /// <summary>
        /// Fetches all songs and populates the available songs view grid.
        /// </summary>
        private void LoadSongs ()
        {
            _allSongs = _songRepository.GetAll().ToList();
            RefreshAvailableSongs();
        }

        /// <summary>
        /// Fetches all songlists and applies the current user filter.
        /// </summary>
        private void LoadSonglists ()
        {
            _allSonglists = _songlistRepository.GetAll().ToList();
            ApplySonglistFilter();
        }

        /// <summary>
        /// Refreshes the songlist sidebar display based on the "Only My Songlists" filter checkbox.
        /// Maps domain models to DTOs for view consumption.
        /// </summary>
        private void ApplySonglistFilter ()
        {
            string currentUserId = _currentUserService.CurrentUserId;
            IEnumerable<Songlist> filtered = _allSonglists;

            if (_view.IsFilterMySonglistsOnlyActive)
            {
                filtered = filtered.Where(s => s.CreatorId == currentUserId);
            }

            var sortedList = filtered
                .OrderByDescending(s => s.CreatorId == currentUserId)
                .ThenBy(s => s.Name)
                .ToList();

            var songlistDtos = _dtoService.MapToSonglistDtos(sortedList);

            _view.DisplaySonglists(songlistDtos, currentUserId);
            UpdateReadOnlyState();
        }

        /// <summary>
        /// Refreshes the grid of available songs based on search criteria.
        /// </summary>
        private void RefreshAvailableSongs ()
        {
            string search = _view.AvailableSongsSearchTerm;
            IEnumerable<Song> songs = _allSongs;

            if (!string.IsNullOrWhiteSpace(search))
            {
                songs = songs.Where(s =>
                {
                    string artistName = _artistNames.TryGetValue(s.ArtistId ?? string.Empty, out var name) ? name : string.Empty;
                    return s.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                           artistName.Contains(search, StringComparison.OrdinalIgnoreCase);
                });
            }

            var songDtos = _dtoService.MapToSongDisplayDtos(songs, _artistNames);
            _view.DisplayAvailableSongs(songDtos);
        }

        /// <summary>
        /// Updates the assigned songs grid when the selected songlist changes and evaluates read-only rules.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnSonglistSelectionChanged (object? sender, EventArgs e)
        {
            RefreshAssignedSongs();
            UpdateReadOnlyState();
        }

        /// <summary>
        /// Loads assigned songs for the currently selected setlist DTO in their specified order.
        /// </summary>
        private void RefreshAssignedSongs ()
        {
            var selectedDto = _view.SelectedSonglist;
            if (selectedDto == null)
            {
                _view.DisplayAssignedSongs(Enumerable.Empty<SongDisplayDto>());
                return;
            }

            var selectedList = _allSonglists.FirstOrDefault(s => s.Id == selectedDto.Id);
            if (selectedList == null || selectedList.SongIds == null || !selectedList.SongIds.Any())
            {
                _view.DisplayAssignedSongs(Enumerable.Empty<SongDisplayDto>());
                return;
            }

            // Map IDs in SongIds order to maintain setlist ordering
            var songMap = _allSongs.ToDictionary(s => s.Id);
            var assignedSongs = selectedList.SongIds
                .Where(id => songMap.ContainsKey(id))
                .Select(id => songMap[id])
                .ToList();

            var assignedDtos = _dtoService.MapToSongDisplayDtos(assignedSongs, _artistNames);
            _view.DisplayAssignedSongs(assignedDtos);
        }

        /// <summary>
        /// Enforces read-only controls if the active setlist does not belong to the active user.
        /// </summary>
        private void UpdateReadOnlyState ()
        {
            var selectedList = _view.SelectedSonglist;
            bool isEditable = selectedList != null && selectedList.CreatorId == _currentUserService.CurrentUserId;

            _view.SetReadOnlyState(!isEditable);
        }

        /// <summary>
        /// Adds a selected song to the active setlist, ensuring duplicates are prevented.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnAddSongToSonglistClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;
            var songToAdd = _view.SelectedAvailableSong;

            if (selectedListDto == null || songToAdd == null) return;
            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId) return;

            var selectedList = _allSonglists.FirstOrDefault(s => s.Id == selectedListDto.Id);
            if (selectedList == null) return;

            // Ensure duplicate songs cannot be added to the same playlist
            if (selectedList.SongIds.Contains(songToAdd.Id))
            {
                return;
            }

            selectedList.SongIds.Add(songToAdd.Id);
            _songlistRepository.Update(selectedList);

            RefreshAssignedSongs();
        }

        /// <summary>
        /// Removes a selected song from the active setlist.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnRemoveSongFromSonglistClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;
            var songToRemove = _view.SelectedAssignedSong;

            if (selectedListDto == null || songToRemove == null) return;
            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId) return;

            var selectedList = _allSonglists.FirstOrDefault(s => s.Id == selectedListDto.Id);
            if (selectedList == null) return;

            selectedList.SongIds.Remove(songToRemove.Id);
            _songlistRepository.Update(selectedList);

            RefreshAssignedSongs();
        }

        /// <summary>
        /// Moves a song up one position in the setlist sequence.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnMoveSongUpClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;
            var selectedSong = _view.SelectedAssignedSong;

            if (selectedListDto == null || selectedSong == null) return;
            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId) return;

            var selectedList = _allSonglists.FirstOrDefault(s => s.Id == selectedListDto.Id);
            if (selectedList == null) return;

            int index = selectedList.SongIds.IndexOf(selectedSong.Id);
            if (index > 0)
            {
                selectedList.SongIds.RemoveAt(index);
                selectedList.SongIds.Insert(index - 1, selectedSong.Id);
                _songlistRepository.Update(selectedList);

                RefreshAssignedSongs();
            }
        }

        /// <summary>
        /// Moves a song down one position in the setlist sequence.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnMoveSongDownClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;
            var selectedSong = _view.SelectedAssignedSong;

            if (selectedListDto == null || selectedSong == null) return;
            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId) return;

            var selectedList = _allSonglists.FirstOrDefault(s => s.Id == selectedListDto.Id);
            if (selectedList == null) return;

            int index = selectedList.SongIds.IndexOf(selectedSong.Id);
            if (index >= 0 && index < selectedList.SongIds.Count - 1)
            {
                selectedList.SongIds.RemoveAt(index);
                selectedList.SongIds.Insert(index + 1, selectedSong.Id);
                _songlistRepository.Update(selectedList);

                RefreshAssignedSongs();
            }
        }

        /// <summary>
        /// Prompts user input and creates a new songlist.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnCreateSonglistClicked (object? sender, EventArgs e)
        {
            string listName = Microsoft.VisualBasic.Interaction.InputBox("Name of the new setlist:", "New Setlist", "New Setlist");
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
        /// Prompts user input and renames the currently selected setlist if authorized.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnRenameSonglistClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;

            if (selectedListDto == null)
            {
                MessageBox.Show("Please select a setlist to rename.", "Rename Setlist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId)
            {
                MessageBox.Show("You can only rename setlists created by you.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedList = _allSonglists.FirstOrDefault(s => s.Id == selectedListDto.Id);
            if (selectedList == null) return;

            string newName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter a new name for the setlist:",
                "Rename Setlist",
                selectedList.Name);

            if (!string.IsNullOrWhiteSpace(newName) && newName != selectedList.Name)
            {
                selectedList.Name = newName;
                _songlistRepository.Update(selectedList);

                // Reload lists to reflect the updated name in the UI
                LoadSonglists();
            }
        }

        /// <summary>
        /// Soft-deletes the selected songlist after user confirmation.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event args.</param>
        private void OnDeleteSonglistClicked (object? sender, EventArgs e)
        {
            var selectedListDto = _view.SelectedSonglist;
            if (selectedListDto == null) return;
            if (selectedListDto.CreatorId != _currentUserService.CurrentUserId) return;

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete the setlist '{selectedListDto.Name}'?",
                "Delete Setlist",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _songlistRepository.Delete(selectedListDto.Id);
                LoadSonglists();
            }
        }
    }
}