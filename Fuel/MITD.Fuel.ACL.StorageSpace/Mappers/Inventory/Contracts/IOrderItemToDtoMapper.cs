using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IOrderItemToDtoMapper : IFacadeMapper<OrderItem,OrderItemDto>
    {
        //OrderItemDto MapEntityToDto(OrderItem orderItem);
        //IEnumerable<OrderItemDto> MapEntityToDto(IEnumerable<OrderItem> entity);
    }
}