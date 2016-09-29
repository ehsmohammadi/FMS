using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.IDomainServices.Events.FinanceOperations;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events
{
    public class FinanceNotifier : IFinanceNotifier
    {
        private readonly IAddOffhireVoucher addOffhireVoucher;

        public FinanceNotifier(IAddOffhireVoucher addOffhireVoucher)
        {
            this.addOffhireVoucher = addOffhireVoucher;
        }

        public void NotifySubmittingOffhire(Offhire offhire, long approverId,VoucherDetailType voucherDetailType)
        {
            addOffhireVoucher.Execute(offhire, approverId, voucherDetailType);
        }

        public void NotifyOffhireCancelled(Offhire offhire, long approverId)
        {

        }
    }
}
