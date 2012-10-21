using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace ForeclosureAppTester
{    

    public partial class Form1 : Form
    {
        private string Base_Folder_Path = "C:\\Property\\DailyReport\\";

        public Form1()
        {
            InitializeComponent();

            DirectoryInfo directory = new DirectoryInfo(Base_Folder_Path);

            if (directory.Exists)
            {
                foreach (FileInfo file in directory.GetFiles("*.txt"))
                {
                    comboBox1.Items.Add(file.Name);
                }

                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Base_Folder_Path NOT Correct");
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                string FileToLoad = Base_Folder_Path + comboBox1.Items[comboBox1.SelectedIndex].ToString();

                if (File.Exists(FileToLoad))
                {
                    if (textRE.Text != "")
                    {
                        // make sure that all the data is gone before continuing
                        dataGridView1.Columns.Clear();

                        StreamReader sr = new StreamReader(FileToLoad, (System.Text.Encoding.GetEncoding(1252)));
                        string ToParse = sr.ReadToEnd();

                        Regex temprx;
                        MatchCollection tmatches;

                        temprx = new Regex(@textRE.Text);

                        string[] Groups = temprx.GetGroupNames();
                        int noOfGroups = Groups.Length - 1;         // we don't need group 0

                        foreach (string str in Groups)
                        {
                            // I don't need the whole match group
                            if (str != "0")
                            {
                               System.Windows.Forms.DataGridViewTextBoxColumn dg = new System.Windows.Forms.DataGridViewTextBoxColumn();
                               dg.HeaderText = str;
                               dg.Name = str;
                               dg.FillWeight = (100 / (noOfGroups - 1));
                               this.dataGridView1.Columns.Add(dg);
                            }

                        }

                        // Now Parse the TextFile and we'll see what happens yeah! :)
                        tmatches = temprx.Matches(ToParse);

                        string[] tstring = new string[noOfGroups];                        
                        DataGridViewRowCollection rows = this.dataGridView1.Rows;

                        foreach (Match tmatch in tmatches)
                        {
                            // we don't need group 0
                            for (int i = 1; i <= noOfGroups; ++i)
                            {                                
                                tstring[i-1] = tmatch.Groups[Groups[i]].ToString();                                                                                                                            
                            }

                            rows.Add(tstring);
                        }
                        
                        //match(matchf.Groups["compl"].ToString());
                        //pd.Address = tmatch.Groups["addr"].ToString();
                        //rows.Add(new string[] {"ist einfach", "toll"} );
                        
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
                

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}