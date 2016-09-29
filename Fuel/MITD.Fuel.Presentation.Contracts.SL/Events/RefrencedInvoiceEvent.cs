using System;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Events
{
    public class RefrencedInvoiceEvent
    {
        public InvoiceDto ReferencedInvoice { get; set; }
        public Guid UniqId { get; set; }
    }
}