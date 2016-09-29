using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MITD.Core;
using MITD.Core.Mapping;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs;
using MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts;


namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory
{
    public class OrderToDtoMapper : BaseFacadeMapper<Order, OrderDto>, IOrderToDtoMapper
    {
        #region props

        private IOrderItemToDtoMapper OrderItemToDtoMapper { get; set; }

        private IVesselToVesselDtoMapper VesselMapper { get; set; }

        #endregion

        #region ctor

        public OrderToDtoMapper()
        {
            this.VesselMapper = ServiceLocator.Current.GetInstance<IVesselToVesselDtoMapper>();
            this.OrderItemToDtoMapper = ServiceLocator.Current.GetInstance<IOrderItemToDtoMapper>();
        }

        public OrderToDtoMapper(IVesselToVesselDtoMapper vesselMapper,
                                     IOrderItemToDtoMapper orderItemToDtoMapper)
        {
            this.VesselMapper = vesselMapper;
            this.OrderItemToDtoMapper = orderItemToDtoMapper;
        }

        #endregion

        #region methods


        public override OrderDto MapToModel(Order order)
        {
            var dto = new OrderDto();
            var orderDto = (OrderDto)base.Map(dto, order);

            //ToDO : OrderDate shoulde Map 
            orderDto.OrderDate = order.OrderDate;



            if (order.FromVesselInCompany != null)
                orderDto.FromVessel = base.Map(new VesselDto(), order.FromVesselInCompany) as VesselDto;
            else
                orderDto.FromVessel = new VesselDto();

            if (order.ToVesselInCompany != null)
                orderDto.ToVessel = base.Map(new VesselDto(), order.ToVesselInCompany) as VesselDto;
            else
                orderDto.ToVessel = new VesselDto();

            //            TODO: <M.A> Review
            //            if (order.CurrentApproveFlowId != null)
            //            {
            //               // orderDto.UserInChargName = order.CurrentApproveWorkFlowConfig.ActorUser.Name;
            //            }

            orderDto.UserInChargName = order.ApproveWorkFlows.Last().ActorUser.Name;
            //orderDto.UserInChargName = order.ApproveWorkFlows.Last().CurrentWorkflowStep.ActorUser.Name;
            orderDto.CurrentStateName = order.ApproveWorkFlows.Last().CurrentWorkflowStep.CurrentWorkflowStage.ToString();

            orderDto.ApproveStatus = WorkflowStagesToDto(order.ApproveWorkFlows.Last().CurrentWorkflowStep.CurrentWorkflowStage);



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
            return orderDto;
        }

        public override IEnumerable<OrderDto> MapToModel(IEnumerable<Order> entities)
        {
            return entities.Select(MapToModel);
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
                case OrderTypeEnum.PurchaseWithTransfer:
                    return OrderTypes.PurchaseWithTransfer;
                case OrderTypeEnum.InternalTransfer:
                    return OrderTypes.InternalTransfer;
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
                case OrderTypes.PurchaseWithTransfer:
                    return OrderTypeEnum.PurchaseWithTransfer;
                case OrderTypes.InternalTransfer:
                    return OrderTypeEnum.InternalTransfer;
                default:
                    throw new ArgumentOutOfRangeException("OrderTypes");
            }
        }
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

        #endregion
    }
}