using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IBalanceDomainService : IDomainService
    {
        void DeleteInvoiceItemRefrencesFromBalance(long id);

        IEnumerable<InvoiceItem> GenerateInvoiceItemFromOrders(List<long> orderList);
        List<OrderItemBalance> CreateBalanceRecordForInvoiceItem(InvoiceItem invoiceItem, List<Order> orderRefrences, InvoiceItem pairingInvoiceItem);
        void SetReceivedData(long orderId, long fuelReportDetailId, long goodId, long unitId, decimal receivedQuantity);
        void SetTransferData(long orderId, long fuelReportDetailId, long goodId, long unitId, decimal transferedQuantity);

        void RemoveOperatedQuantity(long fuelReportDetailId);

        bool DoesFuelReportDetailHaveInvoices(long fuelReportDetailId);
    }
}