using MongoDB_SongManager.Services.DTOs;
using SongManager.Views;

namespace MongoDB_SongManager.Views;

/// <summary>
/// Concrete WinForms view implementation managing controls and standard user interactions.
/// </summary>
public partial class MainForm : Form, IMainView
{
    public event EventHandler? ViewLoaded;
    public event EventHandler? SearchRequested;
    public event EventHandler? ImportCsvRequested;
    public event EventHandler? ExportCsvRequested;

    //public string SearchText => txtSearch.Text;
    public string SelectedCsvPath { get; private set; } = string.Empty;

    string IMainView.SearchText => throw new NotImplementedException();

    public MainForm ()
    {
        InitializeComponent();

        // Bind control events to interface events
        Load += (s, e) => ViewLoaded?.Invoke(this, EventArgs.Empty);

        SongsView songsView = new SongsView();
        songsView.Dock = DockStyle.Fill;

        tabPageSongs.Controls.Add(songsView);

        //btnSearch.Click += (s, e) => SearchRequested?.Invoke(this, EventArgs.Empty);
        //btnImportCsv.Click += OnImportCsvClicked;
    }

    private void OnImportCsvClicked (object? sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog
        {
            Filter = "CSV Files (*.csv)|*.csv",
            Title = "Select CSV File to Import"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            SelectedCsvPath = openFileDialog.FileName;
            ImportCsvRequested?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DisplaySongs (List<SongDisplayDto> songs)
    {
        //dgvSongs.DataSource = null;
        //dgvSongs.DataSource = songs;
    }

    public void ShowErrorMessage (string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowSuccessMessage (string message)
    {
        MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}