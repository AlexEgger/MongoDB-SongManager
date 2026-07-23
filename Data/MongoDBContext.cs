using MongoDB.Driver;
using MongoDB_SongManager.Models;
using User = MongoDB_SongManager.Models.User;

namespace MongoDB_SongManager.Data;

/// <summary>
/// Manages connection settings and provides access to MongoDB collections.
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    /// <summary>
    /// Initializes a new instance of the MongoDbContext class.
    /// </summary>
    /// <param name="connectionString">The connection string for MongoDB.</param>
    /// <param name="databaseName">The name of the database.</param>
    public MongoDbContext (string connectionString = "mongodb://localhost:27017", string databaseName = "SongManagerDb")
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    /// <summary>
    /// Gets the Artists collection.
    /// </summary>
    public IMongoCollection<Artist> Artists => _database.GetCollection<Artist>("Artists");

    /// <summary>
    /// Gets the Songs collection.
    /// </summary>
    public IMongoCollection<Song> Songs => _database.GetCollection<Song>("Songs");

    /// <summary>
    /// Gets the Users collection.
    /// </summary>
    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

    /// <summary>
    /// Gets the Songlists collection.
    /// </summary>
    public IMongoCollection<Songlist> Songlists => _database.GetCollection<Songlist>("Songlists");

    /// <summary>
    /// Gets the UserSongInteractions collection.
    /// </summary>
    public IMongoCollection<UserSongInteraction> UserInteractions => _database.GetCollection<UserSongInteraction>("UserSongInteractions");
}