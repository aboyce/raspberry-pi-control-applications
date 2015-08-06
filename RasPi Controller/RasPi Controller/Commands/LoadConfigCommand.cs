using RasPi_Controller.Helpers;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class LoadConfigCommand : CommandBase
    {
        public LoadConfigCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return ModelHelper.ConfigFileExists();
        }

        public override void Execute(object parameter)
        {
            base_vm.LoadedFromConfig = true;
            base_vm.RaspberryPis = ModelHelper.LoadRaspberryPisFromConfiguration();
            base_vm.Scripts = ModelHelper.LoadScripsFromConfiguration();
            base_vm.MessageToView("Loaded", "Loaded in config");
        }
    }
}
