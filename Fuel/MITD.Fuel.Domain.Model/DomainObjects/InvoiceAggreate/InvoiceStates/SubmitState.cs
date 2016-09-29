using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates
{
    public class SubmitState : InvoiceState
    {

        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
        private readonly IInvoiceStateFactory invoiceStateFactory;
        private readonly IInventoryOperationNotifier inventoryOperationNotifier;
        private IBalanceDomainService balanceDomainService;

        public SubmitState(IInventoryOperationDomainService inventoryOperationDomainService,
            IInvoiceStateFactory invoiceStateFactory, IInventoryOperationNotifier inventoryOperationNotifier, IBalanceDomainService balanceDomainService)
        {
            this.inventoryOperationDomainService = inventoryOperationDomainService;
            this.invoiceStateFactory = invoiceStateFactory;
            this.inventoryOperationNotifier = inventoryOperationNotifier;
            this.balanceDomainService = balanceDomainService;
        }

        public override void RejectInvoice(Invoice invoice, long approverId)
        {
            invoice.SetInvoiceStateType(invoiceStateFactory.CreateSubmitRejectedState());
            invoice.RejectSubmittedInvoice(this.inventoryOperationNotifier, this.balanceDomainService, approverId);
        }

        public override void CancelInvoice(Invoice invoice, long approverId)
        {
            invoice.SetInvoiceStateType(this.invoiceStateFactory.CreateCancelState());
            invoice.CancelInvoice(this.inventoryOperationNotifier,this.balanceDomainService , approverId);
        }

    }
}