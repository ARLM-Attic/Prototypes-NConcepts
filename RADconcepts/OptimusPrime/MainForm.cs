using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OptimusPrime
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }        

        private void Siteserver_Tools_button_Click(object sender, EventArgs e)
        {
            Tools tools = new Tools("SiteServer");
            tools.FormClosed += new FormClosedEventHandler(tools_FormClosed);
            tools.Show();
            this.Hide();
        }

        private void POS_explorer_button_Click(object sender, EventArgs e)
        {
            
        }

        private void POS_Tools_button_Click(object sender, EventArgs e)
        {
            Tools tools = new Tools("POS");            
            tools.FormClosed += new FormClosedEventHandler(tools_FormClosed);
            tools.Show();
            this.Hide();
        }

        private void tools_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool dan = true;
            
            this.Close();
        }
    }
}