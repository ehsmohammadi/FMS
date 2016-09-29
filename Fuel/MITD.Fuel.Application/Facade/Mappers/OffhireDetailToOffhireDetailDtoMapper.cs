using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class OffhireDetailToOffhireDetailDtoMapper : BaseFacadeMapper<OffhireDetail, OffhireDetailDto>, IOffhireDetailToOffhireDetailDtoMapper
    {
        private readonly IGoodToGoodDtoMapper goodMapper;
        private readonly ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper;
        private readonly ITankToTankDtoMapper tankMapper;

        public OffhireDetailToOffhireDetailDtoMapper(
            IGoodToGoodDtoMapper goodMapper,
            ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper,
            ITankToTankDtoMapper tankMapper)
        {
            this.goodMapper = goodMapper;
            this.goodUnitMapper = goodUnitMapper;
            this.tankMapper = tankMapper;
        }

        public override OffhireDetailDto MapToModel(OffhireDetail entity)
        {
            var dto = new OffhireDetailDto()
                      {
                          Id = entity.Id,
                          Quantity = entity.Quantity,
                          Good = goodMapper.MapToModel(entity.Good),
                          Tank = tankMapper.MapToModel(entity.Tank),
                          Unit = goodUnitMapper.MapToModel(entity.Unit),
                          FeeInVoucherCurrency = entity.FeeInVoucherCurrency,
                          FeeInMainCurrency = entity.FeeInMainCurrency
                      };

            return dto;
        }
    }
}