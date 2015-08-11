using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            if (string.IsNullOrEmpty(base_vm.Arguments)) // Check that we are ok to continue without arguments.
            {
                if (!base_vm.YesNoToView("Warning", "Do you want to send the command with no arguments?"))
                    return;
            }

            if (base_vm.SelectedRasPi != null && base_vm.SelectedScript != null &&
                !string.IsNullOrEmpty(base_vm.SelectedRasPi.Username) && !string.IsNullOrEmpty(base_vm.SelectedScript.Name)) // Check we have what we need from the UI.
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
        }

        private void Send(string networkLocation)
        {
            string password = SecureStringHelper.GetString(base_vm.Password); // Checked in here to keep the unsecured password in memory as little as possible.

            if (string.IsNullOrEmpty(password))
            {
                base_vm.MessageToView("Error", "You have not entered a password");
                return;
            }

            string response = SSHControllerHelper.SendCommand(networkLocation, base_vm.SelectedRasPi.Username, password,
                base_vm.SelectedScript.Name, base_vm.Arguments);

            if (response.Length > 1)
            {
                string result = response.Substring(0, 1);
                response = response.Substring(1, response.Length - 1);

                switch (result)
                {
                    case "P":
                        base_vm.MessageToView("Success", response);
                        return;
                    case "F":
                        base_vm.MessageToView("Error", response);
                        return;
                    default:
                        base_vm.MessageToView("Error", "Logic Error [SendCommand - default switch]");
                        return;
                }
            }
            else if (response.Length == 1)
            {
                base_vm.MessageToView("Error", string.Format("No response from {0}, please check the details are correct.", networkLocation));
                return;
            }

            base_vm.MessageToView("Error", "Logic Error [SendCommand - unexpected outcome]");
        }
    }
}
