using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Application.Facade;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class CurrencyController : ApiController
    {
        #region props
        private ICurrencyFacadeService FacadeService { get; set; }
        #endregion

        #region ctor

        public CurrencyController()
        {
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<ICurrencyFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CurrencyController(ICurrencyFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public IEnumerable<CurrencyDto> Get()
        {
            var data = this.FacadeService.GetAll();
            return data;

        }

        public CurrencyDto Get(int id)
        {
            var result = this.FacadeService.GetById(id);
            return result;
        }

        public decimal Get(decimal value, long sourceCurrencyId, long destinationCurrencyId, DateTime dateTime)
        {
            var result = this.FacadeService.ConvertPrice(value, sourceCurrencyId, destinationCurrencyId, dateTime);
            return result;
        }

        public decimal GetInMainCurrency(long sourceCurrencyId, decimal value, DateTime dateTime)
        {
            var result = this.FacadeService.GetCurrencyValueInMainCurrency(sourceCurrencyId, value, dateTime);
            return result;
        }

        public void Put()
        {
            this.FacadeService.UpdateCurrencies();
        }

        #endregion
    }
}
