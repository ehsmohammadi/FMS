
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using MITD.Fuel.Presentation.Contracts.FacadeServices.Fuel;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class FuelReportCompanyFacadeService : IFuelReportCompanyFacadeService
    {
        #region props

        private readonly ICompanyDomainService companyDomainService;
        private readonly IFacadeMapper<Company, CompanyDto> companyMapper;
        private readonly IVesselInCompanyDomainService vesselDomainService;
        private readonly IFacadeMapper<VesselInCompany, VesselInCompanyDto> vesselInCompanyMapper;

        private readonly IFuelUserDomainService fuelUserDomainService;
        #endregion

        #region ctor

        public FuelReportCompanyFacadeService(
            ICompanyDomainService companyDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            IFacadeMapper<Company, CompanyDto> companyMapper,
            IFacadeMapper<VesselInCompany, VesselInCompanyDto> vesselInCompanyMapper, IFuelUserDomainService fuelUserDomainService)
        {
            this.companyDomainService = companyDomainService;
            this.companyMapper = companyMapper;
            this.vesselDomainService = vesselDomainService;
            this.vesselInCompanyMapper = vesselInCompanyMapper;
            this.fuelUserDomainService = fuelUserDomainService;
        }

        #endregion

        #region methods

        public List<CompanyDto> GetAll()
        {
            var companyEntities = this.companyDomainService.GetAll();
            //var enterprisePartiesVesselInCompany = companyEntities.SelectMany(e => e.VesselsOperationInCompany).Distinct().ToList();
            ////var vessels = this.vesselDomainService.Get(enterprisePartiesVesselInCompany.Select(v => v.Id).ToList());

            var result = new List<CompanyDto>();
            foreach (var ent in companyEntities)
            {
                //var entVessels = vessels.Where(v => ent.VesselsOperationInCompany.Any(item => item.Id == v.Id)).ToList();
                var dto = this.companyMapper.MapToModel(ent);
                var dtoVessels = this.vesselInCompanyMapper.MapToModel(ent.VesselsOperationInCompany).OrderBy(e => e.Name).ToList();

                dto.VesselInCompanies.AddRange(dtoVessels);

                result.Add(dto);
            }
            return result.OrderBy(e => e.Name).ToList();

        }

        #endregion

        //public CompanyDto Add(CompanyDto data)
        //{
        //    return null;
        //}

        //public CompanyDto Update(CompanyDto data)
        //{
        //    return null;
        //}

        //public void Delete(CompanyDto data)
        //{
        //}

        //public CompanyDto GetById(int id)
        //{
        //    return null;
        //}

        //public PageResultDto<CompanyDto> GetAll(int pageSize, int pageIndex)
        //{
        //    return null;
        //}

        //public void DeleteById(int id)
        //{
        //}

        public List<CompanyDto> GetByUserId(long userId)
        {
            var companyEntities = this.companyDomainService.GetUserCompanies(userId);

            var result = new List<CompanyDto>();
            foreach (var ent in companyEntities)
            {
                var dto = this.companyMapper.MapToModel(ent);
                var dtoVessels = this.vesselInCompanyMapper.MapToModel(ent.VesselsOperationInCompany).OrderBy(e => e.Name).ToList();

                dto.VesselInCompanies.AddRange(dtoVessels);

                result.Add(dto);
            }

            return result.OrderBy(e=>e.Name).ToList();
        }
        
        public List<CompanyDto> GetByCurrentUserId()
        {
            var companyEntities = this.companyDomainService.GetUserCompanies(fuelUserDomainService.GetCurrentFuelUserId());

            var result = new List<CompanyDto>();
            foreach (var ent in companyEntities)
            {
                var dto = this.companyMapper.MapToModel(ent);
                var dtoVessels = this.vesselInCompanyMapper.MapToModel(ent.VesselsOperationInCompany).OrderBy(e => e.Name).ToList();

                dto.VesselInCompanies.AddRange(dtoVessels);

                result.Add(dto);
            }

            return result.OrderBy(e => e.Name).ToList();
        }
    }
}
