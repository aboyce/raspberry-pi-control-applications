using System;
using System.Configuration;
using NFC_Card_Reader.Properties;

namespace NFC_Card_Reader
{
    public class Configuration
    {
        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Script { get; set; }

        public string ServerUrl { get; set; }

        public string DoorId { get; set; }


        /// <summary>
        /// Will try to populate the properties. The preferred password is located in Settings.settings, although the one in App.Config will be checked first and if present used. Leave blank to use the proper one.
        /// </summary>
        /// <returns></returns>
        public bool Populate()
        {
            string address = ConfigurationManager.AppSettings["SSH_address"];
            string username = ConfigurationManager.AppSettings["SSH_username"];
            string password = ConfigurationManager.AppSettings["SSH_identification"];
            string script = ConfigurationManager.AppSettings["SSH_script"];
            string scriptPrefix = ConfigurationManager.AppSettings["SSH_script_prefix"];
            string serverUrl = ConfigurationManager.AppSettings["SSH_server_url"];
            string doorId = ConfigurationManager.AppSettings["SSH_door_id"];

            if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(script) && !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(doorId))
            {
                Address = address;
                Username = username;
                ServerUrl = serverUrl;
                DoorId = doorId;

                if (!string.IsNullOrEmpty(scriptPrefix))
                {
                    Script = $"{scriptPrefix} {script}";
                }
                else
                {
                    Script = script;
                }
            }
            else
            {
                return false;
            }

            if (!string.IsNullOrEmpty(password))
            {
                Password = password;
            }
            else
            {
                password = Settings.Default.SSH_password;
                if (!string.IsNullOrEmpty(password))
                {
                    Password = password;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
