using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;
using EWB;
using System.Threading;
using System.Collections;

namespace ForeclosureApp
{
    public struct PropertyDetails
    {
        public string Address;
        public bool isApt;
        public string AptNo;
        public string City;        
        public string State;
        public int ZipCode;
        public int SQLlink;

        public PropertyDetails(string city, string state, bool isApt)
        {
            // default values:
            this.isApt = isApt;
            this.City = city;
            this.State = state;
            this.Address = "";            
            this.ZipCode = 0;
            this.AptNo = "";
            this.SQLlink = 0;
        }
    }

    public struct ParsingState
    {
        public string Status;
        public string currentPage;
        public string currentProperty;
        public int SuccessfullyParsed;
        public int UnsuccesfullyParsed;        
    }    

    public partial class FormMain : Form
    {                               

        private Parser_DailyReport p_DailyR;

        // private flags
        private bool bIsDRstarted = false;
        private bool bHARDStop = false;
        private bool progressReported = false;
        private bool bNonNumberEntered = false;

        // private data
        private int iStartPage = Parser_DailyReport.URL_PAGE_START_INDEX_DEFAULT;
        private string Base_Folder_Path = "C:\\Property\\DailyReport\\";

        // Lock
        object cs_FrmLock;

        public FormMain()
        {
            InitializeComponent();         
            
            p_DailyR = new Parser_DailyReport("DailyReport", "C:\\Property");
            BrowserControl browser1 = new BrowserControl(ref this.panelMain, Parser_DailyReport.INDEX_URL_TO_PARSE);
            BrowserControl browser2 = new BrowserControl(ref this.tabUnsuccessful, "file://c:\\Property\\DailyReport\\NOT_PARSED\\index.htm");
            textBoxStartPage.Text = iStartPage.ToString();

            cs_FrmLock = new Object();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (bNonNumberEntered == true)
            {
                e.Handled = true;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            bNonNumberEntered = false;

            if (e.Shift == true || e.Alt == true)
            {
                bNonNumberEntered = true;
                return;
            }
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back)
                    {
                        bNonNumberEntered = true;
                    }
                }
            }
        }

        private void StartParsingAction(object sender, DoWorkEventArgs e )
        {            
            
            BackgroundWorker bwAsync = (BackgroundWorker) sender;
            bool bDoneParsing = false;

            try
            {
                // Parse Daily Report for foreclosures
                while (!bHARDStop)
                {                    

                    if (p_DailyR.GetIndexPage(iStartPage))
                    {

                        ParsingState ps = new ParsingState();
                        ps.Status = "Started";
                        ps.SuccessfullyParsed = p_DailyR.SuccessfullyParsed;
                        ps.UnsuccesfullyParsed = p_DailyR.UnsuccessfullyParsed;
                        ps.currentPage = p_DailyR.CurrentPage.ToString();
                        // this.lblShowProperty.Text = p_DailyR.CurrentProperty;

                        bwAsync.ReportProgress(10, ps);

                        // Make Sure Progress is reported BEFORE parsing
                        while (!progressReported)
                        {
                            System.Threading.Thread.Sleep(50);
                        }

                        lock (cs_FrmLock)
                        {
                            progressReported = false;
                        }

                        // parse the current index page for all 
                        // the foreclosure listings
                        if (!p_DailyR.Parse())
                        {
                            MessageBox.Show("We are done parsing for this particular day");                            
                            bHARDStop = true;
                        }                               
                 
                        // Be nice and timeout every once in awhile
                        System.Threading.Thread.Sleep(Parser_DailyReport.SLEEP_TIME_OUT_AFTER_EVERY_INDEX);
                    }                    
                }

                if (p_DailyR.LastPageReached)
                {
                    MessageBox.Show("We parsed everything - yeah!");
                }
                else if(!bDoneParsing)
                {
                    MessageBox.Show("Not Everything was parsed - either Application or User aborted");
                }

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }            
        }      

        private void textBoxURL_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!bIsDRstarted)
            {
                bIsDRstarted = true;
                bHARDStop = false;
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);                
                worker.DoWork += new DoWorkEventHandler(StartParsingAction);

                iStartPage = int.Parse(this.textBoxStartPage.Text);

                // Refresh all the controls and sleep for a bit to make sure
                // all is well...
                this.tabControls.Refresh();
                System.Threading.Thread.Sleep(50);

                worker.RunWorkerAsync();
            }
        }

        private void ProgressChanged
            (object sender, ProgressChangedEventArgs e)
        {
            
            ParsingState ps = (ParsingState)e.UserState;

            if (bIsDRstarted)
            {
                this.lblShowStatus.Text = ps.Status;
            }
            this.lblShowSuccessful.Text = ps.SuccessfullyParsed.ToString();
            this.lblShowUnsuccessful.Text = ps.UnsuccesfullyParsed.ToString();
            this.lblShowPage.Text = ps.currentPage;

            // Refresh all the controls and sleep for a bit to make sure
            // all is well...
            this.tabControls.Refresh();
            System.Threading.Thread.Sleep(50);

            // Let the Background Thread know it can continue
            this.progressReported = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (bIsDRstarted)
            {
                p_DailyR.Stop = true;

                this.lblShowStatus.Text = "Stopping";  

                while(!p_DailyR.IsStopped)
                {
                    System.Threading.Thread.Sleep(50);  
                }
                
                this.lblShowStatus.Text = "Stopped";  

                // Refresh all the controls and sleep for a bit to make sure
                // all is well...
                this.tabControls.Refresh();
                System.Threading.Thread.Sleep(50);

                bIsDRstarted = false;
                bHARDStop = true;                  
            }
        }

        private void lblUnsuccesful_Click(object sender, EventArgs e)
        {

        }

        private void textBoxStartPage_TextChanged(object sender, EventArgs e)
        {

        }        

        private void tabREFile_Click(object sender, EventArgs e)
        {
            // Make sure all data is gone first
            comboBoxFileSelect.Items.Clear();

            DirectoryInfo directory = new DirectoryInfo(Base_Folder_Path);

            if (directory.Exists)
            {
                foreach (FileInfo file in directory.GetFiles("*.txt"))
                {
                    comboBoxFileSelect.Items.Add(file.Name);
                }

                comboBoxFileSelect.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Base_Folder_Path NOT Correct");
            } 
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            try
            {

                string FileToLoad = Base_Folder_Path + comboBoxFileSelect.Items[comboBoxFileSelect.SelectedIndex].ToString();

                if (File.Exists(FileToLoad))
                {
                    if (chkParseParam.Checked || textBoxRE.Text != "")
                    {
                        // make sure that all the data is gone before continuing
                        dataGridView1.Columns.Clear();

                        StreamReader sr = new StreamReader(FileToLoad, (System.Text.Encoding.GetEncoding(1252)));
                        string ToParse = sr.ReadToEnd();

                        Regex temprx;
                        MatchCollection tmatches;
                        String WideCondition = "";
                        String NarrowCondition = "";

                        // Use built in REexpression
                        if (chkParseParam.Checked)
                        {
                           // First parse out what we need from the selection box
                           temprx = new Regex(@"\d\)\s(?<sename>.*)\s-\s(?<pname>.*).txt");
                           Match tmatch = temprx.Match(comboBoxFileSelect.Items[comboBoxFileSelect.SelectedIndex].ToString());
                           
                            // Now Assign the correct RE
                           NarrowCondition = tmatch.Groups["sename"].ToString();
                           WideCondition = tmatch.Groups["pname"].ToString();

                           temprx = p_DailyR.GetRESubExpression(NarrowCondition); //, tmatch.Groups["pname"].ToString());

                        }
                        else
                        {
                            temprx = new Regex(@textBoxRE.Text);
                        }                        

                        string[] Groups = temprx.GetGroupNames();
                        int noOfGroups = 0;         // we don't need digit groups

                        // first calculate the number of non-digit groups
                        foreach (string str in Groups)
                        {
                            if (!char.IsDigit(str, 0))
                            {
                                ++noOfGroups;
                            }
                        }

                        System.Windows.Forms.DataGridViewTextBoxColumn dl = new System.Windows.Forms.DataGridViewTextBoxColumn();
                        dl.HeaderText = "LineNumber";
                        dl.Name = "LineNumber";
                        dl.FillWeight = 25;
                        this.dataGridView1.Columns.Add(dl);

                        // Now calculate and draw out the column headers and  group names
                        ArrayList grouplist = new ArrayList();
                        foreach (string str in Groups)
                        {
                            // we only want to show named groups
                            if (!char.IsDigit(str,0))
                            {
                                System.Windows.Forms.DataGridViewTextBoxColumn dg = new System.Windows.Forms.DataGridViewTextBoxColumn();
                                dg.HeaderText = str;
                                dg.Name = str;
                                dg.FillWeight = (100 / noOfGroups);
                                this.dataGridView1.Columns.Add(dg);
                                grouplist.Add(str);
                            }

                        }                        
                        
                        // Now Parse the TextFile and we'll see what happens yeah! :)
                        tmatches = temprx.Matches(ToParse);

                        string[] tstring = new string[(noOfGroups + 1)];
                        DataGridViewRowCollection rows = this.dataGridView1.Rows;                        
                        int it = 1;

                        foreach (Match tmatch in tmatches)
                        {
                            tstring[0] = it.ToString();
                            // we don't need group 0
                            for (int i = 1; i < (noOfGroups + 1); ++i)
                            {

                                string groupname = grouplist[i - 1].ToString();

                                if (chkParseParam.Checked)
                                {

                                    Parser pr = p_DailyR.get_Parser_pointer;

                                    PropertyDetails pd = new PropertyDetails();
                                    Match t = tmatch;
                                    pr.ParsePropertyDetail(ref t, WideCondition, NarrowCondition, ref pd);

                                    if (groupname == "addr")
                                    {
                                        tstring[i] = pd.Address;
                                    }
                                    else if (groupname == "city")
                                    {
                                        tstring[i] = pd.City;
                                    }
                                    else if (groupname == "zip")
                                    {
                                        tstring[i] = pd.ZipCode.ToString();
                                    }
                                    else
                                    {
                                        tstring[i] = tmatch.Groups[groupname].ToString();
                                    }
                                }
                                else
                                {
                                    tstring[i] = tmatch.Groups[groupname].ToString();
                                }
                                
                            }
                            ++it;                                                       
                            rows.Add(tstring);                                                        
                        }                       
                    }
                    else
                    {
                        MessageBox.Show("Enter Regular Expression");
                    }

                }

            }
            catch (Exception eC)
            {
                MessageBox.Show("Regulare Expression Error - " + eC.Message);
            }
        }
    }
}