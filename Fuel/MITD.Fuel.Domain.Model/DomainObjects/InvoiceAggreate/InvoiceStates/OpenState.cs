using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates
{
    public class OpenState : InvoiceState
    {
        private readonly IInvoiceDomainService invoiceDomainService;
        private readonly IInvoiceItemDomainService invoiceItemDomainService;
        private readonly IBalanceDomainService balanceDomainService;
        private readonly IInventoryOperationNotifier inventoryOperationNotifier;
        private readonly IInvoiceStateFactory invoiceStateFactory;
        private readonly IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService;
        private readonly IGoodUnitConvertorDomainService goodUnitConvertorDomainService;

        public OpenState(
            IInvoiceDomainService invoiceDomainService,
            IInvoiceItemDomainService invoiceItemDomainService,
            IBalanceDomainService balanceDomainService,
            IInventoryOperationNotifier inventoryOperationNotifier,
            IInvoiceStateFactory invoiceStateFactory,
            IInvoiceAdditionalPriceDomainService invoiceAdditionalPriceDomainService,
            IGoodUnitConvertorDomainService goodUnitConvertorDomainService)
        {
            this.invoiceDomainService = invoiceDomainService;
            this.invoiceItemDomainService = invoiceItemDomainService;
            this.balanceDomainService = balanceDomainService;
            this.inventoryOperationNotifier = inventoryOperationNotifier;
            this.invoiceStateFactory = invoiceStateFactory;
            this.invoiceAdditionalPriceDomainService = invoiceAdditionalPriceDomainService;
            this.goodUnitConvertorDomainService = goodUnitConvertorDomainService;
        }

        public override void ApproveInvoice(Invoice invoice, long approverId)
        {
            invoice.SetInvoiceStateType(this.invoiceStateFactory.CreateSubmitState());

            invoice.SubmitInvoice(
                this.invoiceDomainService,
                this.invoiceItemDomainService,
                this.balanceDomainService,
                this.inventoryOperationNotifier, 
                this.invoiceAdditionalPriceDomainService, 
                this.goodUnitConvertorDomainService,
                approverId);
        }
    }
}