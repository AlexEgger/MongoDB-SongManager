using CsvHelper;
using CsvHelper.Configuration;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.Dtos;
using System.Globalization;
using System.Text;

namespace MongoDB_SongManager.Services;

/// <summary>
/// Provides CSV export and import capabilities for song entries using CsvHelper.
/// </summary>
public class CsvService : ICsvService
{
    private readonly CsvConfiguration _csvConfig;

    /// <summary>
    /// Initializes a new instance of the CsvService class with culture-invariant settings.
    /// </summary>
    public CsvService ()
    {
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";", // Common separator for Excel compatibility
            Encoding = Encoding.UTF8
        };
    }

    public async Task ExportSongsAsync (List<Song> songs, List<Artist> artists, string filePath)
    {
        // Map domain models to flat CSV DTOs
        var artistDictionary = artists.ToDictionary(a => a.Id, a => a.Name);

        var dtos = songs.Select(song => new SongCsvDto
        {
            Title = song.Title,
            ArtistName = !string.IsNullOrEmpty(song.ArtistId) && artistDictionary.TryGetValue(song.ArtistId, out var artistName)
                ? artistName
                : string.Empty,
            ChordsUrl = song.ChordsUrl,
            YoutubeUrl = song.YoutubeUrl,
            Liederbuchnummer = song.Liederbuchnummer,
            Liederbuchseite = song.Liederbuchseite
        }).ToList();

        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
        using var csv = new CsvWriter(writer, _csvConfig);

        await csv.WriteRecordsAsync(dtos);
    }

    public async Task<List<(Song Song, string ArtistName)>> ImportSongsAsync (string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"CSV file not found: {filePath}");
        }

        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using var csv = new CsvReader(reader, _csvConfig);

        var records = csv.GetRecordsAsync<SongCsvDto>();
        var result = new List<(Song Song, string ArtistName)>();

        await foreach (var record in records)
        {
            var song = new Song
            {
                Title = record.Title,
                ChordsUrl = record.ChordsUrl,
                YoutubeUrl = record.YoutubeUrl,
                Liederbuchnummer = record.Liederbuchnummer,
                Liederbuchseite = record.Liederbuchseite,
                IsDeleted = false
            };

            result.Add((song, record.ArtistName));
        }

        return result;
    }
}