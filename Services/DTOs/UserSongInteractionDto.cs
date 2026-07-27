namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data Transfer Object representing a user's personal interaction with a specific song,
    /// including custom ratings and notes.
    /// </summary>
    public class UserSongInteractionDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the interaction.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the user who owns this interaction.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the song being rated or commented on.
        /// </summary>
        public string SongId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of category-based ratings given by the user.
        /// </summary>
        public List<RatingDto> Ratings { get; set; } = new List<RatingDto>();

        /// <summary>
        /// Gets or sets the user's personal note or comment regarding the song.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when this interaction was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}