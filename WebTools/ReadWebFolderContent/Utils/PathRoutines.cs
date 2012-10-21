using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Utils
{
    public static class PathRoutines
    {

        //'44 seems to be the length where we have to change a byte from 00 to a 01.
        private const int URL_CUTOFF = 44;

        //This is where we construct the target.lnk file byte by byte. Most of the lines are shown in 16 byte chunks,
        //mostly because that is the way I saw it in the Debug utility I was using to inspect shortcut files.
        private static readonly byte[] LINK_CONTENT_PREFIX = new byte[]
        {//Line 1, 16 bytes
          0x4C, 0x00, 0x00, 0x00, 0x01, 0x14, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x00, 0x00,
         //Line 2, 16 bytes
          0x00, 0x00, 0x00, 0x46, 0x81, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
        //Line 3, 16 bytes
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        //Line 4., 16 bytes. 13th byte is significant.
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
        //Line 5. 13th byte is significant.
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        //When I was analyzing the next byte of shortcuts I created, I found that it is set to various values,
        //and I have no idea what they are referring to. In desperation I tried substituting some values.
        //00 caused a crash of Explorer. FF seeems to work fine for all.
          0xFF};

        private static readonly byte[] LINK_CONTENT_MID = new byte[]
        {
            0x14, 0x00,
            //Line 6, 16 bytes
            0x1F, 0x50, 0xE0, 0x4F, 0xD0, 0x20, 0xEA, 0x3A, 0x69, 0x10, 0xA2, 0xD8, 0x08, 0x00, 0x2B, 0x30,
            //Line 7, 16 bytes
            0x30, 0x9D, 0x14, 0x00, 0x2E, 0x00, 0x00, 0xDF, 0xEA, 0xBD, 0x65, 0xC2, 0xD0, 0x11, 0xBC, 0xED,
            //Line 8, 16 bytes
            0x00, 0xA0, 0xC9, 0x0A, 0xB5, 0x0F, 0xA4
        };

        private static readonly byte[] LINK_CONTENT_MID2 = new byte[]
        {
            0x4C, 0x50, 0x00, 0x01, 0x42, 0x57, 0x00, 0x00,
            //Line 9, 16 bytes
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00,
            //Line 10, 2 bytes
            0x00, 0x00
        };

        private static readonly byte[] LINK_CONTENT_POSTFIX = new byte[]
        {
            //Last line, 13 bytes
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };        

        /// <summary>
        /// Uses Shell32
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="filePathPattern">For example, to make a wildcard search ".doc" use the following
        /// reg expression: Regex regex = new Regex(@".*\.doc", RegexOptions.Compiled | RegexOptions.IgnoreCase); 
        /// to exclude word template documents assigned to a SharePoint workspace, use the following reg 
        /// expression: new Regex(@"(?<!.*/Forms/[^/]+)\.doc$", RegexOptions.Compiled | RegexOptions.IgnoreCase)</param>
        /// <returns></returns>
        public static StringCollection SearchFilesInWebFolder(Uri siteUrl, Regex filePathPattern, bool searchSubFolders)
        {
            string mapFolderPath = CreateWebFolderLink(siteUrl);
            
            StringCollection ret = new StringCollection();

            Shell32.ShellClass shell = new Shell32.ShellClass();

            Shell32.Folder mapFolder = shell.NameSpace(mapFolderPath);
            SearchInFolder(mapFolder, filePathPattern, searchSubFolders, ret);

            return ret;
        }

        private static void SearchInFolder(Shell32.Folder folder, Regex filePathPattern, bool searchSubFolders, StringCollection resultList)
        {
            foreach (Shell32.FolderItem item in folder.Items())
            {
                if (item.IsLink)
                    continue;

                if (item.IsFolder && searchSubFolders)
                {
                    SearchInFolder((Shell32.Folder) item.GetFolder, filePathPattern, searchSubFolders, resultList);
                    continue;
                }

                if (filePathPattern.IsMatch(item.Path))
                {
                    resultList.Add(item.Path);
                }  
            }
        }


        public static string GetFileName(Uri uri)
        {
            if (uri == null)
                return null;

            return System.IO.Path.GetFileName(uri.ToString());
        }


        /// <summary>
        /// Returns path to a shortcut folder that is mapped to the specified web siteUrl. Name of the folder is derived
        /// automatically.
        /// If the shortcut folder already exists - does nothing
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="shortcutContainerPath"></param>
        /// <returns></returns>
        public static string CreateWebFolderLink(Uri siteUrl)
        {
            //derive shortcut name
            string sitePath = siteUrl.ToString();
            string shortcutName = sitePath.GetHashCode().ToString();

            return CreateWebFolderLink(sitePath, Path.GetTempPath(), shortcutName);
        }

        public static string CreateWebFolderLink(Uri siteUrl, string shortcutContainerPath, string shortcutName)
        {
            return CreateWebFolderLink(siteUrl.ToString(), shortcutContainerPath, shortcutName);
        }

        /// <summary>
        /// Returns path to a folder with name 'shortcutName' that is mapped to the specified web siteUrl.
        /// If the folder already exists - does nothing
        /// </summary>
        /// <param name="siteUrl">The target url</param>
        /// <param name="siteName">Name of the folder</param>
        /// <param name="siteContainerPath">Directory where the Web Folder mapped to the specified web siteUrl will be created</param>
        public static string CreateWebFolderLink(string siteUrl, string shortcutContainerPath, string shortcutName)
        {
            if (siteUrl.Length > 255)
                throw new ArgumentOutOfRangeException("Length of Url [" + siteUrl + "] is more than 255 symbols");

            string localFolderPath = Path.Combine(shortcutContainerPath, shortcutName);

            DirectoryInfo localDir = null;
            if (Directory.Exists(localFolderPath))
                return localFolderPath;

            localDir = Directory.CreateDirectory(localFolderPath);
            localDir.Attributes = FileAttributes.ReadOnly;

            string desktop_ini_filePath = Path.Combine(localFolderPath, "Desktop.ini");
            StreamWriter writer = new StreamWriter(desktop_ini_filePath, false);
            writer.WriteLine("[.ShellClassInfo]");
            writer.WriteLine("CLSID2={0AFACED1-E828-11D1-9187-B532F1E9575D}");
            writer.WriteLine("Flags=2");
            writer.WriteLine("ConfirmFileOp=0");
            writer.Close();

            //'We make Desktop.ini a system-hidden file by assigning it attribute of 6
            //Set fs = CreateObject("Scripting.FileSystemObject")
            //Set g = fs.GetFile(newPath & "\Desktop.ini")
            //g.Attributes = 6

            //'We make the folder read-only by assigning it 1.
            //Set fs = CreateObject("Scripting.FileSystemObject")
            //Set g = fs.GetFolder(newPath)
            //g.Attributes = 1

            string lnk_filePath = Path.Combine(localFolderPath, "target.lnk");
            FileStream fs = new FileStream(lnk_filePath, FileMode.Create);
            BinaryWriter stream = new BinaryWriter(fs, Encoding.BigEndianUnicode);
            stream.Write(LINK_CONTENT_PREFIX, 0, LINK_CONTENT_PREFIX.Length);


            //'This byte is 00 if the URL is 44 characters or less, 01 if greater.
            if (siteUrl.Length > URL_CUTOFF)
            {
                stream.Write((byte)0x01);
            }
            else
            {
                stream.Write((byte)0x00);
            }

            stream.Write(LINK_CONTENT_MID, 0, LINK_CONTENT_MID.Length);

            //'This byte is 00 if the URL is 44 characters or less, 01 if greater.
            if (siteUrl.Length > URL_CUTOFF)
            {
                stream.Write((byte)0x01);
            }
            else
            {
                stream.Write((byte)0x00);
            }

            stream.Write(LINK_CONTENT_MID2, 0, LINK_CONTENT_MID2.Length);

            //The next byte represents the length of the site name.
            stream.Write((byte)shortcutName.Length);

            //MemoryStream s = new MemoryStream();
            //BinaryWriter b = new BinaryWriter(s, Encoding.BigEndianUnicode);

            char[] webFolderNameChars = shortcutName.ToCharArray();
            stream.Write(webFolderNameChars, 0, webFolderNameChars.Length);


            //b.Write(webFolderNameChars, 0, webFolderNameChars.Length);

            //Middle line, separates the Folder Name from the URL. 3 bytes.
            stream.Write((byte)0x00);
            stream.Write((byte)0x00);
            stream.Write((byte)0x00);

            //The next byte represents the length of the site URL.
            stream.Write((byte)siteUrl.Length);

            char[] sitePathChars = siteUrl.ToCharArray();
            stream.Write(sitePathChars, 0, sitePathChars.Length);

            stream.Write(LINK_CONTENT_POSTFIX, 0, LINK_CONTENT_POSTFIX.Length);

            stream.Close();

            return localFolderPath;

        }

    }
}
