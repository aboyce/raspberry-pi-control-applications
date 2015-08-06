using System;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class HelpCommand : CommandBase
    {
        public HelpCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            base_vm.MessageToView("Help", (string.Format("This application is indented to send SSH commands to a remote location. {0}{0}" +
                                          "The minimal requirements are; {0}" +
                                          " - Network Name or IP Address {0}" +
                                          " - Username/Password and Script Name/SSH Command. {0}{0}" +
                                          "The 'Test' buttons will ping the relevant field, and return either green or red with error. {0}{0}" +
                                          "The arguments can be left blank, but if present, will be added to the end of the script/command. {0}{0}" +
                                          "'Arguments Format' and 'Script Description' are both just for information but may be useful to other users. {0}{0}" +
                                          "You can store both Raspberry Pi (remote machine) and Script information in the XML file that is referenced in the .config file, this is to save retyping the same data. {0}" +
                                          "[Ensure that the XML file follows the examples formatting, and is valid XML] {0}{0}" +
                                          "Any queries contact via https://github.com/aboyce"
                                          , Environment.NewLine)));
        }
    }
}
