using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MITD.CurrencyAndMeasurement.Domain.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class FuelReportDetail
    {
        #region Properties

        public long Id { get; private set; }

        public long FuelReportId { get; private set; }

        public long GoodId { get; private set; }

        public virtual Good Good { get; set; }

        public long TankId { get; private set; }

        public virtual Tank Tank { get; set; }

        public decimal Consumption { get; private set; }

        public decimal ROB { get; private set; }

        public string ROBUOM { get; set; }

        public Quantity ROBQuantity { get; set; }

        public decimal? Receive { get; private set; }

        public ReceiveTypes? ReceiveType { get; set; }

        public Reference ReceiveReference { get; set; }


        public decimal? Transfer { get; private set; }

        public TransferTypes? TransferType { get; set; }

        public Reference TransferReference { get; set; }

        public decimal? Correction { get; private set; }

        public CorrectionTypes? CorrectionType { get; private set; }

        public CorrectionPricingTypes? CorrectionPricingType { get; private set; }

        public Reference CorrectionReference { get; set; }

        public long? CorrectionPriceCurrencyId { get; private set; }

        public decimal? CorrectionPrice { get; private set; }

        public virtual Currency CorrectionPriceCurrency { get; set; }

        public long MeasuringUnitId { get; private set; }

        public string CorrectionPriceCurrencyISOCode { get; set; }
        public Money CorrectionPriceMoney { get; set; }

        public virtual GoodUnit MeasuringUnit { get; set; }


        public virtual FuelReport FuelReport { get; set; }

        public virtual List<InventoryOperation> InventoryOperations
        {
            get;
            set;
            //get
            //{
            //    var result = new List<InventoryOperation>();
            //    result.AddRange(this.ReceiveInventoryOperations);
            //    result.AddRange(this.TransferInventoryOperations);
            //    result.AddRange(this.CorrectionInventoryOperations);

            //    return result;
            //}
        }

        //public virtual List<InventoryOperation> ReceiveInventoryOperations { get; set; }
        //public virtual List<InventoryOperation> TransferInventoryOperations { get; set; }
        //public virtual List<InventoryOperation> CorrectionInventoryOperations { get; set; }


        public byte[] TimeStamp { get; set; }

        public long? TrustIssueInventoryTransactionItemId { get; set; }

        #endregion

        public FuelReportDetail()
        {
            this.ReceiveReference = Reference.Empty;
            this.TransferReference = Reference.Empty;
            this.CorrectionReference = Reference.Empty;

            //ReceiveInventoryOperations = new List<InventoryOperation>();
            //TransferInventoryOperations = new List<InventoryOperation>();
            //CorrectionInventoryOperations = new List<InventoryOperation>();
        }

        internal FuelReportDetail(
            long id,
            long fuelReportId,
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
            : this()
        {
            //Id = id;
            this.FuelReportId = fuelReportId;

            //No validation performed due to unavailability of such validation.

            Reference transferReferenceToBypass = null;
            Reference correctionReferenceToBypass = null;
            Reference receiveReferenceToBypass = null;
            refineValues(ref rob, ref consumption, ref receive, ref receiveType, ref transfer, ref transferType, ref correction, ref correctionType, ref correctionPrice, ref correctionPriceCurrencyId, ref transferReferenceToBypass, ref receiveReferenceToBypass, ref correctionReferenceToBypass);

            this.Consumption = consumption;
            this.Correction = correction;
            this.CorrectionPrice = correctionPrice;
            this.CorrectionPriceCurrencyISOCode = correctionPriceCurrencyISOCode;
            this.CorrectionType = correctionType;
            this.CorrectionPricingType = correctionPricingType;
            this.Receive = receive;
            this.ROB = rob;
            this.ROBUOM = robUOM;
            this.Transfer = transfer;
            this.ReceiveType = receiveType;
            this.TransferType = transferType;
            this.CorrectionPriceCurrencyId = correctionPriceCurrencyId;
            this.GoodId = fuelTypeId;
            this.MeasuringUnitId = measuringUnitId;
            this.TankId = tankId;
        }

        internal void Update(decimal rob, decimal consumption, decimal? receive, ReceiveTypes? receiveType, decimal? transfer, TransferTypes? transferType, decimal? correction, CorrectionTypes? correctionType, CorrectionPricingTypes? correctionPricingType, decimal? correctionPrice, long? correctionPriceCurrencyId, Reference transferReference, Reference receiveReference, Reference correctionReference, FuelReportDetail relevantFuelReportDetailOfTheDayBefore, bool isDetailOfFirstFuelReport, CharterItem relevantCharterItemAtFuelReportTime, long? trustIssueInventoryTransactionItemId, ICurrencyDomainService currencyDomainService)
        {
            this.validateValues(rob, consumption, receive, receiveType, transfer, transferType, correction, correctionType,
                correctionPricingType,
                correctionPrice, correctionPriceCurrencyId,
                transferReference,
                receiveReference,
                correctionReference,
                relevantFuelReportDetailOfTheDayBefore, isDetailOfFirstFuelReport, relevantCharterItemAtFuelReportTime, currencyDomainService);

            refineValues(ref rob, ref consumption, ref receive, ref receiveType, ref transfer, ref transferType, ref correction, ref correctionType, ref correctionPrice, ref correctionPriceCurrencyId, ref transferReference, ref receiveReference, ref correctionReference);

            if (this.FuelReport.IsActive())
            {
                this.ROB = rob;
                this.Consumption = consumption;
                this.Receive = receive;
                this.ReceiveType = receiveType;
                this.Transfer = transfer;
                this.TransferType = transferType;
                this.Correction = correction;
                this.CorrectionType = correctionType;

                this.TransferReference = transferReference;
                this.ReceiveReference = receiveReference;

                this.TrustIssueInventoryTransactionItemId = trustIssueInventoryTransactionItemId;
            }

            if (this.FuelReport.IsSubmittedByCommercial())
            {
                this.IsCorrectionPricingTypeRevised = true;
                this.CorrectionPricingType = correctionPricingType;
                this.CorrectionPrice = correctionPrice;
                this.CorrectionPriceCurrencyId = correctionPriceCurrencyId;
                this.CorrectionReference = correctionReference;
            }
        }

        internal void UpdateFromEventReport(
            decimal rob,
            decimal consumption,
            decimal? receive,
            decimal? transfer,
            decimal? correction,
            CorrectionTypes? correctionType)
        {
            refineValues(ref rob, ref consumption, ref receive, ref transfer, ref correction, ref correctionType);

            this.ROB = rob;
            this.Consumption = consumption;
            this.Receive = receive;
            this.Transfer = transfer;
            this.Correction = correction;
            this.CorrectionType = correctionType;

            //if (this.FuelReport.State == States.Open)
            //{
            //    this.ROB = rob;
            //    this.Consumption = consumption;
            //    this.Receive = receive;
            //    this.Transfer = transfer;
            //    this.Correction = correction;
            //    this.CorrectionType = correctionType;

            //    this.TrustIssueInventoryTransactionItemId = null;
            //}
            //else if (this.FuelReport.State == States.SubmitRejected)
            //{
            //    this.ROB = rob;
            //    this.Consumption = consumption;
            //}
        }

        private void validateValues(
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
            long? correctionPriceCurrencyId,
            Reference transferReference,
            Reference receiveReference,
            Reference correctionReference,
            FuelReportDetail relevantFuelReportDetailOfTheDayBefore,
            bool isDetailOfFirstFuelReport,
            CharterItem relevantCharterItemAtFuelReportTime,
            ICurrencyDomainService currencyDomainService)
        {
            refineValues(ref rob, ref consumption, ref receive, ref receiveType, ref transfer, ref transferType, ref correction, ref correctionType, ref correctionPrice, ref correctionPriceCurrencyId, ref transferReference, ref receiveReference, ref correctionReference);

            //BR_FR3
            this.validateConsumptionAndROBAvailability(consumption, rob);

            //BR_FR4
            this.validateNotNegativeConsumptionAndROB(consumption, rob);

            //BR_FR5
            this.validateReceiveType(receive, receiveType);

            //BR_FR6
            this.validateTransferType(transfer, transferType);

            //BR_FR7
            this.validateCorrectionType(correction, correctionType
                /*, correctionPricingType, correctionPriceCurrencyId,
                                                         correctionPrice, correctionReference, currencyDomainService*/);

            //BR_FR8
            this.validateNotNegativeReceiveTransferCorrection(receive, transfer, correction);

            //BR_FR9
            //This is already implemented via BR_FR5, BR_FR6, BR_FR7.

            //BR_FR34
            this.validateValuesForEndOfVoyageEndOfMonthEndOfYearFuelReport(
                rob,
                consumption,
                receive,
                receiveType,
                transfer,
                transferType,
                correction,
                correctionType,
                correctionPrice,
                correctionPriceCurrencyId);

            if (relevantFuelReportDetailOfTheDayBefore == null)
            {
                if (isDetailOfFirstFuelReport)
                {
                    if (relevantCharterItemAtFuelReportTime != null)
                    {
                        this.validateROB(rob, consumption,
                            receive,
                            correction,
                            correctionType,
                            transfer,
                            relevantCharterItemAtFuelReportTime.Rob);
                    }
                    else
                    {
                        //NOTE: No need to validate ROB for Details of the First Fuel Report in the system
                        //TODO: This should be checked against vessel activation ROB fethced from Inventory.
                    }
                }
                else
                {
                    //NOTE: The Detail of Yesterday Fuel Report is not found.
                    //throw new ObjectNotFound(string.Format("RelevantFuelReportDetailOfTheDayBefore for '{0}'", this.Good.Code));
                    this.validateROB(rob, consumption,
                            receive,
                            correction,
                            correctionType,
                            transfer,
                            0);
                }
            }
            else
            {
                //The Detail for Yesterday Report has been found.
                decimal? robOfTheDayBefore = relevantFuelReportDetailOfTheDayBefore.ROB;

                //BR_FR10
                this.validateROB(
                    rob,
                    consumption,
                    receive,
                    correction,
                    correctionType,
                    transfer,
                    robOfTheDayBefore);
            }
        }

        //===================================================================================

        private static void refineValues(ref decimal rob, ref decimal consumption, ref decimal? receive, ref ReceiveTypes? receiveType, ref decimal? transfer, ref TransferTypes? transferType, ref decimal? correction, ref CorrectionTypes? correctionType, ref decimal? correctionPrice, ref long? correctionPriceCurrencyId, ref Reference transferReference, ref Reference receiveReference, ref Reference correctionReference)
        {
            //Values refinement.
            //rob = !rob.HasValue || rob == 0 ? null : rob;
            //consumption = !consumption.HasValue || consumption == 0 ? null : consumption;
            rob = Math.Round(rob, 3);
            consumption = Math.Round(consumption, 3);

            receive = !receive.HasValue || receive == 0 ? null : (decimal?)Math.Round(receive.Value, 3);
            transfer = !transfer.HasValue || transfer == 0 ? null : (decimal?)Math.Round(transfer.Value, 3);
            correction = !correction.HasValue || correction == 0 ? null : (decimal?)Math.Round(correction.Value, 3);
            correctionPrice = !correctionPrice.HasValue || correctionPrice == 0 ? null : (decimal?)Math.Round(correctionPrice.Value, 2);
            correctionPriceCurrencyId = !correctionPriceCurrencyId.HasValue || correctionPriceCurrencyId == 0 ? null : correctionPriceCurrencyId;

            if (!receive.HasValue)
            {
                receiveType = null;
                receiveReference = Reference.Empty;
            }

            if (receive.HasValue && receiveType.HasValue && receiveType.Value == ReceiveTypes.Trust)
            {
                receiveReference = Reference.Empty;
            }

            if (!transfer.HasValue)
            {
                transferType = null;
                transferReference = Reference.Empty;
            }

            if (!correction.HasValue)
            {
                correctionType = null;
                correctionPrice = null;
                correctionPriceCurrencyId = null;
                correctionReference = Reference.Empty;
            }
        }

        //===================================================================================

        private static void refineValues(ref decimal rob, ref decimal consumption, ref decimal? receive, ref decimal? transfer, ref decimal? correction, ref CorrectionTypes? correctionType)
        {
            rob = Math.Round(rob, 3);
            consumption = Math.Round(consumption, 3);

            receive = !receive.HasValue || receive == 0 ? null : (decimal?)Math.Round(receive.Value, 3);
            transfer = !transfer.HasValue || transfer == 0 ? null : (decimal?)Math.Round(transfer.Value, 3);
            correction = !correction.HasValue || correction == 0 ? null : (decimal?)Math.Round(correction.Value, 3);

            if (!correction.HasValue)
            {
                correctionType = null;
            }
        }

        //===================================================================================

        internal void ValidateCurrentValues(
            FuelReportDetail relevantFuelReportDetailOfTheDayBefore,
            bool isDetailOfFirstFuelReport,
            CharterItem relevantCharterItemAtFuelReportTime,
            ICurrencyDomainService currencyDomainService)
        {
            this.validateValues(this.ROB, this.Consumption, this.Receive, this.ReceiveType, this.Transfer, this.TransferType,
                this.Correction, this.CorrectionType, this.CorrectionPricingType, this.CorrectionPrice, this.CorrectionPriceCurrencyId,
                this.TransferReference, this.ReceiveReference, this.CorrectionReference,
                relevantFuelReportDetailOfTheDayBefore, isDetailOfFirstFuelReport, relevantCharterItemAtFuelReportTime,
                currencyDomainService);
        }

        //===================================================================================

        #region BusinessRules

        //===================================================================================

        /// <summary>
        /// BR_FR3
        /// </summary>
        private void validateConsumptionAndROBAvailability(decimal? consumption, decimal? rob)
        {
            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)
            {
                if (!(consumption.HasValue && rob.HasValue))
                    throw new BusinessRuleException("BR_FR3", string.Format("Consumption or ROB for '{0}' is Empty.", this.Good.Code));
            }
        }

        //===================================================================================

        /// <summary>
        /// BR_FR4
        /// </summary>
        /// <param name="fuelReportDetail"></param>
        /// <returns></returns>
        private void validateNotNegativeConsumptionAndROB(decimal? consumption, decimal? rob)
        {
            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)
            {
                if (!(consumption.HasValue && consumption >= 0 &&
                      rob.HasValue && rob >= 0))
                    throw new BusinessRuleException("BR_FR4", string.Format("Consumption or ROB for '{0}'is Negative.", this.Good.Code));
            }
        }

        //===================================================================================

        /// <summary>
        /// BR_FR5
        /// </summary>
        private void validateReceiveType(decimal? receive, ReceiveTypes? receiveType)
        {
            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)

            if (this.FuelReport.FuelReportType != FuelReportTypes.BunkeringCompleted)
            {
                //fuelReportDetail.Receive > 0 : will be checked in BR_FR8
                if (receive.HasValue &&
                    !(
                         receiveType.HasValue &&
                         Enum.IsDefined(typeof(ReceiveTypes), receiveType.Value)/* &&
                         receiveType.Value != ReceiveTypes.NotDefined*/
                     )
                    )
                    throw new BusinessRuleException("BR_FR5", string.Format("The ReceiveType for '{0}' is not specified.", this.Good.Code));
            }
        }

        //===================================================================================

        /// <summary>
        ///     BR_FR6
        /// </summary>
        private void validateTransferType(decimal? transfer, TransferTypes? transferType)
        {
            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)
            if (this.FuelReport.FuelReportType != FuelReportTypes.DebunkeringCompleted)
            {
                //fuelReportDetail.Transfer > 0 : will be checked in BR_FR8
                if (transfer.HasValue &&
                    !(
                        transferType.HasValue &&
                        Enum.IsDefined(typeof(TransferTypes), transferType.Value)
                    )
                )
                    throw new BusinessRuleException("BR_FR6", string.Format("The TransferType for '{0}' is not specified.", this.Good.Code));
            }
        }

        //===================================================================================

        /// <summary>
        ///     BR_FR7
        /// </summary>
        private void validateCorrectionType(
            decimal? correction, CorrectionTypes? correctionType/*,
            CorrectionPricingTypes? correctionPricingType,
            long? currencyId, decimal? correctionPrice,
            Reference correctionReference,
            ICurrencyDomainService currencyDomainService*/)
        {
            #region Commented

            ////This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)
            //{
            //    if (correction.HasValue &&
            //        (
            //            !(correctionType.HasValue && Enum.IsDefined(typeof(CorrectionTypes), correctionType.Value))

            //            /*||
            //            !( //Check currencyId
            //             currencyId.HasValue && currencyDomainService.Get(currencyId.Value) != null
            //             ) ||
            //            !( //Check correctionPrice
            //             correctionPrice.HasValue && correctionPrice > 0
            //             )*/
            //        )
            //        )
            //        throw new BusinessRuleException("BR_FR7", string.Format("CorrectionType, Currency or Price are not specified for '{0}'.", this.Good.Code));
            //}


            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)

            #endregion

            if (this.FuelReport.FuelReportType != FuelReportTypes.ArrivalReport && this.FuelReport.FuelReportType != FuelReportTypes.DepartureReport && this.FuelReport.FuelReportType != FuelReportTypes.Berthing && this.FuelReport.FuelReportType != FuelReportTypes.Anchoring && this.FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage && this.FuelReport.FuelReportType != FuelReportTypes.BunkeringCompleted && this.FuelReport.FuelReportType != FuelReportTypes.DebunkeringCompleted)
            {
                if (correction.HasValue)
                {
                    if (!(correctionType.HasValue && Enum.IsDefined(typeof(CorrectionTypes), correctionType.Value)))
                        throw new BusinessRuleException("", string.Format("CorrectionType is not specified for '{0}'.", this.Good.Code));

                    //if (correctionType.Value == CorrectionTypes.Plus)
                    //{
                    //    validateIncrementalCorrectionTypeValues(correction, correctionType, correctionPricingType, currencyId, correctionPrice, correctionReference, currencyDomainService);

                    //}
                    //else if (correctionType.Value == CorrectionTypes.Minus)
                    //{
                    //    validateDecrementalCorrectionTypeValues(correction, correctionType, correctionPricingType, currencyId, correctionPrice, correctionReference, currencyDomainService);
                    //}
                }
            }
        }

        //===================================================================================

        private void validateIncrementalCorrectionTypeValues(
            decimal? correction, CorrectionTypes? correctionType,
            CorrectionPricingTypes? correctionPricingType,
           long? currencyId, decimal? correctionPrice,
            Reference correctionReference,
           ICurrencyDomainService currencyDomainService)
        {
            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)
            if (this.FuelReport.FuelReportType != FuelReportTypes.ArrivalReport && this.FuelReport.FuelReportType != FuelReportTypes.DepartureReport && this.FuelReport.FuelReportType != FuelReportTypes.Berthing && this.FuelReport.FuelReportType != FuelReportTypes.Anchoring && this.FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage && this.FuelReport.FuelReportType != FuelReportTypes.BunkeringCompleted && this.FuelReport.FuelReportType != FuelReportTypes.DebunkeringCompleted)
            {
                if (correction.HasValue && correctionType.HasValue && this.CorrectionPricingType.HasValue && correctionType.Value == CorrectionTypes.Plus)
                {
                    if (correctionPricingType == CorrectionPricingTypes.Default)
                        throw new BusinessRuleException("", string.Format("Correction Pricing Type is not specified for '{0}'.", this.Good.Code));

                    if ((correctionPricingType == CorrectionPricingTypes.ManualPricing) &&
                        (!currencyId.HasValue || currencyDomainService.Get(currencyId.Value) == null ||
                        !correctionPrice.HasValue || correctionPrice <= 0))
                        throw new BusinessRuleException("", string.Format("Correction Currency or Price are not specified for '{0}'.", this.Good.Code));

                    if ((correctionPricingType == CorrectionPricingTypes.LastPurchasePrice) && (
                        currencyId.HasValue || correctionPrice.HasValue || (correctionReference != null && !correctionReference.IsEmpty())))
                        throw new BusinessRuleException("", string.Format("Values for Last Purchase Pricing Correction are invalid for '{0}'.", this.Good.Code));

                    if ((correctionPricingType == CorrectionPricingTypes.LastIssuedConsumption) && (
                        correctionReference == null || correctionReference.IsEmpty() || correctionReference.ReferenceType.GetValueOrDefault() != ReferenceType.Voyage))
                        throw new BusinessRuleException("", string.Format("Values for Last Issued Consumption Pricing Correction are invalid for '{0}'.", this.Good.Code));
                }
            }
        }

        //===================================================================================

        private void validateDecrementalCorrectionTypeValues(
            decimal? correction, CorrectionTypes? correctionType,
            CorrectionPricingTypes? correctionPricingType,
           long? currencyId, decimal? correctionPrice,
            Reference correctionReference,
           ICurrencyDomainService currencyDomainService)
        {
            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)
            if (this.FuelReport.FuelReportType != FuelReportTypes.ArrivalReport && this.FuelReport.FuelReportType != FuelReportTypes.DepartureReport && this.FuelReport.FuelReportType != FuelReportTypes.Berthing && this.FuelReport.FuelReportType != FuelReportTypes.Anchoring && this.FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage && this.FuelReport.FuelReportType != FuelReportTypes.BunkeringCompleted && this.FuelReport.FuelReportType != FuelReportTypes.DebunkeringCompleted)
            {
                if (correction.HasValue && correctionType.HasValue && this.CorrectionPricingType.HasValue && correctionType.Value == CorrectionTypes.Plus)
                {
                    if (correctionPricingType != CorrectionPricingTypes.Default || correctionPricingType != CorrectionPricingTypes.LastIssuedConsumption)
                        throw new BusinessRuleException("", string.Format("Correction Pricing Type is invalid for '{0}'.", this.Good.Code));

                    if ((correctionPricingType == CorrectionPricingTypes.Default) && (
                        currencyId.HasValue || correctionPrice.HasValue || (correctionReference != null && !correctionReference.IsEmpty())))
                        throw new BusinessRuleException("", string.Format("Values of Correction Default Pricing are invalid for '{0}'.", this.Good.Code));

                    if ((correctionPricingType == CorrectionPricingTypes.LastIssuedConsumption) && (
                        correctionReference == null || correctionReference.IsEmpty() || correctionReference.ReferenceType.GetValueOrDefault() != ReferenceType.Voyage))
                        throw new BusinessRuleException("", string.Format("Values of Last Issued Consumption Pricing Correction are invalid for '{0}'.", this.Good.Code));
                }
            }
        }

        //===================================================================================

        /// <summary>
        ///     BR_FR8
        /// </summary>
        private void validateNotNegativeReceiveTransferCorrection(decimal? receive, decimal? transfer, decimal? correction)
        {
            //This business rule must be checked for non EndOfVoyage, EndOfMonth, EndOfYear FuelReports.
            //if (FuelReport.FuelReportType != FuelReportTypes.EndOfVoyage &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfMonth &&
            //    FuelReport.FuelReportType != FuelReportTypes.EndOfYear)
            {
                if ((receive.HasValue && receive < 0) ||
                    (transfer.HasValue && transfer < 0) ||
                    (correction.HasValue && correction < 0))
                    throw new BusinessRuleException("BR_FR8", string.Format("Receive or Transfer or Correction for '{0}' is Negative.", this.Good.Code));
            }
        }

        //===================================================================================

        /// <summary>
        /// BR_FR10
        /// </summary>
        private void validateROB(
            decimal? robOfCurrentDay,
            decimal? consumptionOfCurrentDay,
            decimal? receiveOfCurrentDay,
            decimal? correctionOfCurrentDay,
            CorrectionTypes? correctionTypeOfCurrentDay,
            decimal? transferOfCurrentDay,
            decimal? robOfTheDayBefore
            )
        {
            decimal? calculatedCorrection = (correctionOfCurrentDay * this.getCorrectionTypeFactor(correctionTypeOfCurrentDay));

            decimal? calculatedValue =
                robOfTheDayBefore +
                (receiveOfCurrentDay ?? 0) +
                (calculatedCorrection ?? 0)
                - (transferOfCurrentDay ?? 0)
                - consumptionOfCurrentDay;

            //TODO: If calculatedValue is null what should be done?
            if (robOfCurrentDay.HasValue &&
                calculatedValue.HasValue &&
                (Math.Round(calculatedValue.Value, 3) != Math.Round(robOfCurrentDay.Value, 3)))
                throw new BusinessRuleException("BR_FR10", string.Format("ROB for '{0}' is invalid.", this.Good.Code));
        }

        //===================================================================================

        /// <summary>
        /// Used in BR_FR10.
        /// </summary>
        private int getCorrectionTypeFactor(CorrectionTypes? correctionType)
        {
            switch (correctionType)
            {
                case CorrectionTypes.Plus:
                    return 1;

                case CorrectionTypes.Minus:
                    return -1;

                //case CorrectionTypes.NotDefined:
                case null:
                default:
                    return 0;
            }
        }

        //===================================================================================

        /// <summary>
        /// BR_FR34
        /// </summary>
        private void validateValuesForEndOfVoyageEndOfMonthEndOfYearFuelReport(
            decimal? rob,
            decimal? consumption,
            decimal? receive,
            ReceiveTypes? receiveType,
            decimal? transfer,
            TransferTypes? transferType,
            decimal? correction,
            CorrectionTypes? correctionType,
            decimal? correctionPrice,
            long? correctionPriceCurrencyId)
        {
            //if (FuelReport.FuelReportType == FuelReportTypes.EndOfVoyage ||
            //    FuelReport.FuelReportType == FuelReportTypes.EndOfMonth ||
            //    FuelReport.FuelReportType == FuelReportTypes.EndOfYear)
            if (this.FuelReport.FuelReportType == FuelReportTypes.EndOfVoyage || this.FuelReport.IsEndOfYearReport())
            {
                if (!rob.HasValue ||
                    !consumption.HasValue ||
                    receive.HasValue ||
                    receiveType.HasValue ||
                    transfer.HasValue ||
                    transferType.HasValue ||
                    correction.HasValue ||
                    correctionType.HasValue ||
                    correctionPrice.HasValue ||
                    correctionPriceCurrencyId.HasValue)
                {
                    throw new BusinessRuleException("BR_FR34",
                                                    string.Format("BR_FR34 : Values assignment for '{0}' is invalid against EndOfVoyage, EndOfMonth or EndOfYear Fuel Report.", this.Good.Code));
                }
            }
        }

        //===================================================================================

        #endregion

        public void ValidateTransferReferences(IOrderDomainService orderDomainService,
            IFuelReportDomainService fuelReportDomainService)
        {
            this.validateEmptyTransferValues();

            this.validateNotEmptyTransferValues();

            this.validateNotEmptyTransferSaleValues();

            this.validateTransferSaleReferenceValue(orderDomainService);

            this.validateNotEmptyInternalTransferValues();

            this.validateInternalTransferReferenceValue(orderDomainService);

            this.validateNotEmptyRejectTransferValues();

            this.validateRejectTransferReferenceValue(fuelReportDomainService);
        }

        //===================================================================================

        private void validateRejectTransferReferenceValue(IFuelReportDomainService fuelReportDomainService)
        {
            if (this.Transfer.HasValue &&
                this.TransferType.Value == TransferTypes.Rejected &&
                !this.TransferReference.IsEmpty())
            {
                var purchaseReceipts = fuelReportDomainService.GetFuelReportDetailRejectedTransferReferences(this);

                var transferRefCount = purchaseReceipts.Count(
                    r => r.ReferenceId == this.TransferReference.ReferenceId && this.TransferReference.ReferenceType == ReferenceType.Inventory);

                if (transferRefCount != 1)
                    throw new BusinessRuleException("", string.Format("Reject Transfer Reference Value for '{0}' is invalid.", this.Good.Code));
                //throw new BusinessRuleException("BR_FR29_2", "BR_FR19 - چنانچه فیلد انتقال مقداردهی گردیده است و نوع آن فروش انتقالی می باشد مرجع آن سفارش  انتقال تائید نهایی شده می باشد");
            }

        }

        //===================================================================================

        private void validateNotEmptyRejectTransferValues()
        {
            if (this.Transfer.HasValue &&
               this.TransferType.Value == TransferTypes.Rejected &&
               this.TransferReference.IsEmpty())
                throw new BusinessRuleException("", string.Format("Reject Transfer Reference Value for '{0}' is not specified.", this.Good.Code));
        }

        //===================================================================================

        private void validateInternalTransferReferenceValue(IOrderDomainService orderDomainService)
        {
            if (this.Transfer.HasValue &&
                this.TransferType.Value == TransferTypes.InternalTransfer &&
                !this.TransferReference.IsEmpty())
            {
                var finalApprovedInternalTransferOrder = orderDomainService.GetFinalApprovedInternalTransferOrders(this.FuelReport.VesselInCompany.CompanyId);

                var transferRefCount = finalApprovedInternalTransferOrder.Count(
                    o => o.Id == this.TransferReference.ReferenceId && this.TransferReference.ReferenceType == ReferenceType.Order);

                if (transferRefCount != 1)
                    throw new BusinessRuleException("BR_FR29_2", string.Format("BR_FR19 : Internal Transfer Reference Value for '{0}' is invalid.", this.Good.Code));
                //throw new BusinessRuleException("BR_FR29_2", "BR_FR19 - چنانچه فیلد انتقال مقداردهی گردیده است و نوع آن فروش انتقالی می باشد مرجع آن سفارش  انتقال تائید نهایی شده می باشد");
            }
        }

        //===================================================================================

        private void validateNotEmptyInternalTransferValues()
        {
            if (this.Transfer.HasValue &&
               this.TransferType.Value == TransferTypes.InternalTransfer &&
               this.TransferReference.IsEmpty())
                throw new BusinessRuleException("BR_FR29_1", string.Format("BR_FR29 : InternalTransfer Reference Value for '{0}' is not specified.", this.Good.Code));
            //throw new BusinessRuleException("BR_FR29_1", "BR_FR29 -  مرجع حواله انتقال داخلی سفارش انتقال داخلی تائید نهایی شده می باشد و بایستی مرجع و شماره مرجع مشخص شده باشد");
        }

        //===================================================================================

        private void validateTransferSaleReferenceValue(IOrderDomainService orderDomainService)
        {
            if (this.Transfer.HasValue &&
                this.TransferType.Value == TransferTypes.TransferSale &&
                !this.TransferReference.IsEmpty())
            {
                var finalApprovedTransferOrder = orderDomainService.GetFinalApprovedTransferOrders(this.FuelReport.VesselInCompany.CompanyId);

                var transferRefCount = finalApprovedTransferOrder.Count(
                    o =>
                        o.Id == this.TransferReference.ReferenceId && this.TransferReference.ReferenceType == ReferenceType.Order);

                if (transferRefCount != 1)
                    throw new BusinessRuleException("BR_FR19", string.Format("BR_FR19: TransferSale Reference Value for '{0}' is invalid.", this.Good.Code));
                //throw new BusinessRuleException("BR_FR19", "BR_FR19 - چنانچه فیلد انتقال مقداردهی گردیده است و نوع آن فروش انتقالی می باشد مرجع آن سفارش  انتقال تائید نهایی شده می باشد");
            }
        }

        //===================================================================================

        private void validateNotEmptyTransferSaleValues()
        {
            if (this.Transfer.HasValue &&
                this.TransferType.Value == TransferTypes.TransferSale &&
                this.TransferReference.IsEmpty())
                throw new BusinessRuleException("BR_FR 20", string.Format("BR_FR 20 : TransferSale Reference for '{0}' is not specified.", this.Good.Code));
            //throw new BusinessRuleException("", "BR_FR 20 - چنانچه فیلد انتقال مقداردهی شده است و نوع آن فروش انتقالی است مرجع و شماره مرجع آن مشخص شده باشد");
        }

        //===================================================================================

        private void validateNotEmptyTransferValues()
        {
            if (this.Transfer.HasValue &&
                !this.TransferType.HasValue)
                throw new BusinessRuleException("", string.Format("TransferType for '{0}' is not specified.", this.Good.Code));
            //throw new BusinessRuleException("", "فیلد نوع انتقال مقداردهی نگردیده است.");
        }

        //===================================================================================

        private void validateEmptyTransferValues()
        {
            if (!this.Transfer.HasValue &&
                (this.TransferType.HasValue ||
                !this.TransferReference.IsEmpty())
            )
                throw new BusinessRuleException("BR_FR 11", string.Format("BR_FR 11: Transfer for '{0}' is empty but related Values are specified.", this.Good.Code));
            //throw new BusinessRuleException("BR_FR 11", "BR_FR 11 -  چنانچه فیلد انتقال مقداردهی نشده باشد ، فیلدهای نوع انتقال ، نوع مرجع و شماره آن غیر فعال می باشد");
        }

        //===================================================================================

        public void ValidateReceiveReferences(IOrderDomainService orderDomainService)
        {
            this.validateEmptyReceiveValues();

            this.validateNotEmptyReceiveTypeValue();

            this.validateNotEmptyReceiveReferenceValue();

            this.validatePurchaseReceiveTypeReference(orderDomainService);

            this.validateTransferPurchaseReceiveReference(orderDomainService);

            this.validateInternalTransferReceiveReference(orderDomainService);

            this.validateTrustReceiveReference(orderDomainService);
        }

        //===================================================================================

        private void validateTrustReceiveReference(IOrderDomainService orderDomainService)
        {
            //Old implementation of Trust Receive validation
            //if (this.ReceiveType.HasValue &&
            //    this.ReceiveType.Value == ReceiveTypes.Trust &&
            //    !this.ReceiveReference.IsEmpty())
            //{
            //    var finalApprovedInternalTransferOrders = orderDomainService.GetFinalApprovedInternalTransferOrders(this.FuelReport.VesselInCompany.CompanyId);

            //    var receiveRefCount = finalApprovedInternalTransferOrders.Count(
            //        o => o.Id == this.ReceiveReference.ReferenceId && this.ReceiveReference.ReferenceType == ReferenceType.Order);

            //    if (receiveRefCount != 1)
            //        throw new BusinessRuleException("", string.Format("BR_FR31 : Trust Receive Reference for '{0}' is invalid.", this.Good.Code));
            //}

            if (this.ReceiveType.HasValue &&
                this.ReceiveType.Value == ReceiveTypes.Trust &&
                !this.ReceiveReference.IsEmpty())
            {
                throw new BusinessRuleException("", string.Format("BR_FR31 : Reference of Trust Receive for '{0}' should be empty.", this.Good.Code));
            }
        }

        //===================================================================================

        private void validateInternalTransferReceiveReference(IOrderDomainService orderDomainService)
        {
            if (this.ReceiveType.HasValue &&
                this.ReceiveType.Value == ReceiveTypes.InternalTransfer)
            {
                var finalApprovedInternalTransferOrders = orderDomainService.GetFinalApprovedInternalTransferOrders(this.FuelReport.VesselInCompany.CompanyId);

                var receiveRefCount = finalApprovedInternalTransferOrders.Count(
                    o => o.Id == this.ReceiveReference.ReferenceId && this.ReceiveReference.ReferenceType == ReferenceType.Order);

                if (receiveRefCount != 1)
                    throw new BusinessRuleException("BR_FR 31", string.Format("BR_FR31 : Internal-Transfer Receive Reference for '{0}' is invalid.", this.Good.Code));
                //throw new BusinessRuleException("BR_FR 31", "BR_FR31 - چنانچه نوع دریافت انتقال داخلی می باشد مرجع آن که سفارش انتقال داخلی تائید نهایی شده  می باشد مشخص شده باشد");
            }
        }

        //===================================================================================

        private void validateTransferPurchaseReceiveReference(IOrderDomainService orderDomainService)
        {
            if (this.ReceiveType.HasValue &&
                this.ReceiveType.Value == ReceiveTypes.TransferPurchase)
            {
                var finalApprovedPrurchaseTransferOrders = orderDomainService.GetBuyerFinalApprovedPurchaseTransferOrders(this.FuelReport.VesselInCompany.CompanyId);

                var receiveRefCount = finalApprovedPrurchaseTransferOrders.Count(
                    o => o.Id == this.ReceiveReference.ReferenceId && this.ReceiveReference.ReferenceType == ReferenceType.Order);

                if (receiveRefCount != 1)
                    throw new BusinessRuleException("BR_FR 16_2", string.Format("BR_FR 16 : Transfer-Purchase Receive Reference for '{0}' is invalid.", this.Good.Code));
                //throw new BusinessRuleException("BR_FR 16_2", "BR_FR 16 - چنانچه نوع آن خرید انتقالی می باشد مرجع آن سفارش خرید انتقالی تائید نهایی شده باشد");
            }
        }

        //===================================================================================

        private void validatePurchaseReceiveTypeReference(IOrderDomainService orderDomainService)
        {
            if (this.ReceiveType.HasValue &&
                this.ReceiveType.Value == ReceiveTypes.Purchase)
            {
                var finalApprovedPrurchaseOrders = orderDomainService.GetFinalApprovedPrurchaseOrders(this.FuelReport.VesselInCompany.CompanyId, this.FuelReport.VesselInCompanyId);

                var receiveRefCount = finalApprovedPrurchaseOrders.Count(
                    o => o.Id == this.ReceiveReference.ReferenceId && this.ReceiveReference.ReferenceType == ReferenceType.Order);

                if (receiveRefCount != 1)
                    throw new BusinessRuleException("BR_FR 16_1", string.Format("BR_FR 16 : Purchase Receive Type Reference for '{0}' is invlaid.", this.Good.Code));
                //throw new BusinessRuleException("BR_FR 16_1", "BR_FR 16 -چنانچه نوع دریافت خرید می باشد  مرجع آن سفارش خرید تائید نهایی شده باشد");
            }
        }

        //===================================================================================

        private void validateNotEmptyReceiveReferenceValue()
        {
            if (this.Receive.HasValue)
            {
                if (this.ReceiveType.Value != ReceiveTypes.Trust && this.ReceiveReference.IsEmpty())
                    throw new BusinessRuleException("BR_FR 14", string.Format("BR_FR 14 : Receive Reference for '{0}' is not specified.", this.Good.Code));
                //throw new BusinessRuleException("BR_FR 14", "BR_FR 14 : -  چنانچه فیلد دریافت مقداردهی گردیده است نوع مرجع و شماره مرجع آن مقداردهی گردیده باشد");
            }
        }

        //===================================================================================

        private void validateNotEmptyReceiveTypeValue()
        {
            if (this.Receive.HasValue &&
                !this.ReceiveType.HasValue)
                throw new BusinessRuleException("", string.Format("Receive Type for '{0}' is not specified.", this.Good.Code));
            //throw new BusinessRuleException("", "فیلد نوع دریافت مقداردهی نگردیده است.");
        }

        //===================================================================================

        private void validateEmptyReceiveValues()
        {
            if (!this.Receive.HasValue &&
                (this.ReceiveType.HasValue ||
                !this.ReceiveReference.IsEmpty())
            )
                throw new BusinessRuleException("BR_FR 26", string.Format("BR_FR 26 : Receive for '{0}' is empty but related Values are specified.", this.Good.Code));
            //throw new BusinessRuleException("BR_FR 26", "BR_FR 26 -  چنانچه فیلد دریافت مقداردهی نشده باشد ، فیلدهای نوع خرید ، نوع مرجع و شماره آن غیر فعال می باشد");
        }

        //===================================================================================

        public void ValidateCorrectionReferences(
            bool isTheFirstFuelReport,
            IVoyageDomainService voyageDomainService,
            IFuelReportDomainService fuelReportDomainService,
            ICurrencyDomainService currencyDomainService
            )
        {
            this.validateEmptyCorrectionValues();

            if (this.Correction.HasValue && this.CorrectionType.HasValue)
            {
                if (this.CorrectionType.Value == CorrectionTypes.Plus)
                {
                    this.validateIncrementalCorrectionTypeValues(this.Correction, this.CorrectionType, this.CorrectionPricingType, this.CorrectionPriceCurrencyId, this.CorrectionPrice, this.CorrectionReference, currencyDomainService);

                }
                else if (this.CorrectionType.Value == CorrectionTypes.Minus)
                {
                    this.validateDecrementalCorrectionTypeValues(this.Correction, this.CorrectionType, this.CorrectionPricingType, this.CorrectionPriceCurrencyId, this.CorrectionPrice, this.CorrectionReference, currencyDomainService);
                }

                this.validatePositiveCorrectionPriceAndReference();

                //validateNegativeCorrectionReferenceValue(isTheFirstFuelReport, voyageDomainService);

                this.validateNegativeCorrectionWithEmptyPrice();

                this.validatePositiveCorrectionReferenceToBeTheLastIssuedVoyage(isTheFirstFuelReport, fuelReportDomainService);
                this.validateNegativeCorrectionReferenceToBeTheLastIssuedVoyage(isTheFirstFuelReport, fuelReportDomainService);
            }
        }

        //===================================================================================

        //private void validateNegativeCorrectionReferenceValue(bool isTheFirstFuelReport, IVoyageDomainService voyageDomainService)
        //{
        //    if (this.Correction.HasValue &&
        //        this.CorrectionType.HasValue &&
        //        this.CorrectionType.Value == CorrectionTypes.Minus)
        //    {
        //        //if (!isTheFirstFuelReport &&
        //        //    (this.CorrectionReference.IsEmpty() ||
        //        //    this.CorrectionReference.ReferenceType != ReferenceType.Voyage ||
        //        //        voyageDomainService.Get(this.CorrectionReference.ReferenceId.Value) == null))
        //        //    throw new BusinessRuleException("BR_FR 24", string.Format("BR_FR 24 : Reference for Negative Correction of '{0}' is not specified.", this.Good.Code));
        //        ////throw new BusinessRuleException("", "BR_FR 24 - چنانچه فیلد اصلاح مقداری مقداردهی شده است و نوع آن منفی است مرجع آن می تواند سفر انتخاب گردد تا در زمان ثبت سند برای سفر هزینه ثبت شود");

        //    }
        //}

        //===================================================================================

        private void validatePositiveCorrectionPriceAndReference()
        {
            if (this.Correction.HasValue &&
                this.CorrectionType.HasValue &&
                this.CorrectionType.Value == CorrectionTypes.Plus &&
                this.CorrectionPrice.HasValue &&
                !this.CorrectionReference.IsEmpty())
                throw new BusinessRuleException("", string.Format("Reference and Price are specified for Positive Correction of '{0}'.", this.Good.Code));
            //throw new BusinessRuleException("", "چنانچه فیلد مقدار اصلاحی مقداردهی شده باشد و نوع آن مثبت باشد و فیلد  مبلغ اصلاحی نیز مقداردهی شده باشد ، مرجع برای آن غیر فعال می گردد");
        }

        //===================================================================================

        private void validateNegativeCorrectionWithEmptyPrice()
        {
            if (this.Correction.HasValue &&
                this.CorrectionType.HasValue &&
                this.CorrectionType.Value == CorrectionTypes.Minus &&
                 (this.CorrectionPrice.HasValue /*&&
                 this.CorrectionPriceCurrencyId.HasValue*/)
                )
                throw new BusinessRuleException("", string.Format("Decremental Correction for '{0}' has Price.", this.Good.Code));
            //throw new BusinessRuleException("", "یکی از مقادیر نوع مقدار اصلاحی، مبلغ و یا نوع ارز برای مقدار اصلاحی وارد نشده است.");
        }

        //===================================================================================

        private void validatePositiveCorrectionReferenceToBeTheLastIssuedVoyage(bool isTheFirstFuelReport, IFuelReportDomainService fuelReportDomainService)
        {
            if (this.Correction.HasValue &&
                !isTheFirstFuelReport &&
                !this.CorrectionReference.IsEmpty() &&
                this.CorrectionType.HasValue &&
                this.CorrectionType.Value == CorrectionTypes.Plus)
            {
                var lastIssuedEOVFuelReportOfPreviousVoyages = fuelReportDomainService.GetLastIssuedEOVFuelReportOfPreviousVoyages(this.FuelReport);

                if (lastIssuedEOVFuelReportOfPreviousVoyages == null && !isTheFirstFuelReport)
                    throw new ObjectNotFound("LastIssuedEndOfVoyageFuelReport");

                Voyage lastIssuedVoyage = null;

                if (lastIssuedEOVFuelReportOfPreviousVoyages != null)
                    lastIssuedVoyage = lastIssuedEOVFuelReportOfPreviousVoyages.Voyage;

                if (lastIssuedVoyage == null || this.CorrectionReference.ReferenceType != ReferenceType.Voyage ||
                    this.CorrectionReference.ReferenceId != lastIssuedVoyage.Id)
                    throw new BusinessRuleException("", string.Format("Correction Reference Is not The Last Issued Voyage for '{0}'.", this.Good.Code));
                //throw new BusinessRuleException("", "سفر مرجع برای مقداراصلاحی آخرین سفر دارای حواله مصرف تائید نهایی شده باشد");
            }
        }

        //===================================================================================

        private void validateNegativeCorrectionReferenceToBeTheLastIssuedVoyage(bool isTheFirstFuelReport, IFuelReportDomainService fuelReportDomainService)
        {
            if (this.Correction.HasValue &&
                !isTheFirstFuelReport &&
                !this.CorrectionReference.IsEmpty() &&
                this.CorrectionType.HasValue &&
                this.CorrectionType.Value == CorrectionTypes.Minus)
            {
                var lastIssuedEOVFuelReportOfPreviousVoyages = fuelReportDomainService.GetLastIssuedEOVFuelReportOfPreviousVoyages(this.FuelReport);

                if (lastIssuedEOVFuelReportOfPreviousVoyages == null && !isTheFirstFuelReport)
                    throw new ObjectNotFound("LastIssuedEndOfVoyageFuelReport");

                Voyage lastIssuedVoyage = null;

                if (lastIssuedEOVFuelReportOfPreviousVoyages != null)
                    lastIssuedVoyage = lastIssuedEOVFuelReportOfPreviousVoyages.Voyage;


                if (lastIssuedVoyage == null || this.CorrectionReference.ReferenceType != ReferenceType.Voyage ||
                    this.CorrectionReference.ReferenceId != lastIssuedVoyage.Id)
                    throw new BusinessRuleException("", string.Format("Correction Reference Is not The Last Issued Voyage for '{0}'.", this.Good.Code));
                //throw new BusinessRuleException("", "سفر مرجع برای مقداراصلاحی آخرین سفر دارای حواله مصرف تائید نهایی شده باشد");
            }
        }

        //===================================================================================

        private void validateEmptyCorrectionValues()
        {
            if (!this.Correction.HasValue &&
                 (this.CorrectionType.HasValue ||
                 this.CorrectionPrice.HasValue ||
                 this.CorrectionPriceCurrencyId.HasValue ||
                 !this.CorrectionReference.IsEmpty())
                )
                throw new BusinessRuleException("BR_FR 25", string.Format("BR_FR 25 : The Correction Values for {0} are not specified correctly.", this.Good.Code));
            //throw new BusinessRuleException("BR_FR 25", "BR_FR 25 -  چنانچه فیلد مقدار اصلاحی مقداردهی نشده باشد ، فیلدهای نوع مقدار اصلاحی ، نوع مرجع و شماره آن غیر فعال می باشد");
        }

        //===================================================================================

        #region Commented

        /*
        internal FuelReportNotificationDataItem GetNotificationDataItem(IFuelReportDomainService fuelReportDomainService)
        {
            FuelReportNotificationDataItem item = new FuelReportNotificationDataItem();

            item.FuelReportDetailId = this.Id;

            item.Transfer = getTransferData();
            item.Receive = getReceiveData();
            item.PositiveCorrection = getPositiveCorrectionData();
            item.NegativeCorrection = getNegativeCorrectionData();
            item.EndOfMonth = getEndOfMonthData(fuelReportDomainService);
            item.EndOfYear = getEndOfYearData(fuelReportDomainService);
            item.EndOfVoyage = getEndOfVoyageData(fuelReportDomainService);

            return item;
        }

        //===================================================================================

        private ReceiveData getReceiveData()
        {
            ReceiveData receiveData = null;

            if (this.Receive.HasValue)
            {
                receiveData = new ReceiveData();

                receiveData.CompanyId = this.FuelReport.VesselInCompany.CompanyId;
                receiveData.VesselInCompanyId = this.FuelReport.VesselInCompanyId;
                receiveData.CompanyGoodId = this.GoodId;
                receiveData.UnitId = this.MeasuringUnitId;
                receiveData.ReportDate = this.FuelReport.ReportDate;
                receiveData.TankId = this.TankId;
                receiveData.Receive = this.Receive.Value;
                receiveData.ReceiveType = this.ReceiveType.Value;
                receiveData.ReferenceType = (ReceiveReferenceType)(int)this.ReceiveReference.ReferenceType.Value;
                receiveData.ReferenceId = this.ReceiveReference.ReferenceId.Value;
            }

            return receiveData;
        }

        //===================================================================================

        private TransferData getTransferData()
        {
            TransferData transferData = null;

            if (this.Transfer.HasValue)
            {
                transferData = new TransferData();

                transferData.CompanyId = this.FuelReport.VesselInCompany.CompanyId;
                transferData.VesselInCompanyId = this.FuelReport.VesselInCompanyId;
                transferData.CompanyGoodId = this.GoodId;
                transferData.UnitId = this.MeasuringUnitId;
                transferData.ReportDate = this.FuelReport.ReportDate;
                transferData.TankId = this.TankId;
                transferData.Transfer = this.Transfer.Value;
                transferData.TransferType = this.TransferType.Value;
            }

            return transferData;
        }

        //===================================================================================

        private PositiveCorrectionData getPositiveCorrectionData()
        {
            PositiveCorrectionData positiveCorrectionData = null;

            if (this.Correction.HasValue && this.CorrectionType.Value == CorrectionTypes.Plus)
            {
                positiveCorrectionData = new PositiveCorrectionData();

                positiveCorrectionData.CompanyId = this.FuelReport.VesselInCompany.CompanyId;
                positiveCorrectionData.VesselInCompanyId = this.FuelReport.VesselInCompanyId;
                positiveCorrectionData.CompanyGoodId = this.GoodId;
                positiveCorrectionData.UnitId = this.MeasuringUnitId;
                positiveCorrectionData.ReportDate = this.FuelReport.ReportDate;
                positiveCorrectionData.TankId = this.TankId;
                positiveCorrectionData.Correction = this.Correction.Value;
                positiveCorrectionData.CorrectionType = this.CorrectionType.Value;

                if (!this.CorrectionReference.IsEmpty())
                {
                    positiveCorrectionData.ReferenceType = CorrectionReferenceType.Voyage;
                    positiveCorrectionData.ReferenceId = this.CorrectionReference.ReferenceId.Value;
                }
                else
                {
                    if (this.CorrectionPrice.HasValue)
                    {
                        positiveCorrectionData.ReferenceType = CorrectionReferenceType.FuelReportDetailNumber;
                        positiveCorrectionData.ReferenceId = this.Id;
                    }
                    else
                    {
                        positiveCorrectionData.ReferenceType = CorrectionReferenceType.NotSpecified;
                        positiveCorrectionData.ReferenceId = 0L;
                    }
                }
            }

            return positiveCorrectionData;
        }

        //===================================================================================

        private NegativeCorrectionData getNegativeCorrectionData()
        {
            NegativeCorrectionData negativeCorrectionData = null;

            if (this.Correction.HasValue && this.CorrectionType.Value == CorrectionTypes.Minus)
            {
                negativeCorrectionData = new NegativeCorrectionData();

                negativeCorrectionData.CompanyId = this.FuelReport.VesselInCompany.CompanyId;
                negativeCorrectionData.VesselInCompanyId = this.FuelReport.VesselInCompanyId;
                negativeCorrectionData.CompanyGoodId = this.GoodId;
                negativeCorrectionData.UnitId = this.MeasuringUnitId;
                negativeCorrectionData.ReportDate = this.FuelReport.ReportDate;
                negativeCorrectionData.TankId = this.TankId;
                negativeCorrectionData.Correction = this.Correction.Value;
                negativeCorrectionData.IsReferenceSpecified = !this.CorrectionReference.IsEmpty();
            }

            return negativeCorrectionData;
        }

        //===================================================================================

        private EndOfMonthData getEndOfMonthData(IFuelReportDomainService fuelReportDomainService)
        {
            EndOfMonthData endOfMonthData = null;

            if (this.FuelReport.FuelReportType == FuelReportTypes.EndOfMonth)
            {
                endOfMonthData = new EndOfMonthData();

                endOfMonthData.CompanyId = this.FuelReport.VesselInCompany.CompanyId;
                endOfMonthData.VesselInCompanyId = this.FuelReport.VesselInCompanyId;
                endOfMonthData.CompanyGoodId = this.GoodId;
                endOfMonthData.UnitId = this.MeasuringUnitId;
                endOfMonthData.ReportDate = this.FuelReport.ReportDate;
                endOfMonthData.TankId = this.TankId;
                endOfMonthData.ConsumptionQuantity = claculateInventoryOperationEndOfMonthOrYearConsumption(fuelReportDomainService);
            }

            return endOfMonthData;
        }

        //===================================================================================

        private EndOfYearData getEndOfYearData(IFuelReportDomainService fuelReportDomainService)
        {
            EndOfYearData endOfYearData = null;

            if (this.FuelReport.FuelReportType == FuelReportTypes.EndOfYear)
            {
                endOfYearData = new EndOfYearData();

                endOfYearData.CompanyId = this.FuelReport.VesselInCompany.CompanyId;
                endOfYearData.VesselInCompanyId = this.FuelReport.VesselInCompanyId;
                endOfYearData.CompanyGoodId = this.GoodId;
                endOfYearData.UnitId = this.MeasuringUnitId;
                endOfYearData.ReportDate = this.FuelReport.ReportDate;
                endOfYearData.TankId = this.TankId;
                endOfYearData.ConsumptionQuantity = claculateInventoryOperationEndOfMonthOrYearConsumption(fuelReportDomainService);
            }

            return endOfYearData;
        }

        //===================================================================================

        private EndOfVoyageData getEndOfVoyageData(IFuelReportDomainService fuelReportDomainService)
        {
            EndOfVoyageData endOfVoyageData = null;

            if (this.FuelReport.FuelReportType == FuelReportTypes.EndOfVoyage)
            {
                endOfVoyageData = new EndOfVoyageData();

                endOfVoyageData.CompanyId = this.FuelReport.VesselInCompany.CompanyId;
                endOfVoyageData.VesselInCompanyId = this.FuelReport.VesselInCompanyId;
                endOfVoyageData.CompanyGoodId = this.GoodId;
                endOfVoyageData.UnitId = this.MeasuringUnitId;
                endOfVoyageData.ReportDate = this.FuelReport.ReportDate;
                endOfVoyageData.TankId = this.TankId;
                endOfVoyageData.ConsumptionQuantity = claculateInventoryOperationEndOfMonthOrYearConsumption(fuelReportDomainService);
            }

            return endOfVoyageData;
        }

        //===================================================================================

        private double claculateInventoryOperationEndOfMonthOrYearConsumption(IFuelReportDomainService fuelReportDomainService)
        {
            var lastIssuedFuelReportBefore = fuelReportDomainService.GetLastIssuedFuelReportBefore(this.FuelReport);

            var allFuelReportsFromLastIssuedTillCurrent =
                fuelReportDomainService.GetVesselFuelReports(
                    this.FuelReport.VesselInCompany,
                    (lastIssuedFuelReportBefore == null) ? null : (DateTime?)lastIssuedFuelReportBefore.ReportDate,
                    this.FuelReport.ReportDate);

            double totalTransfers =
                allFuelReportsFromLastIssuedTillCurrent
                    .Sum(fr =>
                            fr.FuelReportDetails
                                .Where(frd => frd.GoodId == this.GoodId && frd.Transfer.HasValue)
                                .Sum(frd => frd.Transfer.Value));

            double totalReceives =
                allFuelReportsFromLastIssuedTillCurrent
                    .Sum(fr =>
                            fr.FuelReportDetails
                                .Where(frd => frd.GoodId == this.GoodId && frd.Receive.HasValue)
                                .Sum(frd => frd.Receive.Value));

            double totalCorrections =
                allFuelReportsFromLastIssuedTillCurrent
                    .Sum(fr =>
                            fr.FuelReportDetails
                                .Where(frd => frd.GoodId == this.GoodId && frd.Correction.HasValue)
                                .Sum(frd => frd.Correction.Value * getCorrectionTypeFactor(frd.CorrectionType)));

            double currentROB = this.ROB;

            double lastIssuedROB = lastIssuedFuelReportBefore == null ? 0 : lastIssuedFuelReportBefore.FuelReportDetails.First(frd => frd.GoodId == this.GoodId).ROB;

            double inventotyConsumptionQuantity = lastIssuedROB - currentROB - totalTransfers + totalReceives + totalCorrections;

            return inventotyConsumptionQuantity;
        }
        */

        #endregion

        //===================================================================================

        //internal void Update(decimal rob, decimal consumption,
        //    FuelReportDetail relevantFuelReportDetailOfTheDayBefore,
        //    bool isDetailOfFirstFuelReport,
        //    CharterItem relevantCharterItemAtFuelReportTime,
        //    ICurrencyDomainService currencyDomainService)
        //{
        //    rob = Math.Round(rob, 3);
        //    consumption = Math.Round(consumption, 3);

        //    this.validateValues(rob, consumption, this.Receive, this.ReceiveType, this.Transfer, this.TransferType, this.Correction,
        //        this.CorrectionType, this.CorrectionPricingType, this.CorrectionPrice, this.CorrectionPriceCurrencyId, this.TransferReference, this.ReceiveReference, this.CorrectionReference, relevantFuelReportDetailOfTheDayBefore, isDetailOfFirstFuelReport, relevantCharterItemAtFuelReportTime, currencyDomainService);

        //    this.ROB = rob;
        //    this.Consumption = consumption;

            
        //}

        //===================================================================================

        internal void UpdateConsumption(FuelReportDetail relevantFeulReportDetailOfPreviousReport)
        {
            decimal? calculatedCorrection = (this.Correction * this.getCorrectionTypeFactor(this.CorrectionType));

            decimal calculatedNewConsumption =
                this.ROB - relevantFeulReportDetailOfPreviousReport.ROB +
                    (this.Receive ?? 0) +
                    (calculatedCorrection ?? 0) -
                    (this.Transfer ?? 0);


            this.Consumption = calculatedNewConsumption;
        }

        //===================================================================================

        public bool IsCorrectionPriceEmpty()
        {
            return !(this.CorrectionPrice.HasValue && this.CorrectionPriceCurrencyId.HasValue);
        }

        internal void ChangeCompany(long destinationCompanyId, IGoodDomainService goodDomainService)
        {
            var destinationGood = goodDomainService.FindGood(destinationCompanyId, this.Good.SharedGoodId);

            if (destinationGood == null)
                throw new ObjectNotFound("Good in Destination Company could not be found for " + this.Good.Code);
            this.GoodId = destinationGood.Id;
            this.Good = destinationGood;
        }

        /// <summary>
        /// Indicates whether the correction values prices are reviewed and updated by financial user.
        /// </summary>
        public bool IsCorrectionPricingTypeRevised { get; private set; }

        internal void ValidateToBeRevisedByFinance()
        {
            if (this.Correction.HasValue && this.CorrectionType.HasValue &&
                !this.IsCorrectionPricingTypeRevised)
            {
                throw new BusinessRuleException("", "Correction Pricing for \"" + this.Good.Code + "\" is not revised.");
            }
        }

        public CharterItem GetRelevantActivationCharterItem()
        {
            return FuelReport.ActivationCharterContract.CharterItems.SingleOrDefault(ci=>ci.GoodId == this.GoodId);
        }

        internal void ResetCorrectionPriceRevisionState()
        {
            this.IsCorrectionPricingTypeRevised = false;
        }
    }
    }
