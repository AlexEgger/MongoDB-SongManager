namespace MongoDB_SongManager.Services.DTOs;

/// <summary>
/// Data transfer object tailored for presentation inside the DataGridView.
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
}