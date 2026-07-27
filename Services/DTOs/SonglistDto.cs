namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data transfer object representing setlist/playlist details for views.
    /// </summary>
    public class SonglistDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public List<string> SongIds { get; set; } = new();
    }
}
