using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class SubmitRejectedState : OrderState
    {

        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
  
        private readonly IOrderStateFactory orderStateFactory;

        public SubmitRejectedState(IInventoryOperationDomainService inventoryOperationDomainService, IOrderStateFactory orderStateFactory)
        {
            this.inventoryOperationDomainService = inventoryOperationDomainService;
          
            this.orderStateFactory = orderStateFactory;
        }

        public override void ApproveOrder(Order order, long approverId)
        {
            order.SubmitOrder(orderStateFactory.CreateSubmitState());
        }
        
        public override void CancelOrder(Order order, long approverId)
        {
            order.CancelOrder(orderStateFactory.CreateCancelState(), inventoryOperationDomainService);
        }
    }
}