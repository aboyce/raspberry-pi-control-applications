using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class SaveRasPiCommand : CommandBase
    {
        public SaveRasPiCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return false;
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();

            //TextRange idContent = new TextRange(TbxRasPiId.Document.ContentStart, TbxRasPiId.Document.ContentEnd);
            //string id = idContent.Text.Clean();
            //TextRange networkNameContent = new TextRange(TbxRasPiNetworkName.Document.ContentStart, TbxRasPiNetworkName.Document.ContentEnd);
            //string networkName = networkNameContent.Text.Clean();
            //TextRange ipAddressContent = new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd);
            //string ipAddress = ipAddressContent.Text.Clean();
            //string username = TbxRasPiUsername.Text;

            //if (!_vm.CheckRaspberryPiIdIsUnique(id))
            //{
            //    MessageBox.Show("The Id already exists", "Error");
            //    TextRange content = new TextRange(TbxRasPiId.Document.ContentStart, TbxRasPiId.Document.ContentEnd);
            //    content.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
            //    return;
            //}

            //if (networkName == string.Empty || ipAddress == string.Empty || username == string.Empty)
            //{
            //    MessageBox.Show("Cannot save Raspberry Pi as there is missing content, please fill in relevant fields.", "Error");
            //}
            //else
            //{
            //    _vm.RaspberryPis.Add(new RaspberryPi { Id = id, NetworkName = networkName, IpAddress = ipAddress, Username = username });
            //    MessageBox.Show("Raspberry Pi Added", "Success");
            //}

            //LbxRasPis.Items.Refresh();
        }
    }
}
