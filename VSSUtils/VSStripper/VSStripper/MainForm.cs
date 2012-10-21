using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace VSStripper
{    

    public partial class MainForm : Form
    {
        private string m_strBaseDirectory;
        private const string PROGRESS_DONE = " ******* DONE ******* ";

        enum LineChanges { none, skip, modified }
        private delegate void ProcessFile(string fileNameAndPath);
        private delegate LineChanges ProcessLine(ref string line);

        // Line change State Variables (not ideal) - change later
        private bool m_bIsGlobalSection = false;

        private void DisableAllButtons()
        {
            Iterate_All_Buttons(false);
        }

        private void EnableAllButtons()
        {
            Iterate_All_Buttons(true);
        }
        
        private void Iterate_All_Buttons(bool bEnable)
        {
            foreach (Control cr in this.Controls)
            {
                if(cr.Name.Contains("gBox"))
                {
                    GroupBox gb = (GroupBox)cr;

                    foreach (Control cr2 in gb.Controls)
                    {
                        if (cr2.Name.Contains("btn"))
                        {
                            Button bt = (Button)cr2;
                            bt.Enabled = bEnable;                            
                            bt.Refresh();                            
                        }
                    }
                }
            }
        }

        private void ShowProgress(string line)
        {
            const int MAX_LBL_SIZE = 40;            

            if (line.Length >= MAX_LBL_SIZE)
            {
                lblShowProcessedFile.Text = "..." + line.Substring(line.Length - MAX_LBL_SIZE);
            }
            else
            {
                lblShowProcessedFile.Text = line;
            }

            lblShowProcessedFile.Refresh();

            if (line == PROGRESS_DONE)
            {
                EnableAllButtons();
            }
        }

        public MainForm()
        {
            InitializeComponent();

            const int MAX_LBL_SIZE = 35;
            m_strBaseDirectory = Directory.GetCurrentDirectory();

            if (m_strBaseDirectory.Length >= MAX_LBL_SIZE)
            {
                lblShowCurrentFolder.Text = "..." + m_strBaseDirectory.Substring(m_strBaseDirectory.Length - MAX_LBL_SIZE);
            }
            else
            {
                lblShowCurrentFolder.Text = m_strBaseDirectory;
            }            

            // Clear ShowProgress (now shows nothing)
            ShowProgress("");

            // Set up the delays for the ToolTip.
            ttipShowCurrentFolder.AutoPopDelay = 2000;
            ttipShowCurrentFolder.InitialDelay = 500;
            ttipShowCurrentFolder.ReshowDelay = 500;
            
            // Force the ToolTip text to be displayed whether or not the form is active.
            ttipShowCurrentFolder.ShowAlways = true;

            // Set up the ToolTip text for the label
            ttipShowCurrentFolder.SetToolTip(this.lblShowCurrentFolder, m_strBaseDirectory);
        }

        private void btnStripeVSS_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Control cr in gBoxVssVstsBindings.Controls)
            {
                if (cr.Name.Contains("chk"))
                {
                    CheckBox cb = (CheckBox)cr; 
                    if(cb.Checked)
                    {
                        sb.Append(cb.Tag.ToString() + " ");
                    }
                }
            }

            if(sb.Length >= 4)  // has at least one file extension
            {
                DisableAllButtons();
                IterateAllDirectoriesInFor(m_strBaseDirectory, sb.ToString(), ProcessFile_VSS_Striping);
                ShowProgress(PROGRESS_DONE);
                EnableAllButtons();
            }
            
        }

        private void ProcessFile_VSS_Striping(string fileNameAndPath)
        {            
            DirectoryInfo di = new DirectoryInfo(fileNameAndPath);
            string path = Path.GetDirectoryName(di.ToString());
            string file = di.Name;

            bool bIsNewFileCreated = false;            

            // Sadly we need to do a switch here and process different file types differently
            switch (di.Extension)
            {
                case ".dsp":
                case ".vcp":
                    bIsNewFileCreated = Iterate_File_And_Make_Changes(path, file, ProcessLine_dspAndvcp);
                    break;
                case ".vcproj":
                    bIsNewFileCreated = Iterate_File_And_Make_Changes(path, file, ProcessLine_vcproj);
                    break;
                case ".csproj":
                    bIsNewFileCreated = Iterate_File_And_Make_Changes(path, file, ProcessLine_csproj);
                    break;
                case ".sln":
                    bIsNewFileCreated = Iterate_File_And_Make_Changes(path, file, ProcessLine_sln);
                    break;
            }

            if (bIsNewFileCreated)
            {
                try
                {                   
                    // Overwrite and Backup Logic here
                    if (chkBackupFiles.Checked)
                    {
                        if (File.Exists(fileNameAndPath + ".backup"))                            
                        {
                            if(chkOverwriteBackupFiles.Checked)
                            {
                                File.Copy(fileNameAndPath, (fileNameAndPath + ".backup"));
                            }
                        }
                        else
                        {
                            File.Copy(fileNameAndPath, (fileNameAndPath + ".backup"));
                        }
                    }                                        

                    // We are going to delete the original
                    MarkFileAs(fileNameAndPath, FileAttributes.Normal);
                    File.Delete(fileNameAndPath);
                    File.Move((fileNameAndPath + ".new"), fileNameAndPath);
                    // we can now delete the .new file
                    File.Delete((fileNameAndPath + ".new"));                    
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void ProcessFile_Remove_Backup(string fileNameAndPath)
        {
            ShowProgress(fileNameAndPath);
            File.Delete(fileNameAndPath);
        }
        
        private void ProcessFile_Restore_Projects_From_Backup(string fileNameAndPath)
        {

            try
            {
                // For Some reason Trim End would fail on some files so i am doing it this way
                string fileNameAndPathToRestoreTo = fileNameAndPath.Substring(0, fileNameAndPath.Length - ".backup".Length);
                // string fileNameAndPathToRestoreTo = fileNameAndPath.TrimEnd(".backup".ToCharArray());

                ShowProgress(fileNameAndPathToRestoreTo);

                // Just in case
                MarkFileAs(fileNameAndPathToRestoreTo, FileAttributes.Normal);

                File.Copy(fileNameAndPath, fileNameAndPathToRestoreTo, true);

            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private LineChanges ProcessLine_sln(ref string line)
        {
            ////
            // We want to skipe the entire section GlobalSection(SourceControl)
            // the global var keeps the state whether it exist (has been found)
            ////            
            if (line.Contains("GlobalSection(SourceCodeControl)"))
            {
                m_bIsGlobalSection = true;
                return LineChanges.skip;
            }
            else if (!(line.Contains("EndGlobalSection")) && m_bIsGlobalSection)
            {
                return LineChanges.skip;
            }
            else if (line.Contains("EndGlobalSection") && m_bIsGlobalSection)
            {
                m_bIsGlobalSection = false;
                return LineChanges.skip;
            }
            else
            {
                return LineChanges.none;
            }
        }

        private LineChanges ProcessLine_vcproj(ref string line)
        {
            // Get Rid of annoying VSS bindings!!!
            if (line.Contains("SccProjectName="))
            {
                return LineChanges.skip;
            }
            else if (line.Contains("SccLocalPath="))
            {
                return LineChanges.skip;
            }
            else if (line.Contains("SccProvider="))
            {
                return LineChanges.skip;
            }
            else if (line.Contains("SccAuxPath="))
            {
                return LineChanges.skip;
            }
            else
            {
                return LineChanges.none;
            }
        }

        private LineChanges ProcessLine_csproj(ref string line)
        {
            // Get Rid of annoying VSS bindings!!!
            if (line.Contains("SccProjectName"))
            {
                return LineChanges.skip;
            }
            else if (line.Contains("SccLocalPath"))
            {
                return LineChanges.skip;
            }
            else if (line.Contains("SccProvider"))
            {
                return LineChanges.skip;
            }
            else if (line.Contains("SccAuxPath"))
            {
                return LineChanges.skip;
            }
            else
            {
                return LineChanges.none;
            }
        }

        private LineChanges ProcessLine_dspAndvcp(ref string line)
        {
            // Get Rid of annoying VSS bindings!!!
            if (line.Contains("PROP Scc_ProjName"))
            {
                return LineChanges.skip;
            }
            else if (line.Contains("# PROP Scc_LocalPath"))
            {
                return LineChanges.skip;
            }
            else
            {
                return LineChanges.none;
            }            
        }

        private bool Iterate_File_And_Make_Changes(string path, string file, ProcessLine pl)
        {
            bool bAChangeIsMade = false;

            using (TextWriter writer = File.CreateText((path + '\\' + file + ".new")))
            {
                using (TextReader reader = File.OpenText(path + '\\' + file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        
                        // Make a line Change (passed by ref)
                        LineChanges change = pl(ref line);
                        
                        if (change == LineChanges.skip)
                        {
                            bAChangeIsMade = true;
                            continue; // ignore this line
                        }
                        else if (change == LineChanges.modified)
                        {
                            bAChangeIsMade = true;
                            writer.WriteLine(line); // write the modified line to file
                        }
                        else
                        {
                            // No Change is made so just write the original line
                            writer.WriteLine(line);
                        }                                                
                    }
                    reader.Close();
                }
                writer.Close();
            }

            if (!bAChangeIsMade)
            {
                // If there was no change made no need to keep the .new file
                File.Delete((path + '\\' + file + ".new"));
                return false;
            }
            else
            {
                return true; // A change was made (.new file created)
            }
        }
       
        private void IterateAllDirectoriesInFor(string Path, string LookForFileExtensions, ProcessFile processFunc)
        {
            DirectoryInfo dir = new DirectoryInfo(Path);

            foreach (FileInfo file in dir.GetFiles())
            {
                if (LookForFileExtensions.ToUpper().Contains(file.Extension.ToUpper())
                    && ( file.Extension != "")
                    )
                {
                    ShowProgress(file.FullName);
                    processFunc(file.FullName);
                }
                else
                {
                    continue;
                }
            }

            // Now Copy Each SubDirectory using recursion
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                IterateAllDirectoriesInFor(subdir.FullName, LookForFileExtensions, processFunc);
            }
        }

        private void btnRemoveAllBackup_Click(object sender, EventArgs e)
        {
            
            DialogResult result;
            
            result = MessageBox.Show(this, "This will remove all project Backup Files - you should only do this when you just did a GetLatest on you entire source tree", "Are You sure?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                DisableAllButtons();
                IterateAllDirectoriesInFor(m_strBaseDirectory, ".backup", ProcessFile_Remove_Backup);
                ShowProgress(PROGRESS_DONE);
                EnableAllButtons();
            }

        }

        private void btnRestoreProjectFiles_Click(object sender, EventArgs e)
        {

            DialogResult result;

            result = MessageBox.Show(this, "Make Sure that your VS is closed!", "Is Visual Studio Closed?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                DisableAllButtons();
                IterateAllDirectoriesInFor(m_strBaseDirectory, ".backup", ProcessFile_Restore_Projects_From_Backup);
                ShowProgress(PROGRESS_DONE);
                EnableAllButtons();
            }
        }

        private void MarkFileAs(string fileAndPath, FileAttributes attr)
        {
            FileInfo file = new FileInfo(fileAndPath);
            file.Attributes = attr;
        }
        
        public void setAllFilesInDirTo(string path, FileAttributes attr)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo file in dir.GetFiles())
            {
                ShowProgress(file.FullName);
                file.Attributes = attr;
            }

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                setAllFilesInDirTo(subdir.FullName, attr);
            }            
        }
        
        private void btnMarkReadOnly_Click(object sender, EventArgs e)
        {
            DisableAllButtons();
            setAllFilesInDirTo(m_strBaseDirectory, FileAttributes.ReadOnly);
            ShowProgress(PROGRESS_DONE);
        }

        private void btnMarkWritable_Click(object sender, EventArgs e)
        {
            DisableAllButtons();
            setAllFilesInDirTo(m_strBaseDirectory, FileAttributes.Normal);
            ShowProgress(PROGRESS_DONE);
        }

    }
}