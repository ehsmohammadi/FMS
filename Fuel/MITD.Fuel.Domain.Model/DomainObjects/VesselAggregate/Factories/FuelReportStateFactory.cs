using System;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories
{
    public class VesselInCompanyStateFactory : IVesselInCompanyStateFactory
    {
        public VesselInCompanyState CreateInactiveVesselInCompanyState()
        {
            return new InactiveVesselInCompanyState(this);
        }

        public VesselInCompanyState CreateCharteredInVesselInCompanyState()
        {
            return new CharteredInVesselInCompanyState(this);
        }

        public VesselInCompanyState CreateCharteredOutVesselInCompanyState()
        {
            return new CharteredOutVesselInCompanyState(this);
        }

        public VesselInCompanyState CreateOwnedVesselInCompanyState()
        {
            return new OwnedVesselInCompanyState(this);
        }

        public VesselInCompanyState CreateScrappedVesselInCompanyState()
        {
            return new ScrappedVesselInCompanyState(this);
        }

        public VesselInCompanyState CreatState(VesselStates state)
        {
            switch (state)
            {
                case VesselStates.Inactive:
                    return CreateInactiveVesselInCompanyState();
                case VesselStates.CharterIn:
                    return CreateCharteredInVesselInCompanyState();
                case VesselStates.CharterOut:
                    return CreateCharteredOutVesselInCompanyState();
                case VesselStates.Owned:
                    return CreateOwnedVesselInCompanyState();
                case VesselStates.Scrapped:
                    return CreateScrappedVesselInCompanyState();
                default:
                    throw new ArgumentOutOfRangeException("Vessel State");
            }
        }
    }
}