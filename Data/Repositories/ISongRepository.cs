using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines data access contracts for Song domain entities.
/// </summary>
public interface ISongRepository
{
    /// <summary>
    /// Retrieves all non-deleted songs from the collection.
    /// </summary>
    Task<List<Song>> GetAllActiveAsync ();

    /// <summary>
    /// Retrieves a specific song by its unique identifier.
    /// </summary>
    Task<Song?> GetByIdAsync (string id);

    /// <summary>
    /// Retrieves all active songs associated with a specific artist ID.
    /// </summary>
    Task<List<Song>> GetByArtistIdAsync (string artistId);

    /// <summary>
    /// Performs a case-insensitive search for songs by title.
    /// </summary>
    Task<List<Song>> SearchByTitleAsync (string titleQuery);

    /// <summary>
    /// Inserts a new song into the database.
    /// </summary>
    Task CreateAsync (Song song);

    /// <summary>
    /// Updates an existing song document in the database.
    /// </summary>
    Task UpdateAsync (Song song);

    /// <summary>
    /// Performs a soft delete by setting the IsDeleted flag to true.
    /// </summary>
    Task DeleteSoftAsync (string id);
}