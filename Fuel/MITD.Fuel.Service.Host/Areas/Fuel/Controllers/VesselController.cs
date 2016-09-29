using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class VesselController : ApiController
    {
        #region props
        private IVesselFacadeService VesselFacadeService { get; set; }

        #endregion

        #region ctor

        public VesselController(IVesselFacadeService vesselFacadeService)
        {
            if (vesselFacadeService == null)
                throw new Exception(" facade service can not be null");

            this.VesselFacadeService = vesselFacadeService;
        }
        #endregion

        #region methods

        public PageResultDto<VesselDto> Get(int pageSize, int pageIndex)
        {
            var dtos = this.VesselFacadeService.GetPagedData(pageSize, pageIndex);
            return dtos;
        }

        public VesselDto Get(long id)
        {
            var dtos = this.VesselFacadeService.Get(id);
            return dtos;
        }

        public PageResultDto<VesselDto> Get(long? ownerId, int pageSize, int pageIndex)
        {
            var result = this.VesselFacadeService.GetPagedDataByFilter(ownerId, pageSize, pageIndex);
            return result;
        }

        public void Post([FromBody] VesselDto entity)
        {
            this.VesselFacadeService.Add(entity);
        }

        #endregion
    }
}
