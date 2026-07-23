using MongoDB_SongManager.Models;
using MongoDB_SongManager.Views;

namespace SongManager.Views
{
    /// <summary>
    /// UserControl representing the main song management view implementation of <see cref="ISongsView"/>.
    /// </summary>
    public partial class SongsView : UserControl, ISongsView
    {
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
            txtSearch.TextChanged += (s, e) => SearchTextChanged?.Invoke(this, EventArgs.Empty);
            dgvSongs.SelectionChanged += (s, e) => SongSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnDeleteSong.Click += (s, e) => DeleteSongClicked?.Invoke(this, EventArgs.Empty);
        }

        #region ISongsView Implementation

        /// <summary>
        /// Gets the current text entered in the search textbox.
        /// </summary>
        public string SearchTerm => txtSearch?.Text ?? string.Empty;

        /// <summary>
        /// Gets the currently selected <see cref="Song"/> object stored in the active row's Tag property.
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
        /// Occurs when the user changes the text in the search input box.
        /// </summary>
        public event EventHandler? SearchTextChanged;

        /// <summary>
        /// Occurs when the selected row in the songs DataGridView changes.
        /// </summary>
        public event EventHandler? SongSelectionChanged;

        /// <summary>
        /// Occurs when the user clicks the delete button.
        /// </summary>
        public event EventHandler? DeleteSongClicked;

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
        public void DisplaySongDetails (Song song, string? artistName)
        {
            if (song == null) return;

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

        #endregion
    }
}