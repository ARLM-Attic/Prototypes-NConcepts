using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Collections.Specialized;

namespace SolutionLauncher
{
    //////////////////////////////////////////////////////////////////////
    // Clss: RadioButtonTag
    // Desc: This object is tagged onto the Radio Buttons, this way each
    //       radio button contains the information that i need to make 
    //       everything work        
    //////////////////////////////////////////////////////////////////////
    struct RadioButtonTag
    {
        public string SDKname;
        public string OptionName;
        public string ProjectType;
        public string Folder;
        public string Disabled;
    }

    //////////////////////////////////////////////////////////////////////
    // Clss: groupSDKs
    // Desc: This class is responsible to draw all the SDK components to a panel.
    //       It handles all the logic containing to the SDKs & Selection Components
    //
    // Note: ~
    //        
    //////////////////////////////////////////////////////////////////////
    class groupSDKs
    {
        //Pointer to global Settings
        private Settings MySet;        

        // This variable holds the panel pointer to draw to (IMP)
        private GroupBox PanelToDrawTo;

        // Local Control and Position Vars
        private int topPosition;
        private int LEFT;
        private int tabIndex;
        private int HEIGHT;           // this variable holds the HEIGHT required to show all the components in the Panel       
        private bool hOnly;           // don't actually draw the components just calculate the height
        

        public groupSDKs(ref Settings settings, GroupBox panelToDrawTo, int initTopPosition, int initLeftPosition, bool bHeightOnly)
        {
            MySet = settings;

            if (MySet.isValid(panelToDrawTo))
            {
                tabIndex = 0;
                PanelToDrawTo = panelToDrawTo;

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

                hOnly = bHeightOnly;  // should we only calculate the height

                HEIGHT = 0;
                Initialize();                
            }            
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

            // Then Iterate thru the SDK Settings
            foreach (string keyname in MySet.SDKSettings.GetSubKeyNames())
            {
                // Draw Label and ComboBox
                DrawSDKLabel(keyname);
                DrawSDKComboBox(keyname);

                // Find out if there is a Folder Specified for the SDK
                RegistryKey SDKSpec = MySet.SDKSettings.OpenSubKey(keyname);
                object FolderToParse = SDKSpec.GetValue("Folder");

                // If it is fill cBox with it                                   --- we check for Folder to PARSE here!
                if (MySet.isValidStr(FolderToParse))
                {
                    if (MySet.bSDKManagerNetworkPathIsValid)
                    {
                        FillCBoxWithFolder(keyname, FolderToParse.ToString(), true, "", false);
                    }
                }
                else     // look for Radio Options
                {
                    DrawOptionalRadioButton(keyname);
                }

            }            

            //After drawing all the components Assign the Height Property
            HEIGHT = topPosition;
        }

        //////////////////////////////////////////////////////////////////////
        // Func: DrawSDKLabel()
        // Desc: Draws a specified label to the Panel
        //       
        // Prms: +keyname = the SDK from which to receive the 'label' from                
        //////////////////////////////////////////////////////////////////////
        private void DrawSDKLabel(string SDKName)
        {

            if (!hOnly)
            {
                RegistryKey SDKSpec = MySet.SDKSettings.OpenSubKey(SDKName);

                object labelText = SDKSpec.GetValue("Label");
                string lblName = "lbl" + SDKName;

                // Draw SDK Label
                Label label = new Label();
                label.Left = LEFT;
                label.Top = topPosition;
                label.Width = Settings.WIDTH;
                label.TextAlign = ContentAlignment.MiddleLeft;

                label.UseCompatibleTextRendering = true;
                label.Name = lblName;

                if (MySet.isValidStr(labelText))
                {
                    label.Text = labelText.ToString();
                }
                else
                {
                    label.Text = lblName;
                }
                PanelToDrawTo.Controls.Add(label);
            }

            topPosition += Settings.LABEL_SPACING; // spacing for the label
        }

        //////////////////////////////////////////////////////////////////////
        // Func: DrawSDKComboBox()
        // Desc: Draws a specified comboBox to the Panel
        //       
        // Prms: +keyname = the registry key from which to receive the 'label' from                
        //
        // Note: By default no items will be in the cBox
        //////////////////////////////////////////////////////////////////////
        private void DrawSDKComboBox(string SDKName)
        {
            if (!hOnly)
            {
                // Draw SDK ComboBox
                ComboBox comboBox = new ComboBox();
                comboBox.Left = LEFT;
                comboBox.Top = topPosition;
                comboBox.TabIndex = tabIndex++;
                comboBox.Width = Settings.COMBOBOX_WIDTH;
                comboBox.MaxDropDownItems = Settings.COMBOBOX_MAX_DROP_DOWN_ITEMS;

                comboBox.Name = "cBox" + SDKName;
                comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

                PanelToDrawTo.Controls.Add(comboBox);
            }
            topPosition += Settings.CBOX_SPACING; //spacing for the combo box
        }

