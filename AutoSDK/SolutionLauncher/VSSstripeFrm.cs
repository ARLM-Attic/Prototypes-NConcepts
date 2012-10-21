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
    public partial class VSSstripeFrm : Form
    {
        private string m_dir;

        public VSSstripeFrm(int left, int top, string Project, string Version, string directory)
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
            IterateAllDirectories(m_dir); 
            this.labelDisplayProcessing.Text = "Done!";
            this.labelDisplayProcessing.Refresh();
            this.Close();
        }

        private void IterateAllDirectories(string directory)
        {

            DirectoryInfo dir = new DirectoryInfo(directory);

            foreach (FileInfo file in dir.GetFiles())
            {

                if ((file.Extension.ToLower() == ".dsp") || (file.Extension.ToLower() == ".vcp"))
                {
                    this.labelDisplayProcessing.Text = file.Name.ToString();
                    this.labelDisplayProcessing.Refresh();
                    stripeVSS(file.FullName);
                }
                else
                {
                    continue;
                }

            }

            // Now Copy Each SubDirectory using recursion
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                IterateAllDirectories(subdir.FullName);
            }
        }

        private void stripeVSS(string file)
        {
            
            string Path = file.Substring(0, (file.LastIndexOf('\\') + 1));
            string FileName = file.Substring((file.LastIndexOf('\\') + 1), (file.Length - (file.LastIndexOf('\\') + 1)));

            using (TextWriter writer = File.CreateText((Path + FileName + ".new")))
            {
                using (TextReader reader = File.OpenText(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Get Rid of annoying VSS bindings!!!
                        if (line.Contains("PROP Scc_ProjName"))
                        {
                            continue;
                        }
                        else if (line.Contains("# PROP Scc_LocalPath"))
                        {
                            continue;
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                    reader.Close();
                }
                writer.Close();
            }
            
            File.Delete(file);
            File.Move((Path + FileName + ".new"), file);                                              
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}