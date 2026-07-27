using MongoDB_SongManager.Data;
using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Presenters;
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
        static void Main ()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync ()
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
            ICurrentUserService currentUserService = new CurrentUserService();
            IDtoService dtoService = new DtoService();
            ICsvService csvService = new CsvService();

            // 4. Instantiate main view and presenter
            var mainForm = new MainForm();
            var mainPresenter = new MainPresenter(
                mainForm,
                userRepository,
                songRepository,
                artistRepository,
                songlistRepository,
                userInteractionRepository,
                currentUserService,
                dtoService,
                csvService
            );

            // 5. Initialize presenter logic and launch application
            mainPresenter.Initialize();
            Application.Run(mainForm);
        }
    }
}