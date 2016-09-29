#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

#endregion

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public class CurrencyServiceWrapper : ICurrencyServiceWrapper
    {
        private readonly string currencyAddressFormatString;
        
        private readonly string currencyExchangeAddressFormatString;
                
        public CurrencyServiceWrapper()
        {
            currencyAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/Currency/{0}");
            currencyExchangeAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Fuel/CurrencyExchange/{0}");
        }

        #region ICurrencyServiceWrapper Members

        public void GetById(Action<CurrencyDto, Exception> action, long id)
        {
            var url = string.Format(currencyAddressFormatString, id);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetAllCurrency(Action<List<CurrencyDto>, Exception> action)
        {
            var url = string.Format(currencyAddressFormatString, string.Empty);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        //decimal GetCurrencyValueInMainCurrency(long currencyId, decimal value);
        public void GetCurrencyValueInMainCurrency(Action<decimal, Exception> action, long sourceCurrencyId, decimal value)
        {
            string url = string.Format(currencyAddressFormatString, string.Empty);
            var sb = new StringBuilder(url);
            sb.Append(string.Concat("?sourceCurrencyId=", sourceCurrencyId));
            sb.Append(string.Concat("&value=", value));

            WebClientHelper.Get(new Uri(sb.ToString(), UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetCurrencyValueInMainCurrency(Action<decimal, Exception> action, long sourceCurrencyId, decimal value, DateTime dateTime)
        {
            string url = string.Format(currencyAddressFormatString, string.Empty);
            var sb = new StringBuilder(url);
            sb.Append(string.Concat("?sourceCurrencyId=", sourceCurrencyId));
            sb.Append(string.Concat("&value=", value));
            sb.Append(string.Concat("&dateTime=", HttpUtil.DateTimeToString(dateTime)));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sb), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void ConvertPrice(Action<decimal, Exception> action, decimal value, long sourceCurrencyId, long destinationCurrencyId, DateTime dateTime)
        {
            string url = string.Format(currencyAddressFormatString, string.Empty);
            var sb = new StringBuilder(url);
            sb.Append(string.Concat("?value=", value));
            sb.Append(string.Concat("&sourceCurrencyId=", sourceCurrencyId));
            sb.Append(string.Concat("&destinationCurrencyId=", destinationCurrencyId));
            sb.Append(string.Concat("&dateTime=", HttpUtil.DateTimeToString(dateTime)));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sb), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetExchangeRates(Action<PageResultDto<CurrencyExchangeDto>, Exception> action, long? fromCurrencyId, long? toCurrencyId, int fiscalYear, int? pageSize, int? pageIndex)
        {
            string url = string.Format(currencyExchangeAddressFormatString, string.Empty);
            var sb = new StringBuilder(url);
            sb.Append(string.Concat("?fromCurrencyId=", fromCurrencyId));
            sb.Append(string.Concat("&toCurrencyId=", toCurrencyId));
            sb.Append(string.Concat("&fiscalYear=", fiscalYear));
            sb.Append(string.Concat("&pageSize=", pageSize));
            sb.Append(string.Concat("&pageIndex=", pageIndex));

            WebClientHelper.Get(ApiServiceAddressHelper.BuildUri(sb), action, WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        public void UpdateCurrencies(Action<object, Exception> action)
        {
            string url = string.Format(currencyAddressFormatString, string.Empty);
            WebClientHelper.Put(ApiServiceAddressHelper.BuildUri(url), action, new object(), WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        #endregion
    }
}