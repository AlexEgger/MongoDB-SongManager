using MongoDB_SongManager.Models;
using System.Linq.Expressions;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Generic repository contract for performing CRUD and query operations on MongoDB documents implementing <see cref="IEntity"/>.
/// </summary>
/// <typeparam name="T">The entity type extending <see cref="IEntity"/>.</typeparam>
public interface IRepository<T> where T : class, IEntity
{
    /// <summary>
    /// Retrieves all active (non-deleted) entities.
    /// </summary>
    /// <returns>An enumerable collection of active entities.</returns>
    IEnumerable<T> GetAll ();

    /// <summary>
    /// Finds an active entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique document ID string.</param>
    /// <returns>The matching entity if found and active; otherwise, null.</returns>
    T? GetById (string id);

    /// <summary>
    /// Inserts a new entity document into the database.
    /// </summary>
    /// <param name="entity">The entity instance to insert.</param>
    void Insert (T entity);

    /// <summary>
    /// Replaces/updates an existing entity document in the database.
    /// </summary>
    /// <param name="entity">The entity instance containing updated values.</param>
    /// <returns>True if the document was successfully updated; otherwise, false.</returns>
    bool Update (T entity);

    /// <summary>
    /// Soft-deletes an entity by updating its <see cref="IEntity.IsDeleted"/> property to true.
    /// </summary>
    /// <param name="id">The unique identifier of the document to soft-delete.</param>
    /// <returns>True if the document was successfully marked as deleted; otherwise, false.</returns>
    bool Delete (string id);

    /// <summary>
    /// Finds active entities matching the given expression predicate evaluated on the server side.
    /// </summary>
    /// <param name="predicate">The filter predicate expression.</param>
    /// <returns>A collection of matching entities.</returns>
    IEnumerable<T> Find (Expression<Func<T, bool>> predicate);
}