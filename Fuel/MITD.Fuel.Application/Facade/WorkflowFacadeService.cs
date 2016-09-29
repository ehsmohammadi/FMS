#region

using System;
using Castle.Core;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

#endregion

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof(SecurityInterception))]
    public class WorkflowFacadeService : IWorkflowFacadeService
    {
        private IWorkflowApplicationService workflowAppService { get; set; }

        private IFuelUserDomainService fuelUserDomainService;

        public WorkflowFacadeService(IWorkflowApplicationService workflowAppService, IFuelUserDomainService fuelUserDomainService)
        {
            this.workflowAppService = workflowAppService;
            this.fuelUserDomainService = fuelUserDomainService;
        }

        #region IApprovalFlowFacadeService Members
        
        public ApprovmentDto MoveToNextStep(ApprovmentDto entity)
        {
            var result = workflowAppService.MoveToNextStep(entity.EntityId,
                (WorkflowActionEntityType)(int)entity.ActionEntityType,
                fuelUserDomainService.GetCurrentFuelUserId(),
                entity.Remark,
                MapDesicionToAction(entity)
                );

            var retVal = new ApprovmentDto
                {
                    EntityId = result.EntityId,
                    ActorId = result.ActorId,
                    ActionType = MapEntityActionTypeToDtoActionType(result.WorkflowAction),
                    ActionEntityType = (ActionEntityTypeEnum)(int)result.Entity,
                    DecisionType = (DecisionTypeEnum)(int)result.DecisionType,
                    Remark = entity.Remark
                };
            return retVal;
        }

        public void ApplyBatchAction(ApprovmentDto entity)
        {
            if (MapDesicionToAction(entity) == WorkflowActions.Approve)
            {
                workflowAppService.SubmitAllFuelReportsFromReportId(entity.EntityId, fuelUserDomainService.GetCurrentFuelUserId());
            }
            else if (MapDesicionToAction(entity) == WorkflowActions.Reject)
            {
                workflowAppService.RevertAllFuelReportsFromReportId(entity.EntityId, fuelUserDomainService.GetCurrentFuelUserId());
            }
        }

        private static WorkflowActions MapDesicionToAction(ApprovmentDto entity)
        {
            WorkflowActions action;
            switch (entity.DecisionType)
            {
                case DecisionTypeEnum.Approved:
                    action = WorkflowActions.Approve;
                    break;
                case DecisionTypeEnum.Rejected:
                    action = WorkflowActions.Reject;
                    break;
                case DecisionTypeEnum.Canceled:
                    action = WorkflowActions.Cancel;
                    break;
                case DecisionTypeEnum.Close:
                    action = WorkflowActions.Close;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return action;
        }

        public ActionTypeEnum MapEntityActionTypeToDtoActionType(WorkflowActions workflowAction)
        {
            switch (workflowAction)
            {
                case WorkflowActions.Approve:

                    return ActionTypeEnum.Approved;

                case WorkflowActions.Reject:

                    return ActionTypeEnum.Rejected;

                default:
                    return ActionTypeEnum.Undefined;
            }
        }

        #endregion
    }
}