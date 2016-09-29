using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.CharterStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories
{
    public interface ICharterStateFactory:IFactory
    {
        CharterState CreateSubmittedState();
        CharterState CreateSubmitRejectedState();
        CharterState CreateOpenState();
        CharterState CreateClosedState();
        CharterState CreateCancelledState();

        CharterState CreateState(States state);
    }
}
