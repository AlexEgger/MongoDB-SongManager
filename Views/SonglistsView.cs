using MongoDB_SongManager.Models;
using MongoDB_SongManager.Views;

namespace SongManager.Views
{
    /// <summary>
    /// UserControl representing the songlist management view implementation.
    /// </summary>
    public partial class SonglistsView : UserControl, ISonglistsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SonglistsView"/> class.
        /// </summary>
        public SonglistsView ()
        {
            InitializeComponent();
            WireUpEvents();
        }

        /// <summary>
        /// Subscribes internal UI control events and forwards them to interface subscribers.
        /// </summary>
        private void WireUpEvents ()
        {
            lstSetlists.SelectedIndexChanged += (s, e) => SonglistSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnAddSongToList.Click += (s, e) => AddSongToPlaylistClicked?.Invoke(this, EventArgs.Empty);
            btnRemoveSongFromList.Click += (s, e) => RemoveSongFromPlaylistClicked?.Invoke(this, EventArgs.Empty);
            btnCreateList.Click += (s, e) => CreateSonglistClicked?.Invoke(this, EventArgs.Empty);
            btnDeleteList.Click += (s, e) => DeleteSonglistClicked?.Invoke(this, EventArgs.Empty);
        }

        #region ISonglistsView Implementation

        /// <summary>
        /// Gets the currently selected <see cref="Songlist"/> document.
        /// </summary>
        public Songlist? SelectedSonglist
        {
            get
            {
                if (lstSetlists.SelectedItem is ListBoxItemWrapper wrapper && wrapper.Value is Songlist songlist)
                {
                    return songlist;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the currently selected song from the available library grid.
        /// </summary>
        public Song? SelectedAvailableSong
        {
            get
            {
                if (dgvAvailableSongs.CurrentRow != null && dgvAvailableSongs.CurrentRow.Tag is Song song)
                {
                    return song;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the currently selected song from the playlist grid.
        /// </summary>
        public Song? SelectedPlaylistSong
        {
            get
            {
                if (dgvSonglistSongs.CurrentRow != null && dgvSonglistSongs.CurrentRow.Tag is Song song)
                {
                    return song;
                }
                return null;
            }
        }

        /// <summary>
        /// Occurs when the selected songlist changes in the ListBox.
        /// </summary>
        public event EventHandler? SonglistSelectionChanged;

        /// <summary>
        /// Occurs when the user clicks the button to add a song to the active playlist.
        /// </summary>
        public event EventHandler? AddSongToPlaylistClicked;

        /// <summary>
        /// Occurs when the user clicks the button to remove a song from the active playlist.
        /// </summary>
        public event EventHandler? RemoveSongFromPlaylistClicked;

        /// <summary>
        /// Occurs when the user requests to create a new songlist.
        /// </summary>
        public event EventHandler? CreateSonglistClicked;

        /// <summary>
        /// Occurs when the user requests to delete the active songlist.
        /// </summary>
        public event EventHandler? DeleteSonglistClicked;

        /// <summary>
        /// Binds the collection of available songlists to the ListBox.
        /// </summary>
        /// <param name="songlists">The collection of songlist documents.</param>
        public void DisplaySonglists (IEnumerable<Songlist> songlists)
        {
            lstSetlists.Items.Clear();
            foreach (var songlist in songlists)
            {
                lstSetlists.Items.Add(new ListBoxItemWrapper(songlist.Name, songlist));
            }
        }

        /// <summary>
        /// Binds available songs to the left DataGridView.
        /// </summary>
        /// <param name="songs">The collection of available song entities.</param>
        /// <param name="artistNames">A dictionary mapping Artist IDs to Artist Names.</param>
        public void DisplayAvailableSongs (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames)
        {
            dgvAvailableSongs.Rows.Clear();
            foreach (var song in songs)
            {
                string artistName = "Unbekannt";
                if (!string.IsNullOrEmpty(song.ArtistId) && artistNames.TryGetValue(song.ArtistId, out var resolved))
                {
                    artistName = resolved;
                }

                int index = dgvAvailableSongs.Rows.Add(song.Title, artistName);
                dgvAvailableSongs.Rows[index].Tag = song;
            }
        }

        /// <summary>
        /// Binds songs contained in the selected songlist to the right DataGridView.
        /// </summary>
        /// <param name="songs">The collection of assigned song entities.</param>
        /// <param name="artistNames">A dictionary mapping Artist IDs to Artist Names.</param>
        public void DisplayPlaylistSongs (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames)
        {
            dgvSonglistSongs.Rows.Clear();
            int pos = 1;
            foreach (var song in songs)
            {
                string artistName = "Unbekannt";
                if (!string.IsNullOrEmpty(song.ArtistId) && artistNames.TryGetValue(song.ArtistId, out var resolved))
                {
                    artistName = resolved;
                }

                int index = dgvSonglistSongs.Rows.Add(pos++, song.Title, artistName);
                dgvSonglistSongs.Rows[index].Tag = song;
            }
        }

        #endregion

        /// <summary>
        /// Helper wrapper class for binding objects to ListBox controls.
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