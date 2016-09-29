using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class CurrencyToCurrencyDtoMapper : BaseFacadeMapper<Currency, CurrencyDto>, ICurrencyToCurrencyDtoMapper
    {
        public CurrencyDto MapEntityToDto(Currency currency)
        {
            var res = new CurrencyDto();
            if (currency != null)
            {
                res.Id = currency.Id;
                res.Abbreviation = currency.Abbreviation;
                res.Name = currency.Name;
            }

            return res;
        }

        public List<CurrencyDto> MapEntityToDto(IEnumerable<Currency> currencies)
        {
            List<CurrencyDto> dtos = new List<CurrencyDto>();

            if (currencies != null)
                dtos = currencies.Select(MapEntityToDto).ToList();

            return dtos;
        }
    }
}