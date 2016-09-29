using System;
using System.Data;
using System.Linq;
using System.Transactions;
using MITD.Core;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;
using CharterType = MITD.Fuel.Domain.Model.Enums.CharterType;

namespace MITD.Fuel.Application.Service
{
    public class FuelReportApplicationService : IFuelReportApplicationService
    {
        private readonly IUnitOfWorkScope unitOfWorkScope;
        private readonly IFuelReportRepository fuelReportRepository;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IVoyageDomainService voyageDomainService;
        private readonly IFuelReportDomainService fuelReportDomainService;
        private readonly IWorkflowApplicationService workflowApplicationService;
        private readonly IVesselInCompanyRepository vesselInCompanyRepository;
        private readonly IVesselInInventoryRepository vesselInInventoryRepository;
        private readonly IFuelReportFactory fuelReportFactory;
        private readonly IVoyageRepository voyageRepository;
        private readonly IFuelUserRepository fuelUserRepository;
        private readonly IRepository<Tank> tankRepository;
        private readonly IGoodRepository goodRepository;
        private readonly IRepository<GoodUnit> goodUnitRepository;
        private readonly ICharterOutRepository charterOutRepository;
        private readonly ICharterInRepository charterInRepository;
        private readonly IFuelUserDomainService fuelUserDomainService;
        private readonly IFuelReportStateFactory fuelReportStateFactory;
        private readonly IWorkflowStepRepository workflowStepRepository;
        private readonly ICharteringDomainService charteringDomainService;

        public FuelReportApplicationService(
            IUnitOfWorkScope unitOfWorkScope,
            IFuelReportRepository fuelReportRepository,
            ICurrencyDomainService currencyDomainService,
            IVoyageDomainService voyageDomainService,
            IFuelReportDomainService fuelReportDomainService,
            IWorkflowApplicationService workflowApplicationService,
            IVesselInCompanyRepository vesselInCompanyRepository,
            IVesselInInventoryRepository vesselInInventoryRepository,
            IFuelReportFactory fuelReportFactory,
            IVoyageRepository voyageRepository,
            IFuelUserRepository fuelUserRepository,
            IRepository<Tank> tankRepository,
            IGoodRepository goodRepository,
            IRepository<GoodUnit> goodUnitRepository, ICharterOutRepository charterOutRepository, ICharterInRepository charterInRepository, IFuelUserDomainService fuelUserDomainService, IFuelReportStateFactory fuelReportStateFactory, IWorkflowStepRepository workflowStepRepository, ICharteringDomainService charteringDomainService)
        {
            this.unitOfWorkScope = unitOfWorkScope;
            this.fuelReportRepository = fuelReportRepository;
            this.currencyDomainService = currencyDomainService;
            this.voyageDomainService = voyageDomainService;
            this.fuelReportDomainService = fuelReportDomainService;
            this.workflowApplicationService = workflowApplicationService;

            this.vesselInCompanyRepository = vesselInCompanyRepository;
            this.vesselInInventoryRepository = vesselInInventoryRepository;
            this.fuelReportFactory = fuelReportFactory;
            this.voyageRepository = voyageRepository;
            this.fuelUserRepository = fuelUserRepository;
            this.tankRepository = tankRepository;
            this.goodRepository = goodRepository;
            this.goodUnitRepository = goodUnitRepository;
            this.charterOutRepository = charterOutRepository;
            this.charterInRepository = charterInRepository;
            this.fuelUserDomainService = fuelUserDomainService;
            this.fuelReportStateFactory = fuelReportStateFactory;
            this.workflowStepRepository = workflowStepRepository;
            this.charteringDomainService = charteringDomainService;
        }

        //================================================================================

