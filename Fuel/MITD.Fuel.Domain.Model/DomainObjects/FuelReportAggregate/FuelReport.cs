using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class FuelReport
    {
        #region Properties

        public long Id { get; private set; }

        public string Code { get; private set; }

        public string Description { get; private set; }

        public DateTime EventDate { get; private set; }

        public DateTime ReportDate { get; private set; }

        public long VesselInCompanyId { get; private set; }

        public virtual VesselInCompany VesselInCompany { get; private set; }

        public long? VoyageId { get; private set; }

        public virtual Voyage Voyage { get; private set; }

        public FuelReportTypes FuelReportType { get; private set; }

        public virtual List<FuelReportWorkflowLog> ApproveWorkFlows { get; private set; }

        public byte[] TimeStamp { get; private set; }

        public virtual ICollection<FuelReportDetail> FuelReportDetails { get; private set; }

        public States State { get; private set; }

        public FuelReportState EntityState { get; private set; }

        public virtual List<InventoryOperation> ConsumptionInventoryOperations { get; set; }

        public bool IsTheFirstReport { get; set; }
        public long? ActivationCharterContractId { get; set; }
        public virtual Charter ActivationCharterContract { get; set; }

        public long? CreatedCharterId { get; set; }

        public virtual Charter CreatedCharter { get; set; }
        public bool HasUpdateRequest { get; set; }

        private readonly IsFuelReportClosed isFuelReportClosed;

        private readonly IsFuelReportOpen isFuelReportOpen;

        private readonly IsFuelReportWaitingToBeClosed isFuelReportWaitingToBeClosed;

        private readonly IsFuelReportOperational isFuelReportOperational;

        private readonly IsFuelReportNotCancelled isFuelReportNotCancelled;

        private readonly IsFuelReportSubmitRejected isFuelReportSubmitRejected;

        private readonly IsFuelReportSubmitted isFuelReportSubmitted;

        private readonly IsFuelReportSubmittedByCommercial isFuelReportSubmittedByCommercial;
        private readonly IsFuelReportSubmittedByFinance isFuelReportSubmittedByFinance;

        #endregion

        #region Constructors

        public FuelReport()
        {
            this.isFuelReportClosed = new IsFuelReportClosed();

            this.isFuelReportOpen = new IsFuelReportOpen();

            this.isFuelReportWaitingToBeClosed = new IsFuelReportWaitingToBeClosed();

            this.isFuelReportOperational = new IsFuelReportOperational();
            this.isFuelReportNotCancelled = new IsFuelReportNotCancelled();
            this.isFuelReportSubmitRejected = new IsFuelReportSubmitRejected();
            this.isFuelReportSubmitted = new IsFuelReportSubmitted();
            this.isFuelReportSubmittedByCommercial = new IsFuelReportSubmittedByCommercial();
            this.isFuelReportSubmittedByFinance = new IsFuelReportSubmittedByFinance();

            this.ApproveWorkFlows = new List<FuelReportWorkflowLog>();
            this.FuelReportDetails = new Collection<FuelReportDetail>();
            this.ConsumptionInventoryOperations = new List<InventoryOperation>();
        }

        internal FuelReport(
            string code,
            string description,
            DateTime eventDate,
            DateTime reportDate,
            long vesselInCompanyId,
            long? voyageId,
            FuelReportTypes fuelReportType,
            States state)
            : this()
        {
            this.Code = code;

            this.Description = description;

            this.EventDate = eventDate;

            this.ReportDate = reportDate;

            this.VesselInCompanyId = vesselInCompanyId;

            this.VoyageId = voyageId;


            if (!Enum.IsDefined(typeof(FuelReportTypes), fuelReportType))
                throw new InvalidArgument("The FuelReport Type is invalid.", "fuelReportType");

            this.FuelReportType = fuelReportType;

            this.State = state;
            HasUpdateRequest = false;
        }

        internal FuelReport(
            string code,
            string description,
            DateTime eventDate,
            DateTime reportDate,
            VesselInCompany vesselInCompany,
            long? voyageId,
            FuelReportTypes fuelReportType,
            States state)
            : this(code, description, eventDate, reportDate, vesselInCompany.Id, voyageId, fuelReportType, state)
        {
            this.VesselInCompany = vesselInCompany;
        }

        #endregion

        #region Methods

        //===================================================================================

        public void UpdateVoyageId(long? voyageId, IVoyageDomainService voyageDomainService)
        {
            if (this.IsNotCancelled())
            {
                this.CheckToBeOperational();
            }

            //checkToHaveNoQuantityInInventory();

            this.validateVoyageId(voyageId, voyageDomainService);

            this.VoyageId = voyageId;
        }

        //===================================================================================

        private void validateVoyageId(long? voyageId, IVoyageDomainService voyageDomainService)
        {
            //BR_FR2
            this.validateVoyageValue(voyageId, voyageDomainService);

            //BR_FR35
            this.validateVoyageForEndOfVoyageCondition(voyageId);

            //BR_FR36
            this.validateVoyageEndDateForEndOfVoyageReportType(voyageId, voyageDomainService);
        }

        //===================================================================================

        public bool IsVoyageValid(IVoyageDomainService voyageDomainService)
        {
            try
            {
                this.validateVoyageId(this.VoyageId, voyageDomainService);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //===================================================================================

        #endregion

        #region FuelReportDetail Operations

        //===================================================================================

        public FuelReportDetail UpdateFuelReportDetail(long fuelReportDetailId, decimal rob, decimal consumption, decimal? receive, ReceiveTypes? receiveType, decimal? transfer, TransferTypes? transferType, decimal? correction, CorrectionTypes? correctionType, CorrectionPricingTypes? correctionPricingType, decimal? correctionPrice, long? currencyId, Reference transferReference, Reference receiveReference, Reference correctionReference, long? trustIssueInventoryTransactionItemId, IFuelReportDomainService fuelReportDomainService, ICurrencyDomainService currencyDomainService)
        {
            this.CheckToBeOperational();

            FuelReportDetail updatingFuelReportDetail = this.FuelReportDetails.FirstOrDefault(c => c.Id == fuelReportDetailId);

            if (updatingFuelReportDetail == null) throw new ObjectNotFound("FuelReportDetail", fuelReportDetailId);

            this.validatePreviousFuelReportsToBeFinalApproved(fuelReportDomainService);

            #region Commented 1
            //IOrderedEnumerable<FuelReport> fuelReportsOfYesterday =
            //    fuelReportDomainService
            //        .GetYesterdayFuelReports(this)
            //            .OrderBy(fr => fr.ReportDate);//The ordering is used to find the last report in previous day.

            //FuelReport previousFuelReport = null;

            //bool isTheFirstReport = false;

            //if (fuelReportsOfYesterday.Count() == 0)
            //{
            //    //If nothing found for previous day...

            //    isTheFirstReport = isCurrentFuelReportTheFirstOne(fuelReportDomainService);
            //}
            //else
            //{
            //    //Retrieve the last Fuel Report of yesterday. 
            //    previousFuelReport = fuelReportsOfYesterday.LastOrDefault();
            //}


            //FuelReportDetail fuelReportDetailOfYesterdayForRelevantGood = null;

            //if (!isTheFirstReport)
            //{
            //    //TODO: The validation of fuel types against Yesterday valid fuel types must be revised.
            //    fuelReportDetailOfYesterdayForRelevantGood =
            //        previousFuelReport.FuelReportDetails.FirstOrDefault(c => c.GoodId == updatingFuelReportDetail.GoodId);

            //    if (fuelReportDetailOfYesterdayForRelevantGood == null)
            //        //Because current Fuel Report is not the first one, 
            //        //its relevant Fuel Report Detail of yesterday must be found.
            //        throw new ObjectNotFound("FuelReportDetailOfYesterdayForRelevantGood");
            //}

            #endregion

            //bool isTheFirstReport = fuelReportDomainService.IsFuelReportTheFirstOneInVesselActivePeriod(this);
            bool isTheFirstReport = this.IsTheFirstReport;

            FuelReport previousFuelReport = this.getLastFuelReportOfYesterday(fuelReportDomainService);

            this.validatePreviousFuelReport(previousFuelReport, isTheFirstReport);

            FuelReportDetail fuelReportDetailOfYesterdayForRelevantGood = this.getGoodRelevantFuelReportDetailOfYesterday(
                    updatingFuelReportDetail.GoodId,
                    isTheFirstReport,
                    previousFuelReport);


            //Charter activationCharterContract = null;
            //if (isTheFirstReport)
            //{
            //    activationCharterContract = fuelReportDomainService.GetVesselActivationCharterContract(this);
            //}
            var activationCharterContract = this.ActivationCharterContract;

            CharterItem relevantCharterItemAtFuelReprotTime = null;

            //ROB Comparison with the Activation Charter contract. 
            if (isTheFirstReport && activationCharterContract != null && this.EventDate == activationCharterContract.ActionDate)
            {
                //relevantCharterItemAtFuelReprotTime = activationCharterContract.CharterItems.SingleOrDefault(ci => ci.GoodId == updatingFuelReportDetail.GoodId);
                relevantCharterItemAtFuelReprotTime = updatingFuelReportDetail.GetRelevantActivationCharterItem();
            }


            if ((this.State == States.Open) || this.IsSubmittedByCommercial() || (this.State == States.SubmitRejected))
            {
                updatingFuelReportDetail.Update(rob, consumption, receive, receiveType, transfer, transferType,
                                                correction, correctionType, correctionPricingType, correctionPrice, currencyId,
                                                transferReference,
                                                receiveReference,
                                                correctionReference,
                                                fuelReportDetailOfYesterdayForRelevantGood,
                                                isTheFirstReport,
                                                relevantCharterItemAtFuelReprotTime,
                                                trustIssueInventoryTransactionItemId,
                                                currencyDomainService);
            }
            //else if (this.State == States.SubmitRejected)
            //{
            //    updatingFuelReportDetail.Update(rob, consumption, fuelReportDetailOfYesterdayForRelevantGood, isTheFirstReport, relevantCharterItemAtFuelReprotTime, currencyDomainService);
            //}
            else
            {
                throw new InvalidOperation("UpdateFuelReportDetail", "The Fuel Report is in an invalid state.");
            }

            return updatingFuelReportDetail;
        }

        //===================================================================================

        public FuelReportDetail UpdateFuelReportDetailFromEventReport(long fuelReportDetailId, decimal rob, decimal consumption, decimal? receive, decimal? transfer, decimal? correction, CorrectionTypes? correctionType, bool isVesselInActiveState)
        {
            if (isVesselInActiveState)
            {
                //BR_FR1
                //this.validateToBeOpenOrSubmitRejected();
                if (this.isFuelReportClosed.IsSatisfiedBy(this))
                    throw new BusinessRuleException("", string.Format("Fuel Report (Code : {0}) State is invalid.", this.Code));
            }

            //checkToHaveNoQuantityInInventory();

            FuelReportDetail updatingFuelReportDetail = this.FuelReportDetails.FirstOrDefault(c => c.Id == fuelReportDetailId);

            if (updatingFuelReportDetail == null) throw new ObjectNotFound("FuelReportDetail", fuelReportDetailId);

            updatingFuelReportDetail.UpdateFromEventReport(rob, consumption, receive, transfer, correction, correctionType);

            return updatingFuelReportDetail;
        }

        //===================================================================================

        private FuelReportDetail getGoodRelevantFuelReportDetailOfYesterday(long goodId, bool isTheFirstReport, FuelReport previousFuelReport)
        {
            FuelReportDetail fuelReportDetailOfYesterdayForRelevantGood = null;

            if (!isTheFirstReport)
            {
                //TODO: The validation of fuel types against Yesterday valid fuel types must be revised.
                fuelReportDetailOfYesterdayForRelevantGood =
                    previousFuelReport.FuelReportDetails.FirstOrDefault(c => c.GoodId == goodId);

                //Commented on 1394-12-17 as if new fuels should be considered in system process.
                //if (fuelReportDetailOfYesterdayForRelevantGood == null)
                //    //Because current Fuel Report is not the first one, 
                //    //its relevant Fuel Report Detail of yesterday must be found.
                //    throw new ObjectNotFound("FuelReportDetailOfYesterdayForRelevantGood", goodId);
            }

            return fuelReportDetailOfYesterdayForRelevantGood;
        }

        //===================================================================================

        private FuelReport getLastFuelReportOfYesterday(IFuelReportDomainService fuelReportDomainService)
        {
            IOrderedEnumerable<FuelReport> fuelReportsOfYesterday =
                fuelReportDomainService.GetFuelReportsFromYesterday(this)
                    .OrderBy(fr => fr.EventDate);//The ordering is used to find the last report from previous day till current Report.

            return fuelReportsOfYesterday.LastOrDefault();
        }

        //===================================================================================

        private void validateToBeOpenOrSubmitRejected()
        {
            if (!this.isFuelReportOpen.IsSatisfiedBy(this) && !this.isFuelReportSubmitRejected.IsSatisfiedBy(this))
                throw new BusinessRuleException("", string.Format("Fuel Report (Code : {0}) State is invalid.", this.Code));
        }

        //===================================================================================

        #endregion

        #region BusinessRules

        //===================================================================================

        /// <summary>
        /// BR_FR2
        /// </summary>
        private void validateVoyageValue(long? voyageId, IVoyageDomainService voyageDomainService)
        {
            if (!voyageId.HasValue ||
                !(
                    voyageDomainService.IsVoyageAvailable(voyageId.Value) && this.isVoyageDurationAndVesselValid(voyageId, voyageDomainService)
                )
            )
                throw new BusinessRuleException("BR_FR2", "Voyage is not Valid.");
        }

        //===================================================================================

        /// <summary>
        /// This validation is part of validateVoyageValue
        /// </summary>
        private bool isVoyageDurationAndVesselValid(long? voyageId, IVoyageDomainService voyageDomainService)
        {
            if (voyageId.HasValue)
            {
                var givenVoyage = voyageDomainService.Get(voyageId.Value);

                if (
                    !(
                        givenVoyage.VesselInCompany.Id == this.VesselInCompanyId &&
                        (
                            givenVoyage.StartDate <= this.EventDate && this.EventDate <= givenVoyage.EndDate.GetValueOrDefault(DateTime.MaxValue)
                        )
                    )
                )
                {
                    return false;
                }
            }

            return true;
        }

        //===================================================================================

        private void validatePreviousFuelReport(
            FuelReport fuelReportOfTheDayBefore,
            bool isCurrentFuelReportTheFirst)
        {
            if (fuelReportOfTheDayBefore == null && !isCurrentFuelReportTheFirst)
                //This means that current Fuel Report is not the first one in the whole system and 
                //has no Fuel Report in yesterday.
                throw new ObjectNotFound("FuelReportOfTheDayBefore"); //"No fuel report found for previous day.";


            if (fuelReportOfTheDayBefore != null)
            {
                //Checking FuelReport of the Day Before for final approval state.
                //TODO: The business rule code must be indicated.
                if (!this.isFuelReportClosed.IsSatisfiedBy(fuelReportOfTheDayBefore))
                    throw new BusinessRuleException("", "The previous Fuel Report must be Final Approved.");
            }
        }

        //===================================================================================

        /// <summary>
        /// BR_FR35
        /// </summary>
        private void validateVoyageForEndOfVoyageCondition(long? voyageId)
        {
            if (this.FuelReportType == FuelReportTypes.EndOfVoyage &&
                !voyageId.HasValue)
                throw new BusinessRuleException("BR_FR35", "Voyage is not specified for End Of Voyage Fuel Report.");
        }

        //===================================================================================

        //private void validateVoyageExistance(long? voyageId)
        //{
        //    if (FuelReportType == FuelReportTypes.EndOfVoyage &&
        //        !voyageId.HasValue)
        //        throw new BusinessRuleException("BR_FR35", "Voyage is mandatory for EOV Fuel Report.");
        //}

        //===================================================================================

        /// <summary>
        /// BR_FR36
        /// </summary>
        private void validateVoyageEndDateForEndOfVoyageReportType(
            long? voyageId,
            IVoyageDomainService voyageDomainService)
        {
            if (this.FuelReportType == FuelReportTypes.EndOfVoyage &&
                voyageId.HasValue)
            {
                var givenVoyage = voyageDomainService.Get(voyageId.Value);

                if (!givenVoyage.EndDate.HasValue)
                    throw new BusinessRuleException("", "Voyage has not ended or its EndDate is not reported yet.");

                if (!(
                        givenVoyage.VesselInCompany.Id == this.VesselInCompanyId && //This is already checked in BR_FR2, but implemented due to Analysis indication.
                        (//Compare found voyage End Date with current fuel Report Date with the resolution of hour and minutes.
                            this.EventDate.Date == givenVoyage.EndDate.Value.Date &&
                            this.EventDate.Hour == givenVoyage.EndDate.Value.Hour &&
                            this.EventDate.Minute == givenVoyage.EndDate.Value.Minute
                        )
                    )
                )
                {
                    throw new BusinessRuleException("BR_FR36", "Given Voyage is not match with Fuel Report Date and Vessel.");
                }
            }
        }

        //===================================================================================

        /// <summary>
        /// BR_FR27
        /// </summary>
        private void validateToBeInOpenState()
        {
            if (!this.IsOpen())
                throw new BusinessRuleException("BR_FR27", "Entity is not in Open state.");
        }

        public bool IsOpen()
        {
            return this.isFuelReportOpen.IsSatisfiedBy(this);
        }

        public bool IsOpenToUpdateByEventReport()
        {
            return !this.isFuelReportClosed.IsSatisfiedBy(this);
        }

        //===================================================================================

        #endregion

        #region Approve Workflow

        //===================================================================================

        private void validateSubmittingState(IVoyageDomainService voyageDomainService,
            IFuelReportDomainService fuelReportDomainService, IInventoryOperationDomainService inventoryOperationDomainService, IGoodDomainService goodDomainService,
            IOrderDomainService orderDomainService, ICurrencyDomainService currencyDomainService)
        {
            this.validateToBeOpenOrSubmitRejected();

            this.validateToNotHaveAnySubmittedFuelReportAfter(fuelReportDomainService);
            this.validateToNotHaveAnySubmittedFuelReportAtTheTimeOfCurrentReport(fuelReportDomainService);

            this.validateVoyageId(this.VoyageId, voyageDomainService);

            //BR_FR37
            this.validatePreviousEndedVoyagesToBeIssued(
                voyageDomainService,
                fuelReportDomainService,
                inventoryOperationDomainService);

            //BR_FR38
            this.validatePreviousFuelReportsToBeFinalApproved(fuelReportDomainService);

            //var isTheFirstReport = fuelReportDomainService.IsFuelReportTheFirstOneInVesselActivePeriod(this);
            var isTheFirstReport = this.IsTheFirstReport;

            FuelReport previousFuelReport = this.getLastFuelReportOfYesterday(fuelReportDomainService);

            this.validatePreviousFuelReport(previousFuelReport, isTheFirstReport);

            this.validateEOVReportToBeAvailableForChangeOfVoyage(this.VoyageId.Value, previousFuelReport, isTheFirstReport);
            this.validateChangeOfVoyageAgainstPriorEOVReport(this.VoyageId.Value, previousFuelReport, isTheFirstReport);
            this.validateEOVReportWithTheSameVoyageAsPreviousReport(this.VoyageId.Value, this.FuelReportType, previousFuelReport, isTheFirstReport);
            this.validateToBeUniqueEOVForCurrentVoyage(this.VoyageId.Value, this.FuelReportType, voyageDomainService);

            //Charter activationCharterContract = null;
            //if (isTheFirstReport)
            //{
            //    activationCharterContract = fuelReportDomainService.GetVesselActivationCharterContract(this);
            //}
            Charter activationCharterContract = this.ActivationCharterContract;

            foreach (var detailItem in this.FuelReportDetails)
            {
                FuelReportDetail previousFuelReportDetailForRelevantGood = this.getGoodRelevantFuelReportDetailOfYesterday(
                        detailItem.GoodId,
                        isTheFirstReport,
                        previousFuelReport);

                CharterItem relevantCharterItemAtFuelReprotTime = null;

                //TODO: ROB Comparison with the Activation Charter contract. 
                if (isTheFirstReport && activationCharterContract != null && this.EventDate >= activationCharterContract.ActionDate)
                {
                    relevantCharterItemAtFuelReprotTime = activationCharterContract.CharterItems.SingleOrDefault(ci => ci.GoodId == detailItem.GoodId);
                }

                //All Edit Rules must be checked for details.
                detailItem.ValidateCurrentValues(
                    previousFuelReportDetailForRelevantGood,
                    isTheFirstReport,
                    relevantCharterItemAtFuelReprotTime,
                    currencyDomainService);

                detailItem.ValidateTransferReferences(orderDomainService, fuelReportDomainService);
                detailItem.ValidateReceiveReferences(orderDomainService);
                //detailItem.ValidateCorrectionReferences(isTheFirstReport, voyageDomainService, fuelReportDomainService);
            }
        }

        private void validateToBeUniqueEOVForCurrentVoyage(long currentVoyageId, FuelReportTypes currentFuelReportType, IVoyageDomainService voyageDomainService)
        {
            if (currentFuelReportType == FuelReportTypes.EndOfVoyage)
            {
                var eovSubmittedFuelReport = voyageDomainService.GetVoyageValidEndOfVoyageFuelReport(currentVoyageId);

                if (eovSubmittedFuelReport != null)
                    throw new BusinessRuleException("", "There is already one submitted EOV Fuel Report in system for current Voyage.");
            }
        }

        //===================================================================================

        private void validateFinancialSubmittingState(IVoyageDomainService voyageDomainService, IFuelReportDomainService fuelReportDomainService, ICurrencyDomainService currencyDomainService)
        {
            this.CheckToBeSubmittedByCommercial();

            this.checkIsThereAnyFuelReportWithCorrectionButNotRevisedByFinance(fuelReportDomainService);

            var isTheFirstReport = fuelReportDomainService.IsFuelReportTheFirstOneInVesselActivePeriod(this);

            foreach (var detailItem in this.FuelReportDetails)
            {
                detailItem.ValidateToBeRevisedByFinance();
                detailItem.ValidateCorrectionReferences(isTheFirstReport, voyageDomainService, fuelReportDomainService, currencyDomainService);
            }
        }

        //===================================================================================

        private void validateEOVReportToBeAvailableForChangeOfVoyage(long currentVoyageId, FuelReport previousFuelReport, bool isTheFirstReport)
        {
            if (!isTheFirstReport &&
                currentVoyageId != previousFuelReport.VoyageId.GetValueOrDefault(0) &&
                previousFuelReport.FuelReportType != FuelReportTypes.EndOfVoyage)
                throw new BusinessRuleException("", "Change of voyage detected but the previous Fuel Report is not of type End of Voyage.");
        }

        //===================================================================================

        private void validateChangeOfVoyageAgainstPriorEOVReport(long currentVoyageId, FuelReport previousFuelReport, bool isTheFirstReport)
        {
            if (!isTheFirstReport &&
                previousFuelReport.FuelReportType == FuelReportTypes.EndOfVoyage &&
                currentVoyageId == previousFuelReport.VoyageId.GetValueOrDefault(0))
                throw new BusinessRuleException("", "Previous Fuel Report is of type End of Voyage but the voyage of current report is not changed.");
        }

        //===================================================================================

        private void validateEOVReportWithTheSameVoyageAsPreviousReport(long currentVoyageId, FuelReportTypes currentFuelReportType, FuelReport previousFuelReport, bool isTheFirstReport)
        {
            if (!isTheFirstReport &&
                currentFuelReportType == FuelReportTypes.EndOfVoyage &&
                currentVoyageId != previousFuelReport.VoyageId.GetValueOrDefault(0))
                throw new BusinessRuleException("", "Current Fuel Report is of type End of Voyage but with Voyage other than previous report.");
        }

        //===================================================================================

        /// <summary>
        /// BR_FR37
        /// </summary>
        private void validatePreviousEndedVoyagesToBeIssued(
            IVoyageDomainService voyageDomainService,
            IFuelReportDomainService fuelReportDomainService,
            IInventoryOperationDomainService inventoryOperationDomainService)
        {
            var notIssuedEOVFuelReportsOfPreviousVoyages = fuelReportDomainService.GetNotIssuedEOVFuelReportsOfPreviousVoyages(this);

            if (notIssuedEOVFuelReportsOfPreviousVoyages.Count != 0)
                throw new BusinessRuleException("BR_FR37", "There are some ended voyages before current Fuel Report with no issue for End Of Voyage Fuel Report.\n\nFuelReport Ids:\n" + string.Join(" , ", notIssuedEOVFuelReportsOfPreviousVoyages.Select(fr => fr.Id)));
        }

        //===================================================================================

        /// <summary>
        /// BR_FR38
        /// </summary>
        private void validatePreviousFuelReportsToBeFinalApproved(IFuelReportDomainService fuelReportDomainService)
        {
            var previousNotFinalApprovedReports = fuelReportDomainService.GetPreviousNotFinalApprovedReports(this);

            if (previousNotFinalApprovedReports.Count() != 0)
                throw new BusinessRuleException("BR_FR38", "There are some not final approved Fuel Reports before current FuelReport.\n\nFuelReport Ids:\n" + string.Join(" , ", previousNotFinalApprovedReports.Select(fr => fr.Id)));
        }

        //===================================================================================

        #endregion

        //===================================================================================

        public void IsWaitingToBeClosed()
        {
            if (!this.isFuelReportWaitingToBeClosed.IsSatisfiedBy(this))
                throw new BusinessRuleException("FR_Close", this.State + " : Fuel Report could not accept Inventory Results.");
        }

        //===================================================================================

        public void Close(FuelReportState entityNewState, long approverId)
        {
            this.CheckToBeOperational();

            this.IsWaitingToBeClosed();

            this.setEntityState(entityNewState);
        }

        //===================================================================================

        public void Submit(FuelReportState entityNewState, IVoyageDomainService voyageDomainService, IFuelReportDomainService fuelReportDomainService, IInventoryOperationDomainService inventoryOperationDomainService, IGoodDomainService goodDomainService, IOrderDomainService orderDomainService, ICurrencyDomainService currencyDomainService, IBalanceDomainService balanceDomainService, IInventoryManagementDomainService inventoryManagementDomainService, IInventoryOperationNotifier inventoryOperationNotifier, long approverId)
        {
            this.checkNextCharterContract();

            this.UpdateFirstPositionStatus(fuelReportDomainService);

            this.CheckToBeOperational();

            this.validateSubmittingState(
                    voyageDomainService,
                    fuelReportDomainService,
                    inventoryOperationDomainService,
                    goodDomainService,
                    orderDomainService,
                    currencyDomainService);

            this.sendDataToBalancingDomainService(balanceDomainService);

            var consumptionInventoryResult = inventoryOperationNotifier.NotifySubmittingFuelReportConsumption(this, approverId);

            this.ConsumptionInventoryOperations.MergeInventoryOperationResult(consumptionInventoryResult);
            foreach (var fuelReportDetail in this.FuelReportDetails)
            {
                //TODO:
                var inventoryResult = inventoryOperationNotifier.NotifySubmittingFuelReportDetail(fuelReportDetail, fuelReportDomainService, approverId);

                if (inventoryResult != null)
                    fuelReportDetail.InventoryOperations.MergeInventoryOperationResult(inventoryResult);
            }

            if (this.FuelReportType == FuelReportTypes.EndOfVoyage)
                this.Voyage.Lock();

            this.setEntityState(entityNewState);
        }

        private void checkNextCharterContract()
        {
            var charteringDomainService = ServiceLocator.Current.GetInstance<ICharteringDomainService>();
            var nextcharterContract = charteringDomainService.GetNextCharterContractForCompany(this.VesselInCompany.Code, this.VesselInCompany.CompanyId, this.EventDate, null, false);

            if (nextcharterContract != null)
                throw new BusinessRuleException("", string.Format("There are submitted chartering contracts after current Fuel Reprot ' {0} '. \nAll Chartering Contracts after this FuelReport should be reverted.", this.Id));
        }

        //===================================================================================

        private void sendDataToBalancingDomainService(IBalanceDomainService balanceDomainService)
        {
            foreach (var fuelReportDetail in this.FuelReportDetails)
            {
                balanceDomainService.RemoveOperatedQuantity(fuelReportDetail.Id);

                if (fuelReportDetail.Receive.HasValue && fuelReportDetail.ReceiveType.HasValue && fuelReportDetail.ReceiveReference != null && fuelReportDetail.ReceiveReference.ReferenceId.HasValue && fuelReportDetail.ReceiveType.Value != ReceiveTypes.Trust)
                    balanceDomainService.SetReceivedData(fuelReportDetail.ReceiveReference.ReferenceId.Value, fuelReportDetail.Id, fuelReportDetail.GoodId, fuelReportDetail.MeasuringUnitId, (decimal)fuelReportDetail.Receive.Value);

                //if (fuelReportDetail.Transfer.HasValue && fuelReportDetail.TransferType.HasValue && fuelReportDetail.TransferReference != null && fuelReportDetail.TransferReference.ReferenceId.HasValue && fuelReportDetail.TransferType.Value != TransferTypes.Rejected)
                //    balanceDomainService.SetTransferData(fuelReportDetail.TransferReference.ReferenceId.Value, fuelReportDetail.Id, fuelReportDetail.GoodId, fuelReportDetail.MeasuringUnitId, (decimal)fuelReportDetail.Transfer.Value);
            }
        }

        //===================================================================================

        public void Invalidate(FuelReportState entityNewState, long approverId)
        {
            this.setEntityState(entityNewState);
        }

        //===================================================================================

        public void CheckToBeOperational()
        {
            if (!this.IsActive())
                throw new InvalidOperation("CheckToBeOperational", "The Fuel Report is not Operational.");
        }

        //===================================================================================

        public void CheckToBeNotCancelled()
        {
            if (!this.IsNotCancelled())
                throw new InvalidOperation("CheckToNotBeCancelled", "The Fuel Report is cancelled.");
        }

        //===================================================================================

        public bool IsActive()
        {
            return this.isFuelReportOperational.IsSatisfiedBy(this);
        }

        //===================================================================================

        public bool IsNotCancelled()
        {
            return this.isFuelReportNotCancelled.IsSatisfiedBy(this);
        }

        //===================================================================================

        private void setEntityState(FuelReportState entityNewState)
        {
            this.EntityState = entityNewState;

            this.State = entityNewState.State;
        }

        //===================================================================================

        public void ResetStateToInitial(IFuelReportStateFactory fuelReportStateFactory, WorkflowStep initWorkflowStep, long userId)
        {
            if (IsNotCancelled())
                this.CheckToBeOperational();

            checkToHaveNoQuantityInInventory();

            var fuelReportWorkflow = new FuelReportWorkflowLog(this.Id, WorkflowEntities.FuelReport, DateTime.Now, WorkflowActions.Init,
                userId, "", initWorkflowStep.Id, true);

            this.ApproveWorkFlows.ForEach(frw => frw.Active = false);

            this.ApproveWorkFlows.Add(fuelReportWorkflow);

            this.setEntityState(fuelReportStateFactory.CreateOpenState());
        }

        //===================================================================================

        public void ResetStateToInvalid(IFuelReportStateFactory fuelReportStateFactory, IWorkflowStepRepository workflowRepository, long userId)
        {
            if (IsNotCancelled())
                this.CheckToBeOperational();

            checkToHaveNoQuantityInInventory();

            var cancelledWorkflowStep = workflowRepository.Single(c => c.Workflow.WorkflowEntity == WorkflowEntities.FuelReport &&
                 c.Workflow.CompanyId == this.VesselInCompany.CompanyId && c.CurrentWorkflowStage == WorkflowStages.Canceled &&
                c.Workflow.Name == Workflow.DEFAULT_NAME);

            if (cancelledWorkflowStep == null)
                throw new ObjectNotFound("FuelReportInvalidStep");

            var fuelReportWorkflow = new FuelReportWorkflowLog(this.Id, WorkflowEntities.FuelReport, DateTime.Now, WorkflowActions.Cancel,
                userId, "", cancelledWorkflowStep.Id, true);

            this.ApproveWorkFlows.ForEach(frw => frw.Active = false);

            this.ApproveWorkFlows.Add(fuelReportWorkflow);

            this.setEntityState(fuelReportStateFactory.CreateInvalidState());
        }

        //===================================================================================

        private void checkToHaveNoQuantityInInventory()
        {
            if (this.ConsumptionInventoryOperations.Count > 0 || this.FuelReportDetails.Any(frd => frd.InventoryOperations.Count > 0))
            {
                var inventoryDomainService = ServiceLocator.Current.GetInstance<IInventoryManagementDomainService>();

                var inventoryOperationsToCheck = new List<InventoryOperation>(this.ConsumptionInventoryOperations);
                inventoryOperationsToCheck.AddRange(this.FuelReportDetails.SelectMany(frd => frd.InventoryOperations));

                if (inventoryDomainService.IsThereAnyInventoryOperationWithNotEmptyQuantity(inventoryOperationsToCheck))
                    throw new BusinessRuleException("",
                        string.Format("The FuelReport ' {0} ' has Inventory Operations that should be reverted completely.", this.Id));
            }
        }

        //===================================================================================

        private void checkToHaveNoSubmittedInvoice()
        {
            var balanceDomainService = ServiceLocator.Current.GetInstance<IBalanceDomainService>();

            var fuelReportDetailGoodsWithSubmittedInvoices = new List<string>();

            foreach (var fuelReportDetail in FuelReportDetails)
            {

                if (balanceDomainService.DoesFuelReportDetailHaveInvoices(fuelReportDetail.Id))
                    fuelReportDetailGoodsWithSubmittedInvoices.Add(fuelReportDetail.Good.Code);
            }

            if (fuelReportDetailGoodsWithSubmittedInvoices.Count > 0)
                throw new BusinessRuleException("", string.Format("The invoices of following goods in FuelReport ' {0} ' should be reverted by finance Dpt.:\n{1}", this.Id, string.Join(" , ", fuelReportDetailGoodsWithSubmittedInvoices)));
        }

        //===================================================================================

        private void checkToHaveNoInventoryOperation()
        {
            if (this.ConsumptionInventoryOperations.Count > 0 ||
                this.FuelReportDetails.Any(frd => frd.InventoryOperations.Count > 0))
                throw new BusinessRuleException("WITH_INVENTORY_OPERATION", "The selected FuelReport has some inventory operations.");
        }

        //===================================================================================

        internal void Configure(IFuelReportStateFactory fuelReportStateFactory)
        {
            this.EntityState = fuelReportStateFactory.CreatState(this.State);

            //foreach (var fuelReportDetail in FuelReportDetails)
            //{
            //    fuelReportDetail.ROBQuantity = new Quantity((decimal)fuelReportDetail.ROB, new UnitOfMeasure(fuelReportDetail.ROBUOM));

            //    if (fuelReportDetail.CorrectionPrice.HasValue && !string.IsNullOrWhiteSpace(fuelReportDetail.CorrectionPriceCurrencyISOCode))
            //        fuelReportDetail.CorrectionPriceMoney = new Money(fuelReportDetail.CorrectionPrice.Value, new CurrencyAndMeasurement.Domain.Contracts.Currency(fuelReportDetail.CorrectionPriceCurrencyISOCode));
            //}
        }

        //===================================================================================

        public void UpdateEventDate(DateTime eventDateTime)
        {
            this.CheckToBeOperational();

            this.validateToBeSubmitRejected();

            this.validateToBeEndOfVoyageFuelReport();

            this.EventDate = eventDateTime;
        }

        //===================================================================================

        public void UpdateFirstPositionStatus(IFuelReportDomainService fuelReportDomainService)
        {
            this.ActivationCharterContract = fuelReportDomainService.GetVesselActivationCharterContract(this);
            this.IsTheFirstReport = fuelReportDomainService.IsFuelReportTheFirstOneInVesselActivePeriod(this, this.ActivationCharterContract);

            this.ActivationCharterContractId = this.ActivationCharterContract == null ? null : (long?)this.ActivationCharterContract.Id;

            fuelReportDomainService.SetNextFuelReportsFirstPositionStatus(this);
        }

        //===================================================================================

        private void validateToBeEndOfVoyageFuelReport()
        {
            if (this.FuelReportType != FuelReportTypes.EndOfVoyage)
                throw new BusinessRuleException("", "Fuel Report is not End of Voyage Fuel Report.");
        }

        //===================================================================================

        private void validateToBeSubmitRejected()
        {
            if (!this.isFuelReportSubmitRejected.IsSatisfiedBy(this))
                throw new BusinessRuleException("", "The Fuel Report is not submit rejected.");
        }

        //===================================================================================

        private void validateToBeInSubmittedState()
        {
            if (!this.isFuelReportSubmitted.IsSatisfiedBy(this))
                throw new BusinessRuleException("", string.Format("The Fuel Report ' {0} ' is not submitted.", this.Id));
        }

        //===================================================================================

        public void RejectSubmitted(FuelReportState entityNewState, long approverId)
        {
            this.checkToHaveNoSubmittedInvoice();

            this.validateToBeInSubmittedState();

            this.checkNextCharterContract();

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

            this.validateToNotHaveAnySubmittedFuelReportAfter(fuelReportDomainService);

            var balanceDomainService = ServiceLocator.Current.GetInstance<IBalanceDomainService>();

            foreach (var fuelReportDetail in this.FuelReportDetails)
            {
                balanceDomainService.RemoveOperatedQuantity(fuelReportDetail.Id);
            }

            if (this.FuelReportType == FuelReportTypes.EndOfVoyage)
                this.Voyage.Unlock();

            this.setEntityState(entityNewState);

            this.RevertFuelReportInventoryOperations(approverId);
        }

        //===================================================================================

        private void validateToNotHaveAnySubmittedFuelReportAfter(IFuelReportDomainService fuelReportDomainService)
        {
            if (!fuelReportDomainService.IsThereAnySubmittedFuelReportAfter(this))
                throw new BusinessRuleException("", "There are some submitted FuelReports after.");
        }

        private void validateToNotHaveAnySubmittedFuelReportAtTheTimeOfCurrentReport(IFuelReportDomainService fuelReportDomainService)
        {
            if (!fuelReportDomainService.IsThereAnySubmittedFuelReportAtTheTimeOfCurrentReport(this))
                throw new BusinessRuleException("", "There are some submitted FuelReports after.");
        }

        //===================================================================================

        public void UpdateConsumption(IFuelReportDomainService fuelReportDomainService)
        {
            //validatePreviousFuelReportsToBeFinalApproved(fuelReportDomainService);

            var previousFuelReports = fuelReportDomainService.GetFuelReportsFromYesterday(this);

            var previousFuelReport = previousFuelReports.LastOrDefault();

            if (previousFuelReport != null)
            {
                foreach (var fuelReportDetail in this.FuelReportDetails)
                {
                    var relevantFeulReportDetailOfPreviousDay = previousFuelReport.FuelReportDetails.FirstOrDefault(frd => frd.GoodId == fuelReportDetail.GoodId);

                    if (relevantFeulReportDetailOfPreviousDay != null)
                    {
                        fuelReportDetail.UpdateConsumption(relevantFeulReportDetailOfPreviousDay);
                    }
                }
            }
        }

        //===================================================================================

        public void ChangeCompany(Company destinationCompany, IVesselInCompanyDomainService vesselInCompanyDomainService, IGoodDomainService goodDomainService)
        {
            this.validateToBeOpenOrSubmitRejected();

            var destinationVesselInCompany = vesselInCompanyDomainService.GetVesselInCompany(destinationCompany.Id, this.VesselInCompany.Vessel.Code);

            if (destinationVesselInCompany == null)
                throw new ObjectNotFound("No Vessel found with Code " + this.VesselInCompany.Vessel.Code + " in Company " + destinationCompany.Name);

            this.VesselInCompanyId = destinationVesselInCompany.Id;

            this.FuelReportDetails.ToList().ForEach(frd => frd.ChangeCompany(destinationCompany.Id, goodDomainService));

            this.VoyageId = null;
        }

        //===================================================================================

        public void ChangeCompany(Company destinationCompany, VesselInCompany destinationVesselInCompany, IGoodDomainService goodDomainService)
        {
            this.validateToBeOpenOrSubmitRejected();

            this.VesselInCompanyId = destinationVesselInCompany.Id;
            this.VesselInCompany = destinationVesselInCompany;

            this.FuelReportDetails.ToList().ForEach(frd => frd.ChangeCompany(destinationCompany.Id, goodDomainService));

            this.VoyageId = null;
            this.Voyage = null;
        }

        //===================================================================================

        public bool IsSubmitted()
        {
            return this.isFuelReportSubmitted.IsSatisfiedBy(this);
        }

        //===================================================================================

        public void UpdateCode(string vesselReportReference, bool isVesselInActiveState)
        {
            if (isVesselInActiveState)
                this.CheckToBeOperational();

            this.Code = vesselReportReference;
        }

        //===================================================================================

        public bool IsEndOfYearReport()
        {
            var pCal = new PersianCalendar();

            return this.FuelReportType == FuelReportTypes.NoonReport &&
                        (
                            (
                                pCal.GetMonth(this.EventDate) == 1 &&
                                pCal.GetDayOfMonth(this.EventDate) == 1 &&
                                this.EventDate.Year > 2016
                            ) ||
                            (
                                this.EventDate.Year <= 2016 &&
                                this.EventDate.Month == 3 &&
                                this.EventDate.Day == 21
                            )
                        ) &&
                    this.EventDate.TimeOfDay == new TimeSpan(12, 0, 0);
        }

        //===================================================================================

        public bool IsSubmittedByCommercial()
        {
            return this.isFuelReportSubmittedByCommercial.IsSatisfiedBy(this);
        }

        public bool IsSubmittedByFinance()
        {
            return this.isFuelReportSubmittedByFinance.IsSatisfiedBy(this);
        }

        //===================================================================================

        public void CheckToBeSubmittedByCommercial()
        {
            if (!this.IsSubmittedByCommercial())
                throw new BusinessRuleException("", "The Fuel Report is not submitted by Commercial operator.");
        }

        public void CheckToBeSubmittedByFinance()
        {
            if (!this.IsSubmittedByFinance())
                throw new BusinessRuleException("", "The Fuel Report is not submitted by Finance operator.");
        }

        //===================================================================================

        public void SubmitByFinancial(long approverId)
        {
            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
            var inventoryOperationNotifier = ServiceLocator.Current.GetInstance<IInventoryOperationNotifier>();
            var voyageDomainService = ServiceLocator.Current.GetInstance<IVoyageDomainService>();
            var currencyDomainService = ServiceLocator.Current.GetInstance<ICurrencyDomainService>();
            this.validateVoyageId(this.Voyage.Id, voyageDomainService);
            this.validateFinancialSubmittingState(voyageDomainService, fuelReportDomainService, currencyDomainService);

            foreach (var fuelReportDetail in this.FuelReportDetails)
            {
                inventoryOperationNotifier.NotifySubmittingFuelReportDetailByFinance(fuelReportDetail, fuelReportDomainService, approverId);
            }
        }

        //===================================================================================

        private void checkIsThereAnyFuelReportWithCorrectionButNotRevisedByFinance(IFuelReportDomainService fuelReportDomainService)
        {
            var fuelReportsWithCorrectionButNotRevised =
                fuelReportDomainService.GetFuelReportsWithCorrectionButNotRevisedByFinance(VesselInCompany.CompanyId,
                    VesselInCompany.Code, EventDate.AddMilliseconds(-1));

            if (fuelReportsWithCorrectionButNotRevised.Count > 0)
                throw new BusinessRuleException("",
                    "There are some Fuel Reports with Correction, which are not manipulated by relevant financial operator.\nThe Ids are:\n\n" +
                    string.Join("\n", fuelReportsWithCorrectionButNotRevised));

            //var fuelReportRepository = ServiceLocator.Current.GetInstance<IFuelReportRepository>();

            //var fuelReportsWithCorrectionButNotRevised = fuelReportRepository.Find(
            //        fr => fr.EventDate >= new DateTime(2015, 4, 1, 0, 0, 0) &&
            //            fr.EventDate < this.EventDate &&
            //            fr.Id != this.Id &&
            //            (fr.VesselInCompanyId == this.VesselInCompanyId) &&
            //            (fr.FuelReportDetails.Any(
            //                frd => frd.Correction.HasValue && //Check the details for corrections without revision or not submitted yet by finance.
            //                    (!frd.IsCorrectionPricingTypeRevised ||
            //                    !(fr.State == States.Submitted && fr.ApproveWorkFlows.Any(w => w.Active && w.CurrentWorkflowStep.CurrentWorkflowStage == WorkflowStages.FinancialSubmitted)))
            //                )
            //            )
            //    ).ToList().OrderBy(fr => fr.EventDate).Select(fr => fr.Id).ToList();

            //if (fuelReportsWithCorrectionButNotRevised.Count > 0)
            //    throw new BusinessRuleException("",
            //        "There are some Fuel Reports with Correction, which are not manipulated by relevant financial operator.\n\nFuelReport Ids:\n" +
            //        string.Join(" , ", fuelReportsWithCorrectionButNotRevised)
            //);
        }

        //===================================================================================

        public void Delete(IFuelReportDomainService fuelReportDomainService, IFuelReportStateFactory fuelReportStateFactory, IWorkflowStepRepository workflowRepository, long userId)
        {
            this.validateToBeOpenOrSubmitRejected();

            try
            {
                checkToHaveNoInventoryOperation();

                fuelReportDomainService.Delete(this);
            }
            catch (BusinessRuleException ex)
            {
                if (ex.BusinessRuleCode == "WITH_INVENTORY_OPERATION")
                {
                    this.ResetStateToInvalid(fuelReportStateFactory, workflowRepository, userId);
                }
                else
                    throw;
            }
            catch
            {
                throw;
            }
        }

        public void RevertFuelReportInventoryOperations(long userId)
        {
            validateToBeSubmitRejected();
            var inventoryOperationNotifier = ServiceLocator.Current.GetInstance<IInventoryOperationNotifier>();

            var revertConsumptionResult = inventoryOperationNotifier.RevertFuelReportConsumptionInventoryOperations(this, (int)userId);

            this.ConsumptionInventoryOperations.MergeInventoryOperationResult(revertConsumptionResult);

            foreach (var fuelReportDetail in FuelReportDetails)
            {
                var fuelReportDetailRevertResult = new InventoryOperationResult();

                fuelReportDetailRevertResult.Merge(inventoryOperationNotifier.RevertFuelRpeortDetailReceiveInventoryOperations(fuelReportDetail, (int)userId));
                fuelReportDetailRevertResult.Merge(inventoryOperationNotifier.RevertFuelRpeortDetailTransferInventoryOperations(fuelReportDetail, (int)userId));
                fuelReportDetailRevertResult.Merge(inventoryOperationNotifier.RevertFuelRpeortDetailCorrectionInventoryOperations(fuelReportDetail, (int)userId));

                fuelReportDetail.InventoryOperations.MergeInventoryOperationResult(fuelReportDetailRevertResult);
            }
        }

        public void RejectFinancialSubmitted(long approverId)
        {
            this.CheckToBeSubmittedByFinance();

            var inventoryOperationNotifier = ServiceLocator.Current.GetInstance<IInventoryOperationNotifier>();

            foreach (var fuelReportDetailWithCorrectionQuantity in FuelReportDetails.Where(frd => frd.Correction.HasValue && frd.CorrectionType.HasValue))
            {
                inventoryOperationNotifier.RevertFuelReportDetailCorrectionPricing(fuelReportDetailWithCorrectionQuantity, approverId);

                fuelReportDetailWithCorrectionQuantity.ResetCorrectionPriceRevisionState();
            }
        }
    }
}