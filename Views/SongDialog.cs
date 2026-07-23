using MongoDB_SongManager.Models;

namespace SongManager.Views
{
    /// <summary>
    /// Modal dialog form for creating or editing a <see cref="Song"/> entity.
    /// </summary>
    public partial class SongDialog : Form
    {
        private readonly Song _song;

        /// <summary>
        /// Gets the updated or newly created <see cref="Song"/> document.
        /// </summary>
        public Song Song => _song;

        /// <summary>
        /// Initializes a new instance of the <see cref="SongDialog"/> form.
        /// </summary>
        /// <param name="song">The song entity to edit, or null/new song for creation mode.</param>
        /// <param name="artists">The collection of available active artists for selection.</param>
        public SongDialog (Song? song, IEnumerable<Artist> artists)
        {
            InitializeComponent();

            _song = song ?? new Song();

            PopulateArtistComboBox(artists);
            BindDataToControls();
            WireUpEvents();
        }

        /// <summary>
        /// Populates the artist dropdown ComboBox with available artist records.
        /// </summary>
        /// <param name="artists">The list of active artists.</param>
        private void PopulateArtistComboBox (IEnumerable<Artist> artists)
        {
            var artistList = artists.ToList();

            // Insert default option for unassigned artist
            artistList.Insert(0, new Artist { Id = string.Empty, Name = "-- Kein Interpret --" });

            cmbArtist.DataSource = artistList;
            cmbArtist.DisplayMember = "Name";
            cmbArtist.ValueMember = "Id";
        }

        /// <summary>
        /// Binds values from the <see cref="Song"/> entity model to UI input controls.
        /// </summary>
        private void BindDataToControls ()
        {
            Text = string.IsNullOrEmpty(_song.Id) ? "Neuen Song erstellen" : "Song bearbeiten";

            txtTitle.Text = _song.Title;
            numTempo.Value = _song.Tempo.HasValue ? Math.Min(_song.Tempo.Value, numTempo.Maximum) : 0;
            txtChordsUrl.Text = _song.ChordsUrl ?? string.Empty;
            txtYoutubeUrl.Text = _song.YoutubeUrl ?? string.Empty;
            numLiederbuchnummer.Value = _song.Liederbuchnummer.HasValue ? Math.Min(_song.Liederbuchnummer.Value, numLiederbuchnummer.Maximum) : 0;
            numLiederbuchseite.Value = _song.Liederbuchseite.HasValue ? Math.Min(_song.Liederbuchseite.Value, numLiederbuchseite.Maximum) : 0;

            if (!string.IsNullOrEmpty(_song.ArtistId))
            {
                cmbArtist.SelectedValue = _song.ArtistId;
            }
            else
            {
                cmbArtist.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Wires up UI event handlers for dialog actions.
        /// </summary>
        private void WireUpEvents ()
        {
            btnSave.Click += OnSaveClicked;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Validates user input and saves entered data into the <see cref="Song"/> model instance.
        /// </summary>
        private void OnSaveClicked (object? sender, EventArgs e)
        {
            // Validate required title field
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Bitte geben Sie einen Songtitel ein.", "Validierungsfehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            // Validate chord URL format if provided
            string chordsUrl = txtChordsUrl.Text.Trim();
            if (!string.IsNullOrEmpty(chordsUrl) && !Uri.IsWellFormedUriString(chordsUrl, UriKind.Absolute))
            {
                MessageBox.Show("Die eingegebene Akkorde-URL ist ungültig.", "Validierungsfehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChordsUrl.Focus();
                return;
            }

            // Validate YouTube URL format if provided
            string youtubeUrl = txtYoutubeUrl.Text.Trim();
            if (!string.IsNullOrEmpty(youtubeUrl) && !Uri.IsWellFormedUriString(youtubeUrl, UriKind.Absolute))
            {
                MessageBox.Show("Die eingegebene YouTube-URL ist ungültig.", "Validierungsfehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtYoutubeUrl.Focus();
                return;
            }

            // Write back inputs to model object
            _song.Title = txtTitle.Text.Trim();
            _song.ArtistId = cmbArtist.SelectedValue as string;
            if (string.IsNullOrEmpty(_song.ArtistId))
            {
                _song.ArtistId = null;
            }

            _song.Tempo = numTempo.Value > 0 ? (uint?)numTempo.Value : null;
            _song.ChordsUrl = string.IsNullOrEmpty(chordsUrl) ? null : chordsUrl;
            _song.YoutubeUrl = string.IsNullOrEmpty(youtubeUrl) ? null : youtubeUrl;
            _song.Liederbuchnummer = numLiederbuchnummer.Value > 0 ? (uint?)numLiederbuchnummer.Value : null;
            _song.Liederbuchseite = numLiederbuchseite.Value > 0 ? (uint?)numLiederbuchseite.Value : null;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}