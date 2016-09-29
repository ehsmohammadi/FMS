using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.DomainObjects.Factories
{
    public class FuelReportFactory : IFuelReportFactory
    {
        private readonly IEntityConfigurator<FuelReport> fuelReportConfigurator;
        private readonly IWorkflowStepRepository _workflowStepRepository;
        private readonly IVesselInCompanyRepository vesselInCompanyRepository;
        
        //private readonly IVesselInCompanyDomainService vesselDomainService;
        //private readonly ICompanyDomainService companyDomainService;
        //private readonly ITankDomainService tankDomainService;
        //private readonly ICurrencyDomainService currencyDomainService;
        //private readonly IGoodDomainService goodDomainService;
        //private readonly IGoodUnitDomainService goodUnitDomainService;
        //private readonly IOffhireManagementSystemDomainService offhireManagementSystemDomainService;
        //private readonly IVoyageDomainService voyageDomainService;
        //private readonly IActivityLocationDomainService activityLocationDomainService;

        public FuelReportFactory(
            IEntityConfigurator<FuelReport> fuelReportConfigurator,
            IWorkflowStepRepository _workflowStepRepository, 
            IVesselInCompanyRepository vesselInCompanyRepository /*,
            IOffhireDomainService offhireDomainService,
            IVesselInCompanyDomainService vesselDomainService,
            ICompanyDomainService companyDomainService,
            ITankDomainService tankDomainService,
            ICurrencyDomainService currencyDomainService,
            IGoodDomainService goodDomainService,
            IGoodUnitDomainService goodUnitDomainService,
            IOffhireManagementSystemDomainService offhireManagementSystemDomainService,
            IVoyageDomainService voyageDomainService,
            IActivityLocationDomainService activityLocationDomainService*/)
        {
            this.fuelReportConfigurator = fuelReportConfigurator;
            this._workflowStepRepository = _workflowStepRepository;
            //this.vesselInInventoryRepository = vesselInInventoryRepository;
            this.vesselInCompanyRepository = vesselInCompanyRepository;
            
        }

        public FuelReport CreateFuelReport(string code, string description, DateTime eventDate, DateTime reportDate, long vesselInCompanyId, long? voyageId, FuelReportTypes fuelReportType, long userId, bool isVesselInActiveState)
        {
            var vesselInCompany = vesselInCompanyRepository.First(vic => vic.Id == vesselInCompanyId);

            var fuelReport = new FuelReport(
                code,
                description,
                eventDate,
                reportDate,
                vesselInCompany,
                voyageId,
                fuelReportType,
                isVesselInActiveState ? States.Open : States.Cancelled);

            var workflowStageToFind = isVesselInActiveState ? WorkflowStages.Initial : WorkflowStages.Canceled;

            var initWorkflowStep = this._workflowStepRepository.Single(
                    c => c.Workflow.WorkflowEntity == WorkflowEntities.FuelReport &&
                         c.CurrentWorkflowStage == workflowStageToFind &&
                         c.Workflow.CompanyId == vesselInCompany.CompanyId &&
                         c.Workflow.Name == Workflow.DEFAULT_NAME);
                    //c.ActorUser.CompanyId == vesselInCompany.CompanyId &&
                    //c.ActorUser.IsFRApprover);

            if (initWorkflowStep == null)
                throw new ObjectNotFound("FuelReportInitialStep");

            var fuelReportWorkflow = new FuelReportWorkflowLog(-1, WorkflowEntities.FuelReport, DateTime.Now, WorkflowActions.Init,
                userId, 
                "", initWorkflowStep.Id, true);

            fuelReport.ApproveWorkFlows.Add(fuelReportWorkflow);

            fuelReportConfigurator.Configure(fuelReport);

            return fuelReport;
        }

        public FuelReportDetail CreateFuelReportDetail(long fuelReportId,
            decimal rob,
            string robUOM,
            decimal consumption,
            decimal? receive,
            ReceiveTypes? receiveType,
            decimal? transfer,
            TransferTypes? transferType,
            decimal? correction,
            CorrectionTypes? correctionType,
            CorrectionPricingTypes? correctionPricingType,
            decimal? correctionPrice,
            string correctionPriceCurrencyISOCode,
            long? correctionPriceCurrencyId,
            long fuelTypeId,
            long measuringUnitId,
            long tankId)
        {
            var fuelReportDetail = new FuelReportDetail(
                0, 
                fuelReportId,
                rob,
                robUOM,
                consumption,
                receive,
                receiveType,
                transfer,
                transferType,
                correction,
                correctionType,
                correctionPricingType,
                correctionPrice,
                correctionPriceCurrencyISOCode,
                correctionPriceCurrencyId,
                fuelTypeId,
                measuringUnitId,
                tankId);

            return fuelReportDetail;
        }

}
}