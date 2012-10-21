namespace SMSharp
{
    partial class FrmJagLogViewer
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
            this.toolStripJagLogViewer = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.dataGridViewJagLogs = new System.Windows.Forms.DataGridView();
            this.ColumnNode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEventID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTextDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJagLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripJagLogViewer
            // 
            this.toolStripJagLogViewer.AllowDrop = true;
            this.toolStripJagLogViewer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripJagLogViewer.Location = new System.Drawing.Point(0, 0);
            this.toolStripJagLogViewer.Name = "toolStripJagLogViewer";
            this.toolStripJagLogViewer.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripJagLogViewer.Size = new System.Drawing.Size(566, 25);
            this.toolStripJagLogViewer.TabIndex = 0;
            this.toolStripJagLogViewer.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 331);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(566, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // dataGridViewJagLogs
            // 
            this.dataGridViewJagLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewJagLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNode,
            this.ColumnTime,
            this.ColumnModule,
            this.ColumnEventID,
            this.ColumnTextDescription});
            this.dataGridViewJagLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewJagLogs.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewJagLogs.Name = "dataGridViewJagLogs";
            this.dataGridViewJagLogs.RowHeadersVisible = false;
            this.dataGridViewJagLogs.Size = new System.Drawing.Size(566, 306);
            this.dataGridViewJagLogs.TabIndex = 2;
            // 
            // ColumnNode
            // 
            this.ColumnNode.HeaderText = "Node";
            this.ColumnNode.Name = "ColumnNode";
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.Name = "ColumnTime";
            // 
            // ColumnModule
            // 
            this.ColumnModule.HeaderText = "Module";
            this.ColumnModule.Name = "ColumnModule";
            // 
            // ColumnEventID
            // 
            this.ColumnEventID.HeaderText = "Event Id";
            this.ColumnEventID.Name = "ColumnEventID";
            // 
            // ColumnTextDescription
            // 
            this.ColumnTextDescription.HeaderText = "Text Description";
            this.ColumnTextDescription.Name = "ColumnTextDescription";
            // 
            // FrmJagLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 353);
            this.Controls.Add(this.dataGridViewJagLogs);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripJagLogViewer);
            this.Name = "FrmJagLogViewer";
            this.Text = "FrmJagLogViewer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJagLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripJagLogViewer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridViewJagLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEventID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTextDescription;
    }
}