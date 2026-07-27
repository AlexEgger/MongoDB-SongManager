namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data transfer object representing artist information for UI binding.
    /// </summary>
    public class ArtistDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
