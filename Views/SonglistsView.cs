namespace SongManager.Views
{
    public partial class SonglistsView : UserControl
    {
        public SonglistsView ()
        {
            InitializeComponent();
            LoadMockData();
        }

        private void LoadMockData ()
        {
            lstSetlists.SelectedIndex = 0;

            // Mockup: Song-Bibliothek (Links)
            dgvAvailableSongs.Rows.Add("Enter Sandman", "Metallica", "5:31");
            dgvAvailableSongs.Rows.Add("Hotel California", "Eagles", "6:30");
            dgvAvailableSongs.Rows.Add("Sweet Child O' Mine", "Guns N' Roses", "5:56");
            dgvAvailableSongs.Rows.Add("Wonderwall", "Oasis", "4:18");

            // Mockup: Setlist Inhalte (Rechts)
            dgvSonglistSongs.Rows.Add("1", "Bohemian Rhapsody", "Bb", "5:55");
            dgvSonglistSongs.Rows.Add("2", "Master of Puppets", "Em", "8:35");
            dgvSonglistSongs.Rows.Add("3", "Highway to Hell", "A", "3:28");
            dgvSonglistSongs.Rows.Add("4", "Nothing Else Matters", "Em", "6:28");
        }
    }
}