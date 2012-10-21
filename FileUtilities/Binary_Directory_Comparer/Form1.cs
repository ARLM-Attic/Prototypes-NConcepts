using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Binary_Directory_Comparer
{
    public partial class Form1 : Form
    {
        // private String 

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrows1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtBrowse1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtBrowse2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check that everything is cool
            if (txtBrowse1.Text != "" && txtBrowse2.Text != "" &&
               Directory.Exists(txtBrowse1.Text) &&
               Directory.Exists(txtBrowse2.Text)
               )
            {
                // Clear Output Window!
                richTextBox1.Clear();

                //Start -- no subdirs for now
                CheckAllFilesInDir1AgainstDir2(txtBrowse1.Text, txtBrowse2.Text, false);
            }
            else
            {
                MessageBox.Show("Check Directories!");
            }
        }

        private void CheckAllFilesInDir1AgainstDir2(string dirPath1, string dirPath2, bool bIterateSubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath1);
            
            foreach (FileInfo file in dir.GetFiles())
            {

                if (File.Exists(dirPath2 + '\\' + file.Name))
                {
                    FileInfo file2 = new FileInfo(dirPath2 + '\\' + file.Name);

                    if (file.Length < file2.Length)
                    {
                        richTextBox1.AppendText("File: " + file.Name + " is smaller in DirPath1 \n");
                    }
                    else if (file.Length == file2.Length)
                    {
                        richTextBox1.AppendText("File: " + file.Name + " is equal to DirPath2 \n");
                    }
                    else if (file.Length > file2.Length)
                    {
                        richTextBox1.AppendText("File: " + file.Name + " is greater to DirPath2 \n");
                    }                    
                }
                else
                {
                    richTextBox1.AppendText("File: " + file.Name + " only exists in DirPath1 \n");
                }
            }

            if (bIterateSubDirs)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    if (Directory.Exists(dirPath2 + '\\' + subdir.Name))
                    {
                        CheckAllFilesInDir1AgainstDir2(dirPath1 + '\\' + subdir.Name, dirPath2 + '\\' + subdir.Name, bIterateSubDirs);
                    }
                    else
                    {
                        richTextBox1.AppendText("Dir: " + subdir.Name + " only exists in DirPath1 \n");
                    }
                }
            }            
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}