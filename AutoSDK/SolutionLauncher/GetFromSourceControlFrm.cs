using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace SolutionLauncher
{
    public partial class GetFromSourceControlFrm : Form
    {
        private Settings MySet;

        public GetFromSourceControlFrm(int left, int top, ref Settings settings)
        {
            InitializeComponent();

            this.Left = left;
            this.Top = top;
            this.MySet = settings;
            this.Location = new System.Drawing.Point(left, top);
       
            this.labelDisplayProject.Text = MySet.ProjectType;
            this.labelDisplayVersion.Text = MySet.ProjectVersion;
            this.labelDisplayProcessing.Text = "N/A";
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            GetFromSourceControl(); 
            this.labelDisplayProcessing.Text = "Done!";
            this.labelDisplayProcessing.Refresh();
            this.Close();
        }

        private void GetFromSourceControl()
        {
            //RegistryKey curr = new RegistryKey(MySet.ProjectTypes.OpenSubKey(MySet.ProjectType));
            
            //object sourcecontrol = curr.GetValue("SourceControl");
            //object sourcecontrolpath = curr.GetValue("SourceControlPath");

            //if (MySet.isValidStr(sourcecontrol) && MySet.isValidStr(sourcecontrolpath))
            //{
            //    if (sourcecontrol.ToString().ToLower() == "vss")
            //    {

            //    }
            //    else if (sourcecontrol.ToString().ToLower() == "vsts")
            //    {
            //    }
            //    else
            //    {
            //        MySet.Alert("Source control not setup correctly for project " + MySet.ProjectType);
            //    }
            //}
            //else
            //{
            //    MySet.Alert("Source control not setup for project " + MySet.ProjectType);
            //    this.Close();
            //}
                        
        }        

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetFromSourceControlFrm_Load(object sender, EventArgs e)
        {

        }

    }
}