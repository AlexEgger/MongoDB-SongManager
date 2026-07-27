using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;
using SongManager.Views;

namespace MongoDB_SongManager.Presenters
{
    /// <summary>
    /// Presenter responsible for main application orchestration, view hosting, user CRUD processing, and global user state management.
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

        private readonly StatisticsView _statisticsView;
        private readonly StatisticsPresenter _statisticsPresenter;

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
            IUserSongInteractionRepository userInteractionRepository,
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

            _statisticsView = new StatisticsView();
            _statisticsPresenter = new StatisticsPresenter(
                _statisticsView,
                songRepository,
                artistRepository,
                songlistRepository,
                userInteractionRepository,
                _currentUserService,
                _dtoService
            );

            WireUpEvents();
        }

        /// <summary>
        /// Subscribes to main view UI events and global domain service notifications.
        /// </summary>
        private void WireUpEvents ()
        {
            _view.UserSelectionChanged += OnUserSelectionChanged;
            _view.NavSongsClicked += OnNavSongsClicked;
            _view.NavSonglistsClicked += OnNavSonglistsClicked;
            _view.NavStatisticsClicked += OnNavStatisticsClicked;

            _view.AddUserClicked += OnAddUserClicked;
            _view.EditUserClicked += OnEditUserClicked;

            // Reactively update the view when the active user state changes in CurrentUserService
            _currentUserService.CurrentUserChanged += OnCurrentUserChanged;
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

            if (userDtos.Count > 0 && _currentUserService.CurrentUser == null)
            {
                _currentUserService.SetCurrentUser(userDtos[0]);
            }
            else if (_currentUserService.CurrentUser != null)
            {
                _view.SelectUser(_currentUserService.CurrentUserId);
            }
        }

        /// <summary>
        /// Handles state changes emitted by <see cref="ICurrentUserService.CurrentUserChanged"/> to update the UI dropdown.
        /// </summary>
        private void OnCurrentUserChanged (object? sender, EventArgs e)
        {
            _view.SelectUser(_currentUserService.CurrentUserId);
        }

        /// <summary>
        /// Handles changes to the active user selection initiated in the UI view.
        /// </summary>
        private void OnUserSelectionChanged (object? sender, EventArgs e)
        {
            if (_view.SelectedUser != null)
            {
                _currentUserService.SetCurrentUser(_view.SelectedUser);
            }
        }

        /// <summary>
        /// Handles creation of a new user profile by prompting input, storing the entity, refreshing list, and updating user state.
        /// </summary>
        private void OnAddUserClicked (object? sender, EventArgs e)
        {
            var inputDto = _view.GetUserInput(null);
            if (inputDto == null || string.IsNullOrWhiteSpace(inputDto.Name))
            {
                return;
            }

            var newUser = new User
            {
                Name = inputDto.Name
            };

            _userRepository.Insert(newUser);
            var newUserDto = _dtoService.MapToUserDto(newUser);

            LoadUsers();

            // Setting active user in the service triggers CurrentUserChanged and auto-selects the new user in the UI
            _currentUserService.SetCurrentUser(newUserDto);
        }

        /// <summary>
        /// Handles updating the active user profile's details.
        /// </summary>
        private void OnEditUserClicked (object? sender, EventArgs e)
        {
            var selectedUser = _view.SelectedUser;
            if (selectedUser == null)
            {
                return;
            }

            var inputDto = _view.GetUserInput(selectedUser);
            if (inputDto == null || string.IsNullOrWhiteSpace(inputDto.Name))
            {
                return;
            }

            var existingUser = _userRepository.GetById(selectedUser.Id);
            if (existingUser != null)
            {
                existingUser.Name = inputDto.Name;
                _userRepository.Update(existingUser);

                var updatedUserDto = _dtoService.MapToUserDto(existingUser);
                LoadUsers();
                _currentUserService.SetCurrentUser(updatedUserDto);
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

        /// <summary>
        /// Refreshes statistical calculations and displays the Statistics view.
        /// </summary>
        private void OnNavStatisticsClicked (object? sender, EventArgs e)
        {
            _statisticsPresenter.LoadStatistics();
            _view.ShowView(_statisticsView);
        }
    }
}