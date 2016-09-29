using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.ScrapStates
{
    public abstract class ScrapState
    {
        public States State { get; private set; }

        protected readonly IScrapStateFactory ScrapStateFactory;
        protected readonly IApprovableScrapDomainService ApprovableDomainService;

        protected ScrapState(IScrapStateFactory scrapStateFactory, States state, IApprovableScrapDomainService approvableDomainService)
        {
            this.ScrapStateFactory = scrapStateFactory;
            this.State = state;
            this.ApprovableDomainService = approvableDomainService;
        }

        public virtual void Approve(Scrap scrap, long approverId /*, IApprovableFuelReportDomainService approveService*/)
        {
            throw new InvalidStateException("Approve", string.Format("Cannot Approve {0} State", scrap.State.ToString()));
        }

        public virtual void Reject(Scrap scrap, long approverId)
        {
            throw new InvalidStateException("Reject", string.Format("Cannot Reject {0} State", scrap.State.ToString()));
        }

        public virtual void Cancel(Scrap scrap, long approverId)
        {
            throw new InvalidStateException("Cancel", string.Format("Cannot Cancel {0} State", scrap.State.ToString()));
        }
    }
}