using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class VesselInCompanyController : ApiController
    {
        #region props
        private IVesselInCompanyFacadeService VesselInCompanyFacadeService { get; set; }
        #endregion

        #region ctor

        public VesselInCompanyController(IVesselInCompanyFacadeService vesselInCompanyFacadeService)
        {
            if (vesselInCompanyFacadeService == null)
                throw new Exception(" facade service can not be null");

            this.VesselInCompanyFacadeService = vesselInCompanyFacadeService;
        }
        #endregion

        #region methods

        public List<VesselInCompanyDto> Get(long? companyId, string vesselStates)
        {
            this.ControllerContext.Request.GetQueryNameValuePairs();

            var dtos = this.VesselInCompanyFacadeService.GetAll( companyId,  vesselStates);
            return dtos;
        }

        public PageResultDto<VesselInCompanyDto> Get(long companyId, bool operatedVessels, int? pageSize = null, int? pageIndex = null)
        {
            var dtos = this.VesselInCompanyFacadeService.GetCompanyVessels(companyId, operatedVessels);

            var result = new PageResultDto<VesselInCompanyDto>
                         {
                             CurrentPage = 0,
                             PageSize = 0,
                             TotalCount = dtos.Count,
                             Result = dtos,
                             TotalPages = 0
                         };

            return result;
        }
        public PageResultDto<VesselInCompanyDto> Get(string vesselCode, bool operatedVessels, int? pageSize = null, int? pageIndex = null)
        {
            var dtos = this.VesselInCompanyFacadeService.GetVesselInCompanies(vesselCode);

            var result = new PageResultDto<VesselInCompanyDto>
            {
                CurrentPage = 0,
                PageSize = 0,
                TotalCount = dtos.Count,
                Result = dtos,
                TotalPages = 0
            };

            return result;
        }

        public PageResultDto<VesselInCompanyDto> Get(long companyId, string vesselCode,bool operatedVessels, int? pageSize = null, int? pageIndex = null)
        {
            var dtos = this.VesselInCompanyFacadeService.GetVesselInCompanies(companyId, vesselCode);

            var result = new PageResultDto<VesselInCompanyDto>
            {
                CurrentPage = 0,
                PageSize = 0,
                TotalCount = dtos.Count,
                Result = dtos,
                TotalPages = 0
            };

            return result;
        }

        public void PutActivateWarehouseIncludingRecieptsOperation(string vesselCode, long companyId, DateTime activationDate,
            List<VesselActivationItemDto> vesselActivationItemDtos)
        {
            VesselInCompanyFacadeService.ActivateWarehouseIncludingRecieptsOperation(
                vesselCode, 
                companyId,
                activationDate,
                vesselActivationItemDtos);
        }
    
        #endregion
    }
}
