using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FileBrowser;
using ShellDll;
using System.Collections;
using System.IO;
using System.Drawing.Imaging;

namespace BrowserPlugins
{
    public partial class ImageViewer : UserControl, IViewPlugin
    {
        private Stream imageStream;
        private ImageFormat imageFormat;

        public ImageViewer()
        {
            InitializeComponent();

            this.imageScroller.MouseWheel += new MouseEventHandler(imageScroller_MouseWheel);
            foreach (Control control in this.Controls)
                control.MouseWheel += new MouseEventHandler(imageScroller_MouseWheel);
        }

        #region IViewPlugin Members

        public void Reset()
        {
            if (imageScroller.Image != null)
            {
                imageScroller.Image.Dispose();
                imageScroller.Image = null;
            }

            rotate90Button.Enabled = false;
            rotate270Button.Enabled = false;
            EnableViewButtons(false);

            SetInfoText("No Preview Available");
        }

        public void FileSelected(IFileInfoProvider provider, ShellItem item)
        {
            try
            {
                if (imageScroller.Image != null)
                {
                    imageScroller.Image.Dispose();
                    imageScroller.Image = null;
                }

                SetInfoText("Generating Preview...");

                imageStream = provider.GetFileStream();

                rotate90Button.Enabled = imageStream.CanWrite;
                rotate270Button.Enabled = imageStream.CanWrite;

                EnableViewButtons(true);

                if (imageStream.Length < 50000000)
                {
                    imageScroller.Image = Image.FromStream(imageStream);
                    imageFormat = imageScroller.Image.RawFormat;
                    SetInfoText(string.Empty);
                }
            }
            catch (Exception)
            {
                Reset();
            }
        }

        public void FolderSelected(IDirInfoProvider provider, ShellItem item)
        {
            Reset();
        }

        public Control ViewControl
        {
            get { return this; }
        }

        public string ViewName
        {
            get { return "Image View"; }
        }

        #endregion

        #region IBrowserPlugin Members

        public string Info
        {
            get { return "Plugin for viewing images"; }
        }

        string IBrowserPlugin.Name
        {
            get { return "Image View Plugin"; }
        }

        #endregion

        #region Zoom

        void imageScroller_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                imageScroller.ZoomIn();
            else
                imageScroller.ZoomOut();
        }

        private void zoomTotal_Click(object sender, EventArgs e)
        {
            imageScroller.ZoomTotal();
        }

        private void zoomFit_Click(object sender, EventArgs e)
        {
            imageScroller.ZoomFit();
        }

        private void zoomIn_Click(object sender, EventArgs e)
        {
            imageScroller.ZoomIn();
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            imageScroller.ZoomOut();
        }

        #endregion

        #region Other Methods

        private void SetInfoText(string text)
        {
            if (infoLabel.Visible == string.IsNullOrEmpty(text))
                infoLabel.Visible = !string.IsNullOrEmpty(text);

            infoLabel.Text = text;
            Application.DoEvents();
        }

        private void EnableViewButtons(bool enable)
        {
            zoomTotal.Enabled = enable;
            zoomFit.Enabled = enable;
            zoomIn.Enabled = enable;
            zoomOut.Enabled = enable;
        }

        private void rotate90Button_Click(object sender, EventArgs e)
        {
            imageScroller.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            imageScroller.ResetView();
            imageStream.Seek(0, SeekOrigin.Begin);
            imageScroller.Image.Save(imageStream, imageFormat);
        }

        private void rotate270Button_Click(object sender, EventArgs e)
        {
            imageScroller.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            imageScroller.ResetView();
            imageStream.Seek(0, SeekOrigin.Begin);
            imageScroller.Image.Save(imageStream, imageFormat);
        }

        #endregion
    }
}

