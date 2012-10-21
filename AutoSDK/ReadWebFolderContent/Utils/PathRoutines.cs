using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Utils
{
    // This class is responsible for creating a web folder shortcut which consists of a binary
    // target.lnk file and a desktop.ini file.
    // the target.lnk file has a binary format of the http shortcut link
    // the desktop.ini tells the shell what Object should handle the folder

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

        public static StringCollection CopyFilesFromWebFolderToLocal(Uri siteUrl,  string localFolder, bool copySubFolders)
        {
            
            StringCollection ret = new StringCollection();
            string mapFolderPath;

            try
            {
                if (!Directory.Exists(localFolder))
                {
                    Directory.CreateDirectory(localFolder);
                }
                
                mapFolderPath = CreateWebFolderLink(siteUrl);

            }
            catch (Exception e)
            {
                return ret;
            }
            
            Shell32.ShellClass shell = new Shell32.ShellClass();

            Shell32.Folder mapFolder = shell.NameSpace(mapFolderPath);
            Shell32.Folder dsFolder = shell.NameSpace(localFolder);

            // shell.Explore(mapFolder); // For Debugging (opens up folder in explorer)
            CopyToFolder(mapFolder, dsFolder, copySubFolders, ref ret);

            File.Delete(mapFolderPath);
            return ret;
        }                

        private static void CopyToFolder(Shell32.Folder srcFolder, Shell32.Folder dstFolder, bool copySubFolders, ref StringCollection resultList)
        {
            const string FileTypesNotToCopy = "aspx";
            const string FoldersNotToCopy = "forms";

            foreach (Shell32.FolderItem item in srcFolder.Items())
            {                

                if (item.IsLink)
                    continue;

                if (item.IsFolder && copySubFolders)
                {                    

                   Shell32.Folder subFolder = (Shell32.Folder) item.GetFolder;                                      
                   Shell32.FolderItem it = dstFolder.ParseName(subFolder.Title);

                   // Only Copy the folders we want to copy
                   if (FoldersNotToCopy.Contains(subFolder.Title.ToLower()))
                   {
                        continue; //skip folder
                   }

                   // if destination folder doesn't exist create it
                   if (!(it != null && it.Type != null && it.Type != ""))
                   {
                       dstFolder.NewFolder(subFolder.Title, 0x414);  
                   }                   

                   // We can now safely assume we have the folder, so we can copy it
                   it = dstFolder.ParseName(subFolder.Title);
                   CopyToFolder((Shell32.Folder) item.GetFolder, (Shell32.Folder) it.GetFolder, copySubFolders, ref resultList);

                   continue;
                }


                string fType = Path.GetExtension(item.Path);
                fType = fType.TrimStart('.');

                // Only Copy the files we want to copy
                if (FileTypesNotToCopy.Contains(fType.ToLower()))
                {
                    continue; //skip file
                }

                // Add to result set
                resultList.Add(item.Path);

                //check if this file already exists locally
                Shell32.FolderItem si = srcFolder.ParseName(item.Name);
                Shell32.FolderItem di = dstFolder.ParseName(item.Name);

                string tmpFile = Path.GetTempPath() + Path.GetFileName(item.Name);

                // if file already exists - we only copy over when newer
                if (di != null && di.Type != null && di.Type != "")
                {
                    // It looks like we first must copy the file over locally into a temp
                    // directory JUST to check the timestamp (modified date) ~very inefficient, but works
                    Shell32.ShellClass shell = new Shell32.ShellClass();
                    Shell32.Folder tmpFolder = shell.NameSpace(Path.GetTempPath());

                    if (File.Exists(tmpFile))   // Delete first otherwise Explorer will prompt
                    {
                        File.Delete(tmpFile);
                    }

                    tmpFolder.CopyHere(item, 0x414);
                    Shell32.FolderItem ti = tmpFolder.ParseName(item.Name);

                    // check timestamps of the files
                    if (ti.ModifyDate > di.ModifyDate)
                    {
                        File.Delete(di.Path);   // we can delete the old file
                        dstFolder.CopyHere(ti, 0x414);   //it appears there is a microsoft bug 0x414 won't do anything                                                         
                    }

                    File.Delete(tmpFile);

                }
                else // just copy over
                {
                    dstFolder.CopyHere(item, 0x414);    //it appears there is a microsoft bug 0x414 won't do anything                    
                }

            }

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

        // by the way, you may use the CreateWebFolderLink method to create the necessary web folder shortcuts 
        // to be visible in the “My Network Places” folder; for that, just pass the “c:\Documents and Settings\{USER NAME}\NetHood” 
        // string as the shortcutContainerPath parameter.
        private static string CreateWebFolderLink(Uri siteUrl, string shortcutContainerPath, string shortcutName)
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
        private static string CreateWebFolderLink(string siteUrl, string shortcutContainerPath, string shortcutName)
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
