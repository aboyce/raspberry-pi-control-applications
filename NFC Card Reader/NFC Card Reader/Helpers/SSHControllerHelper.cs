using System;
using NFC_Card_Reader.Models;

namespace NFC_Card_Reader.Helpers
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
                SSHController _controller = new SSHController(address, username, password);
                _controller.Connect();
                if (_controller.IsConnected)
                {
                    _controller.CreateCommand(string.Format("{0} {1}", scriptName, arguments));
                    _controller.Execute();
                    response = _controller.Result();
                }
                _controller.CleanUp();
            }
            catch (Exception exception)
            {
                return "F" + exception.Message; // F = Fail
            }

            return "P" + response; // P = Pass
        }
    }
}
