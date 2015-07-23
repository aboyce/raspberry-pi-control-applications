using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using RasPi_Controller.Models;
using RasPi_Controller.Helpers;

namespace RasPi_Controller.ViewModels
{
    class MainWindowViewModel
    {
        public ObservableCollection<RaspberryPi> RaspberryPis;
        public ObservableCollection<Script> Scripts;

        public MainWindowViewModel()
        {
            // TODO: Check that they both are not null, and decide what to do if they are?
            RaspberryPis = ViewModelPopulater.LoadRaspberryPisFromConfiguration();
            Scripts = ViewModelPopulater.LoadScripsFromConfiguration();

            
        }

        /// <summary>
        /// Tries to ping the parameter.
        /// </summary>
        /// <param name="toPing">The string to try and ping.</param>
        /// <returns>Null if it succeeded. The error message if not.</returns>
        public string TryToPing(string toPing)
        {
            if (toPing != null)
                toPing = CleanString(toPing);
            else
                return "Cannot ping as the value is null.";

            return toPing == string.Empty ? "Cannot ping as the value is empty" : Ping(toPing);
        }

        /// <summary>
        /// Removes extra characters that RichTextBoxes may add \r\n to the end of the string.
        /// </summary>
        /// <param name="stringToClean">The string to clean.</param>
        /// <returns>The string without the \r\n.</returns>
        public string CleanString(string stringToClean)
        {
            return stringToClean.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        /// <summary>
        /// Will try to convert a string into an IP Address. Failing that, assume it is a hostname, and try to resolve that.
        /// </summary>
        /// <param name="toCheck">The string to try and convert.</param>
        /// <returns>Null if it failed. The IP Address if successful.</returns>
        private IPAddress CheckOrResolveIpAddress(string toCheck)
        {
            IPAddress ipAddress;

            toCheck = CleanString(toCheck);

            if (!IPAddress.TryParse(toCheck, out ipAddress)) // Try to parse the string into an IP Address.
            {
                // If that fails, it may be a hostname, try to resolve it into an IP Address (seems to throw a lot of exceptions when it doesn't like something).

                IPHostEntry ipHost;

                // TODO: if it cannot resolve it, it seems to crash... The Try-Catch works for now, but would be nice to relay the actual error to the user.
                try
                {
                    ipHost = Dns.GetHostEntry(toCheck);
                }
                catch (Exception e)
                {
                    string debug = e.Message;
                    return null;
                }

                if (ipHost != null && ipHost.AddressList != null && ipHost.AddressList.Length > 0)
                {
                    ipAddress = ipHost.AddressList[0]; // Assume it is the first one.
                }
                else
                {
                    return null;
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Will use CheckOrResolveIpAddress() to get an IP Address, then try to ping it.
        /// </summary>
        /// <param name="toPing">The IP Address or Hostname to ping</param>
        /// <returns>Null if successful. The error message if an error occurred.</returns>
        private string Ping(string toPing)
        {
            IPAddress ipAddress = CheckOrResolveIpAddress(toPing);

            if (ipAddress == null)
                return "Invalid IP Address/Hostname";

            Ping ping = new Ping();
            PingReply reply = null;

            try
            {
                reply = ping.Send(ipAddress);
            }
            catch (PingException pe)
            {
                return string.Format("Could not ping {0}, {1}", toPing, pe.Message);
            }
            catch (Exception e)
            {
                return string.Format("Could not ping {0}, {1}", toPing, e.Message);
            }

            if (reply != null && reply.Status == IPStatus.Success)
            {
                //string message = reply.Status.ToString();
                return null;
            }
            return string.Format("Could not ping {0}", toPing);
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
