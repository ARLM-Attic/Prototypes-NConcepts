using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace SyncFiles
{
    //////////////////////////////////////////////////////////////////////
    // Clss: Program    
    //////////////////////////////////////////////////////////////////////
    class Program
    {
        //////////////////////////////////////////////////////////////////////
        // Func: ShowCommandHelp
        // Desc: Writes the HowTo manual out to the command line
        //////////////////////////////////////////////////////////////////////
        static void ShowCommandHelp()
        {       
            StringBuilder sb = new StringBuilder();
            sb.Append("\nSyncFiles Helps you to quickly syncronize your files \n\n");
            sb.Append("   Syntax: SyncFiles -s \"[dir]\" -d \"[dir]\" -o \"[Options]\" \n\n");
            sb.Append("   Parameters:\n");
            sb.Append("   -s \"[dir]\"      - specify a source directory to sync from \n");
            sb.Append("   -d \"[dir]\"      - specify a destination directory to sync to \n\n");
            sb.Append("   Options:\n");
            sb.Append("   -f  - create the destination direction if it doesn't exist \n");
            sb.Append("   -b  - enable bi-direction sync (source to destination and back)\n");
            sb.Append("   -m  - disable File Size Check (only datetime will be checked)\n");
            sb.Append("   -e  - only copy files over that exist in the other directory \n");
            sb.Append("   -s  - *exclusive* option will force all files to be copied \n");
            sb.Append("         options b,m,e options are ignored. Use this to force a backup \n\n");
            sb.Append("   Example:\n");
            sb.Append("   SyncFiles -s \"c:\\SrcDir\" -d \"c:\\DstDir\" \n");
            sb.Append("   SyncFiles -s \"c:\\SrcDir\" -d \"c:\\DstDir\" -o \"fmeb\" \n");
            sb.Append("   SyncFiles -s \"c:\\SrcDir\" -d \"c:\\DstDir\" -o \"mb\" \n");            
            sb.Append("   SyncFiles -s \"c:\\SrcDir\" -d \"c:\\DstDir\" -o \"fs\" \n");
            sb.Append("   SyncFiles -s \"c:\\SrcDir\" -d \"c:\\DstDir\" -o \"s\" \n");
            Console.WriteLine(sb.ToString());
        }
        //////////////////////////////////////////////////////////////////////
        // Func: Main
        // Desc: Execution Entry Point
        //////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
            const int MIN_ARG_LENGTH = 4;

            if (args.Length < MIN_ARG_LENGTH)
            {
                ShowCommandHelp();
                return;
            }

            int argsFound = 0;
            string strSourceDir = "";
            string strDestDir   = "";
            string strOptions   = "";
            bool DestDirIsInvalid = false;
            
            // Options
            bool optCreateDestDir = false;

            for(int i = 0; i < args.Length; ++i)
            {
                if (args[i].ToLower() == "-s" && (args.Length >= MIN_ARG_LENGTH))
                {
                    ++argsFound;
                    strSourceDir = args[i + 1];
                    ++i;

                    if (!Directory.Exists(strSourceDir))
                    {
                        Console.WriteLine("-s " + strSourceDir + " invalid. \n\n");
                        ShowCommandHelp();
                        return;
                    }                    
                }
                else if (args[i].ToLower() == "-d" && (args.Length >= MIN_ARG_LENGTH))
                {
                    ++argsFound;
                    strDestDir = args[i + 1];
                    ++i;

                    if (!Directory.Exists(strDestDir))
                    {
                        DestDirIsInvalid = true;
                    }                    
                }
                else if (args[i].ToLower() == "-o" && (args.Length >= MIN_ARG_LENGTH))
                {
                    ++argsFound;
                    strOptions = args[i + 1].ToLower();
                    ++i;

                    if(strOptions.Contains("f"))
                    {
                        optCreateDestDir = true;
                    }                                                           
                }
            }

            ////
            // Passed in 'f' Options allows you to specify if you want the destination
            // path created if it doesn't exist.
            ////
            if (DestDirIsInvalid && !optCreateDestDir)
            {
                Console.WriteLine("-d " + strDestDir + " invalid. \n\n");
                ShowCommandHelp();
                return;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(strDestDir);                    
                }
                catch (System.Exception)
                {
                    Console.WriteLine("-d " + strDestDir + " invalid (-f option failed to create directory). \n\n");
                    ShowCommandHelp();
                    return;
                }                
            }

            if (argsFound < (MIN_ARG_LENGTH / 2))
            {
                ShowCommandHelp();
                return;
            }

            Sync sync = new Sync(strSourceDir, strDestDir, strOptions, true);
            
            // Now do the grunt of the work            
            sync.StartSync();
        }
    }
    //////////////////////////////////////////////////////////////////////
    // Clss: Sync
    // Desc: Clas Responsible for all the Syncronization work        
    //////////////////////////////////////////////////////////////////////
    class Sync
    {
        private string m_SourceDir = "";
        private string m_DestDir = "";
        private string m_Options = "";
        
        // shows file/directory copying progress to the command-line (for now)
        private bool m_bshowProgress = true; 

        private ArrayList m_FileList = new ArrayList();
        private bool m_bStop = false;
        
        // Copy files from source to dest and from dest to source *see Sync_Readme.txt*
        // Bidirectionally syncing the files (*determination is done whether 'm' is on or off)
        private bool m_isBiDirecSync = false;
        // Force all files from source that are different to copy to dest (file backup behavior)
        // This setting is exclusive and will force other settings to be disabled
        private bool m_forceCopyofSource = false;
        //Don't copy if the file sizes are identical (saves redundant copying, the reason this program exists)
        //turning it off means that this program will behave like most other sync programs out there
        private bool m_SkipCopyIfSizeMatches = true;
        //This option allows you to specify to only sync files that exist in the source & dest Folder.
        //Usefull if you only want to update files but not copy over everything
        private bool m_CopyOnlyFilesThatExist = false;

        //////////////////////////////////////////////////////////////////////
        // Constructor        
        //////////////////////////////////////////////////////////////////////
        public Sync(string SourceDir, string DestDir, string Options, bool bShowProgress)
        {
            m_SourceDir = SourceDir;
            m_DestDir = DestDir;
            m_Options = Options.ToLower();
            m_bshowProgress = bShowProgress;

            parseOptions();
        }
        //////////////////////////////////////////////////////////////////////
        // Func: parseOptions()
        // Desc: parses the passed in options setting and sets flags accordingly       
        //////////////////////////////////////////////////////////////////////
        private void parseOptions()
        {
            if (m_Options.Contains("b"))
            {
                m_isBiDirecSync = true;
            }            
            if(m_Options.Contains("m"))
            {
                m_SkipCopyIfSizeMatches = false;
            }
            if (m_Options.Contains("e"))
            {
                m_CopyOnlyFilesThatExist = true;
            }
            if (m_Options.Contains("s"))
            {
                m_forceCopyofSource = true;
                m_isBiDirecSync = false;
                m_SkipCopyIfSizeMatches = false;
                m_CopyOnlyFilesThatExist = false;
            }
        }
        //////////////////////////////////////////////////////////////////////
        // Func: StartSync()
        // Desc: Call this method when you are ready to start syncing files
        //////////////////////////////////////////////////////////////////////
        public void StartSync()
        {
            // First Sync Source to Dest
            Syncer(m_SourceDir,m_DestDir);

            // Also, if set, sync Dest to Source
            if (m_isBiDirecSync)
            {
                Syncer(m_DestDir, m_SourceDir);
            }
        }
        //////////////////////////////////////////////////////////////////////
        // Func: StopSync()
        // Desc: Use this method to stop syncing Abruptly
        //////////////////////////////////////////////////////////////////////
        public void StopSync()
        {
            m_bStop = true;
        }
        //////////////////////////////////////////////////////////////////////
        // Func: Syncer()
        // Desc: Main Body - Driver funct - responsible for getting all the files
        //       from srcDir and iterating thru each one (calling ForEachFile())
        //////////////////////////////////////////////////////////////////////
        private void Syncer(string srcDir, string destDir)
        {

            LoadFileList(srcDir);
            string lastDirectory = "";
            string currDirectory = "";
            bool bDirectoryHasChanged = false;

            foreach (FileInfo file in m_FileList)
            {
                if (m_bStop)
                {
                    break;
                }

                currDirectory = GetRelativeDir(file.Directory, srcDir);

                if ((currDirectory != "") && (currDirectory != lastDirectory))
                {
                    lastDirectory = currDirectory;
                    bDirectoryHasChanged = true;
                }
                else
                {
                    bDirectoryHasChanged = false;
                }

                ForEachFile(file, bDirectoryHasChanged, currDirectory, srcDir, destDir);    
            }
        }
        //////////////////////////////////////////////////////////////////////
        // Func: ForEachFile()
        // Desc: Responsible for calline CreateDirectory..() and CopyFile...()
        //       as needed. Also Writes to the Command-line the status of a 
        //       file/directory if m_bshowProgress is true;
        //////////////////////////////////////////////////////////////////////
        private void ForEachFile(FileInfo file, bool bDirectoryHasChanged, string relSrcDir, string srcDir, string destDir)
        {
            
            bool bCopiedFile = false; 
            string strCreatedDir = "";            

            if (bDirectoryHasChanged)
            {
                strCreatedDir = CreateDirectoryAccordingToOptions(relSrcDir, destDir);

                if ((strCreatedDir == "EXISTS") || (strCreatedDir == "CREATED"))
                {
                    bCopiedFile = CopyFileAccordingToOptions(file, relSrcDir, srcDir, destDir);
                }
            }
            else
            {
                bCopiedFile = CopyFileAccordingToOptions(file, relSrcDir, srcDir, destDir);
            }

            if (m_bshowProgress)
            {
                if (strCreatedDir == "CREATED")
                {
                    Console.Write("Directory Created: " + relSrcDir + " in " + destDir + "\n");
                }

                if (bCopiedFile)
                {
                    Console.Write("File Copied: " + file.Name + " to " + destDir + "\n");
                }
                else
                {
                    Console.Write("File Skipped: " + file.Name + "\n");
                }
            }
        }
        //////////////////////////////////////////////////////////////////////
        // Func: CopyFileAccordingToOptions()
        // Desc: Responsible for making copying file decisions. The decision is
        //       based on what options were passed into the class        
        //////////////////////////////////////////////////////////////////////
        private bool CopyFileAccordingToOptions(FileInfo file, string relDir, string srcDir, string destDir)
        {
            
            string TargetDir = "";
            string SourceDir = "";

            if (relDir == "")
            {
                SourceDir = srcDir;
                TargetDir = destDir;
            }
            else
            {
                SourceDir = srcDir + "\\" + relDir;
                TargetDir = destDir + "\\" + relDir;
            }

            string SourceFile = SourceDir + "\\" + file.Name;
            string TargetFile = TargetDir + "\\" + file.Name;

            // If file exists there are multiple strategies
            if (File.Exists(TargetFile))
            {
                FileInfo srcInfo = new FileInfo(SourceFile);
                FileInfo dstInfo = new FileInfo(TargetFile);

                if (m_SkipCopyIfSizeMatches && (srcInfo.Length == dstInfo.Length))
                {
                    //skip this file (length matches, files are most likely the same)
                    return false;
                }
                // Source is newer
                else if(srcInfo.LastWriteTime > dstInfo.LastWriteTime)
                {
                    // Copy Left to right (always) (fall-through for below)
                }
                // Source is older - we don't want to copy it unless specifically specified.
                // (this will also allow us to use bidirectional sync)
                else if (srcInfo.LastWriteTime < dstInfo.LastWriteTime)
                {
                    // If forceCopy is enabled we'll force the 
                    // copying of the file below
                    if (!m_forceCopyofSource)
                    {
                        return false;
                    }
                }
                // Source and Target are of same date (highly unlikely)
                else if (srcInfo.LastWriteTime == dstInfo.LastWriteTime)
                {
                    // If forceCopy is enabled we'll force the 
                    // copying of the file below
                    if (!m_forceCopyofSource)
                    {
                        return false;
                    }
                }

                // Copying File
                try
                {
                    File.Copy(SourceFile, TargetFile, true);
                }
                catch
                {
                    return false;
                }                
                return true;
            }
            // if it doesn't, only a few,... 
            else
            {
                if (m_CopyOnlyFilesThatExist)
                {
                    return false;
                }

                // Copying File
                try
                {
                    File.Copy(SourceFile, TargetFile);
                }
                catch
                {
                    return false;
                }
                return true;
            }   
        }
        //////////////////////////////////////////////////////////////////////
        // Func: CreateDirectoryAccordingToOptions()
        // Desc: Responsible for making Directory creation decisions. The decision is
        //       based on what options were passed into the class        
        //////////////////////////////////////////////////////////////////////
        private string CreateDirectoryAccordingToOptions(string relDir, string destdir)
        {
            string dir = (destdir + "\\" + relDir);

            if (Directory.Exists(dir))
            {
                return "EXISTS";
            }

            if (!Directory.Exists(dir) && !m_CopyOnlyFilesThatExist)
            {
                Directory.CreateDirectory(dir);
                return "CREATED";
            }

            return "NOTEXISTS";
        }
        //////////////////////////////////////////////////////////////////////
        // Func: GetRelativeDir()
        // Desc: Gets the directory location relative to the passed in directory        
        //////////////////////////////////////////////////////////////////////
        private String GetRelativeDir(DirectoryInfo dir, string srcDir)
        {
            string relDir = dir.FullName.Replace(srcDir,"");

            if (relDir.StartsWith("\\"))
            {
                relDir = relDir.Remove(0, 1);
            }
            return (relDir);
        }
        //////////////////////////////////////////////////////////////////////
        // Func: LoadFileList()
        // Desc: Loads all the files (file paths) passed into by the path string into memory        
        //////////////////////////////////////////////////////////////////////
        private void LoadFileList(string strPath)
        {
            m_FileList.Clear();
            DirectoryInfo dir = new DirectoryInfo(strPath);

            foreach (FileInfo file in dir.GetFiles("*.*", SearchOption.AllDirectories))
            {
                m_FileList.Add(file);                
            }
        }        
    }
}
