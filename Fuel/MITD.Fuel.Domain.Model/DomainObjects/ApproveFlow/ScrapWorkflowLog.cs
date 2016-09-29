using System;
using MITD.Core;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class ScrapWorkflowLog : WorkflowLog
    {
        public ScrapWorkflowLog()
        {

        }

        public ScrapWorkflowLog(
            Scrap scrap,
            WorkflowEntities actionEntity,
            DateTime actionDate,
            WorkflowActions? workflowAction,
            long actorUserId,
            string remark, long currentWorkflowStepId, bool active)
            : base(actionEntity, workflowAction, actorUserId, actionDate, remark, currentWorkflowStepId, active)
        {
            Scrap = scrap;
        }

        public long ScrapId { get; set; }

        public virtual Scrap Scrap { get; set; }

        public override WorkflowLog CreateWorkflowLog(long actorUserId, long workflowStepId, WorkflowActions performedAction, States state, WorkflowStages currentWorkflowStage)
        {
            var scrapWorkflowLog = new ScrapWorkflowLog(
                Scrap,
                WorkflowEntity,
                DateTime.Now, 
                performedAction,
                actorUserId,
                Remark,
                workflowStepId, true);

            return scrapWorkflowLog;
        }

        public override void ComplyWithWorkflowStateChanges(WorkflowStep newWorkflowStep, WorkflowActions performedAction, long approverId)
        {
            if (CurrentWorkflowStep.State != newWorkflowStep.State)
            {
                //Manage Change State:
                switch (performedAction)
                {
                    case WorkflowActions.Approve:

                        this.Scrap.EntityState.Approve(this.Scrap, approverId);

                        break;

                    case WorkflowActions.Reject:

                        this.Scrap.EntityState.Reject(this.Scrap, approverId);

                        break;

                    case WorkflowActions.Cancel:

                        this.Scrap.EntityState.Cancel(this.Scrap, approverId);

                        break;
                }
            }
            else
            {
                var approvableDomainService = ServiceLocator.Current.GetInstance<IApprovableScrapDomainService>();

                if ((this.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Initial ||
                    this.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Approved) &&
                    performedAction == WorkflowActions.Approve)
                {
                    //Manage Middle Approve:
                    approvableDomainService.ValidateMiddleApprove(this.Scrap);
                }
            }
        }

    }
}