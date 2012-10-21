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


namespace SolutionLauncher
{
    public partial class MainFrm : Form
    {
        // Pointer to global Settings
        private Settings MySet;        

        // Stores where to begin to draw both Groups        
        const int HEIGHT_TOP_ADJUSTMENT = 35;      //adjusting height for Top Toolbar
        const int HEIGHT_BOTTOM_ADJUSTMENT = 60;   //adjusting height for Bottom StatusBar        

        public MainFrm(ref Settings settings)
        {            
            InitializeComponent();
            MySet = settings;            
            Initialize();
        }

        //////////////////////////////////////////////////////////////////////
        // Func: Initialize()
        // Desc: Draws the main components onto the main form                
        //////////////////////////////////////////////////////////////////////
        private void Initialize()
        {
            
            // Form Specific Initialization            
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.BringToFront();

            // Groups Initialization
            this.groupLauncher = null;  // means OFF
            this.groupSDKs = null;      // means OFF            
            
            // Which groups to Draw
            DrawGroupsAccordingToSettings();
            
            this.CenterToScreen();
            
            // You can easily remove Controls!!! yeah
            // this.Controls.Remove(this.groupLauncher);

            if (MySet.isValidStr(MySet.SLSettings.GetValue("FadeCount")))
            {
                this.FadeTimer.Interval = Int32.Parse(MySet.SLSettings.GetValue("FadeCount").ToString());
                FadeTimer.Enabled = true;
            }
            else
            {
                this.Opacity = 1;
            }
            
        }

        //////////////////////////////////////////////////////////////////////
        // Func: DrawGroupsAccordingToSettings()
        // Desc: Draws Groups onto main Form according to values in Registry
        //////////////////////////////////////////////////////////////////////
        private void DrawGroupsAccordingToSettings()
        {
            // assign same Width to both Panels
            const int PANEL_WIDTH = 200;

            // height and Width required to show panels
            int height = 0;
            int width = 0;

            // the difference is used in NoteArea Calculation
            int diff = 0;            

            // We want to draw the SDK panel first (on right side) just to know 
            // what height it is this way we can load the launch panel with a text
            // area height that changes dynamically 
            width = DrawSDKGroup(15, 15, PANEL_WIDTH, PANEL_WIDTH, true);                                    
            height = m_groupSDKs.Height;

            // now drawLaunchGroup
            width += DrawLaunchGroup(15, 25, PANEL_WIDTH, 0, 0, true);                        
          
            // And Height if bigger, otherwise assign other way around
            if (m_groupLauncher.Height > height)
            {
                groupSDKs.Height = m_groupLauncher.Height;
                height = m_groupLauncher.Height;
            }
            else
            {
                // Dynamically redraw Launch Panel
                diff = (m_groupSDKs.Height - m_groupLauncher.Height);

                if (diff > 0)
                {
                    // Redraw Launch Panel!!! ~oh mighty, mighty        
                    // this.Controls.Remove(this.groupLauncher);                    
                    // DrawLaunchGroup(15, 25, PANEL_WIDTH, 0, (diff - 16));                                       

                    groupLauncher.Height = m_groupSDKs.Height;
                    height = m_groupSDKs.Height;
                }
            }

            // Now we can finally draw the Launch group (we draw it first in order for tabIndex to work correctly)
            DrawLaunchGroup(15, 25, PANEL_WIDTH, 0, diff, false);
            
            // Now we can finally draw the SDK group
            DrawSDKGroup(15, 15, PANEL_WIDTH, PANEL_WIDTH, false);

            // callback, otherwise SDKs won't work
            m_groupLauncher.SetSDKEnable(ref m_groupSDKs);
                       
            // Do this at end to initialize everything in the Launch Group
            m_groupLauncher.Start();                     

           // Adjust the Height for TOP & Bottom Toolbar!
           this.Height = HEIGHT_TOP_ADJUSTMENT + height + HEIGHT_BOTTOM_ADJUSTMENT;
           this.Width = width; // Both Panels!
        }

        //////////////////////////////////////////////////////////////////////
        // Func: DrawLaunchGroup()
        // Desc: Draws the Launch Group Panel                
        //////////////////////////////////////////////////////////////////////
        private int DrawLaunchGroup(int components_Top_Position, int components_Left_Position, int GroupBox_Launch_WIDTH, int intialPosX, int TextAreaDiff, bool hOnly)
        {            
            // 
            // groupLauncher
            // 
            this.groupLauncher = new System.Windows.Forms.GroupBox();

            if (!hOnly)
            {
                this.groupLauncher.Dock = System.Windows.Forms.DockStyle.None;
                this.groupLauncher.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.groupLauncher.Location = new System.Drawing.Point(intialPosX, HEIGHT_TOP_ADJUSTMENT);
                this.groupLauncher.Name = "groupLauncher";
                this.groupLauncher.TabStop = false;
                this.groupLauncher.Text = "Launch";
                this.groupLauncher.Width = GroupBox_Launch_WIDTH;
            }

            // Now call the main class that does all the work and draw the Launchers to the panel
            m_groupLauncher = new groupLauncher(ref MySet, this, this.groupLauncher, components_Top_Position, components_Left_Position, TextAreaDiff, hOnly);
            this.groupLauncher.Height = m_groupLauncher.Height;

            if (!hOnly)
            {
                this.Controls.Add(this.groupLauncher);
            }

            // Assign Width
            return (GroupBox_Launch_WIDTH);
        }

