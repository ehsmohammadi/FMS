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
    public class VoucherSetingDetailController : ApiController
    {
        // GET api/vouchersetingdetail
        private IVoucherSetingFacadeService _voucherSetingFacadeService;
        
        public VoucherSetingDetailController(IVoucherSetingFacadeService voucherSetingFacadeService)
        {
            _voucherSetingFacadeService = voucherSetingFacadeService;
        }
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/vouchersetingdetail/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/vouchersetingdetail
        public void Post([FromBody]VoucherSetingDetailDto value)
        {
            _voucherSetingFacadeService.AddVoucherSetingDetail(value);

        }

        // PUT api/vouchersetingdetail/5
        public void Put(int id, [FromBody]VoucherSetingDetailDto value)
        {
            _voucherSetingFacadeService.UpdateVoucherSetingDetail(value);
        }

        // DELETE api/vouchersetingdetail/5
        public void Delete(int id)
        {
        }
    }
}
