using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.ScrapStates
{
    public class SubmitRejectedState : ScrapState
    {
        public SubmitRejectedState(
            IScrapStateFactory scrapStateFactory,
            IApprovableScrapDomainService approvableDomainService)
            : base(scrapStateFactory, States.SubmitRejected, approvableDomainService)
        {
        }

        public override void Approve(Scrap scrap, long approverId)
        {
            ApprovableDomainService.Submit(scrap, (SubmittedState)this.ScrapStateFactory.CreateSubmittedState(), approverId);
        }

        public override void Cancel(Scrap scrap, long approverId)
        {
            ApprovableDomainService.Cancel(scrap, (CancelledState)this.ScrapStateFactory.CreateCancelledState(), approverId);
        }
    }
}