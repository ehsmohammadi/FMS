using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class FuelReportInventoryOperationController : ApiController
    {
        #region props

        private IFuelReportFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public FuelReportInventoryOperationController()
        {

        }

        public FuelReportInventoryOperationController(IFuelReportFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public List<FuelReportInventoryOperationDto> Get(long id)
        {
            return this.FacadeService.GetInventoryOperations(id);
        }

        void Put(long id)
        {
            this.FacadeService.RevertFuelReportConsumptionInventoryOperations(id);            
        }

        #endregion
    }
}
