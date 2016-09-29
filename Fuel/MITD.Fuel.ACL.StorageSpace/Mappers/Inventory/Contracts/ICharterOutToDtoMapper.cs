using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Facade;
using MITD.Presentation.Contracts;
using OffHirePricingType = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.OffHirePricingType;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface ICharterOutToDtoMapper : IFacadeMapper<CharterOut, CharterDto>
    {
        CharterDto MapToDtoModel(CharterOut charterIn);
        PageResultDto<CharterDto> MapToDtoModels(PageResult<CharterOut> charterIns);

        OffHirePricingType OffhireToDtoConvertor(
            MITD.Fuel.Domain.Model.Enums.OffHirePricingType pricingType);

        MITD.Fuel.Domain.Model.Enums.OffHirePricingType DtoToOffhireConvertor(
          OffHirePricingType pricingType);

        CharterEndTypeEnum CharterEndTypeConvertor(CharterEndType charterEndType);

        PageResultDto<FuelReportInventoryOperationDto> MapToInvDtoModels(PageResult<InventoryOperation> entities);

        FuelReportInventoryOperationDto MapToInvDtoModel(InventoryOperation entity);
    }
}
