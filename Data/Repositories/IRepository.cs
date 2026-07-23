using MongoDB_SongManager.Models;
using System.Linq.Expressions;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Generic repository contract defining fundamental synchronous CRUD and query operations 
/// for domain entities implementing <see cref="IEntity"/>.
/// </summary>
/// <typeparam name="T">The entity type derived from <see cref="IEntity"/>.</typeparam>
public interface IRepository<T> where T : class, IEntity
{
    /// <summary>
    /// Retrieves all active (non-deleted) entities from the database collection.
    /// </summary>
    /// <returns>An enumerable collection of active entities.</returns>
    IEnumerable<T> GetAll ();

    /// <summary>
    /// Finds an active entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique document identifier string.</param>
    /// <returns>The matching entity if found and active; otherwise, <c>null</c>.</returns>
    T? GetById (string id);

    /// <summary>
    /// Finds active entities matching the given expression predicate evaluated on the server side.
    /// </summary>
    /// <param name="predicate">The filter expression predicate.</param>
    /// <returns>A collection of matching entities.</returns>
    IEnumerable<T> Find (Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Inserts a new entity document into the database collection.
    /// </summary>
    /// <param name="entity">The entity instance to insert.</param>
    void Insert (T entity);

    /// <summary>
    /// Replaces or updates an existing entity document in the database.
    /// </summary>
    /// <param name="entity">The entity instance containing updated values.</param>
    /// <returns><c>true</c> if the document was successfully updated; otherwise, <c>false</c>.</returns>
    bool Update (T entity);

    /// <summary>
    /// Performs a soft delete on an entity by marking its soft-delete state.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to mark as deleted.</param>
    /// <returns><c>true</c> if the document was successfully updated; otherwise, <c>false</c>.</returns>
    bool Delete (string id);
}