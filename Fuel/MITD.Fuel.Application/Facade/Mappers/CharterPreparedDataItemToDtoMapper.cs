using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class CharterPreparedDataItemToDtoMapper : BaseFacadeMapper<CharterPreparedDataItem, CharterItemDto>, ICharterPreparedDataItemToDtoMapper
    {
        public override CharterItemDto MapToModel(CharterPreparedDataItem entity)
        {
            return new CharterItemDto
                   {

                       Rob = entity.Rob,
                       Good = new GoodDto
                              {
                                  Id = entity.Good.Id,
                                  Name = entity.Good.Name,
                                  Unit = new GoodUnitDto
                                  {
                                      Id = entity.Unit.Id,
                                      Name = entity.Unit.Name
                                  }
                              },
                       TankDto = new TankDto
                                 {
                                     Id = entity.Tank.Id,
                                     Code = entity.Tank.Name
                                 }
                   };
        }
    }
}
