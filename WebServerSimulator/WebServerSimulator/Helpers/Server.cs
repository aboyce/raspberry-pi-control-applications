using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServerSimulator.Commands;

namespace WebServerSimulator.Helpers
{
    public class Server
    {
        public bool IsRunning { get; set; }

        public Server(ObservableCollection<string> oc)
        {
            _listener = new HttpListener();
            _success = false;
            _logMessages = oc;
        }

        private HttpListener _listener;

        private ObservableCollection<string> _logMessages;

        private bool _success;
        private string Response {
            get { return _success ? "True" : "False"; }
        }

        public void Start()
        {
            try
            {
                Log("Server Started");
                _listener.Prefixes.Add("http://localhost:1500/DoorManagement/");
                _listener.Start();
                Log("Listener Started");
                IsRunning = true;

                while (_listener.IsListening)
                {
                    Log("Listener is listening");
                    HttpListenerContext context = _listener.GetContext();
                    // check variables - update _success.
                    context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(Response);

                    if (_success)
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                    using (Stream stream = context.Response.OutputStream)
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(Response);
                        }
                    }

                    Log("Responded: " + Response);
                }
            }
            catch (Exception e)
            {
                Log("Error occurred: " + e.Message);
            }
        }

        private void Log(string message)
        {
            if(_logMessages == null)
                return;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                _logMessages.Add(message);
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            IsRunning = false;
        }
    }
}
