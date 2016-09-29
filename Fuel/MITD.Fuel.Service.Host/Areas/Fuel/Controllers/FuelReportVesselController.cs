using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class FuelReportVesselController : ApiController
    {
        private IFuelReportVesselFacadeService FacadeService { get; set; }

        #region ctor

        public FuelReportVesselController(IFuelReportVesselFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public List<VesselInCompanyDto> Get()
        {
            return this.FacadeService.GetAll();

        }

        #endregion
    }
}
