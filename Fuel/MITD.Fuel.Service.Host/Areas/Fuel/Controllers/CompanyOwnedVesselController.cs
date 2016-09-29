using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class CompanyOwnedVesselController : ApiController
    {
        private ICompanyFacadeService FacadeService { get; set; }

        public CompanyOwnedVesselController(ICompanyFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        public PageResultDto<VesselInCompanyDto> Get(long id)
        {
            return this.FacadeService.GetOwnedVessels(id);
        }
    }
}
