using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MITD.Fuel.Domain.Model.Enums;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class ActivityFlow
    {
        public long Id { get; set; }

        public long WorkflowStepId { get; set; }
        public virtual WorkflowStep WorkflowStep{ get; set; }
        public long WorkflowNextStepId { get; set; }
        public virtual  WorkflowStep WorkflowNextStep { get; set; }

        public WorkflowActions WorkflowAction { get; set; }

        // Business Action to indicate what privilege is required 
        // for the user to perform the corresponding workflow action
        public int ActionTypeId { get; set; }
        public virtual ActionType ActionType { get; set; }

        public  ActivityFlow()
        {
        }

        public ActivityFlow(long workflowStepId, long workflowNextStepId, int actionTypeId, WorkflowActions workflowAction)
        {
            WorkflowStepId = workflowStepId;
            WorkflowNextStepId = workflowNextStepId;
            ActionTypeId = actionTypeId;
            WorkflowAction = workflowAction;
        }

        public ActivityFlow(WorkflowStep workflowStep, WorkflowStep workflowNextStep, WorkflowActions workflowAction, ActionType actionType)
        {
            WorkflowStep = workflowStep;
            WorkflowNextStep = workflowNextStep;
            WorkflowAction = workflowAction;
            ActionType = actionType;
        }
    }
}