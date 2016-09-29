using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class CharterVesselController : ApiController
    {

        #region Prop

        private IVesselInCompanyFacadeService _vesselInCompanyFacadeService;

        #endregion

        #region Ctor

        public CharterVesselController(IVesselInCompanyFacadeService vesselInCompanyFacadeService)
        {
            this._vesselInCompanyFacadeService = vesselInCompanyFacadeService;
        }
        #endregion

        #region Method

        public PageResultDto<VesselInCompanyDto> Get(CharterType charterType, long companyId)
        {
            var res = new PageResultDto<VesselInCompanyDto>();
            if (charterType == CharterType.In)
            {
                res.Result = this._vesselInCompanyFacadeService.GetOwnedVessels(companyId) as IList<VesselInCompanyDto>;
            }
            else
            {
                // todo bzcomment
                res.Result = this._vesselInCompanyFacadeService.GetOwnedOrCharterInVessels(companyId) as IList<VesselInCompanyDto>;
            }
            return res;

        }

        public VesselInCompanyDto GetById(CharterType charterType,  long id,bool flag)
        {
            var res = new VesselInCompanyDto();
            if (charterType == CharterType.In)
            {
                res = this._vesselInCompanyFacadeService.GetById(id);
            }
            else
            {
               
                res = this._vesselInCompanyFacadeService.GetById(id);
            }
            return res;

        }

        // POST api/chartervessel
        public void Post([FromBody]string value)
        {
        }

        // PUT api/chartervessel/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/chartervessel/5
        public void Delete(int id)
        {
        }

        #endregion


       
    }
}
