using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class ScrapController : ApiController
    {
        #region props

        private IScrapFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public ScrapController()
        {

        }

        public ScrapController(IScrapFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public ScrapDto Get(long id)
        {
            var result = this.FacadeService.GetById(id);
            return result;
        }

        public PageResultDto<ScrapDto> Get(long? companyId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var result = this.FacadeService.GetPagedDataByFilter(companyId, fromDate, toDate, pageSize, pageIndex);
            return result;
        }

        public ScrapDto Post([FromBody] ScrapDto dto)
        {
            var result = this.FacadeService.ScrapVessel(dto);
            return result;
        }

        public ScrapDto Put(long id, [FromBody] ScrapDto dto)
        {
            var result = this.FacadeService.Update(dto);
            return result;
        }

        public void Delete(long id)
        {
            this.FacadeService.Delete(id);
        }

        #endregion
    }
}
