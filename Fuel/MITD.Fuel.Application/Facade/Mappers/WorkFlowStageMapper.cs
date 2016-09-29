using System;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Application.Facade.Mappers
{
    public class WorkFlowStageMapper :IWorkFlowStageMapper
    {
        public WorkflowStageEnum MapToModel(WorkflowStages workflowStage)
        {
            switch (workflowStage)
            {
                case WorkflowStages.None:
                    return WorkflowStageEnum.None;
                    break;
                case WorkflowStages.Initial:
                    return WorkflowStageEnum.Initial;
                    break;
                case WorkflowStages.Approved:
                    return WorkflowStageEnum.Approved;
                    break;
                case WorkflowStages.FinalApproved:
                    return WorkflowStageEnum.FinalApproved;
                    break;
                case WorkflowStages.Submited:
                    return WorkflowStageEnum.Submited;
                    break;
                case WorkflowStages.Closed:
                    return WorkflowStageEnum.Closed;
                    break;
                case WorkflowStages.Canceled:
                    return WorkflowStageEnum.Canceled;
                    break;
                case WorkflowStages.SubmitRejected:
                    return WorkflowStageEnum.SubmitRejected;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("workflowStage");
            }
        }
    }

    public interface IWorkFlowStageMapper
    {
    }
}