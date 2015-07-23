using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using RasPi_Controller.Models;

namespace RasPi_Controller.Helpers
{
    public static class ModelHelper
    {
        /// <summary>
        /// Tries to load in the Configuration XML file to populate and return an ObservableCollection of RaspberryPis.
        /// </summary>
        /// <returns>The null if something goes wrong. The ObservableCollection if successful.</returns>
        public static ObservableCollection<RaspberryPi> LoadRaspberryPisFromConfiguration()
        {
            ObservableCollection<RaspberryPi> raspberryPis = new ObservableCollection<RaspberryPi>();

            string configurationFilePath = ConfigurationManager.AppSettings["ConfigurationFilePath"];

            if (!File.Exists(configurationFilePath))
            {
                return null;
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
                        raspberryPis.Add(new RaspberryPi {Id = id, NetworkName = networkName, IpAddress = ipAddress, Username = username});
                    }
            }
            catch (Exception e)
            {
                // TODO: Log
                return null;
            }

            return raspberryPis;
        }

        /// <summary>
        /// Tries to load in the Configuration XML file to populate and return a ObservableCollection of Scripts.
        /// </summary>
        /// <returns>The null if something goes wrong. The ObservableCollection if successful.</returns>
        public static ObservableCollection<Script> LoadScripsFromConfiguration()
        {
            ObservableCollection<Script> scripts = new ObservableCollection<Script>();

            string configurationFilePath = ConfigurationManager.AppSettings["ConfigurationFilePath"];

            if (!File.Exists(configurationFilePath))
            {
                return null;
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configurationFilePath);

                XmlNode scriptsNode = doc.SelectSingleNode("/Configuration/Scripts");
                if (scriptsNode != null)
                    foreach (XmlNode script in scriptsNode)
                    {
                        if (script.Attributes == null) continue;
                        string id = script.Attributes.GetNamedItem("id").Value;
                        string name = script.Attributes.GetNamedItem("name").Value;
                        string description = script.Attributes.GetNamedItem("description").Value;
                        string argumentFormat = script.Attributes.GetNamedItem("argumentFormat").Value;
                        scripts.Add(new Script { Id = id, Name = name, Description = description, ArgumentFormat = argumentFormat });
                    }
            }
            catch (Exception e)
            {
                // TODO: Log
                return null;
            }

            return scripts;
        }

        /// <summary>
        /// Tries to write the updated configuration back to file.
        /// </summary>
        /// <returns>The error message if something goes wrong. Null if successful.</returns>
        public static bool SaveConfiguration(ObservableCollection<RaspberryPi> raspberryPis, ObservableCollection<Script> scripts)
        {
            // TODO: Currently only adds new values, would be ideal to be able to update existing entries. Would also need implementing through out the app.

            string configurationFilePath = ConfigurationManager.AppSettings["ConfigurationFilePath"];

            if (!File.Exists(configurationFilePath))
            {
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(configurationFilePath);

            List<string> rasPiIdsInConfigFile = new List<string>();
            XmlNode raspberryPisNode = doc.SelectSingleNode("/Configuration/RaspberryPis");

            if (raspberryPisNode != null)
                rasPiIdsInConfigFile.AddRange(from XmlNode rp in raspberryPisNode where rp.Attributes != null select rp.Attributes.GetNamedItem("id").Value);

            foreach (RaspberryPi pi in raspberryPis)
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

            foreach (Script s in scripts)
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

            return true;
        }
    }
}
