using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Drawing;

namespace BrowserPlugins
{
    internal class ImageBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (this.Image != null)
            {
                Rectangle imageRect = ImageRectangleFromSizeMode(this.SizeMode);
                pe.Graphics.DrawRectangle(Pens.Black, imageRect);
            }
        }

        private Rectangle ImageRectangleFromSizeMode(PictureBoxSizeMode mode)
        {
            Rectangle rect = DeflateRect(base.ClientRectangle, base.Padding);
            if (this.Image != null)
            {
                switch (mode)
                {
                    case PictureBoxSizeMode.Normal:
                    case PictureBoxSizeMode.AutoSize:
                        rect.Size = this.Image.Size;
                        return rect;

                    case PictureBoxSizeMode.StretchImage:
                        return rect;

                    case PictureBoxSizeMode.CenterImage:
                        rect.X += (rect.Width - this.Image.Width) / 2;
                        rect.Y += (rect.Height - this.Image.Height) / 2;
                        rect.Size = this.Image.Size;
                        return rect;

                    case PictureBoxSizeMode.Zoom:
                        {
                            Size imageSize = this.Image.Size;
                            float max = Math.Min(
                                (float)(((float)base.ClientRectangle.Width) / ((float)imageSize.Width)), 
                                (float)(((float)base.ClientRectangle.Height) / ((float)imageSize.Height)));
                            rect.Width = (int)(imageSize.Width * max);
                            rect.Height = (int)(imageSize.Height * max);
                            rect.X = (base.ClientRectangle.Width - rect.Width) / 2;
                            rect.Y = (base.ClientRectangle.Height - rect.Height) / 2;
                            return rect;
                        }
                }
            }
            return rect;
        }

        private Rectangle DeflateRect(Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }
    }
}
