using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class VoyageToVoyageDtoMapper : BaseFacadeMapper<Voyage, VoyageDto>, IVoyageToVoyageDtoMapper
    {
        public VoyageToVoyageDtoMapper()
        {

        }

        public override VoyageDto MapToModel(Voyage entity)
        {
            var dto = base.MapToModel(entity);

            dto.Code = entity.VoyageNumber;

            return dto;
        }
    }
}