        //////////////////////////////////////////////////////////////////////
        // Func: DrawSDKGroup()
        // Desc: Draws the SDK Group Panel               
        //////////////////////////////////////////////////////////////////////
        private int DrawSDKGroup(int components_Top_Position, int components_Left_Position, int GroupBox_SDKs_WIDTH, int intialPosX, bool hOnly)
        {                 
            // 
            // groupSDKs
            // 
            
            this.groupSDKs = new System.Windows.Forms.GroupBox();       // intentionally initialized
        
            if (!hOnly)
            {                
                this.groupSDKs.Dock = System.Windows.Forms.DockStyle.None;
                this.groupSDKs.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.groupSDKs.Location = new System.Drawing.Point(intialPosX, HEIGHT_TOP_ADJUSTMENT);
                this.groupSDKs.Name = "groupSDKs";
                this.groupSDKs.TabStop = false;
                this.groupSDKs.Text = "SDKs";
                this.groupSDKs.Width = GroupBox_SDKs_WIDTH;
            }            
                        
            // Now call the main class that does all the work and draw the SDKs to the panel
            m_groupSDKs = new groupSDKs(ref MySet, this.groupSDKs, components_Top_Position, components_Left_Position, hOnly);
            this.groupSDKs.Height = m_groupSDKs.Height;
            
            if (!hOnly)
            {
                this.Controls.Add(this.groupSDKs);
            }
            // Assign Width
            return (GroupBox_SDKs_WIDTH);
        }

        //////////////////////////////////////////////////////////////////////        
        // Click Event Handler - FadeTimer_Tick              
        //////////////////////////////////////////////////////////////////////
        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            this.Opacity += .04;

            if (this.Opacity >= 1)
            {
                FadeTimer.Enabled = false;
            }
        }

        private void stripeVSSBindingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VSSstripeFrm stripe = new VSSstripeFrm(this.Left, this.Top, MySet.ProjectType, MySet.ProjectVersion, m_groupLauncher.CalculateProjectPath(false, false));
            stripe.Show();
        }

        private void markAllFilesNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkAsWritableFrm writable = new MarkAsWritableFrm(this.Left, this.Top, MySet.ProjectType, MySet.ProjectVersion, m_groupLauncher.CalculateProjectPath(false, false));
            writable.Show();
        }

        private void getFromSourceControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetFromSourceControlFrm source = new GetFromSourceControlFrm(this.Left, this.Top, ref MySet);
            source.Show();
        }



        ////////////////////////////////////////////////////////////////////////        
        //// Menu Items Clicks - windowatCenterToolStripMenuItem        
        ////////////////////////////////////////////////////////////////////////
        //private void windowatCenterToolStripMenuItem_Click(object sender, EventArgs e)
        //{
            
        //    // First Check / Uncheck corresponding Menu Item
        //    this.windowatCenterToolStripMenuItem.Checked = true;
        //    this.windowatCenterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;

        //    this.windowAtlastLocationToolStripMenuItem.Checked = false;
        //    this.windowAtlastLocationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Unchecked;
                      



        //}

        ////////////////////////////////////////////////////////////////////////        
        //// Menu Items Clicks - windowAtlastLocationToolStripMenuItem        
        ////////////////////////////////////////////////////////////////////////
        //private void windowAtlastLocationToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //    // First Check / Uncheck corresponding Menu Item
        //    this.windowatCenterToolStripMenuItem.Checked = false;
        //    this.windowatCenterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Unchecked;

        //    this.windowAtlastLocationToolStripMenuItem.Checked = true;
        //    this.windowAtlastLocationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;



            

        //}

        ////////////////////////////////////////////////////////////////////////        
        //// Menu Items Clicks - launchSetModebothToolStripMenuItem        
        ///////////////////////////////////////////////////////////////////////
        //private void launchSetModebothToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //    // First Check / Uncheck corresponding Menu Item
        //    this.setModeOnlyToolStripMenuItem.Checked = false;
        //    this.setModeOnlyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Unchecked;

        //    this.launchSetModebothToolStripMenuItem.Checked = true;
        //    this.launchSetModebothToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;




        //}

        ////////////////////////////////////////////////////////////////////////        
        //// Menu Items Clicks - setModeOnlyToolStripMenuItem        
        ///////////////////////////////////////////////////////////////////////
        //private void setModeOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        //{
            
        //    // First Check / Uncheck corresponding Menu Item
        //    this.launchSetModebothToolStripMenuItem.Checked = false;
        //    this.launchSetModebothToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Unchecked;

        //    this.setModeOnlyToolStripMenuItem.Checked = true;
        //    this.setModeOnlyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;


        //}   

    }
}