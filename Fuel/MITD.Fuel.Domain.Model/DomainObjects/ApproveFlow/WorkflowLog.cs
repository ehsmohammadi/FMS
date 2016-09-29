#region

using System;
using System.Linq;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class WorkflowLog
    {
        #region Properties

        public long Id { get; private set; }

        public WorkflowEntities WorkflowEntity { get; private set; }

        public DateTime ActionDate { get; private set; }

        public WorkflowActions? WorkflowAction { get; private set; }

        public long? ActorUserId { get; private set; }

        public string Remark { get; private set; }

        public bool Active { get; set; }

        public virtual FuelUser ActorUser { get; private set; }

        public virtual WorkflowStep CurrentWorkflowStep { get; set; }
        public long CurrentWorkflowStepId { get; set; }

        #endregion

        public WorkflowLog()
        {
            //To be used as parameter 'TEntity' in the generic type or method 'MITD.DataAccess.EF.EntityTypeConfigurationBase<TEntity>'	
        }

        public WorkflowLog(WorkflowEntities actionEntity, WorkflowActions? workflowAction, long actorUserId, DateTime actionDate, string remark, long currentWorkflowStepId, bool active)
        {
            WorkflowEntity = actionEntity;

            WorkflowAction = workflowAction;
            ActorUserId = actorUserId;
            ActionDate = actionDate;
            Remark = remark;
            CurrentWorkflowStepId = currentWorkflowStepId;
            Active = active;
        }

        public void Deactivate()
        {
            //ActionDate = DateTime.Now;
            Active = false;
        }
        public virtual Type GetDomainServiceType()
        {
            throw new NotImplementedException();
        }

        // SharifWFChange -- Changed name from CreateNextStep to CreateWorkflowLog
        public virtual WorkflowLog CreateWorkflowLog(long actorUserId, long workflowStepId,WorkflowActions performedAction, States state, WorkflowStages currentWorkflowStage)
        {
            throw new NotImplementedException();
        }

        // SharifWFChange -- Commented
        //public void UpdateNextStep(long actorUserId, long workflowStepId)
        //{
        //    Active = true;
        //    CurrentWorkflowStepId = workflowStepId;
        //    ActorUserId = actorUserId;
        //}

        protected object Clone()
        {
            return new
                       {
                           Id,
                           ActionEntityType = WorkflowEntity,
                           ActionDate,
                           ActionType = WorkflowAction,
                           ActorUserId,
                           Remark,
                           ApproveWorkFlowConfigId = CurrentWorkflowStepId
                       };
        }

        public virtual void ComplyWithWorkflowStateChanges(WorkflowStep newWorkflowStep, WorkflowActions performedAction, long approverId)
        {
            throw new NotImplementedException();
        }

    }
}
