using System.Data.Entity.Migrations;
using System.Linq;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.Enums;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Data.EF.Migrations
{
    public class DataConfigurationProvider
    {
        public static void AddOrUpdateActionTypes(DataContainer context)
        {
            var actionTypes = ActionType.GetAllActions();

            foreach (var actionType in actionTypes)
            {
                context.ActionTypes.AddOrUpdate(actionType);
            }

            context.SaveChanges();
        }

        public static void InsertWorkflowConfigs(DataContainer context, long companyId)
        {
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.Order, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.FuelReport, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.Invoice, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.Scrap, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.CharterInStart, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.CharterInEnd, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.CharterOutStart, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.CharterOutEnd, companyId));
            context.Workflows.Add(new Workflow(Workflow.DEFAULT_NAME, WorkflowEntities.Offhire, companyId));

            context.SaveChanges();

            insertOrderWorkflowConfig(companyId, context);
            insertFuelReportWorkflowConfig(companyId, context);
            insertInvoiceWorkflowConfig(companyId, context);
            insertScrapWorkflowConfig(companyId, context);
            insertCharterInStartWorkflowConfig(companyId, context);
            insertCharterInEndWorkflowConfig(companyId, context);
            insertCharterOutStartWorkflowConfig(companyId, context);
            insertCharterOutEndWorkflowConfig(companyId, context);
            insertOffhireWorkflowConfig(companyId, context);
        }

        private static ActionType GetActionTypeFromContext(DataContainer context, ActionType actinType)
        {
            return context.ActionTypes.Single(a => a.Id == actinType.Id);
        }

        private static void insertInvoiceWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.Invoice && w.CompanyId == companyId).Id;

            //Workflow Config
            var initialInvoice = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedInvoice = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedInvoice = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var cancelledInvoice = new WorkflowStep(workflowId, States.Cancelled, WorkflowStages.Canceled);

            initialInvoice.AddActivityFlow(
                    approvedInvoice,
                    WorkflowActions.Approve,
                    GetActionTypeFromContext(context, ActionType.ManageInvoiceApprovement));

            approvedInvoice.AddActivityFlow(
                    submittedInvoice,
                    WorkflowActions.Approve,
                    GetActionTypeFromContext(context, ActionType.ManageInvoiceSubmittion));

            approvedInvoice.AddActivityFlow(
                    initialInvoice,
                    WorkflowActions.Reject,
                    GetActionTypeFromContext(context, ActionType.ManageInvoiceApprovement));

            submittedInvoice.AddActivityFlow(
                    cancelledInvoice,
                    WorkflowActions.Cancel,
                    GetActionTypeFromContext(context, ActionType.CancelInvoice));

            context.WorkflowSteps.AddOrUpdate(initialInvoice, approvedInvoice, submittedInvoice, cancelledInvoice);
            context.SaveChanges();
        }

        private static void insertOrderWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.Order && w.CompanyId == companyId).Id;

            //Workflow Config
            var initialOrder = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedOrder = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedOrder = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var cancelledOrder = new WorkflowStep(workflowId, States.Cancelled, WorkflowStages.Canceled);
            var closedOrder = new WorkflowStep(workflowId, States.Closed, WorkflowStages.Closed);

            initialOrder.AddActivityFlow(approvedOrder, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageOrderApprovement));
            approvedOrder.AddActivityFlow(submittedOrder, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageOrderSubmittion));
            approvedOrder.AddActivityFlow(initialOrder, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageOrderApprovement));
            submittedOrder.AddActivityFlow(closedOrder, WorkflowActions.Close, GetActionTypeFromContext(context, ActionType.CloseOrder));
            submittedOrder.AddActivityFlow(cancelledOrder, WorkflowActions.Cancel, GetActionTypeFromContext(context, ActionType.CancelOrder));

            context.WorkflowSteps.AddOrUpdate(initialOrder, approvedOrder, submittedOrder, cancelledOrder, closedOrder);
            context.SaveChanges();

        }

        private static WorkflowStep insertOffhireWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.Offhire && w.CompanyId == companyId).Id;

            //Workflow Config
            var initialOffhire = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedOffhire = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedOffhire = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var submitRejectedOffhire = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

            initialOffhire.AddActivityFlow(approvedOffhire, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageOffhireApprovement));
            approvedOffhire.AddActivityFlow(submittedOffhire, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageOffhireSubmittion));
            approvedOffhire.AddActivityFlow(initialOffhire, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageOffhireApprovement));
            submittedOffhire.AddActivityFlow(submitRejectedOffhire, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageOffhireSubmittion));
            submitRejectedOffhire.AddActivityFlow(submittedOffhire, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageOffhireSubmittion));

            context.WorkflowSteps.AddOrUpdate(initialOffhire, approvedOffhire, submittedOffhire, submitRejectedOffhire);
            context.SaveChanges();

            return initialOffhire;
        }

        private static WorkflowStep insertFuelReportWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.FuelReport && w.CompanyId == companyId).Id;

            //Workflow Config
            var initialFuelReport = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedFuelReport = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedFuelReport = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var submitRejectedFuelReport = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

            initialFuelReport.AddActivityFlow(approvedFuelReport, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageFuelReportApprovement));
            approvedFuelReport.AddActivityFlow(submittedFuelReport, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageFuelReportSubmittion));
            approvedFuelReport.AddActivityFlow(initialFuelReport, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageFuelReportApprovement));
            submittedFuelReport.AddActivityFlow(submitRejectedFuelReport, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageFuelReportSubmittion));
            submitRejectedFuelReport.AddActivityFlow(submittedFuelReport, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageFuelReportSubmittion));

            context.WorkflowSteps.AddOrUpdate(initialFuelReport, approvedFuelReport, submittedFuelReport, submitRejectedFuelReport);
            context.SaveChanges();

            return initialFuelReport;
        }

        public static void ModifyFuelReportWorkflowConfigForFinancialSubmit_9404041400(DataContainer context)
        {
            AddOrUpdateActionTypes(context);

            foreach (var company in context.Companies.ToList())
            {
                var workflow = context.Workflows.SingleOrDefault(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.FuelReport && w.CompanyId == company.Id);

                if (workflow == null) continue;

                ////Workflow Config
                //var initialFuelReport = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
                //var approvedFuelReport = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
                //var submittedFuelReport = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
                //var submitRejectedFuelReport = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

                //initialFuelReport.AddActivityFlow(approvedFuelReport, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageFuelReportApprovement));
                //approvedFuelReport.AddActivityFlow(submittedFuelReport, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageFuelReportSubmittion));
                //approvedFuelReport.AddActivityFlow(initialFuelReport, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageFuelReportApprovement));
                //submittedFuelReport.AddActivityFlow(submitRejectedFuelReport, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageFuelReportSubmittion));
                //submitRejectedFuelReport.AddActivityFlow(submittedFuelReport, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageFuelReportSubmittion));

                //context.WorkflowSteps.AddOrUpdate(initialFuelReport, approvedFuelReport, submittedFuelReport, submitRejectedFuelReport);
                //context.SaveChanges();

                var submittedFuelReportStep = context.WorkflowSteps.Single(wfs => wfs.WorkflowId == workflow.Id && 
                    wfs.State == States.Submitted && wfs.CurrentWorkflowStage == WorkflowStages.Submited);

                var financialSubmittedFuelReport = new WorkflowStep(workflow.Id, States.Submitted, WorkflowStages.FinancialSubmitted);

                submittedFuelReportStep.AddActivityFlow(financialSubmittedFuelReport, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageFuelReportFinancialSubmition));
                financialSubmittedFuelReport.AddActivityFlow(submittedFuelReportStep, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageFuelReportFinancialSubmition));

                context.WorkflowSteps.AddOrUpdate(submittedFuelReportStep);
            }

            context.SaveChanges();
        }

        public static void ModifyFuelReportWorkflowConfigForCancel_9408071200(DataContainer context)
        {
            AddOrUpdateActionTypes(context);

            foreach (var company in context.Companies.Where(c => c.IsMemberOfHolding).ToList())
            {
                var workflow = context.Workflows.SingleOrDefault(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.FuelReport && w.CompanyId == company.Id);

                if(workflow == null) continue;
                
                var initialFuelReportStep = context.WorkflowSteps.Single(wfs => wfs.WorkflowId == workflow.Id && 
                    wfs.State == States.Open && wfs.CurrentWorkflowStage == WorkflowStages.Initial);

                var cancelledFuelReportStepToAdd = new WorkflowStep(workflow.Id, States.Cancelled, WorkflowStages.Canceled);

                initialFuelReportStep.AddActivityFlow(cancelledFuelReportStepToAdd, WorkflowActions.Cancel, GetActionTypeFromContext(context, ActionType.ManageFuelReportCancel));

                context.WorkflowSteps.AddOrUpdate(initialFuelReportStep);
            }

            context.SaveChanges();
        }

        private static WorkflowStep insertScrapWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.Scrap && w.CompanyId == companyId).Id;

            var initialScrap = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedScrap = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedScrap = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var submitRejectedScrap = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

            initialScrap.AddActivityFlow(approvedScrap, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageScrapApprovement));
            approvedScrap.AddActivityFlow(initialScrap, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageScrapApprovement));
            submittedScrap.AddActivityFlow(submitRejectedScrap, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageScrapSubmittion));
            submitRejectedScrap.AddActivityFlow(submittedScrap, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageScrapSubmittion));

            context.WorkflowSteps.AddOrUpdate(initialScrap, approvedScrap, submittedScrap, submitRejectedScrap);
            context.SaveChanges();

            return initialScrap;
        }

        private static WorkflowStep insertCharterInStartWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.CharterInStart && w.CompanyId == companyId).Id;

            var initialCharterInStart = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedCharterInStart = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedCharterInStart = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var submitRejectedCharterInStart = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

            initialCharterInStart.AddActivityFlow(approvedCharterInStart, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            approvedCharterInStart.AddActivityFlow(submittedCharterInStart, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            approvedCharterInStart.AddActivityFlow(initialCharterInStart, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            submittedCharterInStart.AddActivityFlow(submitRejectedCharterInStart, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            submitRejectedCharterInStart.AddActivityFlow(submittedCharterInStart, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));

            context.WorkflowSteps.AddOrUpdate(initialCharterInStart, approvedCharterInStart, submittedCharterInStart, submitRejectedCharterInStart);
            context.SaveChanges();

            return initialCharterInStart;
        }

        private static WorkflowStep insertCharterInEndWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.CharterInEnd && w.CompanyId == companyId).Id;

            var initialCharterInEnd = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedCharterInEnd = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedCharterInEnd = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var submitRejectedCharterInEnd = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

            initialCharterInEnd.AddActivityFlow(approvedCharterInEnd, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            approvedCharterInEnd.AddActivityFlow(submittedCharterInEnd, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            approvedCharterInEnd.AddActivityFlow(initialCharterInEnd, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            submittedCharterInEnd.AddActivityFlow(submitRejectedCharterInEnd, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            submitRejectedCharterInEnd.AddActivityFlow(submittedCharterInEnd, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));

            context.WorkflowSteps.AddOrUpdate(initialCharterInEnd, approvedCharterInEnd, submittedCharterInEnd, submitRejectedCharterInEnd);
            context.SaveChanges();

            return initialCharterInEnd;
        }

        private static WorkflowStep insertCharterOutStartWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.CharterOutStart && w.CompanyId == companyId).Id;

            var initialCharterOutStart = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedCharterOutStart = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedCharterOutStart = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var submitRejectedCharterOutStart = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

            initialCharterOutStart.AddActivityFlow(approvedCharterOutStart, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            approvedCharterOutStart.AddActivityFlow(submittedCharterOutStart, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            approvedCharterOutStart.AddActivityFlow(initialCharterOutStart, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            submittedCharterOutStart.AddActivityFlow(submitRejectedCharterOutStart, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            submitRejectedCharterOutStart.AddActivityFlow(submittedCharterOutStart, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));

            context.WorkflowSteps.AddOrUpdate(initialCharterOutStart, approvedCharterOutStart, submittedCharterOutStart, submitRejectedCharterOutStart);
            context.SaveChanges();

            return initialCharterOutStart;
        }

        private static WorkflowStep insertCharterOutEndWorkflowConfig(long companyId, DataContainer context)
        {
            long workflowId = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.CharterOutEnd && w.CompanyId == companyId).Id;

            var initialCharterOutEnd = new WorkflowStep(workflowId, States.Open, WorkflowStages.Initial);
            var approvedCharterOutEnd = new WorkflowStep(workflowId, States.Open, WorkflowStages.Approved);
            var submittedCharterOutEnd = new WorkflowStep(workflowId, States.Submitted, WorkflowStages.Submited);
            var submitRejectedCharterOutEnd = new WorkflowStep(workflowId, States.SubmitRejected, WorkflowStages.SubmitRejected);

            initialCharterOutEnd.AddActivityFlow(approvedCharterOutEnd, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            approvedCharterOutEnd.AddActivityFlow(submittedCharterOutEnd, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            approvedCharterOutEnd.AddActivityFlow(initialCharterOutEnd, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInApprovement));
            submittedCharterOutEnd.AddActivityFlow(submitRejectedCharterOutEnd, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));
            submitRejectedCharterOutEnd.AddActivityFlow(submittedCharterOutEnd, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ManageCharterInSubmition));

            context.WorkflowSteps.AddOrUpdate(initialCharterOutEnd, approvedCharterOutEnd, submittedCharterOutEnd, submitRejectedCharterOutEnd);
            context.SaveChanges();

            return initialCharterOutEnd;
        }

        public static void ModifyOrderWorkflowConfigForRejectSubmittedOrder_941128(DataContainer context)
        {
            AddOrUpdateActionTypes(context);

            foreach (var company in context.Companies.Where(c=>c.IsMemberOfHolding).ToList())
            {
                var workflow = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.Order && w.CompanyId == company.Id);
                if(workflow == null) continue;

                //Workflow Config
                var submittedOrder = context.WorkflowSteps.Single(wfs => wfs.WorkflowId == workflow.Id && wfs.State == States.Submitted && wfs.CurrentWorkflowStage == WorkflowStages.Submited);
                var cancelledOrder = context.WorkflowSteps.Single(wfs => wfs.WorkflowId == workflow.Id && wfs.State == States.Cancelled && wfs.CurrentWorkflowStage == WorkflowStages.Canceled);

                var submitRejectedOrder = new WorkflowStep(workflow.Id, States.SubmitRejected, WorkflowStages.SubmitRejected);

                submittedOrder.AddActivityFlow(submitRejectedOrder, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.RejectSubmittedOrder));
                submitRejectedOrder.AddActivityFlow(submittedOrder, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ResubmitRejectedOrder));
                submitRejectedOrder.AddActivityFlow(cancelledOrder, WorkflowActions.Cancel, GetActionTypeFromContext(context, ActionType.CancelOrder));

                context.WorkflowSteps.AddOrUpdate(submittedOrder, submitRejectedOrder);

                context.SaveChanges();
            }
        }

        public static void ModifyInvoiceWorkflowConfigForRejectSubmittedInvoice_950326(DataContainer context)
        {
            AddOrUpdateActionTypes(context);

            foreach (var company in context.Companies.Where(c => c.IsMemberOfHolding).ToList())
            {
                var workflow = context.Workflows.Single(w => w.Name == Workflow.DEFAULT_NAME && w.WorkflowEntity == WorkflowEntities.Invoice && w.CompanyId == company.Id);
                if (workflow == null) continue;

                //Workflow Config
                var submittedInvoice = context.WorkflowSteps.Single(wfs => wfs.WorkflowId == workflow.Id && wfs.State == States.Submitted && wfs.CurrentWorkflowStage == WorkflowStages.Submited);
                var cancelledInvoice = context.WorkflowSteps.Single(wfs => wfs.WorkflowId == workflow.Id && wfs.State == States.Cancelled && wfs.CurrentWorkflowStage == WorkflowStages.Canceled);

                var submitRejectedInvoice = new WorkflowStep(workflow.Id, States.SubmitRejected, WorkflowStages.SubmitRejected);

                submittedInvoice.AddActivityFlow(submitRejectedInvoice, WorkflowActions.Reject, GetActionTypeFromContext(context, ActionType.RejectSubmittedInvoice));
                submitRejectedInvoice.AddActivityFlow(submittedInvoice, WorkflowActions.Approve, GetActionTypeFromContext(context, ActionType.ResubmitRejectedInvoice));
                submitRejectedInvoice.AddActivityFlow(cancelledInvoice, WorkflowActions.Cancel, GetActionTypeFromContext(context, ActionType.CancelInvoice));

                context.WorkflowSteps.AddOrUpdate(submittedInvoice, submitRejectedInvoice);
                context.SaveChanges();
            }
        }
    }
}
