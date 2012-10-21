using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace DiagnoseConfig
{    

    public partial class FileHelper : Component
    {

        SortedDictionary<String, XmlDocument> LoadedXMLfiles;
        
        public FileHelper()
        {
            LoadedXMLfiles = null;
            InitializeComponent();
        }

        public FileHelper(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }       

        public void LoadXMLintoMemory(string XMLFile)
        {

            if (!LoadedXMLfiles.ContainsKey(XMLFile))
            {
              XmlDocument doc = new XmlDocument();
              doc.Load(XMLFile);
              LoadedXMLfiles.Add(XMLFile, doc);                
            }

        }

        public XmlNodeList GetXMLElementsByTagName(string XMLFile, string TagName)
        {
            LoadXMLintoMemory(XMLFile);
            XmlDocument doc;
            LoadedXMLfiles.TryGetValue(XMLFile, out doc);                        

            return (doc.GetElementsByTagName(TagName));
        }

    }

}
