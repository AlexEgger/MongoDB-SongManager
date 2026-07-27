namespace SongManager.Views
{
    partial class StatisticsView
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
            mainLayout = new TableLayoutPanel();
            pnlHeader = new Panel();
            txtSearch = new TextBox();
            lblSearch = new Label();
            cmbSongSelector = new ComboBox();
            lblSongSelect = new Label();
            lblUserSetlists = new Label();
            lblTotalSetlists = new Label();
            lblTotalSongs = new Label();
            formsPlotRatings = new ScottPlot.WinForms.FormsPlot();
            mainLayout.SuspendLayout();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(pnlHeader, 0, 0);
            mainLayout.Controls.Add(formsPlotRatings, 0, 1);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Size = new Size(850, 550);
            mainLayout.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(240, 243, 246);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(lblSearch);
            pnlHeader.Controls.Add(cmbSongSelector);
            pnlHeader.Controls.Add(lblSongSelect);
            pnlHeader.Controls.Add(lblUserSetlists);
            pnlHeader.Controls.Add(lblTotalSetlists);
            pnlHeader.Controls.Add(lblTotalSongs);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(5, 5);
            pnlHeader.Margin = new Padding(5);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(840, 65);
            pnlHeader.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.Location = new Point(70, 35);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Title or Artist...";
            txtSearch.Size = new Size(160, 23);
            txtSearch.TabIndex = 6;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSearch.ForeColor = Color.FromArgb(40, 40, 40);
            lblSearch.Location = new Point(12, 38);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(57, 15);
            lblSearch.TabIndex = 5;
            lblSearch.Text = "🔍 Search:";
            // 
            // cmbSongSelector
            // 
            cmbSongSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSongSelector.Font = new Font("Segoe UI", 9F);
            cmbSongSelector.FormattingEnabled = true;
            cmbSongSelector.Location = new Point(340, 35);
            cmbSongSelector.Name = "cmbSongSelector";
            cmbSongSelector.Size = new Size(300, 23);
            cmbSongSelector.TabIndex = 4;
            // 
            // lblSongSelect
            // 
            lblSongSelect.AutoSize = true;
            lblSongSelect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSongSelect.ForeColor = Color.FromArgb(40, 40, 40);
            lblSongSelect.Location = new Point(245, 38);
            lblSongSelect.Name = "lblSongSelect";
            lblSongSelect.Size = new Size(91, 15);
            lblSongSelect.TabIndex = 3;
            lblSongSelect.Text = "Select Song:";
            // 
            // lblUserSetlists
            // 
            lblUserSetlists.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblUserSetlists.ForeColor = Color.FromArgb(40, 40, 40);
            lblUserSetlists.Location = new Point(640, 8);
            lblUserSetlists.Name = "lblUserSetlists";
            lblUserSetlists.Size = new Size(180, 25);
            lblUserSetlists.TabIndex = 2;
            lblUserSetlists.Text = "👤 My Setlists: 0";
            lblUserSetlists.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalSetlists
            // 
            lblTotalSetlists.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTotalSetlists.ForeColor = Color.FromArgb(40, 40, 40);
            lblTotalSetlists.Location = new Point(330, 8);
            lblTotalSetlists.Name = "lblTotalSetlists";
            lblTotalSetlists.Size = new Size(180, 25);
            lblTotalSetlists.TabIndex = 1;
            lblTotalSetlists.Text = "📋 Total Setlists: 0";
            lblTotalSetlists.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalSongs
            // 
            lblTotalSongs.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTotalSongs.ForeColor = Color.FromArgb(40, 40, 40);
            lblTotalSongs.Location = new Point(12, 8);
            lblTotalSongs.Name = "lblTotalSongs";
            lblTotalSongs.Size = new Size(200, 25);
            lblTotalSongs.TabIndex = 0;
            lblTotalSongs.Text = "🎵 Total Songs: 0";
            lblTotalSongs.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // formsPlotRatings
            // 
            formsPlotRatings.DisplayScale = 1F;
            formsPlotRatings.Dock = DockStyle.Fill;
            formsPlotRatings.Location = new Point(3, 78);
            formsPlotRatings.Name = "formsPlotRatings";
            formsPlotRatings.Size = new Size(844, 469);
            formsPlotRatings.TabIndex = 1;
            // 
            // StatisticsView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainLayout);
            Name = "StatisticsView";
            Size = new Size(850, 550);
            mainLayout.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainLayout;
        private Panel pnlHeader;
        private Label lblTotalSongs;
        private Label lblTotalSetlists;
        private Label lblUserSetlists;
        private Label lblSearch;
        private TextBox txtSearch;
        private ComboBox cmbSongSelector;
        private Label lblSongSelect;
        private ScottPlot.WinForms.FormsPlot formsPlotRatings;
    }
}