using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class TankToTankDtoMapper : BaseFacadeMapper<Tank, TankDto>, ITankToTankDtoMapper
    {
        private readonly IVesselInInventoryToVesselDtoMapper vesselMapper;

        public TankToTankDtoMapper(IVesselInInventoryToVesselDtoMapper  vesselMapper)
        {
            this.vesselMapper = vesselMapper;
        }

        public override TankDto MapToModel(Tank entity)
        {
            var dto = base.MapToModel(entity);
            dto.Code = entity.Name;
           dto.VesselDto = vesselMapper.MapToModel(entity.VesselInInventory);

            return dto;
        }


    }
}
