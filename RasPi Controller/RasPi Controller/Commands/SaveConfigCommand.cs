using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;
using RasPi_Controller.Helpers;


namespace RasPi_Controller.Commands
{
    public class SaveConfigCommand : CommandBase
    {
        public SaveConfigCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return ModelHelper.ConfigFileExists();
        }

        public override void Execute(object parameter)
        {
            if (!ModelHelper.ConfigFileExists())
            {
                base_vm.MessageToView("Error", "The config file doesn't exist");
                return;
            }

            if (base_vm.SaveToConfiguration())
            {
                base_vm.MessageToView("Success", "Saved to the config file");
            }
            else
            {
                base_vm.MessageToView("Error", "Could not save to the config file");
            }
        }
    }
}
