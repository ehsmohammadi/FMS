using System.Collections.Generic;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface ICurrencyToCurrencyDtoMapper : IFacadeMapper<Currency, CurrencyDto>
    {
        CurrencyDto MapEntityToDto(Currency currency);
        List<CurrencyDto> MapEntityToDto(IEnumerable<Currency> currencies);

        
    }
}