        //////////////////////////////////////////////////////////////////////
        // Func: TagComboBox()
        // Desc: Tags a string to a Combo Box, very useful for identifying
        //       
        // Prms: +SDKname = the name of an SDK (to tag to) 
        //       +strTag = the string to tag on        
        //////////////////////////////////////////////////////////////////////
        private void TagComboBox(string SDKName, string strTag)
        {
            string comparer = "cBox" + SDKName;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox)cr;
                    cb.Tag = strTag;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: FillCBoxWithFolder()
        // Desc: Fills any combo box with information obtained from a file folder        
        //       
        // Prms: +SDKname = the name of an SDK (only used if bUseSDKDir = true) 
        //       +FolderDir = the absolute path to a folder to use (only used if bUseSDKDir = false) 
        //       +Dirs = space seperate string containing any folder names you want to omit or include
        //       +bIsInclude = do you want to include or exclude folders in string?
        //
        // Note: CBox is not cleared! should be done beforehand. After folder is
        //       is added to cBox, by default the first item is selected
        //////////////////////////////////////////////////////////////////////
        private void FillCBoxWithFolder(string SDKName, string FolderDir, bool bUseSDKDir, string Dirs, bool bIsInclude)
        { 

            string comparer = "cBox" + SDKName;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox) cr;

                    DirectoryInfo directory;

                    if (bUseSDKDir)
                    {
                        // directory = new DirectoryInfo(MySet.DriveLetter + ":\\" + FolderDir);
                        directory = new DirectoryInfo(MySet.NetworkPath + '\\' + FolderDir);
                    }
                    else
                    {
                       directory = new DirectoryInfo(FolderDir);
                    }

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

