using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Represents the main application window implementing <see cref="IMainView"/> for passive UI rendering.
    /// </summary>
    public partial class MainForm : Form, IMainView
    {
        /// <inheritdoc />
        public event EventHandler? UserSelectionChanged;

        /// <inheritdoc />
        public event EventHandler? NavSongsClicked;

        /// <inheritdoc />
        public event EventHandler? NavSonglistsClicked;

        /// <inheritdoc />
        public event EventHandler? NavStatisticsClicked;

        /// <inheritdoc />
        public event EventHandler? AddUserClicked;

        /// <inheritdoc />
        public event EventHandler? EditUserClicked;

        /// <inheritdoc />
        public UserDto? SelectedUser => cmbUser.SelectedItem as UserDto;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm ()
        {
            InitializeComponent();

            // Forward WinForms control events to interface contract events
            cmbUser.SelectedIndexChanged += (s, e) => UserSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnNavSongs.Click += (s, e) => NavSongsClicked?.Invoke(this, EventArgs.Empty);
            btnNavSonglists.Click += (s, e) => NavSonglistsClicked?.Invoke(this, EventArgs.Empty);
            btnNavStatistics.Click += (s, e) => NavStatisticsClicked?.Invoke(this, EventArgs.Empty);

            btnAddUser.Click += (s, e) => AddUserClicked?.Invoke(this, EventArgs.Empty);
            btnEditUser.Click += (s, e) => EditUserClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void DisplayUsers (IEnumerable<UserDto> users)
        {
            cmbUser.DisplayMember = nameof(UserDto.Name);
            cmbUser.ValueMember = nameof(UserDto.Id);
            cmbUser.DataSource = users.ToList();
        }

        /// <inheritdoc />
        public void SelectUser (string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                cmbUser.SelectedIndex = -1;
                return;
            }

            if (cmbUser.DataSource is IEnumerable<UserDto> users)
            {
                var targetUser = users.FirstOrDefault(u => u.Id == userId);
                if (targetUser != null && !Equals(cmbUser.SelectedItem, targetUser))
                {
                    cmbUser.SelectedItem = targetUser;
                }
            }
        }

        /// <inheritdoc />
        public void ShowView (System.Windows.Forms.UserControl view)
        {
            pnlContentContainer.SuspendLayout();
            pnlContentContainer.Controls.Clear();
            view.Dock = DockStyle.Fill;
            pnlContentContainer.Controls.Add(view);
            pnlContentContainer.ResumeLayout();
        }

        /// <inheritdoc />
        public UserDto? GetUserInput (UserDto? userToEdit = null)
        {
            using var dialog = userToEdit == null ? new UserEditDialog() : new UserEditDialog(userToEdit);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                return dialog.UserDto;
            }

            return null;
        }
    }
}