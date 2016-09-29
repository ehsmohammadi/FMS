using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface ICharterOutToDtoMapper : IFacadeMapper<CharterOut, CharterDto>
    {
        CharterDto MapToDtoModel(CharterOut charterIn);
        PageResultDto<CharterDto> MapToDtoModels(PageResult<CharterOut> charterIns);

        MITD.Fuel.Presentation.Contracts.Enums.OffHirePricingType OfHreToDtoConvertor(
            MITD.Fuel.Domain.Model.Enums.OffHirePricingType pricingType);

        MITD.Fuel.Domain.Model.Enums.OffHirePricingType DtoToOfHreConvertor(
          MITD.Fuel.Presentation.Contracts.Enums.OffHirePricingType pricingType);

        CharterEndTypeEnum CharterEndTypeConvertor(CharterEndType charterEndType);
        CharterEndType CharterEndTypeEnumConvertor(CharterEndTypeEnum charterEndTypeEnum);

        PageResultDto<FuelReportInventoryOperationDto> MapToInvDtoModels(PageResult<InventoryOperation> entities);

        FuelReportInventoryOperationDto MapToInvDtoModel(InventoryOperation entity);
    }
}
