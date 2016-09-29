using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.VesselInCompanyAggregate.Factories
{
    public class VesselInCompanyConfigurator : IEntityConfigurator<VesselInCompany>
    {
        private readonly IVesselInInventoryRepository vesselInInventoryRepository;
        private readonly IVesselInCompanyStateFactory vesselInCompanyStateFactory;


        public VesselInCompanyConfigurator(IVesselInCompanyStateFactory vesselInCompanyStateFactory, IVesselInInventoryRepository vesselInInventoryRepository)
        {
            this.vesselInInventoryRepository = vesselInInventoryRepository;
            this.vesselInCompanyStateFactory = vesselInCompanyStateFactory;
        }

        public VesselInCompany Configure(VesselInCompany entity)
        {
            if (entity != null)
            {
                var vesselInInvenotry = vesselInInventoryRepository.First(v => v.Code == entity.Code && v.CompanyId == entity.CompanyId);
                entity.Configure(vesselInInvenotry, vesselInCompanyStateFactory);
            }
            return entity;
        }
    }
}
