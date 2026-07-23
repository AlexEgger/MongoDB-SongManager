using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views;

/// <summary>
/// Represents the contract between the main WinForms user interface and the presenter.
/// </summary>
public interface IMainView
{
    // Events emitted by user interactions
    event EventHandler ViewLoaded;
    event EventHandler SearchRequested;
    event EventHandler ImportCsvRequested;
    event EventHandler ExportCsvRequested;

    // View inputs
    string SearchText { get; }
    string SelectedCsvPath { get; }

    // Display updates
    void DisplaySongs (List<SongDisplayDto> songs);
    void ShowErrorMessage (string message);
    void ShowSuccessMessage (string message);
}