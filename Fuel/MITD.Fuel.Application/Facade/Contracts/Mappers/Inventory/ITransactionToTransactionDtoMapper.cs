using System;
using System.Collections;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
namespace MITD.Fuel.Application.Facade.Contracts.Mappers
{
    public interface ITransactionToTransactionDtoMapper : IFacadeMapper<Inventory_Transaction, Inventory_TransactionDto>
    {
    }
}