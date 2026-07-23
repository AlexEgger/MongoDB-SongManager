using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Presenters;
using MongoDB_SongManager.Services;
using SongManager.Views;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Represents the main application form containing top-level navigation and user selection.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly CurrentUserService _currentUserService;
        private readonly IDtoService _dtoService;
        private readonly IRepository<User> _userRepository;

        private readonly SongsView _songsView;
        private readonly SongsPresenter _songsPresenter;

        private readonly SonglistsView _songlistsView;
        private readonly SonglistsPresenter _songlistsPresenter;

        /// <summary>
        /// Initializes the main application window with dependency injection.
        /// </summary>
        /// <param name="currentUserService">Service tracking active user state.</param>
        /// <param name="dtoService">Service mapping domain models to presentation DTOs.</param>
        /// <param name="userRepository">Repository for user data access.</param>
        /// <param name="songRepository">Repository for song data access.</param>
        /// <param name="artistRepository">Repository for artist data access.</param>
        /// <param name="songlistRepository">Repository for songlist data access.</param>
        /// <param name="userInteractionRepository">Repository for user interactions and favorites.</param>
        public MainForm (
            CurrentUserService currentUserService,
            IDtoService dtoService,
            IRepository<User> userRepository,
            ISongRepository songRepository,
            IArtistRepository artistRepository,
            ISonglistRepository songlistRepository,
            IUserInteractionRepository userInteractionRepository)
        {
            InitializeComponent();

            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _dtoService = dtoService ?? throw new ArgumentNullException(nameof(dtoService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            // Initialize Sub-Views & Presenters with required repositories and DTO service
            _songsView = new SongsView();
            _songsPresenter = new SongsPresenter(
                _songsView,
                songRepository,
                artistRepository,
                songlistRepository,
                userInteractionRepository,
                _currentUserService,
                _dtoService
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

            // Setup Navigation and Control Event Bindings
            btnNavSongs.Click += (s, e) => ShowView(_songsView);
            cmbUser.SelectedIndexChanged += OnUserSelectionChanged;
            btnNavSonglists.Click += (s, e) => ShowView(_songlistsView);

            LoadUsersIntoComboBox();

            // Display Default View
            ShowView(_songsView);
        }

        /// <summary>
        /// Loads all registered active users from MongoDB into the header ComboBox.
        /// </summary>
        private void LoadUsersIntoComboBox ()
        {
            var users = _userRepository.GetAll().ToList();
            cmbUser.DisplayMember = "Name";
            cmbUser.ValueMember = "Id";
            cmbUser.DataSource = users;

            if (users.Count > 0)
            {
                cmbUser.SelectedIndex = 0;
                _currentUserService.SetCurrentUser(users[0]);
            }
        }

        /// <summary>
        /// Handles changes in the user ComboBox selection and notifies the <see cref="CurrentUserService"/>.
        /// </summary>
        private void OnUserSelectionChanged (object? sender, EventArgs e)
        {
            if (cmbUser.SelectedItem is User selectedUser)
            {
                _currentUserService.SetCurrentUser(selectedUser);
            }
        }

        /// <summary>
        /// Displays the specified UserControl inside the main content container panel.
        /// </summary>
        /// <param name="view">The view control to display.</param>
        private void ShowView (UserControl view)
        {
            pnlContentContainer.Controls.Clear();
            view.Dock = DockStyle.Fill;
            pnlContentContainer.Controls.Add(view);
        }
    }
}