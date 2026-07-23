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
    /// Finds an active artist by their exact name using a case-insensitive search.
    /// </summary>
    /// <param name="name">The artist name to search for.</param>
    /// <returns>The matching artist entity if found; otherwise, <c>null</c>.</returns>
    public Artist? GetByName (string name)
    {
        string normalizedName = name.Trim().ToLower();
        return Collection
            .Find(a => a.Name.ToLower() == normalizedName && !a.IsDeleted)
            .FirstOrDefault();
    }
}