using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IOrderDomainService : IDomainService
    {

        IEnumerable<Order> GetFinalApprovedPrurchaseOrders(long companyId, long receiverVesselInCompanyId);

        IList<Order> GetSellerFinalApprovedPurchaseTransferOrders(long companyId);

        IEnumerable<Order> GetBuyerFinalApprovedPurchaseTransferOrders(long companyId);

        IList<Order> GetFinalApprovedInternalTransferOrders(long companyId);

        IEnumerable<Order> GetFinalApprovedTransferOrders(long companyId);

        void UpdateOrderItemsFromInvoiceItem(Invoice invoice);
      
    }
}
