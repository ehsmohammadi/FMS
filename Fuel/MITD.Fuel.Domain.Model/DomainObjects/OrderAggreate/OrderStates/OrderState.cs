using System;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate
{
    public abstract class OrderState
    {
        public virtual void ApproveOrder(Order order, long approverId)
        {
            throw new InvalidStateException("Approve",string.Format("Cannot Approve {0} State",order.State.ToString()));
        }
        public virtual void RejectOrder(Order order, long approverId)
        {
            throw new InvalidStateException("Reject", string.Format("Cannot Reject {0} State", order.State.ToString()));
        }
        public virtual void CancelOrder(Order order, long approverId)
        {
            throw new InvalidStateException("Cancel", string.Format("Cannot Cancel {0} State", order.State.ToString()));
        }
        public virtual void CloseOrder(Order order, long approverId)
        {
            throw new InvalidStateException("Close", string.Format("Cannot Close {0} State", order.State.ToString()));
        }
    }
}