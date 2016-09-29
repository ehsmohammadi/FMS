using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class CurrencyController : BaseController, ICurrencyController
    {
        public CurrencyController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void ShowExchangesList()
        {
            var view = this.viewManager.ShowInTabControl<ICurrencyExchangeListView>();
            (view.ViewModel as CurrencyExchangeListVM).Load();
        }
    }
}
