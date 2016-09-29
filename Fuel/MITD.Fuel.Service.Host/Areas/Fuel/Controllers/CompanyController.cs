using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class CompanyController : ApiController
    {
        #region props

        private ICompanyFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public CompanyController()
        {
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<ICompanyFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public CompanyController(ICompanyFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public PageResultDto<CompanyDto> Get()
        {
            var data = this.FacadeService.GetAll();
            var paged = new PageResultDto<CompanyDto>
                        {
                            CurrentPage = 1,
                            PageSize = data.Count,
                            Result = data,
                            TotalCount = data.Count,
                            TotalPages = 1
                        };
            return paged;
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
            List<CompanyDto> result = filterByUser
                ? this.FacadeService.GetByCurrentUserId()
                : this.FacadeService.GetAll();
            return result;
        }

        public List<CompanyDto> Get(bool filterByUser, bool operatedVessels)
        {
            List<CompanyDto> result = this.FacadeService.GetAll(filterByUser, operatedVessels);

            return result;
        }

        #endregion
    }
}
