using MITD.Fuel.Application.Facade.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Test
{
    public class BasicInfoFacadeServiceObjectsContainer
    {
        public BasicInfoFacadeServiceObjectsContainer()
        {
            this.GoodDtoMapper = new GoodToGoodDtoMapper(new CompanyGoodUnitToGoodUnitDtoMapper());
            this.CurrencyDtoMapper = new CurrencyToCurrencyDtoMapper();

            this.CompanyDtoMapper = new BaseFacadeMapper<Company, CompanyDto>();
            this.VesselInCompanyDtoMapper = new VesselInCompanyToVesselInCompanyDtoMapper(this.CompanyDtoMapper);
            this.VesselInInventoryDtoMapper = new VesselInInventoryToVesselDtoMapper(this.CompanyDtoMapper);
            this.TankDtoMapper = new TankToTankDtoMapper(VesselInInventoryDtoMapper);
        }

        public TankToTankDtoMapper TankDtoMapper { get; private set; }

        public VesselInCompanyToVesselInCompanyDtoMapper VesselInCompanyDtoMapper { get; private set; }
        public VesselInInventoryToVesselDtoMapper VesselInInventoryDtoMapper { get; private set; }

        public BaseFacadeMapper<Company, CompanyDto> CompanyDtoMapper { get; private set; }

        public CurrencyToCurrencyDtoMapper CurrencyDtoMapper { get; private set; }

        public GoodToGoodDtoMapper GoodDtoMapper { get; private set; }
    }
}