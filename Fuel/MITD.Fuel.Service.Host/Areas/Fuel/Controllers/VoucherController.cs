using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using MITD.Core;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class VoucherController:ApiController
    {

        private IVoucherFacadeService _voucherFacadeService;
        private readonly ISendToFinancial _sendToFinancial;


        public VoucherController(IVoucherFacadeService voucherFacadeService,ISendToFinancial sendToFinancial)
        {
            _voucherFacadeService = voucherFacadeService;
            _sendToFinancial = sendToFinancial;
        }

        // GET api/charter/5
        public VoucherDto GetById(long id)
        {
            var res = _voucherFacadeService.GetById(id);
           

            return res;
        }

        public VoucherEntityDto GetEntityByRefNo(string refNo)
        {
            var res = _voucherFacadeService.GetEntityId(refNo);
            return res;
        }

        public PageResultDto<VoucherDto> Get(long companyId, string fromDate, string toDate, int voucherTypr, string refNo, string state, int pageIndex, int pageSize)
        {
            var nuldate = new DateTime?();
            var res = 
            _voucherFacadeService.GetAll(companyId,

            string.IsNullOrEmpty(fromDate) ? nuldate : DateTime.Parse(fromDate),
           string.IsNullOrEmpty(toDate) ? nuldate : DateTime.Parse(toDate),
           voucherTypr, refNo,
                state, pageIndex, pageSize);
            
            return res;
        }

        // POST api/charter
        public void Post([FromBody]List<long> ids,string date,string code)
        {

            _sendToFinancial.Send(ids, date, code);
        }
    }
}