        /// <summary>
        /// Handles the request from EventReport Importer console app, to add new FuelReport or update an existing one by sent data.
        /// </summary>
        /// <param name="data">A dto that is created by console app which contains Fuel Report Type, Date and fuels data</param>
        /// <returns>Created or Updated FuelReport</returns>
        public FuelReport ManageCommand(FuelReportCommandDto data)
        {
            try
            {
                //var vesselInCompany = vesselInCompanyRepository.Single(vic => vic.Vessel.Code == data.VesselCode && (vic.VesselStateCode == VesselStates.Owned || vic.VesselStateCode == VesselStates.CharterIn));

                //if (vesselInCompany == null) throw new ObjectNotFound("vesselInCompany for " + data.VesselCode);

                var charterContract = charteringDomainService.GetLastCharterContract(data.VesselCode, data.EventDate);  //Get last availabe charter contract to find the active vessel in a company.

                VesselInCompany vesselInCompany = null;

                var isVesselInActiveState = false;

                if (charterContract == null)
                {
                    //No charter is found, so the vessel code with owned status shoud be searched for.
                    vesselInCompany = vesselInCompanyRepository.Single(vic => vic.Vessel.Code == data.VesselCode && (vic.VesselStateCode == VesselStates.Owned || vic.VesselStateCode == VesselStates.CharterIn));

                    if (vesselInCompany == null) throw new ObjectNotFound("vesselInCompany for " + data.VesselCode);  //The vessel in company is required for the process.

                    isVesselInActiveState = true;
                }
                else
                {
                    //The vessel in company is taken from charter contract.
                    vesselInCompany = charterContract.VesselInCompany;

                    if (charterContract is CharterOut && charterContract.CharterType == CharterType.Start)
                        isVesselInActiveState = false;
                    else
                    {
                        isVesselInActiveState = true;
                    }
                }

                //Find any existing FuelReport in FMS with given vesselInCompany, EventDateTime, and fuel report type (noon, end of voyage, Berting,....)
                var existingFuelReports = fuelReportRepository.Find(fr => fr.VesselInCompanyId == vesselInCompany.Id && fr.EventDate == data.EventDate && fr.FuelReportType == (FuelReportTypes)data.FuelReportType);

                if (!existingFuelReports.All(fr => fr.IsOpenToUpdateByEventReport())) //Check all found FuelReports for their status...
                {
                    //No open FuelReport is available to Update (All are in Submitted or Cancelled state), so
                    //the Fuel Report is marked with an available update, to inform the user.
                    var fuelReport = existingFuelReports.SingleOrDefault(fr => fr.State == States.Submitted);
                    if (fuelReport == null) return null;
                    fuelReport.HasUpdateRequest = true;
                    unitOfWorkScope.Commit();
                    throw new BusinessRuleException("3", "Fuel report is submitted, and ready to retry by vrms");
                }
                else
                {
                    //Find one available FuelReport to update...
                    var fuelReport = existingFuelReports.SingleOrDefault(fr => fr.IsOpenToUpdateByEventReport());

                    bool existsFuelReport = fuelReport != null;

                    //Find matching Voyage...
                    var voyage = voyageRepository.First(v => v.VesselInCompanyId == vesselInCompany.Id && ((v.StartDate <= data.EventDate && (!v.EndDate.HasValue || data.EventDate <= v.EndDate))));

                    //Find default import User to initiate the WorkFlow/StageFlow
                    var user = fuelUserRepository.Single(fu => fu.CompanyId == vesselInCompany.CompanyId && fu.Name.StartsWith("frimporter"));

                    //Check wether the fuelReport is available or not
                    if (!existsFuelReport) 
                        //The fuelReport is not found
                        //Create a new fuel report with given data, in FMS
                        fuelReport = fuelReportFactory.CreateFuelReport(data.VesselReportReference, data.Remark, data.EventDate, data.ReportDate, vesselInCompany.Id,
                                                                        voyage == null ? null : (long?)voyage.Id, (FuelReportTypes)data.FuelReportType, user.Id, isVesselInActiveState);
                    else
                    {
                        //Update found FuelReport HEADER with data from Console App.
                        if (voyage != null && isVesselInActiveState)
                            try
                            {
                                fuelReport.UpdateVoyageId(voyage.Id, this.voyageDomainService);
                            }
                            catch
                            {
                            }

                        fuelReport.UpdateCode(data.VesselReportReference, isVesselInActiveState);
                    }

                    if (isVesselInActiveState)
                        fuelReport.UpdateFirstPositionStatus(this.fuelReportDomainService);

                    //Iterate through all FR DETAILs from Console App...
                    foreach (var detail in data.FuelReportDetails)
                    {
                        //Find relevant good in FMS ...
                        var good = goodRepository.Single(g => g.CompanyId == vesselInCompany.CompanyId && g.Code == detail.FuelType);

                        if (good == null) continue;

                        //Find proper goodUnit ...
                        var goodUnit = goodUnitRepository.Single(g => g.GoodId == good.Id && g.Abbreviation == detail.MeasuringUnitCode);

                        //Find vesselInInventory to set default tank to FuelReport Detail...
                        var vesselInInventory = fuelReport.VesselInCompany.VesselInInventory;

                        //Find any availble mathcing FuelReport Detail in processing fuelReport (created or fetched in previous stages)
                        var existingDetail = fuelReport.FuelReportDetails.SingleOrDefault(frd => frd.GoodId == good.Id);

                        //If the processing fuelReport is CREATED or no mathcing FR Detail found, then a new FR Detail should be created and added to processing FuelReport.
                        if (!existsFuelReport || existingDetail == null)
                        {
                            var fuelReportDetail = fuelReportFactory.CreateFuelReportDetail(
                                                                                            0,
                                                                                            detail.ROB,
                                                                                            detail.MeasuringUnitCode,
                                                                                            detail.Consumption,
                                                                                            detail.Receive,
                                                                                            null,
                                                                                            detail.Transfer,
                                                                                            null,
                                                                                            detail.Correction,
                                                                                            detail.CorrectionType.HasValue ? (CorrectionTypes?)(detail.CorrectionType == CorrectionTypeEnum.Plus ? CorrectionTypes.Plus : CorrectionTypes.Minus) : null,
                                                                                            null,
                                                                                            null,
                                                                                            "USD",
                                                                                            null,
                                                                                            good.Id,
                                                                                            goodUnit.Id,
                                                                                            this.tankRepository.First(t => t.VesselInInventoryId == vesselInInventory.Id).Id);

                            fuelReport.FuelReportDetails.Add(fuelReportDetail);
                        }
                        else
                        {
                            //Update the matching FR Detail with given detail Data by console app.
                            fuelReport.UpdateFuelReportDetailFromEventReport(existingDetail.Id,
                                    detail.ROB,
                                    detail.Consumption,
                                    detail.Receive,
                                    detail.Transfer,
                                    detail.Correction,
                                    detail.CorrectionType.HasValue ? (CorrectionTypes?)(detail.CorrectionType == CorrectionTypeEnum.Plus ? CorrectionTypes.Plus : CorrectionTypes.Minus) : null,
                                    isVesselInActiveState);
                        }
                    }

                    if (!existsFuelReport)
                        fuelReportRepository.Add(fuelReport);
                    else
                    {
                        fuelReport.HasUpdateRequest=false;
                        fuelReportRepository.Update(fuelReport);
                    }
                    unitOfWorkScope.Commit();

                    return fuelReport;
                }
            }
            catch (FuelException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("Create", ex.Message + "\nStackTrace:\n" + ex.StackTrace);
            }
        }

