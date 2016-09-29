using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.DomainObjects.OffhireStates;
using MITD.Fuel.Domain.Model.Enums;
using ClosedState = MITD.Fuel.Domain.Model.DomainObjects.OffhireStates.ClosedState;
using SubmitRejectedState = MITD.Fuel.Domain.Model.DomainObjects.OffhireStates.SubmitRejectedState;
using SubmittedState = MITD.Fuel.Domain.Model.DomainObjects.OffhireStates.SubmittedState;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IApprovableOffhireDomainService : IApprovableDomainService
    {
        void ValidateMiddleApprove(Offhire offhire);
        void Submit(Offhire offhire, SubmittedState submittedState, long approverId);
        void Cancel(Offhire offhire, CancelledState cancelledState, long approverId);
        void RejectSubmittedState(Offhire offhire, SubmitRejectedState submitRejectedState, long approverId);
        void Close(Offhire offhire, ClosedState closedState, long approverId);
    }
}