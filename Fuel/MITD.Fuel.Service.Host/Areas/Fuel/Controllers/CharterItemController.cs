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
    public class CharterItemController : ApiController
    {
        #region Prop

        private ICharterInFacadeService _charterInFacadeService;
        private ICharterOutFacadeService _charterOutFacadeService;

        #endregion

        #region Ctor

        public CharterItemController(ICharterInFacadeService charterInFacadeService,ICharterOutFacadeService charterOutFacadeService)
        {
            this._charterInFacadeService = charterInFacadeService;
            this._charterOutFacadeService = charterOutFacadeService;
        }

        #endregion

        #region Method

        // GET api/charteritem
        public PageResultDto<CharterItemDto> Get(long id,CharterType charterType)
        {
            var res = new PageResultDto<CharterItemDto>();
            if (charterType==CharterType.In)
            {
                res= this._charterInFacadeService.GetAllItem(id,0,0);
            }
            else
            {
                res = this._charterOutFacadeService.GetAllItem(id, 0, 0);
            }

            return res;
        }

        public CharterItemDto GetById(long id, long charterItemId, CharterType charterType)
        {
            var res = new CharterItemDto();
            if (charterType==CharterType.In)
            {
                res= this._charterInFacadeService.GetItemById(id,charterItemId);
            }
            else
            {
                res = this._charterOutFacadeService.GetItemById(id, charterItemId);
            }

            return res;
        }


     

        // POST api/charteritem
        public CharterItemDto Post(long id, CharterType charterType, [FromBody]CharterItemDto value)
        {
            var res = new CharterItemDto();
            if (charterType == CharterType.In)
            {
                this._charterInFacadeService.AddItem(value);
            }
            else
            {
                this._charterOutFacadeService.AddItem(value);
            }

            return res;
        }

        // PUT api/charteritem/5
        public CharterItemDto Put(int id, long charterItemId, CharterType charterType, [FromBody]CharterItemDto value)
        {
            var res = new CharterItemDto();
            if (charterType == CharterType.In)
            {
                this._charterInFacadeService.UpdateItem(value);
            }
            else
            {
                 this._charterOutFacadeService.UpdateItem(value);
            }

            return res;
        }

        // DELETE api/charteritem/5
        public void Delete(CharterType charterType, int id,int charterItemId)
        {
            if (charterType == CharterType.In)
            {
                this._charterInFacadeService.DeleteItem(id, charterItemId);
            }
            else
            {
                this._charterOutFacadeService.DeleteItem(id, charterItemId);
            }
        }

        #endregion


      
    }
}
