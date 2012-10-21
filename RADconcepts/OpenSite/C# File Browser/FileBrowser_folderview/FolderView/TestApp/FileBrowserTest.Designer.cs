namespace TestApp
{
    partial class FileBrowserTest
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Local Disk (C:)", 37, 38, new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Local Disk (D:)", 37, 38, new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("DVD-RW Drive (E:)", 41, 42, new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("DVD Drive (F:)", 43, 44, new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("DVD Drive (G:)", 43, 44, new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Removable Disk (H:)", 45, 46, new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Removable Disk (I:)", 45, 46, new System.Windows.Forms.TreeNode[] {
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Removable Disk (J:)", 45, 46, new System.Windows.Forms.TreeNode[] {
            treeNode15});
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Removable Disk (K:)", 45, 46, new System.Windows.Forms.TreeNode[] {
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Control Panel", 19, 20, new System.Windows.Forms.TreeNode[] {
            treeNode19});
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Mijn Gedeelde mappen", 27, 28, new System.Windows.Forms.TreeNode[] {
            treeNode21});
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Mobile Device", 23, 24, new System.Windows.Forms.TreeNode[] {
            treeNode23});
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("My Logitech Pictures", 21, 22);
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Logitech QuickCam Express ", 31, 36);
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Shared Documents", 16, 18, new System.Windows.Forms.TreeNode[] {
            treeNode27});
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Steven Roebert\'s Documents", 29, 30, new System.Windows.Forms.TreeNode[] {
            treeNode29});
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("My Computer", 7, 8, new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode4,
            treeNode6,
            treeNode8,
            treeNode10,
            treeNode12,
            treeNode14,
            treeNode16,
            treeNode18,
            treeNode20,
            treeNode22,
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode28,
            treeNode30});
            this.shellBrowser = new ShellDll.ShellBrowser();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.helpInfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.seperator1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentDirInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileBrowser1 = new FileBrowser.Browser();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpInfoLabel,
            this.seperator1,
            this.currentDirInfo});
            this.statusStrip.Location = new System.Drawing.Point(0, 477);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(326, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // helpInfoLabel
            // 
            this.helpInfoLabel.Name = "helpInfoLabel";
            this.helpInfoLabel.Size = new System.Drawing.Size(307, 17);
            this.helpInfoLabel.Spring = true;
            this.helpInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // seperator1
            // 
            this.seperator1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.seperator1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.seperator1.Name = "seperator1";
            this.seperator1.Size = new System.Drawing.Size(4, 17);
            // 
            // currentDirInfo
            // 
            this.currentDirInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.currentDirInfo.Name = "currentDirInfo";
            this.currentDirInfo.Size = new System.Drawing.Size(0, 17);
            this.currentDirInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileBrowser1
            // 
            this.fileBrowser1.AllowDrop = true;
            this.fileBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileBrowser1.Location = new System.Drawing.Point(0, 0);
            this.fileBrowser1.MinimumSize = new System.Drawing.Size(300, 200);
            this.fileBrowser1.Name = "fileBrowser1";
            treeNode1.Name = "";
            treeNode1.Text = "";
            treeNode2.ImageIndex = 37;
            treeNode2.Name = "Local Disk (C:)";
            treeNode2.SelectedImageIndex = 38;
            treeNode2.Text = "Local Disk (C:)";
            treeNode3.Name = "";
            treeNode3.Text = "";
            treeNode4.ImageIndex = 37;
            treeNode4.Name = "Local Disk (D:)";
            treeNode4.SelectedImageIndex = 38;
            treeNode4.Text = "Local Disk (D:)";
            treeNode5.Name = "";
            treeNode5.Text = "";
            treeNode6.ImageIndex = 41;
            treeNode6.Name = "DVD-RW Drive (E:)";
            treeNode6.SelectedImageIndex = 42;
            treeNode6.Text = "DVD-RW Drive (E:)";
            treeNode7.Name = "";
            treeNode7.Text = "";
            treeNode8.ImageIndex = 43;
            treeNode8.Name = "DVD Drive (F:)";
            treeNode8.SelectedImageIndex = 44;
            treeNode8.Text = "DVD Drive (F:)";
            treeNode9.Name = "";
            treeNode9.Text = "";
            treeNode10.ImageIndex = 43;
            treeNode10.Name = "DVD Drive (G:)";
            treeNode10.SelectedImageIndex = 44;
            treeNode10.Text = "DVD Drive (G:)";
            treeNode11.Name = "";
            treeNode11.Text = "";
            treeNode12.ImageIndex = 45;
            treeNode12.Name = "Removable Disk (H:)";
            treeNode12.SelectedImageIndex = 46;
            treeNode12.Text = "Removable Disk (H:)";
            treeNode13.Name = "";
            treeNode13.Text = "";
            treeNode14.ImageIndex = 45;
            treeNode14.Name = "Removable Disk (I:)";
            treeNode14.SelectedImageIndex = 46;
            treeNode14.Text = "Removable Disk (I:)";
            treeNode15.Name = "";
            treeNode15.Text = "";
            treeNode16.ImageIndex = 45;
            treeNode16.Name = "Removable Disk (J:)";
            treeNode16.SelectedImageIndex = 46;
            treeNode16.Text = "Removable Disk (J:)";
            treeNode17.Name = "";
            treeNode17.Text = "";
            treeNode18.ImageIndex = 45;
            treeNode18.Name = "Removable Disk (K:)";
            treeNode18.SelectedImageIndex = 46;
            treeNode18.Text = "Removable Disk (K:)";
            treeNode19.Name = "";
            treeNode19.Text = "";
            treeNode20.ImageIndex = 19;
            treeNode20.Name = "Control Panel";
            treeNode20.SelectedImageIndex = 20;
            treeNode20.Text = "Control Panel";
            treeNode21.Name = "";
            treeNode21.Text = "";
            treeNode22.ImageIndex = 27;
            treeNode22.Name = "Mijn Gedeelde mappen";
            treeNode22.SelectedImageIndex = 28;
            treeNode22.Text = "Mijn Gedeelde mappen";
            treeNode23.Name = "";
            treeNode23.Text = "";
            treeNode24.ImageIndex = 23;
            treeNode24.Name = "Mobile Device";
            treeNode24.SelectedImageIndex = 24;
            treeNode24.Text = "Mobile Device";
            treeNode25.ImageIndex = 21;
            treeNode25.Name = "My Logitech Pictures";
            treeNode25.SelectedImageIndex = 22;
            treeNode25.Text = "My Logitech Pictures";
            treeNode26.ImageIndex = 31;
            treeNode26.Name = "Logitech QuickCam Express ";
            treeNode26.SelectedImageIndex = 36;
            treeNode26.Text = "Logitech QuickCam Express ";
            treeNode27.Name = "";
            treeNode27.Text = "";
            treeNode28.ImageIndex = 16;
            treeNode28.Name = "Shared Documents";
            treeNode28.SelectedImageIndex = 18;
            treeNode28.Text = "Shared Documents";
            treeNode29.Name = "";
            treeNode29.Text = "";
            treeNode30.ImageIndex = 29;
            treeNode30.Name = "Steven Roebert\'s Documents";
            treeNode30.SelectedImageIndex = 30;
            treeNode30.Text = "Steven Roebert\'s Documents";
            treeNode31.ImageIndex = 7;
            treeNode31.Name = "My Computer";
            treeNode31.SelectedImageIndex = 8;
            treeNode31.Text = "My Computer";
            this.fileBrowser1.SelectedNode = treeNode31;
            this.fileBrowser1.ShellBrowser = this.shellBrowser;
            this.fileBrowser1.Size = new System.Drawing.Size(326, 477);
            this.fileBrowser1.TabIndex = 2;
            this.fileBrowser1.ContextMenuMouseHover += new FileBrowser.ContextMenuMouseHoverEventHandler(this.fileBrowser_ContextMenuMouseHover);
            this.fileBrowser1.SelectedFolderChanged += new FileBrowser.SelectedFolderChangedEventHandler(this.fileBrowser_SelectedFolderChanged);
            // 
            // FileBrowserTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 499);
            this.Controls.Add(this.fileBrowser1);
            this.Controls.Add(this.statusStrip);
            this.Name = "FileBrowserTest";
            this.Text = "FileBrowser Test";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel helpInfoLabel;
        private System.Windows.Forms.ToolStripStatusLabel currentDirInfo;
        private System.Windows.Forms.ToolStripStatusLabel seperator1;
        private ShellDll.ShellBrowser shellBrowser;
        private FileBrowser.Browser fileBrowser1;
    }
}

