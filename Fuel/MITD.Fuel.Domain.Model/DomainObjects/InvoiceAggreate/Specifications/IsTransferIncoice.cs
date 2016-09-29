#region

using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Specifications
{
    // Br_IN2,
    public class IsTransferIncoice : SpecificationBase<Invoice>
    {
        public IsTransferIncoice()
            : base(invoice => 
                invoice.InvoiceType == InvoiceTypes.PurchaseOperations) 
        {
        }
    }
}