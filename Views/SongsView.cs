using MongoDB_SongManager.Models;
using MongoDB_SongManager.Views;

namespace SongManager.Views
{
    /// <summary>
    /// UserControl representing the main song management view implementation of <see cref="ISongsView"/>.
    /// </summary>
    public partial class SongsView : UserControl, ISongsView
    {
        private bool _isFavoritesFilterActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="SongsView"/> class.
        /// </summary>
        public SongsView ()
        {
            InitializeComponent();
            WireUpEvents();
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
                btnFilterFavorites.Text = _isFavoritesFilterActive ? "⭐ Nur Favoriten" : "⭐ Favoriten";
                FilterFavoritesClicked?.Invoke(this, EventArgs.Empty);
            };

            // Song Selection & CRUD Events
            dgvSongs.SelectionChanged += (s, e) => SongSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnAddSong.Click += (s, e) => AddSongClicked?.Invoke(this, EventArgs.Empty);
            btnEditSong.Click += (s, e) => EditSongClicked?.Invoke(this, EventArgs.Empty);
            btnDeleteSong.Click += (s, e) => DeleteSongClicked?.Invoke(this, EventArgs.Empty);

            // Songlist Selection & CRUD Events
            lstSongLists.SelectedIndexChanged += (s, e) => SonglistSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnCreateList.Click += (s, e) => CreateSonglistClicked?.Invoke(this, EventArgs.Empty);

            // Artist CRUD Events
            btnAddArtist.Click += (s, e) => AddArtistClicked?.Invoke(this, EventArgs.Empty);
        }

        #region ISongsView Implementation

        #region Properties

        /// <summary>
        /// Gets the current text entered in the search textbox.
        /// </summary>
        public string SearchTerm => txtSearch?.Text ?? string.Empty;

        /// <summary>
        /// Gets the currently selected <see cref="Song"/> object stored in the active DataGridView row's Tag property.
        /// </summary>
        public Song? SelectedSong
        {
            get
            {
                if (dgvSongs.CurrentRow != null && dgvSongs.CurrentRow.Tag is Song song)
                {
                    return song;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the currently selected <see cref="Songlist"/> object stored in the active ListBox item's Tag property.
        /// </summary>
        public Songlist? SelectedSonglist
        {
            get
            {
                if (lstSongLists.SelectedItem is ListBoxItemWrapper wrapper && wrapper.Value is Songlist songlist)
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

        /// <summary>
        /// Occurs when the user changes the text in the search input box.
        /// </summary>
        public event EventHandler? SearchTextChanged;

        /// <summary>
        /// Occurs when the selected row in the songs DataGridView changes.
        /// </summary>
        public event EventHandler? SongSelectionChanged;

        /// <summary>
        /// Occurs when the user clicks the button to add a new song.
        /// </summary>
        public event EventHandler? AddSongClicked;

        /// <summary>
        /// Occurs when the user clicks the button to edit the selected song.
        /// </summary>
        public event EventHandler? EditSongClicked;

        /// <summary>
        /// Occurs when the user clicks the delete song button.
        /// </summary>
        public event EventHandler? DeleteSongClicked;

        /// <summary>
        /// Occurs when the user toggles the favorites filter button.
        /// </summary>
        public event EventHandler? FilterFavoritesClicked;

        /// <summary>
        /// Occurs when the user toggles the favorite status of a song.
        /// </summary>
        public event EventHandler? ToggleFavoriteClicked;

        /// <summary>
        /// Occurs when the user requests to create a new artist.
        /// </summary>
        public event EventHandler? AddArtistClicked;

        #endregion

        #region Songlist Events

        /// <summary>
        /// Occurs when the user selects a different song list from the list box.
        /// </summary>
        public event EventHandler? SonglistSelectionChanged;

        /// <summary>
        /// Occurs when the user requests to create a new song list.
        /// </summary>
        public event EventHandler? CreateSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to delete the selected song list.
        /// </summary>
        public event EventHandler? DeleteSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to add the selected song to the active song list.
        /// </summary>
        public event EventHandler? AddSongToSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to remove the selected song from the active song list.
        /// </summary>
        public event EventHandler? RemoveSongFromSonglistClicked;

        #endregion

        #region Display Methods

        /// <summary>
        /// Displays the collection of songs in the DataGridView, resolving artist names via lookup.
        /// </summary>
        /// <param name="songs">The collection of active song documents.</param>
        /// <param name="artistNames">A lookup dictionary mapping artist IDs to artist names.</param>
        public void DisplaySongs (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames)
        {
            dgvSongs.Rows.Clear();

            foreach (var song in songs)
            {
                string artistName = "Unbekannt";
                if (!string.IsNullOrEmpty(song.ArtistId) && artistNames.TryGetValue(song.ArtistId, out var resolvedName))
                {
                    artistName = resolvedName;
                }

                int rowIndex = dgvSongs.Rows.Add(song.Title, artistName);

                // Store entity reference directly in row Tag
                dgvSongs.Rows[rowIndex].Tag = song;
            }
        }

        /// <summary>
        /// Displays the detailed metadata of a song in the details panel.
        /// </summary>
        /// <param name="song">The song entity to display.</param>
        /// <param name="artistName">The resolved name of the associated artist.</param>
        public void DisplaySongDetails (Song? song, string? artistName)
        {
            if (song == null)
            {
                lblSongTitle.Text = "Titel: -";
                lblArtist.Text = "Interpret: -";
                lblTempo.Text = "Tempo: -";
                lblBookInfo.Text = "Liederbuch: -";
                lnkChords.Text = "Kein Link";
                lnkYoutube.Text = "Kein Link";
                return;
            }

            lblSongTitle.Text = $"Titel: {song.Title}";
            lblArtist.Text = $"Interpret: {artistName ?? "Unbekannt"}";
            lblTempo.Text = song.Tempo.HasValue ? $"Tempo: {song.Tempo} BPM" : "Tempo: -";

            string bookInfo = "-";
            if (song.Liederbuchnummer.HasValue || song.Liederbuchseite.HasValue)
            {
                bookInfo = $"Nr. {song.Liederbuchnummer?.ToString() ?? "-"}, S. {song.Liederbuchseite?.ToString() ?? "-"}";
            }
            lblBookInfo.Text = $"Liederbuch: {bookInfo}";

            lnkChords.Text = string.IsNullOrEmpty(song.ChordsUrl) ? "Kein Link" : "🎸 Akkorde öffnen";
            lnkYoutube.Text = string.IsNullOrEmpty(song.YoutubeUrl) ? "Kein Link" : "▶️ YouTube Video";
        }

        /// <summary>
        /// Binds the collection of available song lists to the sidebar list box.
        /// </summary>
        public void DisplaySonglists (IEnumerable<Songlist> songlists, string currentUserId)
        {
            lstSongLists.Items.Clear();

            // Option für "Alle Songs (Keine Filterung)"
            lstSongLists.Items.Add(new ListBoxItemWrapper("🎵 Alle Songs", null!));

            foreach (var songlist in songlists)
            {
                // Indikator-Icon zur Abgrenzung: Ist es meine Liste oder eine fremde/öffentliche?
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