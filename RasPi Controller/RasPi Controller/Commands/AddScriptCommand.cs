using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class AddScriptCommand : CommandBase
    {
        public AddScriptCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return base_vm.SelectedScript != null;
        }

        public override void Execute(object parameter)
        {
            if (string.IsNullOrEmpty(base_vm.SelectedScript.Id) || string.IsNullOrEmpty(base_vm.SelectedScript.Name)
                || string.IsNullOrEmpty(base_vm.SelectedScript.ArgumentFormat) || string.IsNullOrEmpty(base_vm.SelectedScript.Description))
            {
                base_vm.MessageToView("Error", "Cannot save Script as there is missing content, please fill in relevant fields.");
                return;
            }
            else
            {
                if (!base_vm.CheckScriptIdIsUnique(base_vm.SelectedScript.Id))
                {
                    base_vm.MessageToView("Error", "The Id already exists");
                    return;
                }
                else
                {
                    base_vm.Scripts.Add(base_vm.SelectedScript);
                    base_vm.MessageToView("Success", "Script " + base_vm.SelectedScript.Id + " was added to memory.");
                }
            }
        }
    }
}
