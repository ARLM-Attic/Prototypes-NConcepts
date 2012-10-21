using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace OpenSite
{    
    public class Settings
    {
        // Registry Keys that i am accessing
        public RegistryKey root;
        public RegistryKey OSSettings;

        public string LastSiteAddress;
        public string LastUsername;
        public string LastPassword;
        
        public string UsernameCache;
        public string PasswordCache;

        public Settings()
        {
            ReadInRegistryKeys();
        }
        
        private void ReadInRegistryKeys()
        {
            root = Registry.CurrentUser;
            OSSettings = root.CreateSubKey("Software\\OpenSite");

            LastSiteAddress = GetAndVerifyValue(ref OSSettings, "LastSiteAddress");
            LastUsername = GetAndVerifyValue(ref OSSettings, "LastUsername");
            LastPassword = GetAndVerifyValue(ref OSSettings, "LastPassword");
            UsernameCache = GetAndVerifyValue(ref OSSettings, "UsernameCache");
            PasswordCache = GetAndVerifyValue(ref OSSettings, "PasswordCache");           
        }

        public string GetAndVerifyValue(ref RegistryKey reg, string value)
        {
            object ob = reg.GetValue(value);

            if (isValidStr(ob))
            {
                return ob.ToString();
            }
            else
            {
                return "";
            }
        }

        public bool isValid(object oToValidate)
        {
            if (oToValidate != null)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        public bool isValidStr(object oToValidate)
        {
            if ((oToValidate != null) && (oToValidate.ToString() != ""))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
    }
}
