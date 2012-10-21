using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SolutionLauncher
{
    public partial class MarkAsWritableFrm : Form
    {
        private string m_dir;

        public MarkAsWritableFrm(int left, int top, string Project, string Version, string directory)
        {
            InitializeComponent();

            this.Left = left;
            this.Top = top;
            this.Location = new System.Drawing.Point(left, top);

            m_dir = directory;
            this.labelDisplayProject.Text = Project;
            this.labelDisplayVersion.Text = Version;
            this.labelDisplayProcessing.Text = "N/A";
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            setAllFilesInDirToNormal(m_dir); 
            this.labelDisplayProcessing.Text = "Done!";
            this.labelDisplayProcessing.Refresh();
            this.Close();
        }

        public void setAllFilesInDirToNormal(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                this.labelDisplayProcessing.Text = file.FullName;
                this.labelDisplayProcessing.Refresh();
                file.Attributes = FileAttributes.Normal;
            }

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                setAllFilesInDirToNormal((dir + "\\" + subdir.ToString()));
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}