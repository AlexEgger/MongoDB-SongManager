using MongoDB.Driver;
using MongoDB_SongManager.Models;
using System.Linq.Expressions;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// Generic MongoDB repository implementation providing automated soft-delete filtering and CRUD operations.
/// </summary>
/// <typeparam name="T">The document entity type implementing <see cref="IEntity"/>.</typeparam>
public class MongoRepository<T> : IRepository<T> where T : class, IEntity
{
    /// <summary>
    /// Holds the underlying MongoDB collection reference.
    /// Protected to allow derived repositories access to driver-specific query operations.
    /// </summary>
    protected readonly IMongoCollection<T> Collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepository{T}"/> class using a specific collection instance.
    /// </summary>
    /// <param name="collection">The target MongoDB collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
    public MongoRepository (IMongoCollection<T> collection)
    {
        Collection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepository{T}"/> class using connection configuration strings.
    /// </summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <param name="databaseName">The name of the target database.</param>
    public MongoRepository (string connectionString = "mongodb://localhost:27017", string databaseName = "SongManagerDb")
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        Collection = database.GetCollection<T>(typeof(T).Name);
    }

    /// <summary>
    /// Retrieves all active (non-deleted) entities from the collection.
    /// </summary>
    /// <returns>An enumerable collection of entities.</returns>
    public IEnumerable<T> GetAll () => Collection.Find(e => !e.IsDeleted).ToList();

    /// <summary>
    /// Retrieves a single active entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The matching entity instance, or null if not found or marked as deleted.</returns>
    public T? GetById (string id) => Collection.Find(e => e.Id == id && !e.IsDeleted).FirstOrDefault();

    /// <summary>
    /// Inserts a new entity document into the MongoDB collection.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    public void Insert (T entity) => Collection.InsertOne(entity);

    /// <summary>
    /// Replaces an existing active entity document with updated values.
    /// </summary>
    /// <param name="entity">The updated entity instance.</param>
    /// <returns><c>true</c> if the document was successfully modified; otherwise, <c>false</c>.</returns>
    public bool Update (T entity)
    {
        var filter = Builders<T>.Filter.And(
            Builders<T>.Filter.Eq(e => e.Id, entity.Id),
            Builders<T>.Filter.Eq(e => e.IsDeleted, false)
        );
        var result = Collection.ReplaceOne(filter, entity);
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Performs a soft-delete by setting the <see cref="IEntity.IsDeleted"/> flag to true.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to mark as deleted.</param>
    /// <returns><c>true</c> if the document was updated; otherwise, <c>false</c>.</returns>
    public bool Delete (string id)
    {
        var filter = Builders<T>.Filter.And(
            Builders<T>.Filter.Eq(e => e.Id, id),
            Builders<T>.Filter.Eq(e => e.IsDeleted, false)
        );
        var update = Builders<T>.Update.Set(e => e.IsDeleted, true);
        var result = Collection.UpdateOne(filter, update);
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Finds active entities matching a specified filter expression while automatically excluding soft-deleted items.
    /// </summary>
    /// <param name="predicate">A LINQ predicate expression to filter entities.</param>
    /// <returns>An enumerable collection of matching entities.</returns>
    public IEnumerable<T> Find (Expression<Func<T, bool>> predicate)
    {
        var softDeleteFilter = Builders<T>.Filter.Eq(e => e.IsDeleted, false);
        var customFilter = Builders<T>.Filter.Where(predicate);
        var combinedFilter = Builders<T>.Filter.And(softDeleteFilter, customFilter);

        return Collection.Find(combinedFilter).ToList();
    }
}