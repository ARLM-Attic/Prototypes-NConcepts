namespace VSStripper
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
            this.components = new System.ComponentModel.Container();
            this.btnStripeVSS = new System.Windows.Forms.Button();
            this.lblCurrentFolder = new System.Windows.Forms.Label();
            this.lblDispCurrentFolder = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSln = new System.Windows.Forms.CheckBox();
            this.chkDsp = new System.Windows.Forms.CheckBox();
            this.chkVcproj = new System.Windows.Forms.CheckBox();
            this.chkVcp = new System.Windows.Forms.CheckBox();
            this.gBoxBackupRestore = new System.Windows.Forms.GroupBox();
            this.lblWarning = new System.Windows.Forms.Label();
            this.btnRestoreProjectFiles = new System.Windows.Forms.Button();
            this.btnRemoveAllBackup = new System.Windows.Forms.Button();
            this.chkOverwriteBackupFiles = new System.Windows.Forms.CheckBox();
            this.chkBackupFiles = new System.Windows.Forms.CheckBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.gBoxReadWrite = new System.Windows.Forms.GroupBox();
            this.btnMarkWritable = new System.Windows.Forms.Button();
            this.btnMarkReadOnly = new System.Windows.Forms.Button();
            this.lblShowProcessedFile = new System.Windows.Forms.Label();
            this.lblShowCurrentFolder = new System.Windows.Forms.Label();
            this.ttipShowCurrentFolder = new System.Windows.Forms.ToolTip(this.components);
            this.gBoxVssVstsBindings = new System.Windows.Forms.GroupBox();
            this.buttoNOTusedYet = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkcsProj = new System.Windows.Forms.CheckBox();
            this.gBoxBackupRestore.SuspendLayout();
            this.gBoxReadWrite.SuspendLayout();
            this.gBoxVssVstsBindings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStripeVSS
            // 
            this.btnStripeVSS.Location = new System.Drawing.Point(0, 152);
            this.btnStripeVSS.Name = "btnStripeVSS";
            this.btnStripeVSS.Size = new System.Drawing.Size(260, 23);
            this.btnStripeVSS.TabIndex = 0;
            this.btnStripeVSS.Text = "Stripe VSS Bindings From Files";
            this.btnStripeVSS.UseVisualStyleBackColor = true;
            this.btnStripeVSS.Click += new System.EventHandler(this.btnStripeVSS_Click);
            // 
            // lblCurrentFolder
            // 
            this.lblCurrentFolder.AutoSize = true;
            this.lblCurrentFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblCurrentFolder.Location = new System.Drawing.Point(12, 15);
            this.lblCurrentFolder.Name = "lblCurrentFolder";
            this.lblCurrentFolder.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCurrentFolder.Size = new System.Drawing.Size(79, 13);
            this.lblCurrentFolder.TabIndex = 2;
            this.lblCurrentFolder.Text = "Current Folder: ";
            // 
            // lblDispCurrentFolder
            // 
            this.lblDispCurrentFolder.AutoSize = true;
            this.lblDispCurrentFolder.Location = new System.Drawing.Point(97, 29);
            this.lblDispCurrentFolder.Name = "lblDispCurrentFolder";
            this.lblDispCurrentFolder.Size = new System.Drawing.Size(0, 13);
            this.lblDispCurrentFolder.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select your Project Type(s):";
            // 
            // chkSln
            // 
            this.chkSln.AutoSize = true;
            this.chkSln.Checked = true;
            this.chkSln.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSln.Location = new System.Drawing.Point(12, 65);
            this.chkSln.Name = "chkSln";
            this.chkSln.Size = new System.Drawing.Size(66, 17);
            this.chkSln.TabIndex = 5;
            this.chkSln.Tag = ".sln";
            this.chkSln.Text = ".sln Files";
            this.chkSln.UseVisualStyleBackColor = true;
            // 
            // chkDsp
            // 
            this.chkDsp.AutoSize = true;
            this.chkDsp.Checked = true;
            this.chkDsp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDsp.Location = new System.Drawing.Point(12, 94);
            this.chkDsp.Name = "chkDsp";
            this.chkDsp.Size = new System.Drawing.Size(70, 17);
            this.chkDsp.TabIndex = 6;
            this.chkDsp.Tag = ".dsp";
            this.chkDsp.Text = ".dsp Files";
            this.chkDsp.UseVisualStyleBackColor = true;
            // 
            // chkVcproj
            // 
            this.chkVcproj.AutoSize = true;
            this.chkVcproj.Checked = true;
            this.chkVcproj.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVcproj.Location = new System.Drawing.Point(135, 64);
            this.chkVcproj.Name = "chkVcproj";
            this.chkVcproj.Size = new System.Drawing.Size(82, 17);
            this.chkVcproj.TabIndex = 7;
            this.chkVcproj.Tag = ".vcproj";
            this.chkVcproj.Text = ".vcproj Files";
            this.chkVcproj.UseVisualStyleBackColor = true;
            // 
            // chkVcp
            // 
            this.chkVcp.AutoSize = true;
            this.chkVcp.Location = new System.Drawing.Point(12, 123);
            this.chkVcp.Name = "chkVcp";
            this.chkVcp.Size = new System.Drawing.Size(71, 17);
            this.chkVcp.TabIndex = 8;
            this.chkVcp.Tag = ".vcp";
            this.chkVcp.Text = ".vcp Files";
            this.chkVcp.UseVisualStyleBackColor = true;
            // 
            // gBoxBackupRestore
            // 
            this.gBoxBackupRestore.Controls.Add(this.lblWarning);
            this.gBoxBackupRestore.Controls.Add(this.btnRestoreProjectFiles);
            this.gBoxBackupRestore.Controls.Add(this.btnRemoveAllBackup);
            this.gBoxBackupRestore.Controls.Add(this.chkOverwriteBackupFiles);
            this.gBoxBackupRestore.Controls.Add(this.chkBackupFiles);
            this.gBoxBackupRestore.Location = new System.Drawing.Point(15, 300);
            this.gBoxBackupRestore.Name = "gBoxBackupRestore";
            this.gBoxBackupRestore.Size = new System.Drawing.Size(260, 143);
            this.gBoxBackupRestore.TabIndex = 9;
            this.gBoxBackupRestore.TabStop = false;
            this.gBoxBackupRestore.Text = "Backup / Restore";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(23, 65);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(235, 13);
            this.lblWarning.TabIndex = 4;
            this.lblWarning.Text = "(Select only if you just got new source from VSS)";
            // 
            // btnRestoreProjectFiles
            // 
            this.btnRestoreProjectFiles.Location = new System.Drawing.Point(0, 118);
            this.btnRestoreProjectFiles.Name = "btnRestoreProjectFiles";
            this.btnRestoreProjectFiles.Size = new System.Drawing.Size(260, 23);
            this.btnRestoreProjectFiles.TabIndex = 3;
            this.btnRestoreProjectFiles.Text = "Restore Project Files From Backup";
            this.btnRestoreProjectFiles.UseVisualStyleBackColor = true;
            this.btnRestoreProjectFiles.Click += new System.EventHandler(this.btnRestoreProjectFiles_Click);
            // 
            // btnRemoveAllBackup
            // 
            this.btnRemoveAllBackup.Location = new System.Drawing.Point(0, 86);
            this.btnRemoveAllBackup.Name = "btnRemoveAllBackup";
            this.btnRemoveAllBackup.Size = new System.Drawing.Size(260, 23);
            this.btnRemoveAllBackup.TabIndex = 2;
            this.btnRemoveAllBackup.Text = "Remove All Backup Project Files";
            this.btnRemoveAllBackup.UseVisualStyleBackColor = true;
            this.btnRemoveAllBackup.Click += new System.EventHandler(this.btnRemoveAllBackup_Click);
            // 
            // chkOverwriteBackupFiles
            // 
            this.chkOverwriteBackupFiles.AutoSize = true;
            this.chkOverwriteBackupFiles.Location = new System.Drawing.Point(8, 48);
            this.chkOverwriteBackupFiles.Name = "chkOverwriteBackupFiles";
            this.chkOverwriteBackupFiles.Size = new System.Drawing.Size(173, 17);
            this.chkOverwriteBackupFiles.TabIndex = 1;
            this.chkOverwriteBackupFiles.Text = "Overwrite existing Backup Files";
            this.chkOverwriteBackupFiles.UseVisualStyleBackColor = true;
            // 
            // chkBackupFiles
            // 
            this.chkBackupFiles.AutoSize = true;
            this.chkBackupFiles.Checked = true;
            this.chkBackupFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBackupFiles.Location = new System.Drawing.Point(8, 25);
            this.chkBackupFiles.Name = "chkBackupFiles";
            this.chkBackupFiles.Size = new System.Drawing.Size(250, 17);
            this.chkBackupFiles.TabIndex = 0;
            this.chkBackupFiles.Text = "Backup Project File Before Making any Change";
            this.chkBackupFiles.UseVisualStyleBackColor = true;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(9, 39);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(182, 13);
            this.lblNote.TabIndex = 10;
            this.lblNote.Text = "(Project Files will be marked Writable)";
            // 
            // gBoxReadWrite
            // 
            this.gBoxReadWrite.Controls.Add(this.btnMarkWritable);
            this.gBoxReadWrite.Controls.Add(this.btnMarkReadOnly);
            this.gBoxReadWrite.Location = new System.Drawing.Point(15, 454);
            this.gBoxReadWrite.Name = "gBoxReadWrite";
            this.gBoxReadWrite.Size = new System.Drawing.Size(258, 72);
            this.gBoxReadWrite.TabIndex = 11;
            this.gBoxReadWrite.TabStop = false;
            this.gBoxReadWrite.Text = "Read / Write";
            // 
            // btnMarkWritable
            // 
            this.btnMarkWritable.Location = new System.Drawing.Point(0, 47);
            this.btnMarkWritable.Name = "btnMarkWritable";
            this.btnMarkWritable.Size = new System.Drawing.Size(258, 23);
            this.btnMarkWritable.TabIndex = 1;
            this.btnMarkWritable.Text = "Mark all Files Writable";
            this.btnMarkWritable.UseVisualStyleBackColor = true;
            this.btnMarkWritable.Click += new System.EventHandler(this.btnMarkWritable_Click);
            // 
            // btnMarkReadOnly
            // 
            this.btnMarkReadOnly.Location = new System.Drawing.Point(0, 19);
            this.btnMarkReadOnly.Name = "btnMarkReadOnly";
            this.btnMarkReadOnly.Size = new System.Drawing.Size(258, 23);
            this.btnMarkReadOnly.TabIndex = 0;
            this.btnMarkReadOnly.Text = "Mark all Files as Read-Only";
            this.btnMarkReadOnly.UseVisualStyleBackColor = true;
            this.btnMarkReadOnly.Click += new System.EventHandler(this.btnMarkReadOnly_Click);
            // 
            // lblShowProcessedFile
            // 
            this.lblShowProcessedFile.AutoSize = true;
            this.lblShowProcessedFile.Location = new System.Drawing.Point(6, 18);
            this.lblShowProcessedFile.Name = "lblShowProcessedFile";
            this.lblShowProcessedFile.Size = new System.Drawing.Size(52, 13);
            this.lblShowProcessedFile.TabIndex = 3;
            this.lblShowProcessedFile.Text = "(dynamic)";
            // 
            // lblShowCurrentFolder
            // 
            this.lblShowCurrentFolder.AutoSize = true;
            this.lblShowCurrentFolder.Location = new System.Drawing.Point(85, 15);
            this.lblShowCurrentFolder.Name = "lblShowCurrentFolder";
            this.lblShowCurrentFolder.Size = new System.Drawing.Size(52, 13);
            this.lblShowCurrentFolder.TabIndex = 12;
            this.lblShowCurrentFolder.Text = "(dynamic)";
            // 
            // gBoxVssVstsBindings
            // 
            this.gBoxVssVstsBindings.Controls.Add(this.chkcsProj);
            this.gBoxVssVstsBindings.Controls.Add(this.buttoNOTusedYet);
            this.gBoxVssVstsBindings.Controls.Add(this.label1);
            this.gBoxVssVstsBindings.Controls.Add(this.btnStripeVSS);
            this.gBoxVssVstsBindings.Controls.Add(this.chkSln);
            this.gBoxVssVstsBindings.Controls.Add(this.lblNote);
            this.gBoxVssVstsBindings.Controls.Add(this.chkDsp);
            this.gBoxVssVstsBindings.Controls.Add(this.chkVcproj);
            this.gBoxVssVstsBindings.Controls.Add(this.chkVcp);
            this.gBoxVssVstsBindings.Location = new System.Drawing.Point(15, 84);
            this.gBoxVssVstsBindings.Name = "gBoxVssVstsBindings";
            this.gBoxVssVstsBindings.Size = new System.Drawing.Size(260, 210);
            this.gBoxVssVstsBindings.TabIndex = 13;
            this.gBoxVssVstsBindings.TabStop = false;
            this.gBoxVssVstsBindings.Text = "VSS / VSTS Bindings";
            // 
            // buttoNOTusedYet
            // 
            this.buttoNOTusedYet.Enabled = false;
            this.buttoNOTusedYet.Location = new System.Drawing.Point(0, 181);
            this.buttoNOTusedYet.Name = "buttoNOTusedYet";
            this.buttoNOTusedYet.Size = new System.Drawing.Size(260, 23);
            this.buttoNOTusedYet.TabIndex = 11;
            this.buttoNOTusedYet.Text = "Stripe VSTS Bindings From Files";
            this.buttoNOTusedYet.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox4.Controls.Add(this.lblShowProcessedFile);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox4.Location = new System.Drawing.Point(15, 38);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(260, 42);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Currently Processing File:";
            // 
            // chkcsProj
            // 
            this.chkcsProj.AutoSize = true;
            this.chkcsProj.Checked = true;
            this.chkcsProj.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkcsProj.Location = new System.Drawing.Point(135, 94);
            this.chkcsProj.Name = "chkcsProj";
            this.chkcsProj.Size = new System.Drawing.Size(81, 17);
            this.chkcsProj.TabIndex = 12;
            this.chkcsProj.Tag = ".csproj";
            this.chkcsProj.Text = ".csproj Files";
            this.chkcsProj.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 544);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gBoxVssVstsBindings);
            this.Controls.Add(this.lblShowCurrentFolder);
            this.Controls.Add(this.gBoxReadWrite);
            this.Controls.Add(this.gBoxBackupRestore);
            this.Controls.Add(this.lblDispCurrentFolder);
            this.Controls.Add(this.lblCurrentFolder);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "VSStripper";
            this.gBoxBackupRestore.ResumeLayout(false);
            this.gBoxBackupRestore.PerformLayout();
            this.gBoxReadWrite.ResumeLayout(false);
            this.gBoxVssVstsBindings.ResumeLayout(false);
            this.gBoxVssVstsBindings.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStripeVSS;
        private System.Windows.Forms.Label lblCurrentFolder;
        private System.Windows.Forms.Label lblDispCurrentFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSln;
        private System.Windows.Forms.CheckBox chkDsp;
        private System.Windows.Forms.CheckBox chkVcproj;
        private System.Windows.Forms.CheckBox chkVcp;
        private System.Windows.Forms.GroupBox gBoxBackupRestore;
        private System.Windows.Forms.CheckBox chkOverwriteBackupFiles;
        private System.Windows.Forms.CheckBox chkBackupFiles;
        private System.Windows.Forms.Button btnRestoreProjectFiles;
        private System.Windows.Forms.Button btnRemoveAllBackup;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.GroupBox gBoxReadWrite;
        private System.Windows.Forms.Button btnMarkWritable;
        private System.Windows.Forms.Button btnMarkReadOnly;
        private System.Windows.Forms.Label lblShowProcessedFile;
        private System.Windows.Forms.Label lblShowCurrentFolder;
        private System.Windows.Forms.ToolTip ttipShowCurrentFolder;
        private System.Windows.Forms.GroupBox gBoxVssVstsBindings;
        private System.Windows.Forms.Button buttoNOTusedYet;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkcsProj;
    }
}

