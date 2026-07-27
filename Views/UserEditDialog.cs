using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Views
{
    /// <summary>
    /// Modal dialog window for creating or editing user details.
    /// </summary>
    public partial class UserEditDialog : Form
    {
        /// <summary>
        /// Gets or sets the original user ID when editing an existing user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets the resulting user DTO populated with input data from the dialog.
        /// </summary>
        public UserDto UserDto => new UserDto
        {
            Id = UserId ?? string.Empty,
            Name = txtName.Text.Trim()
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditDialog"/> class for creating a new user.
        /// </summary>
        public UserEditDialog ()
        {
            InitializeComponent();
            Text = "Neuen Benutzer anlegen";
            WireUpEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditDialog"/> class with existing user data for editing.
        /// </summary>
        /// <param name="user">The user DTO containing existing values to edit.</param>
        public UserEditDialog (UserDto user) : this()
        {
            ArgumentNullException.ThrowIfNull(user);

            Text = "Benutzer bearbeiten";
            UserId = user.Id;
            txtName.Text = user.Name;
        }

        /// <summary>
        /// Attaches event handlers for dialog actions and input validations.
        /// </summary>
        private void WireUpEvents ()
        {
            btnSave.Click += BtnSave_Click;
        }

        /// <summary>
        /// Validates user input and sets the dialog result to OK if validation succeeds.
        /// </summary>
        private void BtnSave_Click (object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Bitte geben Sie einen Benutzernamen ein.", "Validierungsfehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}