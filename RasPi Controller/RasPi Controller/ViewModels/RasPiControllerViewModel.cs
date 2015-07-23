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

namespace RasPi_Controller.ViewModels
{
    class MainWindowViewModel
    {
        public ObservableCollection<RaspberryPi> RaspberryPis;
        public ObservableCollection<Script> Scripts;

        public MainWindowViewModel()
        {
            // TODO: Check that they both are not null, and decide what to do if they are?
            RaspberryPis = ModelHelper.LoadRaspberryPisFromConfiguration();
            Scripts = ModelHelper.LoadScripsFromConfiguration();
        }

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
