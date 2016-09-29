using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    //public interface IInvoiceDomainService3 : IDomainService
    //{
    //    bool OrderHaveAnyInvoices(long order);
    //}

    public interface IInvoiceDomainService : IDomainService
    {
        bool IsInvoiceNumberUniqueForCompnay(long Id, string invoiceNumber, long? supplier, long? transporter);

        Invoice FindPairTransferPurchaseInvoice(Invoice comparingInvoice);

    }
}
