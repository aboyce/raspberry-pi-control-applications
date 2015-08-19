using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFC_Card_Reader.ViewModels;

namespace NFC_Card_Reader.Commands
{
    public class MonitorCommand : CommandBase
    {
        public MonitorCommand(ReaderViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return base_vm.CurrentReader != null && !base_vm.ReaderHelper.IsMonitoringCardReader(base_vm.CurrentReader.Name);
        }

        public override void Execute(object parameter)
        {
            string result = base_vm.ReaderHelper.StartMonitoringReader(base_vm.CurrentReader.Name);

            if (result == null)
                base_vm.MessageToView("Success", "Now monitoring " + base_vm.CurrentReader.Name);
            else
                base_vm.MessageToView("Error", string.Format("Unable to monitor {0}, Error: {1}", base_vm.CurrentReader.Name, result));
        }
    }
}
