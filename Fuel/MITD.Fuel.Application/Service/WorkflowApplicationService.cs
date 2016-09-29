#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using System.Transactions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using MITD.Fuel.Application.Service.Security;
using MITD.FuelSecurity.Domain.Model;
using MITD.FuelSecurity.Domain.Model.Service;
using MITD.Fuel.Application.Facade;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

#endregion

namespace MITD.Fuel.Application.Service
{
    public class WorkflowApplicationService : IWorkflowApplicationService
    {
        #region prop

        private readonly IWorkflowLogRepository _workflowLogRepository;
        private readonly IWorkflowStepRepository _workflowStepRepository;
        private readonly IEntityConfigurator<Order> orderConfigurator;
        private readonly IEntityConfigurator<Invoice> invoiceConfigurator;
        private readonly IEntityConfigurator<FuelReport> fuelReportConfigurator;
        private readonly IEntityConfigurator<Scrap> scrapConfigurator;
        private readonly IEntityConfigurator<Charter> _charterConfigurator;
        private readonly IEntityConfigurator<Offhire> offhireConfigurator;
        private readonly ISecurityServiceChecker securityServiceChecker;

        private ICharterInRepository charterInRepository;
        private ICharterOutRepository charterOutRepository;

        private readonly IUnitOfWorkScope _unitOfWorkScope;
        //    private readonly IApproveWorkFlowFactory _approveWorkFlowFactory;
        //  private readonly IOrderApplicationService _orderApplicationService;

        #endregion

        #region ctor

        public WorkflowApplicationService(IUnitOfWorkScope unitOfWorkScope, IWorkflowLogRepository workflowLogRepository,
                                             IWorkflowStepRepository _workflowStepRepository,
            IEntityConfigurator<Order> orderConfigurator,
            IEntityConfigurator<FuelReport> fuelReportConfigurator,
            IEntityConfigurator<Scrap> scrapConfigurator,
            IEntityConfigurator<Charter> charterConfigurator,
            IEntityConfigurator<Invoice> invoiceConfigurator,
            IEntityConfigurator<Offhire> offhireConfigurator,
            ICharterInRepository charterInRepository,
            ICharterOutRepository charterOutRepository,
            ISecurityServiceChecker securityServiceChecker)
        {
            _unitOfWorkScope = unitOfWorkScope;
            _workflowLogRepository = workflowLogRepository;
            this._workflowStepRepository = _workflowStepRepository;
            this.orderConfigurator = orderConfigurator;
            this.fuelReportConfigurator = fuelReportConfigurator;
            this.scrapConfigurator = scrapConfigurator;
            this.invoiceConfigurator = invoiceConfigurator;
            this.offhireConfigurator = offhireConfigurator;
            this.charterInRepository = charterInRepository;
            this.charterOutRepository = charterOutRepository;
            this._charterConfigurator = charterConfigurator;
            this.securityServiceChecker = securityServiceChecker;
            //          _approveWorkFlowFactory = approveWorkFlowFactory;
            //           _orderApplicationService = orderApplicationService;
        }

        #endregion

        public ApprovalResult MoveToNextStep(long entityId, WorkflowActionEntityType workflowEntity, long approverId, string remark, WorkflowActions action)
        {
            /* <H.A>
             * "in SQL 2014, for client drivers with TDS version 7.3 or higher, SQL server will reset transaction isolation level to default (read committed) for pooled connections. for clients with TDS version lower than 7.3 they will have the old behavior when running against SQL 2014."
             * Source:  https://social.msdn.microsoft.com/Forums/sqlserver/en-US/916b3d8a-c464-4ad5-8901-6f845a2a3447/sql-server-2014-reseting-isolation-level?forum=sqldatabaseengine
             *          http://blogs.msdn.com/b/dbrowne/archive/2010/06/03/using-new-transactionscope-considered-harmful.aspx
            */
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                var lastWorkflowLog = GetLastWorkflowLog(entityId, workflowEntity);

                // SharifWFChange 
                // var step = GetWorkflowStep(lastWorkflowLog.CurrentWorkflowStep, action, approverId);
                var step = lastWorkflowLog.CurrentWorkflowStep;

                // Begin SharifWFChange
                // if (step.NextWorkflowStep != null)
                var activityFlow = step.ActivityFlows.FirstOrDefault(s => s.WorkflowAction == action);
                WorkflowStep nextWorkflowStep = null;

                if (activityFlow != null)
                {
                    ValidateUserAccess(activityFlow.ActionType);

                    nextWorkflowStep = activityFlow.WorkflowNextStep;
                }
                else
                {
                    throw new WorkFlowException("Invalid Action.");
                }


                // End SharifWFChange

                lastWorkflowLog.ComplyWithWorkflowStateChanges(nextWorkflowStep, action, approverId);

                var newWorkFlowLog = lastWorkflowLog.CreateWorkflowLog(approverId,
                                                                         nextWorkflowStep.Id, action, nextWorkflowStep.State, nextWorkflowStep.CurrentWorkflowStage);

                _workflowLogRepository.Add(newWorkFlowLog);

                // Begin SharifWFChange
                // var result = updateApproveState(entityId, workflowEntity, approverId, remark, step.WithWorkflowAction, lastWorkflowLog);
                lastWorkflowLog.Deactivate();
                _unitOfWorkScope.Commit();

                var result = new ApprovalResult
                {
                    WorkflowAction = action,
                    ActorId = approverId,
                    DecisionType = DecisionTypes.Approved,
                    EntityId = entityId,
                    Entity = workflowEntity,
                    Remark = remark
                };

                // End SharifWFChange

                transactionScope.Complete();

                return result;
            }
        }

