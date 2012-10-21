using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using EWB;

namespace AutoWeb
{
    public partial class MainForm : Form
    {

        //Pointer to global Settings
        private Settings MySet;
        private bool isHidden = false;

        public MainForm(ref Settings settings)
        {
            MySet = settings;
            InitializeComponent();
            LoadAllTabs();
        }      

        private void LoadAllTabs()
        {
            int tabIndex = 0;

            // Iterate thru all the Settings Sub-keys and open tabs and
            // browser windows for each
            foreach (string key in MySet.AWSettings.GetSubKeyNames())
            {

                RegistryKey URLsToLoad = MySet.AWSettings.OpenSubKey(key);
                string URL = MySet.GetAndVerifyValue(ref URLsToLoad, "URL");
                                
                if (MySet.isValidStr(URL)) 
                {

                    System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
                    tabPage.Location = new System.Drawing.Point(0, 0);//4, 23);
                    tabPage.Name = "tabPage" + key;
                    tabPage.Padding = new System.Windows.Forms.Padding(3);
                    // tabPage.Size = new System.Drawing.Size(553, 297);                    
                    tabPage.TabIndex = tabIndex++;
                    tabPage.Text = key;
                    tabPage.UseVisualStyleBackColor = true;

                    // Load the Browser into the Tab for better viewing.... :)                    
                    LoadBrowserIntoTab(ref tabPage, URL); 

                    // Now add it to the Main TabControl
                    this.tabControlMain.Controls.Add(tabPage);                    
                }

            }

            this.tabControlMain.ResumeLayout(false);
            this.tabControlMain.PerformLayout();
        }

        private void LoadBrowserIntoTab(ref TabPage tab, string url)
        {
            BrowserControl browser = new BrowserControl(ref tab, url);
        }       

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            if (isHidden)
            {
                isHidden = false;
                this.Show();
                this.Activate();
            }
            else
            {                
                isHidden = true;
                this.Hide();
            }
        }

        private void Form_Closing(object Sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!bIsOKtoClose)
            {
                e.Cancel = true;
                isHidden = true;
                this.Hide();
            }
            else
            {
                // Save Form last position on close
                MySet.writeValueToReg("Height", this.Height.ToString());
                MySet.writeValueToReg("Width", this.Width.ToString());
                MySet.writeValueToReg("Top", this.Top.ToString());
                MySet.writeValueToReg("Left", this.Left.ToString());
            }
        }

        private void Form_Load(object Sender, EventArgs e)
        {

            this.Width = MySet.getValueFromReg("Width");            
            this.Left = MySet.getValueFromReg("Left");            
            this.Height = MySet.getValueFromReg("Height");            
            this.Top = MySet.getValueFromReg("Top");            
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {                       
            // Close the form, which closes the application.
            bIsOKtoClose = true;
            this.Close();
        }
    }
}