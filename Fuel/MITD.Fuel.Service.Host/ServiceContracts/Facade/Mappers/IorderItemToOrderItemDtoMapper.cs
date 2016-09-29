using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Service.Host.ServiceContracts.Facade.Mappers
{
    public interface IorderItemToOrderItemDtoMapper : IFacadeMapper<OrderItem,OrderItemDto>
    {
        OrderItemDto MapEntityToDto(OrderItem orderItem);
        List<OrderItemDto> MapEntityToDto(List<OrderItem> entity);
//        OrderItem MapDtoToEntity(OrderItemDto entity);
//        List<OrderItem> MapDtoToEntity(List<OrderItemDto> entity);
    }
}