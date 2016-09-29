using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class OrderItemToDtoMapper : BaseFacadeMapper<OrderItem, OrderItemDto>, IOrderItemToDtoMapper
    {
        private readonly IGoodToGoodDtoMapper goodMapper;
        private readonly IOrderItemBalanceToDtoMapper orderItemBalanceToDtoMapper;

        public OrderItemToDtoMapper(IGoodToGoodDtoMapper goodMapper, IOrderItemBalanceToDtoMapper orderItemBalanceToDtoMapper)
        {
            this.goodMapper = goodMapper;
            this.orderItemBalanceToDtoMapper = orderItemBalanceToDtoMapper;
        }

        public override OrderItemDto MapToModel(OrderItem orderItem)
        {

            var dto = new OrderItemDto
                          {
                              Id = orderItem.Id,
                              Description = orderItem.Description,
                              Quantity = orderItem.Quantity, //.HasValue ? entity.Quantity.Value : 0,                            
             
                              OrderId = orderItem.OrderId
                          };


            if (orderItem.Good == null)
                return dto;

            GoodDto goodDto = this.goodMapper.MapEntityToDtoWithUnits(orderItem.Good);
            goodDto.Unit = new GoodUnitDto {Id = orderItem.MeasuringUnit.Id, Name = orderItem.MeasuringUnit.Name};
            dto.Good = goodDto;

            dto.OrderItemBalances = new ObservableCollection<OrderItemBalanceDto>(orderItemBalanceToDtoMapper.MapToModel(orderItem.OrderItemBalances));

            return dto;
        }


        public override IEnumerable<OrderItemDto> MapToModel(IEnumerable<OrderItem> entities)
        {
            return entities.Select(MapToModel);
        }
    }
}