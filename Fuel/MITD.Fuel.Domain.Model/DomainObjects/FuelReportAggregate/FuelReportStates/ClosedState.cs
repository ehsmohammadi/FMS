using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class ClosedState : FuelReportState
    {
        public ClosedState(IFuelReportStateFactory fuelReportStateFactory)
            : base(fuelReportStateFactory, States.Closed)
        {

        }
    }
}