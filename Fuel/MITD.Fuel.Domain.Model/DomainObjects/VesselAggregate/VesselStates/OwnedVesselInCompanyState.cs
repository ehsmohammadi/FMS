using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class OwnedVesselInCompanyState : VesselInCompanyState
    {
        public OwnedVesselInCompanyState(IVesselInCompanyStateFactory vesselInCompanyStateFactory)
            : base(vesselInCompanyStateFactory, VesselStates.Owned)
        {

        }

        public override void StartCharterOut(VesselInCompany vesselInCompany)
        {
            vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.CharterOut));
        }

        public override void Deactivate(VesselInCompany vesselInCompany)
        {
            vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.Inactive));
        }

        public override void Scrap(VesselInCompany vesselInCompany)
        {
            vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.Scrapped));
        }
    }
}