using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class OffhireDetailController : ApiController
    {
        #region props

        private IOffhireFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public OffhireDetailController()
        {

        }

        public OffhireDetailController(IOffhireFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        //================================================================================

        public PageResultDto<OffhireDetailDto> Get(long id, int pageSize, int pageIndex)
        {
            return this.FacadeService.GetPagedOffhireDetailData(id, pageSize, pageIndex);
        }

        //================================================================================

        public OffhireDetailDto Get(long id, long detailId)
        {
            return this.FacadeService.GetOffhireDetail(id, detailId);
        }

        //================================================================================

        //public OffhireDetailDto Post(long id, long detailId, [FromBody] OffhireDetailDto dto)
        //{
        //    return this.FacadeService.AddOffhireDetail(dto);
        //}

        ////================================================================================

        //public OffhireDetailDto Put(long id, long detailId, [FromBody] OffhireDetailDto dto)
        //{
        //    return this.FacadeService.UpdateOffhireDetail(dto);
        //}

        ////================================================================================

        //public void Delete(long id, long detailId)
        //{
        //    FacadeService.DeleteOffhireDetail(id, detailId);
        //}

        //================================================================================

        #endregion
    }
}
