using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class LoadConfigCommand : ICommand
    {
        private readonly MainWindowViewModel _vm;

        public LoadConfigCommand(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
            // TODO: Add some logic here
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {



            //_vm.EnableAll();
            //BtnLoadConfig.Content = "Re-Load Config";
            //BtnLoadConfig.Background = Brushes.FloralWhite;

            //LbxRasPis.ItemsSource = _vm.RaspberryPis;
            //LbxScripts.ItemsSource = _vm.Scripts;
        }
    }
}
