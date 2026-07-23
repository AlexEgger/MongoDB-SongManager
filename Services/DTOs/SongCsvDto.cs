using CsvHelper.Configuration.Attributes;

namespace MongoDB_SongManager.Services.Dtos;

/// <summary>
/// Data transfer object used for serializing and deserializing song records in CSV format.
/// </summary>
public class SongCsvDto
{
    [Name("Title")]
    public string Title { get; set; } = string.Empty;

    [Name("Artist")]
    public string ArtistName { get; set; } = string.Empty;

    [Name("ChordsUrl")]
    public string? ChordsUrl { get; set; }

    [Name("YoutubeUrl")]
    public string? YoutubeUrl { get; set; }

    [Name("SongbookNumber")]
    public uint? Liederbuchnummer { get; set; }

    [Name("SongbookPage")]
    public uint? Liederbuchseite { get; set; }
}