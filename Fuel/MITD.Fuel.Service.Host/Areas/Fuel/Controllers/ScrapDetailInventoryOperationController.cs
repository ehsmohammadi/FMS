using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class ScrapInventoryOperationController : ApiController
    {
        #region props

        private IScrapFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public ScrapInventoryOperationController()
        {

        }

        public ScrapInventoryOperationController(IScrapFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public PageResultDto<FuelReportInventoryOperationDto> Get(long id, int? pageSize = null, int? pageIndex = null)
        {
            return this.FacadeService.GetInventoryOperations(id, pageSize, pageIndex);
        }

        #endregion
    }
}
