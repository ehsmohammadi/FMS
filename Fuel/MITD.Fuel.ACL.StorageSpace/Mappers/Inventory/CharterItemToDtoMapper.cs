using System.Collections.Generic;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
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
                                                    },
                                                    SharedGoodId =  charterItem.Good.SharedGoodId
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
            charterItems.Result.ToList().ForEach(c => res.Result.Add(this.MapToDtoModel(c)));
            res.TotalCount = charterItems.TotalCount;
            res.TotalPages = charterItems.TotalPages;
            res.CurrentPage = charterItems.CurrentPage;
            return res;
        }
    }
}
