using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MITD.Core;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.ACL.StorageSpace.SAPIDVoucherService;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Integration.Inventory;
using MITD.Fuel.Integration.Inventory.Infrastructure;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices.Inventory;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Integration.Inventory.Infrastructure;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events
{
    public class InventoryOperationNotifier : IInventoryOperationNotifier
    {
        private readonly IAddCharterInStartReceiptVoucher addCharterInStartReceiptVoucher;
        private readonly IAddCharterOutStartIssueVoucher addCharterOutStartIssueVoucher;
        private readonly IAddCharterInEndIssueVoucher addCharterInEndIssueVoucher;
        private readonly IAddCharterOutEndReceiptVoucher addCharterOutEndReceiptVoucher;
        private readonly IAddConsumptionIssueVoucher addConsumptionIssueVoucher;
        private readonly IAddPurchesInvoiceVoucher addPurchaseInvoiceVoucher;
        private readonly IAddTransferBarjingInvoiceVoucher addTransferBarjingInvoiceVoucher;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IGoodRepository goodRepository;

        private readonly IAddCharterInEndBackReciptVoucher addCharterInEndBackReceiptVoucher;
        private readonly IAddCharterInEndConsumptionIssueVoucher addCharterInEndConsumptionIssueVoucher;

        private readonly IAddCharterOutStartBackReceiptVoucher addCharterOutStartBackReceiptVoucher;
        private readonly IAddCharterOutStartConsumptionIssueVoucher addCharterOutStartConsumptionIssueVoucher;
        private readonly IAddSaleTransitionIssueVoucher addSaleTransitionIssueVoucher;
        private IAddPlusCorrectionReceiptVoucher addPlusCorrectionReceiptVoucher;
        private IAddMinusCorrectionReceiptVoucher addMinusCorrectionReceiptVoucher;

        private readonly IInventoryOperationManager inventoryOperationManager;
        private readonly IEntityConfigurator<VesselInCompany> vesselInCompanyConfigurator;

        public InventoryOperationNotifier(
            IInventoryOperationManager inventoryOperationManager,
            IAddCharterInStartReceiptVoucher addCharterInStartReceiptVoucher,
            IAddCharterOutStartIssueVoucher addCharterOutStartIssueVoucher,
            IAddCharterInEndIssueVoucher addCharterInEndIssueVoucher,
            IAddCharterOutEndReceiptVoucher addCharterOutEndReceiptVoucher,
            IAddConsumptionIssueVoucher addConsumptionIssueVoucher,

            IAddPurchesInvoiceVoucher addPurchaseInvoiceVoucher,
            IAddCharterInEndBackReciptVoucher addCharterInEndBackReceiptVoucher,
            IAddCharterInEndConsumptionIssueVoucher addCharterInEndConsumptionIssueVoucher,
            IAddCharterOutStartConsumptionIssueVoucher addCharterOutStartConsumptionIssueVoucher,
            IAddCharterOutStartBackReceiptVoucher addCharterOutStartBackReceiptVoucher,
            IAddSaleTransitionIssueVoucher addSaleTransitionIssueVoucher,
            ICurrencyDomainService currencyDomainService,
            IGoodRepository goodRepository, IAddTransferBarjingInvoiceVoucher addTransferBarjingInvoiceVoucher, IAddPlusCorrectionReceiptVoucher addPlusCorrectionReceiptVoucher, IAddMinusCorrectionReceiptVoucher addMinusCorrectionReceiptVoucher, IEntityConfigurator<VesselInCompany> vesselInCompanyConfigurator)
        {
            //this.inventoryOperationManager = new InventoryOperationManager();
            this.inventoryOperationManager = inventoryOperationManager;
            this.addCharterInStartReceiptVoucher = addCharterInStartReceiptVoucher;
            this.addCharterOutStartIssueVoucher = addCharterOutStartIssueVoucher;
            this.addCharterInEndIssueVoucher = addCharterInEndIssueVoucher;
            this.addCharterOutEndReceiptVoucher = addCharterOutEndReceiptVoucher;
            this.addConsumptionIssueVoucher = addConsumptionIssueVoucher;

            this.addPurchaseInvoiceVoucher = addPurchaseInvoiceVoucher;
            this.currencyDomainService = currencyDomainService;
            this.goodRepository = goodRepository;
            this.addTransferBarjingInvoiceVoucher = addTransferBarjingInvoiceVoucher;
            this.addPlusCorrectionReceiptVoucher = addPlusCorrectionReceiptVoucher;
            this.addMinusCorrectionReceiptVoucher = addMinusCorrectionReceiptVoucher;
            this.vesselInCompanyConfigurator = vesselInCompanyConfigurator;
            this.addCharterInEndBackReceiptVoucher = addCharterInEndBackReceiptVoucher;
            this.addCharterInEndConsumptionIssueVoucher = addCharterInEndConsumptionIssueVoucher;
            this.addCharterOutStartBackReceiptVoucher = addCharterOutStartBackReceiptVoucher;
            this.addCharterOutStartConsumptionIssueVoucher = addCharterOutStartConsumptionIssueVoucher;
            this.addCharterOutStartBackReceiptVoucher = addCharterOutStartBackReceiptVoucher;
            this.addSaleTransitionIssueVoucher = addSaleTransitionIssueVoucher;
        }

        public InventoryOperationResult NotifySubmittingFuelReportConsumption(FuelReport fuelReport, long userId)
        {
            try
            {
                var result = new InventoryOperationResult();

                var operationReference = inventoryOperationManager.GetFuelReportConsumptionReference(fuelReport);

                if (operationReference == null && !(fuelReport.FuelReportType == FuelReportTypes.EndOfVoyage || fuelReport.IsEndOfYearReport()))
                    return result; //This line states that the submitting FuelReport is not EOV, EOY and does not have any previous registered consumption transaction in inventory, also.

                if (operationReference == null)
                {
                    //No inventory operation is found, so the inventory operation will be done from scratch...
                    var goodsConsumption = calculateGoodsConsumption(fuelReport);

                    var operation = this.inventoryOperationManager.ManageFuelReportConsumption(fuelReport, goodsConsumption, (int)userId);

                    var operationTransaction = this.inventoryOperationManager.GetTransaction(operation.InventoryOperationId, InventoryOperationType.Issue);

                    if (operationTransaction.Status.Value == (byte)TransactionState.FullPriced)
                    {
                        var issueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(fuelReport.VesselInCompany.CompanyId, goodRepository);

                        try
                        {
                            addConsumptionIssueVoucher.Execute(issueDataForFinance, fuelReport, operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId);
                        }
                        catch
                        {
                        }
                    }

                    result.CreatedInventoryOperations.Add(operation);
                }
                else if (operationReference != null)
                {
                    //There is already an inventory operation for given fuel report...

                    if (!(fuelReport.FuelReportType == FuelReportTypes.EndOfVoyage || fuelReport.IsEndOfYearReport()))
                    {
                        //A Consumption Inventory Issue is found for current FuelReport but as the type of current FuelReport is changed, the consumption operation should be reverted.
                        //result.Merge(inventoryOperationManager.RevertTransaction(operationReference, (int)userId));

                        result.Merge(this.RevertFuelReportConsumptionInventoryOperations(fuelReport, (int)userId));
                    }
                    else
                    {
                        //The type of current FuelReport is EOV or EOY, so its inventory operation should be corrected based on new values for goods.
                        var goodsConsumption = calculateGoodsConsumption(fuelReport);

                        var entityGoodsQuantities = createEntityFinalValues(fuelReport, (int)operationReference.OperationId, goodsConsumption);

                        result.CreatedInventoryOperations.AddRange(inventoryOperationManager.CorrectTransaction(operationReference, null, entityGoodsQuantities, null, (int)userId));
                    }
                }

                return result;
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }


        /// <summary>
        /// Performs inventory related operations for given fuel report detail.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fuelReportDomainService"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InventoryOperationResult NotifySubmittingFuelReportDetail(FuelReportDetail source, IFuelReportDomainService fuelReportDomainService, long userId)
        {
            var result = new InventoryOperationResult();

            try
            {
                #region Old Correction Management Version

                //if (source.Correction.HasValue && source.CorrectionType.HasValue)
                //{
                //    if (source.CorrectionType.Value == CorrectionTypes.Plus)
                //    {
                //        if (source.CorrectionReference.IsEmpty())
                //        {
                //            if (source.IsCorrectionPriceEmpty())
                //            {
                //                var lastReceiveFuelReportDetailBefore = fuelReportDomainService.GetLastPurchasingReceiveFuelReportDetailBefore(source);

                //                if (lastReceiveFuelReportDetailBefore == null)
                //                    throw new BusinessRuleException("", "No valid receive Report found for " + source.Good.Code + ". You may price the entered incremental correction directly in Correction price field.");


                //                var lastReceiveInventoryOperationId = inventoryOperationManager.GetFueReportDetailReceiveOperationReference(lastReceiveFuelReportDetailBefore).OperationId;

                //                result.AddRange(this.inventoryOperationManager.ManageFuelReportDetailIncrementalCorrectionUsingPricingReference(source, lastReceiveInventoryOperationId,
                //                        "PricingByLastReceipt", (int)userId));
                //            }
                //            else
                //            {
                //                result.AddRange(this.inventoryOperationManager.ManageFuelReportDetailIncrementalCorrectionDirectPricing(source, (int)userId));
                //            }
                //        }
                //        else
                //        {
                //            if (source.CorrectionReference.ReferenceType.Value == ReferenceType.Voyage)
                //            {
                //                //var eovFuelReport = fuelReportDomainService.GetVoyageValidEndOfVoyageFuelReport(source.CorrectionReference.ReferenceId.Value);

                //                //var consumptionInventoryOperationId = eovFuelReport.ConsumptionInventoryOperations.Last().InventoryOperationId;

                //                var consumptionInventoryOperationId = fuelReportDomainService.GetVoyageConsumptionIssueOperation(source.CorrectionReference.ReferenceId.Value).InventoryOperationId;

                //                result.AddRange(this.inventoryOperationManager.ManageFuelReportDetailIncrementalCorrectionUsingPricingReference(source, consumptionInventoryOperationId, "PricingByIssuedVoyage", (int)userId));
                //            }
                //        }
                //    }
                //    else
                //    {
                //        result.AddRange(this.inventoryOperationManager.ManageFuelReportDetailDecrementalCorrection(source, (int)userId));
                //    }
                //}

                #endregion

                #region Manage Correction

                if (source.Correction.HasValue && source.CorrectionType.HasValue)
                {
                    if (source.CorrectionType.Value == CorrectionTypes.Plus)
                    {
                        var DECREMENTAL_CorrectionTransactionReference = this.inventoryOperationManager.GetFuelReportDetailDecrementalCorrectionReference(source);

                        if (DECREMENTAL_CorrectionTransactionReference != null)
                        {
                            //A decremental inventory operation for current FuelReport Detail is found, but as the correction type of current Detail is changed to incremental (Plus), so previous invnetory operation should be reverted.
                            var revertResult = this.inventoryOperationManager.RevertTransaction(DECREMENTAL_CorrectionTransactionReference, (int)userId);

                            result.Merge(revertResult);
                        }

                        var correctionTransactionReference = this.inventoryOperationManager.GetFuelReportDetailIncrementalCorrectionReference(source);

                        if (correctionTransactionReference == null)
                        {
                            var operations = this.inventoryOperationManager.ManageFuelReportDetailIncrementalCorrectionWithoutPricing(source, (int)userId);

                            result.CreatedInventoryOperations.AddRange(operations);
                        }
                        else
                        {
                            GoodQuantity goodQuantity;

                            this.createFuelReportDetailCorrectionFinalValues(source, (int)correctionTransactionReference.OperationId, out goodQuantity);

                            var goodsFinalQuantities = new Dictionary<long, List<GoodQuantity>>();
                            goodsFinalQuantities.Add(goodQuantity.InventoryGoodId, new List<GoodQuantity> { goodQuantity });

                            var operations = this.inventoryOperationManager.CorrectTransaction(correctionTransactionReference, null, goodsFinalQuantities, null, (int)userId);

                            result.CreatedInventoryOperations.AddRange(operations);
                        }
                    }
                    else
                    {
                        var INCREMENTAL_CorrectionTransactionReference = this.inventoryOperationManager.GetFuelReportDetailIncrementalCorrectionReference(source);

                        if (INCREMENTAL_CorrectionTransactionReference != null)
                        {
                            //An incremental inventory operation for current FuelReport Detail is found, but as the correction type of current Detail is changed to decremental (Minus), so previous invnetory operation should be reverted.
                            var revertResult = this.inventoryOperationManager.RevertTransaction(INCREMENTAL_CorrectionTransactionReference, (int)userId);

                            result.Merge(revertResult);
                        }

                        var correctionTransactionReference = this.inventoryOperationManager.GetFuelReportDetailDecrementalCorrectionReference(source);

                        if (correctionTransactionReference == null)
                        {
                            var operations = this.inventoryOperationManager.ManageFuelReportDetailDecrementalCorrectionWithoutPricing(source, (int)userId);

                            result.CreatedInventoryOperations.AddRange(operations);
                        }
                        else
                        {
                            GoodQuantity goodQuantity;

                            this.createFuelReportDetailCorrectionFinalValues(source, (int)correctionTransactionReference.OperationId, out goodQuantity);

                            var goodsFinalQuantities = new Dictionary<long, List<GoodQuantity>>();
                            goodsFinalQuantities.Add(goodQuantity.InventoryGoodId, new List<GoodQuantity> { goodQuantity });

                            var operations = this.inventoryOperationManager.CorrectTransaction(correctionTransactionReference, null, goodsFinalQuantities, null, (int)userId);

                            result.CreatedInventoryOperations.AddRange(operations);
                        }
                    }
                }
                else
                {
                    //Revert any available Correction Transaction.
                    result.Merge(this.RevertFuelRpeortDetailCorrectionInventoryOperations(source, (int)userId));
                }

                #endregion

                #region Manage Receive

                if (source.Receive.HasValue && source.ReceiveType.HasValue)
                {
                    var receiveTransactionReference = this.inventoryOperationManager.GetFuelReportDetailReceiveReference(source);

                    if (receiveTransactionReference == null)
                    {
                        var operations = this.inventoryOperationManager.ManageFuelReportDetailReceive(source, (int)userId);

                        result.CreatedInventoryOperations.AddRange(operations);
                    }
                    else
                    {
                        GoodQuantity goodQuantity;

                        this.createFuelReportDetailReceiveFinalValues(source, (int)receiveTransactionReference.OperationId, out goodQuantity);

                        var goodsFinalQuantities = new Dictionary<long, List<GoodQuantity>>();
                        goodsFinalQuantities.Add(goodQuantity.InventoryGoodId, new List<GoodQuantity> { goodQuantity });

                        var operations = this.inventoryOperationManager.CorrectTransaction(receiveTransactionReference, null, goodsFinalQuantities, null, (int)userId);

                        result.CreatedInventoryOperations.AddRange(operations);
                    }
                }
                else
                {
                    //Revert any available Receipt Transaction.
                    result.Merge(this.RevertFuelRpeortDetailReceiveInventoryOperations(source, (int)userId));
                }


                #endregion

                #region Manage Transfer

                if (source.Transfer.HasValue && source.TransferType.HasValue)
                {
                    var transferTransactionReference = this.inventoryOperationManager.GetFuelReportDetailTransferReference(source);

                    if (transferTransactionReference == null)
                    {
                        var operations = this.inventoryOperationManager.ManageFuelReportDetailTransfer(source,
                                            source.TransferType.Value == TransferTypes.Rejected ? source.TransferReference.ReferenceId : null,
                                            (int)userId);

                        result.CreatedInventoryOperations.AddRange(operations);
                    }
                    else
                    {
                        GoodQuantity goodQuantity;

                        this.createFuelReportDetailTransferFinalValues(source, (int)transferTransactionReference.OperationId, out goodQuantity);

                        var goodsFinalQuantities = new Dictionary<long, List<GoodQuantity>>();
                        goodsFinalQuantities.Add(goodQuantity.InventoryGoodId, new List<GoodQuantity> { goodQuantity });

                        var operations = this.inventoryOperationManager.CorrectTransaction(transferTransactionReference,
                            source.TransferType.Value == TransferTypes.Rejected ? source.TransferReference.ReferenceId : null,
                            goodsFinalQuantities, null, (int)userId);

                        result.CreatedInventoryOperations.AddRange(operations);
                    }
                }
                else
                {
                    //Revert any available Transfer Transaction.
                    result.Merge(this.RevertFuelRpeortDetailTransferInventoryOperations(source, (int)userId));
                }

                #endregion

                foreach (var operation in result.CreatedInventoryOperations)
                {
                    var operationTransaction = inventoryOperationManager.GetTransaction(operation.InventoryOperationId, operation.ActionType == InventoryActionType.Issue ? InventoryOperationType.Issue : InventoryOperationType.Receipt);

                    if (operationTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                    {
                        switch (operationTransaction.ReferenceType)
                        {
                            case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER:
                                {
                                    var transferIssueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(source.FuelReport.VesselInCompany.CompanyId, goodRepository);

                                    var orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();

                                    try
                                    {
                                        if (source.TransferType.Value == TransferTypes.TransferSale)
                                        {
                                            var saleTransferOrder = orderRepository.Single(o => o.Id == source.TransferReference.ReferenceId);

                                            this.addSaleTransitionIssueVoucher.Execute(transferIssueDataForFinance,
                                                                                       source.FuelReport,
                                                                                       operationTransaction.Inventory_Warehouse.Code,
                                                                                       saleTransferOrder.ToVesselInCompany.Code,
                                                                                       operation.ActionNumber, userId);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                break;


                            case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION:
                                {
                                    var issueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(source.FuelReport.VesselInCompany.CompanyId, goodRepository);

                                    try
                                    {
                                        if (source.CorrectionType.Value == CorrectionTypes.Minus)
                                        {
                                            this.addMinusCorrectionReceiptVoucher.Execute(source.FuelReport,
                                                                                          issueDataForFinance,
                                                                                          operationTransaction.Inventory_Warehouse.Code,
                                                                                          operation.ActionNumber, userId);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                break;

                            case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION:
                                {

                                    var incrementalCorrectionReceiptDataForFinance = operationTransaction.CreateReceiptDataForFinanceArticles(source.FuelReport.VesselInCompany.CompanyId, goodRepository);

                                    try
                                    {
                                        if (source.CorrectionType.Value == CorrectionTypes.Plus)
                                        {
                                            this.addPlusCorrectionReceiptVoucher.Execute(source.FuelReport,
                                                                                         incrementalCorrectionReceiptDataForFinance,
                                                                                         operationTransaction.Inventory_Warehouse.Code,
                                                                                         operation.ActionNumber, userId);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }

            return result;
        }

        /// <summary>
        /// Pricing FuelReport detail based on Finance user choices.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fuelReportDomainService"></param>
        /// <param name="userId"></param>
        public void NotifySubmittingFuelReportDetailByFinance(FuelReportDetail source, IFuelReportDomainService fuelReportDomainService, long userId)
        {
            try
            {
                #region Manage Correction

                if (source.Correction.HasValue && source.CorrectionType.HasValue && source.IsCorrectionPricingTypeRevised)
                {
                    if (source.CorrectionType.Value == CorrectionTypes.Plus)  //Incremental Correction Pricing
                    {
                        var transactionId = -1L;
                        if (source.CorrectionReference.IsEmpty()) //آخرین خرید قیمت دار    OR    قیمت دهی مستقیم
                        {
                            if (source.IsCorrectionPriceEmpty())
                            {
                                //Price based on "آخرین خرید قیمت دار"
                                var lastReceiveFuelReportDetailBefore = fuelReportDomainService.GetLastPurchasingReceiveFuelReportDetailBefore(source);

                                if (lastReceiveFuelReportDetailBefore == null)
                                    throw new BusinessRuleException("", "No valid receive Report found for " + source.Good.Code + ". You may price the entered incremental correction using direct pricing.");

                                var lastReceiveInventoryOperationId = inventoryOperationManager.GetFuelReportDetailReceiveReference(lastReceiveFuelReportDetailBefore).OperationId;

                                transactionId = this.inventoryOperationManager.PriceFuelReportDetailIncrementalCorrectionUsingPricingReference(source, lastReceiveInventoryOperationId, "PricingByLastReceipt", (int)userId);
                            }
                            else
                            {
                                //Price based on "قیمت دهی مستقیم"
                                transactionId = this.inventoryOperationManager.PriceFuelReportDetailIncrementalCorrectionDirectPricing(source, (int)userId);
                            }
                        }
                        else
                        {
                            //Price based on "آخرین حواله مصرف"
                            if (source.CorrectionReference.ReferenceType.Value == MITD.Fuel.Domain.Model.Enums.ReferenceType.Voyage)
                            {
                                var consumptionInventoryOperationId = fuelReportDomainService.GetVoyageConsumptionIssueOperation(source.CorrectionReference.ReferenceId.Value).InventoryOperationId;

                                transactionId = this.inventoryOperationManager.PriceFuelReportDetailIncrementalCorrectionUsingPricingReference(source, consumptionInventoryOperationId, "PricingByIssuedVoyage", (int)userId);
                            }
                        }

                        var operationTransaction = this.inventoryOperationManager.GetTransaction(transactionId, InventoryOperationType.Receipt);

                        if (operationTransaction != null && operationTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                        {
                            var incrementalCorrectionReceiptDataForFinance = operationTransaction.CreateReceiptDataForFinanceArticles(source.FuelReport.VesselInCompany.CompanyId, goodRepository);

                            try
                            {
                                if (source.CorrectionType.Value == CorrectionTypes.Plus)
                                {
                                    this.addPlusCorrectionReceiptVoucher.Execute(source.FuelReport,
                                                                                 incrementalCorrectionReceiptDataForFinance,
                                                                                 operationTransaction.Inventory_Warehouse.Code,
                                                                                 operationTransaction.GetActionNumber(), userId);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    else  //Decremental Correction Pricing
                    {
                        var transactionId = -1L;
                        if (source.CorrectionReference.IsEmpty())  
                        {  
                            //قیمت دهی پیشفرض
                            transactionId = this.inventoryOperationManager.PriceFuelReportDetailDecrementalCorrectionDefaultPricing(source, (int)userId);
                        }
                        else if (source.CorrectionReference.ReferenceType.Value == MITD.Fuel.Domain.Model.Enums.ReferenceType.Voyage)
                        {
                            //Price based on "آخرین حواله مصرف"
                            var consumptionInventoryOperationId = fuelReportDomainService.GetVoyageConsumptionIssueOperation(source.CorrectionReference.ReferenceId.Value).InventoryOperationId;

                            transactionId = this.inventoryOperationManager.PriceFuelReportDetailDecrementalCorrectionUsingPricingReference(source, consumptionInventoryOperationId, "PricingByIssuedVoyage", (int)userId);
                        }

                        var operationTransaction = this.inventoryOperationManager.GetTransaction(transactionId, InventoryOperationType.Issue);

                        if (operationTransaction != null && operationTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                        {
                            var issueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(source.FuelReport.VesselInCompany.CompanyId, goodRepository);

                            try
                            {
                                if (source.CorrectionType.Value == CorrectionTypes.Minus)
                                {
                                    this.addMinusCorrectionReceiptVoucher.Execute(source.FuelReport,
                                                                                  issueDataForFinance,
                                                                                  operationTransaction.Inventory_Warehouse.Code,
                                                                                  operationTransaction.GetActionNumber(), userId);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                #endregion
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }


        //private void setConsumption(FuelReportDetailDto dto, FuelReportDetail source)
        //{
        //    if (!(source.FuelReport.FuelReportType == FuelReportTypes.EndOfMonth ||
        //        source.FuelReport.FuelReportType == FuelReportTypes.EndOfVoyage ||
        //        source.FuelReport.FuelReportType == FuelReportTypes.EndOfYear))
        //    {
        //        dto.Consumption = 0;
        //    }
        //    else
        //    {
        //        var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

        //        var reportingConsumption = fuelReportDomainService.CalculateReportingConsumption(source);

        //        dto.Consumption = (double?)reportingConsumption;
        //    }
        //}

        public List<InventoryOperation> NotifySubmittingScrap(Scrap source, long userId)
        {
            try
            {
                var result = this.inventoryOperationManager.ManageScrap(source, (int)userId);


                //var issueResult = result.Last();
                //var issueTransaction = inventoryOperationManager.GetTransaction(issueResult.InventoryOperationId, InventoryOperationType.Issue);

                //if (issueTransaction.Status.Value == (byte)TransactionState.FullPriced)
                //{
                //    var issueArticleToFinance = new List<Issue>();

                //    foreach (var transactionItem in issueTransaction.Inventory_TransactionItem)
                //    {
                //        issueArticleToFinance.AddRange(transactionItem.Inventory_TransactionItemPrice.Select(
                //                        tip =>
                //                            new Issue(0, transactionItem.GoodId, (int)tip.QuantityAmount.Value, tip.Fee.Value, tip.FeeInMainCurrency.Value / tip.Fee.Value, tip.Inventory_Unit_QuantityUnit.Name, transactionItem.Inventory_Good.Name, issueTransaction.RegistrationDate.Value, tip.PriceUnitId, tip.Inventory_Unit_PriceUnit.Name)).ToList());
                //    }

                //try
                //{
                //    addCharterInEndIssueVoucher.Execute(source, issueArticleToFinance,
                //                                         issueResult.ActionNumber, issueTransaction.Inventory_Warehouse.Code);
                //}
                //catch
                //{
                //}
                //}

                return result;
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }

        public InventoryOperation NotifySubmittingOrderItemBalance(OrderItemBalance orderItemBalance, long userId)
        {
            try
            {
                var pricingOperationReference = this.inventoryOperationManager.GetFuelReportDetailReceivePricingReference(orderItemBalance);

                if (pricingOperationReference == null)
                {
                    var result = this.inventoryOperationManager.ManageOrderItemBalance(orderItemBalance, (int)userId);

                    var receiptOperationReference = inventoryOperationManager.GetFuelReportDetailReceiveReference(orderItemBalance.FuelReportDetail);

                    var operationTransaction = inventoryOperationManager.GetTransaction(receiptOperationReference.OperationId, InventoryOperationType.Receipt);

                    if (operationTransaction.Status.Value == (byte)TransactionState.FullPriced)
                    {
                        var receiptDataForFinance = operationTransaction.CreateReceiptDataForFinanceArticles(orderItemBalance.FuelReportDetail.FuelReport.VesselInCompany.CompanyId, goodRepository);

                        try
                        {
                            var exchangeRate = currencyDomainService.GetCurrencyToMainCurrencyRate(orderItemBalance.InvoiceItem.Invoice.CurrencyId, orderItemBalance.InvoiceItem.Invoice.InvoiceDate);

                            //var inventoryActionNumber = operationTransaction.GetActionNumber(orderItemBalance.FuelReportDetail.FuelReport.VesselInCompany.Code);
                            var inventoryActionNumber = operationTransaction.GetActionNumber();

                            if (orderItemBalance.InvoiceItem.Invoice.InvoiceType == InvoiceTypes.Purchase)
                                this.addPurchaseInvoiceVoucher.Execute(orderItemBalance.InvoiceItem.Invoice, receiptDataForFinance, operationTransaction.Inventory_Warehouse.Code, exchangeRate, orderItemBalance.FuelReportDetail.FuelReport, inventoryActionNumber, userId);
                            else if (orderItemBalance.InvoiceItem.Invoice.InvoiceType == InvoiceTypes.PurchaseOperations)
                                this.addTransferBarjingInvoiceVoucher.Execute(orderItemBalance.InvoiceItem.Invoice, receiptDataForFinance, operationTransaction.Inventory_Warehouse.Code, exchangeRate, orderItemBalance.FuelReportDetail.FuelReport, inventoryActionNumber, userId);

                            if (orderItemBalance.PairingInvoiceItem != null)
                            {
                                if (orderItemBalance.PairingInvoiceItem.Invoice.InvoiceType == InvoiceTypes.Purchase)
                                    this.addPurchaseInvoiceVoucher.Execute(orderItemBalance.PairingInvoiceItem.Invoice, receiptDataForFinance, operationTransaction.Inventory_Warehouse.Code, exchangeRate, orderItemBalance.FuelReportDetail.FuelReport, inventoryActionNumber, userId);
                                else if (orderItemBalance.PairingInvoiceItem.Invoice.InvoiceType == InvoiceTypes.PurchaseOperations)
                                    this.addTransferBarjingInvoiceVoucher.Execute(orderItemBalance.PairingInvoiceItem.Invoice, receiptDataForFinance, operationTransaction.Inventory_Warehouse.Code, exchangeRate, orderItemBalance.FuelReportDetail.FuelReport, inventoryActionNumber, userId);
                            }

                            //TODO: For all attachments of main and paired invoices, the relevant vouchers should be created.
                        }
                        catch (BusinessRuleException businessRuleException)
                        {
                            throw;
                        }
                        catch { }
                    }

                    return result;
                }
                else
                {
                    var inventoryOperationReference = this.inventoryOperationManager.GetFuelReportDetailReceiveReference(orderItemBalance.FuelReportDetail);

                    GoodQuantity goodQuantity;
                    GoodQuantityPricing goodQuantityPricing;

                    this.createEntityFinalValues(orderItemBalance, (int)inventoryOperationReference.OperationId, out goodQuantity, out goodQuantityPricing);

                    this.inventoryOperationManager.CorrectReceiptTransactionPricing((int)pricingOperationReference.OperationId, goodQuantityPricing, (int)userId);

                    pricingOperationReference = this.inventoryOperationManager.GetFuelReportDetailReceivePricingReference(orderItemBalance);

                    var result = new InventoryOperation(
                        pricingOperationReference.OperationId,
                           actionNumber: string.Format("{0}/{1}/Invoice|{2}", (InventoryOperationType)pricingOperationReference.OperationType, pricingOperationReference.OperationId, orderItemBalance.InvoiceItem.Invoice.InvoiceNumber),
                           actionDate: pricingOperationReference.RegistrationDate,
                           actionType: InventoryActionType.Pricing);

                    //TODO: For all attachments of main and paired invoices, the relevant vouchers should be created.

                    return result;
                }
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }

        public void RevertOrderItemBalancePricing(OrderItemBalance orderItemBalance, long userId)
        {
            try
            {
                var pricingOperationReference = this.inventoryOperationManager.GetFuelReportDetailReceivePricingReference(orderItemBalance);

                if (pricingOperationReference != null)
                {
                    this.inventoryOperationManager.RevertTransactionPricing((int)pricingOperationReference.OperationId, (int)userId);
                }
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }

        public void RevertFuelReportDetailCorrectionPricing(FuelReportDetail fuelReportDetail, long userId)
        {
            try
            {
                if (fuelReportDetail.Correction.HasValue && fuelReportDetail.CorrectionType.HasValue)
                {
                    var pricingOperationReference = this.inventoryOperationManager.GetFuelReportDetailCorrectionPricingReference(fuelReportDetail);

                    if (pricingOperationReference != null)
                    {
                        this.inventoryOperationManager.RevertTransactionPricing((int)pricingOperationReference.OperationId, (int)userId);
                    }
                }
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }

        public InventoryOperationResult NotifyCharterOutEndResubmit(Voyage voyage, CharterOut charterOutEnd, long userId, bool vesselShouldBeDeactivated)
        {
            if (charterOutEnd.CharterType != CharterType.End)
                throw new InvalidArgument("Given Charter is not a Charter Out End object.", "charterOutEnd");

            inventoryOperationManager.ActivateWarehouse(charterOutEnd.VesselInCompany.Code, charterOutEnd.VesselInCompany.CompanyId, charterOutEnd.ActionDate.AddSeconds(-1), (int)userId);

            //var charterOutEndOperationReferences = inventoryOperationManager.GetCharterOutEndReference(charterOutEnd);

            //Dictionary<long, List<GoodQuantity>> finalGoodQuantity;
            //Dictionary<long, List<GoodQuantityPricing>> finalGoodQuantityPricing;

            //createEntityFinalValues(charterOutEnd, (int)charterOutEndOperationReferences.OperationId, out finalGoodQuantity, out finalGoodQuantityPricing);

            //var correctionResult = inventoryOperationManager.CorrectTransaction(charterOutEndOperationReferences, null, finalGoodQuantity, null, (int)userId);
            //inventoryOperationManager.CorrectReceiptTransactionPricing(charterOutEndOperationReferences, finalGoodQuantity, finalGoodQuantityPricing, (int)userId);

            var result = this.NotifySubmittingCharterOutEnd(voyage, charterOutEnd, userId);

            if (vesselShouldBeDeactivated)
                inventoryOperationManager.DeactivateWarehouse(charterOutEnd.VesselInCompany.Code, charterOutEnd.VesselInCompany.CompanyId, charterOutEnd.ActionDate, (int)userId);

            //return new InventoryOperationResult { CreatedInventoryOperations = correctionResult };
            return result;
        }

        //public List<InventoryOperation> NotifySubmittingRevertPriceCharterInStart(
        //                                        CharterIn charterInStart,
        //                                        Voyage voyage,
        //                                        DateTime? charterInEndDate,
        //                                        DateTime? nextCharterOutStart,
        //                                        long userId,
        //                                        IGoodRepository goodRepository,
        //                                        decimal diffQuantity,
        //                                        CharterItem charterItem)
        //{
        //    try
        //    {
        //        var iUpdateFactory = new UpdatePriceSubmitedReciptFactory(charterInStart, charterItem);

        //        var result = this.inventoryOperationManager.
        //            UpdatePriceSubmitedReciptFlow(
        //            //UpdateCountSubmitedReciptFlow( 
        //            iUpdateFactory,
        //            goodRepository,
        //            voyage,
        //            userId,
        //            diffQuantity);


        //        return result;
        //    }
        //    catch (InvalidOperation) { throw; }
        //    catch (InvalidArgument) { throw; }
        //    catch (ObjectNotFound) { throw; }
        //    catch (BusinessRuleException) { throw; }
        //    catch (Exception ex)
        //    {
        //        throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
        //    }
        //}

        public InventoryOperationResult RevertFuelReportConsumptionInventoryOperations(FuelReport fuelReport, int userId)
        {
            var revertResult = new InventoryOperationResult();

            var operationReference = inventoryOperationManager.GetFuelReportConsumptionReference(fuelReport);

            if (operationReference != null)
            {
                revertResult = inventoryOperationManager.RevertTransaction(operationReference, userId);
            }

            return revertResult;
        }

        public InventoryOperationResult RevertCharterInStartInventoryOperations(CharterIn charterInStart, int userId)
        {
            if (charterInStart.CharterType != CharterType.Start)
                throw new InvalidArgument("Given Charter is not a Charter In Start object.", "charterInStart");

            inventoryOperationManager.ActivateWarehouse(charterInStart.VesselInCompany.Code, charterInStart.VesselInCompany.CompanyId, charterInStart.ActionDate, userId);

            var charterInStartOperationReferences = inventoryOperationManager.GetCharterInStartReference(charterInStart);

            var revertResult = inventoryOperationManager.RevertTransaction(charterInStartOperationReferences, userId);

            return revertResult;
        }

        public InventoryOperationResult RevertCharterInEndInventoryOperations(CharterIn charterInEnd, int userId)
        {
            if (charterInEnd.CharterType != CharterType.End)
                throw new InvalidArgument("Given Charter is not a Charter In End object.", "charterInEnd");

            var procedureResult = new InventoryOperationResult();

            inventoryOperationManager.ActivateWarehouse(charterInEnd.VesselInCompany.Code, charterInEnd.VesselInCompany.CompanyId, charterInEnd.ActionDate, userId);

            var charterInEndOperationReferences = inventoryOperationManager.GetCharterInEndReference(charterInEnd);

            for (int index = charterInEndOperationReferences.Count - 1; index >= 0; index--)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(charterInEndOperationReferences[index], userId);

                procedureResult.Merge(revertResult);
            }

            var incrementalCorrectionOperationReference = inventoryOperationManager.GetCharterInEndIncrementalCorrectionReference(charterInEnd);

            for (int index = incrementalCorrectionOperationReference.Count - 1; index >= 0; index--)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(incrementalCorrectionOperationReference[index], userId);

                procedureResult.Merge(revertResult);
            }

            var decrementalCorrectionOperationReference = inventoryOperationManager.GetCharterInEndDecrementalCorrectionReference(charterInEnd);

            for (int index = decrementalCorrectionOperationReference.Count - 1; index >= 0; index--)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(decrementalCorrectionOperationReference[index], userId);

                procedureResult.Merge(revertResult);
            }

            var issueTrustReceiptsOperationReference = inventoryOperationManager.GetCharterInEndIssueTrustReceiptsReference(charterInEnd);

            if (issueTrustReceiptsOperationReference != null)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(issueTrustReceiptsOperationReference, userId);

                procedureResult.Merge(revertResult);
            }

            return procedureResult;
        }

        public InventoryOperationResult RevertCharterOutStartInventoryOperations(CharterOut charterOutStart, int userId)
        {
            if (charterOutStart.CharterType != CharterType.Start)
                throw new InvalidArgument("Given Charter is not a Charter Out Start object.", "charterOutStart");

            var procedureResult = new InventoryOperationResult();

            inventoryOperationManager.ActivateWarehouse(charterOutStart.VesselInCompany.Code, charterOutStart.VesselInCompany.CompanyId, charterOutStart.ActionDate, userId);

            var charterOutStartOperationReferences = inventoryOperationManager.GetCharterOutStartReference(charterOutStart);

            for (int index = charterOutStartOperationReferences.Count - 1; index >= 0; index--)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(charterOutStartOperationReferences[index], userId);

                procedureResult.Merge(revertResult);
            }

            var incrementalCorrectionOperationReference = inventoryOperationManager.GetCharterOutStartIncrementalCorrectionReference(charterOutStart);

            for (int index = incrementalCorrectionOperationReference.Count - 1; index >= 0; index--)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(incrementalCorrectionOperationReference[index], userId);

                procedureResult.Merge(revertResult);
            }

            var decrementalCorrectionOperationReference = inventoryOperationManager.GetCharterOutStartDecrementalCorrectionReference(charterOutStart);

            for (int index = decrementalCorrectionOperationReference.Count - 1; index >= 0; index--)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(decrementalCorrectionOperationReference[index], userId);

                procedureResult.Merge(revertResult);
            }

            var issueTrustReceiptsOperationReference = inventoryOperationManager.GetCharterOutStartIssueTrustReceiptsReference(charterOutStart);

            if (issueTrustReceiptsOperationReference != null)
            {
                var revertResult = inventoryOperationManager.RevertTransaction(issueTrustReceiptsOperationReference, userId);

                procedureResult.Merge(revertResult);

            }

            return procedureResult;
        }

        public InventoryOperationResult RevertCharterOutEndInventoryOperations(CharterOut charterOutEnd, int userId)
        {
            if (charterOutEnd.CharterType != CharterType.End)
                throw new InvalidArgument("Given Charter is not a Charter Out End object.", "charterOutEnd");

            inventoryOperationManager.ActivateWarehouse(charterOutEnd.VesselInCompany.Code, charterOutEnd.VesselInCompany.CompanyId, charterOutEnd.ActionDate, userId);

            var charterOutEndOperationReferences = inventoryOperationManager.GetCharterOutEndReference(charterOutEnd);

            var revertResult = inventoryOperationManager.RevertTransaction(charterOutEndOperationReferences, userId);

            return revertResult;
        }


        public InventoryOperationResult RevertFuelRpeortDetailReceiveInventoryOperations(FuelReportDetail fuelReportDetail, int userId)
        {
            var revertResult = new InventoryOperationResult();

            var receiveTransactionReference = this.inventoryOperationManager.GetFuelReportDetailReceiveReference(fuelReportDetail);

            if (receiveTransactionReference != null)
            {
                revertResult = this.inventoryOperationManager.RevertTransaction(receiveTransactionReference, (int)userId);
            }

            return revertResult;
        }

        public InventoryOperationResult RevertFuelRpeortDetailTransferInventoryOperations(FuelReportDetail fuelReportDetail, int userId)
        {
            var revertResult = new InventoryOperationResult();

            var transferTransactionReference = inventoryOperationManager.GetFuelReportDetailTransferReference(fuelReportDetail);

            if (transferTransactionReference != null)
            {
                revertResult = this.inventoryOperationManager.RevertTransaction(transferTransactionReference, (int)userId);
            }

            return revertResult;
        }

        public InventoryOperationResult RevertFuelRpeortDetailCorrectionInventoryOperations(FuelReportDetail fuelReportDetail, int userId)
        {
            //Any type of correction should be reverted

            var revertResult = new InventoryOperationResult();

            var decrementalCorrectionTransactionReference = this.inventoryOperationManager.GetFuelReportDetailDecrementalCorrectionReference(fuelReportDetail);

            if (decrementalCorrectionTransactionReference != null)
            {
                revertResult.Merge(this.inventoryOperationManager.RevertTransaction(decrementalCorrectionTransactionReference, (int)userId));
            }

            var incrementalCorrectionTransactionReference = this.inventoryOperationManager.GetFuelReportDetailIncrementalCorrectionReference(fuelReportDetail);

            if (incrementalCorrectionTransactionReference != null)
            {
                revertResult.Merge(this.inventoryOperationManager.RevertTransaction(incrementalCorrectionTransactionReference, (int)userId));
            }

            return revertResult;
        }

        public InventoryOperationResult NotifySubmittingCharterInStart(Voyage voyage, CharterIn charterInStart, long userId)
        {
            try
            {
                var result = new InventoryOperationResult();

                var operations = this.inventoryOperationManager.ManageCharterInStart(charterInStart, (int)userId);

                result.CreatedInventoryOperations.AddRange(operations);

                foreach (var operation in operations)
                {
                    if (!(operation.ActionType == InventoryActionType.Issue ||
                        operation.ActionType == InventoryActionType.Receipt))
                        continue;

                    var operationTransaction = inventoryOperationManager.GetTransaction(operation.InventoryOperationId, operation.ActionType == InventoryActionType.Issue ? InventoryOperationType.Issue : InventoryOperationType.Receipt);

                    if (operationTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                    {
                        var receiptDataForFinance = operationTransaction.CreateReceiptDataForFinanceArticles(charterInStart.ChartererId.Value, goodRepository);

                        try
                        {
                            this.addCharterInStartReceiptVoucher.Execute(charterInStart, receiptDataForFinance,
                                                                         operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                        }
                        catch
                        {
                        }
                    }
                }

                return result;
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }

        public InventoryOperationResult NotifyCharterInStartResubmit(Voyage voyage, CharterIn charterInStart, long userId, bool vesselShouldBeDeactivated)
        {
            if (charterInStart.CharterType != CharterType.Start)
                throw new InvalidArgument("Given Charter is not a Charter In Start object.", "charterInStart");

            inventoryOperationManager.ActivateWarehouse(charterInStart.VesselInCompany.Code, charterInStart.VesselInCompany.CompanyId, charterInStart.ActionDate, (int)userId);

            //var charterInStartOperationReferences = inventoryOperationManager.GetCharterInStartReference(charterInStart);

            //Dictionary<long, List<GoodQuantity>> finalGoodQuantity;
            //Dictionary<long, List<GoodQuantityPricing>> finalGoodQuantityPricing;

            //createEntityFinalValues(charterInStart, (int)charterInStartOperationReferences.OperationId, out finalGoodQuantity, out finalGoodQuantityPricing);

            //var correctionResult = inventoryOperationManager.CorrectTransaction(charterInStartOperationReferences, null, finalGoodQuantity, null, (int)userId);
            //inventoryOperationManager.CorrectReceiptTransactionPricing(charterInStartOperationReferences, finalGoodQuantity, finalGoodQuantityPricing, (int)userId);

            var result = this.NotifySubmittingCharterInStart(voyage, charterInStart, userId);

            if (vesselShouldBeDeactivated)
                inventoryOperationManager.DeactivateWarehouse(charterInStart.VesselInCompany.Code, charterInStart.VesselInCompany.CompanyId, charterInStart.ActionDate, (int)userId);

            //return new InventoryOperationResult { CreatedInventoryOperations = correctionResult };
            return result;
        }

        public InventoryOperationResult NotifySubmittingCharterInEnd(Voyage voyage, CharterIn charterInEnd, long userId)
        {
            return performCharterInEndOperation(voyage, charterInEnd, userId, true);
        }

        public InventoryOperationResult NotifyCharterInEndResubmit(Voyage voyage, CharterIn charterInEnd, long userId, bool vesselShouldBeDeactivated)
        {
            return performCharterInEndOperation(voyage, charterInEnd, userId, vesselShouldBeDeactivated);
        }

        private InventoryOperationResult performCharterInEndOperation(Voyage voyage, CharterIn charterInEnd, long userId, bool inventoryShouldBeDeactivated)
        {
            try
            {
                var result = new InventoryOperationResult();

                vesselInCompanyConfigurator.Configure(charterInEnd.VesselInCompany);

                //var revertResult = RevertCharterInEndInventoryOperations(charterInEnd, (int)userId);

                //result.Merge(revertResult);

                var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

                var goodsTotalTrustQuantity = fuelReportDomainService.GetGoodsTotalTrustReceiveData(charterInEnd);

                var issueTrustReceivesResult = this.inventoryOperationManager.ManageTrustReceivesIssue(charterInEnd, goodsTotalTrustQuantity, (int)userId);

                if (issueTrustReceivesResult != null)
                    result.CreatedInventoryOperations.Add(issueTrustReceivesResult);

                var operations = this.inventoryOperationManager.ManageCharterInEnd(charterInEnd, (int)userId, inventoryShouldBeDeactivated/*, lastIssuedVoyageInventoryOperationId*/);

                result.CreatedInventoryOperations.AddRange(operations);

                foreach (var operation in operations)
                {
                    if (!(operation.ActionType == InventoryActionType.Issue || operation.ActionType == InventoryActionType.Receipt))
                        continue;

                    var operationTransaction = inventoryOperationManager.GetTransaction(operation.InventoryOperationId, operation.ActionType == InventoryActionType.Issue ? InventoryOperationType.Issue : InventoryOperationType.Receipt);

                    if (operationTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                    {
                        if (operationTransaction.ReferenceType == InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE)
                        {
                            var issueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(charterInEnd.ChartererId.Value, goodRepository);

                            try
                            {
                                this.addCharterInEndIssueVoucher.Execute(charterInEnd, issueDataForFinance,
                                                    operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }
                        }
                        else if (operationTransaction.ReferenceType == InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION)
                        {
                            var issueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(charterInEnd.ChartererId.Value, goodRepository);

                            try
                            {
                                this.addCharterInEndConsumptionIssueVoucher.Execute(charterInEnd, issueDataForFinance,
                                    operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }
                        }
                        else if (operationTransaction.ReferenceType == InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION)
                        {
                            var receiptDataForFinance = operationTransaction.CreateReceiptDataForFinanceArticles(charterInEnd.ChartererId.Value, goodRepository);

                            try
                            {
                                this.addCharterInEndBackReceiptVoucher.Execute(charterInEnd, receiptDataForFinance,
                                                    operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }
                        }

                    }
                }

                return result;
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }

        public InventoryOperationResult NotifySubmittingCharterOutStart(Voyage voyage, CharterOut charterOutStart, long userId)
        {
            return performCharterOutStartOperations(voyage, charterOutStart, userId, true);
        }

        public InventoryOperationResult NotifyCharterOutStartResubmit(Voyage voyage, CharterOut charterOutStart, long userId, bool vesselShouldBeDeactivated)
        {
            return performCharterOutStartOperations(voyage, charterOutStart, userId, vesselShouldBeDeactivated);
        }

        private InventoryOperationResult performCharterOutStartOperations(Voyage voyage, CharterOut charterOutStart, long userId, bool inventoryShouldBeDeactivated)
        {
            try
            {
                var result = new InventoryOperationResult();

                vesselInCompanyConfigurator.Configure(charterOutStart.VesselInCompany);

                var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

                var goodsTotalTrustQuantity = fuelReportDomainService.GetGoodsTotalTrustReceiveData(charterOutStart);

                var issueTrustReceivesResult = this.inventoryOperationManager.ManageTrustReceivesIssue(charterOutStart, goodsTotalTrustQuantity, (int)userId);

                if (issueTrustReceivesResult != null)
                    result.CreatedInventoryOperations.Add(issueTrustReceivesResult);

                var operations = this.inventoryOperationManager.ManageCharterOutStart(charterOutStart, (int)userId, inventoryShouldBeDeactivated);

                result.CreatedInventoryOperations.AddRange(operations);

                foreach (var operation in operations)
                {
                    if (!(operation.ActionType == InventoryActionType.Issue ||
                        operation.ActionType == InventoryActionType.Receipt))
                        continue;

                    var operationTransaction = inventoryOperationManager.GetTransaction(operation.InventoryOperationId, operation.ActionType == InventoryActionType.Issue ? InventoryOperationType.Issue : InventoryOperationType.Receipt);

                    if (operationTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                    {
                        if (operationTransaction.ReferenceType == InventoryOperationReferenceTypes.CHARTER_OUT_START_ISSUE)
                        {
                            var issueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(charterOutStart.OwnerId.Value, goodRepository);

                            try
                            {
                                this.addCharterOutStartIssueVoucher.Execute(charterOutStart, issueDataForFinance,
                                                    operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }
                        }
                        else if (operationTransaction.ReferenceType == InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION)
                        {
                            var receiptDataForFinance = operationTransaction.CreateReceiptDataForFinanceArticles(charterOutStart.OwnerId.Value, goodRepository);

                            try
                            {
                                this.addCharterOutStartBackReceiptVoucher.Execute(charterOutStart, receiptDataForFinance,
                                                    operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }
                        }
                        else if (operationTransaction.ReferenceType == InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION)
                        {
                            var issueDataForFinance = operationTransaction.CreateIssueDataForFinanceArticles(charterOutStart.OwnerId.Value, goodRepository);

                            try
                            {
                                this.addCharterOutStartConsumptionIssueVoucher.Execute(charterOutStart, issueDataForFinance,
                                                    operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }
                        }

                    }
                }

                return result;
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }
        }

        public InventoryOperationResult NotifySubmittingCharterOutEnd(Voyage voyage, CharterOut charterOutEnd, long userId)
        {
            try
            {
                var result = new InventoryOperationResult();

                var operations = this.inventoryOperationManager.ManageCharterOutEnd(charterOutEnd, (int)userId);

                result.CreatedInventoryOperations.AddRange(operations);

                foreach (var operation in operations)
                {
                    if (!(operation.ActionType == InventoryActionType.Issue ||
                        operation.ActionType == InventoryActionType.Receipt))
                        continue;

                    var operationTransaction = inventoryOperationManager.GetTransaction(operation.InventoryOperationId, InventoryOperationType.Receipt);

                    if (operationTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                    {
                        //var issueArticleToFinance = new List<Receipt>();

                        //foreach (var transactionItem in operationTransaction.Inventory_TransactionItem)
                        //{
                        //    var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == charterOutEnd.OwnerId);

                        //    issueArticleToFinance.AddRange(transactionItem.Inventory_TransactionItemPrice.Select(
                        //                                                                                         tip =>
                        //                                                                                             new Receipt(0, good.Id, tip.QuantityAmount.Value, tip.Fee.Value, tip.FeeInMainCurrency.Value / tip.Fee.Value, tip.Inventory_Unit_QuantityUnit.Name, transactionItem.Inventory_Good.Name, operationTransaction.RegistrationDate.Value, tip.PriceUnitId, tip.Inventory_Unit_PriceUnit.Name)).ToList());
                        //}

                        var receiptDataForFinance = operationTransaction.CreateReceiptDataForFinanceArticles(charterOutEnd.OwnerId.Value, goodRepository);


                        try
                        {
                            addCharterOutEndReceiptVoucher.Execute(charterOutEnd, receiptDataForFinance,
                                                                   operationTransaction.Inventory_Warehouse.Code, operation.ActionNumber, userId, voyage.VoyageNumber, "");
                        }
                        catch
                        {
                        }
                    }
                }

                return result;
            }
            catch (InvalidOperation) { throw; }
            catch (InvalidArgument) { throw; }
            catch (ObjectNotFound) { throw; }
            catch (BusinessRuleException) { throw; }
            catch (Exception ex)
            {
                throw new FuelException(ex.Message, this.extractUsefulInnerException(ex));
            }

        }

        private Dictionary<long, List<GoodQuantity>> createEntityFinalValues(FuelReport fuelReport, int inventoryOperationId, Dictionary<long, decimal> goodsConsumption)
        {
            var result = new Dictionary<long, List<GoodQuantity>>();

            foreach (var fuelReportDetail in fuelReport.FuelReportDetails)
            {
                result.Add(fuelReportDetail.Good.SharedGoodId, new List<GoodQuantity>
				{ 
					new GoodQuantity
						{
							InventoryGoodId = fuelReportDetail.Good.SharedGoodId,
							SignedQuantity = goodsConsumption[fuelReportDetail.GoodId] * -1,
							//QuantityAbbreviation = fuelReportDetail.MeasuringUnit.Abbreviation,
							InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(fuelReportDetail.MeasuringUnit.Abbreviation),
							TransactionId = inventoryOperationId
						}
					}
                );
            }

            return result;
        }

        private Dictionary<long, decimal> calculateGoodsConsumption(FuelReport fuelReport)
        {
            var goodsConsumption = new Dictionary<long, decimal>();

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

            foreach (var detail in fuelReport.FuelReportDetails)
            {
                var consumption = fuelReportDomainService.CalculateReportingConsumption(detail);

                goodsConsumption.Add(detail.GoodId, consumption);
            }

            return goodsConsumption;
        }

        private void createEntityFinalValues(Charter charter, int inventoryOperationId, out Dictionary<long, List<GoodQuantity>> finalGoodQuantity, out Dictionary<long, List<GoodQuantityPricing>> finalGoodQuantityPricing)
        {
            finalGoodQuantity = new Dictionary<long, List<GoodQuantity>>();
            finalGoodQuantityPricing = new Dictionary<long, List<GoodQuantityPricing>>();

            var mainCurrency = currencyDomainService.GetMainCurrency();

            foreach (var charterItem in charter.CharterItems)
            {
                finalGoodQuantity.Add(charterItem.Good.SharedGoodId, new List<GoodQuantity>
				{ 
					new GoodQuantity
						{
							InventoryGoodId = charterItem.Good.SharedGoodId,
							SignedQuantity = charterItem.Rob * getCharterQuantitySign(charter),
							InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(charterItem.GoodUnit.Abbreviation),
							TransactionId = inventoryOperationId,
						}
					}
                );

                finalGoodQuantityPricing.Add(charterItem.Good.SharedGoodId, new List<GoodQuantityPricing>
				{ 
					new GoodQuantityPricing
					{
						InventoryGoodId = charterItem.Good.SharedGoodId,
						SignedQuantity = charterItem.Rob * getCharterQuantitySign(charter),
						InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(charterItem.GoodUnit.Abbreviation),
						Fee = charterItem.Fee,
						FeeInventoryCurrencyUnitId = inventoryOperationManager.GetCurrencyId(charter.Currency.Abbreviation),
						FeeInMainCurrency = currencyDomainService.ConvertPrice(charterItem.Fee, charter.Currency, mainCurrency, charter.ActionDate),
						TransactionId = inventoryOperationId,
						Description = string.Format("Charter {0} {1} Pricing > {2}", (charter is CharterIn) ? "In" : "Out", charter.CharterType.ToString(), charterItem.Good.Code),
					}
				});
            }


        }

        private int getCharterQuantitySign(Charter charter)
        {
            if (charter is CharterIn)
            {
                if (charter.CharterType == CharterType.Start) return +1;  //Charter-In-Start is equal to Receipt in Inventory
                else return -1;  //Charter-In-End is equal to Issue in Inventory
            }
            else
            {
                if (charter.CharterType == CharterType.Start) return -1;  //Charter-Out-Start is equal to Issue in Inventory
                else return +1;  //Charter-Out-End is equal to Receipt in Inventory
            }
        }

        private void createEntityFinalValues(OrderItemBalance orderItemBalance, int inventoryOperationId, out GoodQuantity finalGoodQuantity, out GoodQuantityPricing finalGoodQuantityPricing)
        {

            var mainCurrency = currencyDomainService.GetMainCurrency();

            finalGoodQuantity = new GoodQuantity
                {
                    InventoryGoodId = orderItemBalance.InvoiceItem.Good.SharedGoodId,
                    SignedQuantity = orderItemBalance.QuantityAmountInMainUnit,
                    InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(orderItemBalance.UnitCode),
                    TransactionId = inventoryOperationId,
                };

            var priceInMainCurrency = getOrderItemBalancePriceInMainCurrency(orderItemBalance);

            finalGoodQuantityPricing = new GoodQuantityPricing
                {
                    InventoryGoodId = orderItemBalance.InvoiceItem.Good.SharedGoodId,
                    SignedQuantity = orderItemBalance.QuantityAmountInMainUnit,
                    InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(orderItemBalance.InvoiceItem.MeasuringUnit.MainGoodUnit.Abbreviation),
                    Fee = priceInMainCurrency / orderItemBalance.QuantityAmountInMainUnit,
                    FeeInventoryCurrencyUnitId = inventoryOperationManager.GetCurrencyId(mainCurrency.Abbreviation),
                    FeeInMainCurrency = priceInMainCurrency / orderItemBalance.QuantityAmountInMainUnit,
                    TransactionId = inventoryOperationId,
                    Description = string.Format("FuelReport Receipt Pricing > {0}", orderItemBalance.FuelReportDetail.Good.Code),
                    PricingReferenceNumber = orderItemBalance.GetOrderItemBalancePricingReferenceNumber(),
                    PricingReferenceType = InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE
                };
        }

        private decimal getOrderItemBalancePriceInMainCurrency(OrderItemBalance orderItemBalance)
        {
            var mainCurrency = currencyDomainService.GetMainCurrency();

            var goodId = orderItemBalance.InvoiceItem.GoodId;

            //Calculating main invoice price
            decimal price = currencyDomainService.ConvertPrice(orderItemBalance.InvoiceItem.Price, orderItemBalance.InvoiceItem.Invoice.Currency, mainCurrency, orderItemBalance.InvoiceItem.Invoice.InvoiceDate);

            //Calculating main invoice attachments price
            price += orderItemBalance.InvoiceItem.Invoice.Attachments.Where(a => a.State == States.Submitted).Select(a =>
            {
                var sign = a.IsCreditor ? -1 : 1;

                var attachmentPrice = a.InvoiceItems.Where(item => item.GoodId == goodId).Sum(item => sign * currencyDomainService.ConvertPrice(item.Price, item.Invoice.Currency, mainCurrency, item.Invoice.InvoiceDate));

                return attachmentPrice;
            }).Sum();

            if (orderItemBalance.PairingInvoiceItem != null)
            {
                //Calculating paired invoices price
                price += currencyDomainService.ConvertPrice(orderItemBalance.PairingInvoiceItem.Price, orderItemBalance.PairingInvoiceItem.Invoice.Currency, mainCurrency, orderItemBalance.PairingInvoiceItem.Invoice.InvoiceDate);

                //Calculating paired invoices attachments price
                price += orderItemBalance.PairingInvoiceItem.Invoice.Attachments.Where(a => a.State == States.Submitted).Select(a =>
                {
                    var sign = a.IsCreditor ? -1 : 1;

                    var attachmentPrice = a.InvoiceItems.Where(item => item.GoodId == goodId).Sum(item => sign * currencyDomainService.ConvertPrice(item.Price, item.Invoice.Currency, mainCurrency, item.Invoice.InvoiceDate));

                    return attachmentPrice;
                }).Sum();
            }

            return price;
        }


        private void createFuelReportDetailCorrectionFinalValues(FuelReportDetail fuelReportDetail, int inventoryOperationId, out GoodQuantity finalGoodQuantity)
        {
            if (!fuelReportDetail.Correction.HasValue || !fuelReportDetail.CorrectionType.HasValue)
            {
                finalGoodQuantity = new GoodQuantity();
                return;
            }

            finalGoodQuantity = new GoodQuantity
            {
                InventoryGoodId = fuelReportDetail.Good.SharedGoodId,
                SignedQuantity = fuelReportDetail.Correction.Value * (fuelReportDetail.CorrectionType.Value == CorrectionTypes.Plus ? +1 : -1),
                InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(fuelReportDetail.MeasuringUnit.Abbreviation),
                TransactionId = inventoryOperationId,
            };
        }

        private void createFuelReportDetailReceiveFinalValues(FuelReportDetail fuelReportDetail, int inventoryOperationId, out GoodQuantity finalGoodQuantity)
        {
            if (!fuelReportDetail.Receive.HasValue)
            {
                finalGoodQuantity = new GoodQuantity();
                return;
            }

            finalGoodQuantity = new GoodQuantity
            {
                InventoryGoodId = fuelReportDetail.Good.SharedGoodId,
                SignedQuantity = fuelReportDetail.Receive.Value,
                InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(fuelReportDetail.MeasuringUnit.Abbreviation),
                TransactionId = inventoryOperationId,
            };
        }

        private void createFuelReportDetailTransferFinalValues(FuelReportDetail fuelReportDetail, int inventoryOperationId, out GoodQuantity finalGoodQuantity)
        {
            if (!fuelReportDetail.Transfer.HasValue)
            {
                finalGoodQuantity = new GoodQuantity();
                return;
            }

            finalGoodQuantity = new GoodQuantity
            {
                InventoryGoodId = fuelReportDetail.Good.SharedGoodId,
                SignedQuantity = fuelReportDetail.Transfer.Value * -1,
                InventoryQuantityUnitId = inventoryOperationManager.GetMeasurementUnitId(fuelReportDetail.MeasuringUnit.Abbreviation),
                TransactionId = inventoryOperationId,
            };
        }



        #region Commented
        /*
				private List<Issue> createIssueDataForFinanceArticles(long companyId, Inventory_Transaction operationTransaction)
		{
			var articleIssueParameter = new List<Issue>();

			if (operationTransaction.Action != (byte)TransactionType.Issue)
				throw new InvalidArgument("Type of inventory operation to create Finance articles is invalid", "operationTransaction");


			foreach (var transactionItem in operationTransaction.Inventory_TransactionItem)
			{
				var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == companyId);

				//issueArticleToFinance.AddRange(
				//    transactionItem.Inventory_TransactionItemPrice.Select(
				//                tip =>
				//                    new Issue(0, good.Id, (int)tip.QuantityAmount.Value, tip.Fee.Value, tip.FeeInMainCurrency.Value / tip.Fee.Value, tip.Inventory_Unit_QuantityUnit.Name, transactionItem.Inventory_Good.Name, issueTransaction.RegistrationDate.Value, tip.PriceUnitId, tip.Inventory_Unit_PriceUnit.Name)).ToList());

				articleIssueParameter.AddRange(transactionItem.Inventory_TransactionItemPrice.GroupBy(
									tip => new
									{
										Code = tip.Inventory_TransactionItem.Inventory_Transaction.Code,
										RegistrationDate = tip.Inventory_TransactionItem.Inventory_Transaction.RegistrationDate,
										Action = tip.Inventory_TransactionItem.Inventory_Transaction.Action,
										QuantityUnitId = tip.QuantityUnitId,
										QuantityUnitAbbreviation = tip.Inventory_Unit_QuantityUnit.Abbreviation,
										QuantityUnitName = tip.Inventory_Unit_QuantityUnit.Name,
										PriceUnitId = tip.PriceUnitId,
										PriceUnitAbbreviation = tip.Inventory_Unit_PriceUnit.Abbreviation,
										PriceUnitName = tip.Inventory_Unit_PriceUnit.Name,
										MainCurrencyUnitId = tip.MainCurrencyUnitId,
										MainCurrencyAbbreviation = tip.Inventory_Unit_MainCurrencyUnit.Abbreviation,
										MainCurrencyName = tip.Inventory_Unit_MainCurrencyUnit.Name,
										//SUM(c.QuantityAmountPriced) AS QuantityAmountPriced, 
										Fee = tip.Fee,
										FeeInMainCurrency = tip.FeeInMainCurrency
										//SUM(c.TotalPrice) AS TotalPrice

									},
									(key, groupedItemPrices) =>
										new Issue(0, good.Id, groupedItemPrices.Sum(ti => ti.QuantityAmount.Value), key.Fee.Value, key.FeeInMainCurrency.Value / key.Fee.Value,
											key.QuantityUnitName,
											transactionItem.Inventory_Good.Name,
											operationTransaction.RegistrationDate.Value,
											key.PriceUnitId, key.PriceUnitName, transactionItem.Id)
									).ToList());

				//issueArticleToFinance.AddRange(transactionItem.Inventory_TransactionItemPrice.GroupBy(
				//   groupingKeySelector,
				//   (key, groupedItemPrices) =>
				//       new Issue(0, good.Id, groupedItemPrices.Sum(ti => ti.QuantityAmount.Value), key.Fee.Value, key.FeeInMainCurrency.Value / key.Fee.Value,
				//           key.QuantityUnitName,
				//           transactionItem.Inventory_Good.Name,
				//           issueTransaction.RegistrationDate.Value,
				//           key.PriceUnitId, key.PriceUnitName)
				//       ).ToList());
			}

			return articleIssueParameter;
		}

		private List<Receipt> createReceiptDataForFinanceArticles(long companyId, Inventory_Transaction operationTransaction)
		{
			var articleIssueParameter = new List<Receipt>();

			if (operationTransaction.Action != (byte)TransactionType.Receipt)
				throw new InvalidArgument("Type of inventory operation to create Finance articles is invalid", "operationTransaction");


			foreach (var transactionItem in operationTransaction.Inventory_TransactionItem)
			{
				var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == companyId);


				articleIssueParameter.AddRange(transactionItem.Inventory_TransactionItemPrice.GroupBy(
					tip => new
					{
						Code = tip.Inventory_TransactionItem.Inventory_Transaction.Code,
						RegistrationDate = tip.Inventory_TransactionItem.Inventory_Transaction.RegistrationDate,
						Action = tip.Inventory_TransactionItem.Inventory_Transaction.Action,
						QuantityUnitId = tip.QuantityUnitId,
						QuantityUnitAbbreviation = tip.Inventory_Unit_QuantityUnit.Abbreviation,
						QuantityUnitName = tip.Inventory_Unit_QuantityUnit.Name,
						PriceUnitId = tip.PriceUnitId,
						PriceUnitAbbreviation = tip.Inventory_Unit_PriceUnit.Abbreviation,
						PriceUnitName = tip.Inventory_Unit_PriceUnit.Name,
						MainCurrencyUnitId = tip.MainCurrencyUnitId,
						MainCurrencyAbbreviation = tip.Inventory_Unit_MainCurrencyUnit.Abbreviation,
						MainCurrencyName = tip.Inventory_Unit_MainCurrencyUnit.Name,
						//SUM(c.QuantityAmountPriced) AS QuantityAmountPriced, 
						Fee = tip.Fee,
						FeeInMainCurrency = tip.FeeInMainCurrency
						//SUM(c.TotalPrice) AS TotalPrice

					},
					(key, groupedItemPrices) =>
						new Receipt(0, good.Id, groupedItemPrices.Sum(ti => ti.QuantityAmount.Value), key.Fee.Value, key.FeeInMainCurrency.Value / key.Fee.Value,
							key.QuantityUnitName,
							transactionItem.Inventory_Good.Name,
							operationTransaction.RegistrationDate.Value,
							key.PriceUnitId, key.PriceUnitName, transactionItem.Id)
					).ToList());


			}

			return articleIssueParameter;
		}

		*/

        private Exception extractUsefulInnerException(Exception ex)
        {
            return ex.InnerException != null ? extractUsefulInnerException(ex.InnerException) : ex;
        }

        #endregion
    }
}
