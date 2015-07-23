using System;
using Renci.SshNet;

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
                SshClient client = new SshClient(address, username, password);
                client.Connect();
                if (client.IsConnected)
                {
                    SshCommand command = client.CreateCommand(string.Format("{0} {1}", scriptName, arguments));
                    command.Execute();
                    response = command.Result;
                }
                client.Disconnect();
                client.Dispose();
            }
            catch (Exception exception)
            {
                return "F" + exception.Message; // F = Fail
            }

            return "P" + response; // P = Pass
        }
    }
}
