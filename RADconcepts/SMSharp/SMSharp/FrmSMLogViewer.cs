using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SMSharp
{
    public partial class FrmSMLogViewer : Form
    {
        public FrmSMLogViewer()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmSMLogViewer_Activated(object sender, EventArgs e)
        {            

        }

        private void FrmSMLogViewer_Load(object sender, EventArgs e)
        {
            toolStripTitleTextBox.Text = this.Text;
        }

        private void toolStripTitleTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.Text = toolStripTitleTextBox.Text + " [SMLogViewer]";
            }
        }

    }
}