using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface ICharterItemToDtoMapper : IFacadeMapper<CharterItem, CharterItemDto>
    {
        CharterItemDto MapToDtoModel(CharterItem charterItem);
        PageResultDto<CharterItemDto> MapToDtoModels(PageResult<CharterItem> charterItems);

    }
}
