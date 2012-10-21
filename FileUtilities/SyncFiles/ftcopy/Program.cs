using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ftcopy
{
    class Program
    {
        //////////////////////////////////////////////////////////////////////
        // Func: ShowCommandHelp
        // Desc: Writes the HowTo manual out to the command line
        //////////////////////////////////////////////////////////////////////
        static void ShowCommandHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nftcopy Helps you to quickly copy all files of a specified type \n\n");
            sb.Append("   Syntax: ftcopy -s \"[dir]\" -d \"[dir]\" -t \"[type(s)]\" -o \"[Options]\" \n\n");
            sb.Append("   Parameters:\n");
            sb.Append("   -s \"[dir]\"      - specify a source directory to ftcopy from \n");
            sb.Append("   -d \"[dir]\"      - specify a destination directory to ftcopy to \n");
            sb.Append("   -t \"[type(s)]\"  - specify a comma seperated list of type(s) \n\n");
            sb.Append("   Options:\n");
            sb.Append("   -r  - recursively copy all files from all subfolders from the source \n");
            sb.Append("   -e  - on conflict rename file to filename[xyz] to ensure all files get copied over \n\n");
            sb.Append("   Example:\n");
            sb.Append("   ftcopy -s \"c:\\SrcDir\" -d \"c:\\DstDir\" -t \"gif\" \n");
            sb.Append("   ftcopy -s \"c:\\SrcDir\" -d \"c:\\DstDir\" -t \"gif,jpg\" -o \"r\" \n");
            Console.WriteLine(sb.ToString());
        }

        static void Main(string[] args)
        {
            const int MIN_ARG_LENGTH = 6;

            if (args.Length < MIN_ARG_LENGTH)
            {
                ShowCommandHelp();
                return;
            }

            int argsFound = 0;
            string strSourceDir = "";
            string strDestDir = "";
            string strFileTypes = "";
            string strOptions = "";
            bool bRecursive = false;
            bool bEnsureIntegrity = false;

            for (int i = 0; i < args.Length; ++i)
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
                        Console.WriteLine("-s " + strDestDir + " invalid. \n\n");
                        ShowCommandHelp();
                        return;
                    }
                }
                else if (args[i].ToLower() == "-t" && (args.Length >= MIN_ARG_LENGTH))
                {
                    ++argsFound;
                    strFileTypes = args[i + 1].ToLower();
                    ++i;
                }
                else if (args[i].ToLower() == "-o" && (args.Length >= MIN_ARG_LENGTH))
                {
                    ++argsFound;
                    strOptions = args[i + 1].ToLower();
                    ++i;

                    // recursive enabled
                    bRecursive = strOptions.ToLower().Contains('r');

                    // integrity enabled
                    bEnsureIntegrity = strOptions.ToLower().Contains('e');
                }
            }

            if (argsFound < (MIN_ARG_LENGTH / 2))
            {
                ShowCommandHelp();
                return;
            }

            PerformFileTypeCopy(strSourceDir, strDestDir, strFileTypes, bRecursive, bEnsureIntegrity);
        }

        /// <summary>
        /// Use this to do the specific file type directory copy operation
        /// </summary>
        /// <param name="srcDir">valid path to a source dir</param>
        /// <param name="desDir">valid path to a dest dir</param>
        /// <param name="fileTypes">coma seperated list of file types</param>
        /// <param name="bRecursive">scan all directories or only top level</param>
        static void PerformFileTypeCopy(string srcDir, string desDir, string fileTypes, bool bRecursive, bool bIntegrity)
        {
            // Iterate thru all file types
            string[] strFileTypes = fileTypes.Split(',');
            foreach (string strFileType in strFileTypes)
            {
                string[] strGetFiles;
                string searchParam = "*." + strFileType.ToLower();

                if (bRecursive)
                    strGetFiles = Directory.GetFiles(srcDir, searchParam, SearchOption.AllDirectories);
                else
                    strGetFiles = Directory.GetFiles(srcDir, searchParam, SearchOption.TopDirectoryOnly);

                if (strGetFiles.Length == 0)
                {
                    Console.WriteLine(String.Format("No Files of FileType {0} Found", strFileType));
                }

                try
                {
                    // Now Iterate thru each file, check the extension to make
                    // sure and copy each file over to destination
                    foreach (string strFileName in strGetFiles)
                    {
                        string ext = Path.GetExtension(strFileName).ToLower();
                        if (ext == ("." + strFileType))
                        {
                            string destFileNPath = desDir + '\\' + Path.GetFileName(strFileName);
                            Console.WriteLine(String.Format("Copying {0} to {1}", strFileName, destFileNPath));

                            if (bIntegrity)
                            {
                                while (File.Exists(destFileNPath))
                                    destFileNPath = desDir + '\\' + Path.GetFileNameWithoutExtension(strFileName) + "[" + Path.GetRandomFileName() + "]" + Path.GetExtension(strFileName);
                            }

                            File.Copy(strFileName, destFileNPath);
                        }
                    }
                }
                catch (Exception) { /* ignore */ }
            }
        }
      
    }
}
