using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace OpenSite
{
    public partial class MainForm : Form
    {

        //Pointer to global Settings
        private Settings MySet;


        // private FileBrowser.Browser fileBrowser2;
        // private ShellDll.ShellBrowser shellBrowser = new ShellDll.ShellBrowser();


        public MainForm(ref Settings settings)
        {
            MySet = settings;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {            
            LoadAllTabs(MySet.LastSiteAddress);
        }

        private void LoadAllTabs(string SiteAddress)
        {
            int tabIndex = 0;

            // Iterate thru all the Settings Sub-keys and open tabs and
            // browser windows for each
            foreach (string key in MySet.OSSettings.GetSubKeyNames())
            {

                RegistryKey FolderToLoad = MySet.OSSettings.OpenSubKey(key);
                string PartialFolder = MySet.GetAndVerifyValue(ref FolderToLoad, "Folder");
                
                // Check the directory of the partial folder (all are mapped to c$ for now
                if (MySet.isValidStr(PartialFolder)) // && (Directory.Exists((SiteAddress + "\\\\" + "c$" + "\\" + PartialFolder))))
                {

                    System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
                    tabPage.Location = new System.Drawing.Point(4, 23);
                    tabPage.Name = "tabPage" + key;
                    tabPage.Padding = new System.Windows.Forms.Padding(3);
                    tabPage.Size = new System.Drawing.Size(553, 297);
                    tabPage.TabIndex = tabIndex++;
                    tabPage.Text = key;
                    tabPage.UseVisualStyleBackColor = true;

                    // Load the Browser into the Tab for better viewing.... :)
                    //string strFolderToLoad = "file://" + "\\\\" + SiteAddress + "\\" + "c$" + "\\" + PartialFolder;
                    string strFolderToLoad = "\\\\" + SiteAddress + "\\" + "c$" + "\\" + PartialFolder;
                    LoadBrowserIntoTab2(ref tabPage, strFolderToLoad); 

                    // Now add it to the Main TabControl
                    this.tabControlMain.Controls.Add(tabPage);

                    
                }

            }

            this.tabControlMain.ResumeLayout(false);
            this.tabControlMain.PerformLayout();
        }

        private void LoadBrowserIntoTab2(ref TabPage tab, string url)
        {            
            FileBrowser.Browser fileBrowser2 = new FileBrowser.Browser();
            ShellDll.ShellBrowser shellBrowser = new ShellDll.ShellBrowser();
            FileBrowser.BrowserPluginWrapper pluginWrapper = new FileBrowser.BrowserPluginWrapper();

            tab.Controls.Add(fileBrowser2);
            fileBrowser2.AllowDrop = true;
            fileBrowser2.Dock = System.Windows.Forms.DockStyle.Fill;
            fileBrowser2.ListViewMode = System.Windows.Forms.View.List;
            fileBrowser2.Location = new System.Drawing.Point(0, 0);
            fileBrowser2.Name = "fileBrowser2";
            fileBrowser2.PluginWrapper = pluginWrapper;       // check this! prob don't need...
            // this.fileBrowser2.SelectedNode = treeNode59;
            fileBrowser2.ShellBrowser = shellBrowser;
            fileBrowser2.ShowFolders = false;
            fileBrowser2.Size = new System.Drawing.Size(223, 481);
            fileBrowser2.SplitterDistance = 162;
            //this.fileBrowser2.StartUpDirectory = FileBrowser.SpecialFolders.MyDocuments;
            fileBrowser2.StartUpDirectory = FileBrowser.SpecialFolders.Other;
            //this.fileBrowser2.StartUpDirectoryOther = url;
            fileBrowser2.StartUpDirectoryOther = "c:\\Documents and Settings\\dromischer";
            fileBrowser2.TabIndex = 0;

        }

        private void LoadBrowserIntoTab(ref TabPage tab, string url)
        {
            
            System.Windows.Forms.WebBrowser webBrowser = new System.Windows.Forms.WebBrowser();
            
            webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowser.Location = new System.Drawing.Point(3, 3);
            webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            webBrowser.Name = "web" + tab.Name;
            webBrowser.Url = new System.Uri(url);
            webBrowser.Size = new System.Drawing.Size(547, 291);            

            // Now add it to the Tab :)
            tab.Controls.Add(webBrowser);            
        }
    }
}