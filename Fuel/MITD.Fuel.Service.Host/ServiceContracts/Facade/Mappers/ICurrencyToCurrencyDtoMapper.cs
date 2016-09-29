using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Service.Host.ServiceContracts.Facade.Mappers
{
    public interface ICurrencyToCurrencyDtoMapper:IFacadeMapper<Currency, CurrencyDto>
    {
        CurrencyDto MapEntityToDto(Currency currency);
        List<CurrencyDto> MapEntityToDto(List<Currency> currencies);
        Currency MapDtoToEntity(CurrencyDto currency);
        List<Currency> MapDtoToEntity(List<CurrencyDto> currencies);
    }
    }
