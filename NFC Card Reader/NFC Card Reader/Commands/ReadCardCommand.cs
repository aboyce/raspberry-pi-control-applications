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
            return false;
        }

        public override void Execute(object parameter)
        {
            //MessageBox.Show(reader.ReadCard(), "Read Card");
        }
    }
}
