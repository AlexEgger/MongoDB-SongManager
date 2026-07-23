using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Defines data access contracts for Artist domain entities.
/// </summary>
public interface IArtistRepository
{
    /// <summary>
    /// Retrieves all non-deleted artists from the collection.
    /// </summary>
    Task<List<Artist>> GetAllActiveAsync ();

    /// <summary>
    /// Retrieves a specific artist by their unique identifier.
    /// </summary>
    Task<Artist?> GetByIdAsync (string id);

    /// <summary>
    /// Finds an artist by their exact name (case-insensitive).
    /// </summary>
    Task<Artist?> GetByNameAsync (string name);

    /// <summary>
    /// Inserts a new artist into the database.
    /// </summary>
    Task CreateAsync (Artist artist);

    /// <summary>
    /// Updates an existing artist document in the database.
    /// </summary>
    Task UpdateAsync (Artist artist);

    /// <summary>
    /// Performs a soft delete by setting the IsDeleted flag to true.
    /// </summary>
    Task DeleteSoftAsync (string id);
}