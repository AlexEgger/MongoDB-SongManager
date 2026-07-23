using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services;

/// <summary>
/// Service responsible for transforming domain models into DTOs tailored for UI presentation.
/// </summary>
public class DtoService : IDtoService
{
    /// <inheritdoc />
    public SongDisplayDto MapToSongDisplayDto (Song song, IReadOnlyDictionary<string, string> artistNames)
    {
        ArgumentNullException.ThrowIfNull(song);

        string artistName = "Unknown";
        if (!string.IsNullOrEmpty(song.ArtistId) && artistNames != null && artistNames.TryGetValue(song.ArtistId, out var resolvedName))
        {
            artistName = resolvedName;
        }

        string bookInfo = "-";
        if (song.Liederbuchnummer.HasValue || song.Liederbuchseite.HasValue)
        {
            bookInfo = $"Book #{song.Liederbuchnummer?.ToString() ?? "-"} / Page {song.Liederbuchseite?.ToString() ?? "-"}";
        }

        return new SongDisplayDto
        {
            Id = song.Id ?? string.Empty,
            Title = song.Title ?? string.Empty,
            ArtistName = artistName,
            ChordsUrl = song.ChordsUrl,
            YoutubeUrl = song.YoutubeUrl,
            SongbookInfo = bookInfo
        };
    }

    /// <inheritdoc />
    public IEnumerable<SongDisplayDto> MapToSongDisplayDtos (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames)
    {
        if (songs == null) return Enumerable.Empty<SongDisplayDto>();

        return songs.Select(song => MapToSongDisplayDto(song, artistNames)).ToList();
    }
}