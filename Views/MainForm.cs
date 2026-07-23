using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Presenters;
using MongoDB_SongManager.Services;
using SongManager.Views;

namespace MongoDB_SongManager.Views
{
    public partial class MainForm : Form
    {
        private readonly CurrentUserService _currentUserService;
        private readonly IRepository<User> _userRepository;

        private readonly SongsView _songsView;
        private readonly SongsPresenter _songsPresenter;

        /// <summary>
        /// Initializes the main application window with dependency injection.
        /// </summary>
        public MainForm (
            CurrentUserService currentUserService,
            IRepository<User> userRepository,
            IRepository<Song> songRepository,
            IRepository<Artist> artistRepository)
        {
            InitializeComponent();

            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            // Initialize Sub-Views & Presenters
            _songsView = new SongsView();
            _songsPresenter = new SongsPresenter(_songsView, songRepository, artistRepository);

            // Setup Navigation
            btnNavSongs.Click += (s, e) => ShowView(_songsView);
            cmbUser.SelectedIndexChanged += OnUserSelectionChanged;

            LoadUsersIntoComboBox();

            // Default View
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
        /// Handles changes in the user ComboBox and informs the CurrentUserService.
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
        private void ShowView (UserControl view)
        {
            pnlContentContainer.Controls.Clear();
            view.Dock = DockStyle.Fill;
            pnlContentContainer.Controls.Add(view);
        }
    }
}