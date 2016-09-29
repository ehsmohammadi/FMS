using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using System.Linq;
using MITD.Fuel.Integration.Inventory;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices
{
    public class InventoryManagementDomainService : IInventoryManagementDomainService
    {
        private readonly IGoodDomainService goodDomainService;
        private readonly ICurrencyDomainService currencyDomainService;
        private readonly IInventoryOperationManager inventoryOperationManager;
        public InventoryManagementDomainService(IGoodDomainService goodDomainService, ICurrencyDomainService currencyDomainService, IInventoryOperationManager inventoryOperationManager)
        {
            this.goodDomainService = goodDomainService;
            this.currencyDomainService = currencyDomainService;
            this.inventoryOperationManager = inventoryOperationManager;
        }

        public InventoryResult GetPricedIssueResult(long companyId, long operationId)
        {
            var issueTransaction = inventoryOperationManager.GetTransaction(operationId, InventoryOperationType.Issue);
            var mainCurrency = this.currencyDomainService.GetMainCurrency();
            return new InventoryResult()
                   {
                       Id = operationId,
                       Number = issueTransaction.Code.ToString(),
                       ActionType = InventoryActionType.Issue,
                       InventoryResultItems = issueTransaction.Inventory_TransactionItem.Select(
                                    ti => new InventoryResultItem
                                            {
                                                Id = ti.Id,
                                                Good = this.goodDomainService.FindGood(companyId, ti.GoodId),
                                                Currency = mainCurrency, //Base Currency;
                                                Fee = inventoryOperationManager.GetAverageFee(ti.TransactionId, TransactionType.Issue, ti.GoodId, mainCurrency.Id),
                                                Quantity = ti.QuantityAmount.Value,
                                                TransactionId = operationId
                                            }).ToList()
                   };
        }

        public InventoryResult GetVoyageConsumptionResult(long companyId, long endOfVoyageInventoryOperationId)
        {
            var issueTransaction = inventoryOperationManager.GetTransaction(endOfVoyageInventoryOperationId, InventoryOperationType.Issue);

            var previousConsumptionTransaction = inventoryOperationManager.GetLastTransactionBefore(endOfVoyageInventoryOperationId, InventoryOperationType.Issue, InventoryOperationReferenceTypes.EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION);

            var mainCurrency = this.currencyDomainService.GetMainCurrency();
            return new InventoryResult()
            {
                Id = endOfVoyageInventoryOperationId,
                Number = issueTransaction.Code.ToString(),
                ActionType = InventoryActionType.Issue,
                InventoryResultItems = issueTransaction.Inventory_TransactionItem.Select(
                             ti =>
                             {
                                 var currentConsumptionFee = inventoryOperationManager.GetAverageFee(ti.TransactionId, TransactionType.Issue, ti.GoodId, mainCurrency.Id);
                                 var totalQuantity = ti.QuantityAmount.GetValueOrDefault();

                                 if (previousConsumptionTransaction.StoreTypesId == 16) //EOY
                                 {
                                     var previousConsumptionQuantity = previousConsumptionTransaction.Inventory_TransactionItem.SingleOrDefault(pti => pti.GoodId == ti.GoodId).QuantityAmount.GetValueOrDefault();
                                     var previousConsumptionFee = inventoryOperationManager.GetAverageFee(previousConsumptionTransaction.Id, TransactionType.Issue, ti.GoodId, mainCurrency.Id);

                                     totalQuantity = ti.QuantityAmount.GetValueOrDefault() + previousConsumptionQuantity;
                                     currentConsumptionFee = ((currentConsumptionFee * ti.QuantityAmount.GetValueOrDefault()) + (previousConsumptionFee * previousConsumptionQuantity)) / (totalQuantity);
                                 }

                                 return new InventoryResultItem
                                        {
                                            Id = ti.Id,
                                            Good = this.goodDomainService.FindGood(companyId, ti.GoodId),
                                            Currency = mainCurrency, //Base Currency;
                                            Fee = currentConsumptionFee,
                                            TransactionId = endOfVoyageInventoryOperationId,
                                            Quantity = totalQuantity
                                        };
                             }).ToList()
            };
        }

        public bool CanIssuance(long vesselInCompanyId)
        {
            //TODO : Fake implementation


            return true;
        }

        public bool CanRecipt(long VesselInCompanyId)
        {
            //TODO : Fake implementation

            return true;
        }

        public List<Reference> GetFueReportDetailsReceiveOperationReference(List<FuelReportDetail> fuelReportDetails)
        {
            var receiveOperationReferences = inventoryOperationManager.GetFueReportDetailsReceiveOperationReference(fuelReportDetails);

            var inventoryOperations = fuelReportDetails.SelectMany(frd => frd.InventoryOperations)
                .Where(
                    invOp => receiveOperationReferences.Any(opRef => opRef.OperationId == invOp.InventoryOperationId))
                .ToList();

            var result = inventoryOperations.Select(inv => new Reference()
                                                         {
                                                             Code = inv.ActionNumber,
                                                             ReferenceId = inv.InventoryOperationId,
                                                             ReferenceType = ReferenceType.Inventory
                                                         }).ToList();

            return result;
        }

        public InventoryResult GetInventoryResult(long companyId, long operationId, InventoryActionType actionType, bool includePrices)
        {
            if (actionType != InventoryActionType.Issue && actionType != InventoryActionType.Receipt)
                throw new InvalidArgument("Invalid ActionType", "ActionType");

            var operationType = actionType == InventoryActionType.Issue ? InventoryOperationType.Issue : InventoryOperationType.Receipt;

            var issueTransaction = inventoryOperationManager.GetTransaction(operationId, operationType);

            return convertInventoryTransactionToInventoryResult(companyId, issueTransaction, includePrices);
        }

        private InventoryResult convertInventoryTransactionToInventoryResult(long companyId, Inventory_Transaction inventoryTransaction, bool includePrices)
        {
            var mainCurrency = this.currencyDomainService.GetMainCurrency();
            return new InventoryResult()
            {
                Id = inventoryTransaction.Id,
                Number = inventoryTransaction.Code.ToString(),
                ActionType = (InventoryOperationType)inventoryTransaction.Action == InventoryOperationType.Issue ? InventoryActionType.Issue : InventoryActionType.Receipt,
                InventoryResultItems = inventoryTransaction.Inventory_TransactionItem.Select(
                             ti => new InventoryResultItem
                             {
                                 Id = ti.Id,
                                 Good = this.goodDomainService.FindGood(companyId, ti.GoodId),
                                 Currency = mainCurrency, //Base Currency;
                                 Fee = includePrices ? inventoryOperationManager.GetAverageFee(ti.TransactionId, (TransactionType)ti.Inventory_Transaction.Action, ti.GoodId, mainCurrency.Id) : 0,
                                 Quantity = ti.QuantityAmount.Value,
                                 TransactionId = inventoryTransaction.Id
                             }).ToList()
            };
        }

        public InventoryResult GetLastNotOperatedIssuedTrustReceive(long companyId, string vesselCode, DateTime comparingDateTime)
        {
            var issueTransaction = inventoryOperationManager.GetLastIssuedTrustReceivesTransaction(vesselCode, comparingDateTime);

            if (issueTransaction == null)
                return null;

            var fuelReportDetailRepository = ServiceLocator.Current.GetInstance<IRepository<FuelReportDetail>>();

            var fuelReportDetailsWithAssignedTrustIssueInventoryTransactionItemAsReceipt = fuelReportDetailRepository.Find(frd => frd.TrustIssueInventoryTransactionItemId.HasValue && 
                frd.FuelReport.State != States.Open && frd.FuelReport.State != States.Cancelled).ToList();

            var transactionItemsAssignmentToFuelReportDetailsCount = issueTransaction.Inventory_TransactionItem.Join(fuelReportDetailsWithAssignedTrustIssueInventoryTransactionItemAsReceipt, ti=>ti.Id,frd=>frd.TrustIssueInventoryTransactionItemId,(ti,frd)=>ti.Id).Count();

            if (transactionItemsAssignmentToFuelReportDetailsCount != issueTransaction.Inventory_TransactionItem.Count)
                //This determines that there is already another chance to assign found transaction to fuel report.
                return convertInventoryTransactionToInventoryResult(companyId, issueTransaction, false);

            return null;
        }

        public bool IsThereAnyInventoryOperationWithNotEmptyQuantity(List<InventoryOperation> inventoryOperations)
        {
            foreach (var inventoryOperation in inventoryOperations.Where(i=>i.ActionType == InventoryActionType.Issue || i.ActionType == InventoryActionType.Receipt))
            {
                var iventoryFinalQuantity = inventoryOperationManager.CalculateTransactionGoodsFinalQuantities(inventoryOperation);

                if (iventoryFinalQuantity.Any(e => e.Value != 0))
                    return true;
            }

            return false;
        }

        public bool AreInventoryOperationsPartiallyPriced(List<InventoryOperation> inventoryOperations)
        {
            foreach (var inventoryOperation in inventoryOperations.Where(i => i.ActionType == InventoryActionType.Issue || i.ActionType == InventoryActionType.Receipt))
            {
                var iventoryTransaction = inventoryOperationManager.GetTransaction(inventoryOperation.InventoryOperationId, inventoryOperation.ActionType == InventoryActionType.Issue ? InventoryOperationType.Issue : InventoryOperationType.Receipt);

                if (iventoryTransaction.Inventory_TransactionItem.Any(ti=>ti.Inventory_TransactionItemPrice.Count != 0))
                    return true;
            }

            return false;
        }

    }
}