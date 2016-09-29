using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Facade;
using Omu.ValueInjecter;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class InvoiceToDtoMapper : BaseFacadeMapper<Invoice, InvoiceDto>, IInvoiceToDtoMapper
    {
        private readonly IFacadeMapper<InvoiceCommand, InvoiceDto> invoiceCommandMapper;
        private readonly IFacadeMapper<InvoiceItemCommand, InvoiceItemDto> invoiceItemCommandMapper;
        private readonly IFacadeMapper<InvoiceAdditionalPrice, InvoiceAdditionalPriceDto> invoiceAdditionalPriceMapper;
        private readonly IOrderToDtoMapper orderToDtoMapper;
        private readonly IInvoiceAdditionalPriceToDtoMapper invoiceAdditionalPriceToDtoMapper;
        private readonly IInvoiceItemToDtoMapper invoiceItemToDtoMapper;

        private readonly IRepository<FuelReportDetail> fuelReportDetailRepository;

        #region ctor

        //        public InvoiceToDtoMapper()
        //        {
        //            this.invoiceItemToDtoMapper = ServiceLocator.Current.GetInstance<IInvoiceItemToDtoMapper>();
        //            this.orderToDtoMapper = ServiceLocator.Current.GetInstance<IOrderToDtoMapper>();
        //        }

        public InvoiceToDtoMapper(IFacadeMapper<InvoiceCommand, InvoiceDto> invoiceCommandMapper,
                                  IFacadeMapper<InvoiceItemCommand, InvoiceItemDto> invoiceItemCommandMapper,
                                  IFacadeMapper<InvoiceAdditionalPrice, InvoiceAdditionalPriceDto> invoiceAdditionalPriceMapper,
                                  IInvoiceItemToDtoMapper invoiceItemToDtoMapper,
                                  IOrderToDtoMapper orderToDtoMapper,
                                  IInvoiceAdditionalPriceToDtoMapper invoiceAdditionalPriceToDtoMapper, IRepository<FuelReportDetail> fuelReportDetailRepository)
        {
            this.invoiceCommandMapper = invoiceCommandMapper;
            this.invoiceItemCommandMapper = invoiceItemCommandMapper;
            this.invoiceAdditionalPriceMapper = invoiceAdditionalPriceMapper;
            this.orderToDtoMapper = orderToDtoMapper;
            this.invoiceAdditionalPriceToDtoMapper = invoiceAdditionalPriceToDtoMapper;
            this.fuelReportDetailRepository = fuelReportDetailRepository;
            this.invoiceItemToDtoMapper = invoiceItemToDtoMapper;
        }

        #endregion

        #region methods


        public InvoiceDto MapToModelWithAllIncludes(Invoice invoice)
        {
            var invoiceDto = new InvoiceDto();

            //var invoiceDto = (InvoiceDto)base.Map(dto, invoice);
            invoiceDto.InjectFrom<FlatLoopValueInjection>(invoice);
            invoiceDto.DivisionMethod = MapDivisionMethodsToDivisionMethodEnum(invoice.DivisionMethod);
            if (invoice.InvoiceItems != null && invoice.InvoiceItems.Count > 0)
            {
                var list = invoiceItemToDtoMapper.MapEntityToDto(invoice.InvoiceItems);
                invoiceDto.InvoiceItems = new ObservableCollection<InvoiceItemDto>(list);
            }
            if (invoice.ApproveWorkFlows.Any())
                invoiceDto.ApproveStatus = WorkflowStagesToDto(invoice.ApproveWorkFlows.Last(log => log.Active).CurrentWorkflowStep.CurrentWorkflowStage);

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

            var lastWFLog = invoice.ApproveWorkFlows.SingleOrDefault(log => log.Active);

            invoiceDto.CurrentStateName = invoice.State == States.Closed
                                        ? "Closed"
                                        : lastWFLog == null ?
                                            string.Empty : lastWFLog.CurrentWorkflowStep.CurrentWorkflowStage.ToString();

            invoiceDto.UserInChargName = lastWFLog == null ?
                                            string.Empty : lastWFLog.ActorUser.Name;

            var invoiceRelatedFuelReportDetails = getInvoiceRelatedFuelReportDetails(invoice);
            invoiceDto.VesselsNames = extractVesselNames(invoice);
            invoiceDto.VoyagesNumbers = extractVoyageNumber(invoice);
            invoiceDto.FuelReportsDateTimes = new ObservableCollection<DateTime>(invoiceRelatedFuelReportDetails.Select(frd => frd.FuelReport.EventDate).Distinct());
            invoiceDto.OrderNumbers = extractOrderNumber(invoice);
            invoiceDto.FuelReportDetailIds = string.Join(",", invoiceRelatedFuelReportDetails.Select(frd => frd.Id));
            invoiceDto.CurrencyName = invoice.Currency.Name;

            return invoiceDto;
        }

        private string extractVoyageNumber(Invoice invoice)
        {
            switch (invoice.InvoiceType)
            {
                case InvoiceTypes.Purchase:
                case InvoiceTypes.PurchaseOperations:
                case InvoiceTypes.SupplyForDeliveredVessel:
                    return string.Join(", ", invoice.OrderRefrences.Select(getOrderVoyageNumbers).Distinct());
                case InvoiceTypes.Attach:
                    return extractVoyageNumber(invoice.InvoiceRefrence);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string extractOrderNumber(Invoice invoice)
        {
            switch (invoice.InvoiceType)
            {
                case InvoiceTypes.Purchase:
                case InvoiceTypes.PurchaseOperations:
                case InvoiceTypes.SupplyForDeliveredVessel:
                    return string.Join(",", invoice.OrderRefrences.Select(or=>or.Code).Distinct());
                case InvoiceTypes.Attach:
                    return extractOrderNumber(invoice.InvoiceRefrence);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //private static IEnumerable<Domain.Model.DomainObjects.OrderAggreate.OrderItemBalance> getInvoiceOrderItemBalances(Invoice invoice)
        //{
        //    return invoice.OrderRefrences.Where(o => o.OrderType != OrderTypes.None)
        //                                    .SelectMany(o => o.OrderItems).SelectMany(oi => oi.OrderItemBalances);
        //}

        private string getOrderVesselNames(Order order)
        {
            switch (order.OrderType)
            {
                case OrderTypes.SupplyForDeliveredVessel:
                    return order.ToVesselInCompany.Name;
                case OrderTypes.Purchase:
                case OrderTypes.Transfer:
                case OrderTypes.PurchaseWithTransferOperations:
                case OrderTypes.InternalTransfer:
                case OrderTypes.PurchaseForVessel:

                    return order.ToVesselInCompany != null ? order.ToVesselInCompany.Name :
                        string.Join(", ",
                        fuelReportDetailRepository.Find(frd => (frd.ReceiveReference.ReferenceType == ReferenceType.Order && frd.ReceiveReference.ReferenceId == order.Id) ||
                        frd.TransferReference.ReferenceType == ReferenceType.Order && frd.TransferReference.ReferenceId == order.Id).Select(frd => frd.FuelReport.VesselInCompany.Name).Distinct());

                default:
                    return string.Empty;
            }
        }


        private string getOrderVoyageNumbers(Order order)
        {
            switch (order.OrderType)
            {
                case OrderTypes.SupplyForDeliveredVessel:
                case OrderTypes.Purchase:
                case OrderTypes.Transfer:
                case OrderTypes.PurchaseWithTransferOperations:
                case OrderTypes.InternalTransfer:
                case OrderTypes.PurchaseForVessel:
                    return
                        string.Join(", ",
                            fuelReportDetailRepository.Find(frd => (frd.ReceiveReference.ReferenceType == ReferenceType.Order && frd.ReceiveReference.ReferenceId == order.Id) ||
                            frd.TransferReference.ReferenceType == ReferenceType.Order && frd.TransferReference.ReferenceId == order.Id).Select(frd => frd.FuelReport.Voyage.VoyageNumber).Distinct());

                default:
                    return string.Empty;
            }
        }


        private IEnumerable<FuelReportDetail> getInvoiceRelatedFuelReportDetails(Invoice invoice)
        {
            //var orderIds = invoice.OrderRefrences.Select(or => or.Id);

            //return fuelReportDetailRepository.Find(frd => (frd.ReceiveReference.ReferenceType == ReferenceType.Order && orderIds.Contains(frd.ReceiveReference.ReferenceId.Value) ||
            //    frd.TransferReference.ReferenceType == ReferenceType.Order && orderIds.Contains(frd.TransferReference.ReferenceId.Value))).Select(frd => frd.FuelReport);

            //return invoice.InvoiceItems.SelectMany(item => item.OrderItemBalances).Select(oib=>oib.FuelReportDetail);

            if (invoice.State == States.Cancelled)
                return new List<FuelReportDetail>();
            else
            {
                if (invoice.InvoiceType == InvoiceTypes.Attach)
                    return invoice.InvoiceRefrence.OrderRefrences.SelectMany(o => o.OrderItems.Where(oi => invoice.InvoiceItems.Any(invItem => invItem.GoodId == oi.GoodId)).SelectMany(i => i.OrderItemOperatedQuantities.Select(q => q.FuelReportDetail))).ToList();
                else
                    return invoice.OrderRefrences.SelectMany(o => o.OrderItems.Where(oi => invoice.InvoiceItems.Any(invItem => invItem.GoodId == oi.GoodId)).SelectMany(i => i.OrderItemOperatedQuantities.Select(q => q.FuelReportDetail))).ToList();
            }
        }

        private string extractVesselNames(Invoice invoice)
        {
            switch (invoice.InvoiceType)
            {
                case InvoiceTypes.Purchase:
                case InvoiceTypes.PurchaseOperations:
                case InvoiceTypes.SupplyForDeliveredVessel:
                    return string.Join(", ", invoice.OrderRefrences.Select(getOrderVesselNames).Distinct());
                //return string.Join(", ",
                //getInvoiceOrderItemBalances(invoice).Select(oib => oib.FuelReportDetail.FuelReport.VesselInCompany.Name).Distinct());

                case InvoiceTypes.Attach:
                    return extractVesselNames(invoice.InvoiceRefrence);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public InvoiceCommand MapModelToCommandWithAllIncludes(InvoiceDto invoiceDto)
        {
            var invoiceCommand = new InvoiceCommand();

            invoiceCommand.InjectFrom<UnflatLoopValueInjection>(invoiceDto);
            invoiceCommand.DivisionMethod = MapDivisionMethodEnumToDivisionMethods(invoiceDto.DivisionMethod);
            invoiceCommand.InvoiceType = MapInvoiceTypeDtoToInvoiceTypeEntity(invoiceDto.InvoiceType);
            if (invoiceDto.InvoiceItems != null && invoiceDto.InvoiceItems.Count > 0)
            {
                var list = invoiceItemToDtoMapper.MapModelToCommand(invoiceDto.InvoiceItems);
                invoiceCommand.InvoiceItems = list;
            }
            if (invoiceDto.AdditionalPrices != null && invoiceDto.AdditionalPrices.Count > 0)
            {
                var list = invoiceAdditionalPriceToDtoMapper.MapModelToCommand(invoiceDto.AdditionalPrices);
                invoiceCommand.AdditionalPrices = list;
            }

            if (invoiceDto.InvoiceRefrence != null)
                invoiceCommand.InvoiceRefrenceId = invoiceDto.InvoiceRefrence.Id;

            if (invoiceDto.OrderRefrences != null && invoiceDto.OrderRefrences.Count > 0)
                invoiceCommand.OrdersRefrenceId = invoiceDto.OrderRefrences.Select(c => c.Id).ToList();

            return invoiceCommand;
        }

        private DivisionMethods MapDivisionMethodEnumToDivisionMethods(DivisionMethodEnum divisionMethod)
        {
            switch (divisionMethod)
            {
                case DivisionMethodEnum.WithAmount:
                    return DivisionMethods.WithAmount;
                case DivisionMethodEnum.WithPrice:
                    return DivisionMethods.WithPrice;
                case DivisionMethodEnum.Direct:
                    return DivisionMethods.Direct;
                case DivisionMethodEnum.None:
                    return DivisionMethods.None;
                default:
                    throw new ArgumentOutOfRangeException("divisionMethod");
            }
        }


        public IEnumerable<InvoiceDto> MapToModelWithAllIncludes(IEnumerable<Invoice> entities)
        {
            return entities.Select(MapToModelWithAllIncludes);
        }

        public InvoiceTypes MapInvoiceTypeDtoToInvoiceTypeEntity(InvoiceTypeEnum invoiceTypeEnum)
        {
            switch (invoiceTypeEnum)
            {
                case InvoiceTypeEnum.Purchase:
                    return InvoiceTypes.Purchase;
                case InvoiceTypeEnum.PurchaseOperations:
                    return InvoiceTypes.PurchaseOperations;
                case InvoiceTypeEnum.Attach:
                    return InvoiceTypes.Attach;
                case InvoiceTypeEnum.SupplyForDeliveredVessel:
                    return InvoiceTypes.SupplyForDeliveredVessel;
                default:
                    throw new ArgumentOutOfRangeException("invoiceTypeEnum");
            }
        }


        public InvoiceTypeEnum MapInvoiceTypeEntityToInvoiceTypeDto(InvoiceTypes invoiceTypes)
        {
            switch (invoiceTypes)
            {
                case InvoiceTypes.Purchase:
                    return InvoiceTypeEnum.Purchase;
                case InvoiceTypes.PurchaseOperations:
                    return InvoiceTypeEnum.PurchaseOperations;
                case InvoiceTypes.Attach:
                    return InvoiceTypeEnum.Attach;
                case InvoiceTypes.SupplyForDeliveredVessel:
                    return InvoiceTypeEnum.SupplyForDeliveredVessel;
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
                case DivisionMethods.WithAmount:
                    return DivisionMethodEnum.WithAmount;
                case DivisionMethods.WithPrice:
                    return DivisionMethodEnum.WithPrice;
                case DivisionMethods.Direct:
                    return DivisionMethodEnum.Direct;
                default:
                    throw new ArgumentOutOfRangeException("divisionMethod");
            }
        }

        private EffectiveFactorTypeEnum MapEffectiveFactorTypesToEffectiveFactorTypeEnumDto(EffectiveFactorTypes effectiveFactorType)
        {
            switch (effectiveFactorType)
            {
                case EffectiveFactorTypes.Decrease:
                    return EffectiveFactorTypeEnum.Decrease;
                case EffectiveFactorTypes.Increase:
                    return EffectiveFactorTypeEnum.InCrease;
                default:
                    throw new ArgumentOutOfRangeException("effectiveFactorType");
            }
        }

        #endregion

        public WorkflowStageEnum WorkflowStagesToDto(WorkflowStages workflowStage)
        {
            switch (workflowStage)
            {
                case WorkflowStages.None:
                    return WorkflowStageEnum.None;
                    break;
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