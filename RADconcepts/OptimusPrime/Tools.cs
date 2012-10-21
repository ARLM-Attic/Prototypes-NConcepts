using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OptimusPrime
{
    public partial class Tools : Form
    {
        public Tools(string mode)
        {            
            
            if (mode == "SiteServer")
            {

            }
            else if (mode == "POS")
            {

            }

            InitializeComponent();
            setup_dataColumns();
        }

        private void setup_dataColumns()
        {
            // 386
            dc_Select.Width = 50;
            dc_Exes.Width = 226;
            dc_Running.Width = 110;

        }



    }
}