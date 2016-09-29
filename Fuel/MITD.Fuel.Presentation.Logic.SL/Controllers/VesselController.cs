using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class VesselController : BaseController, IVesselController
    {
        public VesselController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<IVesselListView>();
            (view.ViewModel as VesselListVM).Load();
        }

        public void Add()
        {
            var view = this.viewManager.ShowInDialog<IVesselView>();
            (view.ViewModel as VesselVM).Load();
        }

        public void Edit(VesselDto _vesselDto)
        {
            var view = this.viewManager.ShowInDialog<IVesselView>();
            (view.ViewModel as VesselVM).Edit(_vesselDto);
        }

        public void ActivateVessel(VesselDto _vesselDto)
        {
            var view = this.viewManager.ShowInDialog<IVesselActivationView>();
            (view.ViewModel as VesselActivationVM).Load(_vesselDto);
        }
    }
}
