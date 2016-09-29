using System;
using MITD.Fuel.Domain.Model.DomainObjects.OffhireStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public class OffhireStateFactory : IOffhireStateFactory
    {
        private readonly IApprovableOffhireDomainService approvableDomainService;

        public OffhireStateFactory(IApprovableOffhireDomainService approvableDomainService)
        {
            this.approvableDomainService = approvableDomainService;
        }

        public OffhireState CreateSubmitRejectedState()
        {
            return new SubmitRejectedState(this, approvableDomainService);
        }

        public OffhireState CreateSubmittedState()
        {
            return new SubmittedState(this, approvableDomainService);
        }

        public OffhireState CreateOpenState()
        {
            return new OpenState(this, approvableDomainService);
        }

        public OffhireState CreateClosedState()
        {
            return new ClosedState(this, approvableDomainService);
        }

        public OffhireState CreateCancelledState()
        {
            return new CancelledState(this, approvableDomainService);
        }

        public OffhireState CreateState(States state)
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