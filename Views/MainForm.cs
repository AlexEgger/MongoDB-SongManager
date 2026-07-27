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
        public UserDto? SelectedUser => cmbUser.SelectedItem as UserDto;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm ()
        {
            InitializeComponent();

            // Forward WinForms events to interface contract
            cmbUser.SelectedIndexChanged += (s, e) => UserSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnNavSongs.Click += (s, e) => NavSongsClicked?.Invoke(this, EventArgs.Empty);
            btnNavSonglists.Click += (s, e) => NavSonglistsClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void DisplayUsers (IEnumerable<UserDto> users)
        {
            cmbUser.DisplayMember = nameof(UserDto.Name);
            cmbUser.ValueMember = nameof(UserDto.Id);
            cmbUser.DataSource = users.ToList();
        }

        /// <inheritdoc />
        public void ShowView (UserControl view)
        {
            pnlContentContainer.SuspendLayout();
            pnlContentContainer.Controls.Clear();
            view.Dock = DockStyle.Fill;
            pnlContentContainer.Controls.Add(view);
            pnlContentContainer.ResumeLayout();
        }
    }
}