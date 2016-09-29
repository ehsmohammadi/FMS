using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class CharteredOutVesselInCompanyState : VesselInCompanyState
    {
        public CharteredOutVesselInCompanyState(IVesselInCompanyStateFactory vesselInCompanyStateFactory)
            : base(vesselInCompanyStateFactory, VesselStates.CharterOut)
        {

        }

        public override void EndCharterOut(VesselInCompany vesselInCompany)
        {
            if (vesselInCompany.CompanyId == vesselInCompany.Vessel.OwnerId)
            {
                vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.Owned));
            }
            else
            {
                vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.CharterIn));
            }
        }
    }
}