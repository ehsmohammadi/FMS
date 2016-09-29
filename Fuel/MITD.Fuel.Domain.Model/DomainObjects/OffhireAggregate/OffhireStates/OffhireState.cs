using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.OffhireStates
{
    public abstract class OffhireState
    {
        public States State { get; private set; }

        protected readonly IOffhireStateFactory OffhireStateFactory;
        protected readonly IApprovableOffhireDomainService ApprovableDomainService;

        protected OffhireState(IOffhireStateFactory offhireStateFactory, States state, IApprovableOffhireDomainService approvableDomainService)
        {
            this.OffhireStateFactory = offhireStateFactory;
            this.State = state;
            this.ApprovableDomainService = approvableDomainService;
        }

        public virtual void Approve(Offhire offhire, long approverId /*, IApprovableFuelReportDomainService approveService*/)
        {
            throw new InvalidStateException("Approve", string.Format("Cannot Approve {0} State", offhire.State.ToString()));
        }

        public virtual void Reject(Offhire offhire, long approverId)
        {
            throw new InvalidStateException("Reject", string.Format("Cannot Reject {0} State", offhire.State.ToString()));
        }

        public virtual void Cancel(Offhire offhire, long approverId)
        {
            throw new InvalidStateException("Cancel", string.Format("Cannot Cancel {0} State", offhire.State.ToString()));
        }
    }
}