using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServerSimulator.Commands;
using WebServerSimulator.Models;

namespace WebServerSimulator.Helpers
{
    public class Server
    {

#region Properties
        private const string DOORID = "door_id";
        private const string CARDID = "card_id";

        public bool IsRunning { get; set; }

        private ObservableCollection<string> _logMessages;
        private DbSimulator _db;

        private HttpListener _listener;

        private bool _success;
        private string Response { get { return _success ? "True" : "False"; } }

#endregion

        public Server(DbSimulator db, ObservableCollection<string> oc)
        {
            _success = false;
            _logMessages = oc;
            _db = db;
        }

        public void Start()
        {
            _listener = new HttpListener();
            if (_db == null)
            {
                Log("ERROR: Problem Contacting the Database");
                return;
            }

            try
            {
                Log("Server Started");
                _listener.Prefixes.Add("http://localhost:1500/DoorManagement/");
                _listener.Start();
                Log("Listener Started");
                IsRunning = true;

                while (_listener.IsListening)
                {
                    Log("Now listening");
                    HttpListenerContext context = _listener.GetContext();
                    NameValueCollection arguments = context.Request.QueryString;

                    string door = arguments.Get(DOORID);
                    string card = arguments.Get(CARDID);

                    if (string.IsNullOrEmpty(door) || string.IsNullOrEmpty(card))
                    {
                        Log("- Received request, missing arguments");
                        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        _success = false;
                    }
                    else
                    {
                        Log(string.Format("- Received request, with DoorId: {0}, CardId: {1}", door, card));

                        if (_db.Doors.Any(d => d.DoorId == door) && _db.Cards.Any(c => c.CardId == card))
                        {
                            Log("- Found a match for the door and card");
                            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            _success = true;
                        }
                        else
                        {
                            Log(string.Format("- Could not find a match for the {0} and/or {1}", DOORID, CARDID));
                            context.Response.StatusCode = (int) HttpStatusCode.OK;
                            _success = false;
                        }
                    }

                    context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(Response);

                    using (Stream stream = context.Response.OutputStream)
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(Response);
                        }
                    }

                    Log("- Responded with: " + Response);
                }
            }
            catch (Exception e)
            {
                if (e.Message ==
                    "The I/O operation has been aborted because of either a thread exit or an application request")
                    Log("Stopped Listening");
                else
                    Log("ERROR: " + e.Message);
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            IsRunning = false;
        }

        private void Log(string message)
        {
            if (_logMessages == null)
                return;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                _logMessages.Add(message);
            });
        }
    }
}
