#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.Factories
{
    public interface IApproveWorkFlowFactory : IFactory
    {
        WorkflowLog CreateApproveFlowLogObject(long entityId, WorkflowEntities actionEntity, DateTime actionDate, WorkflowActions workflowAction, long actorUserId, string remark, long approveWorkFlowConfigId);
    }
}