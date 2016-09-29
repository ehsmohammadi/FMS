using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class ScrapDetailController : ApiController
    {
        #region props

        private IScrapFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public ScrapDetailController()
        {

        }

        public ScrapDetailController(IScrapFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        //================================================================================

        public PageResultDto<ScrapDetailDto> Get(long id, int pageSize, int pageIndex)
        {
            return this.FacadeService.GetPagedScrapDetailData(id, pageSize, pageIndex);
        }

        //================================================================================

        public ScrapDetailDto Get(long id, long detailId)
        {
            return this.FacadeService.GetScrapDetail(id, detailId);
        }

        //================================================================================

        public ScrapDetailDto Post(long id, [FromBody] ScrapDetailDto dto)
        {
            return this.FacadeService.AddScrapDetail(dto);
        }

        //================================================================================

        public ScrapDetailDto Put(long id, long detailId, [FromBody] ScrapDetailDto dto)
        {
            return this.FacadeService.UpdateScrapDetail(dto);
        }

        //================================================================================

        public void Delete(long id, long detailId)
        {
            FacadeService.DeleteScrapDetail(id, detailId);
        }

        //================================================================================

        #endregion
    }
}
