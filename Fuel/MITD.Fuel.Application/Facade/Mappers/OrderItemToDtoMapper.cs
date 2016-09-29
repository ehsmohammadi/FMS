using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class OrderItemToDtoMapper : BaseFacadeMapper<OrderItem, OrderItemDto>, IOrderItemToDtoMapper
    {
        private readonly IGoodToGoodDtoMapper _goodMapper;

        public OrderItemToDtoMapper(IGoodToGoodDtoMapper goodMapper)
        {
            _goodMapper = goodMapper;
        }

        public override OrderItemDto MapToModel(OrderItem entity)
        {
            var dto = base.MapToModel(entity);

            if (entity.Good == null)
                return dto;

            GoodDto goodDto = _goodMapper.MapEntityToDtoWithUnits(entity.Good);

            goodDto.Unit = new GoodUnitDto { Id = entity.MeasuringUnit.Id, Name = entity.MeasuringUnit.Name };
            dto.Good = goodDto;
            return dto;
        }
    }
}