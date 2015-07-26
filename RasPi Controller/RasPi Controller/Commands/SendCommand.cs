using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RasPi_Controller.ViewModels;

namespace RasPi_Controller.Commands
{
    public class SendCommand : CommandBase
    {
        public SendCommand(RasPiControllerWindowViewModel vm) : base(vm)
        {
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
