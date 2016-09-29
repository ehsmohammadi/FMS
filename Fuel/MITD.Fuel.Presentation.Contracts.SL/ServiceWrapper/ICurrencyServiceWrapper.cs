
using System;
using System.Collections.Generic;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;



namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface ICurrencyServiceWrapper : IServiceWrapper
    {
        void GetById(Action<CurrencyDto, Exception> action, long id);
        void GetAllCurrency(Action<List<CurrencyDto>, Exception> action);
        void GetCurrencyValueInMainCurrency(Action<decimal, Exception> action, long sourceCurrencyId, decimal value);
        void GetCurrencyValueInMainCurrency(Action<decimal, Exception> action, long sourceCurrencyId, decimal value, DateTime dateTime);
        void ConvertPrice(Action<decimal, Exception> action, decimal value, long sourceCurrencyId, long destinationCurrencyId, DateTime dateTime);
        void GetExchangeRates(Action<PageResultDto<CurrencyExchangeDto>, Exception> action, long? fromCurrencyId, long? toCurrencyId, int fiscalYear, int? pageSize, int? pageIndex);
        void UpdateCurrencies(Action<object, Exception> action);
    }
}