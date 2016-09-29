using System;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class CharterWorkflowLog : WorkflowLog
    {
        public CharterWorkflowLog()
        {

        }

        public CharterWorkflowLog(
            Charter charter,
            WorkflowEntities actionEntity,
            DateTime actionDate,
            WorkflowActions? workflowAction,
            long actorUserId,
            string remark, long currentWorkflowStepId, bool active)
            : base(actionEntity, workflowAction, actorUserId, actionDate, remark, currentWorkflowStepId, active)
        {
            Charter = charter;
        }

        public long CharterId { get; set; }

        public virtual Charter Charter { get; set; }

        public override WorkflowLog CreateWorkflowLog(long actorUserId, long workflowStepId,WorkflowActions performedAction, States state, WorkflowStages currentWorkflowStage)
        {
            var charterWorkflowLog = new CharterWorkflowLog(
                Charter,
                WorkflowEntity,
                DateTime.Now, 
                performedAction,
                actorUserId,
                Remark,
                workflowStepId, true);

            return charterWorkflowLog;
        }

        public override void ComplyWithWorkflowStateChanges(WorkflowStep newWorkflowStep, WorkflowActions performedAction, long approverId)
        {
            //var approvableDomainService = ServiceLocator.Current.GetInstance<IApprovableScrapDomainService>();

            if (CurrentWorkflowStep.State != newWorkflowStep.State)
            {
                //Manage Change State:
                switch (performedAction)
                {
                    case WorkflowActions.Approve:

                        this.Charter.CharterState.Approve(this.Charter, approverId);
                        break;

                    case WorkflowActions.Reject:

                        this.Charter.CharterState.Reject(this.Charter, approverId);
                        break;
                }
            }
            else
            {
                if ((this.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Initial ||
                    this.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Approved) &&
                    performedAction == WorkflowActions.Approve)
                {
                    Charter.Approve(approverId);
                    //Manage Middle Approve:
                    //approvableDomainService.ValidateMiddleApprove(this.CharterIn);
                }
            }
        }

    }
}