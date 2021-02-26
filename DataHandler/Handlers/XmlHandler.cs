using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DataHandler.Handlers
{
    class XmlHandler
    {
        private static XmlHandler instance = null;
        public static XmlHandler GetInstance()
        {
            if (instance == null)
            {
                instance = new XmlHandler();
            }
            return instance;
        }

        public XmlHandler()
        {
            LoadXmlData();
        }

        private List<string> listName;

        public List<string> ListSchemaName
        {
            get
            {
                return listName;
            } 
        }

        private void LoadXmlData()
        {
            try
            {
                if(listName == null)
                {
                    listName = new List<string>();
                }

                string sXmlFileName = "DataSettings.xml";
                string sXmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sXmlFileName);

                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(sXmlPath);
                XmlNodeList nodes = xdoc.SelectNodes("/schemas/file");

                foreach (XmlNode node in nodes)
                {
                    string id = node.Attributes["id"].Value;
                    listName.Add(node.SelectSingleNode("name").InnerText);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
