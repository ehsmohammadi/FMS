using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.CurrencyAndMeasurement.Domain.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class OrderItemBalance
    {
        public OrderItemBalance()
        {
        }

        public long Id { get; private set; }
        public long OrderId { get; private set; }
        public long OrderItemId { get; private set; }
        public virtual OrderItem OrderItem { get; private set; }
        public decimal QuantityAmountInMainUnit { get; private set; }

        public string UnitCode { get; private set; }

        public Quantity Quantity { get; set; }

        public long? FuelReportDetailId { get; private set; }
        public virtual FuelReportDetail FuelReportDetail { get; private set; }

        public long InvoiceItemId { get; private set; }
        public virtual InvoiceItem InvoiceItem { get; private set; }

        public long? PairingInvoiceItemId { get; private set; }
        public virtual InvoiceItem PairingInvoiceItem { get; private set; }


        //This will be used for retrieving current registered operations.
        public long? InventoryOperationId { get; private set; }
        public virtual InventoryOperation InventoryOperation { get; set; }

        public byte[] TimeStamp { get; private set; }

        public OrderItemBalance(OrderItem orderItem, InvoiceItem invoiceItem, FuelReportDetail fuelReportDetail, decimal amount, string unitCode, InvoiceItem pairingInvoiceItem)
        {
            OrderItem = orderItem;
            OrderId = orderItem.OrderId;
            InvoiceItem = invoiceItem;
            InvoiceItemId = invoiceItem.Id;
            FuelReportDetail = fuelReportDetail;
            FuelReportDetailId = fuelReportDetail == null ? null : (long?)fuelReportDetail.Id;
            PairingInvoiceItemId = pairingInvoiceItem == null ? null : (long?)pairingInvoiceItem.Id;
            PairingInvoiceItem = pairingInvoiceItem;

            this.UnitCode = unitCode;
            this.QuantityAmountInMainUnit = amount;
            //this.setQuantity(amount, unitCode);
        }
    }
}