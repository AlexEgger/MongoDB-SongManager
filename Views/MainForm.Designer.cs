using SongManager.Views;

namespace MongoDB_SongManager.Views
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            tabControlMain = new TabControl();
            tabPageSongs = new TabPage();
            tabPage2 = new TabPage();
            songsView1 = new SongsView();
            tabControlMain.SuspendLayout();
            tabPageSongs.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageSongs);
            tabControlMain.Controls.Add(tabPage2);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 0);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(800, 450);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageSongs
            // 
            tabPageSongs.Controls.Add(songsView1);
            tabPageSongs.Location = new Point(4, 24);
            tabPageSongs.Name = "tabPageSongs";
            tabPageSongs.Padding = new Padding(3);
            tabPageSongs.Size = new Size(792, 422);
            tabPageSongs.TabIndex = 0;
            tabPageSongs.Text = "Songs and Songlists";
            tabPageSongs.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 422);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // songsView1
            // 
            songsView1.Dock = DockStyle.Fill;
            songsView1.Location = new Point(3, 3);
            songsView1.Name = "songsView1";
            songsView1.Size = new Size(786, 416);
            songsView1.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControlMain);
            Name = "MainForm";
            Text = "Form1";
            tabControlMain.ResumeLayout(false);
            tabPageSongs.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControlMain;
        private TabPage tabPageSongs;
        private TabPage tabPage2;
        private SongsView songsView1;
    }
}