        private WorkflowLog GetLastWorkflowLog(long entityId, WorkflowActionEntityType actionEntity)
        {
            WorkflowLog currentApprovalWorkFlow;
            switch (actionEntity)
            {
                case WorkflowActionEntityType.Order:
                    var orderWorkflow = _workflowLogRepository.GetQuery().OfType<OrderWorkflowLog>().FirstOrDefault(
                            c => c.WorkflowEntity == WorkflowEntities.Order && c.OrderId == entityId && c.Active);
                    if (orderWorkflow == null)
                        throw new ObjectNotFound("OrderWorkflow", entityId);

                    orderConfigurator.Configure(orderWorkflow.Order);
                    currentApprovalWorkFlow = orderWorkflow;

                    break;
                case WorkflowActionEntityType.Invoice:
                    var invoiceWorkflow = _workflowLogRepository.GetQuery().OfType<InvoiceWorkflowLog>().
                        FirstOrDefault(
                            c => c.WorkflowEntity == WorkflowEntities.Invoice && c.InvoiceId == entityId && c.Active);

                    if (invoiceWorkflow == null)
                        throw new ObjectNotFound("InvoiceWorkflow", entityId);

                    invoiceConfigurator.Configure(invoiceWorkflow.Invoice);
                    currentApprovalWorkFlow = invoiceWorkflow;

                    break;
                case WorkflowActionEntityType.FuelReport:
                    var fuelReportWorkflow =
                        _workflowLogRepository.GetQuery().OfType<FuelReportWorkflowLog>().FirstOrDefault(
                            c => c.WorkflowEntity == WorkflowEntities.FuelReport && c.FuelReportId == entityId && c.Active);

                    if (fuelReportWorkflow == null)
                        throw new ObjectNotFound("FuelReportWorkflow", entityId);

                    fuelReportConfigurator.Configure(fuelReportWorkflow.FuelReport);

                    currentApprovalWorkFlow = fuelReportWorkflow;

                    break;

                case WorkflowActionEntityType.CharterIn:
                    var charterIn = charterInRepository.Single(c => c.Id == entityId);


                    var charterInWorkflow =
                        charterIn.CharterType == CharterType.Start ?
                            _workflowLogRepository.GetQuery().OfType<CharterWorkflowLog>().FirstOrDefault(
                                c => c.WorkflowEntity == WorkflowEntities.CharterInStart && c.CharterId == entityId && c.Active) :
                            _workflowLogRepository.GetQuery().OfType<CharterWorkflowLog>().FirstOrDefault(
                                c => c.WorkflowEntity == WorkflowEntities.CharterInEnd && c.CharterId == entityId && c.Active);


                    if (charterInWorkflow == null)
                        throw new ObjectNotFound(string.Format("CharterIn{0}Workflow", charterIn.CharterType), entityId);

                    _charterConfigurator.Configure(charterInWorkflow.Charter);

                    currentApprovalWorkFlow = charterInWorkflow;

                    break;
                case WorkflowActionEntityType.CharterOut:

                    var charterOut = charterOutRepository.Single(c => c.Id == entityId);

                    var charterOutWorkflow =
                        charterOut.CharterType == CharterType.Start ?
                            _workflowLogRepository.GetQuery().OfType<CharterWorkflowLog>().FirstOrDefault(
                                c => c.WorkflowEntity == WorkflowEntities.CharterOutStart && c.CharterId == entityId && c.Active) :
                            _workflowLogRepository.GetQuery().OfType<CharterWorkflowLog>().FirstOrDefault(
                                c => c.WorkflowEntity == WorkflowEntities.CharterOutEnd && c.CharterId == entityId && c.Active);

                    if (charterOutWorkflow == null)
                        throw new ObjectNotFound(string.Format("CharterOut{0}Workflow", charterOut.CharterType), entityId);

                    _charterConfigurator.Configure(charterOutWorkflow.Charter);

                    currentApprovalWorkFlow = charterOutWorkflow;

                    break;
                case WorkflowActionEntityType.Scrap:
                    var scrapWorkflow =
                        _workflowLogRepository.GetQuery().OfType<ScrapWorkflowLog>().FirstOrDefault(
                            c => c.WorkflowEntity == WorkflowEntities.Scrap && c.ScrapId == entityId && c.Active);

                    if (scrapWorkflow == null)
                        throw new ObjectNotFound("ScrapWorkflow", entityId);

                    scrapConfigurator.Configure(scrapWorkflow.Scrap);

                    currentApprovalWorkFlow = scrapWorkflow;

                    break;
                case WorkflowActionEntityType.Offhire:
                    var offhireWorkflow =
                        _workflowLogRepository.GetQuery().OfType<OffhireWorkflowLog>().FirstOrDefault(
                            c => c.WorkflowEntity == WorkflowEntities.Offhire && c.OffhireId == entityId && c.Active);

                    if (offhireWorkflow == null)
                        throw new ObjectNotFound("OffhireWorkflow", entityId);

                    offhireConfigurator.Configure(offhireWorkflow.Offhire);

                    currentApprovalWorkFlow = offhireWorkflow;

                    break;
                default:
                    throw new ArgumentOutOfRangeException("actionEntity");
            }

            if (currentApprovalWorkFlow == null)
                throw new WorkFlowException("Record Not Have WorkFlow Object");
            return currentApprovalWorkFlow;
        }

