using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class ScrapDetailController : BaseController, IScrapDetailController
    {
        public ScrapDetailController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {

        }
        //public void ShowList()
        //{
        //    var view = this.ViewManager.ShowInTabControl<IScrapDetailListView>();
        //    (view.ViewModel as ScrapDetailListVM).Load();
        //}

        public void Add()
        {
            var view = this.ViewManager.ShowInDialog<IScrapDetailView>();
            (view.ViewModel as ScrapDetailVM).Load();
        }

        public void Edit(ScrapDetailDto detailDto)
        {
            var view = this.ViewManager.ShowInDialog<IScrapDetailView>();
            (view.ViewModel as ScrapDetailVM).Edit(detailDto);
        }

        //public void Delete(ScrapDetailDto dto)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
