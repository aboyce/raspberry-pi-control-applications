using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RasPi_Controller.Helpers;
using RasPi_Controller.ViewModels;

namespace RasPi_Controller.Commands
{
    public class SendCommand : CommandBase
    {
        public SendCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            //TODO: Add checking if that be executed
            return true;
        }

        public override void Execute(object parameter)
        {
            if (base_vm.SelectedRasPi != null && base_vm.SelectedScript != null &&
                !string.IsNullOrEmpty(base_vm.SelectedRasPi.Username) && !string.IsNullOrEmpty(base_vm.SelectedScript.Name)) // Check we have what we need.
            {
                string pingResult = NetworkingHelper.TryToPing(base_vm.SelectedRasPi.IpAddress); // First try to ping the IP Address.
                if (pingResult != null)
                {
                    base_vm.MessageToView("Error", pingResult + ", trying Network Name");

                    pingResult = NetworkingHelper.TryToPing(base_vm.SelectedRasPi.NetworkName); // IP Address didn't work, try the Network Name.
                    if (pingResult != null)
                    {
                        base_vm.MessageToView("Error", pingResult + ", cannot reach network location"); // Neither IP Address of Network Name was reachable.
                        return;
                    }

                    Send(base_vm.SelectedRasPi.NetworkName); // Network Name is reachable.
                }
                else // IP Address is reachable.
                {
                    Send(base_vm.SelectedRasPi.IpAddress);
                }
            }
            else
            {
                base_vm.MessageToView("Error", "Please ensure you have selected a Raspberry Pi, Script and Username");
                return;
            }

            string password = SecureStringHelper.GetString(base_vm.Password);


            // TODO: Get everything validated and then send command.
            // TODO: See if passwords are required, if so check this, if not it should be fine.
            // TODO: Check the script name is valid.
            // TODO: Return the result.
        }

        private void Send(string networkLocation)
        {

        }
    }
}
