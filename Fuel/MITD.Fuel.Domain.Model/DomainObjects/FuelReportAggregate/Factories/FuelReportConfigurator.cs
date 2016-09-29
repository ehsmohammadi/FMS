using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories
{
    public class FuelReportConfigurator : IEntityConfigurator<FuelReport>
    {
        private readonly IFuelReportStateFactory fuelReportStateFactory;
        private readonly IVesselInInventoryRepository vesselInInventoryRepository;
        private readonly IVesselInCompanyStateFactory vesselInCompanyStateFactory;

        public FuelReportConfigurator(IFuelReportStateFactory fuelReportStateFactory, IVesselInInventoryRepository vesselInInventoryRepository, IVesselInCompanyStateFactory vesselInCompanyStateFactory)
        {
            this.fuelReportStateFactory = fuelReportStateFactory;
            this.vesselInInventoryRepository = vesselInInventoryRepository;
            this.vesselInCompanyStateFactory = vesselInCompanyStateFactory;
        }

        public FuelReport Configure(FuelReport entity)
        {
            if (entity != null)
            {
                entity.Configure(fuelReportStateFactory);
            
                var vesselInInvenotry = vesselInInventoryRepository.First(v => v.Code == entity.VesselInCompany.Code && v.CompanyId == entity.VesselInCompany.CompanyId);
                entity.VesselInCompany.Configure(vesselInInvenotry, vesselInCompanyStateFactory);
            }

            return entity;
        }
    }
}
