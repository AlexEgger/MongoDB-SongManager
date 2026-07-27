namespace SongManager.Views
{
    partial class SongsView
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

        #region Vom Komponenten-Designer generierter Code

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
            btnFilterFavorites = new Button();
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
            lstSongLists.Items.AddRange(new object[] { "⭐ Meine Favoriten", "🎵 Alle Songs", "────────────────", "Band Setlist 2026", "Akustik Duo" });
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
            dgvSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSongs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSongs.Columns.AddRange(new DataGridViewColumn[] { colTitle, colArtist });
            dgvSongs.Dock = DockStyle.Fill;
            dgvSongs.Location = new Point(5, 40);
            dgvSongs.MultiSelect = false;
            dgvSongs.Name = "dgvSongs";
            dgvSongs.ReadOnly = true;
            dgvSongs.RowHeadersVisible = false;
            dgvSongs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSongs.Size = new Size(397, 412);
            dgvSongs.TabIndex = 0;
            // 
            // colTitle
            // 
            colTitle.HeaderText = "Titel";
            colTitle.Name = "colTitle";
            colTitle.ReadOnly = true;
            // 
            // colArtist
            // 
            colArtist.HeaderText = "Interpret";
            colArtist.Name = "colArtist";
            colArtist.ReadOnly = true;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnFilterFavorites);
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
            txtSearch.Size = new Size(307, 23);
            txtSearch.TabIndex = 0;
            // 
            // btnFilterFavorites
            // 
            btnFilterFavorites.Dock = DockStyle.Right;
            btnFilterFavorites.Location = new Point(307, 0);
            btnFilterFavorites.Name = "btnFilterFavorites";
            btnFilterFavorites.Size = new Size(90, 35);
            btnFilterFavorites.TabIndex = 1;
            btnFilterFavorites.Text = "⭐ Favoriten";
            btnFilterFavorites.UseVisualStyleBackColor = true;
            // 
            // toolStripActionButtons
            // 
            toolStripActionButtons.CanOverflow = true;
            toolStripActionButtons.Dock = DockStyle.Bottom;
            toolStripActionButtons.GripStyle = ToolStripGripStyle.Hidden;
            toolStripActionButtons.Items.AddRange(new ToolStripItem[] {
            btnAddSong,
            btnAddArtist,
            btnEditSong,
            btnDeleteSong,
            toolStripSeparator1,
            btnExportCsv,
            btnImportCsv});
            toolStripActionButtons.Location = new Point(5, 452);
            toolStripActionButtons.Name = "toolStripActionButtons";
            toolStripActionButtons.Size = new Size(397, 35);
            toolStripActionButtons.TabIndex = 2;
            toolStripActionButtons.Text = "Action Toolbar";
            // 
            // btnAddSong
            // 
            btnAddSong.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnAddSong.Name = "btnAddSong";
            btnAddSong.Overflow = ToolStripItemOverflow.AsNeeded;
            btnAddSong.Size = new Size(54, 32);
            btnAddSong.Text = "+ Song";
            // 
            // btnAddArtist
            // 
            btnAddArtist.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnAddArtist.Name = "btnAddArtist";
            btnAddArtist.Overflow = ToolStripItemOverflow.AsNeeded;
            btnAddArtist.Size = new Size(68, 32);
            btnAddArtist.Text = "+ Interpret";
            // 
            // btnEditSong
            // 
            btnEditSong.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnEditSong.Name = "btnEditSong";
            btnEditSong.Overflow = ToolStripItemOverflow.AsNeeded;
            btnEditSong.Size = new Size(67, 32);
            btnEditSong.Text = "Bearbeiten";
            // 
            // btnDeleteSong
            // 
            btnDeleteSong.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnDeleteSong.Name = "btnDeleteSong";
            btnDeleteSong.Overflow = ToolStripItemOverflow.AsNeeded;
            btnDeleteSong.Size = new Size(56, 32);
            btnDeleteSong.Text = "Löschen";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 35);
            // 
            // btnExportCsv
            // 
            btnExportCsv.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Overflow = ToolStripItemOverflow.AsNeeded;
            btnExportCsv.Size = new Size(60, 32);
            btnExportCsv.Text = "📥 Export";
            // 
            // btnImportCsv
            // 
            btnImportCsv.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnImportCsv.Name = "btnImportCsv";
            btnImportCsv.Overflow = ToolStripItemOverflow.AsNeeded;
            btnImportCsv.Size = new Size(63, 32);
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
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblListsHeader;
        private System.Windows.Forms.ListBox lstSongLists;
        private System.Windows.Forms.Button btnCreateList;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnFilterFavorites;
        private System.Windows.Forms.DataGridView dgvSongs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtist;
        private System.Windows.Forms.ToolStrip toolStripActionButtons;
        private System.Windows.Forms.ToolStripButton btnAddSong;
        private System.Windows.Forms.ToolStripButton btnAddArtist;
        private System.Windows.Forms.ToolStripButton btnEditSong;
        private System.Windows.Forms.ToolStripButton btnDeleteSong;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExportCsv;
        private System.Windows.Forms.ToolStripButton btnImportCsv;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.GroupBox grpSongDetails;
        private System.Windows.Forms.Label lblSongTitle;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.LinkLabel lnkChords;
        private System.Windows.Forms.LinkLabel lnkYoutube;
        private System.Windows.Forms.Label lblBookInfo;
    }
}