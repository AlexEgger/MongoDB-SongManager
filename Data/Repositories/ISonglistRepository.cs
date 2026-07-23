using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines data access operations specifically tailored for <see cref="Songlist"/> domain entities.
/// </summary>
public interface ISonglistRepository : IRepository<Songlist>
{
    /// <summary>
    /// Retrieves all songlists created by a specific user.
    /// </summary>
    /// <param name="creatorId">The unique identifier of the user who created the list.</param>
    /// <returns>A collection of matching songlists.</returns>
    IEnumerable<Songlist> GetSonglistsByCreatorId (string creatorId);
}