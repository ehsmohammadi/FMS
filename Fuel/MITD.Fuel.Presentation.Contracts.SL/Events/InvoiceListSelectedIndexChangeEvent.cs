using System;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Events
{
    public class InvoiceListSelectedIndexChangeEvent
    {
        public InvoiceDto Entity { get; set; }
        public Guid UniqId { get; set; }
    }
}