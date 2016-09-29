#region

using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;


#endregion

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class CompanyFacadeService : ICompanyFacadeService
    {
        #region props

        private readonly ICompanyDomainService companyDomainService;
        private readonly IVesselInCompanyDomainService vesselDomainService;
        private readonly IFacadeMapper<Company, CompanyDto> companyMapper;
        private readonly IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyMapper;

        private readonly IFuelUserDomainService fuelUserDomainService;

        #endregion

        #region ctor

        public CompanyFacadeService(
            ICompanyDomainService companyDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            IFacadeMapper<Company, CompanyDto> companyMapper, IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyMapper, IFuelUserDomainService fuelUserDomainService)
        {
            this.companyDomainService = companyDomainService;
            this.vesselDomainService = vesselDomainService;
            this.companyMapper = companyMapper;
            this.vesselInCompanyMapper = vesselInCompanyMapper;
            this.fuelUserDomainService = fuelUserDomainService;
        }

        #endregion

        #region methods

        public List<CompanyDto> GetAll()
        {
            //  var entities = _companyDomainService.GetAll();
            //  var dtos =new List<CompanyDto>();
            //       entities.ForEach(c =>
            //                        {
            //                            var dto = new CompanyDto()
            //                                      {
            //                                  Id=c.Id,
            //                                  Code=c.Code,
            //                                  Name=c.Name

            //                                      };
            //                            dtos.Add(dto);
            //                        });

            ////  var dtos = _mapper.MapToModel(entities).ToList();

            //  return dtos;

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

        public PageResultDto<VesselInCompanyDto> GetOwnedVessels(long companyId)
        {
            var ownedVessels = vesselDomainService.GetOwnedVessels(companyId);

            var dtos = vesselInCompanyMapper.MapToModel(ownedVessels);

            return new PageResultDto<VesselInCompanyDto>
                    {
                        Result = dtos.ToList()
                    };
        }

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

            return result.OrderBy(e => e.Name).ToList();
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

        public List<CompanyDto> GetAll(bool byCurrentUserId, bool operatedVessels)
        {
            var companyEntities = byCurrentUserId
                ? this.companyDomainService.GetUserCompanies(fuelUserDomainService.GetCurrentFuelUserId())
                : this.companyDomainService.GetAll();

            var fuelReportRepository = ServiceLocator.Current.GetInstance<IFuelReportRepository>();
            var charterInRepository = ServiceLocator.Current.GetInstance<ICharterInRepository>();

            var result = new List<CompanyDto>();
            foreach (var ent in companyEntities)
            {
                var dto = this.companyMapper.MapToModel(ent);

                var dtoVessels = new List<VesselInCompanyDto>();

                var vesselsToMap = new List<VesselInCompany>();

                if (operatedVessels)
                {
                    //var vesselsToMap = ent.VesselsOperationInCompany.Join(fuelReportRepository.Find(fr => fr.VesselInCompany.CompanyId == ent.Id).Select(frvic => frvic.VesselInCompanyId), vic => vic.Id, frvicId => frvicId, (vic, frvicId) => vic);

                    vesselsToMap.AddRange(ent.VesselsOperationInCompany.Where(vic => vic.Vessel.OwnerId == ent.Id));
                    vesselsToMap.AddRange(ent.VesselsOperationInCompany.Where(vic=>charterInRepository.Count(ch=>ch.ChartererId == ent.Id && ch.VesselInCompanyId == vic.Id) > 0));

                    dtoVessels = this.vesselInCompanyMapper.MapToModel(vesselsToMap.Distinct()).OrderBy(e => e.Name).ToList();
                }
                else
                {
                    dtoVessels = this.vesselInCompanyMapper.MapToModel(ent.VesselsOperationInCompany).OrderBy(e => e.Name).ToList();
                }


                dto.VesselInCompanies.AddRange(dtoVessels);

                result.Add(dto);
            }
            return result.OrderBy(e => e.Name).ToList();
        }

        #endregion

    }
}