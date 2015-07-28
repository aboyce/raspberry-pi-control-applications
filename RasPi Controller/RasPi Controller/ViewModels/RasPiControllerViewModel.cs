using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using RasPi_Controller.Models;
using RasPi_Controller.Helpers;
using System.Windows.Input;
using RasPi_Controller.Commands;

namespace RasPi_Controller.ViewModels
{
    public class RasPiControllerWindowViewModel : INotifyPropertyChanged
    {
        #region Properties

        private ObservableCollection<RaspberryPi> _raspberryPis;
        public ObservableCollection<RaspberryPi> RaspberryPis {
            get { return _raspberryPis; }
            set { _raspberryPis = value; NotifyPropertyChanged("RaspberryPis"); }
        }

        private ObservableCollection<Script> _scripts;
        public ObservableCollection<Script> Scripts {
            get { return _scripts; }
            set { _scripts = value; NotifyPropertyChanged("Scripts"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadConfigCommand { get; set; }
        public ICommand TestIpAddressCommand  { get; set; }
        public ICommand TestNetworkNameCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand SaveRasPiCommand { get; set; }
        public ICommand SaveScriptCommand { get; set; }
        public ICommand SaveConfigCommand { get; set; }
        public ICommand HelpCommand { get; set; }

        private bool _loadedFromConfig;
        public bool LoadedFromConfig
        {
            get { return _loadedFromConfig;}
            set { _loadedFromConfig = value;
                NotifyPropertyChanged("LoadedFromConfig");
            }
        }
        public bool ObservableCollectionsHaveChanged { get; set; }

#endregion

        #region Constructor and Setup

        public RasPiControllerWindowViewModel()
        {
            Setup();
        }

        private void Setup()
        {
            // TODO: Check that they both are not null, and decide what to do if they are?
            RaspberryPis = ModelHelper.LoadRaspberryPisFromConfiguration();
            Scripts = ModelHelper.LoadScripsFromConfiguration();

            LoadConfigCommand = new LoadConfigCommand(this);
            TestIpAddressCommand = new TestIpAddressCommand(this);
            TestNetworkNameCommand = new TestNetworkNameCommand(this);
            SendCommand = new SendCommand(this);
            SaveRasPiCommand = new SaveRasPiCommand(this);
            SaveScriptCommand = new SaveRasPiCommand(this);
            SaveConfigCommand = new SaveConfigCommand(this);
            HelpCommand = new HelpCommand(this);
        }

        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

#endregion


        /// <summary>
        /// Forwards on a message to the view to display in a message box
        /// </summary>
        /// <param name="header">The message header</param>
        /// <param name="message">The message body</param>
        public void MessageToView(string header, string message)
        {
            RasPiControllerWindow.DisplayMessage(header, message);
        }

        /// <summary>
        /// Tries to save the two ObservableCollections to the XML Config file at the location in the app.config file.
        /// </summary>
        /// <returns>True if successful, False if not.</returns>
        public bool SaveToConfiguration()
        {
            return ModelHelper.SaveConfiguration(RaspberryPis, Scripts);
        }

        /// <summary>
        /// Check that the Raspberry Pi Id does not already exist.
        /// </summary>
        /// <param name="id">The Id to check</param>
        /// <returns>False if it is not unique. True if it is.</returns>
        public bool CheckRaspberryPiIdIsUnique(string id)
        {
            return RaspberryPis.All(pi => pi.Id != id);
        }

        /// <summary>
        /// Check that the Script Id does not already exist.
        /// </summary>
        /// <param name="id">The Id to check</param>
        /// <returns>False if it is not unique. True if it is.</returns>
        public bool CheckScriptIdIsUnique(string id)
        {
            return Scripts.All(script => script.Id != id);
        }
    }
}
