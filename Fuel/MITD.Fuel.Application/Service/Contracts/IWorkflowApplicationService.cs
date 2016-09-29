using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Services.Application;

namespace MITD.Fuel.Application.Service.Contracts
{
    public interface IWorkflowApplicationService : IApplicationService
    {
        ApprovalResult MoveToNextStep(long entityId, WorkflowActionEntityType workflowEntity, long approverId, string remark, WorkflowActions action);

        void RevertAllFuelReportsFromReportId(long fuelReportId, long userId);
        void SubmitAllFuelReportsFromReportId(long fuelReportId, long userId);
    }
}
