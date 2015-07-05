using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;

namespace RasPi_Controller
{
    public class Model
    {
        public List<RaspberryPi> RaspberryPis = new List<RaspberryPi>();
        public List<Script> Scripts = new List<Script>();

        /// <summary>
        /// Tries to load in the Configuration XML file to populate the RaspberryPi and Script lists.
        /// </summary>
        /// <returns>The error message if something goes wrong. Null if successful.</returns>
        public string LoadInConfiguration()
        {
            string configurationFilePath = ConfigurationManager.AppSettings["ConfigurationFilePath"];

            if (!File.Exists(configurationFilePath))
            {
                return string.Format("File does not exist at [{0}]", configurationFilePath);
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configurationFilePath);

                XmlNode raspberryPisNode = doc.SelectSingleNode("/Configuration/RaspberryPis");
                if (raspberryPisNode != null)
                    foreach (XmlNode raspberryPi in raspberryPisNode)
                    {
                        if (raspberryPi.Attributes == null) continue;
                        string id = raspberryPi.Attributes.GetNamedItem("id").Value;
                        string networkName = raspberryPi.Attributes.GetNamedItem("networkName").Value;
                        string ipAddress = raspberryPi.Attributes.GetNamedItem("ipAddress").Value;
                        string username = raspberryPi.Attributes.GetNamedItem("username").Value;
                        RaspberryPis.Add(new RaspberryPi {Id = id, NetworkName = networkName, IpAddress = ipAddress, Username = username});
                    }

                XmlNode scriptsNode = doc.SelectSingleNode("/Configuration/Scripts");
                if (scriptsNode != null)
                    foreach (XmlNode script in scriptsNode)
                    {
                        if (script.Attributes == null) continue;
                        string id = script.Attributes.GetNamedItem("id").Value;
                        string name = script.Attributes.GetNamedItem("name").Value;
                        string description = script.Attributes.GetNamedItem("description").Value;
                        string argumentFormat = script.Attributes.GetNamedItem("argumentFormat").Value;
                        Scripts.Add(new Script {Id = id, Name = name, Description = description, ArgumentFormat = argumentFormat});
                    }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }


    }
}
