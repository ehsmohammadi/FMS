using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.InvoiceStates;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IApprovableInvoiceDomainService : IApprovableDomainService
    {
        void ValidateMiddleApprove(Invoice invoice);
        void Submit(Invoice invoice, SubmitState submittedState);
        void Cancel(Invoice invoice, CancelState cancelledState);
        //        void RejectSubmittedState(Invoice invoice, SubmitRejectedState submitRejectedState);
//        void Close(Invoice invoice, ClosedState closedState);
    }
}