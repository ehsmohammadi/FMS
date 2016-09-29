using System.Collections.Generic;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Fuel.Presentation.Contracts.SL.Views;
using MITD.Fuel.Presentation.Logic.SL.ViewModels;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Logic.SL.Controllers
{
    public class OrderItemController : BaseController, IOrderItemController
    {
        public OrderItemController(IViewManager viewManager, IEventPublisher eventPublisher, IDeploymentManagement deploymentManagement)
            : base(viewManager, eventPublisher, deploymentManagement)
        {
        }

        public void Add(OrderDto order)
        {
            var view = viewManager.ShowInDialog<IOrderItemView>();
            (view.ViewModel as OrderItemVM).SetProp(order);

        }

        public void Edit(OrderItemDto dto)
        {
            var view = viewManager.ShowInDialog<IOrderItemView>();
            (view.ViewModel as OrderItemVM).Load(dto);
        }

        public void ShowList()
        {
            var view = this.viewManager.ShowInTabControl<IOrderItemListView>();
            (view.ViewModel as OrderItemListVM).GetOrderItems();
        }

    }
}
