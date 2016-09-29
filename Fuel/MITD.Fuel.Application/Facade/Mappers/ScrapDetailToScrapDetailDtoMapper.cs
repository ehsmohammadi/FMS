using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class ScrapDetailToScrapDetailDtoMapper : BaseFacadeMapper<ScrapDetail, ScrapDetailDto>, IScrapDetailToScrapDetailDtoMapper
    {
        private readonly ICurrencyToCurrencyDtoMapper currencyMapper;
        private readonly IGoodToGoodDtoMapper goodMapper;
        private readonly ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper;
        private readonly ITankToTankDtoMapper tankMapper;

        public ScrapDetailToScrapDetailDtoMapper(
            ICurrencyToCurrencyDtoMapper currencyMapper,
            IGoodToGoodDtoMapper goodMapper,
            ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper,
            ITankToTankDtoMapper tankMapper)
        {
            this.currencyMapper = currencyMapper;
            this.goodMapper = goodMapper;
            this.goodUnitMapper = goodUnitMapper;
            this.tankMapper = tankMapper;
        }

        public override ScrapDetailDto MapToModel(ScrapDetail entity)
        {
            var dto = base.MapToModel(entity);

            dto.Currency = currencyMapper.MapToModel(entity.Currency);
            dto.Good = goodMapper.MapToModel(entity.Good);
            dto.Tank = tankMapper.MapToModel(entity.Tank);
            dto.Unit = goodUnitMapper.MapToModel(entity.Unit);

            return dto;
        }
    }

}