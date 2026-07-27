using MongoDB_SongManager.Services.DTOs;
using MongoDB_SongManager.Views;
using ScottPlot;

namespace SongManager.Views
{
    /// <summary>
    /// UserControl implementation for the statistical evaluation dashboard using a combined ScottPlot 5 grouped bar chart.
    /// </summary>
    public partial class StatisticsView : UserControl, IStatisticsView
    {
        private readonly List<SongComboItem> _allSongItems = new();
        private bool _isUpdatingSelector;

        /// <summary>
        /// Occurs when the view requires a data refresh.
        /// </summary>
        public event EventHandler? RefreshRequested;

        /// <summary>
        /// Occurs when the user selects a different song from the dropdown.
        /// </summary>
        public event EventHandler? SongSelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsView"/> class.
        /// </summary>
        public StatisticsView ()
        {
            InitializeComponent();

            cmbSongSelector.SelectedIndexChanged += (s, e) =>
            {
                if (!_isUpdatingSelector)
                {
                    SongSelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            };

            txtSearch.TextChanged += (s, e) => ApplySearchFilter();

            Load += (s, e) => RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        #region IStatisticsView Implementation

        /// <summary>
        /// Gets the ID of the currently selected song in the ComboBox wrapper, or null if 'All Songs' is chosen.
        /// </summary>
        public string? SelectedSongId
        {
            get
            {
                if (cmbSongSelector.SelectedItem is SongComboItem item)
                {
                    return item.SongId;
                }
                return null;
            }
        }

        /// <summary>
        /// Displays top-level metadata totals in header card labels.
        /// </summary>
        public void DisplaySummaryMetrics (int totalSongs, int totalSetlists, int userSetlists)
        {
            lblTotalSongs.Text = $"🎵 Total Songs: {totalSongs}";
            lblTotalSetlists.Text = $"📋 Total Setlists: {totalSetlists}";
            lblUserSetlists.Text = $"👤 My Setlists: {userSetlists}";
        }

        /// <summary>
        /// Populates the dropdown list with available songs, prioritizing user-rated songs first, followed by alphabetical order.
        /// </summary>
        public void PopulateSongSelector (IEnumerable<SongDisplayDto> songs, HashSet<string> ratedSongIds)
        {
            _allSongItems.Clear();

            // Default Option: All songs average
            _allSongItems.Add(new SongComboItem("📊 All Songs (Global Average)", null, string.Empty, string.Empty));

            // Sort: Rated songs first (OrderByDescending on boolean isRated), then alphabetically by Title
            var sortedSongs = songs
                .OrderByDescending(song => ratedSongIds.Contains(song.Id))
                .ThenBy(song => song.Title);

            foreach (var song in sortedSongs)
            {
                bool isRated = ratedSongIds.Contains(song.Id);
                string indicator = isRated ? "⭐ [Rated] " : "🎵 ";
                string displayText = $"{indicator}{song.Title} - {song.ArtistName}";

                _allSongItems.Add(new SongComboItem(displayText, song.Id, song.Title, song.ArtistName));
            }

            ApplySearchFilter();
        }

        /// <summary>
        /// Renders personal ratings and community average ratings together in a single grouped bar chart.
        /// </summary>
        public void DisplayRatingComparison (
            IReadOnlyDictionary<string, double> userRatings,
            IReadOnlyDictionary<string, double> communityRatings,
            string chartTitle)
        {
            formsPlotRatings.Plot.Clear();
            formsPlotRatings.Plot.Title(chartTitle);

            if (userRatings == null || userRatings.Count == 0)
            {
                formsPlotRatings.Refresh();
                return;
            }

            var categories = userRatings.Keys.ToArray();
            double[] userValues = categories.Select(c => userRatings[c]).ToArray();
            double[] communityValues = categories.Select(c => communityRatings.TryGetValue(c, out var v) ? v : 0.0).ToArray();

            List<Bar> userBars = new();
            List<Bar> communityBars = new();

            // Build individual ScottPlot Bar structures with positioning and styling
            for (int i = 0; i < categories.Length; i++)
            {
                userBars.Add(new Bar
                {
                    Position = i - 0.18,
                    Value = userValues[i],
                    Size = 0.32,
                    FillColor = ScottPlot.Color.FromHex("#1F77B4"),
                    Label = userValues[i] > 0 ? userValues[i].ToString("0.0") : "-"
                });

                communityBars.Add(new Bar
                {
                    Position = i + 0.18,
                    Value = communityValues[i],
                    Size = 0.32,
                    FillColor = ScottPlot.Color.FromHex("#FF7F0E"),
                    Label = communityValues[i] > 0 ? communityValues[i].ToString("0.0") : "-"
                });
            }

            // Add grouped bar plots to the chart
            var userPlot = formsPlotRatings.Plot.Add.Bars(userBars);
            userPlot.LegendText = "My Rating";

            var commPlot = formsPlotRatings.Plot.Add.Bars(communityBars);
            commPlot.LegendText = "Community Average";

            // Configure X-Axis with category labels
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            for (int i = 0; i < categories.Length; i++)
            {
                tickGen.AddMajor(i, categories[i]);
            }

            formsPlotRatings.Plot.Axes.Bottom.TickGenerator = tickGen;
            formsPlotRatings.Plot.Axes.SetLimitsY(0, 5.5); // Fixed rating scale (0 to 5)

            // Configure chart Legend
            formsPlotRatings.Plot.Legend.IsVisible = true;
            formsPlotRatings.Plot.Legend.Alignment = ScottPlot.Alignment.UpperRight;

            formsPlotRatings.Refresh();
        }

        #endregion

        /// <summary>
        /// Filters song items in the ComboBox based on search text matching Title or Artist, maintaining initial sorting.
        /// </summary>
        private void ApplySearchFilter ()
        {
            _isUpdatingSelector = true;

            string? previousSelectedId = SelectedSongId;
            string query = txtSearch.Text.Trim().ToLowerInvariant();

            cmbSongSelector.Items.Clear();

            var filteredItems = _allSongItems.Where(item =>
                string.IsNullOrEmpty(item.SongId) || // Always keep "All Songs" option
                string.IsNullOrEmpty(query) ||
                item.Title.ToLowerInvariant().Contains(query) ||
                item.Artist.ToLowerInvariant().Contains(query)).ToList();

            foreach (var item in filteredItems)
            {
                cmbSongSelector.Items.Add(item);
            }

            // Restore previously selected song if present in filtered list, otherwise select first item
            int restoreIndex = 0;
            if (!string.IsNullOrEmpty(previousSelectedId))
            {
                for (int i = 0; i < cmbSongSelector.Items.Count; i++)
                {
                    if (cmbSongSelector.Items[i] is SongComboItem item && item.SongId == previousSelectedId)
                    {
                        restoreIndex = i;
                        break;
                    }
                }
            }

            if (cmbSongSelector.Items.Count > 0)
            {
                cmbSongSelector.SelectedIndex = restoreIndex;
            }

            _isUpdatingSelector = false;

            // Trigger chart refresh if selection was adjusted by filter
            SongSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// ComboBox item wrapper class for storing song ID, title, and artist metadata along with display text.
        /// </summary>
        private class SongComboItem
        {
            public string DisplayText { get; }
            public string? SongId { get; }
            public string Title { get; }
            public string Artist { get; }

            public SongComboItem (string displayText, string? songId, string title, string artist)
            {
                DisplayText = displayText;
                SongId = songId;
                Title = title;
                Artist = artist;
            }

            public override string ToString () => DisplayText;
        }
    }
}