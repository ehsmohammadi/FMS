using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
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
                res.Code = currency.Code;
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
