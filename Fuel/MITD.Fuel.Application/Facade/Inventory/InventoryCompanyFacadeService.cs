#region

using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainServices;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

#endregion

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class InventoryCompanyFacadeService : IInventoryCompanyFacadeService
    {
        #region props

        private readonly IInventoryCompanyDomainService _companyDomainService;
        private readonly IFacadeMapper<Inventory_Company, Inventory_CompanyDto> _companyMapper;
        private readonly IFacadeMapper<Inventory_Warehouse, Inventory_WarehouseDto> _warehouseMapper;

        #endregion

        #region ctor

        public InventoryCompanyFacadeService(
            IInventoryCompanyDomainService companyDomainService,
            IFacadeMapper<Inventory_Company, Inventory_CompanyDto> companyMapper,
            IFacadeMapper<Inventory_Warehouse, Inventory_WarehouseDto> warehouseMapper)
        {
            _companyDomainService = companyDomainService;
            _companyMapper = companyMapper;
            _warehouseMapper = warehouseMapper;
        }

        #endregion

        #region methods

        public Inventory_CompanyDto Get(long id)
        {
            var company = _companyDomainService.Get(id);
            var dtos = _companyMapper.MapToModel(company);
            return dtos;
        }

        public List<Inventory_CompanyDto> GetByCurrentUser()
        {
            var companyEntities = this._companyDomainService.GetCurrentUserCompanies();

            var result = new List<Inventory_CompanyDto>();
            foreach (var ent in companyEntities)
            {
                var dto = this._companyMapper.MapToModel(ent);

                result.Add(dto);
            }

            return result.OrderBy(e=>e.Name).ToList();
        }

        public List<Inventory_CompanyDto> Get()
        {
            var company = _companyDomainService.Get();

            var dtos = _companyMapper.MapToModel(company);

            return dtos.OrderBy(e => e.Name).ToList();
            //return new PageResultDto<Inventory_CompanyDto>
            //       {
            //           Result = dtos.ToList()
            //       };
        }

        public Inventory_WarehouseDto Get(long companyId, long warehouseId)
        {
            var warehouse = _companyDomainService.Get(companyId, warehouseId);
            var dtos = _warehouseMapper.MapToModel(warehouse);
            return dtos;
        }

        public List<Inventory_WarehouseDto> GetWarehouse(long companyId)
        {
            var warehouse = _companyDomainService.GetWarehouse(companyId);

            var dtos = _warehouseMapper.MapToModel(warehouse);

            return dtos.OrderBy(e => e.Name).ToList();
            //return new PageResultDto<Inventory_WarehouseDto>
            //        {
            //            Result = dtos.ToList()
            //        };
        }

        #endregion

    }
}