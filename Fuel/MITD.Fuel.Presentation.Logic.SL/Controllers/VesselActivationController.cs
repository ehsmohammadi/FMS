using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class VesselActivationController : BaseController, IVesselActivationController
    {
        public VesselActivationController(IViewManager viewManager, IEventPublisher eventPublisher,
            IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void AddVesselActivationItem(VesselDto toVessel, System.Action<VesselActivationItemDto> vesselActivationItemAdded)
        {
            var view = this.viewManager.ShowInDialog<IVesselActivationItemView>();
            (view.ViewModel as VesselActivationItemVM).Load(toVessel.OwnerId ,toVessel.Id, toVessel.Code, vesselActivationItemAdded);
        }

    }
}
