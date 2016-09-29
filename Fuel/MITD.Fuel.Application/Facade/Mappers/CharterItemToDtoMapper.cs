using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class CharterItemToDtoMapper : BaseFacadeMapper<CharterItem, CharterItemDto>, ICharterItemToDtoMapper
    {
        public CharterItemDto MapToDtoModel(CharterItem charterItem)
        {
            return new CharterItemDto()
                       {
                           Id=charterItem.Id,
                           CharterId = charterItem.Id,
                           Fee=charterItem.Fee,
                           FeeOffhire=charterItem.OffhireFee,
                           Rob=charterItem.Rob,
                           Good = new GoodDto()
                                      {
                                          Id=charterItem.Good.Id,
                                          Name=charterItem.Good.Name,
                                          Unit =new GoodUnitDto
                                                    {
                                                       Id=charterItem.GoodUnit.Id,
                                                       Name = charterItem.GoodUnit.Name
                                                    }
                                      },
                         TankDto =new TankDto()
                                      {
                                         Id= charterItem.Tank.Id,
                                         Code = charterItem.Tank.Name
                                      }
                       };
        }

        public PageResultDto<CharterItemDto> MapToDtoModels(PageResult<CharterItem> charterItems)
        {
            var res = new PageResultDto<CharterItemDto>();
            res.Result=new List<CharterItemDto>();
            charterItems.Result.ToList().ForEach(c => res.Result.Add(MapToDtoModel(c)));
            res.TotalCount = charterItems.TotalCount;
            res.TotalPages = charterItems.TotalPages;
            res.CurrentPage = charterItems.CurrentPage;
            return res;
        }
    }
}
