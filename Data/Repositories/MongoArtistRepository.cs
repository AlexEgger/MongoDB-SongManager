using MongoDB.Driver;
using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// MongoDB implementation of the IArtistRepository contract.
/// </summary>
public class MongoArtistRepository : IArtistRepository
{
    private readonly IMongoCollection<Artist> _artists;

    /// <summary>
    /// Initializes a new repository instance using the shared database context.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public MongoArtistRepository (MongoDbContext context)
    {
        _artists = context.Artists;
    }

    public async Task<List<Artist>> GetAllActiveAsync ()
    {
        return await _artists.Find(a => !a.IsDeleted).ToListAsync();
    }

    public async Task<Artist?> GetByIdAsync (string id)
    {
        return await _artists.Find(a => a.Id == id && !a.IsDeleted).FirstOrDefaultAsync();
    }

    public async Task<Artist?> GetByNameAsync (string name)
    {
        string normalizedName = name.Trim().ToLower();
        return await _artists
            .Find(a => a.Name.ToLower() == normalizedName && !a.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync (Artist artist)
    {
        await _artists.InsertOneAsync(artist);
    }

    public async Task UpdateAsync (Artist artist)
    {
        await _artists.ReplaceOneAsync(a => a.Id == artist.Id, artist);
    }

    public async Task DeleteSoftAsync (string id)
    {
        var filter = Builders<Artist>.Filter.Eq(a => a.Id, id);
        var update = Builders<Artist>.Update.Set(a => a.IsDeleted, true);

        await _artists.UpdateOneAsync(filter, update);
    }
}