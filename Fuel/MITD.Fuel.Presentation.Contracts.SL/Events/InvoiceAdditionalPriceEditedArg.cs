using System;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.Events
{
    public class InvoiceAdditionalPriceEditedArg
    {
        public InvoiceAdditionalPriceDto InvoiceAdditionalPrice { get; set; }
        public Guid UniqId { get; set; }
    }
}