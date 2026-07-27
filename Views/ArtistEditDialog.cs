using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Modal dialog form for creating or editing artist information.
    /// </summary>
    public partial class ArtistEditDialog : Form
    {
        private TextBox txtName = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        /// <summary>
        /// Gets the populated artist DTO created or edited via the dialog interface.
        /// </summary>
        public ArtistDto Artist { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistEditDialog"/> class.
        /// </summary>
        /// <param name="existingArtist">Optional artist DTO to populate for editing; if null, creates a new entry.</param>
        public ArtistEditDialog (ArtistDto? existingArtist = null)
        {
            InitializeDialogComponents();

            if (existingArtist != null)
            {
                Text = "Edit Artist";
                Artist = new ArtistDto
                {
                    Id = existingArtist.Id,
                    Name = existingArtist.Name
                };

                txtName.Text = Artist.Name;
            }
            else
            {
                Text = "Add New Artist";
                Artist = new ArtistDto();
            }
        }

        /// <summary>
        /// Builds and lays out the controls programmatically for quick initialization.
        /// </summary>
        private void InitializeDialogComponents ()
        {
            Size = new System.Drawing.Size(380, 150);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblName = new Label { Text = "Artist Name:", Location = new System.Drawing.Point(20, 20), AutoSize = true };
            txtName = new TextBox { Location = new System.Drawing.Point(120, 17), Width = 220 };

            btnSave = new Button { Text = "Save", DialogResult = DialogResult.OK, Location = new System.Drawing.Point(180, 65), Width = 75 };
            btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new System.Drawing.Point(265, 65), Width = 75 };

            btnSave.Click += BtnSave_Click;

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        /// <summary>
        /// Validates user input before closing the modal dialog with OK status.
        /// </summary>
        private void BtnSave_Click (object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter an artist name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            Artist.Name = txtName.Text.Trim();
        }
    }
}