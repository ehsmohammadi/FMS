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
    public class VoyageLogController : ApiController
    {
        private IVoyageFacadeService FacadeService { get; set; }

        public VoyageLogController()
        {

        }

        public VoyageLogController(IVoyageFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #region Actions

        public PageResultDto<VoyageLogDto> Get(long voyageId, int pageSize, int pageIndex)
        {
            var data = this.FacadeService.GetChenageHistory(voyageId, pageSize, pageIndex);
            return data;
        }


        #endregion
    }
}
