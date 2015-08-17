using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServerSimulator.Commands;
using WebServerSimulator.Helpers;
using WebServerSimulator.Models;

namespace WebServerSimulator.ViewModels
{
    public class WebServerSimulatorViewModel : INotifyPropertyChanged
    {
        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public Server Server;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private DbSimulator _db;

        private ObservableCollection<string> _logMessages; 
        public ObservableCollection<string> LogMessages {
            get { return _logMessages; }
            set { _logMessages = value; NotifyPropertyChanged("LogMessages"); }
        }

        public WebServerSimulatorViewModel()
        {
            _db = new DbSimulator();
            _logMessages = new ObservableCollection<string>();
            Server = new Server(_db, LogMessages);

            StartCommand = new StartCommand(this);
            StopCommand = new StopCommand(this);
        }

        public void Log(string message)
        {
            LogMessages.Add(message);
        }
    }
}
