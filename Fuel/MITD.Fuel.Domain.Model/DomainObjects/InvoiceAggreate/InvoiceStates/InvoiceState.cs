using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates
{
    public abstract class InvoiceState
    {
        public virtual void ApproveInvoice(Invoice invoice, long approverId)
        {
            throw new InvalidStateException("Approve",string.Format("Cannot Approve {0} State",invoice.State.ToString()));
        }
        public virtual void RejectInvoice(Invoice invoice, long approverId)
        {
            throw new InvalidStateException("Reject", string.Format("Cannot Reject {0} State", invoice.State.ToString()));
        }
        public virtual void CancelInvoice(Invoice invoice, long approverId)
        {
            throw new InvalidStateException("Cancel", string.Format("Cannot Cancel {0} State", invoice.State.ToString()));
        }
    }
}