namespace SolutionLauncher
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.FadeTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stripeVSSBindingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAllFilesNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.getFromSourceControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // FadeTimer
            // 
            this.FadeTimer.Tick += new System.EventHandler(this.FadeTimer_Tick);
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.Color.SteelBlue;
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(354, 24);
            this.menuStripMain.TabIndex = 1;
            this.menuStripMain.Text = "menuStripMain";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripeVSSBindingsToolStripMenuItem,
            this.markAllFilesNormalToolStripMenuItem,
            this.getFromSourceControlToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // stripeVSSBindingsToolStripMenuItem
            // 
            this.stripeVSSBindingsToolStripMenuItem.Name = "stripeVSSBindingsToolStripMenuItem";
            this.stripeVSSBindingsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.stripeVSSBindingsToolStripMenuItem.Text = "stripe &VSS bindings";
            this.stripeVSSBindingsToolStripMenuItem.Click += new System.EventHandler(this.stripeVSSBindingsToolStripMenuItem_Click);
            // 
            // markAllFilesNormalToolStripMenuItem
            // 
            this.markAllFilesNormalToolStripMenuItem.Name = "markAllFilesNormalToolStripMenuItem";
            this.markAllFilesNormalToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.markAllFilesNormalToolStripMenuItem.Text = "mark &All files Writable";
            this.markAllFilesNormalToolStripMenuItem.Click += new System.EventHandler(this.markAllFilesNormalToolStripMenuItem_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.BackColor = System.Drawing.Color.SteelBlue;
            this.statusStripMain.Location = new System.Drawing.Point(0, 381);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(354, 22);
            this.statusStripMain.TabIndex = 0;
            this.statusStripMain.Text = "statusStripMain";
            // 
            // getFromSourceControlToolStripMenuItem
            // 
            this.getFromSourceControlToolStripMenuItem.Name = "getFromSourceControlToolStripMenuItem";
            this.getFromSourceControlToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.getFromSourceControlToolStripMenuItem.Text = "get from &Source Control";
            this.getFromSourceControlToolStripMenuItem.Click += new System.EventHandler(this.getFromSourceControlToolStripMenuItem_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(354, 403);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.menuStripMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainFrm";
            this.Opacity = 0;
            this.Text = "Solution Launcher";
            this.TopMost = true;
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupLauncher;
        private System.Windows.Forms.GroupBox groupSDKs;

        private void aKeyPressEvent(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // If Enter is Pressed we just launch
            if (e.KeyChar == (char)System.Windows.Forms.Keys.Return)
            {
                m_groupLauncher.ClickLaunchButton();
            }
        }

        // main private classes
        private groupSDKs m_groupSDKs;
        private groupLauncher m_groupLauncher;
        private System.Windows.Forms.Timer FadeTimer;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stripeVSSBindingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAllFilesNormalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getFromSourceControlToolStripMenuItem;
    }
}