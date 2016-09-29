using System;
using System.Windows;
using MITD.Core;
using MITD.Fuel.Presentation.FuelApp.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Logic.SL.Controllers;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.Controllers
{
    public class VoyageController : BaseController, IVoyageController
    {

        public VoyageController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void Show()
        {
            var view = this.viewManager.ShowInTabControl<IVoyageListView>();
            (view.ViewModel as VoyageListVM).Load();
        }
    }
}
