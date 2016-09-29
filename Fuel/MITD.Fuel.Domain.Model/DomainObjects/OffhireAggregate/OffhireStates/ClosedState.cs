using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
namespace MITD.Fuel.Domain.Model.DomainObjects.OffhireStates
{
    public class ClosedState : OffhireState
    {
        public ClosedState(
            IOffhireStateFactory offhireStateFactory,
            IApprovableOffhireDomainService approvableDomainService)
            : base(offhireStateFactory, States.Closed, approvableDomainService)
        {

        }
    }
}