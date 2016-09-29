using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class OrderItemBalanceToDtoMapper : BaseFacadeMapper<OrderItemBalance, OrderItemBalanceDto>, IOrderItemBalanceToDtoMapper
    {
        public override OrderItemBalanceDto MapToModel(OrderItemBalance entity)
        {
            var dto = base.MapToModel(entity);

            dto.Price = entity.InvoiceItem.Price;
            dto.CurrencyCode = entity.InvoiceItem.Invoice.Currency.Abbreviation;

            return dto;
        }

        public override IEnumerable<OrderItemBalanceDto> MapToModel(IEnumerable<OrderItemBalance> entities)
        {
            return entities.Select(this.MapToModel);
        }
    }
}