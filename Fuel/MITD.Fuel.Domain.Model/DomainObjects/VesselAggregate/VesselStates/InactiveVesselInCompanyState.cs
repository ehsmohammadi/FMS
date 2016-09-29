using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class InactiveVesselInCompanyState : VesselInCompanyState
    {
        public InactiveVesselInCompanyState(IVesselInCompanyStateFactory vesselInCompanyStateFactory)
            : base(vesselInCompanyStateFactory, VesselStates.Inactive)
        {

        }

        public override void Activate(VesselInCompany vesselInCompany)
        {
            if (vesselInCompany.CompanyId == vesselInCompany.Vessel.OwnerId)
            {
                vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.Owned));
            }
            else
            {
                throw new InvalidStateException("Activate", string.Format("Cannot activate the vessel."));

            }
        }

        public override void StartCharterIn(VesselInCompany vesselInCompany)
        {
            {
                if (vesselInCompany.CompanyId != vesselInCompany.Vessel.OwnerId)
                {
                    vesselInCompany.SetState(this.VesselInCompanyStateFactory.CreatState(VesselStates.CharterIn));
                }
                else
                {
                    throw new InvalidStateException("StartCharterIn", string.Format("Cannot start the Charter In of the vessel."));
                }
            }
        }
    }
}