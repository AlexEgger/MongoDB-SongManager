using MongoDB_SongManager.Data;
using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Presenters;
using MongoDB_SongManager.Services;
using MongoDB_SongManager.Views;

namespace MongoDB_SongManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main ()
        {
            ApplicationConfiguration.Initialize();

            // 1. Initialize DB context & Seed data
            var dbContext = new MongoDbContext();
            await DbInitializer.SeedAsync(dbContext);

            // 2. Initialize Repositories & Services
            ISongRepository songRepo = new MongoSongRepository(dbContext);
            IArtistRepository artistRepo = new MongoArtistRepository(dbContext);
            ICsvService csvService = new CsvService();

            // 3. Create View and Presenter
            MainForm mainForm = new MainForm();
            var presenter = new MainPresenter(mainForm, songRepo, artistRepo, csvService);

            // 4. Start WinForms application loop
            Application.Run(mainForm);
        }
    }
}