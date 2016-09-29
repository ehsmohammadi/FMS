using System;
using System.Collections.Generic;
using MITD.Core;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate.Factories
{
    public class CharterFactory
    {
        private readonly IWorkflowStepRepository _workflowStepRepository;
        private readonly ICharterInDomainService _charterInDomainService;
        private readonly ICharterOutDomainService _charterOutDomainService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IVesselInCompanyDomainService vesselInCompanyDomainService;
        private readonly IInventoryOperationNotifier inventoryOperationNotifier;

        public CharterFactory(IWorkflowStepRepository _workflowStepRepository,
            ICharterInDomainService charterInDomainService,
            ICharterOutDomainService charterOutDomainService,
            IEventPublisher eventPublisher, IVesselInCompanyDomainService vesselInCompanyDomainService, IInventoryOperationNotifier inventoryOperationNotifier)
        {
            this._workflowStepRepository = _workflowStepRepository;
            _charterInDomainService = charterInDomainService;
            _charterOutDomainService = charterOutDomainService;
            _eventPublisher = eventPublisher;
            this.vesselInCompanyDomainService = vesselInCompanyDomainService;
            this.inventoryOperationNotifier = inventoryOperationNotifier;
        }


        public CharterIn CreateCharterIn(long id, long chartererId, long ownerId, long vesselInCompanyId, long currencyId, DateTime actionDate, List<CharterItem> charterItems, List<InventoryOperation> inventoryOperationItems, CharterType charterType, CharterEndType charterEndType, OffHirePricingType offHirePricingType, long userId)
        {
            var charterIn = new CharterIn(id, chartererId, ownerId, vesselInCompanyId,
                               currencyId, actionDate,
                               charterItems, inventoryOperationItems
                              , charterType, charterEndType, offHirePricingType,
                              _charterInDomainService, _charterOutDomainService,
                              _eventPublisher);

            var initWorkflowStep =
                charterType == CharterType.Start
                    ? this._workflowStepRepository.Single(
                        c => c.Workflow.WorkflowEntity == WorkflowEntities.CharterInStart &&
                             c.CurrentWorkflowStage == WorkflowStages.Initial &&
                             c.Workflow.CompanyId == chartererId &&
                             c.Workflow.Name == Workflow.DEFAULT_NAME)
                    : this._workflowStepRepository.Single(
                        c => c.Workflow.WorkflowEntity == WorkflowEntities.CharterInEnd &&
                             c.CurrentWorkflowStage == WorkflowStages.Initial &&
                             c.Workflow.CompanyId == chartererId &&
                             c.Workflow.Name == Workflow.DEFAULT_NAME);


            if (initWorkflowStep == null)
                throw new ObjectNotFound(string.Format("CharterIn{0}InitialStep", charterType));

            var charterWorkflowLog = new CharterWorkflowLog(charterIn,
                initWorkflowStep.Workflow.WorkflowEntity,
                DateTime.Now,
                WorkflowActions.Init,
                userId,
                "",
                initWorkflowStep.Id,
                true);

            charterIn.ApproveWorkflows.Add(charterWorkflowLog);

            return charterIn;

        }

        public CharterIn ResolveCharterIn(CharterIn charter)
        {
            charter.Resolve(_charterInDomainService, _charterOutDomainService, _eventPublisher, vesselInCompanyDomainService, inventoryOperationNotifier);
            return charter;
        }

        public CharterIn ReCreateCharterIn(CharterIn charter)
        {
            charter.Resolve(_charterInDomainService, _charterOutDomainService, _eventPublisher, vesselInCompanyDomainService, inventoryOperationNotifier);

            //var init = this._workflowRepository.Single(c => c.WorkflowEntity == WorkflowEntities.CharterIn && c.CurrentWorkflowStage == WorkflowStages.Initial);
            //if (init == null)
            //    throw new ObjectNotFound("CharterInitialStep");

            //var charterWorkflowLog = new CharterWorkflowLog(charter, WorkflowEntities.CharterIn, DateTime.Now, WorkflowActions.Init, 1, "", init.Id, true);

            //charter.ApproveWorkflows.Add(charterWorkflowLog);

            return charter;

        }


        public CharterOut ResolveCharterOut(CharterOut charter)
        {
            charter.Resolve(_charterInDomainService, _charterOutDomainService, _eventPublisher, vesselInCompanyDomainService, inventoryOperationNotifier);
            return charter;
        }

        public CharterOut CreateCharterOut(long id, long chartererId, long ownerId, long vesselInCompanyId, long currencyId,
                DateTime actionDate,
                List<CharterItem> charterItems,
                List<InventoryOperation> inventoryOperationItems
                , CharterType charterType, CharterEndType charterEndType, OffHirePricingType offHirePricingType, long userId)
        {
            var charterOut = new CharterOut(id, chartererId, ownerId, vesselInCompanyId,
                               currencyId, actionDate,
                               charterItems, inventoryOperationItems
                              , charterType, charterEndType, offHirePricingType,
                              _charterInDomainService, _charterOutDomainService, _eventPublisher);

            var initWorkflowStep =
                charterType == CharterType.Start ?
                this._workflowStepRepository.Single(
                        c => c.Workflow.WorkflowEntity == WorkflowEntities.CharterOutStart &&
                             c.CurrentWorkflowStage == WorkflowStages.Initial &&
                             c.Workflow.CompanyId == ownerId &&
                             c.Workflow.Name == Workflow.DEFAULT_NAME)
                    : this._workflowStepRepository.Single(
                        c => c.Workflow.WorkflowEntity == WorkflowEntities.CharterOutEnd &&
                             c.CurrentWorkflowStage == WorkflowStages.Initial &&
                             c.Workflow.CompanyId == ownerId &&
                             c.Workflow.Name == Workflow.DEFAULT_NAME);

            if (initWorkflowStep == null)
                throw new ObjectNotFound(string.Format("CharterOut{0}InitialStep", charterType));

            var charterWorkflowLog = new CharterWorkflowLog(charterOut, initWorkflowStep.Workflow.WorkflowEntity, DateTime.Now, WorkflowActions.Init,
                userId, "", initWorkflowStep.Id, true);

            charterOut.ApproveWorkflows.Add(charterWorkflowLog);

            return charterOut;
        }


        public CharterOut ReCreateCharterOut(CharterOut charter)
        {
            charter.Resolve(_charterInDomainService, _charterOutDomainService, _eventPublisher, vesselInCompanyDomainService, inventoryOperationNotifier);

            //var init = this._workflowRepository.Single(c => c.WorkflowEntity == WorkflowEntities.CharterOut && c.CurrentWorkflowStage == WorkflowStages.Initial);
            //if (init == null)
            //    throw new ObjectNotFound("CharterOutitialStep");

            //var charterWorkflowLog = new CharterWorkflowLog(charter, WorkflowEntities.CharterOut, DateTime.Now, WorkflowActions.Init, 1, "", init.Id, true);

            //charter.ApproveWorkflows.Add(charterWorkflowLog);

            return charter;
        }


        public CharterItem CraeteCharterItem(long id, long charterId, decimal rob, decimal fee,
                                         decimal feeOffhire, long goodId, long tankId, long unitId)
        {
            return new CharterItem(id, charterId, rob, fee, feeOffhire, goodId, tankId, unitId);
        }



    }
}
