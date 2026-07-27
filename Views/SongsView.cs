using MongoDB_SongManager.Services.DTOs;
using MongoDB_SongManager.Views;

namespace SongManager.Views
{
    /// <summary>
    /// UserControl representing the main song management view implementation using DTO objects.
    /// </summary>
    public partial class SongsView : UserControl, ISongsView
    {
        private bool _isFavoritesFilterActive;
        private ContextMenuStrip _songlistContextMenu = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="SongsView"/> class.
        /// </summary>
        public SongsView ()
        {
            InitializeComponent();
            InitializeSonglistContextMenu();
            WireUpEvents();
        }

        /// <summary>
        /// Initializes the context menu for the playlist list box to support right-click actions like renaming.
        /// </summary>
        private void InitializeSonglistContextMenu ()
        {
            _songlistContextMenu = new ContextMenuStrip();
            var renameMenuItem = new ToolStripMenuItem("Rename Playlist");

            renameMenuItem.Click += (s, e) => RenameSonglistClicked?.Invoke(this, EventArgs.Empty);
            _songlistContextMenu.Items.Add(renameMenuItem);

            // Attach context menu to the playlist list box
            lstSongLists.ContextMenuStrip = _songlistContextMenu;

            // Ensure right-clicking selects the item under the cursor before showing the context menu
            lstSongLists.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    int index = lstSongLists.IndexFromPoint(e.Location);
                    if (index != ListBox.NoMatches)
                    {
                        lstSongLists.SelectedIndex = index;
                    }
                }
            };
        }

        /// <summary>
        /// Subscribes internal UI control events to expose them via the <see cref="ISongsView"/> contract.
        /// </summary>
        private void WireUpEvents ()
        {
            // Search & Filter Events
            txtSearch.TextChanged += (s, e) => SearchTextChanged?.Invoke(this, EventArgs.Empty);
            btnFilterFavorites.Click += (s, e) =>
            {
                _isFavoritesFilterActive = !_isFavoritesFilterActive;
                btnFilterFavorites.Text = _isFavoritesFilterActive ? " Only ⭐" : "⭐ Favorites";
                FilterFavoritesClicked?.Invoke(this, EventArgs.Empty);
            };

            // Song Selection & CRUD Events (Using ToolStripButtons)
            dgvSongs.SelectionChanged += (s, e) => SongSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnAddSong.Click += (s, e) => AddSongClicked?.Invoke(this, EventArgs.Empty);
            btnEditSong.Click += (s, e) => EditSongClicked?.Invoke(this, EventArgs.Empty);
            btnDeleteSong.Click += (s, e) => DeleteSongClicked?.Invoke(this, EventArgs.Empty);

            // Songlist Selection & CRUD Events
            lstSongLists.SelectedIndexChanged += (s, e) => SonglistSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnCreateList.Click += (s, e) => CreateSonglistClicked?.Invoke(this, EventArgs.Empty);

            // Artist CRUD Events
            btnAddArtist.Click += (s, e) => AddArtistClicked?.Invoke(this, EventArgs.Empty);

            // CSV Export / Import Events
            btnExportCsv.Click += (s, e) => ExportCsvClicked?.Invoke(this, EventArgs.Empty);
            btnImportCsv.Click += (s, e) => ImportCsvClicked?.Invoke(this, EventArgs.Empty);
        }

        #region ISongsView Implementation

        #region Properties

        /// <summary>
        /// Gets the current text entered in the search textbox.
        /// </summary>
        public string SearchTerm => txtSearch?.Text ?? string.Empty;

        /// <summary>
        /// Gets the currently selected <see cref="SongDisplayDto"/> object stored in the active DataGridView row's Tag property.
        /// </summary>
        public SongDisplayDto? SelectedSong
        {
            get
            {
                if (dgvSongs.CurrentRow != null && dgvSongs.CurrentRow.Tag is SongDisplayDto dto)
                {
                    return dto;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the currently selected <see cref="SonglistDto"/> object stored in the active ListBox item's Tag property.
        /// </summary>
        public SonglistDto? SelectedSonglist
        {
            get
            {
                if (lstSongLists.SelectedItem is ListBoxItemWrapper wrapper && wrapper.Value is SonglistDto songlist)
                {
                    return songlist;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the favorites filter toggle button is active.
        /// </summary>
        public bool IsFavoritesFilterActive => _isFavoritesFilterActive;

        #endregion

        #region Song Events

        public event EventHandler? SearchTextChanged;
        public event EventHandler? SongSelectionChanged;
        public event EventHandler? AddSongClicked;
        public event EventHandler? EditSongClicked;
        public event EventHandler? DeleteSongClicked;
        public event EventHandler? FilterFavoritesClicked;
        public event EventHandler? ToggleFavoriteClicked;
        public event EventHandler? AddArtistClicked;

        #endregion

        #region Songlist Events

        public event EventHandler? SonglistSelectionChanged;
        public event EventHandler? CreateSonglistClicked;
        public event EventHandler? RenameSonglistClicked;
        public event EventHandler? DeleteSonglistClicked;
        public event EventHandler? AddSongToSonglistClicked;
        public event EventHandler? RemoveSongFromSonglistClicked;

        #endregion

        #region CSV Events

        public event EventHandler? ExportCsvClicked;
        public event EventHandler? ImportCsvClicked;

        #endregion

        #region Display Methods

        /// <summary>
        /// Displays the collection of song DTOs in the DataGridView.
        /// </summary>
        /// <param name="songs">The collection of song DTOs tailored for presentation.</param>
        public void DisplaySongs (IEnumerable<SongDisplayDto> songs)
        {
            dgvSongs.Rows.Clear();

            foreach (var song in songs)
            {
                int rowIndex = dgvSongs.Rows.Add(song.Title, song.ArtistName);

                // Store DTO reference directly in row Tag
                dgvSongs.Rows[rowIndex].Tag = song;
            }
        }

        /// <summary>
        /// Displays the detailed metadata of a song using its presentation DTO.
        /// </summary>
        /// <param name="song">The song display DTO containing metadata, or null to clear panel.</param>
        public void DisplaySongDetails (SongDisplayDto? song)
        {
            if (song == null)
            {
                lblSongTitle.Text = "Title: -";
                lblArtist.Text = "Artist: -";
                lblTempo.Text = "Tempo: -";
                lblBookInfo.Text = "Songbook: -";
                lnkChords.Text = "No link";
                lnkYoutube.Text = "No link";
                return;
            }

            lblSongTitle.Text = $"Title: {song.Title}";
            lblArtist.Text = $"Artist: {song.ArtistName}";
            lblTempo.Text = song.Tempo.HasValue ? $"Tempo: {song.Tempo} BPM" : "Tempo: -";
            lblBookInfo.Text = $"Songbook: {song.SongbookInfo}";

            lnkChords.Text = string.IsNullOrEmpty(song.ChordsUrl) ? "No link" : "🎸 Open Chords";
            lnkYoutube.Text = string.IsNullOrEmpty(song.YoutubeUrl) ? "No link" : "▶️ YouTube Video";
        }

        /// <summary>
        /// Binds the collection of available song lists DTOs to the sidebar list box.
        /// Sorts the user's own playlists to the top of the list.
        /// </summary>
        public void DisplaySonglists (IEnumerable<SonglistDto> songlists, string currentUserId)
        {
            lstSongLists.Items.Clear();

            // Option for "All Songs (No filtering)"
            lstSongLists.Items.Add(new ListBoxItemWrapper("🎵 All Songs", null!));

            // Sort playlists: user's own playlists first, then secondary sort alphabetically by name
            var sortedSonglists = songlists
                                    .OrderByDescending(s => s.CreatorId == currentUserId)
                                    .ThenBy(s => s.Name);

            foreach (var songlist in sortedSonglists)
            {
                // Visual indicator: User's own playlist vs. public/other creator's playlist
                string prefix = songlist.CreatorId == currentUserId ? "👤 " : "🌐 ";
                string displayName = $"{prefix}{songlist.Name}";

                lstSongLists.Items.Add(new ListBoxItemWrapper(displayName, songlist));
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Helper wrapper class for displaying songlists in a standard ListBox control.
        /// </summary>
        private class ListBoxItemWrapper
        {
            public string DisplayName { get; }
            public object Value { get; }

            public ListBoxItemWrapper (string displayName, object value)
            {
                DisplayName = displayName;
                Value = value;
            }

            public override string ToString () => DisplayName;
        }
    }
}