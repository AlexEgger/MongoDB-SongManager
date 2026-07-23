using MongoDB_SongManager.Models;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Dialog form for creating or editing an <see cref="Artist"/>.
    /// </summary>
    public partial class ArtistDialog : Form
    {
        /// <summary>
        /// Gets the created or modified artist entity.
        /// </summary>
        public Artist Artist { get; private set; }

        private TextBox txtName = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private Label lblName = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistDialog"/> class.
        /// </summary>
        /// <param name="artist">Optional existing artist to edit, or null to create a new one.</param>
        public ArtistDialog (Artist? artist = null)
        {
            InitializeComponent();

            Artist = artist ?? new Artist();
            if (!string.IsNullOrEmpty(Artist.Name))
            {
                txtName.Text = Artist.Name;
                Text = "Interpret bearbeiten";
            }
            else
            {
                Text = "Neuer Interpret";
            }
        }

        private void InitializeComponent ()
        {
            lblName = new Label();
            txtName = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // Label
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(12, 15);
            lblName.Text = "Name des Interpreten:";

            // TextBox
            txtName.Location = new System.Drawing.Point(15, 35);
            txtName.Size = new System.Drawing.Size(295, 23);

            // Save Button
            btnSave.Text = "Speichern";
            btnSave.Location = new System.Drawing.Point(130, 75);
            btnSave.Size = new System.Drawing.Size(85, 30);
            btnSave.Click += OnSaveClicked;

            // Cancel Button
            btnCancel.Text = "Abbrechen";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(225, 75);
            btnCancel.Size = new System.Drawing.Size(85, 30);

            // Form Settings
            ClientSize = new System.Drawing.Size(324, 115);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AcceptButton = btnSave;
            CancelButton = btnCancel;

            ResumeLayout(false);
            PerformLayout();
        }

        private void OnSaveClicked (object? sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Bitte geben Sie einen Namen für den Interpreten ein.", "Validierungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            Artist.Name = name;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}