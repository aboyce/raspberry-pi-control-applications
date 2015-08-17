using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServerSimulator.ViewModels;

namespace WebServerSimulator.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected readonly WebServerSimulatorViewModel base_vm;

        protected CommandBase(WebServerSimulatorViewModel vm)
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
