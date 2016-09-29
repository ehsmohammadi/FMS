using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.IDomainServices.Inventory;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IInventoryOperationManager : IDomainService
    {
        InventoryOperation ManageFuelReportConsumption(FuelReport fuelReport, Dictionary<long, decimal> goodsConsumption, int userId);

        List<InventoryOperation> ManageFuelReportDetailReceive(FuelReportDetail fuelReportDetail, int userId);

        List<InventoryOperation> ManageFuelReportDetailIncrementalCorrectionUsingPricingReference(FuelReportDetail fuelReportDetail, long pricingReferenceId, string pricingReferenceType, int userId);

        List<InventoryOperation> ManageFuelReportDetailIncrementalCorrectionDirectPricing(FuelReportDetail fuelReportDetail, int userId);

        List<InventoryOperation> ManageFuelReportDetailIncrementalCorrectionWithoutPricing(FuelReportDetail fuelReportDetail, int userId);

        long PriceFuelReportDetailIncrementalCorrectionUsingPricingReference(FuelReportDetail fuelReportDetail, long pricingReferenceId, string pricingReferenceType, int userId);

        long PriceFuelReportDetailIncrementalCorrectionDirectPricing(FuelReportDetail fuelReportDetail, int userId);

        List<InventoryOperation> ManageFuelReportDetailDecrementalCorrection(FuelReportDetail fuelReportDetail, int userId);

        List<InventoryOperation> ManageFuelReportDetailDecrementalCorrectionWithoutPricing(FuelReportDetail fuelReportDetail, int userId);
        
        long PriceFuelReportDetailDecrementalCorrectionDefaultPricing(FuelReportDetail fuelReportDetail, int userId);

        long PriceFuelReportDetailDecrementalCorrectionUsingPricingReference(FuelReportDetail fuelReportDetail, long pricingReferenceId, string pricingReferenceType, int userId);

        List<InventoryOperation> ManageFuelReportDetailTransfer(FuelReportDetail fuelReportDetail, long? pricingReferenceId, int userId);

        List<InventoryOperation> ManageScrap(Scrap scrap, int userId);

        //List<InventoryOperation> ManageInvoice(Invoice invoice, int userId);

        InventoryOperation ManageOrderItemBalance(OrderItemBalance orderItemBalance, int userId);

        List<InventoryOperation> ManageCharterInStart(CharterIn charterInStart, int userId);

        List<InventoryOperation> ManageCharterInEnd(CharterIn charterInEnd, int userId, bool inventoryShouldBeDeactivated/*,long? lastIssuedVoyageInventoryOperationId*/);

        List<InventoryOperation> ManageCharterOutStart(CharterOut charterOutStart, int userId, bool inventoryShouldBeDeactivated/*,long? lastIssuedVoyageInventoryOperationId*/);

        List<InventoryOperation> ManageCharterOutEnd(CharterOut charterOutEnd, int userId);

        Inventory_Transaction GetTransaction(long transactionId, InventoryOperationType operationType);

        Inventory_Transaction GetLastTransactionBefore(long transactionId, InventoryOperationType findingOperationType, string referenceType);

        Inventory_Transaction GetLastTransactionBefore(DateTime dateTime, long warehouseId, InventoryOperationType findingOperationType, string referenceType);

        Inventory_OperationReference GetFuelReportDetailReceiveReference(FuelReportDetail fuelReportDetail);

        List<Inventory_OperationReference> GetFueReportDetailsReceiveOperationReference(List<FuelReportDetail> fuelReportDetails);

        decimal GetAverageFee(long transactionId, TransactionType actionType, long goodId, long unitId);

        decimal GetInventoryQuantity(long sharedGoodId, string unitAbbreviation, long vesselInInventoryId, DateTime? requestDateTime);

        void PriceAllSuspendedIssuedItems(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int userId, out string message);

        void PriceAllSuspendedTransactionItemsUsingReference(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int userId, TransactionType? action, out string message);

        void PriceAllSuspendedTransactions(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int userId, out string message);

        void TryGetFuelPurchasePricingReferences(long pricingOperationId, out long fuelReportDetailId, out long[] invoiceItemId);
      


        List<InventoryOperation> UpdateCountSubmitedReciptFlow<T, L>(
                                                  IUpdateCountSubmitedReciptFactory<T, L> updateCountSubmitedReciptFactory,
                                                  IGoodRepository goodRepository,
                                                  Voyage voyage,
                                                  long userId,
                                                  decimal diffQuantity, decimal? oldFee) where T : class;

        List<InventoryOperation> UpdatePriceSubmitedReciptFlow<T, L>(
           IUpdatePriceSubmitedReciptFactory<T, L> updateCountSubmitedReciptFactory,
           IGoodRepository goodRepository,
           Voyage voyage,
           long userId,
           decimal newPrice
           ) where T : class;

        void ActivateWarehouseIncludingRecieptsOperation(string vesselCode, long companyId, DateTime activationDate, List<VesselActivationItem> vesselActivationItems, int userId);
        void RegisterInventory(long id, long companyId, string vesselCode, string name, string description, int userId);
        void SetInventoryTransactionStatusForRegisteredVoucher(string inventoryReferenceActionNumber);

        void SetInventoryTransactionStatusForDeletedVoucher(string inventoryReferenceActionNumber);
        InventoryOperation ManageTrustReceivesIssue(CharterOut charterOutStart, List<GoodTrustReceiveData> goodsTrustReceiveData, int userId);
        InventoryOperation ManageTrustReceivesIssue(CharterIn charterInEnd, List<GoodTrustReceiveData> goodsTrustReceiveData, int userId);
        Inventory_Transaction GetLastIssuedTrustReceivesTransaction(string warehouseCode, DateTime comparingDateTime);

        Inventory_OperationReference GetFuelReportConsumptionReference(FuelReport fuelReport);
        Inventory_OperationReference GetFuelReportDetailTransferReference(FuelReportDetail fuelReportDetail);
        Inventory_OperationReference GetFuelReportDetailDecrementalCorrectionReference(FuelReportDetail fuelReportDetail);
        Inventory_OperationReference GetFuelReportDetailIncrementalCorrectionReference(FuelReportDetail fuelReportDetail);
        Inventory_OperationReference GetScrapReference(Scrap scrap);
        Inventory_OperationReference GetFuelReportDetailReceivePricingReference(OrderItemBalance orderItemBalance);
        Inventory_OperationReference GetFuelReportDetailCorrectionPricingReference(FuelReportDetail fuelReportDetail);

        Inventory_OperationReference GetCharterInStartReference(CharterIn charterInStart);
        List<Inventory_OperationReference> GetCharterInEndReference(CharterIn charterInEnd);
        List<Inventory_OperationReference> GetCharterOutStartReference(CharterOut charterOutStart);
        Inventory_OperationReference GetCharterOutEndReference(CharterOut charterOutEnd);

        Inventory_OperationReference GetCharterInEndIssueTrustReceiptsReference(CharterIn charterInEnd);
        List<Inventory_OperationReference> GetCharterInEndIncrementalCorrectionReference(CharterIn charterInEnd);
        List<Inventory_OperationReference> GetCharterInEndDecrementalCorrectionReference(CharterIn charterInEnd);


        Inventory_OperationReference GetCharterOutStartIssueTrustReceiptsReference(CharterOut charterOutStart);
        List<Inventory_OperationReference> GetCharterOutStartDecrementalCorrectionReference(CharterOut charterOutStart);
        List<Inventory_OperationReference> GetCharterOutStartIncrementalCorrectionReference(CharterOut charterOutStart);
        


        long GetMeasurementUnitId(string unitAbbreviation);
        long GetCurrencyId(string currencyAbbreviation);

        List<InventoryOperation> CorrectTransaction(Inventory_OperationReference reference, long? pricingReferenceId, Dictionary<long, List<GoodQuantity>> entityGoodsQuantities, Dictionary<long, List<GoodQuantityPricing>> entityGoodsQuantitiesWithPrices, int userId);

        void CorrectReceiptTransactionPricing(Inventory_OperationReference reference, Dictionary<long, List<GoodQuantity>> entityGoodsQuantities, Dictionary<long, List<GoodQuantityPricing>> entityGoodsQuantitiesWithPrices, int userId);

        InventoryOperationResult RevertTransaction(Inventory_OperationReference reference, int userId);

        void RevertTransactionPricing(int inventoryTransactionItemPriceId, int userId);

        void CorrectReceiptTransactionPricing(int inventoryTransactionItemPriceId, GoodQuantityPricing goodQuantityPricing, int userId);

        Dictionary<long, decimal> CalculateTransactionGoodsFinalQuantities(InventoryOperation inventoryOperation);

        bool GetWarehouseCurrentActiveStatus(long warehouseId);

        void DeactivateWarehouse(string warehouseCode, long companyId, DateTime changeDateTime, int userId);

        void ActivateWarehouse(string warehouseCode, long companyId, DateTime changeDateTime, int userId);
    }
}
