using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using RasPi_Controller.Extension_Methods;

namespace RasPi_Controller.Helpers
{
    public static class NetworkingHelper
    {
        /// <summary>
        /// Tries to ping the parameter.
        /// </summary>
        /// <param name="toPing">The string to try and ping.</param>
        /// <returns>Null if it succeeded. The error message if not.</returns>
        public static string TryToPing(string toPing)
        {
            if (toPing != null)
                toPing = toPing.Clean();
            else
                return "Cannot ping as the value is null.";

            return toPing == string.Empty ? "Cannot ping as the value is empty" : Ping(toPing);
        }

        /// <summary>
        /// Will try to convert a string into an IP Address. Failing that, assume it is a hostname, and try to resolve that.
        /// </summary>
        /// <param name="toCheck">The string to try and convert.</param>
        /// <returns>Null if it failed. The IP Address if successful.</returns>
        private static IPAddress CheckOrResolveIpAddress(string toCheck)
        {
            IPAddress ipAddress;

            toCheck = toCheck.Clean();

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
        private static string Ping(string toPing)
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
    }
}
