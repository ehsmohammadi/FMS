using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class CharteredInVesselInCompanyState : VesselInCompanyState
    {
        public CharteredInVesselInCompanyState(IVesselInCompanyStateFactory vesselInCompanyStateFactory)
            : base(vesselInCompanyStateFactory, VesselStates.CharterIn)
        {

        }

        public override void StartCharterOut(VesselInCompany vesselInCompany)
        {
            vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.CharterOut));
        }

        public override void EndCharterIn(VesselInCompany vesselInCompany)
        {
            vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.Inactive));
        }
    }
}