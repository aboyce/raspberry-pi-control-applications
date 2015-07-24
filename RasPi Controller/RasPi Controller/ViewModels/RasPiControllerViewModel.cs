using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using RasPi_Controller.Models;
using RasPi_Controller.Helpers;
using System.Windows.Input;
using RasPi_Controller.Commands;
using System.ComponentModel;

namespace RasPi_Controller.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollection<RaspberryPi> RaspberryPis;
        public ObservableCollection<Script> Scripts;

        public ICommand LoadConfigCommand { get; set; }

        public string LoadButton { get; set; }

        public MainWindowViewModel()
        {
            // TODO: Check that they both are not null, and decide what to do if they are?
            RaspberryPis = ModelHelper.LoadRaspberryPisFromConfiguration();
            Scripts = ModelHelper.LoadScripsFromConfiguration();

            this.LoadButton = "Load Config";

            this.LoadConfigCommand = new LoadConfigCommand(this);

        }

 
        







        /// <summary>
        /// Tries to save the two ObservableCollections to the XML Config file at the location in the app.config file.
        /// </summary>
        /// <returns>True if successfull, False if not.</returns>
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
