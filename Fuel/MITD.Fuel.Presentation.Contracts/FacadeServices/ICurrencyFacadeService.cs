using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade
{
    public interface ICurrencyFacadeService : IFacadeService
    {
        IEnumerable<CurrencyDto> GetAll();
        CurrencyDto GetById(int id);
        decimal GetCurrencyValueInMainCurrency(long currencyId, decimal value, DateTime dateTime);
        decimal ConvertPrice(decimal value, long sourceCurrencyId, long destinationCurrencyId, DateTime dateTime);
        decimal ConvertPriceToMainCurrency(decimal value, long sourceCurrencyId, DateTime dateTime);

        //void UpdateCurrenciesFromFinance();

        void UpdateCurrencies();

        PageResultDto<CurrencyExchangeDto> GetExchangeRates(long? fromCurrencyId, long? toCurrencyId, int fiscalYear, int? pageSize, int? pageIndex);
    }
}