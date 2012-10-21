namespace History
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_tbFile = new System.Windows.Forms.TextBox();
            this.m_tvChangeSets = new System.Windows.Forms.TreeView();
            this.m_tbComments = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.m_cbServerName = new System.Windows.Forms.ComboBox();
            this.m_btnViewHistory = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_tbUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_tbDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "File";
            // 
            // m_tbFile
            // 
            this.m_tbFile.AllowDrop = true;
            this.m_tbFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_tbFile.Location = new System.Drawing.Point(53, 48);
            this.m_tbFile.Margin = new System.Windows.Forms.Padding(4);
            this.m_tbFile.Name = "m_tbFile";
            this.m_tbFile.Size = new System.Drawing.Size(559, 22);
            this.m_tbFile.TabIndex = 3;
            //this.m_tbFile.TextChanged += new System.EventHandler(this.m_tbFile_TextChanged);
            // 
            // m_tvChangeSets
            // 
            this.m_tvChangeSets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tvChangeSets.Location = new System.Drawing.Point(0, 4);
            this.m_tvChangeSets.Margin = new System.Windows.Forms.Padding(4);
            this.m_tvChangeSets.Name = "m_tvChangeSets";
            this.m_tvChangeSets.Size = new System.Drawing.Size(307, 468);
            this.m_tvChangeSets.TabIndex = 4;
            this.m_tvChangeSets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // m_tbComments
            // 
            this.m_tbComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbComments.Location = new System.Drawing.Point(4, 4);
            this.m_tbComments.Margin = new System.Windows.Forms.Padding(4);
            this.m_tbComments.Multiline = true;
            this.m_tbComments.Name = "m_tbComments";
            this.m_tbComments.Size = new System.Drawing.Size(503, 263);
            this.m_tbComments.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(5, 134);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.m_tvChangeSets);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(828, 473);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.m_tbComments);
           // this.splitContainer2.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel1_Paint);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label5);
            this.splitContainer2.Panel2.Controls.Add(this.textBox1);
            this.splitContainer2.Size = new System.Drawing.Size(511, 473);
            this.splitContainer2.SplitterDistance = 271;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(4, 32);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(503, 161);
            this.textBox1.TabIndex = 6;
            // 
            // m_cbServerName
            // 
            this.m_cbServerName.FormattingEnabled = true;
            this.m_cbServerName.Location = new System.Drawing.Point(53, 16);
            this.m_cbServerName.Margin = new System.Windows.Forms.Padding(4);
            this.m_cbServerName.Name = "m_cbServerName";
            this.m_cbServerName.Size = new System.Drawing.Size(559, 24);
            this.m_cbServerName.TabIndex = 7;
            // 
            // m_btnViewHistory
            // 
            this.m_btnViewHistory.Location = new System.Drawing.Point(733, 44);
            this.m_btnViewHistory.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnViewHistory.Name = "m_btnViewHistory";
            this.m_btnViewHistory.Size = new System.Drawing.Size(100, 28);
            this.m_btnViewHistory.TabIndex = 8;
            this.m_btnViewHistory.Text = "View History";
            this.m_btnViewHistory.UseVisualStyleBackColor = true;
            this.m_btnViewHistory.Click += new System.EventHandler(this.m_btnViewHistory_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "User";
            // 
            // m_tbUser
            // 
            this.m_tbUser.Location = new System.Drawing.Point(53, 81);
            this.m_tbUser.Margin = new System.Windows.Forms.Padding(4);
            this.m_tbUser.Name = "m_tbUser";
            this.m_tbUser.ReadOnly = true;
            this.m_tbUser.Size = new System.Drawing.Size(259, 22);
            this.m_tbUser.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(323, 80);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Date";
            // 
            // m_tbDate
            // 
            this.m_tbDate.Location = new System.Drawing.Point(369, 81);
            this.m_tbDate.Margin = new System.Windows.Forms.Padding(4);
            this.m_tbDate.Name = "m_tbDate";
            this.m_tbDate.ReadOnly = true;
            this.m_tbDate.Size = new System.Drawing.Size(243, 22);
            this.m_tbDate.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Files Changed:";
           // this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 117);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "History:";
           // this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(323, 117);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Information:";
            // 
            // Form1
            // 
            this.AcceptButton = this.m_btnViewHistory;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 612);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_tbDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_tbUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_btnViewHistory);
            this.Controls.Add(this.m_cbServerName);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.m_tbFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "History and Label View";
      //      this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_tbFile;
        private System.Windows.Forms.TreeView m_tvChangeSets;
        private System.Windows.Forms.TextBox m_tbComments;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox m_cbServerName;
        private System.Windows.Forms.Button m_btnViewHistory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_tbUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_tbDate;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

