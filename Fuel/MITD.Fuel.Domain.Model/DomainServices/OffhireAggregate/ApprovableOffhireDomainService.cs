using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OffhireStates;
using MITD.Fuel.Domain.Model.IDomainServices.Events.FinanceOperations;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public class ApprovableOffhireDomainService : IApprovableOffhireDomainService
    {
        private readonly IOffhireDomainService offhireDomainService;
        private readonly IFinanceNotifier eventNotifier;
        private readonly ITankDomainService tankDomainService;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IGoodUnitDomainService goodUnitDomainService;


        public ApprovableOffhireDomainService(
            IOffhireDomainService offhireDomainService, IFinanceNotifier eventNotifier,
            ITankDomainService tankDomainService, ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService, IGoodUnitDomainService goodUnitDomainService)
        {
            this.offhireDomainService = offhireDomainService;
            this.eventNotifier = eventNotifier;
            this.tankDomainService = tankDomainService;
            this.currencyDomainService = currencyDomainService;
            this.goodDomainService = goodDomainService;
            this.goodUnitDomainService = goodUnitDomainService;
        }

        public void ValidateMiddleApprove(Offhire offhire)
        {
            offhire.ValidateMiddleApprove(this.offhireDomainService, tankDomainService, currencyDomainService,
                goodDomainService, goodUnitDomainService);
        }

        public void Submit(Offhire offhire, SubmittedState submittedState, long approverId)
        {
            offhire.Submit(submittedState, this.offhireDomainService, eventNotifier,
                tankDomainService, currencyDomainService,
                goodDomainService, goodUnitDomainService, approverId);
        }

        public void Cancel(Offhire offhire, CancelledState cancelledState, long approverId)
        {
            offhire.Cancel(cancelledState, eventNotifier, approverId);
        }

        public void RejectSubmittedState(Offhire offhire, SubmitRejectedState submitRejectedState, long approverId)
        {
            offhire.RejectSubmittedState(submitRejectedState);
        }

        public void Close(Offhire offhire, ClosedState closedState, long approverId)
        {
            offhire.Close(closedState);
        }
    }
}