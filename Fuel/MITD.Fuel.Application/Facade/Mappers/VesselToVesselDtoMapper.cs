using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VesselToVesselDtoMapper : BaseFacadeMapper<Vessel, VesselDto>, IVesselToVesselDtoMapper
    {
        private readonly IFacadeMapper<Company, CompanyDto> companyDtoMapper;

        public VesselToVesselDtoMapper(IFacadeMapper<Company, CompanyDto> companyDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
        }

        public override VesselDto MapToModel(Vessel entity)
        {
            var dto = base.MapToModel(entity);
            dto.Owner = companyDtoMapper.MapToModel(entity.Owner);
            return dto;
        }
    }
}