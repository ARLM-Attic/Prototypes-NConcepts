using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Arachnid;
using System.IO;
using System.Web;

namespace EBook_Creator
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Verify.isValidStr(textBox1.Text) &&
               Directory.Exists(textBox1.Text))
            {

                FileM indexFile = new FileM("__index__", "htm", textBox1.Text, true);
                FileM outlineFile = new FileM("__outline__","htm", textBox1.Text,true);                
                FileM indexForShowFile = new FileM("index","htm", textBox1.Text,true);

                // Prompt before overide
                if (File.Exists(indexForShowFile.PathNFile))
                {
                    MessageBoxButtons bM = MessageBoxButtons.YesNo;
                    DialogResult dR = MessageBox.Show("index.htm exists in this directory - won't overide it - exiting... Check and make sure you want to override it", "Prompt before Overide - Overide Yes/No", bM);

                    // exit unless the user says yes
                    if (dR != DialogResult.Yes)
                    {
                        return;
                    }                    
                }

                DirectoryInfo di = new DirectoryInfo(textBox1.Text);
                FileInfo[] files = di.GetFiles("*.htm", SearchOption.AllDirectories);    // gets all the htm & html files in the directory & subdirectories

                // First create FramePage / index file
                indexFile.WriteLineUTF8("<html><header><meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\"><title>__index__</title>");
                indexFile.WriteLineUTF8("<script> function op() { } </script>");  // see online docs if necessary
                
                // Construct the Frame
                indexFile.WriteLineUTF8("<frameset cols=\"25%,*\" frameborder=\"0\" framespacing=\"0\" border=\"0\">");
                indexFile.WriteLineUTF8("<frame name=\"treeframe\" src=\"__outline__.htm\" >");                               
                indexFile.WriteLineUTF8("<frame name=\"mainus\" src=\"" + files[0].Name + "\">");
                indexFile.WriteLineUTF8("</frameset></header><body>");                

                // Write out Index For Show File
                indexForShowFile.WriteLineUTF8("<html><header><meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\"><title>Index File</title></head>");
                indexForShowFile.WriteLineUTF8("<body>Select Table of Page From Table of Contents<br><br></body></html>");

                // Now Create Outline File
                outlineFile.WriteLineUTF8("<html><header><title>Outline File</title><base target=\"mainus\"></header><body>");
                foreach (FileInfo file in files)
                {
                    // skip index file if already existed
                    if (file.Name == "__index__.htm" || file.Name == "__outline__.htm" || file.Name == "index.htm") continue;
                    
                    // is it a frontpage extension dir?
                    if (file.DirectoryName.Contains("_vti"))
                    {
                        continue; //if so, skip
                    }

                    string path = "";

                    // if it is a subdirectory?
                    if(file.DirectoryName.Length > textBox1.Text.Length)
                    {
                        path = file.DirectoryName.Substring(textBox1.Text.Length + 1);  // we want to omitt the \\
                        path = path.Replace('\\','/'); // replace all backward slashes
                        path += "/"; //it's a subdirectory so add this to the path so that we can display the file
                    }
                   
                    outlineFile.WriteLineUTF8("<a href=\"" + Uri.EscapeUriString(path)+ Uri.EscapeUriString(file.Name) + "\">" 
                                                           + file.Name + "</a><br><br>");
                }
                outlineFile.WriteLineUTF8("</body></html>");


                MessageBox.Show("__index__.htm and __outline__.htm created in target directory");

            }
            else
            {
                MessageBox.Show("Folder specified is incorrect");
            }
            
        }
    }
}