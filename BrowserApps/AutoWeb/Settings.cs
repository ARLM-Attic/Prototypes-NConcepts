using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace AutoWeb
{    
    public class Settings
    {
        // Registry Keys that i am accessing
        public RegistryKey root;
        public RegistryKey AWSettings;        

        public Settings()
        {
            ReadInRegistryKeys();
        }
        
        private void ReadInRegistryKeys()
        {
            root = Registry.CurrentUser;
            AWSettings = root.CreateSubKey("Software\\AutoWeb", RegistryKeyPermissionCheck.ReadWriteSubTree);            
        }

        public void writeValueToReg(string name, string value)
        {
            AWSettings.SetValue(name, value);
        }

        public int getValueFromReg(string name)
        {
            object val = AWSettings.GetValue(name);

            if (isValidStr(val))
            {
                return (int.Parse(val.ToString()));
            }
            else
            {
                return (0);
            }
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
