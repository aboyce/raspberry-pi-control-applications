using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFC_Card_Reader.ViewModels;

namespace NFC_Card_Reader.Commands
{
    public class ReadCardCommand : CommandBase
    {
        public ReadCardCommand(ReaderViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return base_vm.CurrentReader != null && base_vm.ReaderHelper.IsMonitoringCardReader(base_vm.CurrentReader.Name);
        }

        public override void Execute(object parameter)
        {
            base_vm.MessageToView("Read Card", base_vm.ReaderHelper.ReadCard());
        }
    }
}
