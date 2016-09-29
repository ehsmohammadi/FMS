using System.Collections.Generic;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IGoodToGoodDtoMapper : IFacadeMapper<Good, GoodDto>
    {
        GoodDto MapEntityToDtoWithUnits(Good entity);

        List<GoodDto> MapEntityToDtoWithUnits(IEnumerable<Good> entities);
    }
}