
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
    public class FuelReportVesselFacadeService : IFuelReportVesselFacadeService
    {
        #region props

        private readonly IVesselInCompanyDomainService _domainService;
        private readonly IFacadeMapper<VesselInCompany, VesselInCompanyDto> _vesselInCompanyMapper;
        #endregion

        #region ctor

        public FuelReportVesselFacadeService(IVesselInCompanyDomainService appService,
               IFacadeMapper<VesselInCompany, VesselInCompanyDto> vesselInCompanyMapper)
        {
            this._domainService = appService;
            this._vesselInCompanyMapper = vesselInCompanyMapper;
        }

        #endregion

        #region methods

        public List<VesselInCompanyDto> GetAll()
        {
            var entities = this._domainService.GetAll();

            var dtos = this._vesselInCompanyMapper.MapToModel(entities).OrderBy(e => e.Name).ToList();

            return dtos;
        }

        #endregion
    }
}
