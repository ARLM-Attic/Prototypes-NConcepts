using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace ReadWebFolderContent
{
    public class Program
    {
        static Regex WORD_FILES_REGEX_PATTERN = new Regex(@".*\.doc", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex ANY_FILE_REGEX_PATTERN = new Regex(@".*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        static void Main(string[] args)
        {
            Uri webFolder = new Uri("http://localhost");
            StringCollection paths = Utils.PathRoutines.SearchFilesInWebFolder(webFolder, ANY_FILE_REGEX_PATTERN, true);
            foreach (string path in paths)
            {
                Console.WriteLine(path);
            }
            Console.WriteLine("Done. Press Enter");
            Console.ReadKey(true);
        }
    }
}
