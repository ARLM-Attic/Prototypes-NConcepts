using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SMSharp
{
    public partial class FrmNewOpen : Form
    {
        public FrmNewOpen()
        {
            InitializeComponent();
            
            listBoxNEW_OPEN.SelectedIndex = 0;                        
        }
        //////////////////////////////////////////////////////////////////////
        // Clss: buttonOK_Click
        // Desc: Open the type of Window Specified in the listBox
        //////////////////////////////////////////////////////////////////////
        private void buttonOK_Click(object sender, EventArgs e)
        {
            FrmMainMdiParent frmMain = (FrmMainMdiParent)this.MdiParent;
            
            switch (listBoxNEW_OPEN.Text)
            {
                case "JagLogViewer":                    
                    frmMain.CreateChildWindowInstance("JagLogViewer");
                    this.Close();
                    break;

                case "ServerTool":
                    frmMain.CreateChildWindowInstance("ServerTool");
                    this.Close();
                    break;

                case "FuelConsole":
                    frmMain.CreateChildWindowInstance("FuelConsole");
                    this.Close();
                    break;

                case "SMLogViewer":
                    frmMain.CreateChildWindowInstance("SMLogViewer");
                    this.Close();
                    break;

                default:
                    return;
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBoxNEW_OPEN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                buttonOK_Click(sender, e);
            }
        }

    }
}