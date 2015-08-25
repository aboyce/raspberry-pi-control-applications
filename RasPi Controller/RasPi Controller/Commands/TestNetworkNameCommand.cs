using System;
using System.Threading.Tasks;
using BYC.Helpers;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class TestNetworkNameCommand : CommandBase
    {
        public TestNetworkNameCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Task.Run(() =>
            {
                string pingResult = NetworkingHelper.TryToPing(base_vm.SelectedRasPi.NetworkName);

                if (pingResult == null)
                {
                    base_vm.MessageToView("Success", "Ping succeeded");
                }
                else
                {
                    base_vm.MessageToView("Error", pingResult);
                }
            });
        }
    }
}
