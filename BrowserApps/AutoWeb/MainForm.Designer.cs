namespace AutoWeb
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));            
            this.tabControlMain = new System.Windows.Forms.TabControl();                                    
            this.tabControlMain.SuspendLayout();            
            this.SuspendLayout();
            System.Reflection.Assembly assem = this.GetType().Assembly;
            System.Resources.ResourceManager resman = new System.Resources.ResourceManager("AutoWeb.Properties.Resources", assem);

            // 
            // tabControlMain
            //             
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);            
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;            
            
            // 
            // MainForm
            //             
            this.Controls.Add(this.tabControlMain);            
            this.Name = "MainForm";
            this.Text = "AutoWeb";
            this.Icon = ((System.Drawing.Icon)(resman.GetObject("world")));
            this.tabControlMain.ResumeLayout(false);            
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form_Closing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.ShowInTaskbar = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 500);

            //
            // NotificationIcon
            //
            this.components1 = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuItem1 });            
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);            

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components1);                                              
            notifyIcon1.Icon = ((System.Drawing.Icon)(resman.GetObject("world")));  

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;
            
            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "AutoWeb";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form (- we handle single and double click the exact same way!)
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_DoubleClick);
        }

        #endregion       

        private bool bIsOKtoClose = false;                                        
        private System.Windows.Forms.TabControl tabControlMain;

        // The Notification Icon
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.ComponentModel.IContainer components1;
             
    }
}

