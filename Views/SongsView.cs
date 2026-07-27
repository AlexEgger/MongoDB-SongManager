using MongoDB_SongManager.Services.DTOs;
using MongoDB_SongManager.Views;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace SongManager.Views
{
    /// <summary>
    /// UserControl representing the main song management view implementation using DTO objects,
    /// complete with dynamic rating columns, custom sorting, formatting, web link launchers, and user-isolated interaction controls.
    /// </summary>
    public partial class SongsView : UserControl, ISongsView
    {
        /// <summary>
        /// Prefix used for dynamically generated DataGridView rating column names to distinguish them from static columns.
        /// </summary>
        private const string RatingColPrefix = "colRating_";

        /// <summary>
        /// Context menu strip for managing playlist items via right-click actions.
        /// </summary>
        private ContextMenuStrip _songlistContextMenu = null!;

        /// <summary>
        /// Button control allowing users to edit ratings and notes for the selected song.
        /// </summary>
        private Button _btnEditInteraction = null!;

        /// <summary>
        /// Stores the currently displayed user interaction DTO for reference.
        /// </summary>
        private UserSongInteractionDto? _currentDisplayedInteraction;

        /// <summary>
        /// Initializes a new instance of the <see cref="SongsView"/> class.
        /// </summary>
        public SongsView ()
        {
            InitializeComponent();
            InitializeAdditionalControls();
            InitializeSonglistContextMenu();
            ConfigureDataGridViewForRatings();
            WireUpEvents();
        }

        /// <summary>
        /// Configures custom sorting, header click, and cell formatting handlers for dynamic rating columns in the DataGridView.
        /// </summary>
        private void ConfigureDataGridViewForRatings ()
        {
            dgvSongs.SortCompare += DgvSongs_SortCompare;
            dgvSongs.CellFormatting += DgvSongs_CellFormatting;
            dgvSongs.ColumnHeaderMouseClick += DgvSongs_ColumnHeaderMouseClick;
        }

        /// <summary>
        /// Adds extra runtime controls, such as the button to edit user interactions inside the details pane.
        /// </summary>
        private void InitializeAdditionalControls ()
        {
            _btnEditInteraction = new Button
            {
                Text = "⭐ Rate / Comment Song",
                Dock = DockStyle.Bottom,
                Height = 30,
                UseVisualStyleBackColor = true
            };

            grpSongDetails.Controls.Add(_btnEditInteraction);
            grpSongDetails.Controls.SetChildIndex(_btnEditInteraction, 2);
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

            lstSongLists.ContextMenuStrip = _songlistContextMenu;

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
            txtSearch.TextChanged += (s, e) => SearchTextChanged?.Invoke(this, EventArgs.Empty);

            dgvSongs.SelectionChanged += (s, e) => SongSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnAddSong.Click += (s, e) => AddSongClicked?.Invoke(this, EventArgs.Empty);
            btnEditSong.Click += (s, e) => EditSongClicked?.Invoke(this, EventArgs.Empty);
            btnDeleteSong.Click += (s, e) => DeleteSongClicked?.Invoke(this, EventArgs.Empty);

            lstSongLists.SelectedIndexChanged += (s, e) => SonglistSelectionChanged?.Invoke(this, EventArgs.Empty);
            btnCreateList.Click += (s, e) => CreateSonglistClicked?.Invoke(this, EventArgs.Empty);
            btnAddArtist.Click += (s, e) => AddArtistClicked?.Invoke(this, EventArgs.Empty);

            btnExportCsv.Click += (s, e) => ExportCsvClicked?.Invoke(this, EventArgs.Empty);
            btnImportCsv.Click += (s, e) => ImportCsvClicked?.Invoke(this, EventArgs.Empty);

            _btnEditInteraction.Click += (s, e) =>
            {
                if (SelectedSong != null)
                {
                    SaveInteractionClicked?.Invoke(this, EventArgs.Empty);
                }
            };

            lnkChords.LinkClicked += LnkChords_LinkClicked;
            lnkYoutube.LinkClicked += LnkYoutube_LinkClicked;
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

        #endregion

        #region Song & Interaction Events

        /// <summary>
        /// Occurs when the search term input changes.
        /// </summary>
        public event EventHandler? SearchTextChanged;

        /// <summary>
        /// Occurs when the active song selection in the DataGridView changes.
        /// </summary>
        public event EventHandler? SongSelectionChanged;

        /// <summary>
        /// Occurs when the add song button is clicked.
        /// </summary>
        public event EventHandler? AddSongClicked;

        /// <summary>
        /// Occurs when the edit song button is clicked.
        /// </summary>
        public event EventHandler? EditSongClicked;

        /// <summary>
        /// Occurs when the delete song button is clicked.
        /// </summary>
        public event EventHandler? DeleteSongClicked;

        /// <summary>
        /// Occurs when the add artist button is clicked.
        /// </summary>
        public event EventHandler? AddArtistClicked;

        /// <summary>
        /// Occurs when saving a user song interaction is triggered.
        /// </summary>
        public event EventHandler? SaveInteractionClicked;

        #endregion

        #region Songlist Events

        /// <summary>
        /// Occurs when the selected songlist/playlist changes in the sidebar.
        /// </summary>
        public event EventHandler? SonglistSelectionChanged;

        /// <summary>
        /// Occurs when the create new songlist button is clicked.
        /// </summary>
        public event EventHandler? CreateSonglistClicked;

        /// <summary>
        /// Occurs when the rename songlist context menu item is clicked.
        /// </summary>
        public event EventHandler? RenameSonglistClicked;

        /// <summary>
        /// Occurs when deleting a songlist is requested.
        /// </summary>
        public event EventHandler? DeleteSonglistClicked;

        /// <summary>
        /// Occurs when adding a song to a songlist is requested.
        /// </summary>
        public event EventHandler? AddSongToSonglistClicked;

        /// <summary>
        /// Occurs when removing a song from a songlist is requested.
        /// </summary>
        public event EventHandler? RemoveSongFromSonglistClicked;

        #endregion

        #region CSV Events

        /// <summary>
        /// Occurs when the export CSV button is clicked.
        /// </summary>
        public event EventHandler? ExportCsvClicked;

        /// <summary>
        /// Occurs when the import CSV button is clicked.
        /// </summary>
        public event EventHandler? ImportCsvClicked;

        #endregion

        #region Display Methods & Input Retrieval

        /// <summary>
        /// Displays the collection of song DTOs in the DataGridView and dynamically synchronizes category rating columns.
        /// </summary>
        /// <param name="songs">The collection of song DTOs tailored for presentation.</param>
        public void DisplaySongs (IEnumerable<SongDisplayDto> songs)
        {
            var songList = songs.ToList();

            var activeCategories = songList
                .SelectMany(s => s.Ratings)
                .Select(r => r.Category.ToString())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            SyncRatingColumns(activeCategories);

            dgvSongs.Rows.Clear();

            foreach (var song in songList)
            {
                int rowIndex = dgvSongs.Rows.Add();
                var row = dgvSongs.Rows[rowIndex];

                row.Cells[colTitle.Index].Value = song.Title;
                row.Cells[colArtist.Index].Value = song.ArtistName;

                foreach (var category in activeCategories)
                {
                    string colName = RatingColPrefix + category;

                    var ratingEntry = song.Ratings.FirstOrDefault(r => r.Category.ToString() == category);

                    // Store pure numerical value or null for clean numeric sorting
                    row.Cells[colName].Value = ratingEntry?.Value;
                }

                row.Tag = song;
            }
        }

        /// <summary>
        /// Synchronizes DataGridView columns by generating narrow category rating columns with emojis and tooltips.
        /// </summary>
        /// <param name="activeCategories">List of active rating category names present in the song collection.</param>
        private void SyncRatingColumns (List<string> activeCategories)
        {
            var activeColumnNames = activeCategories.Select(c => RatingColPrefix + c).ToHashSet();

            var columnsToRemove = dgvSongs.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Name.StartsWith(RatingColPrefix) && !activeColumnNames.Contains(c.Name))
                .ToList();

            foreach (var col in columnsToRemove)
            {
                dgvSongs.Columns.Remove(col);
            }

            foreach (var category in activeCategories)
            {
                string colName = RatingColPrefix + category;

                if (!dgvSongs.Columns.Contains(colName))
                {
                    string emoji = GetCategoryEmoji(category);

                    var ratingColumn = new DataGridViewTextBoxColumn
                    {
                        Name = colName,
                        HeaderText = emoji,
                        ToolTipText = category,
                        ValueType = typeof(double),
                        SortMode = DataGridViewColumnSortMode.Programmatic,
                        Width = 45,
                        Resizable = DataGridViewTriState.False,
                        DefaultCellStyle = new DataGridViewCellStyle
                        {
                            Alignment = DataGridViewContentAlignment.MiddleCenter
                        }
                    };

                    dgvSongs.Columns.Add(ratingColumn);
                }
            }
        }

        /// <summary>
        /// Maps a category name string to a representative emoji icon.
        /// </summary>
        /// <param name="category">The name of the rating category.</param>
        /// <returns>An emoji string representing the category.</returns>
        private static string GetCategoryEmoji (string category)
        {
            return category.ToLowerInvariant() switch
            {
                var c when c.Contains("vocal") || c.Contains("gesang") || c.Contains("singing") => "🎤",
                var c when c.Contains("guitar") || c.Contains("gitarre") => "🎸",
                var c when c.Contains("drum") || c.Contains("schlagzeug") => "🥁",
                var c when c.Contains("piano") || c.Contains("key") || c.Contains("klavier") => "🎹",
                var c when c.Contains("bass") => "🎸",
                var c when c.Contains("overall") || c.Contains("total") || c.Contains("gesamt") => "⭐",
                var c when c.Contains("diff") || c.Contains("schwierig") => "🔥",
                _ => "🏷️"
            };
        }

        /// <summary>
        /// Handles header clicks on programmatic rating columns to trigger ascending or descending sort execution.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event args containing column index information.</param>
        private void DgvSongs_ColumnHeaderMouseClick (object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            var column = dgvSongs.Columns[e.ColumnIndex];

            if (column.Name.StartsWith(RatingColPrefix))
            {
                ListSortDirection direction = ListSortDirection.Ascending;

                if (dgvSongs.SortedColumn == column && dgvSongs.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }

                dgvSongs.Sort(column, direction);
            }
        }

        /// <summary>
        /// Custom sort comparison handler ensuring numeric rating values sort correctly,
        /// while unrated songs (null values) always stay at the bottom regardless of sort direction.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event args containing cell values and custom sort result.</param>
        private void DgvSongs_SortCompare (object? sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name.StartsWith(RatingColPrefix))
            {
                double? val1 = e.CellValue1 != null && e.CellValue1 != DBNull.Value ? Convert.ToDouble(e.CellValue1) : null;
                double? val2 = e.CellValue2 != null && e.CellValue2 != DBNull.Value ? Convert.ToDouble(e.CellValue2) : null;

                // Determine if current sort direction is descending
                bool isDescending = dgvSongs.SortOrder == SortOrder.Descending;

                if (!val1.HasValue && !val2.HasValue)
                {
                    e.SortResult = 0;
                }
                else if (!val1.HasValue)
                {
                    // Null always goes to the bottom: 
                    // In Ascending, val1 > val2 pushes val1 down (Result = 1).
                    // In Descending, WinForms flips the result, so we pass -1 to ensure it stays down.
                    e.SortResult = isDescending ? -1 : 1;
                }
                else if (!val2.HasValue)
                {
                    // Null always goes to the bottom
                    e.SortResult = isDescending ? 1 : -1;
                }
                else
                {
                    // Standard numerical comparison for cells with values
                    e.SortResult = val1.Value.CompareTo(val2.Value);
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Custom cell formatting handler displaying compact rating numbers and providing cell tooltips.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event args containing formatting configuration and value.</param>
        private void DgvSongs_CellFormatting (object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvSongs.Columns[e.ColumnIndex].Name.StartsWith(RatingColPrefix))
            {
                string categoryName = dgvSongs.Columns[e.ColumnIndex].ToolTipText;

                if (e.Value != null && e.Value != DBNull.Value && double.TryParse(e.Value.ToString(), out double val))
                {
                    e.Value = $"{val:0.#}";
                    dgvSongs.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{categoryName}: {val:0.#}";
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "-";
                    dgvSongs.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{categoryName}: Not rated";
                    e.FormattingApplied = true;
                }
            }
        }

        /// <summary>
        /// Handles click events on the chords link label and opens the URL in the default browser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Link label click event arguments.</param>
        private void LnkChords_LinkClicked (object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (SelectedSong != null && !string.IsNullOrWhiteSpace(SelectedSong.ChordsUrl))
            {
                OpenUrlInBrowser(SelectedSong.ChordsUrl);
            }
        }

        /// <summary>
        /// Handles click events on the YouTube link label and opens the URL in the default browser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Link label click event arguments.</param>
        private void LnkYoutube_LinkClicked (object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (SelectedSong != null && !string.IsNullOrWhiteSpace(SelectedSong.YoutubeUrl))
            {
                OpenUrlInBrowser(SelectedSong.YoutubeUrl);
            }
        }

        /// <summary>
        /// Helper method to open a given web URL in the system's default browser.
        /// </summary>
        /// <param name="url">The web URL to open.</param>
        private static void OpenUrlInBrowser (string url)
        {
            try
            {
                if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    url = "https://" + url;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                lnkChords.Enabled = false;

                lnkYoutube.Text = "No link";
                lnkYoutube.Enabled = false;

                txtNotes.Text = string.Empty;
                return;
            }

            lblSongTitle.Text = $"Title: {song.Title}";
            lblArtist.Text = $"Artist: {song.ArtistName}";
            lblTempo.Text = song.Tempo.HasValue ? $"Tempo: {song.Tempo} BPM" : "Tempo: -";
            lblBookInfo.Text = $"Songbook: {song.SongbookInfo}";

            bool hasChords = !string.IsNullOrWhiteSpace(song.ChordsUrl);
            lnkChords.Text = hasChords ? "🎸 Open Chords" : "No chords link";
            lnkChords.Enabled = hasChords;

            bool hasYoutube = !string.IsNullOrWhiteSpace(song.YoutubeUrl);
            lnkYoutube.Text = hasYoutube ? "▶️ YouTube Video" : "No YouTube link";
            lnkYoutube.Enabled = hasYoutube;
        }

        /// <summary>
        /// Displays the active user's personal interaction (ratings and notes) in the details panel.
        /// </summary>
        /// <param name="interaction">The user interaction DTO containing personal ratings and comments.</param>
        public void DisplayUserInteraction (UserSongInteractionDto? interaction)
        {
            _currentDisplayedInteraction = interaction;

            if (interaction == null || (interaction.Ratings.Count == 0 && string.IsNullOrWhiteSpace(interaction.Notes)))
            {
                lblRatings.Text = "My Ratings:\n- None -";
                txtNotes.Text = "No personal notes available.";
                return;
            }

            if (interaction.Ratings != null && interaction.Ratings.Count > 0)
            {
                var formattedRatings = string.Join(", ", interaction.Ratings.Select(r => $"{r.Category}: {r.Value}"));
                lblRatings.Text = $"My Ratings:\n{formattedRatings}";
            }
            else
            {
                lblRatings.Text = "My Ratings:\n- None -";
            }

            txtNotes.Text = string.IsNullOrWhiteSpace(interaction.Notes) ? "No personal notes available." : interaction.Notes;
        }

        /// <summary>
        /// Binds the collection of available song lists DTOs to the sidebar list box.
        /// </summary>
        /// <param name="songlists">Collection of songlist DTOs to display.</param>
        /// <param name="currentUserId">Identifier of the logged-in user to prioritize personal playlists.</param>
        public void DisplaySonglists (IEnumerable<SonglistDto> songlists, string currentUserId)
        {
            lstSongLists.Items.Clear();

            lstSongLists.Items.Add(new ListBoxItemWrapper("🎵 All Songs", null!));

            var sortedSonglists = songlists
                                    .OrderByDescending(s => s.CreatorId == currentUserId)
                                    .ThenBy(s => s.Name);

            foreach (var songlist in sortedSonglists)
            {
                string prefix = songlist.CreatorId == currentUserId ? "👤 " : "🌐 ";
                string displayName = $"{prefix}{songlist.Name}";

                lstSongLists.Items.Add(new ListBoxItemWrapper(displayName, songlist));
            }
        }

        /// <summary>
        /// Opens the interaction dialog and returns the user input wrapper.
        /// </summary>
        /// <returns>The populated <see cref="SaveUserSongInteractionDto"/> or null if cancelled.</returns>
        public SaveUserSongInteractionDto? GetUserInteractionInput ()
        {
            var selectedSong = SelectedSong;
            if (selectedSong == null) return null;

            using var dialog = new UserSongInteractionDialog(selectedSong.Title, _currentDisplayedInteraction);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.InteractionDto;
            }

            return null;
        }

        #endregion

        #endregion

        /// <summary>
        /// Helper wrapper class for displaying songlists inside a standard ListBox control.
        /// </summary>
        private class ListBoxItemWrapper
        {
            /// <summary>
            /// Gets the display text shown in the UI.
            /// </summary>
            public string DisplayName { get; }

            /// <summary>
            /// Gets the attached payload object.
            /// </summary>
            public object Value { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ListBoxItemWrapper"/> class.
            /// </summary>
            /// <param name="displayName">Display string for the list item.</param>
            /// <param name="value">Underlying data object.</param>
            public ListBoxItemWrapper (string displayName, object value)
            {
                DisplayName = displayName;
                Value = value;
            }

            /// <summary>
            /// Returns the display string representation of the item.
            /// </summary>
            /// <returns>The display name.</returns>
            public override string ToString () => DisplayName;
        }
    }
}