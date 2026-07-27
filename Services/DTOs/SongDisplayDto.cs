using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data transfer object tailored for presentation inside the DataGridView and detail view.
    /// </summary>
    public class SongDisplayDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string? ChordsUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string SongbookInfo { get; set; } = string.Empty;
        public uint? Tempo { get; set; }

        /// <summary>
        /// Gets or sets the personal notes or comments left by the user for this song.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the list of category rating entries submitted by the user.
        /// </summary>
        public List<Rating> Ratings { get; set; } = new();
    }
}