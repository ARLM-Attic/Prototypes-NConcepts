using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoogleSearch;

using System.IO;


namespace WrapperTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //GoogleSearchFactory.CreateGoogleDesktopSearch();

            //ResultTypes Doris;

            IGoogleSearch MySearch = GoogleSearchFactory.CreateGoogleDesktopSearch();
            IGoogleSearchResult MyResult;

            MyResult = MySearch.Search("Peter");

            String path = "C:\\Documents and Settings\\dromischer\\desktop\\GDStest.txt";
            StreamWriter sw = File.CreateText(path);
            sw.Write(MyResult.QueryResult);   

            
            
            //dataSet1. = (System.Data.DataSet) MyResult.QueryResult;
                    
            

            //MyRichTextBox.Text = MyResult.QueryResult.ToString();
            
            //MyRichTextBox.Lines[0] = "Doris DOris";

        }
    }
}