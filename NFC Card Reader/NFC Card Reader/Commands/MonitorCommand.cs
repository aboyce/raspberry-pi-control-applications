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
            return base_vm.CardReaders != null;
        }

        public override void Execute(object parameter)
        {
            //var selected = LbxReaders.SelectedItem;
            //if (selected == null) return;

            //bool result = _rvm.MonitorReader(selected.ToString());

            //if (result)
            //{
            //    MessageBox.Show(string.Format("Now monitoring {0}", selected.ToString()));
            //    _monitoring();
            //}
            //else
            //    MessageBox.Show("Cannot monitor reader");
        }
    }
}
