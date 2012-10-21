using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace SolutionLauncher
{
    /// <summary>
    /// Clss: Settings
    /// Desc: Keeps all the global congfiguration data (such as registry, temporary files, 
    ///       and variables) also has a few utility functions that are often used    
    /// </summary>
    public class Settings
    {
        // Main Program
        public string FileName;
        public string WorkingDirectory;

        // Temporary File
        public string TempBatFile;
        public string TempDirectory;

        // Temporary Stream for Temporary File
        private FileStream fsTempBatFile;
        private StreamWriter swTempBatFile;

        // Global Flags
        public bool bSDKPanelEnabled;
        public bool bSDKManagerNetworkedEnabled;
        public bool bSDKManagerNetworkPathIsValid;

        // Network Information
        public string NetworkPath;     // network location to map for SDKs
        public string DriveLetter;     // driveletter to map location to
        public DirectoryInfo SDKDirectory;

        // Registry Keys that i am accessing
        public RegistryKey root;
        public RegistryKey SLSettings;
        public RegistryKey SDKSettings;
        public RegistryKey ProjectTypes;
        public RegistryKey Launchers;
        public RegistryKey EnvSettings;

        // Launch Project Variables
        public string ProjectType;
        public string ProjectVersion;
        public string LaunchWith;
        public string currentProjectDir; //holds full path for current project

        // Full Project Spec
        public string Project
        {
            get
            {
                return (ProjectType + ProjectVersion);
            }
        }

        // ** APPEARANCE SETTINGS **
        public const int INIT_topPosition = 30; 
        public const int INIT_LEFT = 30;
        public const int WIDTH = 160;               // For Labels
        public const int COMBOBOX_WIDTH = 160;      // for comboboxes
        public const int COMBOBOX_MAX_DROP_DOWN_ITEMS = 12;
        public const int LABEL_SPACING = 24;    //24 - must be 24
        public const int CBOX_SPACING = 24;     //35
        public const int TEXTAREA_SPACING = 5; //18
        public const int TEXTAREA_HEIGHT = 200;     // default height, dynamically changes!
        public const int TEXTAREA_WIDTH = 160;
        public const int RBUTTON_SPACING = 22;  //25        radio buttons
        public const int AFTER_RBUTTONS_SPACING = 9;  //added for better apperance        
        public const int AFTER_LAUNCHANDSETBUTTONS = 30; //~required!!! 20 //added for better apperance
        public const int SET_SPACED_LEFT = 80;         // for set/save/exit button! ~appearance
        // public const int FORM_HEIGHT_SPACER = 50;   //not used   // Height spacer of the bottom of main form
        // public const int GROUP_BOX_SPACING = 25;    // not used


        // ** State Settings ~used to save the configuration State **
        public StringDictionary currentState;

        public void newTempFile()
        {
            closeTempFile();
            fsTempBatFile = new FileStream(TempDirectory + TempBatFile, FileMode.Create);
            swTempBatFile = new StreamWriter(fsTempBatFile, Encoding.ASCII);
        }
        
        public void writeLineToTempFileWithoutClosing(string line)
        {
            if ((fsTempBatFile != null) && (swTempBatFile != null))
            {
                swTempBatFile.WriteLine(line);
                swTempBatFile.Flush();
            }
        }

        public void writeLineToTempFileOpenAndClose(string fileToLaunch, string parameter)
        {
            FileStream fs = new FileStream(TempDirectory + TempBatFile, FileMode.Append);
            StreamWriter w = new StreamWriter(fs, Encoding.ASCII);

            w.WriteLine(fileToLaunch + parameter);

            w.Flush();
            w.Close();
            fs.Close();
        }

        public void closeTempFile()
        {
            if (swTempBatFile != null)
            {
                swTempBatFile.Close();
                swTempBatFile = null;
            }

            if (fsTempBatFile != null)
            {
                fsTempBatFile.Close();
                fsTempBatFile = null;
            }
        }

        public Settings(string filename, string workingdir)
        {
            FileName = filename;  
            WorkingDirectory = workingdir; 
                       
            // For State Logic
            currentState = new StringDictionary();

            // Temporary File 
            TempBatFile = Path.GetFileNameWithoutExtension(FileName) + ".bat";
            TempDirectory = Path.GetTempPath();
           
            // Globally Accessible Registry Keys
            ReadInRegistryKeys();

            // For SDK Logic
            bSDKPanelEnabled = true;
            bSDKManagerNetworkedEnabled = true;
            bSDKManagerNetworkPathIsValid = false;

            // Retrieve whether SDK is enabled or not
            // call accordingly
            MapNetworkPathwithDriveLetter();
        }

        public void clearState()
        {
            currentState.Clear();
        }

        public void addToState(string key, string value)
        {
            currentState.Add(key, value);
        }

        private void ReadInRegistryKeys()
        {
            root = Registry.CurrentUser;
            SLSettings = root.OpenSubKey("Software\\SolutionLauncher\\Settings");
            SDKSettings = root.OpenSubKey("Software\\SolutionLauncher\\SDKSettings");
            ProjectTypes = root.OpenSubKey("Software\\SolutionLauncher\\ProjectTypes");
            Launchers = root.OpenSubKey("Software\\SolutionLauncher\\Launchers");
            EnvSettings = root.OpenSubKey("Software\\SolutionLauncher\\EnvSettings", RegistryKeyPermissionCheck.ReadWriteSubTree);
        }

        private void MapNetworkPathwithDriveLetter()
        {
            if (SLSettings == null)
            {
                Alert("No Settings found in the Registry");
            }
            else
            {
                object NetPath = SLSettings.GetValue("NetworkPath");
                object Drive = SLSettings.GetValue("DriveLetter");

                if (isValidStr(NetPath))
                {
                    NetworkPath = NetPath.ToString();
                }

                if (isValidStr(Drive))
                {
                    DriveLetter = Drive.ToString();
                }
            }

            ////
            // All Network Mapping Knowledge needs to go here:
            //// 
            bSDKManagerNetworkPathIsValid = false;
            //SDKDirectory = new DirectoryInfo(DriveLetter + ":\\");
            SDKDirectory = new DirectoryInfo(NetworkPath + '\\');

            // The SetEnv Directory determines whenter we are connected or NOT
            if(Directory.Exists(SDKDirectory + "SetEnv"))
            {
                bSDKManagerNetworkPathIsValid = true;
            }            
            else
            {
                // we need to map the network drive
            }

        }

        public bool isValid(object oToValidate)
        {
            if (oToValidate != null)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        public bool isValidStr(object oToValidate)
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

        public void Alert(string message)
        {
            MessageBox.Show(message, "Alert");
        }
    }
}
