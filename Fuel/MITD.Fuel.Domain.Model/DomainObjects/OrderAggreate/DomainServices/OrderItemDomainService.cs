#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;

#endregion

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class OrderItemDomainService : IOrderItemDomainService
    {
        private readonly IRepository<OrderItem> orderItemRepository;

        public OrderItemDomainService(IRepository<OrderItem> orderItemRepository)
        {
            this.orderItemRepository = orderItemRepository;
        }

        #region IOrderDomainService Members

        public void DeleteOrderItem(OrderItem orderItem)
        {
            orderItemRepository.Delete(orderItem);
        }

       

        #endregion

    }
}