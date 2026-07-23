using MongoDB_SongManager.Data;
using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;

namespace MongoDB_SongManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main ()
        {
            ApplicationConfiguration.Initialize();

            // 1. Initialize Database Context and trigger automatic seeding
            var dbContext = new MongoDbContext();
            await DbInitializer.SeedAsync(dbContext);

            // 2. Instantiate all concrete repository implementations
            IRepository<User> userRepository = new MongoRepository<User>(dbContext.Users);
            ISongRepository songRepository = new MongoSongRepository(dbContext);
            IArtistRepository artistRepository = new MongoArtistRepository(dbContext);
            ISonglistRepository songlistRepository = new MongoSonglistRepository(dbContext);
            IUserInteractionRepository userInteractionRepository = new MongoUserInteractionRepository(dbContext);

            // 3. Instantiate domain services
            var currentUserService = new CurrentUserService();

            // 4. Launch Main Form with all required dependencies injected
            Application.Run(new MainForm(
                currentUserService,
                userRepository,
                songRepository,
                artistRepository,
                songlistRepository,
                userInteractionRepository
            ));
        }
    }
}