#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class Order
    {
        #region ctor

        public Order()
        {
        }


        public Order(string code, string description, long ownerId, long? transporterId, long? supplierId, long? receiverId, OrderTypes _orderTypeClass, DateTime orderDate, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany, States state, IEntityConfigurator<Order> orderConfigurator)
        {
            Code = code;
            Description = description;
            SupplierId = supplierId;
            OwnerId = ownerId;
            TransporterId = transporterId;
            ReceiverId = receiverId;

            OrderDate = orderDate;
            FromVesselInCompanyId = fromVesselInCompany == null ? (long?)null : fromVesselInCompany.Id;
            ToVesselInCompanyId = toVesselInCompany == null ? (long?)null : toVesselInCompany.Id;
            FromVesselInCompany = fromVesselInCompany;
            ToVesselInCompany = toVesselInCompany;

            this.OrderType = _orderTypeClass;
            State = state;
            orderConfigurator.Configure(this);
            _orderBaseType.Add(this, fromVesselInCompany, toVesselInCompany);
            ApproveWorkFlows = new List<OrderWorkflowLog>();
        }

        #endregion

        #region Properties

        private OrderTypeBase _orderBaseType;
        public long Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public long? SupplierId { get; private set; }
        public long? ReceiverId { get; private set; }
        public long? TransporterId { get; private set; }
        public long OwnerId { get; private set; }

        public OrderTypes OrderType { get; private set; }

        public DateTime OrderDate { get; private set; }

        public OrderState OrderState { get; private set; }

        public long? FromVesselInCompanyId { get; private set; }

        public long? ToVesselInCompanyId { get; private set; }

        public byte[] TimeStamp { get; set; }

        public States State { get; private set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual VesselInCompany FromVesselInCompany { get; set; }
        public virtual VesselInCompany ToVesselInCompany { get; set; }

        public virtual Company Transporter { get; set; }
        public virtual Company Receiver { get; set; }
        public virtual Company Supplier { get; set; }
        public virtual Company Owner { get; set; }
        public virtual List<OrderWorkflowLog> ApproveWorkFlows { get; private set; }

        public virtual IList<Invoice> Invoices { get; set; }

        #endregion

        #region Methods

        #region order related

        #region Operation


        #region OrderState
        public void SubmitOrder(OrderState orderState)
        {
            CheckOrderAnyOrderItem();
            State = States.Submitted;
            OrderState = orderState;
        }

        public void CloseOrder(OrderState orderState)
        {
            State = States.Closed;
            OrderState = orderState;
        }

        public void CancelOrder(OrderState orderState, IInventoryOperationDomainService inventoryOperationDomainService)
        {
            validateToNotBeReferenced();

            State = States.Cancelled;
            OrderState = orderState;
        }

        private void validateToNotBeReferenced()
        {
            if (this.OrderItems.Any(oi => oi.IsReferenced()) /*&& this.OrderType != OrderTypes.SupplyForDeliveredVessel*/)
            {
                var fuelReportsDetailIds = this.OrderItems.SelectMany(oi => oi.OrderItemBalances.Where(oib => oib.FuelReportDetailId.HasValue).Select(oib => oib.FuelReportDetailId.Value.ToString()));
                var fuelReportsDetailIdsText = fuelReportsDetailIds.Count() > 0 ? string.Format("\n\nFuel Report Detail Ids:\n{0}", string.Join(" , ", fuelReportsDetailIds)) : string.Empty;

                var invoiceIds = this.OrderItems.SelectMany(oi => oi.OrderItemBalances.Where(oib => oib.InvoiceItem != null).Select(oib => oib.InvoiceItem.InvoiceId.ToString()));
                invoiceIds = invoiceIds.Concat(this.OrderItems.SelectMany(oi => oi.OrderItemBalances.Where(oib => oib.PairingInvoiceItemId.HasValue).Select(oib => oib.PairingInvoiceItem.InvoiceId.ToString())));

                var invoiceIdsText = invoiceIds.Count() > 0 ? string.Format("\n\nInvoice Ids:\n{0}", string.Join(" , ", invoiceIds)) : string.Empty;

                throw new BusinessRuleException("", string.Format("The action is not possible because selected order is referenced.{0}{1}", fuelReportsDetailIdsText, invoiceIdsText));
            }
        }

        public void RevertBackSubmittedOrder(OrderState orderState)
        {
            validateToNotBeReferenced();

            State = States.SubmitRejected;
            OrderState = orderState;
        }

        internal void SetOrderType(OrderTypeBase type)
        {
            _orderBaseType = type;
        }

        #endregion

        public void Update(string description, OrderTypes orderType, long ownerId, long? transporterId,
            long? supplierId, long? receiverId, VesselInCompany fromVesselInCompany, VesselInCompany toVesselInCompany, DateTime orderDate, IEntityConfigurator<Order> orderConfigurator, IOrderItemDomainService orderItemDomainService)
        {
            if (OrderType != orderType)
            {

                foreach (var orderItem in OrderItems.ToList())
                {
                    orderItemDomainService.DeleteOrderItem(orderItem);
                }
            }
            OrderType = orderType;
            Description = description;
            OwnerId = ownerId;
            TransporterId = transporterId;
            ReceiverId = receiverId;
            SupplierId = supplierId;
            FromVesselInCompanyId = fromVesselInCompany == null ? (long?)null : fromVesselInCompany.Id;
            ToVesselInCompanyId = toVesselInCompany == null ? (long?)null : toVesselInCompany.Id;
            FromVesselInCompany = fromVesselInCompany;
            ToVesselInCompany = toVesselInCompany;

            OrderDate = orderDate;

            orderConfigurator.Configure(this);

            IsInOpenState();

            _orderBaseType.Update(this, fromVesselInCompany, toVesselInCompany);
        }

        #endregion

        #region BusinessRule

        public void CheckDeleteRules()
        {
            IsInOpenState();

            validateToNotBeReferenced();

            HasAnyOrderItem();
        }

        #endregion

        #endregion

        #region orderItem related

        #region Operation

        public void AddItem(OrderItem orderItem, GoodFullInfo goodFullDomainInStorageService)
        {
            //BR_PO04
            IsInOpenOrSubmitRejectedState();
            //BR_PO9
            IsOrderItemGoodUnique(orderItem.GoodId, null);

            this.CheckCommonAddAndEditItemRules(goodFullDomainInStorageService);

            this.OrderItems.Add(orderItem);
        }

        public OrderItem UpdateItem(long id, string description, decimal quantity, long goodId, long unitId,
                                    long? goodPartyAssignmentId, GoodFullInfo goodFullInfo)
        {
            //BR_PO04
            IsInOpenOrSubmitRejectedState();

            var orderItem = this.OrderItems.Single(c => c.Id == id);
            if (orderItem == null)
                throw new ObjectNotFound("Order Item", id);

            IsOrderItemGoodUnique(goodId, id);

            this.CheckCommonAddAndEditItemRules(goodFullInfo);

            orderItem.Update(description, quantity, goodId, unitId, goodFullInfo);

            return orderItem;
        }

        public void DeleteItem(OrderItem item, IOrderItemDomainService orderItemDomainService)
        {
            // Bussuiness Rules
            IsInOpenOrSubmitRejectedState();

            var orderItem = OrderItems.FirstOrDefault(c => c.Id == item.Id);
            if (orderItem == null)
                throw new ObjectNotFound("OrderItem", item.Id);

            orderItem.CheckDeleteRules();
            orderItemDomainService.DeleteOrderItem(orderItem);
        }

        #endregion

        #region BusinessRule

        private void CheckCommonAddAndEditItemRules(GoodFullInfo goodFullInfo)
        {
            // BR_17  developer Dont implement refrence in order

            //BR_PO18
            this.CanOrderThisGood(goodFullInfo);

            //BR_PO20
            this.GoodHaveValidSuplierAndTransporter(goodFullInfo);

            //ValidateGoodQuantity(orderItem, goodDomainService);

            //            //BR_PO21
            //            CanBeOrderWithReOrderLevelCheck(orderItem, goodDomainService);

            //            //BR_PO22
            //            MaxOfOrderCheck(orderItem.Quantity, orderItem, goodPartyAssignmentDomainService);
            //
            //            //BR_PO23
            //            FixOfOrderCheck(orderItem.Quantity, orderItem, goodPartyAssignmentDomainService);
        }


        //BR_PO6
        private void HasAnyOrderItem()
        {
            if (OrderItems != null && OrderItems.Count > 0)
                throw new BusinessRuleException("", "this order have  some items for Delete.");
        }

        //BR_PO7
        private void CheckOrderAnyOrderItem()
        {
            if (OrderItems == null || OrderItems.Count == 0)
                throw new BusinessRuleException("", "this order must have  some items for Submit.");
        }


        //BR_PO9
        private void IsOrderItemGoodUnique(long goodId, long? editingOrderItemId)
        {
            var q1 = OrderItems.FirstOrDefault(c => c.GoodId == goodId &&
                (!editingOrderItemId.HasValue || c.Id == editingOrderItemId.Value)
                // && c.GoodPartyAssignmentId == orderItem.GoodPartyAssignmentId
                );
            if (q1 != null)
                throw new BusinessRuleException("BR_PO9", "OrderItem Good is duplicated");
        }

        //BR_PO4
        private void IsInOpenState()
        {
            var openState = new OrderIsOpenState();
            if (!openState.IsSatisfiedBy(this))
                throw new BusinessRuleException("BR_PO4", "Order is not in Open State");
        }

        private void IsInOpenOrSubmitRejectedState()
        {
            var openState = new OrderIsOpenState();
            var submitRejectedState = new OrderIsSubmitRejectedState();
            if (!openState.IsSatisfiedBy(this) && !submitRejectedState.IsSatisfiedBy(this) )
                throw new BusinessRuleException("BR_PO4", "Order is not in Open or Reverted State");
        }

        //        //BR_PO10
        //        private void IsNotOnMiddleApprovedStates()
        //        {
        //            var isMiddleApprovedState = new IsOrderInMiddleApprovedState();
        //            if (isMiddleApprovedState.IsSatisfiedBy(this))
        //                throw new BusinessRuleException("BR_PO10", "Order in Middle Approve State");
        //        }
        //
        //        //BR_PO13
        //        private void IsOnMiddleApprovedStates()
        //        {
        //            var isMiddleApprovedState = new IsOrderInMiddleApprovedState();
        //            if (isMiddleApprovedState.IsSatisfiedBy(this))
        //                throw new BusinessRuleException("BR_PO10", "Order in Middle Approve State");
        //        }

        //BR_PO18
        private void CanOrderThisGood(GoodFullInfo goodFullInfo)
        {
            if (!goodFullInfo.CanBeOrderedThisGood)
                throw new BusinessRuleException("BR_PO18", "Good Order Cannot Be Ordered.");
        }


        //BR_PO20
        private void GoodHaveValidSuplierAndTransporter(GoodFullInfo goodFullInfo)
        {
            _orderBaseType.ValidateGoodSuplierAndTransporter(this, goodFullInfo);
        }

        #endregion

        #endregion

        #endregion

        public void UpdateItemInvoicedQuantity(long orderItemId, decimal quantity)
        {
            var orderItem = OrderItems.SingleOrDefault(c => c.Id == orderItemId);
            if (orderItem == null)
                throw new ObjectNotFound("OrderItem", orderItemId);
            orderItem.UpdateInvoiced(quantity);
        }

        public void Configure(OrderState orderState)
        {
            this.OrderState = orderState;
        }
    }
}