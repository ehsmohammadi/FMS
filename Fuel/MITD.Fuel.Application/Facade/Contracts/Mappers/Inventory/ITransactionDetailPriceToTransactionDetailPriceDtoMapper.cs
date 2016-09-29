using System;
using System.Collections.Generic;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface ITransactionDetailPriceToTransactionDetailPriceDtoMapper : IFacadeMapper<Inventory_TransactionItemPrice, Inventory_TransactionDetailPriceDto>
    {
        IEnumerable<Inventory_TransactionDetailPriceDto> MapToModel(IEnumerable<Inventory_TransactionItemPrice> entities, bool includePricingReferences);
    }
}