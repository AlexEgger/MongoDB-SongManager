using System.Drawing;
using System.Windows.Forms;

namespace SongManager.Views
{
    /// <summary>
    /// Designer partial class for <see cref="SongsView"/> containing UI control definitions and layout setup.
    /// </summary>
    partial class SongsView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            mainLayout = new TableLayoutPanel();
            pnlLeft = new Panel();
            lstSongLists = new ListBox();
            lblListsHeader = new Label();
            btnCreateList = new Button();
            pnlMiddle = new Panel();
            dgvSongs = new DataGridView();
            colTitle = new DataGridViewTextBoxColumn();
            colArtist = new DataGridViewTextBoxColumn();
            pnlSearch = new Panel();
            txtSearch = new TextBox();
            toolStripActionButtons = new ToolStrip();
            btnAddSong = new ToolStripButton();
            btnAddArtist = new ToolStripButton();
            btnEditSong = new ToolStripButton();
            btnDeleteSong = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnExportCsv = new ToolStripButton();
            btnImportCsv = new ToolStripButton();
            pnlRight = new Panel();
            grpSongDetails = new GroupBox();
            txtNotes = new TextBox();
            lblNotesHeader = new Label();
            lblRatings = new Label();
            lblBookInfo = new Label();
            lnkYoutube = new LinkLabel();
            lnkChords = new LinkLabel();
            lblTempo = new Label();
            lblArtist = new Label();
            lblSongTitle = new Label();
            mainLayout.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSongs).BeginInit();
            pnlSearch.SuspendLayout();
            toolStripActionButtons.SuspendLayout();
            pnlRight.SuspendLayout();
            grpSongDetails.SuspendLayout();
            SuspendLayout();
            // 
            // mainLayout
            // 
            mainLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            mainLayout.ColumnCount = 3;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            mainLayout.Controls.Add(pnlLeft, 0, 0);
            mainLayout.Controls.Add(pnlMiddle, 1, 0);
            mainLayout.Controls.Add(pnlRight, 2, 0);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.RowCount = 1;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Size = new Size(800, 500);
            mainLayout.TabIndex = 0;
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(lstSongLists);
            pnlLeft.Controls.Add(lblListsHeader);
            pnlLeft.Controls.Add(btnCreateList);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Location = new Point(4, 4);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(5);
            pnlLeft.Size = new Size(153, 492);
            pnlLeft.TabIndex = 0;
            // 
            // lstSongLists
            // 
            lstSongLists.Dock = DockStyle.Fill;
            lstSongLists.FormattingEnabled = true;
            lstSongLists.Items.AddRange(new object[] { "🎵 Alle Songs", "────────────────", "Band Setlist 2026", "Akustik Duo" });
            lstSongLists.Location = new Point(5, 35);
            lstSongLists.Name = "lstSongLists";
            lstSongLists.Size = new Size(143, 417);
            lstSongLists.TabIndex = 0;
            // 
            // lblListsHeader
            // 
            lblListsHeader.Dock = DockStyle.Top;
            lblListsHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblListsHeader.Location = new Point(5, 5);
            lblListsHeader.Name = "lblListsHeader";
            lblListsHeader.Size = new Size(143, 30);
            lblListsHeader.TabIndex = 1;
            lblListsHeader.Text = "Meine Listen";
            lblListsHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnCreateList
            // 
            btnCreateList.Dock = DockStyle.Bottom;
            btnCreateList.Location = new Point(5, 452);
            btnCreateList.Name = "btnCreateList";
            btnCreateList.Size = new Size(143, 35);
            btnCreateList.TabIndex = 2;
            btnCreateList.Text = "+ Neue Liste";
            btnCreateList.UseVisualStyleBackColor = true;
            // 
            // pnlMiddle
            // 
            pnlMiddle.Controls.Add(dgvSongs);
            pnlMiddle.Controls.Add(pnlSearch);
            pnlMiddle.Controls.Add(toolStripActionButtons);
            pnlMiddle.Dock = DockStyle.Fill;
            pnlMiddle.Location = new Point(164, 4);
            pnlMiddle.Name = "pnlMiddle";
            pnlMiddle.Padding = new Padding(5);
            pnlMiddle.Size = new Size(407, 492);
            pnlMiddle.TabIndex = 1;
            // 
            // dgvSongs
            // 
            dgvSongs.AllowUserToAddRows = false;
            dgvSongs.ColumnHeadersHeight = 35;
            dgvSongs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSongs.Columns.AddRange(new DataGridViewColumn[] { colTitle, colArtist });
            dgvSongs.Dock = DockStyle.Fill;
            dgvSongs.Location = new Point(5, 40);
            dgvSongs.MultiSelect = false;
            dgvSongs.Name = "dgvSongs";
            dgvSongs.ReadOnly = true;
            dgvSongs.RowHeadersVisible = false;
            dgvSongs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSongs.Size = new Size(397, 422);
            dgvSongs.TabIndex = 0;
            // 
            // colTitle
            // 
            colTitle.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTitle.FillWeight = 50F;
            colTitle.HeaderText = "Titel";
            colTitle.Name = "colTitle";
            colTitle.ReadOnly = true;
            // 
            // colArtist
            // 
            colArtist.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colArtist.FillWeight = 50F;
            colArtist.HeaderText = "Interpret";
            colArtist.Name = "colArtist";
            colArtist.ReadOnly = true;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(5, 5);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(397, 35);
            pnlSearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Suche in Liste...";
            txtSearch.Size = new Size(397, 23);
            txtSearch.TabIndex = 0;
            // 
            // toolStripActionButtons
            // 
            toolStripActionButtons.Dock = DockStyle.Bottom;
            toolStripActionButtons.GripStyle = ToolStripGripStyle.Hidden;
            toolStripActionButtons.Items.AddRange(new ToolStripItem[] { btnAddSong, btnAddArtist, btnEditSong, btnDeleteSong, toolStripSeparator1, btnExportCsv, btnImportCsv });
            toolStripActionButtons.Location = new Point(5, 462);
            toolStripActionButtons.Name = "toolStripActionButtons";
            toolStripActionButtons.Size = new Size(397, 25);
            toolStripActionButtons.TabIndex = 2;
            toolStripActionButtons.Text = "Action Toolbar";
            // 
            // btnAddSong
            // 
            btnAddSong.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnAddSong.Name = "btnAddSong";
            btnAddSong.Size = new Size(49, 22);
            btnAddSong.Text = "+ Song";
            // 
            // btnAddArtist
            // 
            btnAddArtist.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnAddArtist.Name = "btnAddArtist";
            btnAddArtist.Size = new Size(67, 22);
            btnAddArtist.Text = "+ Interpret";
            // 
            // btnEditSong
            // 
            btnEditSong.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnEditSong.Name = "btnEditSong";
            btnEditSong.Size = new Size(67, 22);
            btnEditSong.Text = "Bearbeiten";
            // 
            // btnDeleteSong
            // 
            btnDeleteSong.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnDeleteSong.Name = "btnDeleteSong";
            btnDeleteSong.Size = new Size(55, 22);
            btnDeleteSong.Text = "Löschen";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // btnExportCsv
            // 
            btnExportCsv.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new Size(60, 22);
            btnExportCsv.Text = "📥 Export";
            // 
            // btnImportCsv
            // 
            btnImportCsv.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnImportCsv.Name = "btnImportCsv";
            btnImportCsv.Size = new Size(62, 22);
            btnImportCsv.Text = "📤 Import";
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(grpSongDetails);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(578, 4);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(5);
            pnlRight.Size = new Size(218, 492);
            pnlRight.TabIndex = 2;
            // 
            // grpSongDetails
            // 
            grpSongDetails.Controls.Add(txtNotes);
            grpSongDetails.Controls.Add(lblNotesHeader);
            grpSongDetails.Controls.Add(lblRatings);
            grpSongDetails.Controls.Add(lblBookInfo);
            grpSongDetails.Controls.Add(lnkYoutube);
            grpSongDetails.Controls.Add(lnkChords);
            grpSongDetails.Controls.Add(lblTempo);
            grpSongDetails.Controls.Add(lblArtist);
            grpSongDetails.Controls.Add(lblSongTitle);
            grpSongDetails.Dock = DockStyle.Fill;
            grpSongDetails.Location = new Point(5, 5);
            grpSongDetails.Name = "grpSongDetails";
            grpSongDetails.Padding = new Padding(10);
            grpSongDetails.Size = new Size(208, 482);
            grpSongDetails.TabIndex = 0;
            grpSongDetails.TabStop = false;
            grpSongDetails.Text = "Song Details";
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(10, 251);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ReadOnly = true;
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(188, 221);
            txtNotes.TabIndex = 8;
            // 
            // lblNotesHeader
            // 
            lblNotesHeader.Dock = DockStyle.Top;
            lblNotesHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNotesHeader.Location = new Point(10, 226);
            lblNotesHeader.Name = "lblNotesHeader";
            lblNotesHeader.Size = new Size(188, 25);
            lblNotesHeader.TabIndex = 7;
            lblNotesHeader.Text = "Kommentare & Notizen:";
            // 
            // lblRatings
            // 
            lblRatings.Dock = DockStyle.Top;
            lblRatings.Location = new Point(10, 176);
            lblRatings.Name = "lblRatings";
            lblRatings.Size = new Size(188, 50);
            lblRatings.TabIndex = 6;
            lblRatings.Text = "Bewertungen:\n- Keine -";
            // 
            // lblBookInfo
            // 
            lblBookInfo.Dock = DockStyle.Top;
            lblBookInfo.Location = new Point(10, 151);
            lblBookInfo.Name = "lblBookInfo";
            lblBookInfo.Size = new Size(188, 25);
            lblBookInfo.TabIndex = 5;
            lblBookInfo.Text = "Liederbuch: -";
            // 
            // lnkYoutube
            // 
            lnkYoutube.Dock = DockStyle.Top;
            lnkYoutube.Location = new Point(10, 126);
            lnkYoutube.Name = "lnkYoutube";
            lnkYoutube.Size = new Size(188, 25);
            lnkYoutube.TabIndex = 4;
            lnkYoutube.TabStop = true;
            lnkYoutube.Text = "▶️ YouTube Video";
            // 
            // lnkChords
            // 
            lnkChords.Dock = DockStyle.Top;
            lnkChords.Location = new Point(10, 101);
            lnkChords.Name = "lnkChords";
            lnkChords.Size = new Size(188, 25);
            lnkChords.TabIndex = 3;
            lnkChords.TabStop = true;
            lnkChords.Text = "🎸 Akkorde öffnen";
            // 
            // lblTempo
            // 
            lblTempo.Dock = DockStyle.Top;
            lblTempo.Location = new Point(10, 76);
            lblTempo.Name = "lblTempo";
            lblTempo.Size = new Size(188, 25);
            lblTempo.TabIndex = 2;
            lblTempo.Text = "Tempo: -";
            // 
            // lblArtist
            // 
            lblArtist.Dock = DockStyle.Top;
            lblArtist.Location = new Point(10, 51);
            lblArtist.Name = "lblArtist";
            lblArtist.Size = new Size(188, 25);
            lblArtist.TabIndex = 1;
            lblArtist.Text = "Interpret: Queen";
            // 
            // lblSongTitle
            // 
            lblSongTitle.Dock = DockStyle.Top;
            lblSongTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSongTitle.Location = new Point(10, 26);
            lblSongTitle.Name = "lblSongTitle";
            lblSongTitle.Size = new Size(188, 25);
            lblSongTitle.TabIndex = 0;
            lblSongTitle.Text = "Titel: Bohemian Rhapsody";
            // 
            // SongsView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainLayout);
            Name = "SongsView";
            Size = new Size(800, 500);
            mainLayout.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            pnlMiddle.ResumeLayout(false);
            pnlMiddle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSongs).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            toolStripActionButtons.ResumeLayout(false);
            toolStripActionButtons.PerformLayout();
            pnlRight.ResumeLayout(false);
            grpSongDetails.ResumeLayout(false);
            grpSongDetails.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainLayout;
        private Panel pnlLeft;
        private Label lblListsHeader;
        private ListBox lstSongLists;
        private Button btnCreateList;
        private Panel pnlMiddle;
        private Panel pnlSearch;
        private TextBox txtSearch;
        private DataGridView dgvSongs;
        private DataGridViewTextBoxColumn colTitle;
        private DataGridViewTextBoxColumn colArtist;
        private ToolStrip toolStripActionButtons;
        private ToolStripButton btnAddSong;
        private ToolStripButton btnAddArtist;
        private ToolStripButton btnEditSong;
        private ToolStripButton btnDeleteSong;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnExportCsv;
        private ToolStripButton btnImportCsv;
        private Panel pnlRight;
        private GroupBox grpSongDetails;
        private Label lblSongTitle;
        private Label lblArtist;
        private Label lblTempo;
        private LinkLabel lnkChords;
        private LinkLabel lnkYoutube;
        private Label lblBookInfo;
        private Label lblRatings;
        private Label lblNotesHeader;
        private TextBox txtNotes;
    }
}