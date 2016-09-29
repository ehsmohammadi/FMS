using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class TransactionToTransactionDtoMapper : BaseFacadeMapper<Inventory_Transaction, Inventory_TransactionDto>,
        ITransactionToTransactionDtoMapper
    {
        private readonly IFacadeMapper<Inventory_Company, Inventory_CompanyDto> companyDtoMapper;
        private ITransactionDetailToTransactionDetailDtoMapper transactionDetailMapper;
        private readonly IFacadeMapper<Inventory_User, Inventory_UserDto> userDtoMapper;
        private readonly IFacadeMapper<Inventory_StoreType, Inventory_StoreTypeDto> storeTypeDtoMapper;
        private readonly IFacadeMapper<Inventory_Warehouse, Inventory_WarehouseDto> warehouseDtoMapper;

        public TransactionToTransactionDtoMapper(
            IFacadeMapper<Inventory_Company, Inventory_CompanyDto> companyDtoMapper,
            IFacadeMapper<Inventory_User, Inventory_UserDto> userDtoMapper,
            IFacadeMapper<Inventory_StoreType, Inventory_StoreTypeDto> storeTypeDtoMapper,
            IFacadeMapper<Inventory_Warehouse, Inventory_WarehouseDto> warehouseDtoMapper)
        {
            this.companyDtoMapper = companyDtoMapper;
            this.userDtoMapper = userDtoMapper;
            this.storeTypeDtoMapper = storeTypeDtoMapper;
            this.warehouseDtoMapper = warehouseDtoMapper;
        }

        public override Inventory_TransactionDto MapToModel(Inventory_Transaction entity)
        {
            this.transactionDetailMapper = ServiceLocator.Current.GetInstance<ITransactionDetailToTransactionDetailDtoMapper>();

            var dto = base.MapToModel(entity);
            dto.Code = entity.Code.Value;

            dto.Inventory_TransactionDetail =
                this.transactionDetailMapper.MapToModel(entity.Inventory_TransactionItem).ToList();
            dto.StoreTypes = this.storeTypeDtoMapper.MapToModel(entity.Inventory_StoreType);
            dto.UserCreator = this.userDtoMapper.MapToModel(entity.Inventory_User);
            dto.Warehouse = this.warehouseDtoMapper.MapToModel(entity.Inventory_Warehouse);
            dto.Warehouse.Company = this.companyDtoMapper.MapToModel(entity.Inventory_Warehouse.Inventory_Company);

            return dto;
        }
    }
}