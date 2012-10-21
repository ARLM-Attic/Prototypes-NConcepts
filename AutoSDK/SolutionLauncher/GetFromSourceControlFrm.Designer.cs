namespace SolutionLauncher
{
    partial class GetFromSourceControlFrm
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
            this.labelProject = new System.Windows.Forms.Label();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelDisplayProject = new System.Windows.Forms.Label();
            this.labelDisplayVersion = new System.Windows.Forms.Label();
            this.labelProcessing = new System.Windows.Forms.Label();
            this.labelDisplayProcessing = new System.Windows.Forms.Label();
            this.labelSelectedDate = new System.Windows.Forms.Label();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Location = new System.Drawing.Point(24, 45);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(173, 14);
            this.labelProject.TabIndex = 0;
            this.labelProject.Text = "The Currenctly Selected Project is:";
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(24, 411);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "&No";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(230, 411);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "&Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(24, 66);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(178, 14);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "The Currenctly Selected Version is:";
            // 
            // labelDisplayProject
            // 
            this.labelDisplayProject.AutoSize = true;
            this.labelDisplayProject.Location = new System.Drawing.Point(204, 45);
            this.labelDisplayProject.Name = "labelDisplayProject";
            this.labelDisplayProject.Size = new System.Drawing.Size(35, 14);
            this.labelDisplayProject.TabIndex = 4;
            this.labelDisplayProject.Text = "label3";
            // 
            // labelDisplayVersion
            // 
            this.labelDisplayVersion.AutoSize = true;
            this.labelDisplayVersion.Location = new System.Drawing.Point(207, 66);
            this.labelDisplayVersion.Name = "labelDisplayVersion";
            this.labelDisplayVersion.Size = new System.Drawing.Size(35, 14);
            this.labelDisplayVersion.TabIndex = 5;
            this.labelDisplayVersion.Text = "label4";
            // 
            // labelProcessing
            // 
            this.labelProcessing.AutoSize = true;
            this.labelProcessing.Location = new System.Drawing.Point(24, 354);
            this.labelProcessing.Name = "labelProcessing";
            this.labelProcessing.Size = new System.Drawing.Size(111, 14);
            this.labelProcessing.TabIndex = 6;
            this.labelProcessing.Text = "Currently Processing:";
            // 
            // labelDisplayProcessing
            // 
            this.labelDisplayProcessing.AutoSize = true;
            this.labelDisplayProcessing.Location = new System.Drawing.Point(24, 372);
            this.labelDisplayProcessing.Name = "labelDisplayProcessing";
            this.labelDisplayProcessing.Size = new System.Drawing.Size(0, 14);
            this.labelDisplayProcessing.TabIndex = 7;
            // 
            // labelSelectedDate
            // 
            this.labelSelectedDate.AutoSize = true;
            this.labelSelectedDate.Location = new System.Drawing.Point(24, 120);
            this.labelSelectedDate.Name = "labelSelectedDate";
            this.labelSelectedDate.Size = new System.Drawing.Size(123, 14);
            this.labelSelectedDate.TabIndex = 8;
            this.labelSelectedDate.Text = "Select Date To Retrieve:";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(27, 143);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 9;
            // 
            // GetFromSourceControlFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 463);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.labelSelectedDate);
            this.Controls.Add(this.labelDisplayProcessing);
            this.Controls.Add(this.labelProcessing);
            this.Controls.Add(this.labelDisplayVersion);
            this.Controls.Add(this.labelDisplayProject);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.labelProject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GetFromSourceControlFrm";
            this.Text = "Retrieve Files From Source Control? Yes/No";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.GetFromSourceControlFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelDisplayProject;
        private System.Windows.Forms.Label labelDisplayVersion;
        private System.Windows.Forms.Label labelProcessing;
        private System.Windows.Forms.Label labelDisplayProcessing;
        private System.Windows.Forms.Label labelSelectedDate;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
    }
}