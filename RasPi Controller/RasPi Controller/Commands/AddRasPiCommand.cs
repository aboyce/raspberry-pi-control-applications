﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RasPi_Controller.Models;
using RasPi_Controller.ViewModels;


namespace RasPi_Controller.Commands
{
    public class AddRasPiCommand : CommandBase
    {
        public AddRasPiCommand(RasPiControllerWindowViewModel vm) : base(vm) { }

        public override bool CanExecute(object parameter)
        {
            return base_vm.SelectedRasPi != null;
        }

        public override void Execute(object parameter)
        {
            if (!base_vm.CheckRaspberryPiIdIsUnique(base_vm.SelectedRasPi.Id))
            {
                base_vm.MessageToView("Error", "The Id already exists");
                return;
            }





            base_vm.SelectedRasPi = new RaspberryPi();
            

            //if (base_vm.SelectedRasPi.NetworkName == string.Empty || base_vm.SelectedRasPi.IpAddress == string.Empty || base_vm.SelectedRasPi.Username == string.Empty)
            //{
            //    base_vm.MessageToView("Error", "Cannot save Raspberry Pi as there is missing content, please fill in relevant fields.");
            //}
            //else
            //{
            //    base_vm.RaspberryPis.Add(base_vm.SelectedRasPi);
            //    base_vm.MessageToView("Success", "Raspberry Pi " + base_vm.SelectedRasPi.Id + " was added to memory.");
            //}
        }
    }
}