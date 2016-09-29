using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class FuelReportInventoryResultController : ApiController
    {
        #region props

        private IFuelReportInventoryResultFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public FuelReportInventoryResultController()
        {

        }

        public FuelReportInventoryResultController(IFuelReportInventoryResultFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception("Facade service can not be null.");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public void Get(long id)
        {
            try
            {
                this.FacadeService.IsHandleResultPossible(id);
            }
            catch (BusinessRuleException ex)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }

        public void Post([FromBody]FuelReportInventoryResultDto dto)
        {
            this.FacadeService.HandleResult(dto);
        }

        #endregion
    }
}
