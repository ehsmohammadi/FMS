
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VoyageLogToVoyageLogDtoMapper : BaseFacadeMapper<VoyageLog, VoyageLogDto>, IVoyageLogToVoyageLogDtoMapper
    {
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;
        private readonly IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper;

        public VoyageLogToVoyageLogDtoMapper(
            IFacadeMapper<Company, CompanyDto> companyDtoMapper, IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
            this.vesselInCompanyDtoMapper = vesselInCompanyDtoMapper;
        }

        public override VoyageLogDto MapToModel(VoyageLog entity)
        {
            var dto = base.MapToModel(entity);

            dto.Company = companyDtoMapper.MapToModel(entity.Company);
            dto.VesselInCompany = vesselInCompanyDtoMapper.MapToModel(entity.VesselInCompany);

            return dto;
        }
    }
}