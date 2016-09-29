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
    public class TransferTypeController : ApiController
    {

        #region Prop

        private ITransferTypeFacadeService _transferTypeFacadeService;
        #endregion

        #region Ctor

        public TransferTypeController(ITransferTypeFacadeService transferTypeFacadeService)
        {
            this._transferTypeFacadeService = transferTypeFacadeService;
        }

        #endregion

        #region Method

        // GET api/transfertype
        public List<TransferTypeDto> Get()
        {
            return this._transferTypeFacadeService.GetAll();
        }

        // GET api/transfertype/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/transfertype
        public void Post([FromBody]string value)
        {
        }

        // PUT api/transfertype/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/transfertype/5
        public void Delete(int id)
        {
        }

        #endregion


    
    }
}