        public void ValidateUserAccess(ActionType businessAction)
        {
            var securityFacade = ServiceLocator.Current.GetInstance<ISecurityFacadeService>();
            var actions = securityFacade.GetUserAuthorizedActions(ClaimsPrincipal.Current);
            //var actions = securityFacade.GetUserAuthorizedActionsById(SecurityApplicationService.GetCurrentUserId());

            if (!actions.Exists(ad => ad.Id == businessAction.Id))
                throw new WorkFlowException("Workflow Invalid Access");
        }

        public void RevertAllFuelReportsFromReportId(long fuelReportId, long userId)
        {
            var fuelReportRepository = ServiceLocator.Current.GetInstance<IFuelReportRepository>();

            var tansactionScopeFactory = ServiceLocator.Current.GetInstance<ITransactionScopeFactory>();

            var startingFuelReport = fuelReportRepository.Single(fr => fr.Id == fuelReportId);

            if (startingFuelReport == null)
                throw new ObjectNotFound("FuelReport", fuelReportId);

            var fuelReportsToRevertOrderedDescending = fuelReportRepository.Find(fr => fr.VesselInCompanyId == startingFuelReport.VesselInCompanyId && (fr.Id == fuelReportId || startingFuelReport.EventDate <= fr.EventDate) && fr.State == States.Submitted).OrderByDescending(fr => fr.EventDate);

            using (var transactionScope = tansactionScopeFactory.Create())
            {
                foreach (var fuelReport in fuelReportsToRevertOrderedDescending)
                {
                    this.MoveToNextStep(fuelReport.Id, WorkflowActionEntityType.FuelReport, userId, "Revert Fuel Reports in Batch", WorkflowActions.Reject);
                }

                transactionScope.Complete();
            }
        }

        public void SubmitAllFuelReportsFromReportId(long fuelReportId, long userId)
        {
            var fuelReportRepository = ServiceLocator.Current.GetInstance<IFuelReportRepository>();

            var tansactionScopeFactory = ServiceLocator.Current.GetInstance<ITransactionScopeFactory>();

            var startingFuelReport = fuelReportRepository.Single(fr => fr.Id == fuelReportId);

            if (startingFuelReport == null)
                throw new ObjectNotFound("FuelReport", fuelReportId);

            var fuelReportsToRevertOrderedDescending = fuelReportRepository.Find(fr => fr.VesselInCompanyId == startingFuelReport.VesselInCompanyId && (fr.Id == fuelReportId || startingFuelReport.EventDate <= fr.EventDate) && (fr.State == States.SubmitRejected || fr.State == States.Open)).OrderBy(fr => fr.EventDate);

            foreach (var fuelReport in fuelReportsToRevertOrderedDescending)
            {
                try
                {
                    using (var transactionScope = tansactionScopeFactory.Create())
                    {
                        while (fuelReport.State == States.Open || fuelReport.State == States.SubmitRejected)
                        {
                            this.MoveToNextStep(fuelReport.Id, WorkflowActionEntityType.FuelReport, userId, "Submit Fuel Reports in Batch", WorkflowActions.Approve);
                        }

                        transactionScope.Complete();
                    }
                }
                catch 
                {
                    break;
                }

                //var operatedFuelReport = fuelReportRepository.Single(fr => fr.Id == fuelReportId);

                //if (operatedFuelReport.State != States.Submitted)
                //    break;
            }
        }
    }
}