using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class OffhireManagementSystemPreparedDataController : ApiController
    {
        private IOffhireFacadeService FacadeService { get; set; }

        #region ctor

        public OffhireManagementSystemPreparedDataController()
        {

        }

        public OffhireManagementSystemPreparedDataController(IOffhireFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        //================================================================================

        public OffhireDto Get(long referenceNumber, long introducerId)
        {
            return this.FacadeService.PrepareOffhireData(referenceNumber, introducerId);
        }

        //================================================================================

        //public OffhireManagementSystemDto Get(string referenceNumber)
        //{
        //    return this.FacadeService.GetOffhireManagementSystemPagedDatacompanyId, vesselId, fromDate, toDate, pageSize, pageIndex);
        //}

        //================================================================================

        #endregion
    }
}
