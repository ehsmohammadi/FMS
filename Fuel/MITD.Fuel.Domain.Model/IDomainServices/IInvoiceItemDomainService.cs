#region

using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IInvoiceItemDomainService:IDomainService
    {

        void DeleteInvoiceItem(InvoiceItem invoiceItem);

//        IEnumerable<OrderItem> GetRefrencedOrderItems(List<long> orderList);

        //IEnumerable<Order> GetRefrencedOrders(List<long> toList);
    }

}