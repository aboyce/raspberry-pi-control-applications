using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class HelpCommand : CommandBase
    {
        public HelpCommand(RasPiControllerWindowViewModel vm) : base(vm)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            //_vm.EnableAll();
            //BtnLoadConfig.Content = "Re-Load Config";
            //BtnLoadConfig.Background = Brushes.FloralWhite;

            //LbxRasPis.ItemsSource = _vm.RaspberryPis;
            //LbxScripts.ItemsSource = _vm.Scripts;
        }
    }
}
