namespace OptimusPrime
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
            this.process1 = new System.Diagnostics.Process();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSiteServer = new System.Windows.Forms.Label();
            this.Siteserver_Tools_button = new System.Windows.Forms.Button();
            this.Siteserver_explorer_button = new System.Windows.Forms.Button();
            this.Siteserver_insert_button = new System.Windows.Forms.Button();
            this.Siteserver_edit_button = new System.Windows.Forms.Button();
            this.Siteserver_delete_button = new System.Windows.Forms.Button();
            this.Siteserver_listBox = new System.Windows.Forms.ListBox();
            this.labelPOS = new System.Windows.Forms.Label();
            this.POS_Tools_button = new System.Windows.Forms.Button();
            this.POS_explorer_button = new System.Windows.Forms.Button();
            this.POS_delete_button = new System.Windows.Forms.Button();
            this.POS_insert_button = new System.Windows.Forms.Button();
            this.groupBoxPOS = new System.Windows.Forms.GroupBox();
            this.POS_edit_button = new System.Windows.Forms.Button();
            this.POS_listbox = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxPOS.SuspendLayout();
            this.SuspendLayout();
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelSiteServer);
            this.groupBox1.Controls.Add(this.Siteserver_Tools_button);
            this.groupBox1.Controls.Add(this.Siteserver_explorer_button);
            this.groupBox1.Controls.Add(this.Siteserver_insert_button);
            this.groupBox1.Controls.Add(this.Siteserver_edit_button);
            this.groupBox1.Controls.Add(this.Siteserver_delete_button);
            this.groupBox1.Controls.Add(this.Siteserver_listBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 144);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SiteServer Select";
            // 
            // labelSiteServer
            // 
            this.labelSiteServer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelSiteServer.AutoSize = true;
            this.labelSiteServer.Location = new System.Drawing.Point(183, 16);
            this.labelSiteServer.Name = "labelSiteServer";
            this.labelSiteServer.Size = new System.Drawing.Size(66, 14);
            this.labelSiteServer.TabIndex = 6;
            this.labelSiteServer.Text = "Open with...";
            // 
            // Siteserver_Tools_button
            // 
            this.Siteserver_Tools_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Siteserver_Tools_button.Location = new System.Drawing.Point(186, 76);
            this.Siteserver_Tools_button.Name = "Siteserver_Tools_button";
            this.Siteserver_Tools_button.Size = new System.Drawing.Size(88, 38);
            this.Siteserver_Tools_button.TabIndex = 5;
            this.Siteserver_Tools_button.Text = "Site Tools";
            this.Siteserver_Tools_button.UseVisualStyleBackColor = true;
            this.Siteserver_Tools_button.Click += new System.EventHandler(this.Siteserver_Tools_button_Click);
            // 
            // Siteserver_explorer_button
            // 
            this.Siteserver_explorer_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Siteserver_explorer_button.Location = new System.Drawing.Point(186, 33);
            this.Siteserver_explorer_button.Name = "Siteserver_explorer_button";
            this.Siteserver_explorer_button.Size = new System.Drawing.Size(88, 38);
            this.Siteserver_explorer_button.TabIndex = 4;
            this.Siteserver_explorer_button.Text = "Site Explorer";
            this.Siteserver_explorer_button.UseVisualStyleBackColor = true;
            // 
            // Siteserver_insert_button
            // 
            this.Siteserver_insert_button.Location = new System.Drawing.Point(130, 51);
            this.Siteserver_insert_button.Name = "Siteserver_insert_button";
            this.Siteserver_insert_button.Size = new System.Drawing.Size(46, 23);
            this.Siteserver_insert_button.TabIndex = 3;
            this.Siteserver_insert_button.Text = "Insert";
            this.Siteserver_insert_button.UseVisualStyleBackColor = true;
            // 
            // Siteserver_edit_button
            // 
            this.Siteserver_edit_button.Location = new System.Drawing.Point(130, 80);
            this.Siteserver_edit_button.Name = "Siteserver_edit_button";
            this.Siteserver_edit_button.Size = new System.Drawing.Size(46, 23);
            this.Siteserver_edit_button.TabIndex = 2;
            this.Siteserver_edit_button.Text = "Edit";
            this.Siteserver_edit_button.UseVisualStyleBackColor = true;
            // 
            // Siteserver_delete_button
            // 
            this.Siteserver_delete_button.Location = new System.Drawing.Point(130, 109);
            this.Siteserver_delete_button.Name = "Siteserver_delete_button";
            this.Siteserver_delete_button.Size = new System.Drawing.Size(46, 23);
            this.Siteserver_delete_button.TabIndex = 1;
            this.Siteserver_delete_button.Text = "Delete";
            this.Siteserver_delete_button.UseVisualStyleBackColor = true;
            // 
            // Siteserver_listBox
            // 
            this.Siteserver_listBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.Siteserver_listBox.FormattingEnabled = true;
            this.Siteserver_listBox.ItemHeight = 14;
            this.Siteserver_listBox.Location = new System.Drawing.Point(3, 16);
            this.Siteserver_listBox.Name = "Siteserver_listBox";
            this.Siteserver_listBox.Size = new System.Drawing.Size(121, 116);
            this.Siteserver_listBox.TabIndex = 0;
            // 
            // labelPOS
            // 
            this.labelPOS.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelPOS.AutoSize = true;
            this.labelPOS.Location = new System.Drawing.Point(180, 16);
            this.labelPOS.Name = "labelPOS";
            this.labelPOS.Size = new System.Drawing.Size(63, 14);
            this.labelPOS.TabIndex = 6;
            this.labelPOS.Text = "Open with..";
            // 
            // POS_Tools_button
            // 
            this.POS_Tools_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.POS_Tools_button.Location = new System.Drawing.Point(183, 77);
            this.POS_Tools_button.Name = "POS_Tools_button";
            this.POS_Tools_button.Size = new System.Drawing.Size(88, 38);
            this.POS_Tools_button.TabIndex = 5;
            this.POS_Tools_button.Text = "POS Tools";
            this.POS_Tools_button.UseVisualStyleBackColor = true;
            this.POS_Tools_button.Click += new System.EventHandler(this.POS_Tools_button_Click);
            // 
            // POS_explorer_button
            // 
            this.POS_explorer_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.POS_explorer_button.Location = new System.Drawing.Point(183, 33);
            this.POS_explorer_button.Name = "POS_explorer_button";
            this.POS_explorer_button.Size = new System.Drawing.Size(88, 38);
            this.POS_explorer_button.TabIndex = 4;
            this.POS_explorer_button.Text = "POS Explorer";
            this.POS_explorer_button.UseVisualStyleBackColor = true;
            this.POS_explorer_button.Click += new System.EventHandler(this.POS_explorer_button_Click);
            // 
            // POS_delete_button
            // 
            this.POS_delete_button.Location = new System.Drawing.Point(127, 111);
            this.POS_delete_button.Name = "POS_delete_button";
            this.POS_delete_button.Size = new System.Drawing.Size(46, 23);
            this.POS_delete_button.TabIndex = 1;
            this.POS_delete_button.Text = "Delete";
            this.POS_delete_button.UseVisualStyleBackColor = true;
            // 
            // POS_insert_button
            // 
            this.POS_insert_button.Location = new System.Drawing.Point(127, 53);
            this.POS_insert_button.Name = "POS_insert_button";
            this.POS_insert_button.Size = new System.Drawing.Size(46, 23);
            this.POS_insert_button.TabIndex = 3;
            this.POS_insert_button.Text = "Insert";
            this.POS_insert_button.UseVisualStyleBackColor = true;
            // 
            // groupBoxPOS
            // 
            this.groupBoxPOS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPOS.Controls.Add(this.labelPOS);
            this.groupBoxPOS.Controls.Add(this.POS_Tools_button);
            this.groupBoxPOS.Controls.Add(this.POS_explorer_button);
            this.groupBoxPOS.Controls.Add(this.POS_insert_button);
            this.groupBoxPOS.Controls.Add(this.POS_edit_button);
            this.groupBoxPOS.Controls.Add(this.POS_delete_button);
            this.groupBoxPOS.Controls.Add(this.POS_listbox);
            this.groupBoxPOS.Location = new System.Drawing.Point(15, 161);
            this.groupBoxPOS.Name = "groupBoxPOS";
            this.groupBoxPOS.Size = new System.Drawing.Size(288, 140);
            this.groupBoxPOS.TabIndex = 7;
            this.groupBoxPOS.TabStop = false;
            this.groupBoxPOS.Text = "POSNodes Select";
            // 
            // POS_edit_button
            // 
            this.POS_edit_button.Location = new System.Drawing.Point(127, 82);
            this.POS_edit_button.Name = "POS_edit_button";
            this.POS_edit_button.Size = new System.Drawing.Size(46, 23);
            this.POS_edit_button.TabIndex = 2;
            this.POS_edit_button.Text = "Edit";
            this.POS_edit_button.UseVisualStyleBackColor = true;
            // 
            // POS_listbox
            // 
            this.POS_listbox.Dock = System.Windows.Forms.DockStyle.Left;
            this.POS_listbox.FormattingEnabled = true;
            this.POS_listbox.ItemHeight = 14;
            this.POS_listbox.Location = new System.Drawing.Point(3, 16);
            this.POS_listbox.Name = "POS_listbox";
            this.POS_listbox.Size = new System.Drawing.Size(118, 116);
            this.POS_listbox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(315, 311);
            this.Controls.Add(this.groupBoxPOS);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "OptimusPrime Dev Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxPOS.ResumeLayout(false);
            this.groupBoxPOS.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Diagnostics.Process process1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Siteserver_insert_button;
        private System.Windows.Forms.Button Siteserver_edit_button;
        private System.Windows.Forms.Button Siteserver_delete_button;
        private System.Windows.Forms.ListBox Siteserver_listBox;
        private System.Windows.Forms.Label labelSiteServer;
        private System.Windows.Forms.Button Siteserver_Tools_button;
        private System.Windows.Forms.Button Siteserver_explorer_button;
        private System.Windows.Forms.GroupBox groupBoxPOS;
        private System.Windows.Forms.Label labelPOS;
        private System.Windows.Forms.Button POS_Tools_button;
        private System.Windows.Forms.Button POS_explorer_button;
        private System.Windows.Forms.Button POS_insert_button;
        private System.Windows.Forms.Button POS_edit_button;
        private System.Windows.Forms.Button POS_delete_button;
        private System.Windows.Forms.ListBox POS_listbox;
    }
}

