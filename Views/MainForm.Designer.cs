namespace MongoDB_SongManager.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent ()
        {
            pnlHeader = new Panel();
            cmbUser = new ComboBox();
            lblUserHeader = new Label();
            btnNavSonglists = new Button();
            btnNavSongs = new Button();
            pnlContentContainer = new Panel();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = SystemColors.ControlLight;
            pnlHeader.Controls.Add(cmbUser);
            pnlHeader.Controls.Add(lblUserHeader);
            pnlHeader.Controls.Add(btnNavSonglists);
            pnlHeader.Controls.Add(btnNavSongs);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(10, 5, 10, 5);
            pnlHeader.Size = new Size(1008, 45);
            pnlHeader.TabIndex = 0;
            // 
            // cmbUser
            // 
            cmbUser.Dock = DockStyle.Right;
            cmbUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUser.FormattingEnabled = true;
            cmbUser.Location = new Point(746, 5);
            cmbUser.Name = "cmbUser";
            cmbUser.Size = new Size(180, 23);
            cmbUser.TabIndex = 3;
            // 
            // lblUserHeader
            // 
            lblUserHeader.AutoSize = true;
            lblUserHeader.Dock = DockStyle.Right;
            lblUserHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUserHeader.Location = new Point(926, 5);
            lblUserHeader.Name = "lblUserHeader";
            lblUserHeader.Padding = new Padding(0, 5, 10, 0);
            lblUserHeader.Size = new Size(72, 20);
            lblUserHeader.TabIndex = 2;
            lblUserHeader.Text = "Benutzer:";
            // 
            // btnNavSonglists
            // 
            btnNavSonglists.Location = new Point(155, 7);
            btnNavSonglists.Name = "btnNavSonglists";
            btnNavSonglists.Size = new Size(130, 30);
            btnNavSonglists.TabIndex = 1;
            btnNavSonglists.Text = "🎵 Setlists / Listen";
            btnNavSonglists.UseVisualStyleBackColor = true;
            // 
            // btnNavSongs
            // 
            btnNavSongs.Location = new Point(10, 7);
            btnNavSongs.Name = "btnNavSongs";
            btnNavSongs.Size = new Size(135, 30);
            btnNavSongs.TabIndex = 0;
            btnNavSongs.Text = "🎼 Songs & Interpreten";
            btnNavSongs.UseVisualStyleBackColor = true;
            // 
            // pnlContentContainer
            // 
            pnlContentContainer.Dock = DockStyle.Fill;
            pnlContentContainer.Location = new Point(0, 45);
            pnlContentContainer.Name = "pnlContentContainer";
            pnlContentContainer.Size = new Size(1008, 684);
            pnlContentContainer.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 729);
            Controls.Add(pnlContentContainer);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(800, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MongoDB SongManager";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Button btnNavSonglists;
        private Button btnNavSongs;
        private ComboBox cmbUser;
        private Label lblUserHeader;
        private Panel pnlContentContainer;
    }
}