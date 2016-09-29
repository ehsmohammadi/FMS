using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
using MITD.Fuel.Application.Facade.Contracts.Mappers;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class OffhirePreparedDataDetailToOffhireDetailDtoMapper : IOffhirePreparedDataDetailToOffhireDetailDtoMapper
    {
        private readonly IGoodToGoodDtoMapper goodDtoMapper;
        private readonly ITankToTankDtoMapper tankDtoMapper;
        private readonly ICompanyGoodUnitToGoodUnitDtoMapper goodUnitDtoMapper;

        public OffhirePreparedDataDetailToOffhireDetailDtoMapper(IGoodToGoodDtoMapper goodDtoMapper, ICompanyGoodUnitToGoodUnitDtoMapper goodUnitDtoMapper, ITankToTankDtoMapper tankDtoMapper)
        {
            this.goodDtoMapper = goodDtoMapper;
            this.goodUnitDtoMapper = goodUnitDtoMapper;
            this.tankDtoMapper = tankDtoMapper;
        }


        public OffhireDetailDto MapToModel(OffhirePreparedDataDetail entity)
        {
            var result = new OffhireDetailDto()
                         {
                             Id= -1,
                             Good = goodDtoMapper.MapToModel(entity.Good),
                             Quantity = entity.Quantity,
                             Unit = goodUnitDtoMapper.MapToModel(entity.Unit),
                             FeeInMainCurrency = entity.FeeInMainCurrency,
                             Tank = entity.Tank != null ? tankDtoMapper.MapToModel(entity.Tank) : null
                         };

            return result;
        }

        public IEnumerable<OffhireDetailDto> MapToModel(IEnumerable<OffhirePreparedDataDetail> entities)
        {
            return entities.Select(MapToModel);
        }

        #region Not Implemented
        
        public OffhirePreparedDataDetail MapToEntity(OffhireDetailDto model)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<OffhirePreparedDataDetail> MapToEntity(IEnumerable<OffhireDetailDto> models)
        {
            throw new System.NotImplementedException();
        }

        public OffhireDetailDto RemapModel(OffhireDetailDto model)
        {
            throw new System.NotImplementedException();
        } 

        #endregion
    }
}