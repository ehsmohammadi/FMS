using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class ScrapController : BaseController, IScrapController
    {
        public ScrapController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }

        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<IScrapListView>();
            (view.ViewModel as ScrapListVM).Load();
        }

        public void Add()
        {
            var view = this.viewManager.ShowInDialog<IScrapView>();
            (view.ViewModel as ScrapVM).Load();
        }

        public void Edit(ScrapDto scrapDto)
        {

            var view = this.viewManager.ShowInDialog<IScrapView>();
            (view.ViewModel as ScrapVM).Edit(scrapDto);
        }

        public void AddScrapDetail(ScrapDto scrapDto)
        {
            var view = this.viewManager.ShowInDialog<IScrapDetailView>();
            (view.ViewModel as ScrapDetailVM).Load(scrapDto);

        }

        public void EditScrapDetail(ScrapDto scrapDto, ScrapDetailDto scrapDetailDto)
        {
            var view = this.viewManager.ShowInDialog<IScrapDetailView>();
            (view.ViewModel as ScrapDetailVM).Edit(scrapDto, scrapDetailDto);
        }
    }
}
