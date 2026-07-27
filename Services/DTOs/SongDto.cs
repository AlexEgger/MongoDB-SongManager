namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data transfer object for creating and editing song entries in dialog views.
    /// </summary>
    public class SongDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? ArtistId { get; set; }
        public uint? Tempo { get; set; }
        public string? ChordsUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public uint? Liederbuchnummer { get; set; }
        public uint? Liederbuchseite { get; set; }
    }
}
