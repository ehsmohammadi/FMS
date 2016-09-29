using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public class OffhireConfigurator : IEntityConfigurator<Offhire>
    {
        private readonly IOffhireStateFactory offhireStateFactory;
        private readonly IVesselInInventoryRepository vesselInInventoryRepository;
        private readonly IVesselInCompanyStateFactory vesselInCompanyStateFactory;

        public OffhireConfigurator(IOffhireStateFactory offhireStateFactory, IVesselInInventoryRepository vesselInInventoryRepository, IVesselInCompanyStateFactory vesselInCompanyStateFactory)
        {
            //Resolved
            this.offhireStateFactory = offhireStateFactory;
            this.vesselInInventoryRepository = vesselInInventoryRepository;
            this.vesselInCompanyStateFactory = vesselInCompanyStateFactory;
        }

        public Offhire Configure(Offhire entity)
        {
            if (entity != null)
            {
                entity.Configure(offhireStateFactory);

                var vesselInInvenotry = vesselInInventoryRepository.First(v => v.Code == entity.VesselInCompany.Code && v.CompanyId == entity.VesselInCompany.CompanyId);
                entity.VesselInCompany.Configure(vesselInInvenotry, vesselInCompanyStateFactory);
            }

            return entity;
        }
    }
}
