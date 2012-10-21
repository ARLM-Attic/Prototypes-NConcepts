using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoWeb
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings MySettings = new Settings();
            Application.Run(new MainForm(ref MySettings));
        }
    }
}