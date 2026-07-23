using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SongManager.Views
{
    partial class SongDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            layoutTable = new TableLayoutPanel();
            lblTitle = new Label();
            txtTitle = new TextBox();
            lblArtist = new Label();
            cmbArtist = new ComboBox();
            lblTempo = new Label();
            numTempo = new NumericUpDown();
            lblChordsUrl = new Label();
            txtChordsUrl = new TextBox();
            lblYoutubeUrl = new Label();
            txtYoutubeUrl = new TextBox();
            lblLiederbuchnummer = new Label();
            numLiederbuchnummer = new NumericUpDown();
            lblLiederbuchseite = new Label();
            numLiederbuchseite = new NumericUpDown();
            pnlButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            layoutTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTempo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLiederbuchnummer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLiederbuchseite).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // layoutTable
            // 
            layoutTable.ColumnCount = 2;
            layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            layoutTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            layoutTable.Controls.Add(lblTitle, 0, 0);
            layoutTable.Controls.Add(txtTitle, 1, 0);
            layoutTable.Controls.Add(lblArtist, 0, 1);
            layoutTable.Controls.Add(cmbArtist, 1, 1);
            layoutTable.Controls.Add(lblTempo, 0, 2);
            layoutTable.Controls.Add(numTempo, 1, 2);
            layoutTable.Controls.Add(lblChordsUrl, 0, 3);
            layoutTable.Controls.Add(txtChordsUrl, 1, 3);
            layoutTable.Controls.Add(lblYoutubeUrl, 0, 4);
            layoutTable.Controls.Add(txtYoutubeUrl, 1, 4);
            layoutTable.Controls.Add(lblLiederbuchnummer, 0, 5);
            layoutTable.Controls.Add(numLiederbuchnummer, 1, 5);
            layoutTable.Controls.Add(lblLiederbuchseite, 0, 6);
            layoutTable.Controls.Add(numLiederbuchseite, 1, 6);
            layoutTable.Controls.Add(pnlButtons, 0, 7);
            layoutTable.Dock = DockStyle.Fill;
            layoutTable.Location = new Point(12, 12);
            layoutTable.Name = "layoutTable";
            layoutTable.RowCount = 8;
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layoutTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            layoutTable.Size = new Size(410, 290);
            layoutTable.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Left;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(3, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(77, 15);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Titel (Pflicht):";
            // 
            // txtTitle
            // 
            txtTitle.Dock = DockStyle.Fill;
            txtTitle.Location = new Point(146, 3);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(261, 23);
            txtTitle.TabIndex = 0;
            // 
            // lblArtist
            // 
            lblArtist.Anchor = AnchorStyles.Left;
            lblArtist.AutoSize = true;
            lblArtist.Location = new Point(3, 45);
            lblArtist.Name = "lblArtist";
            lblArtist.Size = new Size(55, 15);
            lblArtist.TabIndex = 1;
            lblArtist.Text = "Interpret:";
            // 
            // cmbArtist
            // 
            cmbArtist.Dock = DockStyle.Fill;
            cmbArtist.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArtist.FormattingEnabled = true;
            cmbArtist.Location = new Point(146, 38);
            cmbArtist.Name = "cmbArtist";
            cmbArtist.Size = new Size(261, 23);
            cmbArtist.TabIndex = 1;
            // 
            // lblTempo
            // 
            lblTempo.Anchor = AnchorStyles.Left;
            lblTempo.AutoSize = true;
            lblTempo.Location = new Point(3, 80);
            lblTempo.Name = "lblTempo";
            lblTempo.Size = new Size(82, 15);
            lblTempo.TabIndex = 2;
            lblTempo.Text = "Tempo (BPM):";
            // 
            // numTempo
            // 
            numTempo.Dock = DockStyle.Fill;
            numTempo.Location = new Point(146, 73);
            numTempo.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            numTempo.Name = "numTempo";
            numTempo.Size = new Size(261, 23);
            numTempo.TabIndex = 2;
            // 
            // lblChordsUrl
            // 
            lblChordsUrl.Anchor = AnchorStyles.Left;
            lblChordsUrl.AutoSize = true;
            lblChordsUrl.Location = new Point(3, 115);
            lblChordsUrl.Name = "lblChordsUrl";
            lblChordsUrl.Size = new Size(78, 15);
            lblChordsUrl.TabIndex = 3;
            lblChordsUrl.Text = "Akkorde URL:";
            // 
            // txtChordsUrl
            // 
            txtChordsUrl.Dock = DockStyle.Fill;
            txtChordsUrl.Location = new Point(146, 108);
            txtChordsUrl.Name = "txtChordsUrl";
            txtChordsUrl.PlaceholderText = "https://...";
            txtChordsUrl.Size = new Size(261, 23);
            txtChordsUrl.TabIndex = 3;
            // 
            // lblYoutubeUrl
            // 
            lblYoutubeUrl.Anchor = AnchorStyles.Left;
            lblYoutubeUrl.AutoSize = true;
            lblYoutubeUrl.Location = new Point(3, 150);
            lblYoutubeUrl.Name = "lblYoutubeUrl";
            lblYoutubeUrl.Size = new Size(80, 15);
            lblYoutubeUrl.TabIndex = 4;
            lblYoutubeUrl.Text = "YouTube URL:";
            // 
            // txtYoutubeUrl
            // 
            txtYoutubeUrl.Dock = DockStyle.Fill;
            txtYoutubeUrl.Location = new Point(146, 143);
            txtYoutubeUrl.Name = "txtYoutubeUrl";
            txtYoutubeUrl.PlaceholderText = "https://youtube.com/...";
            txtYoutubeUrl.Size = new Size(261, 23);
            txtYoutubeUrl.TabIndex = 4;
            // 
            // lblLiederbuchnummer
            // 
            lblLiederbuchnummer.Anchor = AnchorStyles.Left;
            lblLiederbuchnummer.AutoSize = true;
            lblLiederbuchnummer.Location = new Point(3, 185);
            lblLiederbuchnummer.Name = "lblLiederbuchnummer";
            lblLiederbuchnummer.Size = new Size(120, 15);
            lblLiederbuchnummer.TabIndex = 5;
            lblLiederbuchnummer.Text = "Liederbuch Nummer:";
            // 
            // numLiederbuchnummer
            // 
            numLiederbuchnummer.Dock = DockStyle.Fill;
            numLiederbuchnummer.Location = new Point(146, 178);
            numLiederbuchnummer.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numLiederbuchnummer.Name = "numLiederbuchnummer";
            numLiederbuchnummer.Size = new Size(261, 23);
            numLiederbuchnummer.TabIndex = 5;
            // 
            // lblLiederbuchseite
            // 
            lblLiederbuchseite.Anchor = AnchorStyles.Left;
            lblLiederbuchseite.AutoSize = true;
            lblLiederbuchseite.Location = new Point(3, 220);
            lblLiederbuchseite.Name = "lblLiederbuchseite";
            lblLiederbuchseite.Size = new Size(97, 15);
            lblLiederbuchseite.TabIndex = 6;
            lblLiederbuchseite.Text = "Liederbuch Seite:";
            // 
            // numLiederbuchseite
            // 
            numLiederbuchseite.Dock = DockStyle.Fill;
            numLiederbuchseite.Location = new Point(146, 213);
            numLiederbuchseite.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numLiederbuchseite.Name = "numLiederbuchseite";
            numLiederbuchseite.Size = new Size(261, 23);
            numLiederbuchseite.TabIndex = 6;
            // 
            // pnlButtons
            // 
            layoutTable.SetColumnSpan(pnlButtons, 2);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Dock = DockStyle.Fill;
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Location = new Point(3, 248);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 5, 0, 0);
            pnlButtons.Size = new Size(404, 39);
            pnlButtons.TabIndex = 7;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(326, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 28);
            btnSave.TabIndex = 0;
            btnSave.Text = "Speichern";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(245, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Abbrechen";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // SongDialog
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(434, 314);
            Controls.Add(layoutTable);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SongDialog";
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Song bearbeiten";
            layoutTable.ResumeLayout(false);
            layoutTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTempo).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLiederbuchnummer).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLiederbuchseite).EndInit();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layoutTable;
        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblArtist;
        private ComboBox cmbArtist;
        private Label lblTempo;
        private NumericUpDown numTempo;
        private Label lblChordsUrl;
        private TextBox txtChordsUrl;
        private Label lblYoutubeUrl;
        private TextBox txtYoutubeUrl;
        private Label lblLiederbuchnummer;
        private NumericUpDown numLiederbuchnummer;
        private Label lblLiederbuchseite;
        private NumericUpDown numLiederbuchseite;
        private FlowLayoutPanel pnlButtons;
        private Button btnSave;
        private Button btnCancel;
    }
}