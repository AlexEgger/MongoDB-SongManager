using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines data access operations specifically tailored for <see cref="Artist"/> domain entities.
/// </summary>
public interface IArtistRepository : IRepository<Artist>
{
    /// <summary>
    /// Finds an active artist by their exact name using a case-insensitive search.
    /// </summary>
    /// <param name="name">The artist name to search for.</param>
    /// <returns>The matching artist entity if found; otherwise, <c>null</c>.</returns>
    Artist? GetByName (string name);
}