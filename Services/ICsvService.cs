using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Services;

/// <summary>
/// Defines file operation contracts for importing and exporting song data in CSV format.
/// </summary>
public interface ICsvService
{
    /// <summary>
    /// Exports a list of songs along with their artists to a specified CSV file path.
    /// </summary>
    /// <param name="songs">The list of songs to export.</param>
    /// <param name="artists">The list of available artists for resolving names.</param>
    /// <param name="filePath">The target file system path.</param>
    Task ExportSongsAsync (List<Song> songs, List<Artist> artists, string filePath);

    /// <summary>
    /// Imports songs from a CSV file.
    /// </summary>
    /// <param name="filePath">The source CSV file path.</param>
    /// <returns>A list of parsed song models and associated artist names.</returns>
    Task<List<(Song Song, string ArtistName)>> ImportSongsAsync (string filePath);
}