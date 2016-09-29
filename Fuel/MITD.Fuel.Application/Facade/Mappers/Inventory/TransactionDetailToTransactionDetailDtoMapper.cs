using System;
using System.Linq;
using System.Collections.Generic;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
using MITD.Core;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class TransactionDetailToTransactionDetailDtoMapper : BaseFacadeMapper<Inventory_TransactionItem, Inventory_TransactionDetailDto>, ITransactionDetailToTransactionDetailDtoMapper
    {
               //private readonly ITransactionToTransactionDtoMapper transactionToTransactionDtoMapper ;
               private readonly IFacadeMapper<Inventory_User, Inventory_UserDto> userDtoMapper;
               private readonly IFacadeMapper<Inventory_Good, Inventory_GoodDto> goodDtoMapper;
               private readonly IFacadeMapper<Inventory_Unit, Inventory_UnitDto> unitDtoMapper;
               private ITransactionDetailPriceToTransactionDetailPriceDtoMapper transactionDetailPriceMapper;

        public TransactionDetailToTransactionDetailDtoMapper(
            ITransactionDetailPriceToTransactionDetailPriceDtoMapper transactionDetailPriceMapper,
            IFacadeMapper<Inventory_User, Inventory_UserDto> userDtoMapper,
            IFacadeMapper<Inventory_Good, Inventory_GoodDto> goodDtoMapper,
            IFacadeMapper<Inventory_Unit, Inventory_UnitDto> unitDtoMapper)
        {
            this.transactionDetailPriceMapper = transactionDetailPriceMapper;
            this.goodDtoMapper = goodDtoMapper;
            this.unitDtoMapper = unitDtoMapper;
            this.userDtoMapper = userDtoMapper;
        }

        public override Inventory_TransactionDetailDto MapToModel(Inventory_TransactionItem entity)
        {
            //this.transactionDetailPriceMapper = ServiceLocator.Current.GetInstance<ITransactionDetailPriceToTransactionDetailPriceDtoMapper>(); ;

            var dto = base.MapToModel(entity);
            //dto.Transaction = transactionToTransactionDtoMapper.MapToModel(entity.Inventory_Transaction);
            dto.Inventory_TransactionDetailPrice =
                transactionDetailPriceMapper.MapToModel(entity.Inventory_TransactionItemPrice, true).ToList();
            dto.Good = goodDtoMapper.MapToModel(entity.Inventory_Good);
            dto.UserCreator = userDtoMapper.MapToModel(entity.Inventory_User);
            dto.QuantityUnit = unitDtoMapper.MapToModel(entity.Inventory_Unit);

            return dto;
        }

    }

}