using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFC_Card_Reader.ViewModels;

namespace NFC_Card_Reader.Commands
{
    public class InitialiseCommand : CommandBase
    {
        public InitialiseCommand(ReaderViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            base_vm.CardReaders = base_vm.ReaderHelper.GetCardReaders();

            if(base_vm.CardReaders.Count == 0)
                base_vm.MessageToView("Error", "No card readers present");
        }

    }
}
