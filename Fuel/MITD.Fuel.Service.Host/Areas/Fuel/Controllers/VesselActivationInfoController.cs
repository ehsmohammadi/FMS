using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class VesselActivationInfoController : ApiController
    {
        #region props
        private IVesselInCompanyFacadeService VesselInCompanyFacadeService { get; set; }

        #endregion

        #region ctor

        public VesselActivationInfoController(IVesselInCompanyFacadeService vesselInCompanyFacadeService)
        {
            if (vesselInCompanyFacadeService == null)
                throw new Exception(" facade service can not be null");

            this.VesselInCompanyFacadeService = vesselInCompanyFacadeService;
        }
        #endregion

        #region methods

        public VesselActivationDto Get(string vesselCode)
        {
            return this.VesselInCompanyFacadeService.GetVesselActivationInfo(vesselCode);
        }

        #endregion
    }
}
