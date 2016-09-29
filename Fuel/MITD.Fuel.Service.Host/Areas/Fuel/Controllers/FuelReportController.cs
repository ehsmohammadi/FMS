using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class FuelReportController : ApiController
    {
        #region props

        private IFuelReportFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public FuelReportController(IFuelReportFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }
        #endregion

        #region methods

        public PageResultDto<FuelReportDto> Get(long? companyId, long? vesselInCompanyId, string vesselReportCode, DateTime? fromDate, DateTime? toDate, string fuelReportIds, string fuelReportDetailIds, int pageSize, int pageIndex)
        {
            var data = this.FacadeService.GetByFilter(companyId, vesselInCompanyId, vesselReportCode, fromDate, toDate, fuelReportIds, fuelReportDetailIds, pageSize, pageIndex, false);

            return data;
        }

        public FuelReportDto Get(long id, bool includeReferencesLookup = false)
        {
            var result = this.FacadeService.GetById(id, includeReferencesLookup);
            return result;
        }


        public ResultFuelReportDto Post([FromBody] FuelReportCommandDto entity)
        {
            return this.FacadeService.Add(entity);
        }

        //[Route("{id:long}")]
        public FuelReportDto Put(long id, [FromBody] FuelReportDto entity)
        {
            var ent = this.FacadeService.Update(entity);
            return ent;
        }

        public string Delete(long id)
        {
            this.FacadeService.Delete(id);

            return string.Empty;
        }

        //[HttpPut]
        //[Route("{id:long}/Revert")]
        //[ActionName("Revert")]
        //public void PutRevert(long id)
        //{
        //    this.FacadeService.RevertFuelReportInventoryOperations(id);
        //}

        #endregion
    }
}
