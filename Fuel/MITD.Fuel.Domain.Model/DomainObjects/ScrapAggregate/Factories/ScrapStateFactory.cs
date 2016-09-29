using System;
using MITD.Fuel.Domain.Model.DomainObjects.ScrapStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public class ScrapStateFactory : IScrapStateFactory
    {
        private readonly IApprovableScrapDomainService approvableDomainService;

        public ScrapStateFactory(IApprovableScrapDomainService approvableDomainService)
        {
            this.approvableDomainService = approvableDomainService;
        }

        public ScrapState CreateSubmitRejectedState()
        {
            return new SubmitRejectedState(this, approvableDomainService);
        }

        public ScrapState CreateSubmittedState()
        {
            return new SubmittedState(this, approvableDomainService);
        }

        public ScrapState CreateOpenState()
        {
            return new OpenState(this, approvableDomainService);
        }

        public ScrapState CreateClosedState()
        {
            return new ClosedState(this, approvableDomainService);
        }

        public ScrapState CreateCancelledState()
        {
            return new CancelledState(this, approvableDomainService);
        }

        public ScrapState CreateState(States state)
        {
            switch (state)
            {
                case States.Open:
                    return this.CreateOpenState();
                case States.Submitted:
                    return this.CreateSubmittedState();
                case States.SubmitRejected:
                    return this.CreateSubmitRejectedState();
                case States.Closed:
                    return this.CreateClosedState();
                case States.Cancelled:
                    return this.CreateCancelledState();
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}