using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;
using Omu.ValueInjecter;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class InvoiceToDtoMapper : BaseFacadeMapper<Invoice, InvoiceDto>, IInvoiceToDtoMapper
    {
        private readonly IFacadeMapper<InvoiceCommand, InvoiceDto> invoiceCommandMapper;
        private readonly IFacadeMapper<InvoiceItemCommand, InvoiceItemDto> invoiceItemCommandMapper;
        private readonly IFacadeMapper<InvoiceAdditionalPrice, InvoiceAdditionalPriceDto> invoiceAdditionalPriceMapper;
        private readonly IOrderToDtoMapper orderToDtoMapper;
        //private readonly IInvoiceAdditionalPriceToDtoMapper invoiceAdditionalPriceToDtoMapper;
        private readonly IInvoiceItemToDtoMapper invoiceItemToDtoMapper;

        

        #region ctor

        public InvoiceToDtoMapper(IFacadeMapper<InvoiceCommand, InvoiceDto> invoiceCommandMapper,
                                  IFacadeMapper<InvoiceItemCommand, InvoiceItemDto> invoiceItemCommandMapper,
                                  IFacadeMapper<InvoiceAdditionalPrice, InvoiceAdditionalPriceDto> invoiceAdditionalPriceMapper,
                                  IInvoiceItemToDtoMapper invoiceItemToDtoMapper,
                                  IOrderToDtoMapper orderToDtoMapper/*,
                                  IInvoiceAdditionalPriceToDtoMapper invoiceAdditionalPriceToDtoMapper*/)
        {
            this.invoiceCommandMapper = invoiceCommandMapper;
            this.invoiceItemCommandMapper = invoiceItemCommandMapper;
            this.invoiceAdditionalPriceMapper = invoiceAdditionalPriceMapper;
            this.orderToDtoMapper = orderToDtoMapper;
            //this.invoiceAdditionalPriceToDtoMapper = invoiceAdditionalPriceToDtoMapper;
            this.invoiceItemToDtoMapper = invoiceItemToDtoMapper;
        }

        #endregion

        #region methods


        public override InvoiceDto MapToModel(Invoice invoice)
        {
            var invoiceDto = new InvoiceDto();

            //var invoiceDto = (InvoiceDto)base.Map(dto, invoice);
            invoiceDto.InjectFrom<FlatLoopValueInjection>(invoice);
            invoiceDto.DivisionMethod = MapDivisionMethodsToDivisionMethodEnum(invoice.DivisionMethod);
            if (invoice.InvoiceItems != null && invoice.InvoiceItems.Count > 0)
            {
                var list = invoiceItemToDtoMapper.MapEntityToDto(invoice.InvoiceItems).ToList();
                invoiceDto.InvoiceItems = new ObservableCollection<InvoiceItemDto>( list);
            }
            if (invoice.ApproveWorkFlows.Any())
                invoiceDto.ApproveStatus = WorkflowStagesToDto(invoice.ApproveWorkFlows.Last().CurrentWorkflowStep.CurrentWorkflowStage);

            if (invoice.InvoiceRefrence != null)
                invoiceDto.InvoiceRefrence = MapToModel(invoice.InvoiceRefrence);

            if (invoice.OrderRefrences != null && invoice.OrderRefrences.Count > 0)
                invoiceDto.OrderRefrences = new ObservableCollection<OrderDto>(orderToDtoMapper.MapToModel(invoice.OrderRefrences));

            invoiceDto.AdditionalPrices = new ObservableCollection<InvoiceAdditionalPriceDto>();
            foreach (var additionalPrice in invoice.AdditionalPrices)
            {
                var addDto = new InvoiceAdditionalPriceDto();
                addDto.InjectFrom<FlatLoopValueInjection>(additionalPrice);
                addDto.EffectiveFactorType = MapEffectiveFactorTypesToEffectiveFactorTypeEnumDto(additionalPrice.EffectiveFactor.EffectiveFactorType);
                invoiceDto.AdditionalPrices.Add(addDto);
            }
            invoiceDto.CurrencyId = invoice.CurrencyId == 0 ? invoice.Currency.Id : invoice.CurrencyId;
            invoiceDto.OwnerId = invoice.OwnerId == 0 ? invoice.Owner.Id : invoice.OwnerId;
            invoiceDto.InvoiceType = MapInvoiceTypeEntityToInvoiceTypeDto(invoice.InvoiceType);
            return invoiceDto;
        }


        //public InvoiceCommand MapModelToCommandWithAllIncludes(InvoiceDto invoiceDto)
        //{
        //    var invoiceCommand = new InvoiceCommand();

        //    invoiceCommand.InjectFrom<UnflatLoopValueInjection>(invoiceDto);
        //    invoiceCommand.DivisionMethod = MapDivisionMethodEnumToDivisionMethods(invoiceDto.DivisionMethod);
        //    invoiceCommand.InvoiceType = MapInvoiceTypeDtoToInvoiceTypeEntity(invoiceDto.InvoiceType);
        //    if (invoiceDto.InvoiceItems != null && invoiceDto.InvoiceItems.Count > 0)
        //    {
        //        var list = invoiceItemToDtoMapper.MapModelToCommand(invoiceDto.InvoiceItems);
        //        invoiceCommand.InvoiceItems = list;
        //    }
        //    if (invoiceDto.AdditionalPrices != null && invoiceDto.AdditionalPrices.Count > 0)
        //    {
        //        var list = invoiceAdditionalPriceToDtoMapper.MapModelToCommand(invoiceDto.AdditionalPrices);
        //        invoiceCommand.AdditionalPrices = list;
        //    }

        //    if (invoiceDto.InvoiceRefrence != null)
        //        invoiceCommand.InvoiceRefrenceId = invoiceDto.InvoiceRefrence.Id;

        //    if (invoiceDto.OrderRefrences != null && invoiceDto.OrderRefrences.Count > 0)
        //        invoiceCommand.OrdersRefrenceId = invoiceDto.OrderRefrences.Select(c => c.Id).ToList();

        //    return invoiceCommand;
        //}

        //private DivisionMethods MapDivisionMethodEnumToDivisionMethods(DivisionMethodEnum divisionMethod)
        //{
        //    switch (divisionMethod)
        //    {

        //        case DivisionMethodEnum.WithAmount:
        //            return DivisionMethods.WithAmount;
        //            break;
        //        case DivisionMethodEnum.WithPrice:
        //            return DivisionMethods.WithPrice;
        //            break;
        //        case DivisionMethodEnum.Direct:
        //            return DivisionMethods.Direct;
        //            break;
        //        case DivisionMethodEnum.None:
        //            return DivisionMethods.None;

        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("divisionMethod");
        //    }
        //}


        public override IEnumerable<InvoiceDto> MapToModel(IEnumerable<Invoice> entities)
        {
            return entities.Select(MapToModel);
        }

        //public InvoiceTypes MapInvoiceTypeDtoToInvoiceTypeEntity(InvoiceTypeEnum invoiceTypeEnum)
        //{
        //    switch (invoiceTypeEnum)
        //    {
        //        case InvoiceTypeEnum.Purchase:
        //            return InvoiceTypes.Purchase;
        //        case InvoiceTypeEnum.Transfer:
        //            return InvoiceTypes.Transfer;
        //        case InvoiceTypeEnum.Attach:
        //            return InvoiceTypes.Attach;
        //        default:
        //            throw new ArgumentOutOfRangeException("invoiceTypeEnum");
        //    }
        //}


        public InvoiceTypeEnum MapInvoiceTypeEntityToInvoiceTypeDto(InvoiceTypes invoiceTypes)
        {
            switch (invoiceTypes)
            {
                case InvoiceTypes.Purchase:
                    return InvoiceTypeEnum.Purchase;
                case InvoiceTypes.Transfer:
                    return InvoiceTypeEnum.Transfer;
                case InvoiceTypes.Attach:
                    return InvoiceTypeEnum.Attach;
                default:
                    throw new ArgumentOutOfRangeException("InvoiceTypes");
            }
        }

        private DivisionMethodEnum MapDivisionMethodsToDivisionMethodEnum(DivisionMethods divisionMethod)
        {

            switch (divisionMethod)
            {
                case DivisionMethods.None:
                    return DivisionMethodEnum.None;
                    break;
                case DivisionMethods.WithAmount:
                    return DivisionMethodEnum.WithAmount;
                    break;
                case DivisionMethods.WithPrice:
                    return DivisionMethodEnum.WithPrice;
                    break;
                case DivisionMethods.Direct:
                    return DivisionMethodEnum.Direct;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("divisionMethod");
            }

            //return divisionMethod.ToString();
        }

        private EffectiveFactorTypeEnum MapEffectiveFactorTypesToEffectiveFactorTypeEnumDto(EffectiveFactorTypes effectiveFactorType)
        {
            switch (effectiveFactorType)
            {
                case EffectiveFactorTypes.Decrease:
                    return EffectiveFactorTypeEnum.Decrease;
                    break;
                case EffectiveFactorTypes.Increase:
                    return EffectiveFactorTypeEnum.InCrease;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("effectiveFactorType");
            }
        }

        #endregion

        public WorkflowStageEnum WorkflowStagesToDto(WorkflowStages workflowStage)
        {
            switch (workflowStage)
            {
                //case WorkflowStages.None:
                //    return WorkflowStageEnum.None;
                //    break;
                case WorkflowStages.Initial:
                    return WorkflowStageEnum.Initial;
                    break;
                case WorkflowStages.Approved:
                    return WorkflowStageEnum.Approved;
                    break;
                case WorkflowStages.FinalApproved:
                    return WorkflowStageEnum.FinalApproved;
                    break;
                case WorkflowStages.Submited:
                    return WorkflowStageEnum.Submited;
                    break;
                case WorkflowStages.Closed:
                    return WorkflowStageEnum.Closed;
                    break;
                case WorkflowStages.Canceled:
                    return WorkflowStageEnum.Canceled;
                    break;
                case WorkflowStages.SubmitRejected:
                    return WorkflowStageEnum.SubmitRejected;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("workflowStage");
            }
        }
    }

}