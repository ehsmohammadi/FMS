using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Domain.Model;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IInvoiceAdditionalPriceDomainService : IDomainService
    {
        Invoice CalculateAdditionalPrice(Invoice invoice);
        void DeleteInvoiceAdditionalPriceItem(InvoiceAdditionalPrice item);
    }
}