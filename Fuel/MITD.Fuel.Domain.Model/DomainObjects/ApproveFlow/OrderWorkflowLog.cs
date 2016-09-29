#region

using System;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class OrderWorkflowLog : WorkflowLog
    {
        public OrderWorkflowLog()
        {
        }

        public OrderWorkflowLog(long entityId,
                                WorkflowEntities actionEntity,
                                DateTime actionDate,
                                WorkflowActions? workflowAction,
                                long actorUserId,
                                string remark,
                                long currentWorkflowStepId,
                                bool active)
            : base(actionEntity, workflowAction, actorUserId, actionDate, remark, currentWorkflowStepId, active)
        {
            OrderId = entityId;
        }


        public virtual Order Order { get; set; }
        public long OrderId { get; set; }

        public override WorkflowLog CreateWorkflowLog(long actorUserId, long workflowStepId, WorkflowActions performedAction, States state, WorkflowStages currentWorkflowStage)
        {
            var ordeWorkflowLog = new OrderWorkflowLog(OrderId, WorkflowEntity, DateTime.Now, performedAction, actorUserId, Remark, workflowStepId, true);

            return ordeWorkflowLog;
        }

        public override void ComplyWithWorkflowStateChanges(WorkflowStep newWorkflowStep, WorkflowActions performedAction, long approverId)
        {
            if (CurrentWorkflowStep.State != newWorkflowStep.State)


                switch (performedAction)
                {
                    case WorkflowActions.Approve:
                        Order.OrderState.ApproveOrder(Order, approverId);

                        break;
                    case WorkflowActions.Reject:
                        Order.OrderState.RejectOrder(Order, approverId);
                        break;

                    case WorkflowActions.Cancel:
                        Order.OrderState.CancelOrder(Order, approverId);
                        break;

                    case WorkflowActions.Close:
                        Order.OrderState.CloseOrder(Order, approverId);
                        break;
                }
        }
    }
}