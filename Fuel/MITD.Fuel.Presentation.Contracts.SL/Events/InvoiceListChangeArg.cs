using System;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Events
{
    public class InvoiceListChangeArg
    {
        public InvoiceDto Entity { get; set; }
        public Guid UniqId { get; set; }
    }
}
