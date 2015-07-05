using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
                // TODO: Check for NullReferenceException
                foreach (XmlNode raspberryPi in raspberryPisNode)
                {
                    string id = raspberryPi.Attributes.GetNamedItem("id").Value;
                    string name = raspberryPi.Attributes.GetNamedItem("name").Value;
                    string ipAddress = raspberryPi.Attributes.GetNamedItem("ipAddress").Value;
                    RaspberryPis.Add(new RaspberryPi {Id = id, Name = name, IpAddress = ipAddress});
                }

                XmlNode scriptsNode = doc.SelectSingleNode("/Configuration/Scripts");
                // TODO: Check for NullReferenceException
                foreach (XmlNode script in scriptsNode)
                {
                    string id = script.Attributes.GetNamedItem("id").Value;
                    string name = script.Attributes.GetNamedItem("name").Value;
                    Scripts.Add(new Script {Id = id, Name = name});
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
