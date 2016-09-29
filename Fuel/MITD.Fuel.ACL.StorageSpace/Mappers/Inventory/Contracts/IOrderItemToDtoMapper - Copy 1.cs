using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IOrderItemBalanceToDtoMapper : IFacadeMapper<OrderItemBalance, OrderItemBalanceDto>
    {
        
    }
}