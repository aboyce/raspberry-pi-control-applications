using System;
using RasPi_Controller.Models;

namespace RasPi_Controller.Helpers
{
    public static class SSHControllerHelper
    {
        /// <summary>
        /// Sends a command with or without arguments to a remote SSH server, and then returns the response. (Ensure the response will be a single line with no required input!)
        /// </summary>
        /// <param name="address">The address to connect to</param>
        /// <param name="username">The username to use to connect with</param>
        /// <param name="password">The password for the username to connect to</param>
        /// <param name="scriptName">The name of the script to run</param>
        /// <param name="arguments">The arguments (if any) to send after the scriptName</param>
        /// <returns>Either the response from the remote client (prefixed with 'P'), or the error that occurred (prefixed with 'F')</returns>
        public static string SendCommand(string address, string username, string password, string scriptName, string arguments = "")
        {
            string response = string.Empty;

            try
            {
                SSHController controller = new SSHController(address, username, password);
                controller.Connect();
                if (controller.IsConnected)
                {
                    controller.CreateCommand(string.Format("{0} {1}", scriptName, arguments));
                    controller.Execute();
                    response = controller.Result();
                }
                controller.CleanUp();
            }
            catch (Exception exception)
            {
                return "F" + exception.Message; // F = Fail
            }

            return "P" + response; // P = Pass
        }

        /// <summary>
        /// Removes the message from SendCommand's response, leaving only the 'P' or 'F', i.e. the result.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A 'P' or 'F' depending on the result from SendCommand</returns>
        public static string Prefix(string message)
        {
            return message.Substring(0, 1);
        }

        /// <summary>
        /// Removes the prefix from SendCommand's response, leaving just the message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>|The message without the 'P' or 'F' prefix</returns>
        public static string Message(string message)
        {
            return message.Substring(1, message.Length - 1);
        }
    }
}
