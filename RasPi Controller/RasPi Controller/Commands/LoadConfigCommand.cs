using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class LoadConfigCommand : CommandBase
    {
        public LoadConfigCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            base_vm.LoadedFromConfig = true;

            //_vm.EnableAll(); **CANNOT DO YET
            //BtnLoadConfig.Background = Brushes.FloralWhite;

            //LbxRasPis.ItemsSource = _vm.RaspberryPis; **SHOULD BE AUTOMATIC AFTER INOTIFIED
            //LbxScripts.ItemsSource = _vm.Scripts; **SHOULD BE AUTOMATIC AFTER INOTIFIED
        }
    }
}