        //================================================================================

        public FuelReport GetById(long id)
        {
            var fuelReport = fuelReportDomainService.Get(id);

            if (fuelReport == null)
                throw new ObjectNotFound("FuelReport", id);

            return fuelReport;
        }

        //================================================================================

        public FuelReportDetail UpdateFuelReportDetail(
            long fuelReportId,
            long fuelReportDetailId,
            decimal rob,
            decimal consumption,
            decimal? receive,
            ReceiveTypes? receiveType,
            decimal? transfer,
            TransferTypes? transferType,
            decimal? correction,
            CorrectionTypes? correctionType,
            CorrectionPricingTypes? correctionPricingType,
            decimal? correctionPrice,
            long? currencyId,
            Reference transferReference,
            Reference receiveReference,
            Reference correctionReference,
            long? trustIssueInventoryTransactionItemId)
        {
            var currentFuelReport = GetById(fuelReportId);

            currentFuelReport.UpdateFirstPositionStatus(this.fuelReportDomainService);

            var result = currentFuelReport.UpdateFuelReportDetail(
                fuelReportDetailId,
                rob,
                consumption,
                receive,
                receiveType,
                transfer,
                transferType,
                correction,
                correctionType,
                correctionPricingType,
                correctionPrice,
                currencyId,
                transferReference,
                receiveReference,
                correctionReference,
                trustIssueInventoryTransactionItemId,
                fuelReportDomainService,
                currencyDomainService);


            try
            {
                this.unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateFuelReportDetail");
            }
            //            catch (Exception ex)
            //            {
            //                throw new UnHandleException(ex);
            //            }

            return result;
        }

