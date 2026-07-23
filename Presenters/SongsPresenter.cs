using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;
using SongManager.Views;

namespace MongoDB_SongManager.Presenters
{
    /// <summary>
    /// Presenter responsible for binding song data, managing filters, and handling view interactions.
    /// </summary>
    public class SongsPresenter
    {
        private readonly ISongsView _view;
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ISonglistRepository _songlistRepository;
        private readonly IUserInteractionRepository _userInteractionRepository;
        private readonly CurrentUserService _currentUserService;
        private readonly IDtoService _dtoService;

        private List<Song> _allSongs = new();
        private List<Songlist> _allSonglists = new();
        private Dictionary<string, string> _artistNames = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SongsPresenter"/> class.
        /// </summary>
        public SongsPresenter (
            ISongsView view,
            ISongRepository songRepository,
            IArtistRepository artistRepository,
            ISonglistRepository songlistRepository,
            IUserInteractionRepository userInteractionRepository,
            CurrentUserService currentUserService,
            IDtoService dtoService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _songRepository = songRepository ?? throw new ArgumentNullException(nameof(songRepository));
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
            _songlistRepository = songlistRepository ?? throw new ArgumentNullException(nameof(songlistRepository));
            _userInteractionRepository = userInteractionRepository ?? throw new ArgumentNullException(nameof(userInteractionRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _dtoService = dtoService ?? throw new ArgumentNullException(nameof(dtoService));

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

            // Songlist Events
            _view.SonglistSelectionChanged += (s, e) => ApplyFilters();
            _view.CreateSonglistClicked += OnCreateSonglistClicked;

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
        /// Fetches all songlists and updates the view sidebar.
        /// </summary>
        private void LoadSonglists ()
        {
            _allSonglists = _songlistRepository.GetAll().ToList();
            _view.DisplaySonglists(_allSonglists, _currentUserService.CurrentUserId);
        }

        /// <summary>
        /// Applies active playlist, favorite status, and search string filters concurrently.
        /// </summary>
        private void ApplyFilters ()
        {
            IEnumerable<Song> filtered = _allSongs;

            // 1. Filter by selected setlist/playlist (if a songlist is selected in the view)
            var selectedList = _view.SelectedSonglist;
            if (selectedList != null)
            {
                var songIdsInList = new HashSet<string>(selectedList.SongIds ?? new List<string>());
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
        /// Displays detailed song information when selection changes.
        /// </summary>
        private void OnSongSelectionChanged (object? sender, EventArgs e)
        {
            var selectedDto = _view.SelectedSong;
            if (selectedDto == null)
            {
                _view.DisplaySongDetails(null, null);
                return;
            }

            // Find full model entity matching the selected DTO Id
            var selectedSong = _allSongs.FirstOrDefault(s => s.Id == selectedDto.Id);
            string? artistName = selectedDto.ArtistName;

            _view.DisplaySongDetails(selectedSong, artistName);
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
        /// Opens a dialog to create a new song and refreshes state upon confirmation.
        /// </summary>
        private void OnAddSongClicked (object? sender, EventArgs e)
        {
            var artists = _artistRepository.GetAll().ToList();
            using var dialog = new SongDialog(null, artists);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _songRepository.Insert(dialog.Song);
                LoadSongs();
                ApplyFilters();
            }
        }

        /// <summary>
        /// Opens a dialog to edit the selected song and updates the database.
        /// </summary>
        private void OnEditSongClicked (object? sender, EventArgs e)
        {
            var selectedDto = _view.SelectedSong;
            if (selectedDto == null) return;

            var selectedSong = _allSongs.FirstOrDefault(s => s.Id == selectedDto.Id);
            if (selectedSong == null) return;

            var artists = _artistRepository.GetAll().ToList();
            using var dialog = new SongDialog(selectedSong, artists);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _songRepository.Update(dialog.Song);
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
        /// Prompts the user for a title and inserts a new songlist.
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
    }
}