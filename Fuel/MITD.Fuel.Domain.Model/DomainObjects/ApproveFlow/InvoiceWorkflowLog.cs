using System;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class InvoiceWorkflowLog : WorkflowLog
    {
        public InvoiceWorkflowLog()
        {

        }
        public InvoiceWorkflowLog
            (long entityId, WorkflowEntities actionEntity, DateTime actionDate, WorkflowActions? workflowAction, long actorUserId, string remark, long currentWorkflowStepId, bool active)
            : base(actionEntity, workflowAction, actorUserId, actionDate, remark, currentWorkflowStepId, active)
        {
            InvoiceId = entityId;
        }


        public virtual Invoice Invoice { get; set; }
        public long InvoiceId { get; set; }


        public override WorkflowLog CreateWorkflowLog(long actorUserId, long workflowStepId, WorkflowActions performedAction, States state, WorkflowStages currentWorkflowStage)
        {
            var ordeWorkflowLog = new InvoiceWorkflowLog(InvoiceId, WorkflowEntity, DateTime.Now, performedAction, actorUserId,
                                                       Remark, workflowStepId, true);

            return ordeWorkflowLog;
        }

        public override void ComplyWithWorkflowStateChanges(WorkflowStep newWorkflowStep, WorkflowActions performedAction, long approverId)
        {
            if (CurrentWorkflowStep.State != newWorkflowStep.State)

                switch (performedAction)
                {
                    case WorkflowActions.Approve:
                        Invoice.InvoiceState.ApproveInvoice(Invoice, approverId);

                        break;
                    case WorkflowActions.Reject:
                        Invoice.InvoiceState.RejectInvoice(Invoice, approverId);
                        break;

                    case WorkflowActions.Cancel:
                        Invoice.InvoiceState.CancelInvoice(Invoice, approverId);
                        break;
                }
        }
    }
}