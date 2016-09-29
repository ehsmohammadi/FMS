using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class CurrencyExchangeController : ApiController
    {
        #region props
        private ICurrencyFacadeService FacadeService { get; set; }
        #endregion

        #region ctor

        public CurrencyExchangeController()
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

        public CurrencyExchangeController(ICurrencyFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public PageResultDto<CurrencyExchangeDto> Get(long? fromCurrencyId, long? toCurrencyId, int fiscalYear, int? pageSize, int? pageIndex)
        {
            var result = this.FacadeService.GetExchangeRates(fromCurrencyId, toCurrencyId,fiscalYear,pageSize,pageIndex);
            return result;
        }

        #endregion
    }
}
