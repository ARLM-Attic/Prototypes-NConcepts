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
    class Program
    {
        static void Main(string[] args)
        {
            // Load Settings here, so that everyone can access the Executable & Path
            Settings MySettings = new Settings(Path.GetFileName(Application.ExecutablePath), Path.GetDirectoryName(Application.ExecutablePath));                   
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new MainFrm(ref MySettings));            
        }
    }
}
