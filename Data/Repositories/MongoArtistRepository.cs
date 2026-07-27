using MongoDB.Driver;
using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// MongoDB implementation of the <see cref="IArtistRepository"/> contract.
/// </summary>
public class MongoArtistRepository : MongoRepository<Artist>, IArtistRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoArtistRepository"/> class using the shared database context.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public MongoArtistRepository (MongoDbContext context) : base(context.Artists)
    {
    }

    /// <summary>
    /// Inserts a new artist document only if an active artist with the same name does not already exist.
    /// </summary>
    /// <param name="entity">The artist entity to insert.</param>
    /// <exception cref="InvalidOperationException">Thrown when an artist with the same name already exists.</exception>
    public override void Insert (Artist entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existingArtist = GetByName(entity.Name);
        if (existingArtist != null)
        {
            throw new InvalidOperationException($"An artist with the name '{entity.Name}' already exists.");
        }

        base.Insert(entity);
    }

    /// <summary>
    /// Finds an active artist by their exact name using a case-insensitive search.
    /// </summary>
    /// <param name="name">The artist name to search for.</param>
    /// <returns>The matching artist entity if found; otherwise, <c>null</c>.</returns>
    public Artist? GetByName (string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        string normalizedName = name.Trim().ToLower();
        return Collection
            .Find(a => a.Name.ToLower() == normalizedName && !a.IsDeleted)
            .FirstOrDefault();
    }

    /// <summary>
    /// Updates an existing artist document only if no other active artist with the same name exists.
    /// </summary>
    /// <param name="entity"> The artist entity to update. The entity's <see cref="Artist.Id"/> must match an existing document in the collection.</param>
    /// <returns><see langword="true"/> if the update was successful; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException"> Thrown when another active artist with the same name already exists in the collection.</exception>
    public override bool Update (Artist entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        string normalizedName = entity.Name?.Trim().ToLower() ?? string.Empty;

        // Check for duplicates
        bool duplicateExists = Collection.Find(a =>
        !a.IsDeleted &&
        a.Id != entity.Id &&
        a.Name.ToLower() == normalizedName
                                ).Any();

        if (duplicateExists)
        {
            throw new InvalidOperationException($"An artist with the name '{entity.Name}' already exists.");
        }

        return base.Update(entity);
    }
}