using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.DTOs;
using MongoDB_SongManager.Views;

namespace SongManager.Views
{
    /// <summary>
    /// UserControl representing the songlist/setlist management view implementation.
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
            chkMyListsOnly.CheckedChanged += (s, e) => FilterMySonglistsOnlyChanged?.Invoke(this, EventArgs.Empty);
            btnCreateList.Click += (s, e) => CreateSonglistClicked?.Invoke(this, EventArgs.Empty);
            btnDeleteList.Click += (s, e) => DeleteSonglistClicked?.Invoke(this, EventArgs.Empty);

            btnAddSongToList.Click += (s, e) => AddSongToSonglistClicked?.Invoke(this, EventArgs.Empty);
            btnRemoveSongFromList.Click += (s, e) => RemoveSongFromSonglistClicked?.Invoke(this, EventArgs.Empty);
            btnMoveUp.Click += (s, e) => MoveSongUpClicked?.Invoke(this, EventArgs.Empty);
            btnMoveDown.Click += (s, e) => MoveSongDownClicked?.Invoke(this, EventArgs.Empty);

            txtSearchAvailable.TextChanged += (s, e) => AvailableSongsSearchTextChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ISonglistsView Implementation

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

        public SongDisplayDto? SelectedAvailableSong
        {
            get
            {
                if (dgvAvailableSongs.CurrentRow != null && dgvAvailableSongs.CurrentRow.Tag is SongDisplayDto dto)
                {
                    return dto;
                }
                return null;
            }
        }

        public SongDisplayDto? SelectedAssignedSong
        {
            get
            {
                if (dgvSonglistSongs.CurrentRow != null && dgvSonglistSongs.CurrentRow.Tag is SongDisplayDto dto)
                {
                    return dto;
                }
                return null;
            }
        }

        public bool IsFilterMySonglistsOnlyActive => chkMyListsOnly.Checked;

        public string AvailableSongsSearchTerm => txtSearchAvailable.Text.Trim();

        public event EventHandler? SonglistSelectionChanged;
        public event EventHandler? FilterMySonglistsOnlyChanged;
        public event EventHandler? CreateSonglistClicked;
        public event EventHandler? DeleteSonglistClicked;

        public event EventHandler? AddSongToSonglistClicked;
        public event EventHandler? RemoveSongFromSonglistClicked;
        public event EventHandler? MoveSongUpClicked;
        public event EventHandler? MoveSongDownClicked;
        public event EventHandler? AvailableSongsSearchTextChanged;

        public void DisplaySonglists (IEnumerable<Songlist> songlists, string currentUserId)
        {
            lstSetlists.Items.Clear();
            foreach (var songlist in songlists)
            {
                string prefix = songlist.CreatorId == currentUserId ? "👤 " : "🌐 ";
                string displayName = $"{prefix}{songlist.Name}";
                lstSetlists.Items.Add(new ListBoxItemWrapper(displayName, songlist));
            }

            if (lstSetlists.Items.Count > 0 && lstSetlists.SelectedIndex == -1)
            {
                lstSetlists.SelectedIndex = 0;
            }
        }

        public void DisplayAvailableSongs (IEnumerable<SongDisplayDto> songs)
        {
            dgvAvailableSongs.Rows.Clear();
            foreach (var song in songs)
            {
                int index = dgvAvailableSongs.Rows.Add(song.Title, song.ArtistName, song.SongbookInfo);
                dgvAvailableSongs.Rows[index].Tag = song;
            }
        }

        public void DisplayAssignedSongs (IEnumerable<SongDisplayDto> songs)
        {
            dgvSonglistSongs.Rows.Clear();
            int pos = 1;

            foreach (var song in songs)
            {
                int index = dgvSonglistSongs.Rows.Add(pos++, song.Title, song.ArtistName);
                dgvSonglistSongs.Rows[index].Tag = song;
            }

            var activeList = SelectedSonglist;
            lblActiveSetlistTitle.Text = activeList != null ? activeList.Name : "Keine Setlist aktiv";
        }

        public void SetReadOnlyState (bool isReadOnly)
        {
            bool enableEditing = !isReadOnly;

            btnAddSongToList.Enabled = enableEditing;
            btnRemoveSongFromList.Enabled = enableEditing;
            btnMoveUp.Enabled = enableEditing;
            btnMoveDown.Enabled = enableEditing;
            btnDeleteList.Enabled = enableEditing;
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