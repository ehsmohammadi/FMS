#region

using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Enums;
using MITD.FuelSecurity.Domain.Model;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class WorkflowStep
    {
        #region Properties

        public WorkflowStep(long workflowId, States state, WorkflowStages currentWorkflowStage):this()
        {
            WorkflowId = workflowId;
            State = state;
            CurrentWorkflowStage = currentWorkflowStage;
        }

        public WorkflowStep(Workflow workflow, States state, WorkflowStages currentWorkflowStage):this()
        {
            Workflow = workflow;
            State = state;
            CurrentWorkflowStage = currentWorkflowStage;
        }

        public long Id { get; set; }

        public long WorkflowId { get; set; }
        public virtual Workflow Workflow { get; set; }

        public States State { get; private set; }
        public WorkflowStages CurrentWorkflowStage { get; private set; }

        public virtual IList<ActivityFlow> ActivityFlows { get; set; }
        #endregion

        public WorkflowStep()
        {
            ActivityFlows = new List<ActivityFlow>();
        }

        public void AddActivityFlow(WorkflowStep nextWorkflowStep, WorkflowActions workflowAction, ActionType actionType)
        {
            this.ActivityFlows.Add(new ActivityFlow(this, nextWorkflowStep, workflowAction, actionType));
        }
    }
}
