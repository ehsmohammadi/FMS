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
    public class FuelReportCharterPreparedDataController : ApiController
    {
        #region props

        private IFuelReportFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public FuelReportCharterPreparedDataController(IFuelReportFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception("Facade service can not be null.");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public CharterDto Get(long id)
        {
            return FacadeService.PrepareCharterData(id);
        }

        #endregion
    }
}
