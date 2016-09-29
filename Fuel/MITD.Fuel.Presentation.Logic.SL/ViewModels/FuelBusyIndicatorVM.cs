using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Core;

namespace MITD.Fuel.Presentation.Logic.SL.ViewModels
{
    public class FuelBusyIndicatorVM : WorkspaceViewModel, IEventHandler<CloseBusyIndicatorArg>
    {
        IFuelController mainController;

        private string busyMessage;
        public new string BusyMessage
        {
            get { return busyMessage; }
            set { this.SetField(p => p.BusyMessage, ref busyMessage, value); }
        }

        //private bool isBusy;
        //public new bool IsBusy
        //{
        //    get { return isBusy; }
        //    set { this.SetField(p => p.IsBusy, ref isBusy, value); }
        //}

        public FuelBusyIndicatorVM(IFuelController mainController)
        {
            this.mainController = mainController;
            BusyMessage = "در حال بارگذاری...";

            //IsBusy = true;
        }


        public void Handle(CloseBusyIndicatorArg eventData)
        {
            if(eventData.Guid == this.Id)
                mainController.Close(this);
        }
    }
}
