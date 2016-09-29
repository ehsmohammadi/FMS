using MITD.Core;
using MITD.Presentation;
//using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Fuel.Presentation.Contracts.DTOs;
//using MITD.Fuel.Presentation.Contracts.SL.Views;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    // todo: this class must be moved to framework 
    public class BaseController 
    {
        #region props

        protected IViewManager viewManager { get; set; }
        protected IEventPublisher EventPublisher { get; set; }
        protected IDeploymentManagement DeploymentManagement { get; set; }
        #endregion

        public BaseController(IViewManager viewManager,
                              IEventPublisher eventPublisher,
                              IDeploymentManagement deploymentManagement)
        {
            this.viewManager = viewManager;
            this.EventPublisher = eventPublisher;
            this.DeploymentManagement = deploymentManagement;
        }

         
    }
}