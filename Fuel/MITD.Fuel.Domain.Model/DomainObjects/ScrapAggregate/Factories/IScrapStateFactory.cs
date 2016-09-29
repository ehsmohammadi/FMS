using MITD.Fuel.Domain.Model.DomainObjects.ScrapStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public interface IScrapStateFactory : IFactory
    {
        ScrapState CreateSubmittedState();
        ScrapState CreateSubmitRejectedState();
        ScrapState CreateOpenState();
        ScrapState CreateClosedState();
        ScrapState CreateCancelledState();

        ScrapState CreateState(States state);
    }
}