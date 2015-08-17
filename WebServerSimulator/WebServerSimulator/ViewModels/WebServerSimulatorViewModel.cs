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

        private ObservableCollection<string> _logMessages; 
        public ObservableCollection<string> LogMessages {
            get { return _logMessages; }
            set { _logMessages = value; NotifyPropertyChanged("LogMessages"); }
        }

        public WebServerSimulatorViewModel()
        {
            _logMessages = new ObservableCollection<string>();
            Server = new Server(LogMessages);
            StartCommand = new StartCommand(this);
            StopCommand = new StopCommand(this);
            Log("VM Set Up");
        }

        public void Log(string message)
        {
            LogMessages.Add(message);
        }
    }
}
