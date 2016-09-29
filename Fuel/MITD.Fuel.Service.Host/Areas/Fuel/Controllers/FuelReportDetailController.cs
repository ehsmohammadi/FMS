using System;
using System.Web;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class FuelReportDetailController : ApiController
    {
        #region props

        private IFuelReportFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public FuelReportDetailController()
        {
        }

        public FuelReportDetailController(IFuelReportFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public FuelReportDetailDto Get(long id, long detailId, bool includeReferencesLookup = false)
        {
            //HttpContext.Current.User.Identity.Name;

            return this.FacadeService.GetFuelReportDetailById(id, detailId, includeReferencesLookup);
        }

        public FuelReportDetailDto Put(long id, long detailId, [FromBody] FuelReportDetailDto dto)
        {
            if (dto.EnableCommercialEditing)
                return this.FacadeService.UpdateFuelReportDetail(id, dto);

            else if (dto.EnableFinancialEditing)
                //This is implemented this way to simulate authorization for financial user who is in charge of set the Correction References after Commercial submit.
                //This way, the call to update the fuel report detail (after submit of commercial) will be restricted to Financial User only.
                //The properties "EnableCommercialEditing" and "EnableFinancialEditing", are set on "GET" request by the server in fuel report facade service, method "GetFuelReportDetailById" (See "Get" method in current class).
                return this.FacadeService.UpdateFuelReportDetailByFinance(id, dto);

            return null;
        }

        #endregion
    }
}
