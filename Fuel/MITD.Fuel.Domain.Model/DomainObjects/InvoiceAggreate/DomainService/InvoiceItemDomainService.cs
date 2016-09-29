#region

using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.DomainService
{
    public class InvoiceItemDomainService : IInvoiceItemDomainService
    {
        private readonly IRepository<InvoiceItem> invoiceItemRepository;

        private readonly IInvoiceRepository invoiceRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IGoodDomainService goodDomainService;
        private readonly IGoodUnitConvertorDomainService goodUnitConvertorDomainService;


        public InvoiceItemDomainService(IInvoiceRepository invoiceRepository,
            IRepository<InvoiceItem> invoiceItemRepository,
            IOrderRepository orderRepository, IGoodDomainService goodDomainService, IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            this.invoiceRepository = invoiceRepository;
            this.orderRepository = orderRepository;
            this.goodDomainService = goodDomainService;
            this.goodUnitConvertorDomainService = goodUnitConvertorDomainService;
            this.invoiceItemRepository = invoiceItemRepository;

        }




        #region IInvoiceDomainService Members

        public void DeleteInvoiceItem(InvoiceItem invoiceItem)
        {
            invoiceItemRepository.Delete(invoiceItem);
        }

        public IEnumerable<Order> GetRefrencedOrders(List<long> orderList)
        {
            return orderRepository.Find(c => orderList.Contains(c.Id));
        }

        public IEnumerable<InvoiceItem> GenerateInvoiceItemFormInvoice(long invoiceRefrenceId)
        {
            var invoice = invoiceRepository.Single(c => c.Id == invoiceRefrenceId, new SingleResultFetchStrategy<Invoice>().Include(c => c.InvoiceItems));

            return invoice.InvoiceItems.Select(item => new InvoiceItem(item.Quantity, item.Fee, item.GoodId, item.MeasuringUnitId, "")).ToList();
        }



        #endregion
    }
}