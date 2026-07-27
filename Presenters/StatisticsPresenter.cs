using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Models.Enums;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;

namespace MongoDB_SongManager.Presenters
{
    /// <summary>
    /// Presenter handling aggregation of user and community ratings for visualization on the comparative statistics view.
    /// </summary>
    public class StatisticsPresenter
    {
        private readonly IStatisticsView _view;
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ISonglistRepository _songlistRepository;
        private readonly IUserSongInteractionRepository _userInteractionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDtoService _dtoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsPresenter"/> class.
        /// </summary>
        public StatisticsPresenter (
            IStatisticsView view,
            ISongRepository songRepository,
            IArtistRepository artistRepository,
            ISonglistRepository songlistRepository,
            IUserSongInteractionRepository userInteractionRepository,
            ICurrentUserService currentUserService,
            IDtoService dtoService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _songRepository = songRepository ?? throw new ArgumentNullException(nameof(songRepository));
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
            _songlistRepository = songlistRepository ?? throw new ArgumentNullException(nameof(songlistRepository));
            _userInteractionRepository = userInteractionRepository ?? throw new ArgumentNullException(nameof(userInteractionRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _dtoService = dtoService ?? throw new ArgumentNullException(nameof(dtoService));

            WireUpEvents();
        }

        /// <summary>
        /// Subscribes presenter handlers to view and service events.
        /// </summary>
        private void WireUpEvents ()
        {
            _view.RefreshRequested += (s, e) => LoadStatistics();
            _view.SongSelectionChanged += (s, e) => UpdateRatingComparison();
            _currentUserService.CurrentUserChanged += (s, e) => LoadStatistics();
        }

        /// <summary>
        /// Loads full dataset, populates song selector with rated indicators, and updates summary cards.
        /// </summary>
        public void LoadStatistics ()
        {
            var songs = _songRepository.GetAll().ToList();
            var artists = _artistRepository.GetAll().ToDictionary(a => a.Id, a => a.Name);
            var songlists = _songlistRepository.GetAll().ToList();
            var allInteractions = _userInteractionRepository.GetAll().ToList();

            string currentUserId = _currentUserService.CurrentUser?.Id ?? string.Empty;

            // Identify songs rated by active user
            var ratedSongIds = allInteractions
                .Where(i => i.UserId == currentUserId && i.Ratings != null && i.Ratings.Any(r => r.Value > 0))
                .Select(i => i.SongId)
                .ToHashSet();

            // Populate selector dropdown with Song DTOs and rating indicator
            var songDtos = _dtoService.MapToSongDisplayDtos(songs, artists, new Dictionary<string, UserSongInteraction>());
            _view.PopulateSongSelector(songDtos, ratedSongIds);

            // Update top metric panels
            _view.DisplaySummaryMetrics(songs.Count, songlists.Count, songlists.Count(sl => sl.CreatorId == currentUserId));

            // Render single combined chart
            UpdateRatingComparison();
        }

        /// <summary>
        /// Aggregates personal vs community ratings for the selected song or global averages.
        /// </summary>
        private void UpdateRatingComparison ()
        {
            string? selectedSongId = _view.SelectedSongId;
            string currentUserId = _currentUserService.CurrentUser?.Id ?? string.Empty;

            var allInteractions = _userInteractionRepository.GetAll().ToList();

            Dictionary<string, double> userRatingsMap;
            Dictionary<string, double> communityRatingsMap;
            string chartTitle;

            if (string.IsNullOrEmpty(selectedSongId))
            {
                // Global Averages across all songs
                chartTitle = "Overall Rating Averages (My Ratings vs. Community)";

                var userInteractions = allInteractions.Where(i => i.UserId == currentUserId).ToList();
                userRatingsMap = CalculateAverageRatings(userInteractions);
                communityRatingsMap = CalculateAverageRatings(allInteractions);
            }
            else
            {
                // Single Song Comparison
                var song = _songRepository.GetById(selectedSongId);
                string songTitle = song != null ? song.Title : "Selected Song";

                chartTitle = $"Rating Comparison for '{songTitle}'";

                var userInteraction = allInteractions.FirstOrDefault(i => i.UserId == currentUserId && i.SongId == selectedSongId);
                userRatingsMap = userInteraction != null ? ExtractRatings(userInteraction) : GetEmptyRatingMap();

                var songInteractions = allInteractions.Where(i => i.SongId == selectedSongId).ToList();
                communityRatingsMap = CalculateAverageRatings(songInteractions);
            }

            _view.DisplayRatingComparison(userRatingsMap, communityRatingsMap, chartTitle);
        }

        /// <summary>
        /// Calculates average ratings across multiple interaction records for each rating category.
        /// </summary>
        private Dictionary<string, double> CalculateAverageRatings (List<UserSongInteraction> interactions)
        {
            var categoryMap = GetEmptyRatingMap();
            if (interactions.Count == 0) return categoryMap;

            var categoryTotals = new Dictionary<string, (double Sum, int Count)>();
            foreach (var key in categoryMap.Keys)
            {
                categoryTotals[key] = (0.0, 0);
            }

            foreach (var interaction in interactions)
            {
                if (interaction.Ratings == null) continue;

                foreach (var rating in interaction.Ratings)
                {
                    string categoryName = rating.Category.ToString();
                    if (categoryTotals.ContainsKey(categoryName) && rating.Value > 0)
                    {
                        var current = categoryTotals[categoryName];
                        categoryTotals[categoryName] = (current.Sum + rating.Value, current.Count + 1);
                    }
                }
            }

            foreach (var key in categoryTotals.Keys)
            {
                var (sum, count) = categoryTotals[key];
                categoryMap[key] = count > 0 ? Math.Round(sum / count, 1) : 0.0;
            }

            return categoryMap;
        }

        /// <summary>
        /// Extracts rating values for a single interaction instance.
        /// </summary>
        private Dictionary<string, double> ExtractRatings (UserSongInteraction interaction)
        {
            var map = GetEmptyRatingMap();
            if (interaction.Ratings == null) return map;

            foreach (var rating in interaction.Ratings)
            {
                string categoryName = rating.Category.ToString();
                if (map.ContainsKey(categoryName))
                {
                    map[categoryName] = rating.Value;
                }
            }

            return map;
        }

        /// <summary>
        /// Returns a dictionary with all RatingType enum values pre-initialized to 0.0.
        /// </summary>
        private Dictionary<string, double> GetEmptyRatingMap ()
        {
            var map = new Dictionary<string, double>();
            foreach (RatingType type in Enum.GetValues(typeof(RatingType)))
            {
                map[type.ToString()] = 0.0;
            }
            return map;
        }
    }
}