                    cb.SelectedIndex = 0; 
                }

            }

        }

        //////////////////////////////////////////////////////////////////////
        // Func: ClearCBox()
        // Desc: Deletes all the items inside a CBox        
        //       
        // Prms: +SDKname = the name of an SDK to clear
        //////////////////////////////////////////////////////////////////////
        private void ClearCBox(string SDKName)
        {            
            string comparer = "cBox" + SDKName;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox)cr;
                    cb.Items.Clear();                    
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: DisableCBox()
        // Desc: Disables a CBox
        //       
        // Prms: +SDKname = the name of an SDK to disable
        //////////////////////////////////////////////////////////////////////
        private void DisableCBox(string SDKName)
        {

            string comparer = "cBox" + SDKName;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox)cr;
                    cb.Enabled = false;                    
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: EnableCBox()
        // Desc: Enables a CBox
        //       
        // Prms: +SDKname = the name of an SDK to enable
        //////////////////////////////////////////////////////////////////////
        private void EnableCBox(string SDKName)
        {

            string comparer = "cBox" + SDKName;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name == comparer)
                {
                    ComboBox cb = (ComboBox)cr;
                    cb.Enabled = true;
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: DrawOptionalRadioButton()
        // Desc: Draws optional Radio Buttons for the SDK (if they are in the 
        //       registry)
        //       
        // Prms: +SDKname = the name of an SDK to draw Radio Buttons for
        //////////////////////////////////////////////////////////////////////
        private void DrawOptionalRadioButton(string SDKName)
        {

            // check if options are enabled and use thos to draw the combo box with options:
            RegistryKey SDKSpecOptions = MySet.SDKSettings.OpenSubKey(SDKName);

            // Check if options are there first
            if (SDKSpecOptions.SubKeyCount >= 1)
            {

                Panel panel = new Panel();    // intentionally itialized even for hOnly

                if (!hOnly)
                {                
                    panel.Left = LEFT;
                    panel.Top = topPosition;
                    panel.Name = "pnl" + SDKName;
                }
                
                int radioButPosition = 0;                

                foreach (string option in SDKSpecOptions.GetSubKeyNames())
                {

                    if (!hOnly)
                    {
                        RegistryKey OptionSpec = SDKSpecOptions.OpenSubKey(option);
                        object isDefault = OptionSpec.GetValue("default");
                        object opLabel = OptionSpec.GetValue("Label");
                        object opProjectType = OptionSpec.GetValue("ProjectType");
                        object opFolder = OptionSpec.GetValue("Folder");

                        RadioButton radioButton = new RadioButton();
                        radioButton.AutoSize = true;
                        radioButton.Top = radioButPosition;
                        radioButton.Left = 0;
                        radioButton.Name = option;
                        radioButton.UseVisualStyleBackColor = true;
                        radioButton.TabIndex = tabIndex++;

                        RadioButtonTag aTag = new RadioButtonTag();
                        aTag.SDKname = SDKName;
                        aTag.OptionName = option;

                        if (MySet.isValidStr(opProjectType))
                        {
                            aTag.ProjectType = opProjectType.ToString();
                        }

                        if (MySet.isValidStr(opFolder))
                        {
                            aTag.Folder = opFolder.ToString();
                        }

                        if (MySet.isValidStr(opLabel))
                        {
                            radioButton.Text = opLabel.ToString();

                            if (radioButton.Text == "Disabled")
                            {
                                aTag.Disabled = "true";
                            }
                        }
                        else
                        {
                            radioButton.Text = option + " label for " + SDKName + " not specified";
                        }

                        // Assigning the Tag to the Button!
                        radioButton.Tag = aTag;

                        radioButton.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);

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

                    radioButPosition += Settings.RBUTTON_SPACING; //spacing for radio button 
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
            }

        }

        //////////////////////////////////////////////////////////////////////
        // Func: CallScriptWithPath()
        // Desc: This Function writes each script (1 line) into a temporary
        //       bat file; that later can be launced.         
        //       
        // Prms: +ScriptName = the name of the Script to call
        //       +Path = path parameter %1 to pass to script file
        //       +bUseSDKDir =  true, use the SDKDir as base path, false will use path as an
        //                      absolute
        //       +version = optional version information %2 to pass down to the script file
        //////////////////////////////////////////////////////////////////////
        private void CallScriptWithPath(string ScriptName, string Path, bool bUseSDKDir, string version)
        {            
            string parameterString;
            
            bool bUseQuotation = false;

            // FEATURE CURRENTLY DISABLED!
            //
            //object UseQuotationMarks = MySet.SLSettings.GetValue("UseQuotationMarksAroundPath");
            //if (MySet.isValidStr(UseQuotationMarks))
            //{
            //    if (UseQuotationMarks.ToString() == "true")
            //    {
            //        bUseQuotation = true;
            //    }                
            //}

            //Absolute or SDK Path
            if (bUseSDKDir)
            {
                if (bUseQuotation)
                {
                    //parameterString = " " + '\"' + MySet.DriveLetter + ':' + '\\' + Path + '\"';
                    parameterString = " " + '\"' + MySet.NetworkPath + '\\' + Path + '\"';
                }
                else
                {
                    //parameterString = " " + MySet.DriveLetter + ':' + '\\' + Path;
                    parameterString = " " + MySet.NetworkPath + '\\' + Path;
                }
            }
            else
            {
                if (bUseQuotation)
                {
                    parameterString = " " + '\"' + Path + '\"';
                }
                else
                {
                    parameterString = " " + Path;
                }
            }
            
            // Command Script
            // string commandString = MySet.DriveLetter + ':' + '\\' + ScriptName;
            string commandString = MySet.NetworkPath + '\\' + ScriptName;

            // Command Parameter            
            string strCmdLine = commandString + parameterString + " " + version;

            MySet.writeLineToTempFileWithoutClosing("call " + strCmdLine);                        
        }

        //////////////////////////////////////////////////////////////////////
        // Func: SetSDKs()
        // Desc: public function. IMP. This function
        //       Iterates thru all the settings selected at the comboboxes and
        //       uses those settings to write out into the temporary file as
        //       well as saving the 'state' globally (so that it can be written
        //       to the Registry)
        //////////////////////////////////////////////////////////////////////
        public void SetSDKs()
        {
            // FEATURE CURRENTLY DISABLED
            // object showPopUp = MySet.SLSettings.GetValue("ShowEnvPopup");
            object showPopUp = "false";
            bool bShowPopup = false;

            // Popup is useful for seeing what environment settings were set
            if(MySet.isValidStr(showPopUp))
            {
                if(showPopUp.ToString() == "true")
                {
                    bShowPopup = true;
                }                
            }

            string textToShowInPopup = "";
            MySet.newTempFile();

            // State infomration is being done iteratively
            MySet.clearState();
            string OptionsString = ""; //Used for Options StateInformation
            string DisabledString = ""; //Used for Disabled Button Information

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name.Contains("cBox"))
                {
                    string stripe = "cBox";
                    string SDKname = cr.Name.TrimStart(stripe.ToCharArray());

                    ComboBox cb = (ComboBox)cr;
                    int selected = cb.SelectedIndex;

                    // If the button is disabled, selected will be -1
                    if (selected >= 0)
                    {
                        // This means that this SDK has options
                        if (MySet.isValidStr(cb.Tag))
                        {
                            textToShowInPopup += SetSDkwithOptions(SDKname, cb.Tag.ToString(), cb.Items[selected].ToString());

                            //StateInformation
                            MySet.addToState(SDKname, cb.Items[selected].ToString());
                            OptionsString += cb.Tag.ToString() + " ";
                        }
                        // no options so get the settings directly from the registry
                        else
                        {
                            RegistryKey SDKSpec = MySet.SDKSettings.OpenSubKey(SDKname);

                            object obFolder = SDKSpec.GetValue("Folder");
                            object obScriptName = SDKSpec.GetValue("ScriptFile");

                            if ((obFolder != null) && (obScriptName != null) && (obFolder.ToString() != "") && (obScriptName.ToString() != ""))
                            {
                                CallScriptWithPath(obScriptName.ToString(), (obFolder.ToString() + '\\' + cb.Items[selected].ToString()), true, cb.Items[selected].ToString());
                            }

                            textToShowInPopup += SDKname + " Value: " + cb.Items[selected].ToString() + "\n";

                            //StateInformation
                            MySet.addToState(SDKname, cb.Items[selected].ToString());
                        }
                    }
                    else
                    {
                        // Mark this option as an disable option for state information
                        if (MySet.isValidStr(cb.Tag))
                        {
                            DisabledString += cb.Tag.ToString() + " ";
                        }
                    }
                }
            }

            MySet.closeTempFile();            

            //Save Option and Disabled State Information
            MySet.addToState("Options", OptionsString);
            MySet.addToState("Disabled", DisabledString);

            if (bShowPopup)
            {
                MySet.Alert(textToShowInPopup);
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: SetSDkwithOptions()
        // Desc: Created to take logic out of Set(). Handles the scenario when
        //       we want to set an SDK that has an option of either Folder, SDK,
        //       or relative path (*handles relative logic here*)
        //
        // Prms: +SDKname = the name of the SDK
        //       +option = the name of the option that was selected for that SDK
        //       +selected =  the selected item in the combo box (as string)        
        //////////////////////////////////////////////////////////////////////
        private string SetSDkwithOptions(string SDKname, string option, string selected)
        {
            RegistryKey SDKSpec = MySet.SDKSettings.OpenSubKey(SDKname);
            RegistryKey SDKSpecOption = SDKSpec.OpenSubKey(option);

            if (MySet.isValid(SDKSpecOption))
            {

                // First Check for Folder, otherwise check for Project Type
                object obFolder = SDKSpecOption.GetValue("Folder");

                if (MySet.isValidStr(obFolder))
                {
                    object obScriptName = SDKSpec.GetValue("ScriptFile");

                    if (MySet.isValidStr(obScriptName))
                    {
                        CallScriptWithPath(obScriptName.ToString(), (obFolder.ToString() + '\\' + selected), true, selected);
                    }
                }
                else
                {
                    // Looking For Project Type
                    object obProjectType = SDKSpecOption.GetValue("ProjectType");
                    RegistryKey obProject = MySet.ProjectTypes.OpenSubKey(obProjectType.ToString());

                    object obPath = obProject.GetValue("BasePath");
                    object obScriptName = SDKSpec.GetValue("ScriptFile");

                    if (MySet.isValidStr(obProjectType) && MySet.isValidStr(obProject) && MySet.isValidStr(obPath) && MySet.isValidStr(obScriptName))
                    {
                        // This is where I'll have to do the base + Relative Calculations
                        object obRelativePath = obProject.GetValue("RelativePath");

                        if (MySet.isValidStr(obRelativePath))
                        {
                            CallScriptWithPath(obScriptName.ToString(), (obPath.ToString() + '\\' + selected + '\\' + obRelativePath.ToString()), false, selected);
                        }
                        else
                        {
                            CallScriptWithPath(obScriptName.ToString(), (obPath.ToString() + '\\' + selected), false, selected);
                        }
                    }
                }
            }

            return (SDKname + " Value: " + selected + " option: " + option + "\n");
        }

        //////////////////////////////////////////////////////////////////////
        // Func: LoadState()
        // Desc: public function. IMP. This function loads the state information.
        //       This is crucial for loading and saving stating information.        
        //////////////////////////////////////////////////////////////////////
        public void LoadState(StringDictionary aState)
        {
            //first load all the options *making sure they are selected*
            string Options = aState["Options"];
            string Disabled = aState["Disabled"];            

            if(MySet.isValidStr(Options))
            {
                SelectAllOptionsInString(Options);
            }

            // disabled is treated the exact same way
            if (MySet.isValidStr(Disabled))
            {
                SelectAllOptionsInString(Disabled);
            }

            // Now select all the comboboxes
            foreach (string key in aState.Keys)
            {
                SelectComboBoxWithValue(key, aState[key]);
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: SelectAllOptionsInString()
        // Desc: Iterates thru all the Options on the Panel and selects the
        //       ones that are listed in the Options string
        //       
        // Prms: +Options = space seperated List Options you want to have selected        
        //////////////////////////////////////////////////////////////////////
        public void SelectAllOptionsInString(string Options)
        {
            
            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if(cr.Name.Contains("pnl"))
                {

                    // The Radio Buttons are located on their own panel
                    Panel p = (Panel)cr;
                    foreach (Control cr2 in p.Controls)
                    {
                        if (Options.Contains(cr2.Name))
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
        // Func: SelectComboBoxWithValue()
        // Desc: Iterates thru all the ComboBoxes on the Panel and selects the
        //       one that has the same value
        //       
        // Prms: +SDKname = name of SDK ComboBox to select
        //       +value = value which should be selected
        //////////////////////////////////////////////////////////////////////
        public void SelectComboBoxWithValue(string SDKName, string value)
        {

            string comparer = "cBox" + SDKName;

            foreach (Control cr in PanelToDrawTo.Controls)
            {
                if (cr.Name.ToLower() == comparer.ToLower())
                {
                    ComboBox cb = (ComboBox)cr;

                    int selected = cb.SelectedIndex;

                    // I know now that the cbox is not disabled
                    if (selected >= 0)
                    {
                        int index = cb.Items.IndexOf(value);

                        if (index >= 0)
                        {
                            cb.SelectedIndex = index;
                        }
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Func: RadioButtonCheckedChanged()
        // Desc: EventHandler for Radio Buttons, checks the tags associated
        //       with each radio button. Depending on the Tag it will either
        //       retrieve the folder information from the SDK directory or from
        //       the registry        
        //////////////////////////////////////////////////////////////////////
        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            RadioButton RB = (RadioButton)sender;                        
            RadioButtonTag rt = (RadioButtonTag) RB.Tag;

            //If it has a valid folder use that
            if (MySet.isValidStr(rt.Folder))
            {
                EnableCBox(rt.SDKname);
                ClearCBox(rt.SDKname);
                TagComboBox(rt.SDKname, rt.OptionName);         // Each combo box knows what option it has selected
                
                if (MySet.bSDKManagerNetworkPathIsValid)
                {
                    FillCBoxWithFolder(rt.SDKname, rt.Folder, true, "", false);  
                } 
            }
            else  // lookup project specifics
            {
                // Disabled Logic!
                if (rt.Disabled == "true")
                {
                    ClearCBox(rt.SDKname);
                    TagComboBox(rt.SDKname, rt.OptionName);
                    DisableCBox(rt.SDKname);                   // Disable the combo Box
                }
                else
                {
                    EnableCBox(rt.SDKname);
                    ClearCBox(rt.SDKname);
                    TagComboBox(rt.SDKname, rt.OptionName);  // Each combo box knows what option it has selected
                    
                    // Retrive Folder Settings from Project Info
                    RegistryKey ProjectType = MySet.ProjectTypes.OpenSubKey(rt.ProjectType);

                    if (MySet.isValid(ProjectType))
                    {
                        // First check for basepath and exclude dirs
                        object path = ProjectType.GetValue("BasePath");
                        object exclude = ProjectType.GetValue("ExcludeDirs");
                        object include = ProjectType.GetValue("IncludeDirs");

                        if (MySet.isValidStr(path))
                        {
                            if (MySet.isValidStr(include) && Directory.Exists(path.ToString()))
                            {
                                FillCBoxWithFolder(rt.SDKname, path.ToString(), false, include.ToString(), true);
                            }
                            else if (MySet.isValidStr(exclude) && Directory.Exists(path.ToString()))
                            {
                                FillCBoxWithFolder(rt.SDKname, path.ToString(), false, exclude.ToString(), false);
                            }
                            else if (Directory.Exists(path.ToString()))
                            {
                                FillCBoxWithFolder(rt.SDKname, path.ToString(), false, "", false);
                            }
                        }
                    }
                    else
                    {
                        MySet.Alert(rt.ProjectType + " is invalid, check Registry");
                    }
                 
                }

            }
        }
    }
}
