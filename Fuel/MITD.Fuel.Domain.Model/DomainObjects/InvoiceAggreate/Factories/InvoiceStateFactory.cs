using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Factories
{
    public class InvoiceStateFactory : IInvoiceStateFactory
    {
        private readonly IInvoiceDomainService invoiceDomainService;
        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
        private readonly IInventoryOperationNotifier inventoryOperationNotifier;
        private readonly IInvoiceItemDomainService invoiceItemDomainService;
        private readonly IBalanceDomainService balanceDomainService;
        private readonly IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService;
        private readonly IGoodUnitConvertorDomainService goodUnitConvertorDomainService;

        public InvoiceStateFactory(
            IInvoiceDomainService invoiceDomainService,
            IInventoryOperationDomainService inventoryOperationDomainService, 
            IInventoryOperationNotifier inventoryOperationNotifier,
            IInvoiceItemDomainService invoiceItemDomainService,
            IBalanceDomainService balanceDomainService,
            IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService,
            IGoodUnitConvertorDomainService goodUnitConvertorDomainService
            )
        {
            this.invoiceDomainService = invoiceDomainService;
            this.inventoryOperationDomainService = inventoryOperationDomainService;
            this.inventoryOperationNotifier = inventoryOperationNotifier;
            this.invoiceItemDomainService = invoiceItemDomainService;
            this.balanceDomainService = balanceDomainService;
            this.invoiceAdditionalPriceDomainService = invoiceAdditionalPriceDomainService;
            this.goodUnitConvertorDomainService = goodUnitConvertorDomainService;
        }
        public InvoiceState CreateSubmitState()
        {
            return new SubmitState(inventoryOperationDomainService, this, inventoryOperationNotifier, balanceDomainService);
        }

        public InvoiceState CreateOpenState()
        {
            return new OpenState(
                this.invoiceDomainService,
                this.invoiceItemDomainService,
                this.balanceDomainService,
                this.inventoryOperationNotifier, this,
                this.invoiceAdditionalPriceDomainService,
                this.goodUnitConvertorDomainService);
        }

        public InvoiceState CreateCloseState()
        {
            return new CloseState();
        }

        public InvoiceState CreateCancelState()
        {
            return new CancelState();
        }

        public InvoiceState CreateSubmitRejectedState()
        {
            return new SubmitRejectedState(this.invoiceDomainService,
                this.invoiceItemDomainService,
                this.balanceDomainService,
                this.inventoryOperationNotifier, this,
                this.invoiceAdditionalPriceDomainService,
                this.goodUnitConvertorDomainService);
        }
    }
}