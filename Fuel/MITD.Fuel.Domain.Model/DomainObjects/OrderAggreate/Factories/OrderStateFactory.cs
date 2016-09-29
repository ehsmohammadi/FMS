using System.Runtime.InteropServices.WindowsRuntime;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public class OrderStateFactory:IOrderStateFactory
    {

        private readonly IInventoryOperationDomainService inventoryOperationDomainService;

        public OrderStateFactory(IInventoryOperationDomainService inventoryOperationDomainService                           
            )
        {
            this.inventoryOperationDomainService = inventoryOperationDomainService;
        }
        public  OrderState CreateSubmitState() 
        {
            return new SubmitState(inventoryOperationDomainService,  this);
        }

        public OrderState CreateOpenState()
        {
            return new OpenState(this);
        }

        public OrderState CreateCloseState()
        {
            return new CloseState();
        }

        public OrderState CreateCancelState()
        {
            return new CancelState();
        }

        public OrderState CreateSubmitRejectedState()
        {
            return new SubmitRejectedState(inventoryOperationDomainService, this);
        }
    }
}