using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WebServerSimulator.Models;

namespace WebServerSimulator.Core
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
                IsRunning = true;
                Log("Server Started");

                string prefix = ConfigurationManager.AppSettings["ServerPrefix"];
                string ipAddress = ConfigurationManager.AppSettings["ServerAddress"];

                if (string.IsNullOrEmpty(prefix))
                    prefix = "";

                if (string.IsNullOrEmpty(ipAddress))
                    ipAddress = "127.0.0.1/";

                _listener.Prefixes.Add(prefix + ipAddress);
                _listener.Start();
                Log("Listener Started");

                while (_listener.IsListening)
                {
                    Log("Now listening");
                    HttpListenerContext context = _listener.GetContext();

                    string door = "";
                    string card = "";

                    string rawData = System.Web.HttpUtility.UrlDecode(new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd());

                    if (!string.IsNullOrEmpty(rawData) && rawData.Contains("&"))
                    {
                        string[] rawDataSplit = rawData.Split('&');

                        card = rawDataSplit[0].Substring(CARDID.Length + 1); // For some reason the door_id and card_id are reversed compared the order they where sent in.
                        door = rawDataSplit[1].Substring(DOORID.Length + 1);
                    }

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
                IsRunning = false;
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
                _logMessages.Add(string.Format("[{0}:{1}:{2}.{3}] {4}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, message));
            });
        }
    }
}
