#region

using MITD.Fuel.Domain.Model.DomainObjects;
using System;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.Factories
{
    public class ApproveWorkFlowFactory : IApproveWorkFlowFactory
    {
        #region IApproveFlowLogFactory Members

        public WorkflowLog CreateApproveFlowLogObject(long entityId, WorkflowEntities actionEntity, DateTime actionDate, WorkflowActions workflowAction, long actorUserId, string remark, long approveWorkFlowConfigId)
        {
            return new WorkflowLog(actionEntity,
               workflowAction,
               actorUserId,
               actionDate,
               remark, approveWorkFlowConfigId, true);
        }

        #endregion


    }
}