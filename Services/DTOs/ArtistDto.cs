namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data transfer object representing artist information for UI binding.
    /// </summary>
    public class ArtistDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the artist.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name of the artist.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}