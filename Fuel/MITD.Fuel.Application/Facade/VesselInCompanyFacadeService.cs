#region

using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Core;
using MITD.Fuel.Domain.Model.Repositories;

#endregion

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class VesselInCompanyFacadeService : IVesselInCompanyFacadeService
    {

        #region props
        private readonly IVesselInCompanyDomainService _vesselInCompanyDomainService;
        private readonly ICompanyDomainService _iCompanyDomainService;
        private IVesselInCompanyToVesselInCompanyDtoMapper VesselInCompanyMapper { get; set; }
        private IVesselActivationItemToVesselActivationItemDtoMapper vesselActivationItemToVesselActivationItemDtoMapper { get; set; }
        private IGoodUnitDomainService goodUnitDomainService;

        private IInventoryOperationManager inventoryOperationManager;
        private IFuelUserDomainService fuelUserDomainService;

        #endregion

        #region ctor


        public VesselInCompanyFacadeService(IVesselInCompanyDomainService vesselService,
            ICompanyDomainService iCompanyDomainService,
            IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyMapper,
            IGoodUnitDomainService goodUnitDomainService,
            IInventoryOperationManager inventoryOperationManager,
            IFuelUserDomainService fuelUserDomainService,
            IVesselActivationItemToVesselActivationItemDtoMapper vesselActivationItemToVesselActivationItemDtoMapper)
        {
            _vesselInCompanyDomainService = vesselService;
            _iCompanyDomainService = iCompanyDomainService;
            VesselInCompanyMapper = vesselInCompanyMapper;
            this.goodUnitDomainService = goodUnitDomainService;
            this.inventoryOperationManager = inventoryOperationManager;
            this.fuelUserDomainService = fuelUserDomainService;
            this.vesselActivationItemToVesselActivationItemDtoMapper = vesselActivationItemToVesselActivationItemDtoMapper;
        }

        #endregion

        #region methods

        public List<VesselInCompanyDto> GetAll(long? companyId, string vesselStates)
        {
            var entities = _vesselInCompanyDomainService.GetVesselInCompanies(companyId, vesselStates);
            //var entities = _vesselInCompanyDomainService.GetAll();
            var dtos = VesselInCompanyMapper.MapToModel(entities).OrderBy(e=>e.Name).ToList();
            return dtos;
        }

        public List<VesselInCompanyDto> GetCompanyVessels(long enterpriseId, bool operatedVessels)
        {
            var entities = _iCompanyDomainService.GetCompanyVessels(enterpriseId);

            var vesselsToMap = new List<VesselInCompany>();

            if (operatedVessels)
            {
                //var vesselsToMap = ent.VesselsOperationInCompany.Join(fuelReportRepository.Find(fr => fr.VesselInCompany.CompanyId == ent.Id).Select(frvic => frvic.VesselInCompanyId), vic => vic.Id, frvicId => frvicId, (vic, frvicId) => vic);

                var charterInRepository = ServiceLocator.Current.GetInstance<ICharterInRepository>();

                vesselsToMap.AddRange(entities.Where(vic => vic.Vessel.OwnerId == enterpriseId));
                vesselsToMap.AddRange(entities.Where(vic => charterInRepository.Count(ch => ch.ChartererId == enterpriseId && ch.VesselInCompanyId == vic.Id) > 0));

                vesselsToMap = vesselsToMap.Distinct().ToList();
            }
            else
                vesselsToMap = entities;

            var dtos = VesselInCompanyMapper.MapToModel(vesselsToMap).OrderBy(e=>e.Name).ToList();
            return dtos;
        }

        public List<VesselInCompanyDto> GetVesselInCompanies(string vesselCode)
        {
            var entities = _vesselInCompanyDomainService.GetVesselInCompanies(vesselCode);
            var dtos = VesselInCompanyMapper.MapToModel(entities).OrderBy(e=>e.Name).ToList();
            return dtos;
        }

        public List<VesselInCompanyDto> GetVesselInCompanies(long companyId, string vesselCode)
        {
            var entity = _vesselInCompanyDomainService.GetVesselInCompany(companyId, vesselCode);

            var dto = VesselInCompanyMapper.MapToModel(entity);
            dto.TankDtos = entity.Tanks.Select(t => new TankDto() { Code = t.Name, Id = t.Id }).ToList();


            var dtos = new List<VesselInCompanyDto>() {dto};
            return dtos;
        }

        public void ActivateWarehouseIncludingRecieptsOperation(string vesselCode, long companyId, DateTime activationDate, List<VesselActivationItemDto> vesselActivationItemDtos)
        {
            var vesselActivationItems = vesselActivationItemDtos.Select(va => new VesselActivationItem()
            {
                Id = va.Id,
                CurrencyCode = va.CurrencyCode,
                Fee = va.Fee,
                GoodCode = va.Good.Code,
                Rob = va.Rob,
                TankId = va.TankDto.Id,
                UnitCode = goodUnitDomainService.Get(va.Good.Unit.Id).Abbreviation,
            });

            inventoryOperationManager.ActivateWarehouseIncludingRecieptsOperation(vesselCode, companyId, activationDate, vesselActivationItems.ToList(), (int)fuelUserDomainService.GetCurrentFuelUserId());
        }

        public VesselActivationDto GetVesselActivationInfo(string vesselCode)
        {
            DateTime activationDate;
            VesselActivationDto vesselActivationDto = null;
            List<VesselActivationItem> entities = _vesselInCompanyDomainService.GetVesselActivationInfo(vesselCode, out activationDate);

            if (entities != null)
            {
                vesselActivationDto = new VesselActivationDto();
                vesselActivationDto.VesselActivationItemDtos = new List<VesselActivationItemDto>();

                vesselActivationDto.ActivationDate = activationDate;
                vesselActivationDto.VesselActivationItemDtos =
                    entities.Select(e => new VesselActivationItemDto()
                    {
                        CurrencyCode = e.CurrencyCode,
                        Fee = e.Fee,
                        Rob = e.Rob,
                        Good = new GoodDto() {Id = e.GoodId, Code = e.GoodCode, Name =  e.GoodName},
                        TankDto = new TankDto() { Id = e.TankId, Code = e.TankCode},
                        GoodUnit = new GoodUnitDto() { Id = e.UnitId, Name = e.UnitCode}
                    }).ToList();
            }
            return vesselActivationDto;
        }

        public List<VesselInCompanyDto> GetOwnedVessels(long companyId)
        {
            var entities = _vesselInCompanyDomainService.GetInactiveVessels(companyId);
            var dtos = VesselInCompanyMapper.MapToModel(entities).OrderBy(e=>e.Name).ToList();
            return dtos;
        }

        public VesselInCompanyDto GetById(long id)
        {
            var entity = _vesselInCompanyDomainService.Get(id);
            var dto = VesselInCompanyMapper.MapToModel(entity);
            dto.TankDtos = entity.Tanks.Select(t => new TankDto() { Code = t.Name, Id = t.Id }).ToList();

            return dto;
        }

        #endregion

        public List<VesselInCompanyDto> GetOwnedOrCharterInVessels(long companyId)
        {
            var entities = _vesselInCompanyDomainService.GetOwnedOrCharterInVessels(companyId);
            var dtos = VesselInCompanyMapper.MapToModel(entities).OrderBy(e=>e.Name).ToList();
            return dtos;
        }
    }
}