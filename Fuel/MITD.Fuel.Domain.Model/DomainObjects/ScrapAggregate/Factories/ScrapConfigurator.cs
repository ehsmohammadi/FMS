using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public class ScrapConfigurator : IEntityConfigurator<Scrap>
    {
        private readonly IScrapStateFactory scrapStateFactory;
        private readonly IVesselInInventoryRepository vesselInInventoryRepository;
        private readonly IVesselInCompanyStateFactory vesselInCompanyStateFactory;

        public ScrapConfigurator(IScrapStateFactory scrapStateFactory, IVesselInInventoryRepository vesselInInventoryRepository, IVesselInCompanyStateFactory vesselInCompanyStateFactory)
        {
            this.scrapStateFactory = scrapStateFactory;
            this.vesselInInventoryRepository = vesselInInventoryRepository;
            this.vesselInCompanyStateFactory = vesselInCompanyStateFactory;
        }

        public Scrap Configure(Scrap entity)
        {
            if (entity != null)
            {
                entity.Configure(scrapStateFactory);

                var vesselInInvenotry = vesselInInventoryRepository.First(v => v.Code == entity.VesselInCompany.Code && v.CompanyId == entity.VesselInCompany.CompanyId);
                entity.VesselInCompany.Configure(vesselInInvenotry, vesselInCompanyStateFactory);
            }

            return entity;
        }
    }
}
