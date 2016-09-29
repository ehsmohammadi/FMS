using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public   class InvoiceEffectiveFactorController : ApiController
    {

        #region props
        private IInvoiceFacadeService FacadeService { get; set; }
        #endregion

        public InvoiceEffectiveFactorController(IInvoiceFacadeService facadeService)
            : base()
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }
        public IEnumerable<EffectiveFactorDto> Get()
        {
            var result = this.FacadeService.GetAllEffectiveFactors();
            return result;
        }

      


    }
}