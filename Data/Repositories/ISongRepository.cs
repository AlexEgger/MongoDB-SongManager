using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines data access operations specifically tailored for <see cref="Song"/> domain entities.
/// </summary>
public interface ISongRepository : IRepository<Song>
{
    /// <summary>
    /// Retrieves all active songs associated with a specific artist ID.
    /// </summary>
    /// <param name="artistId">The unique identifier of the artist.</param>
    /// <returns>A collection of matching song entities.</returns>
    IEnumerable<Song> GetByArtistId (string artistId);

    /// <summary>
    /// Performs a case-insensitive search for songs matching the specified title query string.
    /// </summary>
    /// <param name="titleQuery">The partial or full song title to search for.</param>
    /// <returns>A collection of matching song entities.</returns>
    IEnumerable<Song> SearchByTitle (string titleQuery);
}