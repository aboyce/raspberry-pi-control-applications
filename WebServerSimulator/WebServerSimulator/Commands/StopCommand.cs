using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServerSimulator.ViewModels;

namespace WebServerSimulator.Commands
{
    public class StopCommand : CommandBase
    {
        public StopCommand(WebServerSimulatorViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return base_vm.Server.IsRunning;
        }

        public override void Execute(object parameter)
        {
            Task.Factory.StartNew((() => base_vm.Server.Stop()));
        }
    }
}
