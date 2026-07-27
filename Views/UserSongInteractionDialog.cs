using MongoDB_SongManager.Models.Enums;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Dialog window for creating or editing user-specific ratings and notes for a song.
    /// </summary>
    public partial class UserSongInteractionDialog : Form
    {
        private readonly TextBox _txtNotes;
        private readonly Dictionary<RatingType, NumericUpDown> _ratingInputs = new();
        private readonly Button _btnSave;
        private readonly Button _btnCancel;

        /// <summary>
        /// Gets the resulting save DTO containing updated ratings and notes.
        /// </summary>
        public SaveUserSongInteractionDto InteractionDto { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSongInteractionDialog"/> class.
        /// </summary>
        /// <param name="songTitle">The title of the song being rated.</param>
        /// <param name="existingInteraction">Existing interaction data to prepopulate, if any.</param>
        public UserSongInteractionDialog (string songTitle, UserSongInteractionDto? existingInteraction)
        {
            InteractionDto = new SaveUserSongInteractionDto();

            Text = $"Edit Interaction: {songTitle}";
            Width = 400;
            Height = 450;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(10)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            int row = 0;

            // Header for Ratings section
            var lblHeader = new Label
            {
                Text = "Category Ratings (1 - 5):",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(lblHeader, 0, row);
            layout.SetColumnSpan(lblHeader, 2);
            row++;

            // Dynamically create numeric inputs for each RatingType enum value
            foreach (RatingType ratingType in Enum.GetValues(typeof(RatingType)))
            {
                var lblCategory = new Label
                {
                    Text = ratingType.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                var numValue = new NumericUpDown
                {
                    Minimum = 0,
                    Maximum = 5,
                    Value = 0,
                    Dock = DockStyle.Fill
                };

                // Prepopulate if existing rating matches
                if (existingInteraction?.Ratings != null)
                {
                    var match = existingInteraction.Ratings.Find(r => r.Category.Equals(ratingType.ToString(), StringComparison.OrdinalIgnoreCase));
                    if (match != null)
                    {
                        numValue.Value = Math.Clamp(match.Value, 0, 5);
                    }
                }

                _ratingInputs[ratingType] = numValue;

                layout.Controls.Add(lblCategory, 0, row);
                layout.Controls.Add(numValue, 1, row);
                row++;
            }

            // Notes header & textbox
            var lblNotes = new Label
            {
                Text = "Personal Notes:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(lblNotes, 0, row);
            layout.SetColumnSpan(lblNotes, 2);
            row++;

            _txtNotes = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                Text = existingInteraction?.Notes ?? string.Empty
            };
            layout.Controls.Add(_txtNotes, 0, row);
            layout.SetColumnSpan(_txtNotes, 2);
            row++;

            // Bottom Buttons Panel
            var pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };

            _btnSave = new Button { Text = "Save", DialogResult = DialogResult.OK };
            _btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel };

            _btnSave.Click += (s, e) => SaveData();

            pnlButtons.Controls.Add(_btnSave);
            pnlButtons.Controls.Add(_btnCancel);

            layout.Controls.Add(pnlButtons, 0, row);
            layout.SetColumnSpan(pnlButtons, 2);

            Controls.Add(layout);
            AcceptButton = _btnSave;
            CancelButton = _btnCancel;
        }

        /// <summary>
        /// Gathers control values and populates the <see cref="InteractionDto"/>.
        /// </summary>
        private void SaveData ()
        {
            var ratingsList = new List<RatingDto>();

            foreach (var kvp in _ratingInputs)
            {
                if (kvp.Value.Value > 0)
                {
                    ratingsList.Add(new RatingDto
                    {
                        Category = kvp.Key.ToString(),
                        Value = (int)kvp.Value.Value
                    });
                }
            }

            InteractionDto.Ratings = ratingsList;
            InteractionDto.Notes = _txtNotes.Text.Trim();
        }
    }
}