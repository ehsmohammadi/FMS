using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.ScrapStates
{
    public class SubmittedState : ScrapState
    {
        public SubmittedState(
            IScrapStateFactory scrapStateFactory,
            IApprovableScrapDomainService approvableDomainService)
            : base(scrapStateFactory, States.Submitted, approvableDomainService)
        {
        }

        public override void Approve(Scrap scrap, long approverId)
        {
            ApprovableDomainService.Close(scrap, (ClosedState)this.ScrapStateFactory.CreateClosedState(), approverId);
        }

        public override void Reject(Scrap scrap, long approverId)
        {
            ApprovableDomainService.RejectSubmittedState(scrap, (SubmitRejectedState)this.ScrapStateFactory.CreateSubmitRejectedState(), approverId);
        }

        public override void Cancel(Scrap scrap, long approverId)
        {
            ApprovableDomainService.Cancel(scrap, (CancelledState)this.ScrapStateFactory.CreateCancelledState(), approverId);
        }
    }
}