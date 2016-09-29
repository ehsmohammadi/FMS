using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.OffhireStates
{
    public class CancelledState : OffhireState
    {
        public CancelledState(
            IOffhireStateFactory offhireStateFactory,
            IApprovableOffhireDomainService approvableDomainService)
            : base(offhireStateFactory, States.Cancelled, approvableDomainService)
        {

        }
    }
}