using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class RecieveTypeController : ApiController
    {

        #region Prop

        private IRecieveTypeFacadeService _recieveTypeFacadeService;

        #endregion


        #region Ctor

        public RecieveTypeController(IRecieveTypeFacadeService recieveTypeFacadeService)
        {
            _recieveTypeFacadeService = recieveTypeFacadeService;
        }

        #endregion

        #region Method

        // GET api/recievetype
        public List<ReceiveTypeDto> Get()
        {
            return _recieveTypeFacadeService.GetAll();
        }

        // POST api/recievetype
        public void Post([FromBody]string value)
        {
        }

        // PUT api/recievetype/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/recievetype/5
        public void Delete(int id)
        {
        }

        #endregion

       
    }
}
