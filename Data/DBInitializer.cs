using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Data;

/// <summary>
/// Provides functionality to seed initial data into MongoDB if the collections are empty.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Checks all database collections and seeds them using data from the specified JSON file if empty.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="jsonFileName">The filename of the seed JSON file located in the application directory.</param>
    public static async Task SeedAsync (MongoDbContext context, string jsonFileName = "SeedData.json")
    {
        // Resolve absolute path based on executable output directory
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonFileName);

        if (!File.Exists(fullPath))
        {
            return;
        }

        // Check if data already exists in any key collection
        bool hasArtists = await context.Artists.Find(_ => true).AnyAsync();
        bool hasSongs = await context.Songs.Find(_ => true).AnyAsync();

        if (hasArtists || hasSongs)
        {
            return; // Database is already populated
        }

        string jsonContent = await File.ReadAllTextAsync(fullPath);
        BsonDocument seedDocument = BsonDocument.Parse(jsonContent);

        await SeedCollectionAsync<Artist>(context.Artists, seedDocument, "Artists");
        await SeedCollectionAsync<Song>(context.Songs, seedDocument, "Songs");
        await SeedCollectionAsync<User>(context.Users, seedDocument, "Users");
        await SeedCollectionAsync<Songlist>(context.Songlists, seedDocument, "Songlists");
        await SeedCollectionAsync<UserSongInteraction>(context.UserInteractions, seedDocument, "UserSongInteractions");
    }

    private static async Task SeedCollectionAsync<T> (IMongoCollection<T> collection, BsonDocument rootDocument, string key)
    {
        if (!rootDocument.Contains(key) || !rootDocument[key].IsBsonArray)
        {
            return;
        }

        BsonArray bsonArray = rootDocument[key].AsBsonArray;
        List<T> entities = new List<T>();

        foreach (BsonValue item in bsonArray)
        {
            T entity = BsonSerializer.Deserialize<T>(item.AsBsonDocument);
            entities.Add(entity);
        }

        if (entities.Count > 0)
        {
            await collection.InsertManyAsync(entities);
        }
    }
}