#region

using System;
using System.Collections.Generic;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;

#endregion

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface IOrderToDtoMapper : IFacadeMapper<Order, OrderDto>
    {
        OrderDto MapToModel(Order entity, Action<Order, OrderDto> action);

        IEnumerable<OrderDto> MapToModel(IEnumerable<Order> entities, Action<Order, OrderDto> action);

        OrderTypes MapOrderTypeDtoToOrderTypeEntity(OrderTypeEnum orderTypeEnum);
        OrderTypeEnum MapOrderTypeEntityToOrderTypeDto(OrderTypes orderTypes);
        IEnumerable<OrderDto> MapToModelWithAllIncludes(IEnumerable<Order> result, Action<Order, OrderDto> action);
        OrderDto MapToModelWithAllIncludes(Order order, Action<Order, OrderDto> action);
    }
}