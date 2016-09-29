using System.Collections.Generic;
using MITD.Core;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IOrderToDtoMapper : IFacadeMapper<Order, OrderDto>
    {
        
        OrderTypes MapOrderTypeDtoToOrderTypeEntity(OrderTypeEnum orderTypeEnum);
        OrderTypeEnum MapOrderTypeEntityToOrderTypeDto(OrderTypes orderTypes);
        //IEnumerable<OrderDto> MapToModelWithAllIncludes(IEnumerable<Order> result);
        //OrderDto MapToModelWithAllIncludes(Order order);
    }
}