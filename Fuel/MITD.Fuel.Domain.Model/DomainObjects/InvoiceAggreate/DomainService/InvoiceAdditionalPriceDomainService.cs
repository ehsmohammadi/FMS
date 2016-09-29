using System;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.DomainService
{
    public class InvoiceAdditionalPriceDomainService : IInvoiceAdditionalPriceDomainService
    {
        private readonly IRepository<InvoiceAdditionalPrice> additionalPriceRepository;

        public InvoiceAdditionalPriceDomainService(IRepository<InvoiceAdditionalPrice> additionalPriceRepository)
        {
            this.additionalPriceRepository = additionalPriceRepository;
        }

        public Invoice CalculateAdditionalPrice(Invoice invoice)
        {

            invoice.TotalOfDivisionPrice = GetSumOfDivisionablePrice(invoice);
            switch (invoice.DivisionMethod)
            {
                case DivisionMethods.WithAmount:
                    return CalculateAdditionalWithAmount(invoice);
                case DivisionMethods.WithPrice:
                    return CalculateAdditionalWithPrice(invoice);
                case DivisionMethods.Direct:
                    return CalculateAdditionalWithDirect(invoice);
                case DivisionMethods.None:

                    invoice.InvoiceItems.ForEach(ii=>ii.SetDivisionPrice(0));
                    invoice.TotalOfDivisionPrice = 0;

                    return invoice;
                default:
                    return invoice;

            }
        }

        private Invoice CalculateAdditionalWithDirect(Invoice invoice)
        {
            //             invoice.TotalOfDivisionPrice = invoice.InvoiceItems.Sum(c => c.DivisionPrice);
            return invoice;
        }

        public void DeleteInvoiceAdditionalPriceItem(InvoiceAdditionalPrice item)
        {
            additionalPriceRepository.Delete(item);

        }

        private static Invoice CalculateAdditionalWithPrice(Invoice invoice)
        {
            var sumOfDivisionableEffectiveFactors = GetSumOfDivisionableEffectiveFactors(invoice);
            var sumOfAllInvoiceItemPrice = invoice.InvoiceItems.Sum(c => c.Fee * c.Quantity);
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                var d = sumOfDivisionableEffectiveFactors * (invoiceItem.Fee * invoiceItem.Quantity) / sumOfAllInvoiceItemPrice;
                invoiceItem.SetDivisionPrice(d);
            }
            return invoice;
        }

        private Invoice CalculateAdditionalWithAmount(Invoice invoice)
        {
            var sumOfDivisionableEffectiveFactors = GetSumOfDivisionableEffectiveFactors(invoice);
            var sumOfAllInvoiceItemAmount = invoice.InvoiceItems.Sum(c => c.Quantity);
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                var d = sumOfDivisionableEffectiveFactors * invoiceItem.Quantity / sumOfAllInvoiceItemAmount;
                invoiceItem.SetDivisionPrice(d);
            }
            return invoice;
        }


        private static decimal GetSumOfDivisionableEffectiveFactors(Invoice invoice)
        {
            var result = invoice.AdditionalPrices.Where(c => c.Divisionable).Sum(c => c.Price * (int)c.EffectiveFactor.EffectiveFactorType);
            result = result * (invoice.IsCreditor ? -1 : 1);
            return result;
        }
        private static decimal GetSumOfDivisionablePrice(Invoice invoice)
        {
            var result = invoice.AdditionalPrices.Sum(c => c.Price * (int)c.EffectiveFactor.EffectiveFactorType);
            result = result * (invoice.IsCreditor ? -1 : 1);
            return result;
        }
    }
}