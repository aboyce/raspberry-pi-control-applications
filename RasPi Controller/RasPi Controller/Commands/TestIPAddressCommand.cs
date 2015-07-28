using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class TestIPAddressCommand : CommandBase
    {
        public TestIPAddressCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            //TextRange content = new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd);
            //string pingResult = NetworkingHelper.TryToPing(content.Text);

            //if (pingResult == null)
            //{
            //    content.ApplyPropertyValue(ForegroundProperty, Brushes.Green);
            //}
            //else
            //{
            //    content.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
            //    MessageBox.Show(pingResult, "Error");
            //}
        }
    }
}
