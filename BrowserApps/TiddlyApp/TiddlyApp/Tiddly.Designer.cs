using System.IO;

namespace TiddlyApp
{
    partial class TiddlyApp
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
            string strCurDir = Directory.GetCurrentDirectory();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TiddlyApp));
            this.webBrowserTiddly = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserTiddly
            // 
            this.webBrowserTiddly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserTiddly.Location = new System.Drawing.Point(0, 0);
            this.webBrowserTiddly.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserTiddly.Name = "webBrowserTiddly";
            this.webBrowserTiddly.Size = new System.Drawing.Size(743, 554);
            this.webBrowserTiddly.TabIndex = 0;
            this.webBrowserTiddly.Url = new System.Uri(("file://" + strCurDir + "/" + "tiddly.html"), System.UriKind.Absolute);
            this.webBrowserTiddly.DocumentTitleChanged += new System.EventHandler(this.webBrowserTiddly_DocumentTitleChanged);
            // 
            // TiddlyApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 554);
            this.Controls.Add(this.webBrowserTiddly);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TiddlyApp";
            this.Text = this.webBrowserTiddly.DocumentTitle;
            this.ResumeLayout(false);

        }

        void webBrowserTiddly_DocumentTitleChanged(object sender, System.EventArgs e)
        {
            this.Text = this.webBrowserTiddly.DocumentTitle;            
        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserTiddly;
    }
}

