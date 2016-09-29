using MITD.Fuel.Domain.Model.DomainObjects.OffhireStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public interface IOffhireStateFactory : IFactory
    {
        OffhireState CreateSubmittedState();
        OffhireState CreateSubmitRejectedState();
        OffhireState CreateOpenState();
        OffhireState CreateClosedState();
        OffhireState CreateCancelledState();

        OffhireState CreateState(States state);
    }
}