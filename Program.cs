using MongoDB_SongManager.Data;
using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;

namespace MongoDB_SongManager
{
    internal static class Program
    {
        [STAThread]
        static async Task Main ()
        {
            ApplicationConfiguration.Initialize();

            // 1. Database Context & Automatic Seeding
            var dbContext = new MongoDbContext();
            await DbInitializer.SeedAsync(dbContext);

            // 2. Repositories
            var songRepo = new MongoRepository<Song>(dbContext.Songs);
            var artistRepo = new MongoRepository<Artist>(dbContext.Artists);
            var userRepo = new MongoRepository<User>(dbContext.Users);

            // 3. Singletons / Services
            var currentUserService = new CurrentUserService();

            // 4. Launch Main Form
            Application.Run(new MainForm(
                currentUserService,
                userRepo,
                songRepo,
                artistRepo
            ));
        }
    }
}