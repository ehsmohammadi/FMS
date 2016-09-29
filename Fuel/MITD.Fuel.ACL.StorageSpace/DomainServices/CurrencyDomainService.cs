using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.FakeDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class CurrencyDomainService : ICurrencyDomainService
    {
        private readonly IRepository<Currency> currencyRepository;
        private readonly IRepository<CurrencyExchange> currencyExchangeRepository;

        private readonly IRepository<Inventory_UnitConvert> unitConvertRepository;

        public CurrencyDomainService(IRepository<Currency> currencyRepository, IRepository<CurrencyExchange> currencyExchangeRepository, IRepository<Inventory_UnitConvert> unitConvertRepository)
        {
            this.currencyRepository = currencyRepository;
            this.currencyExchangeRepository = currencyExchangeRepository;
            this.unitConvertRepository = unitConvertRepository;
        }

        public Currency Get(long id)
        {
            return currencyRepository.Single(c => c.Id == id);
        }


        public List<Currency> GetAll()
        {
            return currencyRepository.GetAll().ToList();
            /*
                 var data = Adapter.Get(id);
            if (data == null)
                throw new ObjectNotFound("EnterpriseParty");
            return data;
             */
        }

        public List<Currency> Get(List<long> IDs)
        {
            return GetAll().Where(c => IDs.Contains(c.Id)).ToList();
        }

        public decimal ConvertPrice(decimal sourceValue, Currency sourceCurrency, Currency destinationCurrency, DateTime dateTime)
        {
            //return sourceValue;

            var mainCurrency = GetMainCurrency();

            if (sourceCurrency.Id == mainCurrency.Id)
            {
                var rateToMainCurrency = GetCurrencyToMainCurrencyRate(destinationCurrency.Id, dateTime);

                var result = sourceValue / rateToMainCurrency;
                return decimal.Round(result, 2, MidpointRounding.AwayFromZero);
                //return result;
            }
            else if (destinationCurrency.Id == mainCurrency.Id)
            {
                var rateToMainCurrency = GetCurrencyToMainCurrencyRate(sourceCurrency.Id, dateTime);

                var result = sourceValue * rateToMainCurrency;
                return decimal.Round(result, 0, MidpointRounding.AwayFromZero);
                //return result;
            }
            else
            {
                var rateFromSourceToMainCurrency = GetCurrencyToMainCurrencyRate(sourceCurrency.Id, dateTime);
                var rateFromDestinationToMainCurrency = GetCurrencyToMainCurrencyRate(destinationCurrency.Id, dateTime);

                var result = sourceValue * rateFromSourceToMainCurrency / rateFromDestinationToMainCurrency;
                return decimal.Round(result, 2, MidpointRounding.AwayFromZero);
                //return result;
            }
        }

        public decimal ConvertPrice(decimal sourceValue, long sourceCurrencyId, long destinationCurrencyId, DateTime dateTime)
        {
            //return sourceValue;

            var mainCurrency = GetMainCurrency();

            if (sourceCurrencyId == mainCurrency.Id)
            {
                var rateToMainCurrency = GetCurrencyToMainCurrencyRate(destinationCurrencyId, dateTime);

                var result = sourceValue / rateToMainCurrency;
                return decimal.Round(result, 2, MidpointRounding.AwayFromZero);
                //return result;
            }
            else if (destinationCurrencyId == mainCurrency.Id)
            {
                var rateToMainCurrency = GetCurrencyToMainCurrencyRate(sourceCurrencyId, dateTime);

                var result = sourceValue * rateToMainCurrency;
                return decimal.Round(result, 0, MidpointRounding.AwayFromZero);
                //return result;
            }
            else
            {
                var rateFromSourceToMainCurrency = GetCurrencyToMainCurrencyRate(sourceCurrencyId, dateTime);
                var rateFromDestinationToMainCurrency = GetCurrencyToMainCurrencyRate(destinationCurrencyId, dateTime);

                var result = sourceValue * rateFromSourceToMainCurrency / rateFromDestinationToMainCurrency;
                return decimal.Round(result, 2, MidpointRounding.AwayFromZero);
                //return result;
            }
        }

        public Currency GetMainCurrency()
        {
            var rialCurrency = this.currencyRepository.First(c => c.Abbreviation.ToUpper() == "IRR");

            if (rialCurrency == null)
                throw new ObjectNotFound("MainCurrency");

            return rialCurrency;
        }

        public Currency GetByAbbreviation(string abbreviation)
        {
            return this.currencyRepository.Single(c => c.Abbreviation == abbreviation);
        }

        public decimal GetCurrencyValueInMainCurrency(Currency currency, decimal valueInCurrency, DateTime date)
        {
            var rate = GetCurrencyToMainCurrencyRate(currency.Id, date);
            return calculateCurrencyToMainCurrencyWithRate(valueInCurrency, rate);
        }

        public decimal GetCurrencyToMainCurrencyRate(long currencyId, DateTime date)
        {
            //Dollar to MainCurrency 
            //var currency = currencyRepository.Single(c => c.Id == currencyId);
            //if (currency.Abbreviation.ToUpper() == "USD")
            //    return 26500;

            //if (currency.Abbreviation.ToUpper() == "EUR")
            //    return 36863;

            //if (currency.Abbreviation.ToUpper() == "IRR")
            //    return 1;

            var mainCurrency = this.GetMainCurrency();
            var currency = currencyRepository.Single(c => c.Id == currencyId);

            if (currency == null)
                throw new ObjectNotFound("From Currecny");

            if (currencyId == mainCurrency.Id)
                return 1;

            var currencyExchange = currencyExchangeRepository.Single(ce => ce.ToCurrencyId == mainCurrency.Id && ce.FromCurrencyId == currencyId && ce.EffectiveDateStart <= date && (!ce.EffectiveDateEnd.HasValue || ce.EffectiveDateEnd >= date));

            if (currencyExchange == null)
                throw new ObjectNotFound("Currency Exchange to Main Currency for " + currency.Abbreviation);

            return currencyExchange.Coefficient;
        }

        public void UpdateCurrenciesFromFinance()
        {
            using (var context = new Integration.Finance.Basis.FinanceViewsContext())
            {
                context.UpdateCurrenciesFromBasis();
            }
        }

        public void UpdateCurrencyRatesFromFinance()
        {
            var currenciesExchange = new List<Inventory_UnitConvert>();

                var mainCurrency = this.GetMainCurrency();

                var fuelSystemAllCurrencies = this.GetAll();

            //var lastRowVersion = unitConvertRepository.GetQuery().Max(ce => ce.RowVersion);

            using (var context = new Integration.Finance.Basis.FinanceViewsContext())
            {
                foreach (var ngsCurrencyRatesView in context.NgsCurrencyRatesViews.ToList())
                {
                    var fromCurrency = fuelSystemAllCurrencies.SingleOrDefault(c => c.Abbreviation == ngsCurrencyRatesView.Abbreviation);

                    if (fromCurrency == null) continue;

                    var ngsCurrencyRatePersianDate = ngsCurrencyRatesView.PersianEffectiveDate;

                    var pCal = new PersianCalendar();
                    var ngsCurrencyRateEffectiveStartDate = pCal.ToDateTime(
                            int.Parse(ngsCurrencyRatePersianDate.Substring(0, 4)),
                            int.Parse(ngsCurrencyRatePersianDate.Substring(4, 2)),
                            int.Parse(ngsCurrencyRatePersianDate.Substring(6, 2)), 0, 0, 0, 0);

                    var currentCurrencyExchanges = unitConvertRepository.Find(ce => ce.UnitId == fromCurrency.Id && ce.SubUnitId == mainCurrency.Id && ce.EffectiveDateStart < ngsCurrencyRateEffectiveStartDate && !ce.EffectiveDateEnd.HasValue).ToList();

                    currentCurrencyExchanges.ForEach(ce => ce.EffectiveDateEnd = ngsCurrencyRateEffectiveStartDate.AddMinutes(-1));

                    currenciesExchange.Where(ce => ce.UnitId == fromCurrency.Id && ce.SubUnitId == mainCurrency.Id && ce.EffectiveDateStart < ngsCurrencyRateEffectiveStartDate && !ce.EffectiveDateEnd.HasValue).ToList()
                        .ForEach(ce => ce.EffectiveDateEnd = ngsCurrencyRateEffectiveStartDate.AddMinutes(-1));

                    if(fromCurrency.Id == mainCurrency.Id)
                        continue;

                    var unitConvert = new Inventory_UnitConvert()
                        {
                            UnitId = fromCurrency.Id,
                            SubUnitId = mainCurrency.Id,
                            EffectiveDateStart = ngsCurrencyRateEffectiveStartDate,
                            EffectiveDateEnd = null,
                            CreateDate = DateTime.Now,
                            UserCreatorId = 100000,  //Inventory Admin
                            Coefficient = ngsCurrencyRatesView.ExchangeRateToMainCurrency,
                        };

                    if(unitConvertRepository.Count(ce => ce.UnitId == unitConvert.UnitId && ce.SubUnitId == unitConvert.SubUnitId && ce.EffectiveDateStart == unitConvert.EffectiveDateStart.Value) == 0)
                        currenciesExchange.Add(unitConvert);
                }
            }

            foreach (var inventoryUnitConvert in currenciesExchange)
            {
                unitConvertRepository.Add(inventoryUnitConvert);
            }
        }

        private decimal calculateCurrencyToMainCurrencyWithRate(decimal valueInCurrency, decimal rate)
        {
            var result = valueInCurrency * rate;
            return decimal.Round(result, 2, MidpointRounding.AwayFromZero);
        }
    }
}
