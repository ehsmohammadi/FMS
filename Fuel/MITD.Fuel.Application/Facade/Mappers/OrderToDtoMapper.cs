using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Core;
using MITD.Core.Mapping;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Services.Facade;


namespace MITD.Fuel.Application.Facade.Mappers
{
    public class OrderToDtoMapper : BaseFacadeMapper<Order, OrderDto>, IOrderToDtoMapper
    {
        #region props

        private IOrderItemToDtoMapper OrderItemToDtoMapper { get; set; }

        private IVesselInCompanyToVesselInCompanyDtoMapper VesselInCompanyMapper { get; set; }

        #endregion

        #region ctor

        public OrderToDtoMapper()
        {
            this.VesselInCompanyMapper = ServiceLocator.Current.GetInstance<IVesselInCompanyToVesselInCompanyDtoMapper>();
            this.OrderItemToDtoMapper = ServiceLocator.Current.GetInstance<IOrderItemToDtoMapper>();
        }

        public OrderToDtoMapper(IVesselInCompanyToVesselInCompanyDtoMapper vesselInCompanyMapper,
                                     IOrderItemToDtoMapper orderItemToDtoMapper)
        {
            this.VesselInCompanyMapper = vesselInCompanyMapper;
            this.OrderItemToDtoMapper = orderItemToDtoMapper;
        }

        #endregion

        #region methods

        public OrderDto MapToModelWithAllIncludes(Order order, Action<Order, OrderDto> action)
        {
            var dto = new OrderDto();
            var orderDto = (OrderDto)base.Map(dto, order);

            //ToDO : OrderDate shoulde Map 
            orderDto.OrderDate = order.OrderDate;



            if (order.FromVesselInCompany != null)
                orderDto.FromVesselInCompany = base.Map(new VesselInCompanyDto(), order.FromVesselInCompany) as VesselInCompanyDto;
            else
                orderDto.FromVesselInCompany = new VesselInCompanyDto();

            if (order.ToVesselInCompany != null)
                orderDto.ToVesselInCompany = base.Map(new VesselInCompanyDto(), order.ToVesselInCompany) as VesselInCompanyDto;
            else
                orderDto.ToVesselInCompany = new VesselInCompanyDto();

            //            TODO: <M.A> Review
            //            if (order.CurrentApproveFlowId != null)
            //            {
            //               // orderDto.UserInChargName = order.CurrentApproveWorkFlowConfig.ActorUser.Name;
            //            }

            orderDto.UserInChargName = order.ApproveWorkFlows.Last(log => log.Active).ActorUser.Name;
            //orderDto.UserInChargName = order.ApproveWorkFlows.Last().CurrentWorkflowStep.ActorUser.Name;
            orderDto.CurrentStateName = order.ApproveWorkFlows.Last(log => log.Active).CurrentWorkflowStep.CurrentWorkflowStage.ToString();

            orderDto.ApproveStatus = WorkflowStagesToDto(order.ApproveWorkFlows.Last(log => log.Active).CurrentWorkflowStep.CurrentWorkflowStage);



            if (order.Supplier != null)
                dto.Supplier = base.Map(new CompanyDto(), order.Supplier) as CompanyDto;
            else
                dto.Supplier = new CompanyDto();
            if (order.Receiver != null)
                dto.Receiver = base.Map(new CompanyDto(), order.Receiver) as CompanyDto;
            else
                dto.Receiver = new CompanyDto();

            if (order.Transporter != null)
                dto.Transporter = base.Map(new CompanyDto(), order.Transporter) as CompanyDto;
            else
                dto.Transporter = new CompanyDto();

            if (order.Owner != null)
                dto.Owner = base.Map(new CompanyDto(), order.Owner) as CompanyDto;

            orderDto.OrderType = MapOrderTypeEntityToOrderTypeDto(order.OrderType);

            if (order.OrderItems != null && order.OrderItems.Count > 0)
            {
                var list = OrderItemToDtoMapper.MapToModel(order.OrderItems);
                dto.OrderItems = new ObservableCollection<OrderItemDto>(list);
            }

            if (action != null)
                action(order, orderDto);

            return orderDto;
        }

        public IEnumerable<OrderDto> MapToModelWithAllIncludes(IEnumerable<Order> entities, Action<Order,OrderDto> action)
        {
            return entities.Select(order => this.MapToModelWithAllIncludes(order, action));
        }

        public OrderTypes MapOrderTypeDtoToOrderTypeEntity(OrderTypeEnum orderTypeEnum)
        {
            switch (orderTypeEnum)
            {
                case OrderTypeEnum.None:
                    return OrderTypes.None;
                case OrderTypeEnum.Purchase:
                    return OrderTypes.Purchase;
                case OrderTypeEnum.Transfer:
                    return OrderTypes.Transfer;
                case OrderTypeEnum.PurchaseWithTransferOperations:
                    return OrderTypes.PurchaseWithTransferOperations;
                case OrderTypeEnum.InternalTransfer:
                    return OrderTypes.InternalTransfer;
                case OrderTypeEnum.PurchaseForVessel:
                    return OrderTypes.PurchaseForVessel;
                case OrderTypeEnum.SupplyForDeliveredVessel:
                    return OrderTypes.SupplyForDeliveredVessel;
                default:
                    throw new ArgumentOutOfRangeException("orderTypeEnum");
            }
        }

        public OrderTypeEnum MapOrderTypeEntityToOrderTypeDto(OrderTypes orderTypes)
        {
            switch (orderTypes)
            {
                case OrderTypes.Purchase:
                    return OrderTypeEnum.Purchase;
                case OrderTypes.Transfer:
                    return OrderTypeEnum.Transfer;
                case OrderTypes.PurchaseWithTransferOperations:
                    return OrderTypeEnum.PurchaseWithTransferOperations;
                case OrderTypes.InternalTransfer:
                    return OrderTypeEnum.InternalTransfer;
                case OrderTypes.PurchaseForVessel:
                    return OrderTypeEnum.PurchaseForVessel;
                case OrderTypes.SupplyForDeliveredVessel:
                    return OrderTypeEnum.SupplyForDeliveredVessel;
                default:
                    throw new ArgumentOutOfRangeException("OrderTypes");
            }
        }
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

        public OrderDto MapToModel(Order entity, Action<Order, OrderDto> action)
        {
            var dto = base.MapToModel(entity);

            if (action != null)
                action(entity, dto);

            return dto;
        }

        public IEnumerable<OrderDto> MapToModel(IEnumerable<Order> entities, Action<Order, OrderDto> action)
        {
            return entities.Select(e => this.MapToModel(e, action));
        }
        #endregion
    }
}