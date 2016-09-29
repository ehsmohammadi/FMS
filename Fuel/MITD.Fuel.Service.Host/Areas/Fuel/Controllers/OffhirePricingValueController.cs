using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class OffhirePricingValueController : ApiController
    {
        private IOffhireFacadeService FacadeService { get; set; }

        #region ctor

        public OffhirePricingValueController()
        {

        }

        public OffhirePricingValueController(IOffhireFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        //================================================================================

        public List<PricingValueDto> Get(long introducerId, long vesselId, DateTime startDateTime)
        {
            var result = this.FacadeService.GetOffhirePricingValuesInMainCurrency(introducerId, vesselId, startDateTime);
            return result;
        }

        //================================================================================

        #endregion
    }
}
