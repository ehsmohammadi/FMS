using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class PricingValueToPricingValueDtoMapper : BaseFacadeMapper<PricingValue, PricingValueDto>, IPricingValueToPricingValueDtoMapper
    {
        private readonly IGoodToGoodDtoMapper goodDtoMapper;
        private readonly ICurrencyToCurrencyDtoMapper currencyDtoMapper;


        public PricingValueToPricingValueDtoMapper(IGoodToGoodDtoMapper goodDtoMapper,
            ICurrencyToCurrencyDtoMapper currencyDtoMapper)
        {
            this.goodDtoMapper = goodDtoMapper;
            this.currencyDtoMapper = currencyDtoMapper;
        }

        public override PricingValueDto MapToModel(PricingValue entity)
        {
            var dto = new PricingValueDto()
                {
                    Fee = entity.Fee,
                    Good = this.goodDtoMapper.MapToModel(entity.Good),
                    Currency = entity.Currency == null ? null : currencyDtoMapper.MapToModel(entity.Currency)
                };

            return dto;
        }
    }
}