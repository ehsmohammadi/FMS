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
    public class VoyageController : ApiController
    {
        private IVoyageFacadeService FacadeService { get; set; }

        public VoyageController()
        {

        }

        public VoyageController(IVoyageFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #region Actions

        public VoyageDto Get(long id)
        {
            var data = this.FacadeService.GetById(id, true, true);
            return data;

        }

        public PageResultDto<VoyageDto> Get(long companyId, long vesselId)
        {
            var data = this.FacadeService.GetByFilter(companyId, vesselId, true, true);
            var result = new PageResultDto<VoyageDto>
                         {
                             PageSize = data.Count,
                             CurrentPage = 0,
                             TotalCount = data.Count,
                             TotalPages = 1,
                             Result = data
                         };
            return result;

        }

        public PageResultDto<VoyageDto> Get(int pageSize, int pageIndex)
        {
            var data = this.FacadeService.GetPagedData(pageSize, pageIndex, true, true);
            return data;
        }

        public PageResultDto<VoyageDto> Get(long companyId, long vesselId, int pageSize, int pageIndex)
        {
            var data = this.FacadeService.GetPagedDataByFilter(companyId, vesselId, pageSize, pageIndex, true, true);
            return data;
        }

        public List<VoyageDto> Get(long companyId, long vesselId, DateTime lookingDateTime)
        {
            var data = this.FacadeService.FindVoyages(companyId, vesselId, lookingDateTime);
            return data;
        }

        [HttpPut]
        [ActionName("UpdateVoyagesFromRotationData")]
        public void PutUpdateVoyagesFromRotationData()
        {
            FacadeService.UpdateVoyagesFromRotationData();
        }

        #endregion
    }
}
