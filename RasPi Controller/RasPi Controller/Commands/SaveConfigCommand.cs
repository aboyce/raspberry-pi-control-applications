using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class SaveConfigCommand : CommandBase
    {
        public SaveConfigCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            //if (_vm.SaveToConfiguration())
            //    MessageBox.Show("Saved to Config File", "Success");
            //else
            //    MessageBox.Show("Could not save to config", "Error");
        }
    }
}
