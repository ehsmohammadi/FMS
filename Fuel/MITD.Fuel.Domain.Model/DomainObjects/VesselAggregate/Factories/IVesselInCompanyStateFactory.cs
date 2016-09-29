using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Factories;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories
{
    public interface IVesselInCompanyStateFactory : IFactory
    {
        VesselInCompanyState CreateInactiveVesselInCompanyState();
        VesselInCompanyState CreateCharteredInVesselInCompanyState();
        VesselInCompanyState CreateCharteredOutVesselInCompanyState();
        VesselInCompanyState CreateOwnedVesselInCompanyState();
        VesselInCompanyState CreateScrappedVesselInCompanyState();

        VesselInCompanyState CreatState(VesselStates state);
    }
}