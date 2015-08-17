using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.Models;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class NewScriptCommand : CommandBase
    {
        public NewScriptCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            base_vm.SelectedScript = new Script();
        }
    }
}
