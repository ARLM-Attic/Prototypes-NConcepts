using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DiagnoseConfig
{
    public class Rules
    {
        FileHelper m_FH;
        int m_numberOfRules;

        Rules()
        {
            m_FH = new FileHelper();
            m_numberOfRules = 0;
        }

        public int XML_IcarusConfig_Rules()
        {
            // Rule 
            ++m_numberOfRules;
            XmlNodeList Nodes = m_FH.GetXMLElementsByTagName("IcarusConfig.xml", "Config");
            
            foreach (XmlNode Node in Nodes)
            {
                //Decimal.Parse(Node.ChildNodes[0].Value);
            }
            
            //string doris = RAD_PATH;
           
            return (0);
        }

        public int XML_IcarusDCs_Rules()
        {           
                      

            return (0);
        }
    }
}
