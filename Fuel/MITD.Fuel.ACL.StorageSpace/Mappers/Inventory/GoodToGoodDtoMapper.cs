using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using Omu.ValueInjecter;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class GoodToGoodDtoMapper : BaseFacadeMapper<Good, GoodDto>, IGoodToGoodDtoMapper
    {
        private readonly ICompanyGoodUnitToGoodUnitDtoMapper _goodUnitMapper;
        public GoodToGoodDtoMapper(ICompanyGoodUnitToGoodUnitDtoMapper goodUnitMapper)
        {
            _goodUnitMapper = goodUnitMapper;
        }


        public List<GoodDto> MapEntityToDtoWithUnits(IEnumerable<Good> goods)
        {
            var result = new List<GoodDto>();
            foreach (var good in goods)
            {
                var dto = MapToModel(good);
                result.Add(dto);
                dto.Units = new List<GoodUnitDto>();
                good.GoodUnits.ForEach(c => dto.Units.Add(_goodUnitMapper.MapToModel(c)));
            }
            return result;
        }


        public GoodDto MapEntityToDtoWithUnits(Good entity)
        {
            var dto = MapToModel(entity);

            dto.Units = new List<GoodUnitDto>();
            entity.GoodUnits.ForEach(c => dto.Units.Add(_goodUnitMapper.MapToModel(c)));

            return dto;
        }
    }
}