using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class CharterController : ApiController
    {

        #region Prop

        private ICharterInFacadeService _charterInFacadeService;
        private ICharterOutFacadeService _charterOutFacadeService;
        #endregion

        #region Ctor

       


        public CharterController(ICharterInFacadeService charterInFacadeService, ICharterOutFacadeService charterOutFacadeService)
        {
            this._charterInFacadeService = charterInFacadeService;
            this._charterOutFacadeService = charterOutFacadeService;
        }

        #endregion


        #region Method

        // GET api/charter/5
        public CharterDto GetById(CharterType charterType, CharterStateTypeEnum charterStateTypeEnum, long id)
        {
            var res = new CharterDto();
            if (charterType == CharterType.In)
            {
                if (charterStateTypeEnum == CharterStateTypeEnum.Start)
                {
                    res = this._charterInFacadeService.GetById(id);
                }
                else
                {
                    res = this._charterInFacadeService.GetCharterEnd(id);
                }

            }
            else
            {
                if (charterStateTypeEnum == CharterStateTypeEnum.Start)
                {
                    res = this._charterOutFacadeService.GetById(id);
                }
                else
                {
                    res = this._charterOutFacadeService.GetCharterEnd(id);
                }
            }

            return res;
        }

        public CharterDto GetCharterStart(CharterType charterType,long vesselInCompanyId, long chartererId,long id)
        {
            var res = new CharterDto();
            if (charterType == CharterType.In)
            {
                res = this._charterInFacadeService.GetCharterStart(vesselInCompanyId, chartererId);
            }
            else
            {
                res = this._charterOutFacadeService.GetCharterStart(id, vesselInCompanyId, chartererId);
            }
            return res;
        }
        public PageResultDto<CharterDto> Get(CharterType charterType,long id,string startdate,string enddate, long companyId, int pageIndex, int pageSize,long vesselInCompanyId)
        {
         var nuldate = new DateTime?();
            var s = string.IsNullOrEmpty(startdate) ? nuldate : DateTime.Parse(startdate);
            var e=string.IsNullOrEmpty(enddate) ? nuldate : DateTime.Parse(enddate);
            
            var res = new PageResultDto<CharterDto>();
            if (charterType == CharterType.In)
            {
                res = this._charterInFacadeService.GetAll(vesselInCompanyId,companyId, id, s, e, pageIndex, pageSize);
            }
            else
            {
                res = this._charterOutFacadeService.GetAll(vesselInCompanyId,companyId, id, s, e, pageIndex, pageSize);
            }
            return res;
        }



        // POST api/charter
        public CharterDto Post(CharterType charterType, [FromBody]CharterDto value)
        {

            if (charterType == CharterType.In)
            {
                this._charterInFacadeService.Add(value);
            }
            else
            {
                this._charterOutFacadeService.Add(value);
            }
            return new CharterDto();
        }

        // PUT api/charter/5
        public CharterDto Put(CharterType charterType, long id, [FromBody]CharterDto value)
        {
            if (charterType == CharterType.In)
            {
                this._charterInFacadeService.Update(value);
            }
            else
            {
                this._charterOutFacadeService.Update(value);
            }
            return new CharterDto();
        }

        // DELETE api/charter/5
        public void Delete(long id, CharterType charterType)
        {
            if (charterType == CharterType.In)
            {
                this._charterInFacadeService.Delete(id);
            }
            else
            {
                this._charterOutFacadeService.Delete(id);
            }
        }

        #endregion

    }
}
