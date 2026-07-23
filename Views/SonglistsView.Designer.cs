namespace SongManager.Views
{
    partial class SonglistsView
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

        #region Component Designer generated code

        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lstSetlists = new System.Windows.Forms.ListBox();
            this.cmsSetlists = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRenameList = new System.Windows.Forms.ToolStripMenuItem();
            this.chkMyListsOnly = new System.Windows.Forms.CheckBox();
            this.lblSetlistHeader = new System.Windows.Forms.Label();
            this.pnlLeftButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnCreateList = new System.Windows.Forms.Button();
            this.btnDeleteList = new System.Windows.Forms.Button();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.builderLayout = new System.Windows.Forms.TableLayoutPanel();
            this.dgvAvailableSongs = new System.Windows.Forms.DataGridView();
            this.colAvailTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvailArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvailBook = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSonglistSongs = new System.Windows.Forms.DataGridView();
            this.colPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTransferButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddSongToList = new System.Windows.Forms.Button();
            this.btnRemoveSongFromList = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.pnlAvailHeader = new System.Windows.Forms.Panel();
            this.txtSearchAvailable = new System.Windows.Forms.TextBox();
            this.lblAvailHeader = new System.Windows.Forms.Label();
            this.lblSetlistSongsHeader = new System.Windows.Forms.Label();
            this.pnlTopHeader = new System.Windows.Forms.Panel();
            this.lblActiveSetlistTitle = new System.Windows.Forms.Label();
            this.mainLayout.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.cmsSetlists.SuspendLayout();
            this.pnlLeftButtons.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.builderLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableSongs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSonglistSongs)).BeginInit();
            this.pnlTransferButtons.SuspendLayout();
            this.pnlAvailHeader.SuspendLayout();
            this.pnlTopHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.mainLayout.Controls.Add(this.pnlLeft, 0, 0);
            this.mainLayout.Controls.Add(this.pnlRight, 1, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 1;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(950, 550);
            this.mainLayout.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lstSetlists);
            this.pnlLeft.Controls.Add(this.chkMyListsOnly);
            this.pnlLeft.Controls.Add(this.lblSetlistHeader);
            this.pnlLeft.Controls.Add(this.pnlLeftButtons);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(4, 4);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(5);
            this.pnlLeft.Size = new System.Drawing.Size(230, 542);
            this.pnlLeft.TabIndex = 0;
            // 
            // lstSetlists
            // 
            this.lstSetlists.ContextMenuStrip = this.cmsSetlists;
            this.lstSetlists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSetlists.FormattingEnabled = true;
            this.lstSetlists.ItemHeight = 15;
            this.lstSetlists.Location = new System.Drawing.Point(5, 55);
            this.lstSetlists.Name = "lstSetlists";
            this.lstSetlists.Size = new System.Drawing.Size(220, 442);
            this.lstSetlists.TabIndex = 0;
            // 
            // cmsSetlists
            // 
            this.cmsSetlists.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRenameList});
            this.cmsSetlists.Name = "cmsSetlists";
            this.cmsSetlists.Size = new System.Drawing.Size(181, 48);
            // 
            // tsmiRenameList
            // 
            this.tsmiRenameList.Name = "tsmiRenameList";
            this.tsmiRenameList.Size = new System.Drawing.Size(180, 22);
            this.tsmiRenameList.Text = "Rename Setlist";
            // 
            // chkMyListsOnly
            // 
            this.chkMyListsOnly.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkMyListsOnly.Location = new System.Drawing.Point(5, 30);
            this.chkMyListsOnly.Name = "chkMyListsOnly";
            this.chkMyListsOnly.Size = new System.Drawing.Size(220, 25);
            this.chkMyListsOnly.TabIndex = 3;
            this.chkMyListsOnly.Text = "👤 Show only my setlists";
            this.chkMyListsOnly.UseVisualStyleBackColor = true;
            // 
            // lblSetlistHeader
            // 
            this.lblSetlistHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSetlistHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSetlistHeader.Location = new System.Drawing.Point(5, 5);
            this.lblSetlistHeader.Name = "lblSetlistHeader";
            this.lblSetlistHeader.Size = new System.Drawing.Size(220, 25);
            this.lblSetlistHeader.TabIndex = 1;
            this.lblSetlistHeader.Text = "Songlists / Setlists";
            this.lblSetlistHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlLeftButtons
            // 
            this.pnlLeftButtons.ColumnCount = 2;
            this.pnlLeftButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLeftButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLeftButtons.Controls.Add(this.btnCreateList, 0, 0);
            this.pnlLeftButtons.Controls.Add(this.btnDeleteList, 1, 0);
            this.pnlLeftButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLeftButtons.Location = new System.Drawing.Point(5, 497);
            this.pnlLeftButtons.Name = "pnlLeftButtons";
            this.pnlLeftButtons.RowCount = 1;
            this.pnlLeftButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLeftButtons.Size = new System.Drawing.Size(220, 40);
            this.pnlLeftButtons.TabIndex = 2;
            // 
            // btnCreateList
            // 
            this.btnCreateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateList.Location = new System.Drawing.Point(0, 0);
            this.btnCreateList.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.btnCreateList.Name = "btnCreateList";
            this.btnCreateList.Size = new System.Drawing.Size(108, 40);
            this.btnCreateList.TabIndex = 0;
            this.btnCreateList.Text = "+ New";
            this.btnCreateList.UseVisualStyleBackColor = true;
            // 
            // btnDeleteList
            // 
            this.btnDeleteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteList.Location = new System.Drawing.Point(112, 0);
            this.btnDeleteList.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.btnDeleteList.Name = "btnDeleteList";
            this.btnDeleteList.Size = new System.Drawing.Size(108, 40);
            this.btnDeleteList.TabIndex = 1;
            this.btnDeleteList.Text = "🗑️ Delete";
            this.btnDeleteList.UseVisualStyleBackColor = true;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.builderLayout);
            this.pnlRight.Controls.Add(this.pnlTopHeader);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(241, 4);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(5);
            this.pnlRight.Size = new System.Drawing.Size(705, 542);
            this.pnlRight.TabIndex = 1;
            // 
            // builderLayout
            // 
            this.builderLayout.ColumnCount = 3;
            this.builderLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.builderLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.builderLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.builderLayout.Controls.Add(this.dgvAvailableSongs, 0, 1);
            this.builderLayout.Controls.Add(this.dgvSonglistSongs, 2, 1);
            this.builderLayout.Controls.Add(this.pnlTransferButtons, 1, 1);
            this.builderLayout.Controls.Add(this.pnlAvailHeader, 0, 0);
            this.builderLayout.Controls.Add(this.lblSetlistSongsHeader, 2, 0);
            this.builderLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.builderLayout.Location = new System.Drawing.Point(5, 40);
            this.builderLayout.Name = "builderLayout";
            this.builderLayout.RowCount = 2;
            this.builderLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.builderLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.builderLayout.Size = new System.Drawing.Size(695, 497);
            this.builderLayout.TabIndex = 1;
            // 
            // dgvAvailableSongs
            // 
            this.dgvAvailableSongs.AllowUserToAddRows = false;
            this.dgvAvailableSongs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAvailableSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailableSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAvailTitle,
            this.colAvailArtist,
            this.colAvailBook});
            this.dgvAvailableSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAvailableSongs.Location = new System.Drawing.Point(3, 33);
            this.dgvAvailableSongs.MultiSelect = false;
            this.dgvAvailableSongs.Name = "dgvAvailableSongs";
            this.dgvAvailableSongs.ReadOnly = true;
            this.dgvAvailableSongs.RowHeadersVisible = false;
            this.dgvAvailableSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvailableSongs.Size = new System.Drawing.Size(299, 461);
            this.dgvAvailableSongs.TabIndex = 0;
            // 
            // colAvailTitle
            // 
            this.colAvailTitle.HeaderText = "Title";
            this.colAvailTitle.Name = "colAvailTitle";
            this.colAvailTitle.ReadOnly = true;
            // 
            // colAvailArtist
            // 
            this.colAvailArtist.HeaderText = "Artist";
            this.colAvailArtist.Name = "colAvailArtist";
            this.colAvailArtist.ReadOnly = true;
            // 
            // colAvailBook
            // 
            this.colAvailBook.HeaderText = "Songbook";
            this.colAvailBook.Name = "colAvailBook";
            this.colAvailBook.ReadOnly = true;
            // 
            // dgvSonglistSongs
            // 
            this.dgvSonglistSongs.AllowUserToAddRows = false;
            this.dgvSonglistSongs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSonglistSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSonglistSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPos,
            this.colSetTitle,
            this.colSetArtist});
            this.dgvSonglistSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSonglistSongs.Location = new System.Drawing.Point(391, 33);
            this.dgvSonglistSongs.MultiSelect = false;
            this.dgvSonglistSongs.Name = "dgvSonglistSongs";
            this.dgvSonglistSongs.ReadOnly = true;
            this.dgvSonglistSongs.RowHeadersVisible = false;
            this.dgvSonglistSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSonglistSongs.Size = new System.Drawing.Size(301, 461);
            this.dgvSonglistSongs.TabIndex = 1;
            // 
            // colPos
            // 
            this.colPos.FillWeight = 25F;
            this.colPos.HeaderText = "#";
            this.colPos.Name = "colPos";
            this.colPos.ReadOnly = true;
            // 
            // colSetTitle
            // 
            this.colSetTitle.HeaderText = "Title in Setlist";
            this.colSetTitle.Name = "colSetTitle";
            this.colSetTitle.ReadOnly = true;
            // 
            // colSetArtist
            // 
            this.colSetArtist.HeaderText = "Artist";
            this.colSetArtist.Name = "colSetArtist";
            this.colSetArtist.ReadOnly = true;
            // 
            // pnlTransferButtons
            // 
            this.pnlTransferButtons.Controls.Add(this.btnAddSongToList);
            this.pnlTransferButtons.Controls.Add(this.btnRemoveSongFromList);
            this.pnlTransferButtons.Controls.Add(this.btnMoveUp);
            this.pnlTransferButtons.Controls.Add(this.btnMoveDown);
            this.pnlTransferButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTransferButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlTransferButtons.Location = new System.Drawing.Point(308, 33);
            this.pnlTransferButtons.Name = "pnlTransferButtons";
            this.pnlTransferButtons.Padding = new System.Windows.Forms.Padding(5, 40, 5, 0);
            this.pnlTransferButtons.Size = new System.Drawing.Size(77, 461);
            this.pnlTransferButtons.TabIndex = 2;
            // 
            // btnAddSongToList
            // 
            this.btnAddSongToList.Location = new System.Drawing.Point(8, 43);
            this.btnAddSongToList.Name = "btnAddSongToList";
            this.btnAddSongToList.Size = new System.Drawing.Size(62, 35);
            this.btnAddSongToList.TabIndex = 0;
            this.btnAddSongToList.Text = "➕ ➡️";
            this.btnAddSongToList.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSongFromList
            // 
            this.btnRemoveSongFromList.Location = new System.Drawing.Point(8, 84);
            this.btnRemoveSongFromList.Name = "btnRemoveSongFromList";
            this.btnRemoveSongFromList.Size = new System.Drawing.Size(62, 35);
            this.btnRemoveSongFromList.TabIndex = 1;
            this.btnRemoveSongFromList.Text = "⬅️ ❌";
            this.btnRemoveSongFromList.UseVisualStyleBackColor = true;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(8, 145);
            this.btnMoveUp.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(62, 35);
            this.btnMoveUp.TabIndex = 2;
            this.btnMoveUp.Text = "▲ Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(8, 186);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(62, 35);
            this.btnMoveDown.TabIndex = 3;
            this.btnMoveDown.Text = "▼ Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // pnlAvailHeader
            // 
            this.pnlAvailHeader.Controls.Add(this.txtSearchAvailable);
            this.pnlAvailHeader.Controls.Add(this.lblAvailHeader);
            this.pnlAvailHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAvailHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlAvailHeader.Name = "pnlAvailHeader";
            this.pnlAvailHeader.Size = new System.Drawing.Size(305, 30);
            this.pnlAvailHeader.TabIndex = 5;
            // 
            // txtSearchAvailable
            // 
            this.txtSearchAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchAvailable.Location = new System.Drawing.Point(105, 3);
            this.txtSearchAvailable.Name = "txtSearchAvailable";
            this.txtSearchAvailable.PlaceholderText = "🔍 Search...";
            this.txtSearchAvailable.Size = new System.Drawing.Size(197, 23);
            this.txtSearchAvailable.TabIndex = 4;
            // 
            // lblAvailHeader
            // 
            this.lblAvailHeader.AutoSize = true;
            this.lblAvailHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAvailHeader.Location = new System.Drawing.Point(0, 7);
            this.lblAvailHeader.Name = "lblAvailHeader";
            this.lblAvailHeader.Size = new System.Drawing.Size(93, 15);
            this.lblAvailHeader.TabIndex = 3;
            this.lblAvailHeader.Text = "Available Songs";
            // 
            // lblSetlistSongsHeader
            // 
            this.lblSetlistSongsHeader.AutoSize = true;
            this.lblSetlistSongsHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSetlistSongsHeader.Location = new System.Drawing.Point(391, 7);
            this.lblSetlistSongsHeader.Name = "lblSetlistSongsHeader";
            this.lblSetlistSongsHeader.Size = new System.Drawing.Size(123, 15);
            this.lblSetlistSongsHeader.TabIndex = 4;
            this.lblSetlistSongsHeader.Text = "Songs in this Setlist";
            // 
            // pnlTopHeader
            // 
            this.pnlTopHeader.Controls.Add(this.lblActiveSetlistTitle);
            this.pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHeader.Location = new System.Drawing.Point(5, 5);
            this.pnlTopHeader.Name = "pnlTopHeader";
            this.pnlTopHeader.Size = new System.Drawing.Size(695, 35);
            this.pnlTopHeader.TabIndex = 0;
            // 
            // lblActiveSetlistTitle
            // 
            this.lblActiveSetlistTitle.AutoSize = true;
            this.lblActiveSetlistTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblActiveSetlistTitle.Location = new System.Drawing.Point(3, 5);
            this.lblActiveSetlistTitle.Name = "lblActiveSetlistTitle";
            this.lblActiveSetlistTitle.Size = new System.Drawing.Size(130, 20);
            this.lblActiveSetlistTitle.TabIndex = 0;
            this.lblActiveSetlistTitle.Text = "No setlist active";
            // 
            // SonglistsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayout);
            this.Name = "SonglistsView";
            this.Size = new System.Drawing.Size(950, 550);
            this.mainLayout.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.cmsSetlists.ResumeLayout(false);
            this.pnlLeftButtons.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.builderLayout.ResumeLayout(false);
            this.builderLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableSongs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSonglistSongs)).EndInit();
            this.pnlTransferButtons.ResumeLayout(false);
            this.pnlAvailHeader.ResumeLayout(false);
            this.pnlAvailHeader.PerformLayout();
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlTopHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ListBox lstSetlists;
        private System.Windows.Forms.ContextMenuStrip cmsSetlists;
        private System.Windows.Forms.ToolStripMenuItem tsmiRenameList;
        private System.Windows.Forms.CheckBox chkMyListsOnly;
        private System.Windows.Forms.Label lblSetlistHeader;
        private System.Windows.Forms.TableLayoutPanel pnlLeftButtons;
        private System.Windows.Forms.Button btnCreateList;
        private System.Windows.Forms.Button btnDeleteList;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblActiveSetlistTitle;
        private System.Windows.Forms.TableLayoutPanel builderLayout;
        private System.Windows.Forms.DataGridView dgvAvailableSongs;
        private System.Windows.Forms.DataGridView dgvSonglistSongs;
        private System.Windows.Forms.FlowLayoutPanel pnlTransferButtons;
        private System.Windows.Forms.Button btnAddSongToList;
        private System.Windows.Forms.Button btnRemoveSongFromList;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Panel pnlAvailHeader;
        private System.Windows.Forms.Label lblAvailHeader;
        private System.Windows.Forms.TextBox txtSearchAvailable;
        private System.Windows.Forms.Label lblSetlistSongsHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvailTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvailArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvailBook;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetArtist;
    }
}