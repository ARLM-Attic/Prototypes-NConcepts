namespace BrowserPlugins
{
    partial class ImageViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.infoLabel = new System.Windows.Forms.Label();
            this.zoomIn = new System.Windows.Forms.Button();
            this.zoomOut = new System.Windows.Forms.Button();
            this.zoomFit = new System.Windows.Forms.Button();
            this.zoomTotal = new System.Windows.Forms.Button();
            this.imageScroller = new BrowserPlugins.ImageScroller();
            this.rotate90Button = new System.Windows.Forms.Button();
            this.rotate270Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.Location = new System.Drawing.Point(28, 118);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(300, 31);
            this.infoLabel.TabIndex = 5;
            this.infoLabel.Text = "No Preview Available";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zoomIn
            // 
            this.zoomIn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.zoomIn.Image = global::BrowserPlugins.Properties.Resources.ZoomIn;
            this.zoomIn.Location = new System.Drawing.Point(151, 257);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(24, 24);
            this.zoomIn.TabIndex = 3;
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // zoomOut
            // 
            this.zoomOut.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.zoomOut.Image = global::BrowserPlugins.Properties.Resources.ZoomOut;
            this.zoomOut.Location = new System.Drawing.Point(181, 257);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(24, 24);
            this.zoomOut.TabIndex = 4;
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // zoomFit
            // 
            this.zoomFit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.zoomFit.Image = global::BrowserPlugins.Properties.Resources.BestFit;
            this.zoomFit.Location = new System.Drawing.Point(109, 257);
            this.zoomFit.Margin = new System.Windows.Forms.Padding(15);
            this.zoomFit.Name = "zoomFit";
            this.zoomFit.Size = new System.Drawing.Size(24, 24);
            this.zoomFit.TabIndex = 2;
            this.zoomFit.UseVisualStyleBackColor = true;
            this.zoomFit.Click += new System.EventHandler(this.zoomFit_Click);
            // 
            // zoomTotal
            // 
            this.zoomTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.zoomTotal.Image = global::BrowserPlugins.Properties.Resources.ActualSize;
            this.zoomTotal.Location = new System.Drawing.Point(79, 257);
            this.zoomTotal.Margin = new System.Windows.Forms.Padding(15);
            this.zoomTotal.Name = "zoomTotal";
            this.zoomTotal.Size = new System.Drawing.Size(24, 24);
            this.zoomTotal.TabIndex = 1;
            this.zoomTotal.UseVisualStyleBackColor = true;
            this.zoomTotal.Click += new System.EventHandler(this.zoomTotal_Click);
            // 
            // imageScroller
            // 
            this.imageScroller.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.imageScroller.AutoScroll = true;
            this.imageScroller.Image = null;
            this.imageScroller.Location = new System.Drawing.Point(31, 24);
            this.imageScroller.Name = "imageScroller";
            this.imageScroller.Size = new System.Drawing.Size(297, 227);
            this.imageScroller.TabIndex = 0;
            // 
            // rotate90Button
            // 
            this.rotate90Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.rotate90Button.Image = global::BrowserPlugins.Properties.Resources.RotateClockWise;
            this.rotate90Button.Location = new System.Drawing.Point(223, 257);
            this.rotate90Button.Margin = new System.Windows.Forms.Padding(15);
            this.rotate90Button.Name = "rotate90Button";
            this.rotate90Button.Size = new System.Drawing.Size(24, 24);
            this.rotate90Button.TabIndex = 6;
            this.rotate90Button.UseVisualStyleBackColor = true;
            this.rotate90Button.Click += new System.EventHandler(this.rotate90Button_Click);
            // 
            // rotate270Button
            // 
            this.rotate270Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.rotate270Button.Image = global::BrowserPlugins.Properties.Resources.RotateCounterClockWise;
            this.rotate270Button.Location = new System.Drawing.Point(253, 257);
            this.rotate270Button.Margin = new System.Windows.Forms.Padding(15);
            this.rotate270Button.Name = "rotate270Button";
            this.rotate270Button.Size = new System.Drawing.Size(24, 24);
            this.rotate270Button.TabIndex = 7;
            this.rotate270Button.UseVisualStyleBackColor = true;
            this.rotate270Button.Click += new System.EventHandler(this.rotate270Button_Click);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.rotate90Button);
            this.Controls.Add(this.rotate270Button);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.zoomTotal);
            this.Controls.Add(this.zoomFit);
            this.Controls.Add(this.zoomOut);
            this.Controls.Add(this.zoomIn);
            this.Controls.Add(this.imageScroller);
            this.DoubleBuffered = true;
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(355, 302);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button zoomIn;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.Button zoomFit;
        private System.Windows.Forms.Button zoomTotal;
        private ImageScroller imageScroller;
        private System.Windows.Forms.Button rotate90Button;
        private System.Windows.Forms.Button rotate270Button;
    }
}
