﻿using RasPi_Controller.Helpers;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class TestIpAddressCommand : CommandBase
    {
        public TestIpAddressCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            // TODO: Get this on a separate thread.
            string pingResult = NetworkingHelper.TryToPing(base_vm.SelectedRasPi.IpAddress);

            if (pingResult == null)
            {
                base_vm.MessageToView("Success", "Ping succeeded");
            }
            else
            {
                base_vm.MessageToView("Error", pingResult);
            }
        }
    }
}