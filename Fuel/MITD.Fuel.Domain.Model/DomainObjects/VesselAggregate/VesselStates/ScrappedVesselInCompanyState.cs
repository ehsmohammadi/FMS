using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates
{
    public class ScrappedVesselInCompanyState : VesselInCompanyState
    {
        public ScrappedVesselInCompanyState(IVesselInCompanyStateFactory vesselInCompanyStateFactory)
            : base(vesselInCompanyStateFactory, VesselStates.Scrapped)
        {

        }
    }
}