        //================================================================================

        public FuelReport UpdateVoyageId(long fuelReportId, long voyageId)
        {
            var fuelReport = GetById(fuelReportId);

            fuelReport.UpdateFirstPositionStatus(this.fuelReportDomainService);

            fuelReport.UpdateVoyageId(voyageId, this.voyageDomainService);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateVoyage");
            }

            return fuelReport;
        }

        //================================================================================

        public void IsSetFuelReportInventoryResultPossible(long fuelReportId)
        {
            //Implemented for integration with Nader Mohammadi Inventory - NOT ACTIVE ANYMORE

            var fuelReport = GetById(fuelReportId);

            fuelReport.IsWaitingToBeClosed();
        }

        //================================================================================

        public void SetFuelReportInventoryResults(InventoryResultCommand resultBag)
        {
            //Implemented for integration with Nader Mohammadi Inventory - NOT ACTIVE ANYMORE

            var fuelReport = GetById(resultBag.FuelReportId);

            this.fuelReportDomainService.SetFuelReportInventoryResults(resultBag, fuelReport);

            try
            {
                this.unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("SetFuelReportInventoryResults");
            }
        }

        //================================================================================

        public FuelReport UpdateVoyageEndOfVoyageFuelReport(long voyageId, DateTime newDateTime)
        {
            //Implemented for proper integration with Rotation system, which requires triggers from Rotation system
            //This method could be used for cases when the EOV date/time is changed.
            var eovFuelReport = this.fuelReportDomainService.GetVoyageValidEndOfVoyageFuelReport(voyageId);

            eovFuelReport.UpdateFirstPositionStatus(this.fuelReportDomainService);

            var changingFuelReportData = this.fuelReportDomainService.GetChangingFuelReportDateData(eovFuelReport.Id, newDateTime);


            this.workflowApplicationService.MoveToNextStep(changingFuelReportData.ChangingFuelReport.Id, WorkflowActionEntityType.FuelReport, 1, "", WorkflowActions.Reject);

            if (changingFuelReportData.NextFuelReportAfterChangeDate != null && changingFuelReportData.NextFuelReportAfterChangeDate.State == States.Submitted)
                this.workflowApplicationService.MoveToNextStep(changingFuelReportData.NextFuelReportAfterChangeDate.Id, WorkflowActionEntityType.FuelReport, 1, "", WorkflowActions.Reject);

            if (changingFuelReportData.NextFuelReportBeforeChangeDate != null && changingFuelReportData.NextFuelReportBeforeChangeDate.State == States.Submitted)
                this.workflowApplicationService.MoveToNextStep(changingFuelReportData.NextFuelReportBeforeChangeDate.Id, WorkflowActionEntityType.FuelReport, 1, "", WorkflowActions.Reject);

            changingFuelReportData.ChangingFuelReport.UpdateEventDate(newDateTime);


            if (changingFuelReportData.NextFuelReportBeforeChangeDate != null)
                changingFuelReportData.NextFuelReportBeforeChangeDate.UpdateConsumption(fuelReportDomainService);

            changingFuelReportData.ChangingFuelReport.UpdateConsumption(fuelReportDomainService);

            if (changingFuelReportData.NextFuelReportAfterChangeDate != null)
                changingFuelReportData.NextFuelReportAfterChangeDate.UpdateConsumption(fuelReportDomainService);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateVoyageEndOfVoyageFuelReport");
            }

