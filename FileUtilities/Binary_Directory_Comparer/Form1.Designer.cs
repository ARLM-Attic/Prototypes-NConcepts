namespace Binary_Directory_Comparer
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
            this.txtBrowse1 = new System.Windows.Forms.TextBox();
            this.btnBrows1 = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblSelect1 = new System.Windows.Forms.Label();
            this.lblSelect2 = new System.Windows.Forms.Label();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.txtBrowse2 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBrowse1
            // 
            this.txtBrowse1.Location = new System.Drawing.Point(86, 34);
            this.txtBrowse1.Name = "txtBrowse1";
            this.txtBrowse1.Size = new System.Drawing.Size(310, 20);
            this.txtBrowse1.TabIndex = 0;
            // 
            // btnBrows1
            // 
            this.btnBrows1.Location = new System.Drawing.Point(5, 32);
            this.btnBrows1.Name = "btnBrows1";
            this.btnBrows1.Size = new System.Drawing.Size(75, 23);
            this.btnBrows1.TabIndex = 1;
            this.btnBrows1.Text = "Browse";
            this.btnBrows1.UseVisualStyleBackColor = true;
            this.btnBrows1.Click += new System.EventHandler(this.btnBrows1_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(12, 18);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(394, 41);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "This Program compares the Byte Sizes of All the files in the given directory agai" +
                "nst all the ones in the other given directory; for easy byte size compare";
            // 
            // lblSelect1
            // 
            this.lblSelect1.AutoSize = true;
            this.lblSelect1.Location = new System.Drawing.Point(5, 11);
            this.lblSelect1.Name = "lblSelect1";
            this.lblSelect1.Size = new System.Drawing.Size(96, 14);
            this.lblSelect1.TabIndex = 3;
            this.lblSelect1.Text = "Select Directory 1:";
            // 
            // lblSelect2
            // 
            this.lblSelect2.AutoSize = true;
            this.lblSelect2.Location = new System.Drawing.Point(5, 74);
            this.lblSelect2.Name = "lblSelect2";
            this.lblSelect2.Size = new System.Drawing.Size(96, 14);
            this.lblSelect2.TabIndex = 6;
            this.lblSelect2.Text = "Select Directory 2:";
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Location = new System.Drawing.Point(5, 95);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse2.TabIndex = 5;
            this.btnBrowse2.Text = "Browse";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // txtBrowse2
            // 
            this.txtBrowse2.Location = new System.Drawing.Point(86, 97);
            this.txtBrowse2.Name = "txtBrowse2";
            this.txtBrowse2.Size = new System.Drawing.Size(310, 20);
            this.txtBrowse2.TabIndex = 4;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 286);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(405, 203);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = " - Generated Automatically - Seletec Directory 1 and 2 - Push Compare Button";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(15, 267);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(85, 14);
            this.lblOutput.TabIndex = 8;
            this.lblOutput.Text = "Output Window:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(142, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Compare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.lblSelect1);
            this.panel1.Controls.Add(this.txtBrowse1);
            this.panel1.Controls.Add(this.btnBrows1);
            this.panel1.Controls.Add(this.txtBrowse2);
            this.panel1.Controls.Add(this.lblSelect2);
            this.panel1.Controls.Add(this.btnBrowse2);
            this.panel1.Location = new System.Drawing.Point(15, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 147);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Location = new System.Drawing.Point(15, 215);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(405, 42);
            this.panel2.TabIndex = 11;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 499);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "Simple Byte Size Directory Compare Tool";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBrowse1;
        private System.Windows.Forms.Button btnBrows1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblSelect1;
        private System.Windows.Forms.Label lblSelect2;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.TextBox txtBrowse2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

