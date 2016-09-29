using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class VesselActivationItemToVesselActivationItemDtoMapper : BaseFacadeMapper<VesselActivationItem, VesselActivationItemDto>, IVesselActivationItemToVesselActivationItemDtoMapper
    {
        private readonly IFacadeMapper<VesselActivationItem, VesselActivationItemDto> vesselActivationItemDtoMapper;

        public VesselActivationItemToVesselActivationItemDtoMapper(IFacadeMapper<VesselActivationItem, VesselActivationItemDto> vesselActivationItemDtoMapper)
        {
            this.vesselActivationItemDtoMapper = vesselActivationItemDtoMapper;
        }

        public override VesselActivationItemDto MapToModel(VesselActivationItem entity)
        {
            var dto = base.MapToModel(entity);
            // dto.Owner = companyDtoMapper.MapToModel(entity.Owner);
            return dto;
        }
    }
}