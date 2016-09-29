using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories
{
    public interface IFuelReportStateFactory : IFactory
    {
        FuelReportState CreateSubmitState();
        FuelReportState CreateOpenState();
        FuelReportState CreateClosedState();
        FuelReportState CreateInvalidState();
        FuelReportState CreateSubmitRejectedState();

        FuelReportState CreatState(States state);
    }
}