using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class FuelReportCompanyController : ApiController
    {
        #region props
        private IFuelReportCompanyFacadeService FacadeService { get; set; }
        #endregion

        #region ctor
        public FuelReportCompanyController(IFuelReportCompanyFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }
        #endregion

        #region methods

        public List<CompanyDto> Get()
        {
            List<CompanyDto> result = this.FacadeService.GetAll();
            return result;

        }

        //public PageResultDto<CompanyDto> Get(int pageSize, int pageIndex)
        //{
        //    var data = this.FacadeService.GetAll(pageSize, pageIndex);
        //    return data;
        //}

        //public CompanyDto Get(int id)
        //{
        //    var result = this.FacadeService.GetById(id);
        //    return result;
        //}

        public List<CompanyDto> Get(long userId)
        {
            List<CompanyDto> result = this.FacadeService.GetByUserId(userId);
            return result;

        }

        public List<CompanyDto> Get(bool filterByUser)
        {
            List<CompanyDto> result = filterByUser ? this.FacadeService.GetByCurrentUserId() : this.FacadeService.GetAll();
            return result;

        }

        #endregion
    }
}
