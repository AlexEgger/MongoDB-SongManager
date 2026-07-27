using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Contract defining user interaction and visualization capabilities for comparative rating statistics.
    /// </summary>
    public interface IStatisticsView
    {
        /// <summary>
        /// Occurs when the view requires a full data refresh.
        /// </summary>
        event EventHandler RefreshRequested;

        /// <summary>
        /// Occurs when the user selects a different song from the comparison selector.
        /// </summary>
        event EventHandler SongSelectionChanged;

        /// <summary>
        /// Gets the ID of the currently selected song for comparison, or null if 'All Songs' is selected.
        /// </summary>
        string? SelectedSongId { get; }

        /// <summary>
        /// Updates global summary metrics cards in the header panel.
        /// </summary>
        /// <param name="totalSongs">Total number of songs registered in the application.</param>
        /// <param name="totalSetlists">Total number of setlists available.</param>
        /// <param name="userSetlists">Number of setlists owned by the current active user.</param>
        void DisplaySummaryMetrics (int totalSongs, int totalSetlists, int userSetlists);

        /// <summary>
        /// Populates the song selection ComboBox with songs and indicates whether the active user rated each song.
        /// </summary>
        /// <param name="songs">Collection of song display DTOs.</param>
        /// <param name="ratedSongIds">Set of song IDs that have ratings from the active user.</param>
        void PopulateSongSelector (IEnumerable<SongDisplayDto> songs, HashSet<string> ratedSongIds);

        /// <summary>
        /// Renders combined user and community ratings in a single grouped bar chart.
        /// </summary>
        /// <param name="userRatings">Dictionary mapping rating category names to user scores.</param>
        /// <param name="communityRatings">Dictionary mapping rating category names to community average scores.</param>
        /// <param name="chartTitle">Dynamic title describing the chart scope.</param>
        void DisplayRatingComparison (
            IReadOnlyDictionary<string, double> userRatings,
            IReadOnlyDictionary<string, double> communityRatings,
            string chartTitle);
    }
}