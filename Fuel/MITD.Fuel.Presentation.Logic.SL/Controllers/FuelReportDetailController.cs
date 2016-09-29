using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class FuelReportDetailController : BaseController, IFuelReportDetailController
    {
        public FuelReportDetailController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }
        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<IFuelReportDetailListView>();
            (view.ViewModel as FuelReportDetailListVM).RefreshData();
        }


        public void Update(FuelReportDetailDto dto)
        {
            var view = this.viewManager.ShowInDialog<IFuelReportDetailView>();
            (view.ViewModel as FuelReportDetailVM).Load(dto);
        }
    }
}
