using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Arachnid;

namespace EWB
{
    public class BrowserControl : UserControl
    {

        private WebBrowser webBrowser;
        private bool doccompleted_fired;

        public event WebBrowserDocumentCompletedEventHandler DocumentCompleted;
        public Stream DocumentStream;
        public String DocumentURL;
        public string URL
        {
            get
            {
                return (DocumentURL);
            }
            set
            {
                webBrowser.Url = new System.Uri(value);
            }
        }

        // Main Loading Function
        private void init_Load_Browser(string name, string url)
        {
            doccompleted_fired = false;
            CheckAndModifyRegistrySettings();       // Do everything possible to avoid all
                                                    // script errors (disable script related IE setttings)

            webBrowser = new ExtendedWebBrowser();
            webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowser.Name = "web" + name;
            
            webBrowser.Validated +=new EventHandler(webBrowser_Validated);
            webBrowser.Validating += new CancelEventHandler(webBrowser_Validating);
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_doccompleted);

            // Start Loading - the magic starts to happen
            webBrowser.Url = new Uri(url);

            // SuppressScriptErrors
            webBrowser.ScriptErrorsSuppressed = true; // does not supress all script errors, 
                                                      // must also turn off some settings in registry
                                                      // as well as handle run-time errors
        }

        public BrowserControl(ref TabPage tab, string url)
        {            
            init_Load_Browser(tab.Name, url);            
            tab.Controls.Add(webBrowser);
        }

        public BrowserControl(ref Panel panel, string url)
        {
            init_Load_Browser(panel.Name, url);            
            panel.Controls.Add(webBrowser);
        }

        private void webBrowser_Validated(object sender, EventArgs e)
        {
            SupressScriptErrors();
        }

        private void webBrowser_Validating(object sender, CancelEventArgs e)
        {
            SupressScriptErrors();                    
        }

        /*
        * Registry Keys that will be modified:
        * HKCU\SOFWARE\Microsoft\Internet Explorer\Main\Disable Script Debugger
        * HKCU\SOFWARE\Microsoft\Internet Explorer\Main\DisableScriptDebuggerIE
        * HKCU\SOFWARE\Microsoft\Internet Explorer\Main\Error Dlg Displayed On Every Error
        * HKCU\SOFWARE\Microsoft\Internet Explorer\Main\Friendly http errors
        * */
        private void CheckAndModifyRegistrySettings()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main", RegistryKeyPermissionCheck.ReadWriteSubTree);
            
            object o = rk.GetValue("Disable Script Debugger");

            if (Verify.isValid(o) && (o.ToString().ToLower() != "yes"))
            {
                rk.SetValue("Disable Script Debugger", "yes");
            }

            o = rk.GetValue("DisableScriptDebuggerIE");

            if (Verify.isValid(o) && (o.ToString().ToLower() != "yes"))
            {
                rk.SetValue("DisableScriptDebuggerIE", "yes");
            }

            o = rk.GetValue("Error Dlg Displayed On Every Error");

            if (Verify.isValid(o) && (o.ToString().ToLower() != "no"))
            {
                rk.SetValue("Error Dlg Displayed On Every Error", "no");
            }

            o = rk.GetValue("Friendly http errors");

            if (Verify.isValid(o) && (o.ToString().ToLower() != "no"))
            {
                rk.SetValue("Friendly http errors", "no");
            }
        }

        // Supresses Run-Time Script Errors
        private void SupressScriptErrors()
        {
            if (webBrowser.Document != null)
            {
                //((System.Windows.Forms.WebBrowser)sender).Document.Window.Error += new HtmlElementErrorEventHandler(scriptWindow_Error); 
                webBrowser.Document.Window.Error += new HtmlElementErrorEventHandler(scriptWindow_Error);
            }
        }

        private void webBrowser_doccompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            // HtmlWindow hw = webBrowser.Document.Window;            
            
            if (doccompleted_fired)
            {
                doccompleted_fired = false;

                // send it out to any outside listeners
                this.DocumentURL = ((System.Windows.Forms.WebBrowser)sender).Url.ToString();
                this.DocumentStream = ((System.Windows.Forms.WebBrowser)sender).DocumentStream;
                this.DocumentCompleted(sender, e);
                
            }
            else
            {
                // we want to skip the 1st time it is called and now set the
                // outside event / the variables
                SupressScriptErrors(); // can never have enough!
                doccompleted_fired = true;                                
            }                                    
        }

        /*
         * The onerror event fires for run-time errors, but not for compilation errors. 
         * In addition, error dialog boxes raised by script debuggers are not suppressed by returning true. 
         * To turn off script debuggers, disable script debugging in Internet Explorer by choosing Internet 
         * Options from the Tools menu. Click the Advanced tab and select the appropriate check box(es).
        */
        private void scriptWindow_Error(object sender, HtmlElementErrorEventArgs e)
        {
            // We got a script error, lie to the browser and
            // Let the browser know we handled this error.            
            e.Handled = true;
        }

    }
}
