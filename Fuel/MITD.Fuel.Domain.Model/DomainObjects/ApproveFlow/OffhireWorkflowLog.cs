using System;
using MITD.Core;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class OffhireWorkflowLog : WorkflowLog
    {
        public OffhireWorkflowLog()
        {

        }

        public OffhireWorkflowLog(
            Offhire offhire,
            WorkflowEntities actionEntity,
            DateTime actionDate,
            WorkflowActions? workflowAction,
            long actorUserId,
            string remark, long currentWorkflowStepId, bool active)
            : base(actionEntity, workflowAction, actorUserId, actionDate, remark, currentWorkflowStepId, active)
        {
            Offhire = offhire;
        }

        public long OffhireId { get; set; }

        public virtual Offhire Offhire { get; set; }

        public override WorkflowLog CreateWorkflowLog(long actorUserId, long workflowStepId, WorkflowActions performedAction, States state, WorkflowStages currentWorkflowStage)
        {
            var offhireWorkflowLog = new OffhireWorkflowLog(
                Offhire,
                WorkflowEntity,
                DateTime.Now, 
                performedAction,
                actorUserId,
                Remark,
                workflowStepId, true);

            return offhireWorkflowLog;
        }

        public override void ComplyWithWorkflowStateChanges(WorkflowStep newWorkflowStep, WorkflowActions performedAction, long approverId)
        {
            var approvableDomainService = ServiceLocator.Current.GetInstance<IApprovableOffhireDomainService>();

            if (CurrentWorkflowStep.State != newWorkflowStep.State)
            {
                //Manage Change State:
                switch (performedAction)
                {
                    case WorkflowActions.Approve:

                        this.Offhire.EntityState.Approve(this.Offhire, approverId);

                        break;

                    case WorkflowActions.Reject:

                        this.Offhire.EntityState.Reject(this.Offhire, approverId);

                        break;

                    case WorkflowActions.Cancel:

                        this.Offhire.EntityState.Cancel(this.Offhire, approverId);

                        break;
                }
            }
            else
            {
                if ((this.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Initial ||
                    this.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Approved) &&
                    performedAction == WorkflowActions.Approve)
                {
                    //Manage Middle Approve:
                    approvableDomainService.ValidateMiddleApprove(this.Offhire);
                }
            }
        }

    }
}