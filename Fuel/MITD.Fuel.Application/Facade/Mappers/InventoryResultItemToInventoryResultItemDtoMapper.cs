using System.Collections.Generic;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class InventoryResultItemToInventoryResultItemDtoMapper : BaseFacadeMapper<InventoryResultItem, InventoryResultItemDto>, IInventoryResultItemToInventoryResultItemDtoMapper
    {
        private readonly IGoodToGoodDtoMapper goodMapper;
        private readonly ICurrencyToCurrencyDtoMapper currencyMapper;

        public InventoryResultItemToInventoryResultItemDtoMapper(
            IGoodToGoodDtoMapper goodMapper,
            ICurrencyToCurrencyDtoMapper currencyMapper)
        {
            this.goodMapper = goodMapper;

            this.currencyMapper = currencyMapper;
        }

        public override InventoryResultItemDto MapToModel(InventoryResultItem item)
        {
            GoodDto goodDto = this.goodMapper.MapEntityToDtoWithUnits(item.Good);

            CurrencyDto currencyDto = this.currencyMapper.MapEntityToDto(item.Currency);

            var dto = new InventoryResultItemDto()
                      {
                          Id = item.Id,
                          Good = goodDto,
                          Currency = currencyDto,
                          Fee = item.Fee,
                          Quantity = item.Quantity,
                          TransactionId = item.TransactionId
                      };

            return dto;
        }

        //public CorrectionPricingTypeEnum MapEntityCorrectionPricingTypeToDtoCorrectionPricingType(CorrectionPricingTypes? correctionPricingType)
        //{
        //    switch (correctionPricingType)
        //    {
        //        case CorrectionPricingTypes.Default: return CorrectionPricingTypeEnum.Default;
        //        case CorrectionPricingTypes.LastIssuedConsumption: return CorrectionPricingTypeEnum.LastIssuedConsumption;
        //        case CorrectionPricingTypes.LastPurchasePrice: return CorrectionPricingTypeEnum.LastPurchasePrice;
        //        case CorrectionPricingTypes.ManualPricing: return CorrectionPricingTypeEnum.ManualPricing;
        //        case null:
        //        default:
        //            return CorrectionPricingTypeEnum.NotDefined;
        //    }
        //}

        //public CorrectionPricingTypes? MapDtoCorrectionPricingTypeToEntityCorrectionPricingType(CorrectionPricingTypeEnum? correctionPricingType)
        //{
        //    switch (correctionPricingType)
        //    {
        //        case CorrectionPricingTypeEnum.Default: return CorrectionPricingTypes.Default;
        //        case CorrectionPricingTypeEnum.LastIssuedConsumption: return CorrectionPricingTypes.LastIssuedConsumption;
        //        case CorrectionPricingTypeEnum.LastPurchasePrice: return CorrectionPricingTypes.LastPurchasePrice;
        //        case CorrectionPricingTypeEnum.ManualPricing: return CorrectionPricingTypes.ManualPricing;
        //        case null:
        //        default:
        //            return null;
        //    }
        //}
    }
}