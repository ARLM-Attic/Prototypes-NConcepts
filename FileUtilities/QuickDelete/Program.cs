using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace QuickDelete
{
    class Program
    {
        static void ShowUsuage()
        {
            Console.WriteLine("QuickDelete - Small, Fast, and useful Delet Program \n");
            Console.WriteLine("options: \n");
            Console.WriteLine("   -delete     (DirectoryName)     deletes that directory incl.");
            Console.WriteLine("   -delete     (DirectoryName)     (SubDirectory)");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsuage();
            }
            else
            {
                if (args[0] == "-delete")
                {
                    
                    if ((args.Length == 3) && (args[1] != null) && (args[2] != null))
                    {                        

                        if (isValidStr(args[1]) && isValidStr(args[2]))
                        {                         
                            DeleteDirectory(args[1], args[2]);
                        }
                        else
                        {
                            Alert("Invalid Arguments");
                        }

                    }
                    else
                    {
                        Alert("No Paremeters were passed in");
                    }
                }
                else
                {
                    Alert("argument 1 not eq delete"); 
                }

            }
        }

        static bool isValidStr(object oToValidate)
        {
            if ((oToValidate != null) && (oToValidate.ToString() != ""))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
        
        static void Alert(string message)
        {
            Console.WriteLine(message);
        }

        static void DeleteDirectory(string directory, string specSubDirectory)
        {
            

            // Is the specific Sub Directory Feature On?
            if (isValidStr(directory) && isValidStr(specSubDirectory))
            {
                int lastindex = directory.LastIndexOf('\\');
                string strippeddir = directory.Substring((lastindex + 1), (directory.Length - (lastindex + 1)));

                if (strippeddir.ToUpper() == specSubDirectory.ToUpper())
                {
                    Alert("Deleting directory " + directory);
                    
                    // Get rid of read-files just in case
                    setAllFilesInDirToNormal(directory);

                    // Now i can delete this directory
                    Directory.Delete((directory), true);
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);

                    if (dir.Exists)
                    {

                        foreach (DirectoryInfo subdir in dir.GetDirectories())
                        {
                            DeleteDirectory((directory + "\\" + subdir.Name), specSubDirectory);                            
                        }

                    }
                    else
                    {
                        Alert("Directory does not exist"); 
                    }

                }

            }
            else
            {
                Alert("not a valid directory or subdirectory"); 
            }

        }

        static void setAllFilesInDirToNormal(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                setAllFilesInDirToNormal((dir + "\\" + subdir.ToString()));
            }
        }
    }
}
