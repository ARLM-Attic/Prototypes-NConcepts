using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ReadWebFolderContent
{
    public class Program
    {
        // static Regex WORD_FILES_REGEX_PATTERN = new Regex(@".*\.doc", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // static Regex ANY_FILE_REGEX_PATTERN = new Regex(@".*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        static void Main(string[] args)
        {
                        
            Uri webFolder = new Uri("http://portal.radiant.com/sites/PCS Domestic/Forecourt/Shared Documents");            
            StringCollection paths = Utils.PathRoutines.CopyFilesFromWebFolderToLocal(webFolder, "C:\\temp_sp", true);
            
            foreach (string path in paths)
            {
                Console.WriteLine(path);
            }
            Console.WriteLine("Done. Press Enter");
            Console.ReadKey(true);
        }
    }
}
