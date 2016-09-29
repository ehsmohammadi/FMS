using System;
using MITD.Core;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class FuelReportWorkflowLog : WorkflowLog
    {
        public FuelReportWorkflowLog()
        {

        }

        public FuelReportWorkflowLog(
            long entityId,
            WorkflowEntities actionEntity,
            DateTime actionDate,
            WorkflowActions? workflowAction,
            long actorUserId,
            string remark, long currentWorkflowStepId, bool active)
            : base(actionEntity, workflowAction, actorUserId, actionDate, remark, currentWorkflowStepId, active)
        {
            FuelReportId = entityId;
        }
        public long FuelReportId { get; set; }

        public virtual FuelReport FuelReport { get; set; }

        //public override Type GetDomainServiceType()
        //{
        //    return typeof(IApprovableFuelReportDomainService);
        //}



        public override WorkflowLog CreateWorkflowLog(long actorUserId, long workflowStepId, WorkflowActions performedAction, States state, WorkflowStages currentWorkflowStage)
        {
            var fuelReportApproveWorkFlow = new FuelReportWorkflowLog(
                FuelReportId,
                WorkflowEntity,
                DateTime.Now, 
                performedAction,
                actorUserId,
                Remark,
                workflowStepId, true);

            return fuelReportApproveWorkFlow;
        }

        public override void ComplyWithWorkflowStateChanges(WorkflowStep newWorkflowStep, WorkflowActions performedAction, long approverId)
        {
            IApprovableFuelReportDomainService approveService = ServiceLocator.Current.GetInstance<IApprovableFuelReportDomainService>();

            if (CurrentWorkflowStep.State != newWorkflowStep.State)
                switch (performedAction)
                {
                    case WorkflowActions.Approve:

                        FuelReport.EntityState.Approve(FuelReport, approveService, approverId);

                        break;

                    case WorkflowActions.Reject:

                        FuelReport.EntityState.Reject(FuelReport, approverId);

                        break;

                    case WorkflowActions.Cancel:

                        FuelReport.EntityState.Cancel(FuelReport, approverId);

                        break;
                }
            else
            {
                if (CurrentWorkflowStep.State == States.Submitted && 
                    CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.Submited && 
                    performedAction == WorkflowActions.Approve)
                {
                    FuelReport.SubmitByFinancial(approverId);
                }
                else if (CurrentWorkflowStep.State == States.Submitted &&
                    CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.FinancialSubmitted &&
                    performedAction == WorkflowActions.Reject)
                {
                    FuelReport.RejectFinancialSubmitted(approverId);
                }
            }
        }
    }
}