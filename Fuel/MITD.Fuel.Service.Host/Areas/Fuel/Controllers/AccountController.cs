using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class AccountController : ApiController
    {
        private IAccountFacadeService _accountFacadeService;
        public AccountController(IAccountFacadeService accountFacadeService)
        {
            _accountFacadeService = accountFacadeService;
        }
        
        // GET api/account
        public PageResultDto<AccountDto> Get(string name,string code,int pageIndex,int pageSize)
        {
            return _accountFacadeService.GetAllByFilter(name, code, pageIndex, pageSize);
        }

        // GET api/account/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/account
        public void Post([FromBody]AccountDto value)
        {
            _accountFacadeService.Add(value);
        }

        // PUT api/account/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/account/5
        public void Delete(int id)
        {
        }
    }
}
