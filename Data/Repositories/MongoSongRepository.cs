using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data.Repositories;

/// <summary>
/// MongoDB implementation of the ISongRepository contract.
/// </summary>
public class MongoSongRepository : ISongRepository
{
    private readonly IMongoCollection<Song> _songs;

    /// <summary>
    /// Initializes a new repository instance using the shared database context.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public MongoSongRepository (MongoDbContext context)
    {
        _songs = context.Songs;
    }

    public async Task<List<Song>> GetAllActiveAsync ()
    {
        return await _songs.Find(s => !s.IsDeleted).ToListAsync();
    }

    public async Task<Song?> GetByIdAsync (string id)
    {
        return await _songs.Find(s => s.Id == id && !s.IsDeleted).FirstOrDefaultAsync();
    }

    public async Task<List<Song>> GetByArtistIdAsync (string artistId)
    {
        return await _songs.Find(s => s.ArtistId == artistId && !s.IsDeleted).ToListAsync();
    }

    public async Task<List<Song>> SearchByTitleAsync (string titleQuery)
    {
        if (string.IsNullOrWhiteSpace(titleQuery))
        {
            return await GetAllActiveAsync();
        }

        // Filter for active songs matching title query (case-insensitive)
        var filter = Builders<Song>.Filter.And(
            Builders<Song>.Filter.Eq(s => s.IsDeleted, false),
            Builders<Song>.Filter.Regex(s => s.Title, new BsonRegularExpression(titleQuery, "i"))
        );

        return await _songs.Find(filter).ToListAsync();
    }

    public async Task CreateAsync (Song song)
    {
        await _songs.InsertOneAsync(song);
    }

    public async Task UpdateAsync (Song song)
    {
        await _songs.ReplaceOneAsync(s => s.Id == song.Id, song);
    }

    public async Task DeleteSoftAsync (string id)
    {
        var filter = Builders<Song>.Filter.Eq(s => s.Id, id);
        var update = Builders<Song>.Update.Set(s => s.IsDeleted, true);

        await _songs.UpdateOneAsync(filter, update);
    }
}