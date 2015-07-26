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
        public SendCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return false;
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();

            // TODO: Get everything validated and then send command.

            // TODO: Check network name and ip address, if possible pick ip address.
            // TODO: Check username is valid.
            // TODO: See if passwords are required, if so check this, if not it should be fine.
            // TODO: Check the script name is valid.
            // TODO: Return the result.
        }
    }
}
