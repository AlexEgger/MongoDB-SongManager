namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data Transfer Object used for creating or updating a user's song interaction.
    /// </summary>
    public class SaveUserSongInteractionDto
    {
        /// <summary>
        /// Gets or sets the target song identifier.
        /// </summary>
        public string SongId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category ratings to be saved.
        /// </summary>
        public List<RatingDto> Ratings { get; set; } = new List<RatingDto>();

        /// <summary>
        /// Gets or sets the personal text note to be saved.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }
}