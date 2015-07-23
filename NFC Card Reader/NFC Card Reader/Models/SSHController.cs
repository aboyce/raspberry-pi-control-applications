using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace NFC_Card_Reader.Models
{
    public class SSHController
    {
        SshClient _client;
        SshCommand _command;

        // TODO: Finish the setters. Check they are not null etc.
        public string Address { get { return _address; } set { _address = value; } }
        public string Username { get { return _username; } set { _username = value; } }
        public string Password { get { return _password; } set { _password = value; } }
        public bool IsConnected { get { return _client.IsConnected; } }

        private string _address;
        private string _username;
        private string _password;

        // TODO: DO SOME CHECKS FOR NULLS ETC AND IF THE METHODS WILL THROW ERRORS ETC IF CALLED IN THE WRONG ORDER!!!

        public SSHController(string address, string username, string password)
        {
            _address = address;
            _username = username;
            _password = password;

            _client = new SshClient(_address, _username, _password);
        }

        public void Connect()
        {
            _client.Connect();
        }

        public void CreateCommand(string command)
        {
            _command = _client.CreateCommand(command);
        }

        public void Execute()
        {
            _command.Execute();
        }

        public string Result()
        {
            return _command.Result;
        }

        public void CleanUp()
        {
            _client.Disconnect();
            _client.Dispose();
            _command.Dispose();
        }




    }
}
