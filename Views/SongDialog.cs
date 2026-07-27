using MongoDB_SongManager.Services.DTOs;

namespace SongManager.Views
{
    /// <summary>
    /// Modal dialog form for creating or editing a song using a <see cref="SongDto"/>.
    /// </summary>
    public partial class SongDialog : Form
    {
        private readonly SongDto _songDto;

        /// <summary>
        /// Gets the updated or newly created <see cref="SongDto"/>.
        /// </summary>
        public SongDto SongDto => _songDto;

        /// <summary>
        /// Initializes a new instance of the <see cref="SongDialog"/> form using DTO parameters.
        /// </summary>
        /// <param name="songDto">The song DTO to edit, or null for creation mode.</param>
        /// <param name="artists">The collection of available active artist DTOs for selection.</param>
        public SongDialog (SongDto? songDto, IEnumerable<ArtistDto> artists)
        {
            InitializeComponent();

            _songDto = songDto ?? new SongDto();

            PopulateArtistComboBox(artists);
            BindDataToControls();
            WireUpEvents();
        }

        /// <summary>
        /// Populates the artist dropdown ComboBox with available artist DTOs.
        /// </summary>
        /// <param name="artists">The list of active artist DTOs.</param>
        private void PopulateArtistComboBox (IEnumerable<ArtistDto> artists)
        {
            var artistList = artists.ToList();

            // Insert default option for unassigned artist
            artistList.Insert(0, new ArtistDto { Id = string.Empty, Name = "-- Kein Interpret --" });

            cmbArtist.DataSource = artistList;
            cmbArtist.DisplayMember = "Name";
            cmbArtist.ValueMember = "Id";
        }

        /// <summary>
        /// Binds values from the <see cref="SongDto"/> instance to UI input controls.
        /// </summary>
        private void BindDataToControls ()
        {
            Text = string.IsNullOrEmpty(_songDto.Id) ? "Neuen Song erstellen" : "Song bearbeiten";

            txtTitle.Text = _songDto.Title;
            numTempo.Value = _songDto.Tempo.HasValue ? Math.Min(_songDto.Tempo.Value, numTempo.Maximum) : 0;
            txtChordsUrl.Text = _songDto.ChordsUrl ?? string.Empty;
            txtYoutubeUrl.Text = _songDto.YoutubeUrl ?? string.Empty;
            numLiederbuchnummer.Value = _songDto.Liederbuchnummer.HasValue ? Math.Min(_songDto.Liederbuchnummer.Value, numLiederbuchnummer.Maximum) : 0;
            numLiederbuchseite.Value = _songDto.Liederbuchseite.HasValue ? Math.Min(_songDto.Liederbuchseite.Value, numLiederbuchseite.Maximum) : 0;

            if (!string.IsNullOrEmpty(_songDto.ArtistId))
            {
                cmbArtist.SelectedValue = _songDto.ArtistId;
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
        /// Validates user input and saves entered data into the <see cref="SongDto"/> instance.
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

            // Write back inputs to DTO object
            _songDto.Title = txtTitle.Text.Trim();
            _songDto.ArtistId = cmbArtist.SelectedValue as string;
            if (string.IsNullOrEmpty(_songDto.ArtistId))
            {
                _songDto.ArtistId = null;
            }

            _songDto.Tempo = numTempo.Value > 0 ? (uint?)numTempo.Value : null;
            _songDto.ChordsUrl = string.IsNullOrEmpty(chordsUrl) ? null : chordsUrl;
            _songDto.YoutubeUrl = string.IsNullOrEmpty(youtubeUrl) ? null : youtubeUrl;
            _songDto.Liederbuchnummer = numLiederbuchnummer.Value > 0 ? (uint?)numLiederbuchnummer.Value : null;
            _songDto.Liederbuchseite = numLiederbuchseite.Value > 0 ? (uint?)numLiederbuchseite.Value : null;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}