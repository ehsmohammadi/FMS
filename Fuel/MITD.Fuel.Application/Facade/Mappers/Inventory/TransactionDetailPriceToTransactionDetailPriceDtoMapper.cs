using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Castle.Core.Internal;
using Castle.MicroKernel.Registration;
using MITD.Core;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.Enums.Inventory;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class TransactionDetailPriceToTransactionDetailPriceDtoMapper : BaseFacadeMapper<Inventory_TransactionItemPrice, Inventory_TransactionDetailPriceDto>, ITransactionDetailPriceToTransactionDetailPriceDtoMapper
    {
        private  ITransactionDetailToTransactionDetailDtoMapper transactionDetailMapper;
               private readonly IFacadeMapper<Inventory_User, Inventory_UserDto> userDtoMapper;
//               private readonly IFacadeMapper<Inventory_Good, Inventory_GoodDto> goodDtoMapper;
               private readonly IFacadeMapper<Inventory_Unit, Inventory_UnitDto> unitDtoMapper;
               public TransactionDetailPriceToTransactionDetailPriceDtoMapper(
            //ITransactionToTransactionDtoMapper transactionToTransactionDtoMapper,
            IFacadeMapper<Inventory_User, Inventory_UserDto> userDtoMapper,
            IFacadeMapper<Inventory_Unit, Inventory_UnitDto> unitDtoMapper)
        {
            this.unitDtoMapper = unitDtoMapper;
            this.userDtoMapper = userDtoMapper;
        }

        public override Inventory_TransactionDetailPriceDto MapToModel(Inventory_TransactionItemPrice entity)
        {
            //this.transactionDetailMapper = ServiceLocator.Current.GetInstance<ITransactionDetailToTransactionDetailDtoMapper>();

            var dto = base.MapToModel(entity);

            //dto.TransactionDetail = transactionDetailMapper.MapToModel(entity.Inventory_TransactionItem);
            //dto.TransactionDetail.Good = goodDtoMapper.MapToModel(entity.Inventory_TransactionItem.Inventory_Good);
            dto.UserCreator = userDtoMapper.MapToModel(entity.Inventory_User);
            dto.MainCurrencyUnit = unitDtoMapper.MapToModel(entity.Inventory_Unit_MainCurrencyUnit);
            dto.PriceUnit = unitDtoMapper.MapToModel(entity.Inventory_Unit_PriceUnit);
            dto.QuantityUnit = unitDtoMapper.MapToModel(entity.Inventory_Unit_QuantityUnit);

            return dto;
        }

        public IEnumerable<Inventory_TransactionDetailPriceDto> MapToModel(IEnumerable<Inventory_TransactionItemPrice> entities, bool includePricingReferences)
        {
            var result = base.MapToModel(entities);

            if (includePricingReferences)
            {
                var operationReferenceRepository = ServiceLocator.Current.GetInstance<IRepository<Inventory_OperationReference>>();

                result.ForEach(r =>{
                    var operationReference = operationReferenceRepository.Single(or => or.OperationType == (int)InventoryOperationType.Pricing && or.OperationId == r.Id);

                    if (operationReference != null)
                    {
                        r.PricingReferenceType = operationReference.ReferenceType;
                        r.PricingReferenceNumber = operationReference.ReferenceNumber;
                    }
                });

            }

            return result;
        }
    }

}