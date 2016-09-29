using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Events;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Main.Presentation.Logic.SL.Infrastructure;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public static class BusyIndicatorExtensions
    {
        public static Guid ShowBusyIndicatorEx(this IFuelController fuelController, string message = null)
        {
            //var busyIndicator = (fuelController as FuelController).ViewManager.ShowInDialog<IFuelBusyIndicatorView>();

            MITD.Presentation.IDialogView instance1 = MITD.Core.ServiceLocator.Current.GetInstance<MITD.Presentation.IDialogView>();
            var busyIndicator = ServiceLocator.Current.GetInstance<IFuelBusyIndicatorView>();
            //Debug.Assert((object)instance2 != null);
            instance1.Title = busyIndicator.ViewModel.DisplayName;
            instance1.DialogContent = (object)busyIndicator;
            (instance1 as ChildWindow).BorderThickness = new Thickness(0);
            (instance1 as ChildWindow).HasCloseButton = false;
            (instance1 as ChildWindow).Show();
            
            (busyIndicator.ViewModel as FuelBusyIndicatorVM).BusyMessage = message;

            return (busyIndicator.ViewModel as FuelBusyIndicatorVM).Id;
        }

        public static void HideBusyIndicatorEx(this IFuelController fuelController, Guid guid)
        {
            (fuelController as FuelController).EventPublisher.Publish(new CloseBusyIndicatorArg(guid));
        }
    }
}