            return changingFuelReportData.ChangingFuelReport;
        }

        //================================================================================

        /// <summary>
        /// Updates not submitted FuelReports voyages with mathcing Voyage based on EventDateTime
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="vesselInCompanyId"></param>
        public void RefreshFuelReportsVoyage(long companyId, long? vesselInCompanyId)
        {
            IsFuelReportOpen isFuelReportOpen = new IsFuelReportOpen();

            var companyDomainService = ServiceLocator.Current.GetInstance<ICompanyDomainService>();

            var company = companyDomainService.Get(companyId);

            var vesselsInCompany = company.VesselsOperationInCompany.Where(vic => !vesselInCompanyId.HasValue || vic.Id == vesselInCompanyId.Value).ToList();

            foreach (var vesselInCompany in vesselsInCompany)
            {
                //todo: it has smelling  
                var openFuelReportIdList = fuelReportRepository.FindOpenFuelReportIdByVesselInCompany(vesselInCompanyId);
                //var openFuelReports = fuelReportRepository.Find(
                //    Extensions.And(isFuelReportOpen.Predicate, fr => fr.VesselInCompanyId == vesselInCompany.Id)).OrderBy(fr => fr.EventDate).ToList();

                var vesselInCompanyVoyages = voyageDomainService.GetByFilter(companyId, vesselInCompany.Id);

                openFuelReportIdList.ForEach(fr =>
                {
                    var fuelReport = fuelReportRepository.FindByKey(fr);
                    fuelReport.UpdateFirstPositionStatus(this.fuelReportDomainService);

                    if (!fuelReport.IsVoyageValid(voyageDomainService))
                    {
                        try
                        {
                            var voyage = vesselInCompanyVoyages.First(v => v.StartDate <= fuelReport.EventDate &&
                                fuelReport.EventDate <= (v.EndDate.HasValue ? v.EndDate : DateTime.MaxValue));

                            if (voyage != null)
                                fuelReport.UpdateVoyageId(voyage.Id, this.voyageDomainService);
                        }
                        catch
                        {
                        }
                    }
                });
            }
            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("UpdateVoyageEndOfVoyageFuelReport");
            }

        }

        //================================================================================

        public void Delete(long id)
        {
            var fuelReport = GetById(id);

            var userId = fuelUserDomainService.GetCurrentFuelUserId();

            fuelReport.Delete(fuelReportDomainService, fuelReportStateFactory, workflowStepRepository, userId);

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("DeleteFuelReport");
            }
        }

        //================================================================================

        public void RevertFuelReportConsumptionInventoryOperations(long fuelReportId)
        {
            //TODO: These method is implemented in FR reject scenario, and is redundant .

            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                var fuelReport = GetById(fuelReportId);

                var userId = fuelUserDomainService.GetCurrentFuelUserId();

                fuelReport.RevertFuelReportInventoryOperations(userId);

                try
                {
                    unitOfWorkScope.Commit();
                }
                catch (OptimisticConcurrencyException ex)
                {
                    throw new ConcurencyException("DeleteFuelReport");
                }

                transactionScope.Complete();
            }
        }

        //================================================================================

        public void RevertFuelReportDetailCorrectionInventoryOperations(long fuelReportId, long fuelReportDetailId)
        {
            //TODO: These method is implemented in FR reject scenario, and is redundant .
            throw new NotImplementedException();
        }

        //================================================================================

        public void RevertFuelReportDetailReceiveInventoryOperations(long fuelReportId, long fuelReportDetailId)
        {
            //TODO: These method is implemented in FR reject scenario, and is redundant .
            throw new NotImplementedException();
        }

        //================================================================================

        public void RevertFuelReportDetailTransferInventoryOperations(long fuelReportId, long fuelReportDetailId)
        {
            //TODO: These method is implemented in FR reject scenario, and is redundant .
            var fuelReport = GetById(fuelReportId);

            //fuelReport.RevertFuelReportDetailInventoryOperations();

            try
            {
                unitOfWorkScope.Commit();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new ConcurencyException("DeleteFuelReport");
            }
        }

        //================================================================================
    }
}
