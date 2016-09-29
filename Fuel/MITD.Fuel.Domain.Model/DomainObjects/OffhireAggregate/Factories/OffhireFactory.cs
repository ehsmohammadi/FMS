using System;
using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public class OffhireFactory : IOffhireFactory
    {
        private readonly IEntityConfigurator<Offhire> offhireConfigurator;
        private readonly IOffhireDomainService offhireDomainService;
        private readonly IWorkflowStepRepository _workflowStepRepository;
        private readonly IVesselInCompanyDomainService vesselDomainService;
        private readonly ICompanyDomainService companyDomainService;
        private readonly ITankDomainService tankDomainService;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IGoodDomainService goodDomainService;
        private readonly IGoodUnitDomainService goodUnitDomainService;
        private readonly IOffhireManagementSystemDomainService offhireManagementSystemDomainService;
        private readonly IVoyageDomainService voyageDomainService;
        private readonly IActivityLocationDomainService activityLocationDomainService;

        public OffhireFactory(
            IEntityConfigurator<Offhire> offhireConfigurator,
            IWorkflowStepRepository _workflowStepRepository,
            IOffhireDomainService offhireDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            ICompanyDomainService companyDomainService,
            ITankDomainService tankDomainService,
            ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService,
            IGoodUnitDomainService goodUnitDomainService,
            IOffhireManagementSystemDomainService offhireManagementSystemDomainService,
            IVoyageDomainService voyageDomainService,
            IActivityLocationDomainService activityLocationDomainService)
        {
            this.offhireConfigurator = offhireConfigurator;
            this._workflowStepRepository = _workflowStepRepository;
            this.offhireDomainService = offhireDomainService;
            this.vesselDomainService = vesselDomainService;
            this.companyDomainService = companyDomainService;
            this.tankDomainService = tankDomainService;
            this.currencyDomainService = currencyDomainService;
            this.goodDomainService = goodDomainService;
            this.goodUnitDomainService = goodUnitDomainService;
            this.offhireManagementSystemDomainService = offhireManagementSystemDomainService;
            this.voyageDomainService = voyageDomainService;
            this.activityLocationDomainService = activityLocationDomainService;
        }

        public Offhire CreateOffhire(long referenceNumber, DateTime startDateTime, DateTime endDateTime, Company introducer, VesselInCompany vesselInCompany, Voyage voyage, ActivityLocation offhireLocation, DateTime voucherDate, Currency voucherCurrency, long userId)
        {
            var offhire = new Offhire(
                referenceNumber,
                startDateTime,
                endDateTime,
                introducer,
                vesselInCompany,
                voyage,
                offhireLocation,
                voucherDate,
                voucherCurrency,
                this.offhireDomainService,
                this.offhireManagementSystemDomainService,
                this.vesselDomainService,
                this.voyageDomainService,
                this.companyDomainService,
                this.activityLocationDomainService,
                this.currencyDomainService);

            var initWorkflowStep = this._workflowStepRepository.Single(c => c.Workflow.WorkflowEntity == WorkflowEntities.Offhire &&
                         c.CurrentWorkflowStage == WorkflowStages.Initial &&
                         c.Workflow.CompanyId == introducer.Id &&
                         c.Workflow.Name == Workflow.DEFAULT_NAME);

            if (initWorkflowStep == null)
                throw new ObjectNotFound("OffhireInitialStep");

            var offhireWorkflow = new OffhireWorkflowLog(offhire, WorkflowEntities.Offhire, DateTime.Now, WorkflowActions.Init,
                userId, "", initWorkflowStep.Id, true);

            offhire.ApproveWorkflows.Add(offhireWorkflow);

            offhireConfigurator.Configure(offhire);

            return offhire;
        }

        public OffhireDetail CreateOffhireDetail(decimal quantity, decimal feeInVoucherCurrency, decimal feeInMainCurrency, Good good, GoodUnit unit, Tank tank, Offhire offhire)
        {
            var offhireDetail = new OffhireDetail(
                quantity, feeInVoucherCurrency, feeInMainCurrency, good, unit, tank, offhire, offhireDomainService,
                tankDomainService, currencyDomainService, goodDomainService, goodUnitDomainService);

            return offhireDetail;
        }
    }
}