using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterStates;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories
{
    public class CharterStateFactory : ICharterStateFactory
    {
        public CharterState CreateSubmittedState()
        {
            return new SubmittedState(this);
        }

        public CharterState CreateSubmitRejectedState()
        {
            return new SubmitRejectedState(this);
        }

        public CharterState CreateOpenState()
        {
            return new OpenState(this);
        }

        public CharterState CreateClosedState()
        {
            return null;
        }

        public CharterState CreateCancelledState()
        {
            return null;
        }

        public CharterState CreateState(States state)
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
