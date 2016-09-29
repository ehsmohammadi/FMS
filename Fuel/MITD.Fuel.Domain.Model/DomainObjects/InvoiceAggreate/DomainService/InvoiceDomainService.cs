#region

using System;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.DomainService
{

    public class InvoiceDomainService : IInvoiceDomainService
    {
        private readonly IInvoiceRepository invoiceRepository;

        public InvoiceDomainService(IInvoiceRepository invoiceRepository)
        {
            this.invoiceRepository = invoiceRepository;
        }

        public bool IsInvoiceNumberUniqueForCompnay(long Id, string invoiceNumber, long? supplier, long? transporter)
        {
            var uniq = invoiceRepository.Count
                (
                    c => (Id == 0 || c.Id != Id) &&
                        ((supplier == null || c.SupplierId == supplier) ||
                        (transporter == null || c.TransporterId == transporter))
                            && c.InvoiceNumber.ToLower() == invoiceNumber.ToLower()) == 0;
            return uniq;
        }

        public Invoice FindPairTransferPurchaseInvoice(Invoice comparingInvoice)
        {
            if(comparingInvoice.OrderRefrences == null || comparingInvoice.OrderRefrences.Count == 0 || comparingInvoice.OrderRefrences[0].OrderType != OrderTypes.PurchaseWithTransferOperations)
                throw new InvalidOperation("FindPairTransferPurchaseInvoice", "The given invoice is not registered for TransferPurchase orders.");
            
            InvoiceTypes findingInvoiceType;
            
            switch (comparingInvoice.InvoiceType)
            {
                case InvoiceTypes.Purchase:
                    findingInvoiceType = InvoiceTypes.PurchaseOperations;
                    break;
                case InvoiceTypes.PurchaseOperations:
                    findingInvoiceType = InvoiceTypes.Purchase;
                    break;
                case InvoiceTypes.Attach:
                default:
                    throw new InvalidArgument("The Selected Invoice type is not proper to find its pair.");
            }

            var foundInvoices = invoiceRepository.Find(i => i.State == States.Submitted && i.InvoiceType == findingInvoiceType && i.OwnerId == comparingInvoice.OwnerId);

            var matchedInvoicesByOrders = foundInvoices.Where(i => i.OrderRefrences.Count == comparingInvoice.OrderRefrences.Count &&
                
                i.OrderRefrences.TrueForAll(o => comparingInvoice.OrderRefrences.Exists(o2 => o2.Id == o.Id)) &&
                comparingInvoice.OrderRefrences.TrueForAll(o2=>i.OrderRefrences.Exists(o=>o.Id == o2.Id))
                ).ToList();

            if (matchedInvoicesByOrders.Count == 0) return null;

            if (matchedInvoicesByOrders.Count == 1)
            {
                if(!matchInvoiceItemsToBeEqualInGoodAndQuantity(matchedInvoicesByOrders.ToList()[0], comparingInvoice))
                    throw new BusinessRuleException("", "Matched Invoice with Id of " + matchedInvoicesByOrders.ToList()[0].Id + " has items other than selected Invoice.");

                return matchedInvoicesByOrders.ToList()[0];
            }
            else
            {
                var matchedInvoicesByItems = matchedInvoicesByOrders.Where(i => matchInvoiceItemsToBeEqualInGoodAndQuantity(i, comparingInvoice)).ToList();

                if(matchedInvoicesByItems.Count == 0) return null;

                if (matchedInvoicesByItems.Count > 1) 
                    throw new BusinessRuleException("", "More than 1 Invoice has been matched for selected Invoice.");

                return matchedInvoicesByItems[0];

            }
        }

        private bool matchInvoiceItemsToBeEqualInGoodAndQuantity(Invoice invoice1, Invoice invoice2)
        {
            if(invoice1 == null || invoice2 == null ||
                invoice1.InvoiceItems == null || invoice2.InvoiceItems == null) 
                return false;

            var compareResultOfInvoice1WithInvoice2 = invoice1.InvoiceItems.TrueForAll(i1 => invoice2.InvoiceItems.Exists(i2 => i2.GoodId == i1.GoodId && i2.Quantity == i1.Quantity && i2.MeasuringUnitId == i1.MeasuringUnitId));
            var compareResultOfInvoice2WithInvoice1 = invoice2.InvoiceItems.TrueForAll(i2 => invoice1.InvoiceItems.Exists(i1 => i1.GoodId == i2.GoodId && i1.Quantity == i2.Quantity && i1.MeasuringUnitId == i2.MeasuringUnitId));

            return compareResultOfInvoice1WithInvoice2 && compareResultOfInvoice2WithInvoice1;
        }
    }
}