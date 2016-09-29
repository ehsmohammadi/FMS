using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class OffhireController : ApiController
    {
        private IOffhireFacadeService FacadeService { get; set; }

        #region ctor

        public OffhireController()
        {

        }

        public OffhireController(IOffhireFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        //================================================================================

        public OffhireDto Get(long id)
        {
            var result = this.FacadeService.GetById(id);
            return result;
        }

        //================================================================================

        public PageResultDto<OffhireDto> Get(long? companyId, long? vesselId, DateTime? fromDate, DateTime? toDate, int pageSize, int pageIndex)
        {
            var result = this.FacadeService.GetPagedDataByFilter(companyId, vesselId, fromDate, toDate, pageSize, pageIndex);
            return result;
        }

        //================================================================================

        public OffhireDto Post([FromBody] OffhireDto dto)
        {
            var result = this.FacadeService.Add(dto);
            return result;
        }

        //================================================================================

        public OffhireDto Put(long id, [FromBody] OffhireDto dto)
        {
            var result = this.FacadeService.Update(dto);
            return result;
        }

        //================================================================================

        public void Delete(long id)
        {
            this.FacadeService.Delete(id);
        }

        //================================================================================

        #endregion
    }
}
