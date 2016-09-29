using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Application.Facade;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class FiscalYearController : ApiController
    {
        #region props
        private IFiscalYearFacadeService FacadeService { get; set; }
        #endregion

        #region ctor

        public FiscalYearController()
        {
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IFiscalYearFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public FiscalYearController(IFiscalYearFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public List<FiscalYearDto> Get()
        {
            var result = this.FacadeService.GetAll();
            return result;
        }

        #endregion
    }
}
