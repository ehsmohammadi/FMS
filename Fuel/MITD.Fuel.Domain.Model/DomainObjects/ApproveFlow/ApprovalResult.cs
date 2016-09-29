#region

using MITD.Fuel.Domain.Model.Enums;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class ApprovalResult
    {
        public long EntityId { get; set; }

        public long ActorId { get; set; }

        public string Remark { get; set; }

        public WorkflowActions WorkflowAction { get; set; }

        public WorkflowActionEntityType Entity { get; set; }

        public DecisionTypes DecisionType { get; set; }
    }
}