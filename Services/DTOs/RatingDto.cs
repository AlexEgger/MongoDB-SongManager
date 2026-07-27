namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Represents a specific rating category and its numeric value.
    /// </summary>
    public class RatingDto
    {
        /// <summary>
        /// Gets or sets the rating category (e.g., "Difficulty", "VocalRange").
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating value, typically on a scale from 1 to 5.
        /// </summary>
        public int Value { get; set; }
    }
}