using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BrowserPlugins
{
    public partial class ImageScroller : UserControl
    {
        private int zoomValue;

        public ImageScroller()
        {
            InitializeComponent();
            zoomValue = 0;
        }

        public Image Image
        {
            get { return imageBox.Image; }
            set 
            {
                imageBox.Image = value;

                if (value != null)
                    ZoomFit();
                else
                    imageBox.SetBounds(0, 0, this.Width, this.Height);
            }
        }

        public void ResetView()
        {
            if (imageBox.Image != null)
                ZoomFit();
            else
                imageBox.SetBounds(0, 0, this.Width, this.Height);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Image != null)
                SetNewSize();

            base.OnResize(e);
        }

        #region Zoom

        private void SetNewSize()
        {
            int newWidth, newHeight;

            newHeight = (int)((double)Math.Abs(this.Height - Image.Height) / 9) * zoomValue +
                Math.Min(this.Height, Image.Height);
            newWidth = (int)((double)(Image.Width * newHeight) / (double)Image.Height);

            if (zoomValue == 0 && newWidth > this.Width)
            {
                newWidth = (int)((double)Math.Abs(this.Width - Image.Width) / 9) * zoomValue +
                    Math.Min(this.Width, Image.Width);
                newHeight = (int)((double)(Image.Height * newWidth) / (double)Image.Width);
            }

            int x = newWidth > this.Width ? 0 : (this.Width - newWidth) / 2;
            int y = newHeight > this.Height ? 0 : (this.Height - newHeight) / 2;

            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;

            imageBox.SetBounds(x, y, newWidth, newHeight);
        }

        public void ZoomIn()
        {
            if (Image != null && zoomValue < 9)
            {
                zoomValue++;
                SetNewSize();
            }
        }

        public void ZoomOut()
        {
            if (Image != null && zoomValue > 0)
            {
                zoomValue--;
                SetNewSize();
            }
        }

        public void ZoomFit()
        {
            if (Image != null)
            {
                zoomValue = 0;
                SetNewSize();
            }
        }

        public void ZoomTotal()
        {
            if (Image != null)
            {
                zoomValue = 9;
                SetNewSize();
            }
        }

        #endregion
    }
}
