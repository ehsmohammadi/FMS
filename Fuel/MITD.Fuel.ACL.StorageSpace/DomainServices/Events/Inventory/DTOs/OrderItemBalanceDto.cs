using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public class OrderItemBalanceDto : DTOBase
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public long OrderId { get; set; }

        [DataMember]
        public long OrderItemId { get; set; }

        [DataMember]
        public decimal QuantityAmountInMainUnit { get; set; }

        [DataMember]
        public string UnitCode { get; set; }

        [DataMember]
        public long FuelReportDetailId { get; set; }

        [DataMember]
        public long InvoiceItemId { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }

    }
}
