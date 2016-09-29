using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class OffhireManagementSystemController : ApiController
    {
        private IOffhireFacadeService FacadeService { get; set; }

        #region ctor

        public OffhireManagementSystemController()
        {

        }

        public OffhireManagementSystemController(IOffhireFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        //================================================================================

        public PageResultDto<OffhireManagementSystemDto> Get(long companyId, long vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            return this.FacadeService.GetOffhireManagementSystemPagedDataByFilter(companyId, vesselId, fromDate, toDate, pageSize, pageIndex);
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
