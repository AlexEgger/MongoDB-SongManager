using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines database operations for managing songlists.
/// </summary>
public interface ISonglistRepository : IRepository<Songlist>
{
    Task<List<Songlist>> GetSonglistsByCreatorIdAsync (string creatorId);
}
