namespace ForeclosureApp
{
    partial class FormMain
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.richTextBoxMain = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabControls = new System.Windows.Forms.TabPage();
            this.groupBoxDR = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblShowUnsuccessful = new System.Windows.Forms.Label();
            this.lblShowSuccessful = new System.Windows.Forms.Label();
            this.lblShowProperty = new System.Windows.Forms.Label();
            this.lblShowPage = new System.Windows.Forms.Label();
            this.lblStartPage = new System.Windows.Forms.Label();
            this.textBoxStartPage = new System.Windows.Forms.TextBox();
            this.lblShowStatus = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.lblUnsuccesful = new System.Windows.Forms.Label();
            this.lblProperty = new System.Windows.Forms.Label();
            this.lblSuccessful = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tabIExplorer = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.tabUnsuccessful = new System.Windows.Forms.TabPage();
            this.tabREFile = new System.Windows.Forms.TabPage();
            this.btnParse = new System.Windows.Forms.Button();
            this.lblRE = new System.Windows.Forms.Label();
            this.textBoxRE = new System.Windows.Forms.TextBox();
            this.lblFileSelect = new System.Windows.Forms.Label();
            this.comboBoxFileSelect = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkParseParam = new System.Windows.Forms.CheckBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabControls.SuspendLayout();
            this.groupBoxDR.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabIExplorer.SuspendLayout();
            this.tabREFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(899, 327);
            this.panelMain.TabIndex = 0;
            // 
            // richTextBoxMain
            // 
            this.richTextBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMain.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMain.Name = "richTextBoxMain";
            this.richTextBoxMain.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.richTextBoxMain.Size = new System.Drawing.Size(899, 324);
            this.richTextBoxMain.TabIndex = 1;
            this.richTextBoxMain.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 34);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxMain);
            this.splitContainer1.Size = new System.Drawing.Size(899, 655);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 2;
            // 
            // textBoxURL
            // 
            this.textBoxURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxURL.Location = new System.Drawing.Point(61, 5);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(671, 20);
            this.textBoxURL.TabIndex = 3;
            this.textBoxURL.TextChanged += new System.EventHandler(this.textBoxURL_TextChanged);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabControls);
            this.tabControlMain.Controls.Add(this.tabIExplorer);
            this.tabControlMain.Controls.Add(this.tabUnsuccessful);
            this.tabControlMain.Controls.Add(this.tabREFile);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(921, 724);
            this.tabControlMain.TabIndex = 4;
            // 
            // tabControls
            // 
            this.tabControls.Controls.Add(this.groupBoxDR);
            this.tabControls.Location = new System.Drawing.Point(4, 23);
            this.tabControls.Name = "tabControls";
            this.tabControls.Padding = new System.Windows.Forms.Padding(3);
            this.tabControls.Size = new System.Drawing.Size(913, 697);
            this.tabControls.TabIndex = 0;
            this.tabControls.Text = "Controls";
            this.tabControls.UseVisualStyleBackColor = true;
            // 
            // groupBoxDR
            // 
            this.groupBoxDR.Controls.Add(this.groupBox1);
            this.groupBoxDR.Controls.Add(this.btnStart);
            this.groupBoxDR.Controls.Add(this.btnStop);
            this.groupBoxDR.Location = new System.Drawing.Point(6, 6);
            this.groupBoxDR.Name = "groupBoxDR";
            this.groupBoxDR.Size = new System.Drawing.Size(312, 263);
            this.groupBoxDR.TabIndex = 0;
            this.groupBoxDR.TabStop = false;
            this.groupBoxDR.Text = "Daily Report";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblShowUnsuccessful);
            this.groupBox1.Controls.Add(this.lblShowSuccessful);
            this.groupBox1.Controls.Add(this.lblShowProperty);
            this.groupBox1.Controls.Add(this.lblShowPage);
            this.groupBox1.Controls.Add(this.lblStartPage);
            this.groupBox1.Controls.Add(this.textBoxStartPage);
            this.groupBox1.Controls.Add(this.lblShowStatus);
            this.groupBox1.Controls.Add(this.lblPage);
            this.groupBox1.Controls.Add(this.lblUnsuccesful);
            this.groupBox1.Controls.Add(this.lblProperty);
            this.groupBox1.Controls.Add(this.lblSuccessful);
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 175);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Currently Processing";
            // 
            // lblShowUnsuccessful
            // 
            this.lblShowUnsuccessful.AutoSize = true;
            this.lblShowUnsuccessful.Location = new System.Drawing.Point(90, 100);
            this.lblShowUnsuccessful.Name = "lblShowUnsuccessful";
            this.lblShowUnsuccessful.Size = new System.Drawing.Size(25, 14);
            this.lblShowUnsuccessful.TabIndex = 13;
            this.lblShowUnsuccessful.Text = "N/A";
            // 
            // lblShowSuccessful
            // 
            this.lblShowSuccessful.AutoSize = true;
            this.lblShowSuccessful.Location = new System.Drawing.Point(123, 78);
            this.lblShowSuccessful.Name = "lblShowSuccessful";
            this.lblShowSuccessful.Size = new System.Drawing.Size(25, 14);
            this.lblShowSuccessful.TabIndex = 12;
            this.lblShowSuccessful.Text = "N/A";
            // 
            // lblShowProperty
            // 
            this.lblShowProperty.AutoSize = true;
            this.lblShowProperty.Location = new System.Drawing.Point(64, 56);
            this.lblShowProperty.Name = "lblShowProperty";
            this.lblShowProperty.Size = new System.Drawing.Size(25, 14);
            this.lblShowProperty.TabIndex = 11;
            this.lblShowProperty.Text = "N/A";
            // 
            // lblShowPage
            // 
            this.lblShowPage.AutoSize = true;
            this.lblShowPage.Location = new System.Drawing.Point(46, 32);
            this.lblShowPage.Name = "lblShowPage";
            this.lblShowPage.Size = new System.Drawing.Size(25, 14);
            this.lblShowPage.TabIndex = 10;
            this.lblShowPage.Text = "N/A";
            // 
            // lblStartPage
            // 
            this.lblStartPage.AutoSize = true;
            this.lblStartPage.Location = new System.Drawing.Point(178, 32);
            this.lblStartPage.Name = "lblStartPage";
            this.lblStartPage.Size = new System.Drawing.Size(53, 14);
            this.lblStartPage.TabIndex = 9;
            this.lblStartPage.Text = "startPage";
            // 
            // textBoxStartPage
            // 
            this.textBoxStartPage.Location = new System.Drawing.Point(237, 29);
            this.textBoxStartPage.Name = "textBoxStartPage";
            this.textBoxStartPage.Size = new System.Drawing.Size(57, 20);
            this.textBoxStartPage.TabIndex = 8;
            this.textBoxStartPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.textBoxStartPage.TextChanged += new System.EventHandler(this.textBoxStartPage_TextChanged);
            this.textBoxStartPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // lblShowStatus
            // 
            this.lblShowStatus.AutoSize = true;
            this.lblShowStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowStatus.ForeColor = System.Drawing.Color.Red;
            this.lblShowStatus.Location = new System.Drawing.Point(152, 130);
            this.lblShowStatus.Name = "lblShowStatus";
            this.lblShowStatus.Size = new System.Drawing.Size(124, 19);
            this.lblShowStatus.TabIndex = 7;
            this.lblShowStatus.Text = "NOT STARTED";
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(6, 32);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(34, 14);
            this.lblPage.TabIndex = 3;
            this.lblPage.Text = "Page:";
            // 
            // lblUnsuccesful
            // 
            this.lblUnsuccesful.AutoSize = true;
            this.lblUnsuccesful.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblUnsuccesful.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblUnsuccesful.Location = new System.Drawing.Point(6, 100);
            this.lblUnsuccesful.Name = "lblUnsuccesful";
            this.lblUnsuccesful.Size = new System.Drawing.Size(77, 14);
            this.lblUnsuccesful.TabIndex = 6;
            this.lblUnsuccesful.Text = "Unsuccessful:";
            this.lblUnsuccesful.Click += new System.EventHandler(this.lblUnsuccesful_Click);
            // 
            // lblProperty
            // 
            this.lblProperty.AutoSize = true;
            this.lblProperty.Location = new System.Drawing.Point(6, 56);
            this.lblProperty.Name = "lblProperty";
            this.lblProperty.Size = new System.Drawing.Size(51, 14);
            this.lblProperty.TabIndex = 4;
            this.lblProperty.Text = "Property:";
            // 
            // lblSuccessful
            // 
            this.lblSuccessful.AutoSize = true;
            this.lblSuccessful.Location = new System.Drawing.Point(6, 78);
            this.lblSuccessful.Name = "lblSuccessful";
            this.lblSuccessful.Size = new System.Drawing.Size(110, 14);
            this.lblSuccessful.TabIndex = 5;
            this.lblSuccessful.Text = "Successfully Parsed:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(200, 200);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(106, 57);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(6, 200);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(89, 57);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tabIExplorer
            // 
            this.tabIExplorer.Controls.Add(this.label1);
            this.tabIExplorer.Controls.Add(this.btnBack);
            this.tabIExplorer.Controls.Add(this.splitContainer1);
            this.tabIExplorer.Controls.Add(this.btnGo);
            this.tabIExplorer.Controls.Add(this.textBoxURL);
            this.tabIExplorer.Location = new System.Drawing.Point(4, 23);
            this.tabIExplorer.Name = "tabIExplorer";
            this.tabIExplorer.Padding = new System.Windows.Forms.Padding(3);
            this.tabIExplorer.Size = new System.Drawing.Size(913, 697);
            this.tabIExplorer.TabIndex = 1;
            this.tabIExplorer.Text = "IExplorer";
            this.tabIExplorer.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "Address";
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(830, 5);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(749, 5);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            // 
            // tabUnsuccessful
            // 
            this.tabUnsuccessful.Location = new System.Drawing.Point(4, 23);
            this.tabUnsuccessful.Name = "tabUnsuccessful";
            this.tabUnsuccessful.Padding = new System.Windows.Forms.Padding(3);
            this.tabUnsuccessful.Size = new System.Drawing.Size(913, 697);
            this.tabUnsuccessful.TabIndex = 2;
            this.tabUnsuccessful.Text = "Unsuccessful";
            this.tabUnsuccessful.UseVisualStyleBackColor = true;
            // 
            // tabREFile
            // 
            this.tabREFile.Controls.Add(this.chkParseParam);
            this.tabREFile.Controls.Add(this.btnParse);
            this.tabREFile.Controls.Add(this.lblRE);
            this.tabREFile.Controls.Add(this.textBoxRE);
            this.tabREFile.Controls.Add(this.lblFileSelect);
            this.tabREFile.Controls.Add(this.comboBoxFileSelect);
            this.tabREFile.Controls.Add(this.dataGridView1);
            this.tabREFile.Location = new System.Drawing.Point(4, 23);
            this.tabREFile.Name = "tabREFile";
            this.tabREFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabREFile.Size = new System.Drawing.Size(913, 697);
            this.tabREFile.TabIndex = 3;
            this.tabREFile.Text = "REFileParser";
            this.tabREFile.UseVisualStyleBackColor = true;
            this.tabREFile.Enter += new System.EventHandler(this.tabREFile_Click);
            // 
            // btnParse
            // 
            this.btnParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParse.Location = new System.Drawing.Point(831, 37);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(74, 23);
            this.btnParse.TabIndex = 5;
            this.btnParse.Text = "Parse";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // lblRE
            // 
            this.lblRE.AutoSize = true;
            this.lblRE.Location = new System.Drawing.Point(8, 16);
            this.lblRE.Name = "lblRE";
            this.lblRE.Size = new System.Drawing.Size(236, 14);
            this.lblRE.TabIndex = 4;
            this.lblRE.Text = "write custom Regular Expression (incl. Groups)";
            // 
            // textBoxRE
            // 
            this.textBoxRE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRE.Location = new System.Drawing.Point(8, 38);
            this.textBoxRE.Name = "textBoxRE";
            this.textBoxRE.Size = new System.Drawing.Size(678, 20);
            this.textBoxRE.TabIndex = 3;
            // 
            // lblFileSelect
            // 
            this.lblFileSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileSelect.AutoSize = true;
            this.lblFileSelect.Location = new System.Drawing.Point(463, 9);
            this.lblFileSelect.Name = "lblFileSelect";
            this.lblFileSelect.Size = new System.Drawing.Size(102, 14);
            this.lblFileSelect.TabIndex = 2;
            this.lblFileSelect.Text = "Select File To Parse";
            // 
            // comboBoxFileSelect
            // 
            this.comboBoxFileSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFileSelect.FormattingEnabled = true;
            this.comboBoxFileSelect.Location = new System.Drawing.Point(571, 6);
            this.comboBoxFileSelect.Name = "comboBoxFileSelect";
            this.comboBoxFileSelect.Size = new System.Drawing.Size(334, 22);
            this.comboBoxFileSelect.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(8, 69);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(897, 620);
            this.dataGridView1.TabIndex = 0;
            // 
            // chkParseParam
            // 
            this.chkParseParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkParseParam.AutoSize = true;
            this.chkParseParam.Checked = true;
            this.chkParseParam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkParseParam.Location = new System.Drawing.Point(692, 40);
            this.chkParseParam.Name = "chkParseParam";
            this.chkParseParam.Size = new System.Drawing.Size(133, 18);
            this.chkParseParam.TabIndex = 6;
            this.chkParseParam.Text = "use COMMON_PARSE";
            this.chkParseParam.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 724);
            this.Controls.Add(this.tabControlMain);
            this.Name = "FormMain";
            this.Text = "ForeclosureApp";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabControls.ResumeLayout(false);
            this.groupBoxDR.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabIExplorer.ResumeLayout(false);
            this.tabIExplorer.PerformLayout();
            this.tabREFile.ResumeLayout(false);
            this.tabREFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.RichTextBox richTextBoxMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabControls;
        private System.Windows.Forms.TabPage tabIExplorer;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxDR;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblUnsuccesful;
        private System.Windows.Forms.Label lblProperty;
        private System.Windows.Forms.Label lblSuccessful;
        private System.Windows.Forms.Label lblShowStatus;
        private System.Windows.Forms.Label lblStartPage;
        private System.Windows.Forms.TextBox textBoxStartPage;
        private System.Windows.Forms.Label lblShowUnsuccessful;
        private System.Windows.Forms.Label lblShowSuccessful;
        private System.Windows.Forms.Label lblShowProperty;
        private System.Windows.Forms.Label lblShowPage;
        private System.Windows.Forms.TabPage tabUnsuccessful;
        private System.Windows.Forms.TabPage tabREFile;
        private System.Windows.Forms.Label lblFileSelect;
        private System.Windows.Forms.ComboBox comboBoxFileSelect;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblRE;
        private System.Windows.Forms.TextBox textBoxRE;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.CheckBox chkParseParam;
    }
}

