using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using System;
using MITD.Fuel.Presentation.Contracts.Enums;



namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IApprovalFlowServiceWrapper : IServiceWrapper
    {
        void ActApproveFlow(Action<ApprovmentDto, Exception> action, long entityId, ActionEntityTypeEnum entityTypeId);
        void ActRejectFlow(Action<ApprovmentDto, Exception> action, long entityId, ActionEntityTypeEnum entityTypeId);
        void ActCancelFlow(Action<ApprovmentDto, Exception> action, long entityId, ActionEntityTypeEnum entityTypeId);
        void ActCloseFlow(Action<ApprovmentDto, Exception> action, long entityId, ActionEntityTypeEnum entityTypeId);
        
        void ApproveAllFuelReportsFromReportId(Action<object, Exception> action, long entityId);
        void RejectAllFuelReportsFromReportId(Action<object, Exception> action, long entityId);
    }
  
}
