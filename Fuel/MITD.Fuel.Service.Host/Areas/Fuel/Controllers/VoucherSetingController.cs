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
    public class VoucherSetingController : ApiController
    {
        // GET api/voucherseting

        private IVoucherSetingFacadeService _voucherSetingFacadeService;
        public VoucherSetingController(IVoucherSetingFacadeService voucherSetingFacadeService)
        {
            _voucherSetingFacadeService = voucherSetingFacadeService;
        }
        
        public PageResultDto<VoucherSetingDto> Get(long companyId, int voucherTypeId, int voucherDetailTypeId, int pageIndex, int pageSize)
        {
            return _voucherSetingFacadeService.GetByFilter(companyId, voucherTypeId, voucherDetailTypeId, pageIndex,
                pageSize);
        }

        // GET api/voucherseting/5
        public VoucherSetingDto Get(long id)
        {
            return _voucherSetingFacadeService.GetById(id);
        }

        public VoucherSetingDetailDto Get(long id,long detailId)
        {
            return _voucherSetingFacadeService.GetDetailById(id, detailId);
        }

        // POST api/voucherseting
        public void Post([FromBody]VoucherSetingDto value)
        {
            _voucherSetingFacadeService.AddVoucherSeting(value);

        }

        // PUT api/voucherseting/5
        public void Put(int id, [FromBody]VoucherSetingDto value)
        {
            _voucherSetingFacadeService.UpdateVoucherSeting(value);
        }

        // DELETE api/voucherseting/5
        public void Delete(long id, long detailId)
        {
            _voucherSetingFacadeService.UpdateDelete(id,detailId);
        }
    }
}
