using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class RefreshFuelReportsVoyageController : ApiController
    {
        #region props

        private IFuelReportFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public RefreshFuelReportsVoyageController(IFuelReportFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }
        #endregion

        #region methods

        public object Put(long companyId, long? vesselInCompanyId)
        {
            this.FacadeService.RefreshFuelReportsVoyage(companyId, vesselInCompanyId);

            return new object();
        }

        #endregion
    }
}
