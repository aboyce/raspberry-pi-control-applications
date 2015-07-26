using System;
using System.Windows.Input;
using RasPi_Controller.ViewModels;

namespace RasPi_Controller.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected readonly RasPiControllerWindowViewModel base_vm;

        protected CommandBase(RasPiControllerWindowViewModel vm)
        {
            base_vm = vm;
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public virtual event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
