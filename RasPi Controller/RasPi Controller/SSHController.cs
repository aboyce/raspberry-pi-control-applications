using System;
using Renci.SshNet;

namespace RasPi_Controller
{
    static class SshController
    {
        public static string SendCommand(string address, string username, string password, string scriptName, string scriptArguments)
        {
            try
            {
                SshClient client = new SshClient(address, username, password);
                client.Connect();
                if (client.IsConnected)
                {
                    client.RunCommand(scriptName + " " + scriptArguments);
                }
                client.Disconnect();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
    }
}
