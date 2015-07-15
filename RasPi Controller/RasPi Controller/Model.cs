using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
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
        public string LoadConfiguration()
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

        /// <summary>
        /// Tries to write the updated configuration back to file.
        /// </summary>
        /// <returns>The error message if something goes wrong. Null if successful.</returns>
        public string SaveConfiguration()
        {
            // TODO: Currently only adds new values, would be ideal to be able to update existing entries. Would also need implementing through out the app.

            string configurationFilePath = ConfigurationManager.AppSettings["ConfigurationFilePath"];

            if (!File.Exists(configurationFilePath))
            {
                return string.Format("File does not exist at [{0}]", configurationFilePath);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(configurationFilePath);

            List<string> rasPiIdsInConfigFile = new List<string>();
            XmlNode raspberryPisNode = doc.SelectSingleNode("/Configuration/RaspberryPis");

            if (raspberryPisNode != null)
                rasPiIdsInConfigFile.AddRange(from XmlNode rp in raspberryPisNode where rp.Attributes != null select rp.Attributes.GetNamedItem("id").Value);

            foreach (RaspberryPi pi in RaspberryPis)
            {
                if (rasPiIdsInConfigFile.Any(id => id == pi.Id)) continue;
                XmlElement rasPiElement = doc.CreateElement(string.Empty, "RaspberryPi", string.Empty);
                if (raspberryPisNode != null) raspberryPisNode.AppendChild(rasPiElement);
                rasPiElement.SetAttribute("id", pi.Id);
                rasPiElement.SetAttribute("networkName", pi.NetworkName);
                rasPiElement.SetAttribute("ipAddress", pi.IpAddress);
                rasPiElement.SetAttribute("username", pi.Username);
            }

            List<string> scriptIdsInConfigFile = new List<string>();
            XmlNode scriptsNode = doc.SelectSingleNode("/Configuration/Scripts");

            if(scriptsNode != null)
                scriptIdsInConfigFile.AddRange(from XmlNode s in scriptsNode where s.Attributes != null select s.Attributes.GetNamedItem("id").Value);

            foreach (Script s in Scripts)
            {
                if (scriptIdsInConfigFile.Any(id => id == s.Id)) continue;
                XmlElement scriptElement = doc.CreateElement(string.Empty, "Script", string.Empty);
                if (scriptsNode != null) scriptsNode.AppendChild(scriptElement);
                scriptElement.SetAttribute("id", s.Id);
                scriptElement.SetAttribute("name", s.Name);
                scriptElement.SetAttribute("description", s.Description);
                scriptElement.SetAttribute("argumentFormat", s.ArgumentFormat);
            }

            doc.Save(configurationFilePath);

            return null;
        }

        /// <summary>
        /// Check that the Raspberry Pi Id does not already exist.
        /// </summary>
        /// <param name="id">The Id to check</param>
        /// <returns>False if it is not unique. True if it is.</returns>
        public bool CheckRaspberryPiIdIsUnique(string id)
        {
            return RaspberryPis.All(pi => pi.Id != id);
        }

        /// <summary>
        /// Check that the Script Id does not already exist.
        /// </summary>
        /// <param name="id">The Id to check</param>
        /// <returns>False if it is not unique. True if it is.</returns>
        public bool CheckScriptIdIsUnique(string id)
        {
            return Scripts.All(script => script.Id != id);
        }
    }
}
