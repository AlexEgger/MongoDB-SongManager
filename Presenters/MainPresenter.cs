using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;
using SongManager.Views;

namespace MongoDB_SongManager.Presenters
{
    /// <summary>
    /// Presenter responsible for main application orchestration, view hosting, and global user state management.
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDtoService _dtoService;

        private readonly SongsView _songsView;
        private readonly SongsPresenter _songsPresenter;

        private readonly SonglistsView _songlistsView;
        private readonly SonglistsPresenter _songlistsPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPresenter"/> class and constructs child views and presenters.
        /// </summary>
        /// <param name="view">The main view contract.</param>
        /// <param name="userRepository">Repository for user data persistence.</param>
        /// <param name="songRepository">Repository for song data access.</param>
        /// <param name="artistRepository">Repository for artist data access.</param>
        /// <param name="songlistRepository">Repository for setlist data access.</param>
        /// <param name="userInteractionRepository">Repository for user interaction data access.</param>
        /// <param name="currentUserService">Service tracking active user state.</param>
        /// <param name="dtoService">Service providing DTO conversions.</param>
        /// <param name="csvService">Service handling CSV import and export operations.</param>
        public MainPresenter (
            IMainView view,
            IRepository<User> userRepository,
            ISongRepository songRepository,
            IArtistRepository artistRepository,
            ISonglistRepository songlistRepository,
            IUserInteractionRepository userInteractionRepository,
            ICurrentUserService currentUserService,
            IDtoService dtoService,
            ICsvService csvService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _dtoService = dtoService ?? throw new ArgumentNullException(nameof(dtoService));

            // Initialize Sub-Views & Presenters
            _songsView = new SongsView();
            _songsPresenter = new SongsPresenter(
                _songsView,
                songRepository,
                artistRepository,
                songlistRepository,
                userInteractionRepository,
                _currentUserService,
                _dtoService,
                csvService
            );

            _songlistsView = new SonglistsView();
            _songlistsPresenter = new SonglistsPresenter(
                _songlistsView,
                songlistRepository,
                songRepository,
                artistRepository,
                _currentUserService,
                _dtoService
            );

            WireUpEvents();
        }

        /// <summary>
        /// Subscribes to main view UI events.
        /// </summary>
        private void WireUpEvents ()
        {
            _view.UserSelectionChanged += OnUserSelectionChanged;
            _view.NavSongsClicked += OnNavSongsClicked;
            _view.NavSonglistsClicked += OnNavSonglistsClicked;
        }

        /// <summary>
        /// Initializes application state, loads registered users, and displays the default initial view.
        /// </summary>
        public void Initialize ()
        {
            LoadUsers();

            // Load and display default initial view
            _songsPresenter.LoadInitialData();
            _view.ShowView(_songsView);
        }

        /// <summary>
        /// Fetches user entities from database, maps them to DTOs, populates the view, and sets default active user.
        /// </summary>
        private void LoadUsers ()
        {
            var users = _userRepository.GetAll().ToList();
            var userDtos = _dtoService.MapToUserDtos(users).ToList();

            _view.DisplayUsers(userDtos);

            if (userDtos.Count > 0)
            {
                _currentUserService.SetCurrentUser(userDtos[0]);
            }
        }

        /// <summary>
        /// Handles changes to the active user selection.
        /// </summary>
        private void OnUserSelectionChanged (object? sender, EventArgs e)
        {
            if (_view.SelectedUser != null)
            {
                _currentUserService.SetCurrentUser(_view.SelectedUser);
            }
        }

        /// <summary>
        /// Refreshes song data and displays the Songs view.
        /// </summary>
        private void OnNavSongsClicked (object? sender, EventArgs e)
        {
            _songsPresenter.LoadInitialData();
            _view.ShowView(_songsView);
        }

        /// <summary>
        /// Refreshes setlist data and displays the Songlists view.
        /// </summary>
        private void OnNavSonglistsClicked (object? sender, EventArgs e)
        {
            _songlistsPresenter.LoadInitialData();
            _view.ShowView(_songlistsView);
        }
    }
}