using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class SubmitState : OrderState
    {

        private readonly IInventoryOperationDomainService inventoryOperationDomainService;
  
        private readonly IOrderStateFactory orderStateFactory;

        public SubmitState(IInventoryOperationDomainService inventoryOperationDomainService,IOrderStateFactory orderStateFactory)
        {
            this.inventoryOperationDomainService = inventoryOperationDomainService;
          
            this.orderStateFactory = orderStateFactory;
        }

        public override void CloseOrder(Order order, long approverId)
        {
            order.CloseOrder(orderStateFactory.CreateCloseState());
        }

        public override void RejectOrder(Order order, long approverId)
        {
            order.RevertBackSubmittedOrder(orderStateFactory.CreateOpenState());
        }

        public override void CancelOrder(Order order, long approverId)
        {
            order.CancelOrder(orderStateFactory.CreateCancelState(), inventoryOperationDomainService);
        }
    }
}