using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NFC_Card_Reader.ViewModels;

namespace NFC_Card_Reader.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected readonly ReaderViewModel base_vm;

        protected CommandBase(ReaderViewModel vm)
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
