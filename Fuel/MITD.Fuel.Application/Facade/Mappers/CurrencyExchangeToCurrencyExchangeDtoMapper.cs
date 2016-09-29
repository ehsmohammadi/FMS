using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class CurrencyExchangeToCurrencyExchangeDtoMapper : BaseFacadeMapper<CurrencyExchange, CurrencyExchangeDto>, ICurrencyExchangeToCurrencyExchangeDtoMapper
    {
        public override CurrencyExchangeDto MapToModel(CurrencyExchange entity)
        {
            var currencyMapper = ServiceLocator.Current.GetInstance<ICurrencyToCurrencyDtoMapper>();

            var res = new CurrencyExchangeDto();
            if (entity != null)
            {
                res = base.MapToModel(entity);

                res.FromCurrency = currencyMapper.MapEntityToDto(entity.FromCurrency);
                res.ToCurrency = currencyMapper.MapEntityToDto(entity.ToCurrency);
            }

            return res;
        }
    }
}