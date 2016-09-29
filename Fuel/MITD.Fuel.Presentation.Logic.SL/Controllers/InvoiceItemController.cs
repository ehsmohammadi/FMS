using System.Collections.Generic;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class InvoiceItemController :BaseController, IInvoiceItemController
    {
        public InvoiceItemController(IViewManager _viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement) : 
            base(_viewManager, eventPublisher, deploymentManagement)
        {
        }

      

//        public void Edit(InvoiceItemDto dto, DivisionMethodEnum divisionMethod)
//        {
//            var view = _viewManager.ShowInDialog<IInvoiceItemView>();
//           (view.ViewModel as InvoiceItemVM).Load(dto,divisionMethod, TODO);
//        }

//        public void ShowList()
//        {
//          
//            var view =this._viewManager.ShowInTabControl<IInvoiceItemListView>();
//           
//              
//                  (view.ViewModel as InvoiceItemListVM).Load();
//           
//        }

    }
}
