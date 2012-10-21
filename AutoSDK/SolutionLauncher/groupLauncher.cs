using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Collections.Specialized;

namespace SolutionLauncher
{
    //////////////////////////////////////////////////////////////////////
    // Clss: groupLauncher
    // Desc: 
    //       
    //       
    //
    // Note: 
    //       
    //
    //////////////////////////////////////////////////////////////////////
    class groupLauncher
    {
        
        // Pointer to global Settings
        private Settings MySet;

        // Pointer to SDKpanel
        private groupSDKs MySDKobject;

        // Pointer to MainForm so that i can close it
        private Form MyForm;

        // This variable holds the panel pointer to draw to (IMP)
        private GroupBox PanelToDrawTo;                

        // Local Control and Position Vars
        private int topPosition;
        private int LEFT;
        private int tabIndex;
        private int HEIGHT;                 // this variable holds the HEIGHT required to show all the components in the Panel
        private int TextAreaHEIGHT;         // for dynamically growing the TextArea
        private bool hOnly;                 // don't actually draw the components just calculate the height

        public groupLauncher(ref Settings settings, Form form, GroupBox panelToDrawTo, int initTopPosition, int initLeftPosition, int TextAreaGrowHeight, bool bHeightOnly)
        {
            MySet = settings;
            MyForm = form;

            if (MySet.isValid(panelToDrawTo))
            {
                PanelToDrawTo = panelToDrawTo;                
                tabIndex = 0;

                if (initTopPosition >= 0)
                {
                    topPosition = initTopPosition;
                }
                else
                {
                    topPosition = Settings.INIT_topPosition;
                }

                if (initLeftPosition >= 0)
                {
                    LEFT = initLeftPosition;
                }
                else
                {
                    LEFT = Settings.INIT_LEFT;
                }

                if (TextAreaGrowHeight > 0)
                {
                    TextAreaHEIGHT = Settings.TEXTAREA_HEIGHT + TextAreaGrowHeight;
                }
                else
                {
                    TextAreaHEIGHT = Settings.TEXTAREA_HEIGHT;
                }

                hOnly = bHeightOnly;

                HEIGHT = 0;              
                Initialize();
            }            
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Start()
        // Desc: Call this after completely drawing all compenents onto the form.
        //       this function initializes the first project        
        //////////////////////////////////////////////////////////////////////
        public void Start()
        {
            // Set the index after first drawing all the cb's
            //System.Threading.Thread.Sleep(100);
            if (!LoadLastProject())
            {
                // Default
                setIndexZero("ProjectType");
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Prop: Write-only SDKEnable Property
        //////////////////////////////////////////////////////////////////////
        public void SetSDKEnable(ref groupSDKs SDKobject)
        {
            MySDKobject = SDKobject;            
        }

        //////////////////////////////////////////////////////////////////////
        // Prop: Read-only Height Property
        //////////////////////////////////////////////////////////////////////
        public int Height
        {
            get
            {
                return HEIGHT;
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Initialize()
        // Desc: Draws all the components onto the Panel *Entry Function*                
        //////////////////////////////////////////////////////////////////////
        private void Initialize()
        {            

            // The order on how you want to draw things on the panel
            Draw_ProjectTypeCBandLabel();
            Draw_VersionCBandLabel();
            Draw_NotesTextArea();
            Draw_LaunchRadioPlusButtons();

            //After drawing all the components Assign the Height Property
            HEIGHT = topPosition;            
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Draw_ProjectTypeCBandLabel()
        // Desc: Draws the "Project Type" Label and ComboBox                
        //
        // Note: By default it the project types are loaded from the
        //       registry but no index is selected, this is because once we
        //       change the index the event handler gets triggered (and we 
        //       have to first draw all the other elements)
        //////////////////////////////////////////////////////////////////////
        private void Draw_ProjectTypeCBandLabel()
        {
                        
            string lblName = "lblProjectType";
            
            if (!hOnly)
            {

                // Draw Label
                Label label = new Label();
                label.Left = LEFT;
                label.Top = topPosition;
                label.Width = Settings.WIDTH;
                label.UseCompatibleTextRendering = true;
                label.Name = lblName;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Text = "Project Type";
                PanelToDrawTo.Controls.Add(label);
            }
            topPosition += Settings.LABEL_SPACING; // spacing for the label

            // Draw ComboBox
            ComboBox comboBox = new ComboBox();

            if (!hOnly)
            {
                comboBox.Left = LEFT;
                comboBox.Top = topPosition;
                comboBox.TabIndex = tabIndex++;
                comboBox.Width = Settings.COMBOBOX_WIDTH;
                comboBox.MaxDropDownItems = Settings.COMBOBOX_MAX_DROP_DOWN_ITEMS;
                comboBox.Name = "cBoxProjectType";
                comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                comboBox.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);

                PanelToDrawTo.Controls.Add(comboBox);
            }

            topPosition += Settings.CBOX_SPACING; //spacing for the combo box

            if (!hOnly)
            {
                // Now fill the ComboBox with the different Project Types:
                if (MySet.ProjectTypes.SubKeyCount == 0)
                {
                    MySet.Alert("No Project Types found in the Registry");
                }
                else
                {
                    foreach (string keyname in MySet.ProjectTypes.GetSubKeyNames())
                    {
                        comboBox.Items.Add(keyname);
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Draw_VersionCBandLabel()
        // Desc: Draws the "Version" Label and ComboBox                
        //
        // Note: By default nothing is loaded into the combobox
        //////////////////////////////////////////////////////////////////////
        private void Draw_VersionCBandLabel()
        {

            string lblName = "lblVersion";

            if (!hOnly)
            {
                // Draw SDK Label
                Label label = new Label();
                label.Left = LEFT;
                label.Top = topPosition;
                label.Width = Settings.WIDTH;
                label.UseCompatibleTextRendering = true;
                label.Name = lblName;

                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Text = "Version";
                PanelToDrawTo.Controls.Add(label);
            }
            topPosition += Settings.LABEL_SPACING; // spacing for the label


            if (!hOnly)
            {
                // Draw SDK ComboBox
                ComboBox comboBox = new ComboBox();
                comboBox.Left = LEFT;
                comboBox.Top = topPosition;
                comboBox.TabIndex = tabIndex++;
                comboBox.Width = Settings.COMBOBOX_WIDTH;
                comboBox.MaxDropDownItems = Settings.COMBOBOX_MAX_DROP_DOWN_ITEMS;
                comboBox.Name = "cBoxVersion";
                comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                comboBox.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);

                PanelToDrawTo.Controls.Add(comboBox);
            }
            topPosition += Settings.CBOX_SPACING; //spacing for the combo box            
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Draw_NotesTextArea()
        // Desc: Draws the "Notes" label and TextArea
        //
        // Note: By default nothing is loaded into the TextArea
        //////////////////////////////////////////////////////////////////////
        private void Draw_NotesTextArea()
        {            

            // Draw Text Area Label
            if (!hOnly)
            {
                Label label = new Label();
                label.Left = LEFT;
                label.Top = topPosition;
                label.Width = Settings.WIDTH;
                label.UseCompatibleTextRendering = true;
                label.Name = "lblNotes";
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Text = "Notes";
                PanelToDrawTo.Controls.Add(label);
            }
            topPosition += Settings.LABEL_SPACING; // spacing for the label                        

            if (!hOnly)
            {
                // Draw Notes Text Area
                RichTextBox richTextBox = new RichTextBox();
                richTextBox.Left = LEFT;
                richTextBox.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                richTextBox.TabIndex = tabIndex++;      //last
                richTextBox.Top = topPosition;
                richTextBox.TextChanged += new System.EventHandler(this.NotesChanged);
                richTextBox.Height = TextAreaHEIGHT;
                richTextBox.Width = Settings.TEXTAREA_WIDTH;
                richTextBox.Name = "txtNotes";

                PanelToDrawTo.Controls.Add(richTextBox);
            }
            topPosition += (TextAreaHEIGHT + Settings.TEXTAREA_SPACING); // spacing for the Textarea
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Draw_LaunchRadioPlusButtons()
        // Desc: Draws the "Launchers" label, Launch radio Buttons,  and 
        //       Save and Save/Launch/Exit buttons
        //
        // Note: The radio button defaults to whatever is specified in the registry
        //////////////////////////////////////////////////////////////////////
        private void Draw_LaunchRadioPlusButtons()
        {

            // Check if launchers exists
            if (MySet.Launchers.SubKeyCount >= 1)
            {
                string lblName = "lblLaunchers";

                if (!hOnly)
                {
                    // Draw Label
                    Label label = new Label();
                    label.Left = LEFT;
                    label.Top = topPosition;
                    label.Width = Settings.WIDTH;
                    label.UseCompatibleTextRendering = true;
                    label.Name = lblName;
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    label.Text = "Launchers";
                    PanelToDrawTo.Controls.Add(label);
                }
                topPosition += Settings.LABEL_SPACING; // spacing for the label

                Panel panel = new Panel();

                if (!hOnly)
                {
                    panel.Left = LEFT;
                    panel.Top = topPosition;
                    panel.Name = "pnlLaunchers";
                }

                int radioButPosition = 0;
                //tabIndex = 2;

                foreach (string LaunchOption in MySet.Launchers.GetSubKeyNames())
                {
                    if (!hOnly)
                    {
                        RadioButton radioButton = new RadioButton();
                        radioButton.AutoSize = true;
                        radioButton.Top = radioButPosition;
                        radioButton.Left = 0;
                        radioButton.Name = "rad" + LaunchOption;
                        radioButton.UseVisualStyleBackColor = true;
                        radioButton.TabIndex = tabIndex++;
                        radioButton.Text = LaunchOption;

                        RegistryKey RadOpSpec = MySet.Launchers.OpenSubKey(LaunchOption);
                        object isDefault = RadOpSpec.GetValue("default");

                        // Checking Radio Button if it is a default
                        if (MySet.isValidStr(isDefault))
                        {
                            if (isDefault.ToString() == "true")
                            {
                                radioButton.Checked = true;
                                radioButton.PerformClick(); // have default radio option selected 
                            }
                        }

                        panel.Controls.Add(radioButton);
                    }

                    radioButPosition += Settings.RBUTTON_SPACING; //spacing for radio button                 }
                }

                if (!hOnly)
                {
                    panel.Height = radioButPosition;
                    panel.Width = Settings.WIDTH;
                }
                topPosition += radioButPosition + Settings.AFTER_RBUTTONS_SPACING;

                if (!hOnly)
                {
                    PanelToDrawTo.Controls.Add(panel);
                }

                if (!hOnly)
                {
                    // Launch & Set Button
                    Button buttonLSet = new System.Windows.Forms.Button();
                    buttonLSet.Name = "btnSSE";

                    // FEATURE CURRENTLY DISABLED - Exit Feature               
                    // object Exit = MySet.SLSettings.GetValue("SSExit");
                    object Exit = "true";

                    if (MySet.isValidStr(Exit))
                    {
                        if ((Exit.ToString() == "true") && MySet.isValid(MyForm))
                        {
                            buttonLSet.Text = "Save/Start/Exit";
                            buttonLSet.Width = 100;
                        }
                        else
                        {
                            buttonLSet.Text = "Save/Start";
                            buttonLSet.Width = 80;
                        }
                    }
                    buttonLSet.Top = topPosition;
                    buttonLSet.Left = LEFT;
                    buttonLSet.TabIndex = tabIndex++;             // this weird tabindex calculation is done so that Notes area is skipped
                    buttonLSet.UseVisualStyleBackColor = true;                    
                    buttonLSet.Click += new System.EventHandler(this.ButtonClick);
                    PanelToDrawTo.Controls.Add(buttonLSet);

                    // Launch Button
                    Button buttonL = new System.Windows.Forms.Button();
                    buttonL.Name = "btnSave";
                    buttonL.Text = "Save";
                    buttonL.Width = 55;
                    buttonL.Top = topPosition;
                    buttonL.Left = LEFT + 105;
                    buttonL.TabIndex = tabIndex++;
                    buttonL.UseVisualStyleBackColor = true;
                    buttonL.Click += new System.EventHandler(this.ButtonClick);
                    PanelToDrawTo.Controls.Add(buttonL);
                }

                topPosition += Settings.AFTER_LAUNCHANDSETBUTTONS; //spacing for Set And Clear Buttons                
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: NotesChanged()
        // Desc: Event Handler that handles the case when Notes have changed                
        //
        // Note: 
        //////////////////////////////////////////////////////////////////////
        private void NotesChanged(object sender, EventArgs e)
        {
            RichTextBox rt = (RichTextBox)sender;
            RegistryKey aNotes = MySet.EnvSettings.CreateSubKey(MySet.Project);
            aNotes.SetValue("Notes", rt.Text);
        }

        //////////////////////////////////////////////////////////////////////
        // Func: setIndexZero()
        // Desc: Function was specifically created to set the index after
        //       all components have been loaded into the panel        
        //////////////////////////////////////////////////////////////////////
        private void setIndexZero(string CBoxname)
        {
            string comparer = "cBox" + CBoxname;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox)cr;
                    cb.SelectedIndex = 0;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: LoadNotes()
        // Desc: Function Loads notes that were saved in the registry          
        //////////////////////////////////////////////////////////////////////
        private void LoadNotes()
        {
            
            RegistryKey kNotes = MySet.EnvSettings.OpenSubKey(MySet.Project);
            bool bClearNotes = true;

            if (MySet.isValidStr(kNotes))
            {
                object pNotes = kNotes.GetValue("Notes");

                if (MySet.isValidStr(pNotes))
                {
                    foreach (Control cr in PanelToDrawTo.Controls)
                    {
                        if (cr.Name == "txtNotes")
                        {
                            RichTextBox rt = (RichTextBox)cr;
                            rt.Text = pNotes.ToString();
                            bClearNotes = false;                            
                        }
                    }
                }
            }
            
            if(bClearNotes)
            {
                foreach (Control cr in PanelToDrawTo.Controls)
                {
                    if (cr.Name == "txtNotes")
                    {
                        RichTextBox rt = (RichTextBox)cr;
                        rt.Text = "";                        
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: SelectedIndexChanged()
        // Desc: EventHandler for both ProjectType and Version ComboBox. When 
        //       Changes occur it must update it's variables and reLoad Notes.        
        //////////////////////////////////////////////////////////////////////
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Sender = (ComboBox)sender;            
            
            if (Sender.Name == "cBoxProjectType")
            {
                
                string ProjectType = Sender.SelectedItem.ToString();                                
                RegistryKey ProjectKey = MySet.ProjectTypes.OpenSubKey(ProjectType); 

                object BasePath = ProjectKey.GetValue("BasePath");
                object ExcludeDirs = ProjectKey.GetValue("ExcludeDirs");
                object IncludeDirs = ProjectKey.GetValue("IncludeDirs");

                ClearCBox("Version");
                int index = 0;
                
                if (DoesLocalStateInformationExist(ProjectType))
                {
                    index = -1; //Don't Load Index!                                
                }

                if (MySet.isValidStr(BasePath))
                {
                    DirectoryInfo directory = new DirectoryInfo(BasePath.ToString());

                    if (directory.Exists)
                    {
                        if (MySet.isValidStr(IncludeDirs))
                        {
                            FillCBoxWithFolder("Version", BasePath.ToString(), IncludeDirs.ToString(), index, true);
                        }
                        else if (MySet.isValidStr(ExcludeDirs))
                        {
                            FillCBoxWithFolder("Version", BasePath.ToString(), ExcludeDirs.ToString(), index, false);
                        }
                        else
                        {
                            FillCBoxWithFolder("Version", BasePath.ToString(), "", index, false);
                        }

                        // Now I can Load the Local State info
                        LoadLocalStateInformation(false, ProjectType, "");

                        // Get the changed options and initialize the Notes
                        GetAllSelections();
                        LoadNotes();

                    }
                    else 
                    {
                        MySet.Alert(BasePath.ToString() + " does not exist - check project settings");
                    }                    
                }                
               
            }           
            else if (Sender.Name == "cBoxVersion")
            {
                int selected = Sender.SelectedIndex;
                                     
                // Load Only Local Launch Information
                LoadLocalStateInformation(true, MySet.ProjectType, (Sender.Items[selected].ToString()));
                
                // Get the changed options and initialize the Notes
                GetAllSelections();
                LoadNotes();                                
            }           

            // Always load State Information if Available
            if (MySet.isValid(MySDKobject))
            {
                LoadStateInformation();
            }            
        }

        //////////////////////////////////////////////////////////////////////
        // Func: ButtonClick()
        // Desc: Event Handler for the save and save/launch/exit buttons.
        //       On save will only save current selections in the registry.
        //       SSE wil save and launch and [optionally] exit the application        
        //////////////////////////////////////////////////////////////////////
        private void ButtonClick(object sender, EventArgs e)
        {
            Button Sender = (Button)sender;

            if (Sender.Name == "btnSave")    // Launch Only Button
            {
                GetAllSelections();
                SaveLocalStateInformation();
                SaveStateInformation();                                
            }
            else if (Sender.Name == "btnSSE")    // Launch & Set Button
            {
                ClickLaunchButton();
                //GetAllSelections();
                //SaveLocalStateInformation();
                //SaveStateInformation();                
                //Launch();                

                // Exit Feature is Currently Disabled
                // Check if we can close the form also if we want to
                //object Exit = MySet.SLSettings.GetValue("SSExit");

                //if (MySet.isValidStr(Exit))
                //{
                //    if ((Exit.ToString() == "true") && MySet.isValid(MyForm))
                //    {
                      //MyForm.Close();
                //    }
                //}                
            }
        }

        // This method does the same
        public void ClickLaunchButton()
        {
            GetAllSelections();
            SaveLocalStateInformation();
            SaveStateInformation();
            Launch();
            MyForm.Close();
        }

        public string CalculateProjectPath(bool bUseProjectDir, bool bUseRelativePath)
        {
                
            RegistryKey ProjectTypeSPEC = MySet.ProjectTypes.OpenSubKey(MySet.ProjectType);
            object Path = ProjectTypeSPEC.GetValue("BasePath");
            object PrjDir = ProjectTypeSPEC.GetValue("ProjectDir");
            object RelDir = ProjectTypeSPEC.GetValue("RelativePath");

            if (MySet.isValidStr(Path))
            {                                                    

                string directoryPath;

                if (bUseRelativePath && (MySet.isValidStr(RelDir)))
                {
                    directoryPath = Path.ToString() + '\\' + MySet.ProjectVersion + '\\' + RelDir.ToString();
                }
                else
                {
                    directoryPath = Path.ToString() + '\\' + MySet.ProjectVersion;
                }

                if (bUseProjectDir && (MySet.isValidStr(PrjDir)))
                {                                        
                    directoryPath += ('\\' + PrjDir.ToString());                    
                }

                return (directoryPath);
            }
            else
            {
                return ("");
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Launch()
        // Desc: This function is responsible for writing out the launch project
        //       to the temporary bat file. It first tries to load the default
        //       file for the project, then the alt. default file, if neither exist
        //       it will copy them over from an existing file with matching filetype.
        //
        // Note: This function will use Relative Path if exists
        //////////////////////////////////////////////////////////////////////
        private void Launch()
        {
            bool bOkToLaunch = false;

            RegistryKey LaunchersSPEC = MySet.Launchers.OpenSubKey(MySet.LaunchWith);
            object Launch = LaunchersSPEC.GetValue("Launch");
            object FileType = LaunchersSPEC.GetValue("FileType");
            object FileTypeAlt = LaunchersSPEC.GetValue("FileTypeAlt");

            if (MySet.isValidStr(Launch) && MySet.isValidStr(FileType))
            {

                string directoryPath = CalculateProjectPath(true, true);
             
                if(MySet.isValidStr(directoryPath))
                {
                    string parameterString;
                                      
                    parameterString = " " + '\"' + directoryPath;                    

                    string LaunchFile = MySet.ProjectType + "-" + MySet.ProjectVersion + '.' + FileType.ToString();
                    string LaunchFileAlt = "";
                    string strFileTypeAlt = "";

                    if (MySet.isValidStr(FileTypeAlt))
                    {
                        strFileTypeAlt = FileTypeAlt.ToString(); // I have to assign it here in case FileTypeAlt is null
                        LaunchFileAlt = MySet.ProjectType + "-" + MySet.ProjectVersion + '.' + FileTypeAlt.ToString();
                    }
                    
                    DirectoryInfo directory = new DirectoryInfo(directoryPath);
                    
                    if (directory.Exists)
                    {
                        if (LaunchFileOrAltFile(directory, LaunchFile, LaunchFileAlt, parameterString, Launch.ToString()))
                        {
                            bOkToLaunch = true;
                        }
                        else
                        {
                            //File or Alt file didn't exist so we must try to create it
                            if (LaunchAfterCopyingFirst(directory, LaunchFile, LaunchFileAlt, parameterString, Launch.ToString(), FileType.ToString(), strFileTypeAlt))
                            {
                                bOkToLaunch = true;
                            }
                        }
                    }
                    else
                    {
                        MySet.Alert("Directory " + parameterString + " Does not exist");
                    }
                                        
                }
                
            }

            if (!bOkToLaunch)
            {
                MySet.Alert("Was NOT able to Launch Project - Check Project Settings");
            }
            else
            {
                // *** Finally, Actually Start The Process Now THat Launches the Project ! ***

                //The "/C" Tells Windows to Run The Command then Terminate 
                string strCmdLine = "/C " + '\"' + MySet.TempDirectory + MySet.TempBatFile + '\"';

                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("C:\\Windows\\System32\\CMD.exe", strCmdLine);
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                System.Diagnostics.Process.Start(startInfo);
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: LaunchFileOrAltFile()
        // Desc: This function is responsible for either launching the File
        //       or alternative project file
        //////////////////////////////////////////////////////////////////////
        private bool LaunchFileOrAltFile(DirectoryInfo directory, string LaunchFile, string LaunchFileAlt, string parameterString, string Launch)
        {

            bool bDoesLaunchFileExist = false;
            bool bDoesLaunchFileAltExist = false;          

            // First figure out which one to use
            foreach (FileInfo file in directory.GetFiles())
            {
                if (file.Name.ToUpper() == LaunchFile.ToUpper())
                {
                    bDoesLaunchFileExist = true;
                }
                else if (LaunchFileAlt != "")
                {
                    if (file.Name.ToUpper() == LaunchFileAlt.ToUpper())
                    {
                        bDoesLaunchFileAltExist = true;
                    }
                }
            }

            if (bDoesLaunchFileExist)
            {
                parameterString = parameterString + '\\' + LaunchFile + '\"';                
                MySet.writeLineToTempFileOpenAndClose(Launch.ToString(), parameterString);
                return (true);
            }
            else if (bDoesLaunchFileAltExist)
            {
                parameterString = parameterString + '\\' + LaunchFileAlt + '\"';
                MySet.writeLineToTempFileOpenAndClose(Launch.ToString(), parameterString);
                return (true);
            }
            else
            {
                return (false);
            }                        
        }

        //////////////////////////////////////////////////////////////////////
        // Func: LaunchAfterCopyingFirst()
        // Desc: If the original file doesn't exist. This function will try to copy one.
        //       Copying is done by the same filetype in the project directory.
        //       ~the bigger file wins!
        //////////////////////////////////////////////////////////////////////
        private bool LaunchAfterCopyingFirst(DirectoryInfo directory, string LaunchFile, string LaunchFileAlt, string parameterString, string Launch, string FileType, string FileTypeAlt)
        {

            string FileToLaunch = "";

            //copy the biggest file ONLY - this will make sure the 
            //files with the most amount of projects wins :)
            long LaunchFileSize = 0;
            long LaunchFileAltSize = 0;

            foreach (FileInfo file in directory.GetFiles())
            {

                if (file.Name.Contains(FileType))
                {
                    if (file.Length > LaunchFileSize)
                    {
                        LaunchFileSize = file.Length;
                        file.CopyTo(file.DirectoryName + '\\' + LaunchFile, true);
                        FileToLaunch = LaunchFile;
                    }
                }
                else if (MySet.isValidStr(FileTypeAlt) && (file.Name.Contains(FileTypeAlt)))
                {
                    if (file.Length > LaunchFileAltSize)
                    {
                        LaunchFileAltSize = file.Length;
                        file.CopyTo(file.DirectoryName + '\\' + LaunchFileAlt, true);
                        FileToLaunch = LaunchFileAlt;
                    }
                }
            }

            if (FileToLaunch != "")
            {
                parameterString = parameterString + '\\' + FileToLaunch + '\"';
                MySet.writeLineToTempFileOpenAndClose(Launch.ToString(), parameterString);
                return (true);
            }
            else
            {
                return (false);
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: SaveStateInformation()
        // Desc: This function calls the SDKpanel's Set function if exist,
        //       then it goes thru all the currentstate dictionary and saves 
        //       it in the registry.                        
        //////////////////////////////////////////////////////////////////////
        private void SaveStateInformation()
        {
            
            //Always call Set() first
            if (MySet.isValid(MySDKobject))
            {                
                MySDKobject.SetSDKs();
            }

            RegistryKey kState = MySet.EnvSettings.CreateSubKey(MySet.Project);

            if (MySet.isValidStr(kState))
            {
                foreach (string key in MySet.currentState.Keys)
                {
                    kState.SetValue(key, MySet.currentState[key]);
                }
            }            
        }

        //////////////////////////////////////////////////////////////////////
        // Func: SaveLocalStateInformation()
        // Desc: This function is specifically designed to save Project Settings
        //       On the LaunchPanel (Locally selected)       
        //////////////////////////////////////////////////////////////////////
        private void SaveLocalStateInformation()
        {

            // assign version to project
            MySet.EnvSettings.SetValue(MySet.ProjectType, MySet.ProjectVersion);

            // assign Launcher to Version
            MySet.EnvSettings.SetValue(MySet.ProjectType+MySet.ProjectVersion, MySet.LaunchWith);

            // SAVE last Project Feature:
            MySet.EnvSettings.SetValue("LAST", MySet.ProjectType);
        }

        //////////////////////////////////////////////////////////////////////
        // Func: LoadStateInformation()
        // Desc: This function iterate thru the registry for the current selected
        //       project and creates a state object out of it. This will optionally
        //       be passed to the SDK panel
        //////////////////////////////////////////////////////////////////////
        public void LoadStateInformation()
        {
            RegistryKey kState = MySet.EnvSettings.OpenSubKey(MySet.Project);

            if (MySet.isValidStr(kState))
            {
                StringDictionary aState = new StringDictionary();

                foreach (string key in kState.GetValueNames())
                {
                    // ignore Notes
                    if (key == "Notes")
                    {
                        continue;
                    }
                    else
                    {
                        object value = kState.GetValue(key);

                        if (MySet.isValidStr(value))
                        {
                            aState.Add(key, value.ToString());
                        }                        
                    }
                }

                if (aState.Count >= 2)
                {
                    //Always check first
                    if (MySet.isValid(MySDKobject))
                    {
                        MySDKobject.LoadState(aState);  
                    }                    
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: LoadLocalStateInformation()
        // Desc: This function loads the local state information for the 
        //       Launcher Panel        
        //////////////////////////////////////////////////////////////////////
        private void LoadLocalStateInformation(bool bSetLauncherOnly, string ProjectType, string ProjectVersion)
        {
            if (!bSetLauncherOnly)
            {
                object LastSavedVersion = MySet.EnvSettings.GetValue(ProjectType);

                if (MySet.isValidStr(LastSavedVersion))
                {
                    object LastSavedLauncher = MySet.EnvSettings.GetValue(ProjectType + LastSavedVersion.ToString());

                    if (MySet.isValidStr(LastSavedLauncher))
                    {
                        SetLocalState(LastSavedVersion.ToString(), LastSavedLauncher.ToString());                        
                    }
                }
            }
            else if (MySet.isValidStr(ProjectVersion)) // else ONLY look up Launcher Information
            {
                object LastSavedLauncher = MySet.EnvSettings.GetValue(ProjectType+ProjectVersion);

                if (MySet.isValidStr(LastSavedLauncher))
                {
                    SetLocalState("", LastSavedLauncher.ToString());
                } 
            }
            
        }

        /////////////////////////////////////////////////////////////////////
        // Func: DoesLocalStateInformationExist()
        // Desc: Check if Current Project contains State Information        
        //////////////////////////////////////////////////////////////////////
        private bool DoesLocalStateInformationExist(string ProjectType)
        {
          object LastSavedVersion = MySet.EnvSettings.GetValue(ProjectType);

          if (MySet.isValidStr(LastSavedVersion))
          {
              return (true);
          }
          else
          {
              return (false);
          }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: LoadLastProject() - public  called from MAIN form
        // Desc: Loads the last selected project (only used at initialization)
        //////////////////////////////////////////////////////////////////////
        private bool LoadLastProject()
        {

            object LastProject = MySet.EnvSettings.GetValue("LAST");

            if (MySet.isValidStr(LastProject))
            {
                // Select the Version
                foreach (Control cr in PanelToDrawTo.Controls)
                {
                    if (cr.Name == "cBoxProjectType")
                    {

                        ComboBox cb = (ComboBox)cr;
                        int index = cb.Items.IndexOf(LastProject.ToString());

                        if (index >= 0)
                        {
                            cb.SelectedIndex = index;
                            return (true);
                        }                      
                    }
                }
            }
          
            return (false);
        }

        //////////////////////////////////////////////////////////////////////
        // Func: SetLocalState()
        // Desc: Sets the local state of the current project to a version & Launcher        
        //////////////////////////////////////////////////////////////////////
        private void SetLocalState(string Version, string Launcher)
        {

            // Only Select Version Sometimes! "" means NO!
            if (MySet.isValidStr(Version))
            {
                // Select the Version
                foreach (Control cr in PanelToDrawTo.Controls)
                {
                    if (cr.Name == "cBoxVersion")
                    {

                        ComboBox cb = (ComboBox)cr;
                        int index = cb.Items.IndexOf(Version);

                        if (index >= 0)
                        {
                            cb.SelectedIndex = index;
                        }

                    }
                }
            }
             
            // Then Select the Radio Button ~Launcher Information
            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == "pnlLaunchers")
                {
                    // The Radio Buttons are located on their own panel
                    Panel p = (Panel)cr;
                    foreach (Control cr2 in p.Controls)
                    {                        
                        if (cr2.Name == ("rad" + Launcher))
                        {
                            RadioButton rb = (RadioButton)cr2;
                            rb.Checked = true;
                            rb.PerformClick();
                        }
                    }
                }

            }            

        }

        //////////////////////////////////////////////////////////////////////
        // Func: GetAllSelections()
        // Desc: Method iterates thru all the controls on the panel and
        //       sets the global project variables according to user selection               
        //////////////////////////////////////////////////////////////////////
        private void GetAllSelections()
        {

            // Iterate thru all the controls
            foreach (Control cr in PanelToDrawTo.Controls)
            {
                // look for combo boxes
                if (cr.Name.Contains("cBox"))
                {
                    ComboBox cb = (ComboBox)cr;

                    int selected = cb.SelectedIndex;

                    if (cb.Name == "cBoxProjectType")
                    {
                        MySet.ProjectType = cb.Items[selected].ToString();
                    }
                    else if (cb.Name == "cBoxVersion")
                    {
                        MySet.ProjectVersion = cb.Items[selected].ToString();
                    }                    
                }
                // look for radio buttons and settings in the panl
                else if (cr.Name == "pnlLaunchers")
                {
                    Panel pl = (Panel)cr;

                    // Iterate thru the Radio buttons on the panel
                    foreach (Control cr2 in pl.Controls)
                    {
                        if(cr2.Name.Contains("rad"))
                        {                           
                            RadioButton rb = (RadioButton) cr2;

                            // assign the one that is checke as the launcher
                            if (rb.Checked == true)
                            {
                                string stripe = "rad";
                                MySet.LaunchWith = rb.Name.TrimStart(stripe.ToCharArray());
                            }
                        }
                    }                                       
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: FillCBoxWithFolder()
        // Desc: Fills any combo box with information obtained from a file folder        
        //       
        // Prms: +SDKname = the name of a combobox
        //       +FolderDir = the absolute path to a folder to use 
        //       +Dirs = space seperate string containing any folder names you want to obmit or include
        //
        // Note: CBox is not cleared! should be done beforehand. After folder is
        //       is added to cBox, by default the first item is selected
        //////////////////////////////////////////////////////////////////////
        private void FillCBoxWithFolder(string Name, string FolderDir, string Dirs, int iSetToIndex, bool bIsInclude)
        {

            string comparer = "cBox" + Name;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox)cr;

                    DirectoryInfo directory = new DirectoryInfo(FolderDir);

                    if (directory.Exists)
                    {
                        foreach (DirectoryInfo dir in directory.GetDirectories())
                        {
                            
                            if (MySet.isValidStr(Dirs))
                            {                                                                
                                string comp = Dirs;
                                comp = comp.ToUpper();
                                
                                if (bIsInclude)
                                {
                                    if (comp.Contains(dir.Name.ToUpper()))
                                    {
                                        cb.Items.Add(dir.Name);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (comp.Contains(dir.Name.ToUpper()))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        cb.Items.Add(dir.Name);
                                    }
                                }
                            }
                            else
                            {
                                cb.Items.Add(dir.Name);
                            }
                        }

                        if (iSetToIndex >= 0) cb.SelectedIndex = iSetToIndex;
                    }
                }
            }

        }

        //////////////////////////////////////////////////////////////////////
        // Func: ClearCBox()
        // Desc: Deletes all the items inside a CBox        
        //       
        // Prms: +SDKname = the name of an SDK to clear
        //////////////////////////////////////////////////////////////////////
        private void ClearCBox(string Name)
        {
            string comparer = "cBox" + Name;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox)cr;
                    cb.Items.Clear();
                }
            }
        }

    }
}
