#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Castle.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

#endregion

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class CurrencyFacadeService : ICurrencyFacadeService
    {
        #region props

        private readonly ICurrencyDomainService currencyDomainService;
        private readonly ICurrencyToCurrencyDtoMapper _mapper;
        private readonly ICurrencyApplicationService currencyApplicationService;
        private readonly IRepository<CurrencyExchange> currencyExchangeRepository;
        private ICurrencyExchangeToCurrencyExchangeDtoMapper currencyExchangeDtoMapper;
        #endregion

        #region ctor

        public CurrencyFacadeService(
            ICurrencyDomainService currencyDomainService,
            ICurrencyToCurrencyDtoMapper currencyToCurrencyDtoMapper,
            ICurrencyApplicationService currencyApplicationService,
            IRepository<CurrencyExchange> currencyExchangeRepository, ICurrencyExchangeToCurrencyExchangeDtoMapper currencyExchangeDtoMapper)
        {
            this.currencyDomainService = currencyDomainService;
            this._mapper = currencyToCurrencyDtoMapper;
            this.currencyApplicationService = currencyApplicationService;
            this.currencyExchangeRepository = currencyExchangeRepository;
            this.currencyExchangeDtoMapper = currencyExchangeDtoMapper;
        }

        #endregion

        #region methods

        public IEnumerable<CurrencyDto> GetAll()
        {
            var entities = currencyDomainService.GetAll();
            var dtos = _mapper.MapToModel(entities).ToList();

            return dtos;
        }

        public CurrencyDto GetById(int id)
        {
            var dto = _mapper.MapEntityToDto(currencyDomainService.Get(id));
            dto.CurrencyToMainCurrencyRate = currencyDomainService.GetCurrencyToMainCurrencyRate(id, DateTime.Now);
            return dto;
        }

        public decimal GetCurrencyValueInMainCurrency(long currencyId, decimal value, DateTime dateTime)
        {
            var currency = currencyDomainService.Get(currencyId);

            var result = currencyDomainService.GetCurrencyValueInMainCurrency(currency, value, dateTime);
            return result;
        }

        public decimal ConvertPrice(decimal value, long sourceCurrencyId, long destinationCurrencyId, DateTime dateTime)
        {
            var sourceCurrency = currencyDomainService.Get(sourceCurrencyId);
            var destinationCurrency = currencyDomainService.Get(destinationCurrencyId);

            var result = currencyDomainService.ConvertPrice(value, sourceCurrency, destinationCurrency, dateTime);

            return result;
        }

        public decimal ConvertPriceToMainCurrency(decimal value, long sourceCurrencyId, DateTime dateTime)
        {
            var sourceCurrency = currencyDomainService.Get(sourceCurrencyId);
            var destinationCurrency = currencyDomainService.GetMainCurrency();

            var result = currencyDomainService.ConvertPrice(value, sourceCurrency, destinationCurrency, dateTime);

            return result;
        }

        //public void UpdateCurrenciesFromFinance()
        //{
        //    this.currencyApplicationService.UpdateCurrenciesFromFinance();
        //}

        public void UpdateCurrencies()
        {
            this.currencyApplicationService.UpdateCurrencies();
        }

        public PageResultDto<CurrencyExchangeDto> GetExchangeRates(long? fromCurrencyId, long? toCurrencyId, int fiscalYear, int? pageSize, int? pageIndex)
        {
            PersianCalendar pCal = new PersianCalendar();

            var fromDateTime = pCal.ToDateTime(fiscalYear, 1, 1, 0, 0, 0, 0);
            var toDateTime = pCal.ToDateTime(fiscalYear + 1, 1, 1, 0, 0, 0, 0).AddSeconds(-1);

            var listFetch = new ListFetchStrategy<CurrencyExchange>().Include(p => p.FromCurrency).Include(p => p.ToCurrency).OrderByDescending(p => p.EffectiveDateStart);

            if (pageIndex.HasValue && pageSize.HasValue)
                listFetch = listFetch.WithPaging(pageSize.Value, pageIndex.Value + 1);

            currencyExchangeRepository.Find(ce => (!fromCurrencyId.HasValue || ce.FromCurrencyId == fromCurrencyId.Value) && (!toCurrencyId.HasValue || ce.ToCurrencyId == toCurrencyId.Value) && (ce.EffectiveDateStart <= toDateTime && (!ce.EffectiveDateEnd.HasValue || ce.EffectiveDateEnd >= fromDateTime)), listFetch);

            return new PageResultDto<CurrencyExchangeDto>()
                   {
                       CurrentPage = listFetch.PageCriteria.PageResult.CurrentPage,
                       PageSize = listFetch.PageCriteria.PageResult.PageSize,
                       TotalCount = listFetch.PageCriteria.PageResult.TotalCount,
                       TotalPages = listFetch.PageCriteria.PageResult.TotalPages,
                       Result = currencyExchangeDtoMapper.MapToModel(listFetch.PageCriteria.PageResult.Result).ToList()
                   };
        }

        #endregion

    }
}