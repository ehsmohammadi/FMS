using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.DTOs.Report;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class VesselEventDataController : ApiController
    {
        #region props

        private IFuelReportFacadeService FuelReportFacadeService { get; set; }

        #endregion

        #region ctor

        public VesselEventDataController(IFuelReportFacadeService fuelReportFacadeService)
        {
            if (fuelReportFacadeService == null)
                throw new Exception(" facade service can not be null");

            this.FuelReportFacadeService = fuelReportFacadeService;
        }
        #endregion

        #region methods

        public VesselEventReportViewDto Get(string id)
        {
            return this.FuelReportFacadeService.GetVesselEventData(id);
        }

        #endregion
    }
}
