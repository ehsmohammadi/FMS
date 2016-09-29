using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using OffHirePricingType = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.OffHirePricingType;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface ICharterInToDtoMapper : IFacadeMapper<CharterIn, CharterDto>
    {
        CharterDto MapToDtoModel(CharterIn charterIn);
        PageResultDto<CharterDto> MapToDtoModels(PageResult<CharterIn> charterIns);

        OffHirePricingType OffhireToDtoConvertor(
            MITD.Fuel.Domain.Model.Enums.OffHirePricingType pricingType);

        MITD.Fuel.Domain.Model.Enums.OffHirePricingType DtoToOfHreConvertor(
          OffHirePricingType pricingType);

        CharterEndTypeEnum CharterEndTypeConvertor(CharterEndType charterEndType);

        PageResultDto<FuelReportInventoryOperationDto> MapToInvDtoModels(PageResult<InventoryOperation> entities);

        FuelReportInventoryOperationDto MapToInvDtoModel(InventoryOperation entity);
    }
}
