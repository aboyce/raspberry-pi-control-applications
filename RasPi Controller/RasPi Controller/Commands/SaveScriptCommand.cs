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
    public class SaveScriptCommand : CommandBase
    {
        public SaveScriptCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return false;
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();

            //TextRange idContent = new TextRange(TbxScriptId.Document.ContentStart, TbxScriptId.Document.ContentEnd);
            //string id = idContent.Text.Clean();
            //string name = TbxScriptName.Text;
            //string description = TbxScriptDescription.Text;
            //string argumentsFormat = TbxScriptArgumentFormat.Text;

            //if (!_vm.CheckScriptIdIsUnique(id))
            //{
            //    MessageBox.Show("The Id already exists", "Error");
            //    TextRange content = new TextRange(TbxScriptId.Document.ContentStart, TbxScriptId.Document.ContentEnd);
            //    content.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
            //    return;
            //}

            //if (name == string.Empty || description == string.Empty || argumentsFormat == string.Empty)
            //{
            //    MessageBox.Show("Cannot save Script as there is missing content, please fill in all relevant fields (including 'Arguments Format').", "Error");
            //}
            //else
            //{
            //    _vm.Scripts.Add(new Script { Id = id, Name = name, Description = description, ArgumentFormat = argumentsFormat });
            //    MessageBox.Show("Script Added", "Success");
            //}

            //LbxScripts.Items.Refresh();
        }
    }
}
