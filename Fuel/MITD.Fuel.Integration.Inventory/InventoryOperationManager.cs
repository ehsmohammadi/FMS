using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.Commands;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.OrderAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.IDomainServices.Inventory;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;
using MITD.Fuel.Integration.Inventory.Infrastructure;

namespace MITD.Fuel.Integration.Inventory
{
    /// <summary>
    /// Manages Operaions that should be done in inventory based on FuelManagement requests.
    /// </summary>
    public partial class InventoryOperationManager : IInventoryOperationManager
    {
        public const string INVENTORY_ADD_ACTION_VALUE = "insert";
        public const string INVENTORY_REMOVE_ACTION_VALUE = "delete";
        public const string INVENTORY_UPDATE_ACTION_VALUE = "update";

        public const string OPERATION_SUCCESSFUL_MESSAGE = "OperationSuccessful";
        public const string INCONSISTENT_PRICING_REFERENCE_ID_MESSAGE = "Inconsistent Pricing ReferenceId";
        public const string NO_PRICING_REFERENCE_ID_MESSAGE = "No Pricing ReferenceId";

        public const long INVALID_ID = -1;

        public enum InventoryStoreTypesCode
        {
            Charter_In_Start = 1,
            FuelReport_Receive_Trust = 2,
            FuelReport_Receive_InternalTransfer = 3,
            FuelReport_Receive_TransferPurchase = 4,
            FuelReport_Receive_Purchase = 5,
            FuelReport_Receive_From_Tank = 6,
            FuelReport_Incremental_Correction_Inventory_Adjustment = 7,
            Charter_Out_End = 8,
            Charter_Out_Start = 9,
            FuelReport_Transfer_InternalTransfer = 10,
            FuelReport_Transfer_TransferSale = 11,
            FuelReport_Transfer_Rejected = 12,
            FuelReport_Decremental_Correction_Inventory_Adjustment = 13,
            EOV_Consumption = 14,
            EOM_Consumption = 15,
            EOY_Consumption = 16,
            Charter_In_End = 17,
            Scrap = 18,
            Vessel_Activation = 19,
            Charter_Out_Start_Decremental_Adjustment = 20,
            Charter_Out_Start_Incremental_Adjustment = 21,
            Charter_In_End_Decremental_Adjustment = 22,
            Charter_In_End_Incremental_Adjustment = 23,
            FuelReport_Decremental_Correction_for_Issued_Voyage = 24,
            FuelReport_Incremental_Correction_for_Issued_Voyage = 25,
            Adjustment_Receipt = 26,
            Adjustment_Issue = 27,
            Issue_Total_Received_Trust = 28,
        }

        private IsTransactionFullyPriced isTransactionFullyPriced;

        //================================================================================

        public InventoryOperationManager()
        {
            this.isTransactionFullyPriced = new IsTransactionFullyPriced();
        }

        //================================================================================

        //Commented due to moving to op manager.
        //private decimal calculateConsumption(FuelReportDetail fuelReportDetail)
        //{
        //    //if (!(/*fuelReportDetail.FuelReport.FuelReportType == FuelReportTypes.EndOfMonth ||
        //    //       fuelReportDetail.FuelReport.FuelReportType == FuelReportTypes.EndOfYear*/
        //    //    fuelReportDetail.FuelReport.FuelReportType == FuelReportTypes.EndOfVoyage ||
        //    //    fuelReportDetail.FuelReport.IsEndOfYearReport()
        //    //    ))
        //    //{
        //    //    return 0;
        //    //}

        //    var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();

        //    var reportingConsumption = fuelReportDomainService.CalculateReportingConsumption(fuelReportDetail);

        //    return reportingConsumption;
        //}

        //================================================================================

        #region Inventory Operations + Calling Inventory Stored Procedures

        //================================================================================

        private List<Inventory_OperationReference> findInventoryOperationReference(DataContainer dbContext, InventoryOperationType transactionType, string referenceType, string referenceNumber)
        {
            var foundOperationReferences = dbContext.Inventory_OperationReference.Where(
                opr =>
                    opr.OperationType == (int)transactionType &&
                    opr.ReferenceType == referenceType &&
                    opr.ReferenceNumber == referenceNumber).ToList();

            return foundOperationReferences;
        }

        //================================================================================

        public Inventory_OperationReference issue(DataContainer dbContext, int companyId, int warehouseId, int timeBucketId, DateTime operationDateTime,
            int storeTypesId, int? pricingReferenceId, int? adjustmentForTransactionId, string referenceType, string referenceNumber, int userId, out string code, out string message)
        {
            var transactionIdParameter = new SqlParameter("@TransactionId", SqlDbType.Int, sizeof(int), ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, 0);
            var codeParameter = new SqlParameter("@Code", SqlDbType.Decimal, sizeof(decimal), ParameterDirection.Output, false, 20, 2, "", DataRowVersion.Default, 0);
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            try
            {
                dbContext.Database.ExecuteSqlCommand(
                                    "[Inventory].[TransactionOperation] @Action=@Action, @TransactionAction=@TransactionAction, @CompanyId=@CompanyId, @WarehouseId=@WarehouseId, @TimeBucketId=@TimeBucketId, @StoreTypesId=@StoreTypesId, @PricingReferenceId=@PricingReferenceId, @RegistrationDate=@RegistrationDate, @AdjustmentForTransactionId=@AdjustmentForTransactionId, @ReferenceType=@ReferenceType, @ReferenceNo=@ReferenceNo, @UserCreatorId=@UserCreatorId, @TransactionId=@TransactionId OUT, @Code=@Code OUT, @Message=@Message OUT",
                                    new SqlParameter("@Action", INVENTORY_ADD_ACTION_VALUE),
                                    new SqlParameter("@TransactionAction", (byte)TransactionType.Issue),
                                    new SqlParameter("@CompanyId", companyId),
                                    new SqlParameter("@WarehouseId", warehouseId),
                                    new SqlParameter("@TimeBucketId", timeBucketId),
                                    new SqlParameter("@StoreTypesId", storeTypesId),
                                    new SqlParameter("@PricingReferenceId", pricingReferenceId.HasValue ? (object)pricingReferenceId : DBNull.Value),
                                    new SqlParameter("@AdjustmentForTransactionId", adjustmentForTransactionId.HasValue ? (object)adjustmentForTransactionId : DBNull.Value),
                                    new SqlParameter("@RegistrationDate", operationDateTime),
                                    new SqlParameter("@ReferenceType", referenceType),
                                    new SqlParameter("@ReferenceNo", referenceNumber),
                                    new SqlParameter("@UserCreatorId", userId),
                                    transactionIdParameter,
                                    codeParameter,
                                    messageParameter);

            }
            catch (Exception ex)
            {
                throw new InvalidOperation("AddIssue", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("AddIssue", message);

            var transactionId = (int)transactionIdParameter.Value;

            code = codeParameter.Value.ToString();

            return addIssueOperationReference(dbContext, referenceType, referenceNumber, transactionId);
        }

        //================================================================================

        public Inventory_OperationReference receipt(DataContainer dbContext, int companyId, int warehouseId, int timeBucketId, DateTime operationDateTime,
            int storeTypesId, int? pricingReferenceId, int? adjustmentForTransactionId, string referenceType, string referenceNumber, int userId, out string code, out string message)
        {
            var transactionIdParameter = new SqlParameter("@TransactionId", SqlDbType.Int, sizeof(int), ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, 0);
            var codeParameter = new SqlParameter("@Code", SqlDbType.Decimal, sizeof(decimal), ParameterDirection.Output, false, 20, 2, "", DataRowVersion.Default, 0);
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                    "[Inventory].[TransactionOperation] @Action=@Action, @TransactionAction=@TransactionAction, @CompanyId=@CompanyId, @WarehouseId=@WarehouseId, @TimeBucketId=@TimeBucketId, @StoreTypesId=@StoreTypesId, @PricingReferenceId=@PricingReferenceId, @RegistrationDate=@RegistrationDate, @AdjustmentForTransactionId=@AdjustmentForTransactionId, @ReferenceType=@ReferenceType, @ReferenceNo=@ReferenceNo, @UserCreatorId=@UserCreatorId, @TransactionId=@TransactionId OUT, @Code=@Code OUT, @Message=@Message OUT",
                                   new SqlParameter("@Action", INVENTORY_ADD_ACTION_VALUE),
                                   new SqlParameter("@TransactionAction", (byte)TransactionType.Receipt),
                                   new SqlParameter("@CompanyId", companyId),
                                   new SqlParameter("@WarehouseId", warehouseId),
                                   new SqlParameter("@TimeBucketId", timeBucketId),
                                   new SqlParameter("@StoreTypesId", storeTypesId),
                                   new SqlParameter("@PricingReferenceId", pricingReferenceId.HasValue ? (object)pricingReferenceId : DBNull.Value),
                                    new SqlParameter("@AdjustmentForTransactionId", adjustmentForTransactionId.HasValue ? (object)adjustmentForTransactionId : DBNull.Value),
                                   new SqlParameter("@RegistrationDate", operationDateTime),
                                   new SqlParameter("@ReferenceType", referenceType),
                                   new SqlParameter("@ReferenceNo", referenceNumber),
                                   new SqlParameter("@UserCreatorId", userId),
                                   transactionIdParameter,
                                   codeParameter,
                                   messageParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("AddReceipt", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("AddReceipt", message);

            var transactionId = (int)transactionIdParameter.Value;

            code = codeParameter.Value.ToString();

            return addReceiptOperationReference(dbContext, referenceType, referenceNumber, transactionId);
        }

        //================================================================================

        public List<int> addTransactionItems(DataContainer dbContext, int transactionId, IEnumerable<Inventory_TransactionItem> transactionItems, int userId, out string message)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            var transactionItemIdsParameter = new SqlParameter("@TransactionItemsId", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            var transactionItemTable = new DataTable();

            transactionItemTable.Columns.AddRange(new DataColumn[]{
                                                            new DataColumn("Id"),
                                                            new DataColumn("GoodId"),
                                                            new DataColumn("QuantityUnitId"),
                                                            new DataColumn("QuantityAmount"),
                                                            new DataColumn("Description")
                                                       });

            foreach (var transactionItem in transactionItems)
            {
                if (transactionItem.QuantityAmount == 0) continue;

                var itemRow = transactionItemTable.NewRow();

                itemRow["Id"] = null;
                itemRow["GoodId"] = transactionItem.GoodId;
                itemRow["QuantityUnitId"] = transactionItem.QuantityUnitId;
                itemRow["QuantityAmount"] = transactionItem.QuantityAmount;
                itemRow["Description"] = transactionItem.Description;

                transactionItemTable.Rows.Add(itemRow);
            }

            var transactionItemParameter = new SqlParameter("@TransactionItems", SqlDbType.Structured, 4096, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Default, transactionItemTable);
            transactionItemParameter.TypeName = "TypeTransactionItems";

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                    "[Inventory].[TransactionItemsOperation] @Action=@Action, @TransactionId=@TransactionId, @UserCreatorId=@UserCreatorId, @TransactionItems=@TransactionItems, @TransactionItemsId=@TransactionItemsId OUT, @Message=@Message OUT",
                                   new SqlParameter("@Action", INVENTORY_ADD_ACTION_VALUE),
                                   new SqlParameter("@TransactionId", transactionId),
                                   new SqlParameter("@UserCreatorId", userId),
                                   transactionItemParameter,
                                   transactionItemIdsParameter,
                                   messageParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("AddTransactionItem", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("AddTransactionItem", message);

            var result = extractIds(transactionItemIdsParameter.Value.ToString());

            return result;
        }

        private List<int> extractIds(string listString)
        {
            return listString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }

        //================================================================================

        private void priceTransactionItemsManually(DataContainer dbContext, IEnumerable<Inventory_TransactionItemPrice> transactionItemPrices,
            int userId, out string message, string pricingReferenceType, string pricingReferenceNumber)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            var transactionItemPriceIdsParameter = new SqlParameter("@TransactionItemPriceIds", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            var transactionItemPricesTable = new DataTable();
            transactionItemPricesTable.Columns.AddRange(new DataColumn[]
                                                         {
                                                            new DataColumn("Id"),
                                                            new DataColumn("TransactionItemId"),
                                                            new DataColumn("QuantityUnitId"),
                                                            new DataColumn("QuantityAmount"),
                                                            new DataColumn("PriceUnitId"),
                                                            new DataColumn("Fee"),
                                                            new DataColumn("RegistrationDate"),
                                                            new DataColumn("Description")
                                                         });

            foreach (var transactionItemPrice in transactionItemPrices)
            {
                if (transactionItemPrice.QuantityAmount == 0) continue;

                var itemRow = transactionItemPricesTable.NewRow();
                itemRow["Id"] = null;
                itemRow["TransactionItemId"] = transactionItemPrice.TransactionItemId;
                itemRow["QuantityUnitId"] = transactionItemPrice.QuantityUnitId;
                itemRow["QuantityAmount"] = transactionItemPrice.QuantityAmount;
                itemRow["PriceUnitId"] = transactionItemPrice.PriceUnitId;
                itemRow["Fee"] = transactionItemPrice.Fee;
                itemRow["RegistrationDate"] = transactionItemPrice.RegistrationDate.HasValue ? transactionItemPrice.RegistrationDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : null;
                itemRow["Description"] = transactionItemPrice.Description;

                transactionItemPricesTable.Rows.Add(itemRow);
            }

            var transactionItemPricesParameter = new SqlParameter("@TransactionItemPrices", SqlDbType.Structured, 4096, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Default, transactionItemPricesTable);
            transactionItemPricesParameter.TypeName = "TypeTransactionItemPrices";

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                                                     "[Inventory].[TransactionItemPricesOperation] @Action=@Action, @UserCreatorId=@UserCreatorId, @TransactionItemPrices=@TransactionItemPrices,@TransactionItemPriceIds=@TransactionItemPriceIds OUT, @Message=@Message OUT",
                                                     new SqlParameter("@Action", INVENTORY_ADD_ACTION_VALUE),
                                                     new SqlParameter("@UserCreatorId", userId),
                                                     transactionItemPricesParameter,
                                                     transactionItemPriceIdsParameter,
                                                     messageParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("AddTransactionItemPrices", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("AddTransactionItemPrices", message);

            addPricingOperationReferences(dbContext, pricingReferenceType, pricingReferenceNumber, transactionItemPriceIdsParameter.Value.ToString());
        }

        //================================================================================

        private void priceTransactionItemManually(DataContainer dbContext, Inventory_TransactionItemPrice transactionItemPrice,
            int userId, out string message, string pricingReferenceType, string pricingReferenceNumber)
        {
            var transactionItemPrices = new List<Inventory_TransactionItemPrice>
                                         {
                                             transactionItemPrice
                                         };

            priceTransactionItemsManually(dbContext, transactionItemPrices, userId, out message, pricingReferenceType, pricingReferenceNumber);
        }

        //================================================================================

        private void priceIssuedItemsInFIFO(DataContainer dbContext, IEnumerable<Inventory_TransactionItemPricingId> transactionItemIds,
            int userId, out string message, string pricingReferenceType, string pricingReferenceNumber)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            var transactionItemPriceIdsParameter = new SqlParameter("@TransactionItemPriceIds", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            var notPricedTransactionIdParameter = new SqlParameter("@NotPricedTransactionId", SqlDbType.Int, sizeof(int), ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, null);

            var transactionItemPricesTable = new DataTable();
            transactionItemPricesTable.Columns.AddRange(new DataColumn[]
                                                         {
                                                            new DataColumn("Id"),
                                                            new DataColumn("Description")
                                                         });

            foreach (var transactionItemId in transactionItemIds)
            {
                var itemRow = transactionItemPricesTable.NewRow();
                itemRow["Id"] = transactionItemId.Id;
                itemRow["Description"] = transactionItemId.Description;

                transactionItemPricesTable.Rows.Add(itemRow);
            }

            var issueItemIds = new SqlParameter("@IssueItemIds", SqlDbType.Structured, 4096, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Default, transactionItemPricesTable);
            issueItemIds.TypeName = "Ids";

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                    "[Inventory].[PriceGivenIssuedItems] @UserCreatorId=@UserCreatorId, @IssueItemIds=@IssueItemIds,@TransactionItemPriceIds=@TransactionItemPriceIds OUT, @Message=@Message OUT, @NotPricedTransactionId=@NotPricedTransactionId OUT",
                                   new SqlParameter("@UserCreatorId", userId),
                                   issueItemIds,
                                   transactionItemPriceIdsParameter,
                                   messageParameter,
                                   notPricedTransactionIdParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("PriceIssuedItemsAutomatically", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            var notPricedTransactionId = notPricedTransactionIdParameter.Value == null ? null : (int?)(int)notPricedTransactionIdParameter.Value;

            if (notPricedTransactionId.HasValue && notPricedTransactionId.Value != 0)
            {
                var transactionCode = dbContext.Inventory_Transaction.Single(t => t.Id == notPricedTransactionId.Value).Code;

                throw new InvalidOperation("PriceIssuedItemsInFIFO", "The pricing procedure for the Issued Items reached to a not priced Receipt. Receipt Code : " + transactionCode);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("PriceIssuedItemsInFIFO", message);

            addPricingOperationReferences(dbContext, pricingReferenceType, pricingReferenceNumber, transactionItemPriceIdsParameter.Value.ToString());
        }

        //================================================================================

        public void priceIssuedItemInFIFO(DataContainer dbContext, int transactionItemId, string description,
            int userId, out string message, string pricingReferenceType, string pricingReferenceNumber)
        {
            var TransactionItemIds = new List<Inventory_TransactionItemPricingId> { new Inventory_TransactionItemPricingId() { Id = transactionItemId, Description = description } };

            priceIssuedItemsInFIFO(dbContext, TransactionItemIds, userId,
                out message, pricingReferenceType, pricingReferenceNumber);
        }

        //================================================================================

        public void priceSuspendedTransactionUsingReference(DataContainer dbContext, int transactionId, string description, DateTime? effectiveDateTime,
            int userId, out string message, string pricingReferenceType, string pricingReferenceNumber)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            var transactionItemPriceIdsParameter = new SqlParameter("@TransactionItemPriceIds", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                            "[Inventory].[PriceSuspendedTransactionUsingReference] @TransactionId=@TransactionId, @Description=@Description, @EffectiveDateTime=@EffectiveDateTime, @UserCreatorId=@UserCreatorId, @TransactionItemPriceIds=@TransactionItemPriceIds OUT, @Message=@Message OUT",
                            new SqlParameter("@TransactionId", transactionId),
                            new SqlParameter("@Description", description),
                            new SqlParameter("@EffectiveDateTime", effectiveDateTime.HasValue ? (object)effectiveDateTime.Value : DBNull.Value),
                            new SqlParameter("@UserCreatorId", userId),
                            transactionItemPriceIdsParameter,
                            messageParameter);
            }
            catch (Exception ex)
            {
                //TODO: Error in pricing using reference
                //throw new InvalidOperation("PriceSuspendedTransactionUsingReference", ex.Message);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("PriceSuspendedTransactionUsingReference", message);

            addPricingOperationReferences(dbContext, pricingReferenceType, pricingReferenceNumber, transactionItemPriceIdsParameter.Value.ToString());
        }

        //================================================================================

        private void priceAllSuspendedIssuedItems(DataContainer dbContext, long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate,
            int userId, out string message)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            var transactionItemPriceIds = new SqlParameter("@TransactionItemPriceIds", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                            "[Inventory].[PriceAllSuspendedIssuedItems] " +
                            "@CompanyId =@CompanyId,@WarehouseId =@WarehouseId,@FromDate =@FromDate,@ToDate =@ToDate,@UserCreatorId=@UserCreatorId, @TransactionItemPriceIds=@TransactionItemPriceIds OUT, @Message=@Message OUT",
                            new SqlParameter("@CompanyId", companyId.HasValue ? (object)companyId : DBNull.Value),
                            new SqlParameter("@WarehouseId", warehouseId.HasValue ? (object)warehouseId : DBNull.Value),
                            new SqlParameter("@FromDate", fromDate.HasValue ? (object)fromDate : DBNull.Value),
                            new SqlParameter("@ToDate", toDate.HasValue ? (object)toDate : DBNull.Value),
                            new SqlParameter("@UserCreatorId", userId),
                            transactionItemPriceIds,
                            messageParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("PriceAllSuspendedIssuedItems", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("PriceAllSuspendedIssuedItems", message);
        }

        //================================================================================

        private void priceAllSuspendedTransactionItemsUsingReference(DataContainer dbContext,
          long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate,
          int userId, TransactionType? action, out string message)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
            var transactionItemPriceIds = new SqlParameter("@TransactionItemPriceIds", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            try
            {


                dbContext.Database.ExecuteSqlCommand(
                                                     "[Inventory].[PriceAllSuspendedTransactionItemsUsingReference] " +
                                                         "@CompanyId =@CompanyId,@WarehouseId =@WarehouseId,@FromDate =@FromDate,@ToDate =@ToDate,@UserCreatorId=@UserCreatorId, @Action=@Action, @TransactionItemPriceIds=@TransactionItemPriceIds OUT, @Message=@Message OUT",
                                                     new SqlParameter("@CompanyId", companyId.HasValue ? (object)companyId : DBNull.Value),
                                                     new SqlParameter("@WarehouseId", warehouseId.HasValue ? (object)warehouseId : DBNull.Value),
                                                     new SqlParameter("@FromDate", fromDate.HasValue ? (object)fromDate : DBNull.Value),
                                                     new SqlParameter("@ToDate", toDate.HasValue ? (object)toDate : DBNull.Value),
                                                     new SqlParameter("@UserCreatorId", userId),
                                                     new SqlParameter("@Action", action.HasValue ? (object)(byte)action.Value : DBNull.Value),
                                                     transactionItemPriceIds,
                                                     messageParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("PriceAllSuspendedTransactionItemsUsingReference", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }
            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("PriceAllSuspendedTransactionItemsUsingReference", message);
        }

        //================================================================================

        private Inventory_OperationReference addPricingOperationReference(DataContainer dbContext, string pricingReferenceType, string pricingReferenceNumber, long operationId)
        {
            var result = dbContext.Inventory_OperationReference.Add(new Inventory_OperationReference()
            {
                OperationId = operationId,
                OperationType = (int)InventoryOperationType.Pricing,
                ReferenceNumber = pricingReferenceNumber,
                ReferenceType = pricingReferenceType
            });

            dbContext.SaveChanges();

            return result;
        }

        private IList<Inventory_OperationReference> addPricingOperationReferences(DataContainer dbContext, string pricingReferenceType, string pricingReferenceNumber, string createdIds)
        {
            var result = new List<Inventory_OperationReference>();

            var addedIds = createdIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

            foreach (var addedId in addedIds)
            {
                result.Add(addPricingOperationReference(dbContext, pricingReferenceType, pricingReferenceNumber, addedId));
            }

            return result;
        }

        //================================================================================

        private Inventory_OperationReference addIssueOperationReference(DataContainer dbContext, string pricingReferenceType, string pricingReferenceNumber, long operationId)
        {
            var result = dbContext.Inventory_OperationReference.Add(new Inventory_OperationReference()
            {
                OperationId = operationId,
                OperationType = (int)InventoryOperationType.Issue,
                ReferenceNumber = pricingReferenceNumber,
                ReferenceType = pricingReferenceType
            });

            dbContext.SaveChanges();

            return result;
        }

        //================================================================================

        private Inventory_OperationReference addReceiptOperationReference(DataContainer dbContext, string pricingReferenceType, string pricingReferenceNumber, long operationId)
        {
            var result = dbContext.Inventory_OperationReference.Add(new Inventory_OperationReference()
            {
                OperationId = operationId,
                OperationType = (int)InventoryOperationType.Receipt,
                ReferenceNumber = pricingReferenceNumber,
                ReferenceType = pricingReferenceType
            });

            dbContext.SaveChanges();

            return result;
        }

        //================================================================================

        private void activateWarehouse(DataContainer dbContext, long warehouseId, DateTime changeDateTime, int userId)
        {
            dbContext.Database.ExecuteSqlCommand(
                "[Inventory].[ChangeWarehouseStatus] @IsActive=@IsActive, @WarehouseId=@WarehouseId, @ChangeDateTime=@ChangeDateTime, @UserCreatorId=@UserCreatorId",
                               new SqlParameter("@IsActive", true),
                               new SqlParameter("@WarehouseId", warehouseId),
                               new SqlParameter("@ChangeDateTime", changeDateTime),
                               new SqlParameter("@UserCreatorId", userId));
        }

        public void ActivateWarehouse(string warehouseCode, long companyId, DateTime changeDateTime, int userId)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                var foundWarehouse = dbContext.Inventory_Warehouse.Single(w => w.Code == warehouseCode && w.CompanyId == companyId);

                var warehouseActiveStatus = getWarehouseCurrentActiveStatus(dbContext, foundWarehouse.Id);

                if (warehouseActiveStatus == false)
                    activateWarehouse(dbContext, foundWarehouse.Id, changeDateTime, userId);
            }
        }

        //================================================================================

        public List<InventoryOperation> UpdatePriceSubmitedReciptFlow<T, L>(IUpdatePriceSubmitedReciptFactory<T, L> updateCountSubmitedReciptFactory, IGoodRepository goodRepository, Voyage voyage, long userId, decimal newPrice) where T : class
        {
            throw new NotImplementedException();
        }

        public void ActivateWarehouseIncludingRecieptsOperation(string vesselCode, long companyId, DateTime activationDate, List<VesselActivationItem> vesselActivationItems, int userId)
        {
            if (activationDate.Date > DateTime.Now)
                throw new BusinessRuleException("", "The activation date is after the current date.");

            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var warehouse = dbContext.Inventory_Warehouse.SingleOrDefault(w => w.Code == vesselCode && w.CompanyId == companyId);

                    if (warehouse == null)
                        throw new InvalidArgument("Warehouse not found.");

                    var referenceType = InventoryOperationReferenceTypes.INVENTORY_INITIATION;
                    var referenceNo = vesselCode;
                    var description = "Inventory Initiation of " + vesselCode;

                    var activationTime = activationDate;

                    var timeBucket =
                        dbContext.Inventory_TimeBucket.SingleOrDefault(
                            tb => (tb.StartDate == null || tb.StartDate <= activationTime) &&
                            (tb.EndDate == null || tb.EndDate >= activationTime));

                    if (timeBucket == null)
                        throw new BusinessRuleException("", "No financial Bucket Date is defined for the activation date.");

                    //ActivateWarehouseIncludingRecieptsOperation

                    //var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096,
                    //    ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");
                    //var transactionItemIdsParameter = new SqlParameter("@TransactionItemsId", SqlDbType.NVarChar, 4096,
                    //    ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

                    var transactionItemTable = new DataTable();

                    transactionItemTable.Columns.AddRange(new DataColumn[]
                    {
                        new DataColumn("Id"),
                        new DataColumn("GoodId"),
                        new DataColumn("QuantityUnitId"),
                        new DataColumn("QuantityAmount"),
                        new DataColumn("PriceUnitId"),
                        new DataColumn("Fee"),
                        new DataColumn("Description")
                    });

                    foreach (var vesselActivationItem in vesselActivationItems)
                    {
                        if (vesselActivationItem.Rob == 0) continue;

                        var good = dbContext.Inventory_Good.Single(g => g.Code == vesselActivationItem.GoodCode);
                        var unitId = getMeasurementUnitId(dbContext, vesselActivationItem.UnitCode);
                        var currencyId = getCurrencyId(dbContext, vesselActivationItem.CurrencyCode);

                        var itemRow = transactionItemTable.NewRow();

                        itemRow["Id"] = null;
                        itemRow["GoodId"] = good.Id;
                        itemRow["QuantityUnitId"] = unitId;
                        itemRow["QuantityAmount"] = vesselActivationItem.Rob;
                        itemRow["PriceUnitId"] = currencyId;
                        itemRow["Fee"] = vesselActivationItem.Fee;
                        itemRow["Description"] = "Starting Receipt > " + good.Code;

                        transactionItemTable.Rows.Add(itemRow);
                    }


                    var transactionItemParameter = new SqlParameter("@TransactionItemsList", SqlDbType.Structured, 4096,
                        ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Default, transactionItemTable);
                    transactionItemParameter.TypeName = "TypeTransactionItemWithPrice";

                    try
                    {
                        dbContext.Database.ExecuteSqlCommand(
                                    @"[Inventory].[ActivateWarehouseIncludingRecieptsOperation] 
                                            @WarehouseId=@WarehouseId, 
                                            @TimeBucketId=@TimeBucketId,
                                            @StoreTypesId=@StoreTypesId,
                                            @RegistrationDate=@RegistrationDate,
                                            @ReferenceType=@ReferenceType,
                                            @ReferenceNo=@ReferenceNo,
                                            @TransactionItems=@TransactionItemsList,
                                            @Description=@Description,
                                            @UserCreatorId=@UserCreatorId",

                                    new SqlParameter("@WarehouseId", warehouse.Id),
                                    new SqlParameter("@TimeBucketId", timeBucket.Id),
                                    new SqlParameter("@StoreTypesId", getVesselActivationStoreType(dbContext)),
                                    new SqlParameter("@RegistrationDate", activationTime),
                                    new SqlParameter("@ReferenceType", referenceType),
                                    new SqlParameter("@ReferenceNo", referenceNo),
                                    transactionItemParameter,
                                    new SqlParameter("@Description", description),
                                    new SqlParameter("@UserCreatorId", userId));
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperation("ActivateWarehouseIncludingRecieptsOperation", ex.Message);
                    }

                    //transaction.Commit();
                }
            }
        }

        public void SetInventoryTransactionStatusForRegisteredVoucher(string inventoryReferenceActionNumber)
        {
            var inventoryTransactionRepository = ServiceLocator.Current.GetInstance<IRepository<Inventory_Transaction>>();

            TransactionType transactionType;
            long inventoryWarehouseId;
            decimal code;
            string vesselCode;
            InventoryExtensions.ExtractActionNumberValues(inventoryReferenceActionNumber, out transactionType, out inventoryWarehouseId, out code, out vesselCode);

            var invntoryTransaction = inventoryTransactionRepository.Single(t => t.WarehouseId == inventoryWarehouseId && t.Code == code);

            if (invntoryTransaction == null)
            {
                throw new ObjectNotFound("InventoryTransaction");

            }

            if (invntoryTransaction.Status.Value == (byte)TransactionState.FullPriced)
            {
                invntoryTransaction.Status = (byte)TransactionState.Vouchered;
            }
            else if (invntoryTransaction.Status.Value == (byte)TransactionState.JustRegistered ||
                invntoryTransaction.Status.Value == (byte)TransactionState.PartialPriced)
            {
                throw new InvalidOperation("SetTransactionStatusToVouchered", "The current transaction status is not full priced.");
            }
        }

        //================================================================================

        public void SetInventoryTransactionStatusForDeletedVoucher(string inventoryReferenceActionNumber)
        {
            var inventoryTransactionRepository = ServiceLocator.Current.GetInstance<IRepository<Inventory_Transaction>>();

            TransactionType transactionType;
            long inventoryWarehouseId;
            decimal code;
            string vesselCode;
            InventoryExtensions.ExtractActionNumberValues(inventoryReferenceActionNumber, out transactionType, out inventoryWarehouseId, out code, out vesselCode);

            var invntoryTransaction = inventoryTransactionRepository.Single(t => t.WarehouseId == inventoryWarehouseId && t.Code == code);

            if (invntoryTransaction == null)
            {
                throw new ObjectNotFound("InventoryTransaction");
            }

            if (invntoryTransaction.Status.Value == (byte)TransactionState.Vouchered)
            {
                invntoryTransaction.Status = (byte)TransactionState.FullPriced;
            }
            else if (invntoryTransaction.Status.Value == (byte)TransactionState.JustRegistered ||
                invntoryTransaction.Status.Value == (byte)TransactionState.PartialPriced)
            {
                //throw new InvalidOperation("RevertTransactionStatusFromVouchered", "The current transaction status is not vouchered.");
            }
        }

        //================================================================================

        private void deactivateWarehouse(DataContainer dbContext, long warehouseId, DateTime changeDateTime, int userId)
        {
            dbContext.Database.ExecuteSqlCommand(
                "[Inventory].[ChangeWarehouseStatus] @IsActive=@IsActive, @WarehouseId=@WarehouseId, @ChangeDateTime=@ChangeDateTime, @UserCreatorId=@UserCreatorId",
                               new SqlParameter("@IsActive", false),
                               new SqlParameter("@WarehouseId", warehouseId),
                               new SqlParameter("@ChangeDateTime", changeDateTime),
                               new SqlParameter("@UserCreatorId", userId));
        }

        public void DeactivateWarehouse(string warehouseCode, long companyId, DateTime changeDateTime, int userId)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                var foundWarehouse = dbContext.Inventory_Warehouse.Single(w => w.Code == warehouseCode && w.CompanyId == companyId);

                var warehouseActiveStatus = getWarehouseCurrentActiveStatus(dbContext, foundWarehouse.Id);

                if (warehouseActiveStatus == true)
                    this.deactivateWarehouse(dbContext, foundWarehouse.Id, changeDateTime, userId);
            }
        }
        //================================================================================

        private long getCurrencyId(DataContainer dbContext, string abbreviation)
        {
            return dbContext.Inventory_Unit.Single(u =>
                 u.IsCurrency != null && u.IsCurrency.Value == true &&
                 u.Abbreviation.ToUpper() == abbreviation.ToUpper()).Id;
        }

        //================================================================================

        private decimal getExchangeRate(DataContainer dbContext, string fromCurrencyAbbreviation, string toCurrencyAbbreviation, DateTime dateTimeOfChange)
        {
            var fromCurrencyId = getCurrencyId(dbContext, fromCurrencyAbbreviation);
            var toCurrencyId = getCurrencyId(dbContext, toCurrencyAbbreviation);

            var exchangeData = dbContext.Inventory_UnitConvert.Where
                (uc =>
                     uc.UnitId == fromCurrencyId && uc.SubUnitId == toCurrencyId);

            bool bIsFromFirstToSecondAbbreviationFound = true;

            if (exchangeData.Count() == 0)
            {
                exchangeData = dbContext.Inventory_UnitConvert.Where(uc => uc.UnitId == toCurrencyId && uc.SubUnitId == fromCurrencyId);

                bIsFromFirstToSecondAbbreviationFound = false;
            }

            var conversion = exchangeData.OrderByDescending(p => p.CreateDate).FirstOrDefault(uc => (!uc.EffectiveDateStart.HasValue || uc.EffectiveDateStart.Value <= dateTimeOfChange) && (!uc.EffectiveDateEnd.HasValue || uc.EffectiveDateEnd.Value >= dateTimeOfChange));

            if (conversion == null)
                throw new ObjectNotFound("UnitConversionInInventory", 0);

            return bIsFromFirstToSecondAbbreviationFound ? conversion.Coefficient : 1 / conversion.Coefficient;
        }

        //================================================================================

        public long getMeasurementUnitId(DataContainer dbContext, string abbreviation)
        {
            return dbContext.Inventory_Unit.Single(u =>
                (u.IsCurrency == null || u.IsCurrency.Value == false) &&
                u.Abbreviation.ToUpper() == abbreviation.ToUpper()).Id;
        }

        //================================================================================

        public void removeTransactionItemPrices(DataContainer dbContext, int transactionItemId, int userId, out string message)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                    "[Inventory].[RemoveTransactionItemPrices] @UserId=@UserId, @TransactionItemId=@TransactionItemId,@Message=@Message OUT",
                                   new SqlParameter("@UserId", userId),
                                   new SqlParameter("@TransactionItemId", transactionItemId),
                                   messageParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("RemoveTransactionItemPrices", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("RemoveTransactionItemPrices", message);
        }

        public void removeTransactionItemPrice(DataContainer dbContext, int transactionItemId, int transactionItemPriceId, int userId, out string message)
        {
            var messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 4096, ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Default, "");

            try
            {
                dbContext.Database.ExecuteSqlCommand(
                    "[Inventory].[RemoveTransactionItemPrice] @TransactionItemId=@TransactionItemId,@TransactionItemPriceId=@TransactionItemPriceId,@UserId=@UserId,@Message=@Message OUT",
                            new SqlParameter("@TransactionItemId", transactionItemId),
                            new SqlParameter("@TransactionItemPriceId", transactionItemPriceId),
                            new SqlParameter("@UserId", userId),
                            messageParameter);
            }
            catch (Exception ex)
            {
                throw new InvalidOperation("RemoveTransactionItemPrice", string.IsNullOrWhiteSpace(messageParameter.Value as string) ? ex.Message : messageParameter.Value as string);
            }

            message = messageParameter.Value as string;

            if (message != OPERATION_SUCCESSFUL_MESSAGE)
                throw new InvalidOperation("RemoveTransactionItemPrice", message);
        }

        //================================================================================

        public int getTimeBucketId(DataContainer dbContext, DateTime dateTime)
        {
            var refinedEndDateSearch = dateTime.AddMilliseconds(-dateTime.Millisecond);

            return dbContext.Inventory_TimeBucket.Single(tb => tb.IsActive.Value &&
                (tb.StartDate <= dateTime) &&
                tb.EndDate >= refinedEndDateSearch).Id;
        }


        //================================================================================

        public long getSharedGoodId(DataContainer dbContext, long fuelGoodId)
        {
            return dbContext.Goods.Single(g => g.Id == fuelGoodId).SharedGoodId;
        }


        //================================================================================

        #endregion

        //================================================================================

        public InventoryOperation ManageFuelReportConsumption(FuelReport fuelReport, Dictionary<long, decimal> goodsConsumption, int userId)
        {
            if (!(fuelReport.FuelReportType == FuelReportTypes.EndOfVoyage || fuelReport.IsEndOfYearReport()))
                throw new InvalidArgument("The given entity is not EOV, EOM, EOY.", "fuelReport");

            using (var dbContext = new DataContainer())
            {
                var sharedGoodIds = goodsConsumption.Select(kv => getSharedGoodId(dbContext, kv.Key)).ToList();

                foreach (var fuelReportDetail in fuelReport.FuelReportDetails)
                {
                    var inventoryQuantity = getInventoryQuantity(dbContext, fuelReportDetail.Good.SharedGoodId, fuelReportDetail.MeasuringUnit.Abbreviation, fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id, null);

                    if (inventoryQuantity - goodsConsumption[fuelReportDetail.GoodId] < 0)
                        throw new BusinessRuleException("", "Consumption value causes inconsistency in Inventory for Good " + fuelReportDetail.Good.Code);
                }

                var issuesWithFIFOPricingAfterCurrentEntity = findIssuesWithFIFOPricingAfterGivenDateTime(dbContext, fuelReport.EventDate, fuelReport.VesselInCompany.VesselInInventory.Id, sharedGoodIds);

                if (issuesWithFIFOPricingAfterCurrentEntity.Count(t => t.Status == (byte)TransactionState.FullPriced || t.Status == (byte)TransactionState.Vouchered) > 0)
                    throw new BusinessRuleException("", "There are some Issues with FIFO pricing in Inventory that should be reverted.");

                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    InventoryOperation result = null;

                    //TODO: EOV-EOM-EOY
                    #region EOV-EOM-EOY

                    //var goodsConsumption = new Dictionary<long, decimal>();

                    //foreach (var detail in fuelReport.FuelReportDetails)
                    //{
                    //    var consumption = calculateConsumption(detail);

                    //    goodsConsumption.Add(detail.GoodId, consumption);
                    //}

                    var transactionReferenceNumber = fuelReport.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION;

                    string transactionCode;
                    string transactionMessage;

                    var operationReference = issue(
                                dbContext,
                                (int)fuelReport.VesselInCompany.CompanyId,
                                (int)fuelReport.VesselInCompany.VesselInInventory.Id,
                                getTimeBucketId(dbContext, fuelReport.EventDate),
                                fuelReport.EventDate,
                                convertFuelReportConsumptionTypeToStoreType(dbContext, fuelReport),
                                null,
                            null,
                            transactionReferenceType,
                                transactionReferenceNumber,
                                userId,
                                out transactionCode,
                                out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();

                    foreach (var fuelReportDetail in fuelReport.FuelReportDetails)
                    {
                        transactionItem.Add(new Inventory_TransactionItem()
                        {
                            GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                            CreateDate = DateTime.Now,
                            Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                            QuantityAmount = goodsConsumption[fuelReportDetail.GoodId],
                            QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                            TransactionId = (int)operationReference.OperationId,
                            UserCreatorId = userId
                        });
                    }

                    var registeredTransactionIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    string issuedItemsPricingMessage;

                    var pricingTransactionIds = registeredTransactionIds.Select(id => new Inventory_TransactionItemPricingId() { Id = id, Description = transactionReferenceType + " FIFO Pricing" });

                    try
                    {
                        priceIssuedItemsInFIFO(dbContext, pricingTransactionIds, userId, out issuedItemsPricingMessage, transactionReferenceType, transactionReferenceNumber);
                    }
                    catch
                    {
                    }

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result = createInventoryOperationResult(createdTransaction);

                    #endregion

                    //transaction.Commit();

                    return result;
                }
            }
        }

        /// <summary>
        /// This method is used to create the result of an inventory operation to be refenced by FuelManagment related parts.
        /// </summary>
        /// <param name="registeredTransaction"></param>
        /// <returns></returns>
        private static InventoryOperation createInventoryOperationResult(Inventory_Transaction registeredTransaction)
        {
            InventoryActionType inventoryActionType;

            if ((TransactionType)registeredTransaction.Action == TransactionType.Issue)
            {
                inventoryActionType = registeredTransaction.AdjustmentForTransactionId.HasValue ? InventoryActionType.AdjustmentIssue : InventoryActionType.Issue;
            }
            else if ((TransactionType)registeredTransaction.Action == TransactionType.Receipt)
            {
                inventoryActionType = registeredTransaction.AdjustmentForTransactionId.HasValue ? InventoryActionType.AdjustmentReceipt : InventoryActionType.Receipt;
            }
            else
            {
                throw new InvalidOperation("Transaction Registration", "Invlaid inventory transaction is created.");
            }

            InventoryOperation result = new InventoryOperation(
                inventoryOperationId: registeredTransaction.Id,
                actionNumber: registeredTransaction.GetActionNumber(),
                                       actionDate: DateTime.Now,
                actionType: inventoryActionType);

            return result;
        }

        private List<Inventory_OperationReference> getFuelReportConsumptionReference(DataContainer dbContext, FuelReport fuelReport)
        {
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION, fuelReport.GetInventoryTransactionReferenceNumber());
            return references;
        }

        public Inventory_OperationReference GetFuelReportConsumptionReference(FuelReport fuelReport)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getFuelReportConsumptionReference(dbContext, fuelReport).FirstOrDefault();
            }
        }

        public List<InventoryOperation> CorrectTransaction(Inventory_OperationReference reference, long? pricingReferenceId, Dictionary<long, List<GoodQuantity>> entityGoodsQuantities, Dictionary<long, List<GoodQuantityPricing>> entityGoodsQuantitiesWithPrices, int userId)
        {
            List<InventoryOperation> result = new List<InventoryOperation>();

            using (DataContainer dbContext = new DataContainer())
            {
                var originalTransaction = dbContext.Inventory_Transaction.AsNoTracking().Single(t => t.Id == reference.OperationId);

                var registeredTransactionFinalValues = calculateTransactionQuantitiesValues(originalTransaction);
                var registeredTransactionFinalValuesWithPrices = calculateTransactionQuantitiesValuesWithPrices(originalTransaction);

                //Dictionary<long, decimal> goodsPriceDifference;  //Commented - Previous Implementation
                Dictionary<long, decimal> goodsQuantityChange;

                var voucherRepository = ServiceLocator.Current.GetInstance<IVoucherRepository>();
                var transationDomainService = ServiceLocator.Current.GetInstance<IInventoryTransactionDomainService>();

                #region Commented - Previous Implementation
                /*if (originalTransaction.Action == (byte)InventoryOperationType.Receipt &&
                    !originalTransaction.PricingReferenceId.HasValue &&
                    this.hasChangedPrice(registeredTransactionFinalValuesWithPrices, entityGoodsQuantitiesWithPrices, out goodsPriceDifference))
                {
                    if (this.hasChangedQuantity(registeredTransactionFinalValues, entityGoodsQuantities, (InventoryOperationType)originalTransaction.Action, out goodsQuantityChange))
                        //This situation is the same as changing Quantity and Price at the same time and should be prevented by raising error.
                        throw new BusinessRuleException("", "Changing quantity and price together is not allowed.");

                    bool hasAnyLockedVoucherInFinancialSystem;
                    removeVouchers(dbContext, originalTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                    originalTransaction.Inventory_TransactionItem.Where(ti => goodsPriceDifference.ContainsKey(ti.GoodId)).ToList().ForEach(ti =>
                    {
                        var pricesOperations = dbContext.Inventory_OperationReference.Where(r => ti.Inventory_TransactionItemPrice.Select(p => (long)p.Id).Contains(r.OperationId) && r.OperationType == (int)InventoryOperationType.Pricing);

                        string message;
                        removeTransactionItemPrices(dbContext, ti.Id, userId, out message);

                        if (entityGoodsQuantitiesWithPrices.ContainsKey(ti.GoodId))
                        {
                            if (entityGoodsQuantitiesWithPrices[ti.GoodId].Count > 1)
                            {
                                throw new BusinessRuleException("", "Invalid direct pricing for Inventory Receipt.");
                            }

                            var transactionItemPrice = new Inventory_TransactionItemPrice()
                            {
                                TransactionItemId = ti.Id,
                                QuantityUnitId = entityGoodsQuantitiesWithPrices[ti.GoodId].First().InventoryQuantityUnitId,
                                QuantityAmount = entityGoodsQuantitiesWithPrices[ti.GoodId].First().UnsignedQuantity,
                                PriceUnitId = entityGoodsQuantitiesWithPrices[ti.GoodId].First().FeeInventoryCurrencyUnitId,
                                Fee = entityGoodsQuantitiesWithPrices[ti.GoodId].First().Fee,
                                RegistrationDate = DateTime.Now,
                                Description = entityGoodsQuantitiesWithPrices[ti.GoodId].First().Description,
                                UserCreatorId = userId
                            };

                            string pricingMessage;

                            priceTransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage,
                                pricesOperations.First().ReferenceType, originalTransaction.ReferenceNo);
                        }
                    });

                    updateContext(dbContext);

                    var newPricedTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);

                    //hasAnyLockedVoucherInFinancialSystem = true; //For Test purposes

                    if (hasAnyLockedVoucherInFinancialSystem)
                    {
                        var transactionFinalValuesWithPricesAfterPricing = calculateTransactionQuantitiesValuesWithPrices(newPricedTransaction);

                        createAdjustmentVoucher(originalTransaction, newPricedTransaction, transactionFinalValuesWithPricesAfterPricing);
                    }
                    else
                    {
                        transationDomainService.CreateVoucherForTransaction(newPricedTransaction, userId);
                    }

                    this.reviseTransactionsPricing(dbContext, originalTransaction, goodsPriceDifference.Keys.ToList(), userId, voucherRepository, transationDomainService);
                }
                else */

                #endregion

                if (this.hasChangedQuantity(registeredTransactionFinalValues, entityGoodsQuantities, (InventoryOperationType)originalTransaction.Action, out goodsQuantityChange))
                {
                    bool hasAnyLockedVoucherInFinancialSystem;
                    removeVouchers(dbContext, originalTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                    var goodsInventoryQuantity = getInventoryStock(dbContext, originalTransaction.WarehouseId, entityGoodsQuantities);

                    checkGoodsInventoryQuantity(goodsInventoryQuantity, goodsQuantityChange);

                    //hasAnyLockedVoucherInFinancialSystem = true; //For Test purposes

                    if (hasAnyLockedVoucherInFinancialSystem)
                    {
                        var inventoryFinalEffectiveGoodsQuantitiesPrices = calculateTransactionFinalEffectiveValuesWithPrices(originalTransaction, registeredTransactionFinalValuesWithPrices);

                        List<Inventory_Transaction> adjustmentTransactions;
                        var inventoryOperations = this.createInventoryAdjustmentTransaction(dbContext, originalTransaction, goodsQuantityChange,
                            //originalTransaction.IsIssueWithFIFOPricing() ? null :   //A.H Commented due to the handling original FIFO pricing inside the method.
                            inventoryFinalEffectiveGoodsQuantitiesPrices, userId, out adjustmentTransactions, voucherRepository, transationDomainService);

                        result.AddRange(inventoryOperations);

                        createAdjustmentVouchers(adjustmentTransactions);
                    }
                    else
                    {
                        var goodIdList = goodsQuantityChange.Keys.ToList();
                        var revisingTransactions = findTransactionsForPricingRevision(dbContext, originalTransaction, goodIdList);

                        removeTransactionsPrices(dbContext, revisingTransactions, goodIdList, userId, voucherRepository);

                        updateContext(dbContext);

                        var updatingTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);
                        updatingTransaction.PricingReferenceId = (int?)pricingReferenceId;
                        dbContext.SaveChanges();

                        originalTransaction.Inventory_TransactionItem.Where(ti => goodsQuantityChange.ContainsKey(ti.GoodId)).ToList().ForEach(ti =>
                        {
                            //var pricesOperations = dbContext.Inventory_OperationReference.Where(r => ti.Inventory_TransactionItemPrice.Select(p => (long)p.Id).Contains(r.OperationId) && r.OperationType == (int)InventoryOperationType.Pricing);

                            string message;
                            removeTransactionItemPrices(dbContext, ti.Id, userId, out message);

                            updateContext(dbContext);

                            updateTransactionItemQuantity(dbContext, entityGoodsQuantities, ti.Id);

                            string pricingMessage;

                            if (updatingTransaction.IsIssueWithFIFOPricing())
                            {
                                try
                                {
                                    priceIssuedItemInFIFO(dbContext,
                                        ti.Id,
                                        //ti.Inventory_TransactionItemPrice.Count == 0 ? string.Empty : ti.Inventory_TransactionItemPrice.First().Description,
                                        originalTransaction.ReferenceType + " FIFO Pricing",
                                        userId,
                                        out pricingMessage,
                                        originalTransaction.ReferenceType,
                                        originalTransaction.ReferenceNo);
                                }
                                catch
                                {
                                    // Should be ignored.
                                }
                            }
                            else
                            {
                                priceSuspendedTransactionUsingReference(dbContext,
                                    ti.TransactionId,
                                    //ti.Inventory_TransactionItemPrice.Count == 0 ? string.Empty : ti.Inventory_TransactionItemPrice.First().Description,
                                    originalTransaction.ReferenceType + " Pricing by Reference",
                                    null,
                                    userId,
                                    out pricingMessage,
                                    originalTransaction.ReferenceType,
                                    originalTransaction.ReferenceNo);
                            }
                        });

                        var newPricedTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);

                        transationDomainService.CreateVoucherForTransaction(newPricedTransaction, userId);
                    }

                    updateContext(dbContext);

                    this.reviseTransactionsPricing(dbContext, originalTransaction, goodsQuantityChange.Keys.ToList(), userId, voucherRepository, transationDomainService);
                }
            }

            return result;
        }

        public void CorrectReceiptTransactionPricing(Inventory_OperationReference reference, Dictionary<long, List<GoodQuantity>> entityGoodsQuantities, Dictionary<long, List<GoodQuantityPricing>> entityGoodsQuantitiesWithPrices, int userId)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                var originalTransaction = dbContext.Inventory_Transaction.AsNoTracking().Single(t => t.Id == reference.OperationId);

                var registeredTransactionFinalValues = calculateTransactionQuantitiesValues(originalTransaction);
                var registeredTransactionFinalValuesWithPrices = calculateTransactionQuantitiesValuesWithPrices(originalTransaction);

                Dictionary<long, decimal> goodsPriceDifference;
                Dictionary<long, decimal> goodsQuantityChange;

                var voucherRepository = ServiceLocator.Current.GetInstance<IVoucherRepository>();
                var transationDomainService = ServiceLocator.Current.GetInstance<IInventoryTransactionDomainService>();


                if (originalTransaction.Action == (byte)InventoryOperationType.Receipt &&
                    !originalTransaction.PricingReferenceId.HasValue &&
                    this.hasChangedPrice(registeredTransactionFinalValuesWithPrices, entityGoodsQuantitiesWithPrices, out goodsPriceDifference))
                {
                    if (this.hasChangedQuantity(registeredTransactionFinalValues, entityGoodsQuantities, (InventoryOperationType)originalTransaction.Action, out goodsQuantityChange))
                        //This situation is the same as changing Quantity and Price at the same time and should be prevented by raising error.
                        throw new BusinessRuleException("", "Changing quantity and price together is not allowed.");

                    bool hasAnyLockedVoucherInFinancialSystem;
                    removeVouchers(dbContext, originalTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                    var goodIdList = goodsPriceDifference.Keys.ToList();
                    var revisingTransactions = findTransactionsForPricingRevision(dbContext, originalTransaction, goodIdList);

                    removeTransactionsPrices(dbContext, revisingTransactions, goodIdList, userId, voucherRepository);

                    updateContext(dbContext);

                    originalTransaction.Inventory_TransactionItem.Where(ti => goodsPriceDifference.ContainsKey(ti.GoodId)).ToList().ForEach(ti =>
                    {
                        var pricesOperations = dbContext.Inventory_OperationReference.Where(r => ti.Inventory_TransactionItemPrice.Select(p => (long)p.Id).Contains(r.OperationId) && r.OperationType == (int)InventoryOperationType.Pricing);

                        string message;
                        removeTransactionItemPrices(dbContext, ti.Id, userId, out message);

                        if (entityGoodsQuantitiesWithPrices.ContainsKey(ti.GoodId))
                        {
                            if (entityGoodsQuantitiesWithPrices[ti.GoodId].Count > 1)
                            {
                                throw new BusinessRuleException("", "Invalid direct pricing for Inventory Receipt.");
                            }

                            var transactionItemPrice = new Inventory_TransactionItemPrice()
                            {
                                TransactionItemId = ti.Id,
                                QuantityUnitId = entityGoodsQuantitiesWithPrices[ti.GoodId].First().InventoryQuantityUnitId,
                                QuantityAmount = entityGoodsQuantitiesWithPrices[ti.GoodId].First().UnsignedQuantity,
                                PriceUnitId = entityGoodsQuantitiesWithPrices[ti.GoodId].First().FeeInventoryCurrencyUnitId,
                                Fee = entityGoodsQuantitiesWithPrices[ti.GoodId].First().Fee,
                                RegistrationDate = originalTransaction.RegistrationDate,
                                Description = entityGoodsQuantitiesWithPrices[ti.GoodId].First().Description,
                                UserCreatorId = userId
                            };

                            string pricingMessage;

                            priceTransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage,
                                originalTransaction.ReferenceType, originalTransaction.ReferenceNo);
                        }
                    });

                    updateContext(dbContext);

                    var newPricedTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);

                    //hasAnyLockedVoucherInFinancialSystem = true; //For Test purposes

                    if (hasAnyLockedVoucherInFinancialSystem)
                    {
                        var transactionFinalValuesWithPricesAfterPricing = calculateTransactionQuantitiesValuesWithPrices(newPricedTransaction);

                        createAdjustmentVoucher(originalTransaction, newPricedTransaction, transactionFinalValuesWithPricesAfterPricing);
                    }
                    else
                    {
                        transationDomainService.CreateVoucherForTransaction(newPricedTransaction, userId);
                    }

                    this.reviseTransactionsPricing(dbContext, originalTransaction, goodsPriceDifference.Keys.ToList(), userId, voucherRepository, transationDomainService);
                }
            }
        }


        public InventoryOperationResult RevertTransaction(Inventory_OperationReference reference, int userId)
        {
            var result = new InventoryOperationResult();

            using (DataContainer dbContext = new DataContainer())
            {
                var originalTransaction = dbContext.Inventory_Transaction.AsNoTracking().Single(t => t.Id == reference.OperationId);

                var registeredTransactionFinalValues = calculateTransactionQuantitiesValues(originalTransaction);
                var registeredTransactionFinalValuesWithPrices = calculateTransactionQuantitiesValuesWithPrices(originalTransaction);

                var voucherRepository = ServiceLocator.Current.GetInstance<IVoucherRepository>();
                var transationDomainService = ServiceLocator.Current.GetInstance<IInventoryTransactionDomainService>();

                var goodsInventoryQuantity = getInventoryStock(dbContext, originalTransaction.WarehouseId, registeredTransactionFinalValues);

                var changingQuantities = registeredTransactionFinalValues.Select(fv => new { fv.Key, Value = fv.Value.Sum(v => v.SignedQuantity) * -1 }).ToDictionary(e => e.Key, e => e.Value);

                checkGoodsInventoryQuantity(goodsInventoryQuantity, changingQuantities);

                bool hasAnyLockedVoucherInFinancialSystem;
                removeVouchers(dbContext, originalTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                //hasAnyLockedVoucherInFinancialSystem = true; //For Test purposes

                if (hasAnyLockedVoucherInFinancialSystem)
                {
                    var inventoryFinalEffectiveGoodsQuantitiesPrices = calculateTransactionFinalEffectiveValuesWithPrices(originalTransaction, registeredTransactionFinalValuesWithPrices);

                    Dictionary<long, decimal> goodsQuantityChangeToRevert = new Dictionary<long, decimal>();

                    var processingTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);

                    foreach (var goodId in registeredTransactionFinalValues.Keys)
                    {
                        goodsQuantityChangeToRevert.Add(goodId, registeredTransactionFinalValues[goodId].Sum(v => v.SignedQuantity) * -1);

                        if (!inventoryFinalEffectiveGoodsQuantitiesPrices.ContainsKey(goodId))
                        {
                            //At this point, the quantities for goods with no pricing, should be deleted from transaction.
                            dbContext.Inventory_TransactionItem.Remove(processingTransaction.Inventory_TransactionItem.FirstOrDefault(ti => ti.GoodId == goodId));
                            dbContext.SaveChanges();
                        }
                    }

                    if (processingTransaction.Inventory_TransactionItem.Count == 0)
                    {
                        dbContext.Inventory_Transaction.Remove(processingTransaction);

                        var referenceToDelete = dbContext.Inventory_OperationReference.Single(p => p.Id == reference.Id);
                        dbContext.Inventory_OperationReference.Remove(referenceToDelete);

                        dbContext.SaveChanges();

                        result.RemovedTransactionIds.Add(originalTransaction.Id);
                    }
                    else
                    {
                        List<Inventory_Transaction> adjustmentTransactions;

                        var inventoryOperations = this.createInventoryAdjustmentTransaction(dbContext, originalTransaction, goodsQuantityChangeToRevert,
                            //originalTransaction.IsIssueWithFIFOPricing() ? null :   //A.H Commented due to the handling original FIFO pricing inside the method.
                            inventoryFinalEffectiveGoodsQuantitiesPrices, userId, out adjustmentTransactions, voucherRepository, transationDomainService);

                        result.CreatedInventoryOperations.AddRange(inventoryOperations);

                        createAdjustmentVouchers(adjustmentTransactions);
                    }

                }
                else
                {
                    originalTransaction.Inventory_TransactionItem.ToList().ForEach(ti =>
                    {
                        string message;
                        removeTransactionItemPrices(dbContext, ti.Id, userId, out message);
                    });

                    updateContext(dbContext);

                    var transactionToDelete = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);

                    transactionToDelete.Inventory_TransactionItem.ToList().ForEach(ti => dbContext.Inventory_TransactionItem.Remove(ti));

                    dbContext.Inventory_Transaction.Remove(transactionToDelete);

                    var referenceToDelete = dbContext.Inventory_OperationReference.Single(p => p.Id == reference.Id);
                    dbContext.Inventory_OperationReference.Remove(referenceToDelete);

                    result.RemovedTransactionIds.Add(originalTransaction.Id);

                    dbContext.SaveChanges();
                }

                this.reviseTransactionsPricing(dbContext, originalTransaction, originalTransaction.Inventory_TransactionItem.Select(ti => ti.GoodId).ToList(), userId, voucherRepository, transationDomainService);

                dbContext.SaveChanges();
            }

            return result;
        }

        private void reviseTransactionsPricing(DataContainer dbContext, Inventory_Transaction baseTransaction, List<long> goodIdList, int userId, IVoucherRepository voucherRepository, IInventoryTransactionDomainService transationDomainService)
        {
            var revisingTransactions = findTransactionsForPricingRevision(dbContext, baseTransaction, goodIdList);

            reviseTransactionsPrices(dbContext, revisingTransactions, goodIdList, userId, voucherRepository, transationDomainService);
        }

        private void reviseTransactionsPrices(DataContainer dbContext, List<Inventory_Transaction> revisingTransactions, List<long> goodIdList, int userId, IVoucherRepository voucherRepository, IInventoryTransactionDomainService transationDomainService)
        {
            var sortedList = revisingTransactions.OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate);

            foreach (var updatingTransaction in sortedList)
            {
                var relatedTransactionsToCurrentTransaction = getIssuesWithGivenTransactionAsPricingRefrence(dbContext, updatingTransaction.Id, updatingTransaction.WarehouseId);
                relatedTransactionsToCurrentTransaction.AddRange(getRecieptsWithGivenTransactionAsPricingRefrence(dbContext, updatingTransaction.Id, updatingTransaction.WarehouseId));

                relatedTransactionsToCurrentTransaction = relatedTransactionsToCurrentTransaction.OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate).ToList();

                var transactionFinalValuesWithPricesBeforePricing = calculateTransactionQuantitiesValuesWithPrices(updatingTransaction);

                bool hasAnyLockedVoucherInFinancialSystem;
                removeVouchers(dbContext, updatingTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                updatingTransaction.Inventory_TransactionItem.Where(ti => goodIdList.Contains(ti.GoodId)).ToList().ForEach(ti =>
                {
                    //var pricesOperations = dbContext.Inventory_OperationReference.Where(r => ti.Inventory_TransactionItemPrice.Select(p => (long)p.Id).Contains(r.OperationId) && r.OperationType == (int)InventoryOperationType.Pricing);

                    string message;
                    removeTransactionItemPrices(dbContext, ti.Id, userId, out message);

                    updateContext(dbContext);

                    string pricingMessage;

                    if (updatingTransaction.IsIssueWithFIFOPricing())
                    {
                        try
                        {
                            priceIssuedItemInFIFO(dbContext,
                                ti.Id,
                                //ti.Inventory_TransactionItemPrice.Count == 0 ? string.Empty : ti.Inventory_TransactionItemPrice.First().Description,
                                updatingTransaction.ReferenceType + " FIFO Pricing",
                                userId,
                                out pricingMessage,
                                updatingTransaction.ReferenceType,
                                updatingTransaction.ReferenceNo);
                        }
                        catch (Exception)
                        {
                            //Should be ignored.
                        }

                    }
                    else
                    {
                        priceSuspendedTransactionUsingReference(dbContext,
                            ti.TransactionId,
                            //ti.Inventory_TransactionItemPrice.Count == 0 ? string.Empty : ti.Inventory_TransactionItemPrice.First().Description,
                            updatingTransaction.ReferenceType + " Pricing by Reference",
                            null,
                            userId,
                            out pricingMessage,
                            updatingTransaction.ReferenceType,
                            updatingTransaction.ReferenceNo);
                    }
                });

                updateContext(dbContext);

                var newPricedTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == updatingTransaction.Id);

                var transactionFinalValuesWithPricesAfterPricing = calculateTransactionQuantitiesValuesWithPrices(newPricedTransaction);

                //Dictionary<long, decimal> goodsPriceDifference;
                //if (this.hasChangedPrice(transactionFinalValuesWithPricesBeforePricing, transactionFinalValuesWithPricesAfterPricing, out goodsPriceDifference))
                //{
                if (hasAnyLockedVoucherInFinancialSystem)
                {
                    createAdjustmentVoucher(updatingTransaction, newPricedTransaction, transactionFinalValuesWithPricesAfterPricing);
                }
                else
                {
                    transationDomainService.CreateVoucherForTransaction(newPricedTransaction, userId);
                }
                //}

                this.reviseTransactionsPrices(dbContext, relatedTransactionsToCurrentTransaction, goodIdList, userId, voucherRepository, transationDomainService);
            }
        }

        private void removeTransactionsPrices(DataContainer dbContext, List<Inventory_Transaction> revisingTransactions, List<long> goodIdList, int userId, IVoucherRepository voucherRepository)
        {
            var sortedList = revisingTransactions.OrderByDescending(t => t.Code).ThenByDescending(t => t.RegistrationDate);

            foreach (var updatingTransaction in sortedList)
            {
                var relatedTransactionsToCurrentTransaction = getIssuesWithGivenTransactionAsPricingRefrence(dbContext, updatingTransaction.Id, updatingTransaction.WarehouseId);
                relatedTransactionsToCurrentTransaction.AddRange(getRecieptsWithGivenTransactionAsPricingRefrence(dbContext, updatingTransaction.Id, updatingTransaction.WarehouseId));

                relatedTransactionsToCurrentTransaction = relatedTransactionsToCurrentTransaction.OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate).ToList();

                removeTransactionsPrices(dbContext, relatedTransactionsToCurrentTransaction, goodIdList, userId, voucherRepository);

                updateContext(dbContext);

                //var transactionFinalValuesWithPricesBeforePricing = calculateTransactionQuantitiesValuesWithPrices(updatingTransaction);

                bool hasAnyLockedVoucherInFinancialSystem;
                removeVouchers(dbContext, updatingTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                updatingTransaction.Inventory_TransactionItem.Where(ti => goodIdList.Contains(ti.GoodId)).ToList().ForEach(ti =>
                {
                    //var pricesOperations = dbContext.Inventory_OperationReference.Where(r => ti.Inventory_TransactionItemPrice.Select(p => (long)p.Id).Contains(r.OperationId) && r.OperationType == (int)InventoryOperationType.Pricing);

                    string message;
                    removeTransactionItemPrices(dbContext, ti.Id, userId, out message);

                });

                updateContext(dbContext);
            }
        }

        public void RevertTransactionPricing(int inventoryTransactionItemPriceId, int userId)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                var originalItemPrice = dbContext.Inventory_TransactionItemPrice.AsNoTracking().Single(p => p.Id == inventoryTransactionItemPriceId);
                var originalTransaction = originalItemPrice.Inventory_TransactionItem.Inventory_Transaction;

                var voucherRepository = ServiceLocator.Current.GetInstance<IVoucherRepository>();
                var transationDomainService = ServiceLocator.Current.GetInstance<IInventoryTransactionDomainService>();

                bool hasAnyLockedVoucherInFinancialSystem;
                removeVouchers(dbContext, originalTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                var goodIdList = new List<long>() { originalItemPrice.Inventory_TransactionItem.GoodId };
                var revisingTransactions = findTransactionsForPricingRevision(dbContext, originalTransaction, goodIdList);

                removeTransactionsPrices(dbContext, revisingTransactions, goodIdList, userId, voucherRepository);

                updateContext(dbContext);

                string message;
                removeTransactionItemPrice(dbContext, originalItemPrice.TransactionItemId, originalItemPrice.Id, userId, out message);

                updateContext(dbContext);

                var newPricedTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalItemPrice.TransactionId);

                //hasAnyLockedVoucherInFinancialSystem = true; //For Test purposes

                if (hasAnyLockedVoucherInFinancialSystem)
                {
                    var transactionFinalValuesWithPricesAfterPricing = calculateTransactionQuantitiesValuesWithPrices(newPricedTransaction);

                    createAdjustmentVoucher(originalTransaction, newPricedTransaction, transactionFinalValuesWithPricesAfterPricing);
                }

                if ((originalTransaction.ReferenceType == InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION) ||
                    (originalTransaction.ReferenceType == InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION))
                {
                    newPricedTransaction.PricingReferenceId = null;  //The pricing reference for Incremental/Decremental Corrections should be reset, as this value for these types of correction will be set upon pricing, if applicable.

                    dbContext.SaveChanges();
                }
            }
        }

        public void CorrectReceiptTransactionPricing(int inventoryTransactionItemPriceId, GoodQuantityPricing goodQuantityPricing, int userId)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                var originalItemPrice = dbContext.Inventory_TransactionItemPrice.AsNoTracking().Single(p => p.Id == inventoryTransactionItemPriceId);
                var originalTransaction = originalItemPrice.Inventory_TransactionItem.Inventory_Transaction;

                var voucherRepository = ServiceLocator.Current.GetInstance<IVoucherRepository>();
                var transationDomainService = ServiceLocator.Current.GetInstance<IInventoryTransactionDomainService>();

                bool hasAnyLockedVoucherInFinancialSystem;
                removeVouchers(dbContext, originalTransaction, voucherRepository, out hasAnyLockedVoucherInFinancialSystem);

                var goodIdList = new List<long>() { originalItemPrice.Inventory_TransactionItem.GoodId };
                var revisingTransactions = findTransactionsForPricingRevision(dbContext, originalTransaction, goodIdList);

                removeTransactionsPrices(dbContext, revisingTransactions, goodIdList, userId, voucherRepository);

                updateContext(dbContext);

                string message;
                removeTransactionItemPrice(dbContext, originalItemPrice.TransactionItemId, originalItemPrice.Id, userId, out message);

                //updateContext(dbContext);

                var transactionItemPrice = new Inventory_TransactionItemPrice()
                {
                    TransactionItemId = originalItemPrice.TransactionItemId,
                    QuantityUnitId = goodQuantityPricing.InventoryQuantityUnitId,
                    QuantityAmount = goodQuantityPricing.UnsignedQuantity,
                    PriceUnitId = goodQuantityPricing.FeeInventoryCurrencyUnitId,
                    Fee = goodQuantityPricing.Fee,
                    RegistrationDate = originalTransaction.RegistrationDate,
                    Description = goodQuantityPricing.Description,
                    UserCreatorId = userId
                };

                string pricingMessage;

                priceTransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage,
                    goodQuantityPricing.PricingReferenceType, goodQuantityPricing.PricingReferenceNumber);

                updateContext(dbContext);

                var newPricedTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);

                //hasAnyLockedVoucherInFinancialSystem = true; //For Test purposes

                if (hasAnyLockedVoucherInFinancialSystem)
                {
                    var transactionFinalValuesWithPricesAfterPricing = calculateTransactionQuantitiesValuesWithPrices(newPricedTransaction);

                    createAdjustmentVoucher(originalTransaction, newPricedTransaction, transactionFinalValuesWithPricesAfterPricing);
                }
                else
                {
                    transationDomainService.CreateVoucherForTransaction(newPricedTransaction, userId);
                }

                this.reviseTransactionsPricing(dbContext, originalTransaction, goodIdList, userId, voucherRepository, transationDomainService);
            }
        }

        private void createAdjustmentVoucher(Inventory_Transaction updatingTransaction, Inventory_Transaction newPricedTransaction, Dictionary<long, List<GoodQuantityPricing>> transactionFinalValuesWithPricesAfterPricing)
        {
            //It should be considered that if the new transaction is not fully priced, the WHOLE previous sent voucher must be reverted.
            return; //TODO Adjustment Vouchers implmenetation.


            //1. Find current registered vouchers total price.
            //2. Register adjustment voucher for difference between current registerred vouchers (i.e. any sent voucher : step 2) and final priced quantity in new transaction.


            updatingTransaction.GetActionNumber();

            Voucher voucherToSend = new Voucher();

            sendToFinance(voucherToSend);

        }

        private void sendToFinance(Voucher voucher)
        {
            throw new NotImplementedException();
        }

        private void createAdjustmentVouchers(List<Inventory_Transaction> adjustmentTransactions)
        {
            return; //TODO Adjustment Vouchers implmenetation.

            foreach (var adjustmentTransaction in adjustmentTransactions)
            {
                //Create proper adjustment voucher for given adjustment transaction.

                Voucher voucherToSend = new Voucher();

                sendToFinance(voucherToSend);
            }
        }

        private void updateTransactionItemQuantity(DataContainer dbContext, Dictionary<long, List<GoodQuantity>> entityGoodsQuantities, int transactionItemId)
        {
            updateContext(dbContext);

            var transactionItem = dbContext.Inventory_TransactionItem.Single(item => item.Id == transactionItemId);

            transactionItem.QuantityAmount = Math.Abs(entityGoodsQuantities[transactionItem.GoodId].Sum(q => q.SignedQuantity));

            dbContext.SaveChanges();
        }

        private List<InventoryOperation> createInventoryAdjustmentTransaction(DataContainer dbContext, Inventory_Transaction originalTransaction, Dictionary<long, decimal> goodsQuantityChange, Dictionary<long, List<GoodQuantityPricing>> inventoryFinalEffectiveGoodsQuantitiesPrices, int userId, out List<Inventory_Transaction> adjustmentTransactions, IVoucherRepository voucherRepository, IInventoryTransactionDomainService transationDomainService)
        {
            var result = new List<InventoryOperation>();

            adjustmentTransactions = new List<Inventory_Transaction>();

            foreach (var goodId in goodsQuantityChange.Keys)
            {
                if (goodsQuantityChange[goodId] < 0)
                {
                    if (originalTransaction.IsIssueWithFIFOPricing())
                    {
                        var goodIdList = goodsQuantityChange.Keys.ToList();

                        var transactionsWithFIFOPricingAfterCurrentFIFOTransaction = getIssuesWithFIFOPricingAfterGivenTransaction(dbContext, originalTransaction, goodIdList);

                        //The prices of FIFO priced transactions shhould be removed before creating adjustment for current FIFO issue transaction.
                        removeTransactionsPrices(dbContext, transactionsWithFIFOPricingAfterCurrentFIFOTransaction, goodIdList, userId, voucherRepository);

                        Inventory_Transaction createdTransaction;
                        result.Add(createIssueWithFIFOPricingAdjustment(dbContext, originalTransaction, goodId, goodsQuantityChange[goodId], inventoryFinalEffectiveGoodsQuantitiesPrices[goodId].First().InventoryQuantityUnitId, userId, out createdTransaction));
                        adjustmentTransactions.Add(createdTransaction);
                    }
                    else
                    {
                        var signedQuantityChange = goodsQuantityChange[goodId];

                        for (int index = inventoryFinalEffectiveGoodsQuantitiesPrices[goodId].Count - 1; index >= 0 || signedQuantityChange != 0; index--)
                        {
                            var quantityRevertReferenceInformation = inventoryFinalEffectiveGoodsQuantitiesPrices[goodId][index];
                            var quantityToRevert = Math.Min(Math.Abs(signedQuantityChange), Math.Abs(quantityRevertReferenceInformation.SignedQuantity));

                            Inventory_Transaction createdTransaction;
                            result.Add(this.createIssueWithReferencedPricingAdjustment(dbContext, originalTransaction, goodId, quantityToRevert, quantityRevertReferenceInformation, userId, out createdTransaction));
                            adjustmentTransactions.Add(createdTransaction);

                            signedQuantityChange = signedQuantityChange - (quantityToRevert * Math.Sign(signedQuantityChange));
                        }
                    }
                }
                else if (goodsQuantityChange[goodId] > 0)
                {
                    var signedQuantityChange = goodsQuantityChange[goodId];

                    for (int index = inventoryFinalEffectiveGoodsQuantitiesPrices[goodId].Count - 1; index >= 0 || signedQuantityChange != 0; index--)
                    {
                        var quantityRevertReferenceInformation = inventoryFinalEffectiveGoodsQuantitiesPrices[goodId][index];
                        var quantityToRevert = Math.Min(Math.Abs(signedQuantityChange), Math.Abs(quantityRevertReferenceInformation.SignedQuantity));

                        Inventory_Transaction createdTransaction;
                        result.Add(createReceiptWithReferencedPricingAdjustment(dbContext, originalTransaction, goodId, quantityToRevert, quantityRevertReferenceInformation, userId, out createdTransaction));
                        adjustmentTransactions.Add(createdTransaction);

                        signedQuantityChange = signedQuantityChange - (quantityToRevert * Math.Sign(signedQuantityChange));
                    }
                }
            }

            return result;
        }

        private InventoryOperation createReceiptWithReferencedPricingAdjustment(DataContainer dbContext, Inventory_Transaction originalTransaction, long goodId, decimal quantityToRevert, GoodQuantityPricing quantityRevertReferenceInformation, int userId, out Inventory_Transaction createdTransaction)
        {
            //var timeBucketId = getTimeBucketId(dbContext, quantityRevertReferenceInformation.DateTimeForRevertAdjustment);
            var timeBucketId = getTimeBucketId(dbContext, originalTransaction.RegistrationDate.Value);
            string code;
            string message;

            var operationReference = receipt(dbContext,
                (int)originalTransaction.Inventory_Warehouse.CompanyId,
                (int)originalTransaction.WarehouseId,
                timeBucketId,
                //quantityRevertReferenceInformation.DateTimeForRevertAdjustment,
                originalTransaction.RegistrationDate.Value,
                getAdjustmentReceiptType(dbContext),
                (int)quantityRevertReferenceInformation.PricingReferenceTransactionIdForRevertAdjustment,
                originalTransaction.Id,
                InventoryOperationReferenceTypes.INVENTORY_RECEIPT_ADJUSTMENT,
                originalTransaction.Id.ToString(),
                userId,
                out code,
                out message);

            var transactionItem = new List<Inventory_TransactionItem>();
            transactionItem.Add(new Inventory_TransactionItem()
            {
                GoodId = (int)goodId,
                CreateDate = DateTime.Now,
                Description = "Receipt Adjustment",
                QuantityAmount = quantityToRevert,
                QuantityUnitId = quantityRevertReferenceInformation.InventoryQuantityUnitId,
                TransactionId = (int)operationReference.OperationId,
                UserCreatorId = userId
            });

            string transactionItemMessage;
            var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

            string pricingMessage;

            priceSuspendedTransactionUsingReference(
                 dbContext,
                 (int)operationReference.OperationId,
                 InventoryOperationReferenceTypes.INVENTORY_RECEIPT_ADJUSTMENT + " Referenced Pricing",
                 quantityRevertReferenceInformation.DateTimeForRevertAdjustment,
                 userId,
                 out pricingMessage,
                 InventoryOperationReferenceTypes.INVENTORY_RECEIPT_ADJUSTMENT,
                 originalTransaction.Id.ToString());

            updateContext(dbContext);

            createdTransaction = this.GetTransaction(operationReference.OperationId, InventoryOperationType.Receipt);

            return createInventoryOperationResult(createdTransaction);
        }

        private InventoryOperation createIssueWithReferencedPricingAdjustment(DataContainer dbContext, Inventory_Transaction originalTransaction, long goodId, decimal quantityToRevert, GoodQuantityPricing quantityRevertReferenceInformation, int userId, out Inventory_Transaction createdTransaction)
        {
            //var timeBucketId = getTimeBucketId(dbContext, quantityRevertReferenceInformation.DateTimeForRevertAdjustment);
            var timeBucketId = getTimeBucketId(dbContext, originalTransaction.RegistrationDate.Value);
            string code;
            string message;

            var operationReference = issue(dbContext,
                (int)originalTransaction.Inventory_Warehouse.CompanyId,
                (int)originalTransaction.WarehouseId,
                timeBucketId,
                //quantityRevertReferenceInformation.DateTimeForRevertAdjustment,
                originalTransaction.RegistrationDate.Value,
                getAdjustmentIssueType(dbContext),
                (int)quantityRevertReferenceInformation.PricingReferenceTransactionIdForRevertAdjustment,
                originalTransaction.Id,
                InventoryOperationReferenceTypes.INVENTORY_ISSUE_ADJUSTMENT,
                originalTransaction.Id.ToString(),
                userId,
                out code,
                out message);

            var transactionItem = new List<Inventory_TransactionItem>();
            transactionItem.Add(new Inventory_TransactionItem()
            {
                GoodId = (int)goodId,
                CreateDate = DateTime.Now,
                Description = "Issue Adjustment",
                QuantityAmount = quantityToRevert,
                QuantityUnitId = quantityRevertReferenceInformation.InventoryQuantityUnitId,
                TransactionId = (int)operationReference.OperationId,
                UserCreatorId = userId
            });

            string transactionItemMessage;
            var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

            string pricingMessage;

            priceSuspendedTransactionUsingReference(
                 dbContext,
                 (int)operationReference.OperationId,
                 InventoryOperationReferenceTypes.INVENTORY_ISSUE_ADJUSTMENT + " Referenced Pricing",
                 quantityRevertReferenceInformation.DateTimeForRevertAdjustment,
                 userId,
                 out pricingMessage,
                 InventoryOperationReferenceTypes.INVENTORY_ISSUE_ADJUSTMENT,
                 originalTransaction.Id.ToString());

            updateContext(dbContext);

            createdTransaction = this.GetTransaction(operationReference.OperationId, InventoryOperationType.Issue);

            return createInventoryOperationResult(createdTransaction);
        }

        private InventoryOperation createIssueWithFIFOPricingAdjustment(DataContainer dbContext, Inventory_Transaction originalTransaction, long goodId, decimal quantityToRevert, long quantityUnitId, int userId, out Inventory_Transaction createdTransaction)
        {
            var timeBucketId = getTimeBucketId(dbContext, originalTransaction.RegistrationDate.Value);
            string code;
            string message;

            var operationReference = issue(dbContext,
                (int)originalTransaction.Inventory_Warehouse.CompanyId,
                (int)originalTransaction.WarehouseId,
                timeBucketId,
                originalTransaction.RegistrationDate.Value,
                getAdjustmentIssueType(dbContext),
                null,
                originalTransaction.Id,
                InventoryOperationReferenceTypes.INVENTORY_ISSUE_ADJUSTMENT,
                originalTransaction.Id.ToString(),
                userId,
                out code,
                out message);

            var transactionItem = new List<Inventory_TransactionItem>();
            transactionItem.Add(new Inventory_TransactionItem()
            {
                GoodId = (int)goodId,
                CreateDate = DateTime.Now,
                Description = "Issue Adjustment",
                QuantityAmount = quantityToRevert,
                QuantityUnitId = quantityUnitId,
                TransactionId = (int)operationReference.OperationId,
                UserCreatorId = userId
            });

            string transactionItemMessage;
            var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

            string pricingMessage;

            priceIssuedItemInFIFO(
                 dbContext,
                 (int)operationReference.OperationId,
                 InventoryOperationReferenceTypes.INVENTORY_ISSUE_ADJUSTMENT + " FIFO Pricing",
                 userId,
                 out pricingMessage,
                 InventoryOperationReferenceTypes.INVENTORY_ISSUE_ADJUSTMENT,
                 originalTransaction.Id.ToString());

            updateContext(dbContext);

            createdTransaction = this.GetTransaction(operationReference.OperationId, InventoryOperationType.Issue);

            return createInventoryOperationResult(createdTransaction);
        }

        private void checkGoodsInventoryQuantity(Dictionary<long, decimal> goodsInventoryQuantity, Dictionary<long, decimal> goodsQuantityChange)
        {
            foreach (var goodId in goodsInventoryQuantity.Keys)
            {
                if (goodsQuantityChange.ContainsKey(goodId) && (goodsInventoryQuantity[goodId] + goodsQuantityChange[goodId]) < 0)
                    throw new BusinessRuleException("", "The change causes inconsistency in Inventory.");
            }
        }

        private Dictionary<long, decimal> getInventoryStock(DataContainer dbContext, long warehouseId, Dictionary<long, List<GoodQuantity>> entityGoodsQuantities)
        {
            Dictionary<long, decimal> result = new Dictionary<long, decimal>();

            foreach (var goodId in entityGoodsQuantities.Keys)
            {
                result.Add(goodId, this.getInventoryQuantity(dbContext, goodId, entityGoodsQuantities[goodId].First().InventoryQuantityUnitId, warehouseId, null));
            }

            return result;
        }

        private void removeVouchers(DataContainer dbContext, Inventory_Transaction originalTransaction, IVoucherRepository voucherRepository, out bool hasAnyLockedVoucherInFinancialSystem)
        {
            var referenceNumber = originalTransaction.GetActionNumber();

            var journalEntryRepository = ServiceLocator.Current.GetInstance<IRepository<JournalEntry>>();
            var segmentRepository = ServiceLocator.Current.GetInstance<IRepository<Segment>>();

            var vouchersToDelete = voucherRepository.Find(v => v.ReferenceNo == referenceNumber).ToList();

            hasAnyLockedVoucherInFinancialSystem = false;
            hasAnyLockedVoucherInFinancialSystem = vouchersToDelete.Any(v => (FinancialVoucherStates)v.FinancialVoucherState.GetValueOrDefault((int)FinancialVoucherStates.NotSent) == FinancialVoucherStates.Sent);

            if (!hasAnyLockedVoucherInFinancialSystem)
            {
                foreach (var voucher in vouchersToDelete)
                {
                    var voucherReferenceNumber = voucher.LocalVoucherNo;

                    //A.H: Commented as the financial service does not support transactional delete voucher operation, so the voucher with SentToFinance status will be considered as a locked voucher and should not be deleted in FuelManagementSystem.
                    //try
                    //{
                    //    voucherService.DeleteVoucher(voucherReferenceNumber);
                    //}
                    //catch (Exception)
                    //{
                    //    hasAnyLockedVoucherInFinancialSystem = true;

                    //    continue;
                    //}

                    if ((FinancialVoucherStates)voucher.FinancialVoucherState.GetValueOrDefault((int)FinancialVoucherStates.NotSent) != FinancialVoucherStates.Sent)
                    {
                        var journalEntriesToDelete = voucher.JournalEntrieses.ToList();

                        foreach (var journalEntry in journalEntriesToDelete)
                        {
                            var segmentsToDelete = journalEntry.Segments.ToList();

                            foreach (var segment in segmentsToDelete)
                            {
                                segmentRepository.Delete(segment);
                            }

                            journalEntryRepository.Delete(journalEntry);
                        }

                        voucherRepository.Delete(voucher);
                    }
                }
            }

            //if (!hasAnyLockedVoucherInFinancialSystem)  //This condition is commented to change the status of transactions Full Priced to re-price them.
            {
                var invntoryTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == originalTransaction.Id);

                if (invntoryTransaction.Status.Value == (byte)TransactionState.Vouchered)
                {
                    invntoryTransaction.Status = (byte)TransactionState.FullPriced;
                }
                //else if (invntoryTransaction.Status.Value == (byte)TransactionState.JustRegistered || invntoryTransaction.Status.Value == (byte)TransactionState.PartialPriced)
                //{
                //    //throw new InvalidOperation("RevertTransactionStatusFromVouchered", "The current transaction status is not vouchered.");
                //}

                dbContext.SaveChanges();
            }
        }

        //private bool hasSentVouchersToFinance(string referenceNumber, IVoucherRepository voucherRepository)
        //{
        //    return voucherRepository.Count(v => v.FinancialVoucherState.Value == (int)FinancialVoucherStates.Sent && v.ReferenceNo == referenceNumber) != 0;
        //}

        private void updateContext(DataContainer context)
        {
            context = new DataContainer();

            //context.ChangeTracker.DetectChanges();

            //var objContext = ((IObjectContextAdapter)context).ObjectContext;
            //objContext.Refresh(RefreshMode.StoreWins, context.Inventory_OperationReference);
            //objContext.Refresh(RefreshMode.StoreWins, context.Inventory_TransactionItemPrice);
            //objContext.Refresh(RefreshMode.StoreWins, context.Inventory_TransactionItem);
            //objContext.Refresh(RefreshMode.StoreWins, context.Inventory_Transaction);

            //objContext.Refresh(RefreshMode.ClientWins, context.Inventory_OperationReference);
            //objContext.Refresh(RefreshMode.ClientWins, context.Inventory_TransactionItemPrice);
            //objContext.Refresh(RefreshMode.ClientWins, context.Inventory_TransactionItem);
            //objContext.Refresh(RefreshMode.ClientWins, context.Inventory_Transaction);
        }

        private List<Inventory_Transaction> findTransactionsForPricingRevision(DataContainer dbContext, Inventory_Transaction originalTransaction, List<long> changedGoodsIdList)
        {
            var transactionId = originalTransaction.Id;
            var warehouseId = originalTransaction.WarehouseId;

            var res = new List<Inventory_Transaction>();

            res.AddRange(getRecieptsWithGivenTransactionAsPricingRefrence(dbContext, transactionId, warehouseId));
            res.AddRange(getIssuesWithGivenTransactionAsPricingRefrence(dbContext, transactionId, warehouseId));
            res.AddRange(getIssuesWithFIFOPricingAfterGivenTransaction(dbContext, originalTransaction, changedGoodsIdList));

            return res.OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate).ToList();
        }

        private bool hasChangedQuantity(Dictionary<long, List<GoodQuantity>> registeredTransactionFinalValues, Dictionary<long, List<GoodQuantity>> entityGoodsQuantities, InventoryOperationType operationType, out Dictionary<long, decimal> goodsQuantityChange)
        {
            if (operationType == InventoryOperationType.Issue)
            {
                if (entityGoodsQuantities.Any(c => c.Value.Sum(i => i.SignedQuantity) > 0))
                    throw new BusinessRuleException("", "The Change Quantity leads to invalid final quantity for Issued transaction.");
            }
            else
            {
                if (entityGoodsQuantities.Any(c => c.Value.Sum(i => i.SignedQuantity) < 0))
                    throw new BusinessRuleException("", "The Change Quantity leads to invalid final quantity for Received transaction.");
            }

            Dictionary<long, decimal> dicSourceGoodsQuantity = new Dictionary<long, decimal>();
            Dictionary<long, decimal> dicTargetGoodsQuantity = new Dictionary<long, decimal>();

            foreach (var goodId in registeredTransactionFinalValues.Keys)
            {
                var firstItemGoodQuantityUnitId = registeredTransactionFinalValues[goodId].First().InventoryQuantityUnitId;

                var sourceTotalPrice = registeredTransactionFinalValues[goodId].Sum(i => i.SignedQuantity);

                dicSourceGoodsQuantity.Add(goodId, sourceTotalPrice);

                dicTargetGoodsQuantity.Add(goodId, 0);  //This is added to perform an outer join between source and target prices.
            }

            foreach (var goodId in entityGoodsQuantities.Keys)
            {
                var firstItemGoodQuantityUnitId = entityGoodsQuantities[goodId].First().InventoryQuantityUnitId;

                var targetTotalPrice = entityGoodsQuantities[goodId].Sum(i => i.SignedQuantity);

                if (dicTargetGoodsQuantity.ContainsKey(goodId))
                    dicTargetGoodsQuantity[goodId] = targetTotalPrice;
                else
                    dicTargetGoodsQuantity.Add(goodId, targetTotalPrice);

                if (!dicSourceGoodsQuantity.ContainsKey(goodId))  //This is added to perform an outer join between source and target prices.
                    dicSourceGoodsQuantity.Add(goodId, 0);
            }

            goodsQuantityChange = dicSourceGoodsQuantity.Join(
                dicTargetGoodsQuantity,
                outer => outer.Key,
                inner => inner.Key,
                (s, t) => new { s.Key, Difference = t.Value - s.Value })
                .Where(r => r.Difference != 0).ToDictionary(r => r.Key, r => r.Difference); //This difference indicates the inventory quantity change from inventory transaction to entity final values.

            return goodsQuantityChange.Any();
        }

        private bool hasChangedPrice(Dictionary<long, List<GoodQuantityPricing>> registeredTransactionFinalValuesWithPrices, Dictionary<long, List<GoodQuantityPricing>> entityGoodsQuantitiesWithPrices, out Dictionary<long, decimal> goodsPriceDifference)
        {
            Dictionary<long, decimal> dicSourceGoodsPrice = new Dictionary<long, decimal>();
            Dictionary<long, decimal> dicTargetGoodsPrice = new Dictionary<long, decimal>();

            foreach (var goodId in registeredTransactionFinalValuesWithPrices.Keys)
            {
                var firstItemGoodQuantityUnitId = registeredTransactionFinalValuesWithPrices[goodId].First().InventoryQuantityUnitId;

                var sourceTotalPrice = registeredTransactionFinalValuesWithPrices[goodId].CalculateSingedTotalPriceInMainCurrency();

                dicSourceGoodsPrice.Add(goodId, sourceTotalPrice);

                dicTargetGoodsPrice.Add(goodId, 0);  //This is added to perform an outer join between source and target prices.
            }

            foreach (var goodId in entityGoodsQuantitiesWithPrices.Keys)
            {
                var firstItemGoodQuantityUnitId = entityGoodsQuantitiesWithPrices[goodId].First().InventoryQuantityUnitId;

                var targetTotalPrice = entityGoodsQuantitiesWithPrices[goodId].CalculateSingedTotalPriceInMainCurrency();

                if (dicTargetGoodsPrice.ContainsKey(goodId))
                    dicTargetGoodsPrice[goodId] = targetTotalPrice;
                else
                    dicTargetGoodsPrice.Add(goodId, targetTotalPrice);

                if (!dicSourceGoodsPrice.ContainsKey(goodId))  //This is added to perform an outer join between source and target prices.
                    dicSourceGoodsPrice.Add(goodId, 0);
            }

            goodsPriceDifference = dicSourceGoodsPrice.Join(
                dicTargetGoodsPrice,
                outer => outer.Key,
                inner => inner.Key,
                (s, t) => new { s.Key, Difference = t.Value - s.Value })
                .Where(r => r.Difference != 0).ToDictionary(r => r.Key, r => r.Difference); //This difference, also, indicates the inventory quantity change from inventory transaction to entity final values.

            return goodsPriceDifference.Any();
        }

        public Dictionary<long, decimal> CalculateTransactionGoodsFinalQuantities(InventoryOperation inventoryOperation)
        {
            InventoryOperationType operationType;
            if (inventoryOperation.ActionType == InventoryActionType.Issue) operationType = InventoryOperationType.Issue;
            else if (inventoryOperation.ActionType == InventoryActionType.Receipt) operationType = InventoryOperationType.Receipt;
            else throw new InvalidOperation("FindingInventoryOperation", "Inventory Operation.");

            var transaction = this.GetTransaction(inventoryOperation.InventoryOperationId, operationType);

            var transactionGoodsQuantities = calculateTransactionQuantitiesValues(transaction);
            var transactionFinalValues = calculateTransactionFinalQuantites(transaction, transactionGoodsQuantities);

            return transactionFinalValues;
        }

        private bool getWarehouseCurrentActiveStatus(DataContainer dbContext, long warehouseId)
        {
            return dbContext.Inventory_Warehouse.Single(w => w.Id == warehouseId).IsActive.Value;
        }

        public bool GetWarehouseCurrentActiveStatus(long warehouseId)
        {
            using (var dbContext = new DataContainer())
            {
                return this.getWarehouseCurrentActiveStatus(dbContext, warehouseId);
            }
        }

        private Dictionary<long, List<GoodQuantity>> calculateTransactionQuantitiesValues(Inventory_Transaction inventoryTransaction)
        {
            var result = new Dictionary<long, List<GoodQuantity>>();

            foreach (var inventoryTransactionItem in inventoryTransaction.Inventory_TransactionItem)
            {
                result.Add(inventoryTransactionItem.GoodId, new List<GoodQuantity>
                    { 
                        new GoodQuantity()
                        {
                            InventoryGoodId = inventoryTransactionItem.GoodId,
                            SignedQuantity = (inventoryTransactionItem.QuantityAmount ?? 0) * (((InventoryOperationType)inventoryTransaction.Action == InventoryOperationType.Receipt) ? 1 : -1),
                            InventoryQuantityUnitId = inventoryTransactionItem.QuantityUnitId,
                            TransactionId = inventoryTransaction.Id
                        }
                    }
                );
            }

            foreach (var adjsutmentTransaction in inventoryTransaction.Inventory_TransactionAdjustments.OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate))
            {
                foreach (var inventoryTransactionItem in adjsutmentTransaction.Inventory_TransactionItem)
                {
                    var transactionItemQuantity = new GoodQuantity()
                                                  {
                                                      InventoryGoodId = inventoryTransactionItem.GoodId,
                                                      SignedQuantity = (inventoryTransactionItem.QuantityAmount ?? 0) * (((InventoryOperationType)adjsutmentTransaction.Action == InventoryOperationType.Receipt) ? 1 : -1),
                                                      //QuantityAbbreviation = inventoryTransactionItem.Inventory_Unit.Abbreviation,
                                                      InventoryQuantityUnitId = inventoryTransactionItem.QuantityUnitId,
                                                      TransactionId = adjsutmentTransaction.Id
                                                  };

                    if (!result.ContainsKey(inventoryTransactionItem.GoodId))
                    {
                        result.Add(inventoryTransactionItem.GoodId, new List<GoodQuantity>());
                    }

                    result[inventoryTransactionItem.GoodId].Add(transactionItemQuantity);
                }
            }

            return result;
        }

        private Dictionary<long, List<GoodQuantityPricing>> calculateTransactionQuantitiesValuesWithPrices(Inventory_Transaction originalTransaction)
        {
            var result = new Dictionary<long, List<GoodQuantityPricing>>();

            var isTransactionIssueWithFIFOPricing = originalTransaction.IsIssueWithFIFOPricing();

            if (originalTransaction.IsFullyPriced())
            {
                foreach (var inventoryTransactionItemPrice in originalTransaction.Inventory_TransactionItem.SelectMany(item => item.Inventory_TransactionItemPrice))
                {
                    DateTime dateTimeForRevertAdjustment;
                    long pricingReferenceTransactionIdForRevertAdjustment;

                    if (isTransactionIssueWithFIFOPricing)
                    {
                        dateTimeForRevertAdjustment = inventoryTransactionItemPrice.Inventory_TransactionItemPriceFIFOReference.Inventory_TransactionItem.Inventory_Transaction.RegistrationDate.Value;
                        pricingReferenceTransactionIdForRevertAdjustment = inventoryTransactionItemPrice.Inventory_TransactionItemPriceFIFOReference.Inventory_TransactionItem.Inventory_Transaction.Id;
                    }
                    else
                    {
                        dateTimeForRevertAdjustment = originalTransaction.RegistrationDate.Value;
                        pricingReferenceTransactionIdForRevertAdjustment = originalTransaction.Id;
                    }

                    var transactionItemQuantityPricing = new GoodQuantityPricing()
                        {
                            InventoryGoodId = inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId,
                            SignedQuantity = (inventoryTransactionItemPrice.QuantityAmount ?? 0) * (((InventoryOperationType)originalTransaction.Action == InventoryOperationType.Receipt) ? 1 : -1),
                            InventoryQuantityUnitId = inventoryTransactionItemPrice.QuantityUnitId,
                            TransactionId = originalTransaction.Id,
                            DateTimeForRevertAdjustment = dateTimeForRevertAdjustment,
                            PricingReferenceTransactionIdForRevertAdjustment = pricingReferenceTransactionIdForRevertAdjustment,
                            Fee = inventoryTransactionItemPrice.Fee.Value,
                            FeeInventoryCurrencyUnitId = inventoryTransactionItemPrice.PriceUnitId,
                            FeeInMainCurrency = inventoryTransactionItemPrice.FeeInMainCurrency.Value,
                            InventoryMainCurrencyUnitId = inventoryTransactionItemPrice.MainCurrencyUnitId,
                            TransactionItemId = inventoryTransactionItemPrice.TransactionItemId,
                            TransactionItemPriceId = inventoryTransactionItemPrice.Id
                        };

                    if (!result.ContainsKey(inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId))
                    {
                        result.Add(inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId, new List<GoodQuantityPricing>());
                    }

                    result[inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId].Add(transactionItemQuantityPricing);
                }
            }

            foreach (var adjsutmentTransaction in originalTransaction.Inventory_TransactionAdjustments.OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate))
            {
                if (adjsutmentTransaction.IsFullyPriced())
                {
                    foreach (var inventoryTransactionItemPrice in adjsutmentTransaction.Inventory_TransactionItem.SelectMany(item => item.Inventory_TransactionItemPrice))
                    {
                        DateTime dateTimeForRevertAdjustment;
                        long pricingReferenceTransactionIdForRevertAdjustment;

                        if (isTransactionIssueWithFIFOPricing)
                        {
                            dateTimeForRevertAdjustment = inventoryTransactionItemPrice.Inventory_TransactionItemPriceFIFOReference.Inventory_TransactionItem.Inventory_Transaction.RegistrationDate.Value;
                            pricingReferenceTransactionIdForRevertAdjustment = inventoryTransactionItemPrice.Inventory_TransactionItemPriceFIFOReference.Inventory_TransactionItem.Inventory_Transaction.Id;
                        }
                        else
                        {
                            dateTimeForRevertAdjustment = originalTransaction.RegistrationDate.Value;
                            pricingReferenceTransactionIdForRevertAdjustment = originalTransaction.Id;
                        }

                        var transactionItemQuantityPricing = new GoodQuantityPricing()
                                                             {
                                                                 InventoryGoodId = inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId,
                                                                 SignedQuantity = (inventoryTransactionItemPrice.QuantityAmount ?? 0) * (((InventoryOperationType)originalTransaction.Action == InventoryOperationType.Receipt) ? 1 : -1),
                                                                 InventoryQuantityUnitId = inventoryTransactionItemPrice.QuantityUnitId,
                                                                 TransactionId = originalTransaction.Id,
                                                                 DateTimeForRevertAdjustment = dateTimeForRevertAdjustment,
                                                                 PricingReferenceTransactionIdForRevertAdjustment = pricingReferenceTransactionIdForRevertAdjustment,
                                                                 Fee = inventoryTransactionItemPrice.Fee.Value,
                                                                 FeeInventoryCurrencyUnitId = inventoryTransactionItemPrice.PriceUnitId,
                                                                 FeeInMainCurrency = inventoryTransactionItemPrice.FeeInMainCurrency.Value,
                                                                 InventoryMainCurrencyUnitId = inventoryTransactionItemPrice.MainCurrencyUnitId,
                                                                 TransactionItemId = inventoryTransactionItemPrice.TransactionItemId,
                                                                 TransactionItemPriceId = inventoryTransactionItemPrice.Id
                                                             };

                        if (!result.ContainsKey(inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId))
                        {
                            result.Add(inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId, new List<GoodQuantityPricing>());
                        }

                        result[inventoryTransactionItemPrice.Inventory_TransactionItem.GoodId].Add(transactionItemQuantityPricing);
                    }
                }
            }

            return result;
        }

        private Dictionary<long, List<GoodQuantityPricing>> calculateTransactionFinalEffectiveValuesWithPrices(Inventory_Transaction originalTransaction, Dictionary<long, List<GoodQuantityPricing>> transactionGoodsQuantitiesWithPrices)
        {
            //This method refines all QuantityPricing parts by removing adjustment segements quantities.
            var result = new Dictionary<long, List<GoodQuantityPricing>>();

            var validSignOfEachElementBasedOnTransactionType = (InventoryOperationType)originalTransaction.Action == InventoryOperationType.Issue ? Sign.Negative : Sign.Positive;

            foreach (var goodId in transactionGoodsQuantitiesWithPrices.Keys)
            {
                result.Add(goodId, new List<GoodQuantityPricing> { transactionGoodsQuantitiesWithPrices[goodId].First() });

                for (int index = 1; index < transactionGoodsQuantitiesWithPrices[goodId].Count; index++)
                {
                    var checkingTransactionQuantityPrice = transactionGoodsQuantitiesWithPrices[goodId][index];
                    //The checking QuantityPricing parts represent the adjustment pricing parts which should not be greater in quantity than regular QuantityPricing parts.

                    if (result[goodId].Last().QuantitySign == checkingTransactionQuantityPrice.QuantitySign)
                    {
                        result[goodId].Add(checkingTransactionQuantityPrice);
                    }
                    else
                    {
                        var lastAddedTransactionQuantityPrice = result[goodId].Last();
                        lastAddedTransactionQuantityPrice.SignedQuantity -= checkingTransactionQuantityPrice.SignedQuantity;

                        if (lastAddedTransactionQuantityPrice.QuantitySign != validSignOfEachElementBasedOnTransactionType)
                            //For Issue Transactions the valid sign of each element is minus (-) and for Receipt Transactions the valid sign is plus (+).
                            //If this "if" statement evaluates to true, it means that an adjustment part is reached which leads to changing the type of transaction to its opposite type.
                            throw new BusinessRuleException("", "Invalid good quantity in adjustment part.");
                    }
                }

                //if (result.ContainsKey(goodId))
                //{
                //    for (int index = 1; index < transactionGoodsQuantitiesWithPrices[goodId].Count; index++)
                //    {
                //        var checkingTransactionQuantityPrice = transactionGoodsQuantitiesWithPrices[goodId][index];
                //        if (result[goodId].Last().QuantitySign == checkingTransactionQuantityPrice.QuantitySign)
                //        {
                //            result[goodId].Add(checkingTransactionQuantityPrice);
                //        }
                //        else
                //        {
                //            var lastAddedTransactionQuantityPrice = result[goodId].Last();
                //            lastAddedTransactionQuantityPrice.SignedQuantity -= checkingTransactionQuantityPrice.SignedQuantity;

                //            if (Math.Sign(lastAddedTransactionQuantityPrice.SignedQuantity) != Math.Sign(validSignOfEachElementBasedOnTransactionNature)) //For Issue Transactions the valid sign of each element is minus (-) and for Receipt Transactions the valid sign is plus (+).
                //                throw new BusinessRuleException("", "Invalid good quantity in ");
                //        }
                //    }
                //}
                //else
                //{
                //    result.Add(goodId, new List<GoodQuantityPricing> { transactionGoodsQuantitiesWithPrices[goodId].First() });
                //}
            }

            var refinedDictionary = result.ToDictionary(keySelector => keySelector.Key, elementSelector => elementSelector.Value.Where(v => v.SignedQuantity != 0).OrderBy(v => v.DateTimeForRevertAdjustment).ToList());

            //foreach (var goodId in result.Keys)
            //{
            //    //Sort and clear result.
            //    result[goodId] = result[goodId].Where(v => v.SignedQuantity != 0).OrderBy(v => v.DateTimeForRevertAdjustment).ToList();
            //}

            return refinedDictionary;
        }

        private Dictionary<long, decimal> calculateTransactionFinalQuantites(Inventory_Transaction originalTransaction, Dictionary<long, List<GoodQuantity>> transactionGoodsQuantities)
        {
            //This method refines all Quantity parts by removing adjustment segements quantities.
            var finalEffectiveQuantities = new Dictionary<long, List<GoodQuantity>>();

            var validSignOfEachElementBasedOnTransactionNature = (InventoryOperationType)originalTransaction.Action == InventoryOperationType.Issue ? Sign.Negative : Sign.Positive;

            foreach (var goodId in transactionGoodsQuantities.Keys)
            {
                finalEffectiveQuantities.Add(goodId, new List<GoodQuantity> { transactionGoodsQuantities[goodId].First() });

                for (int index = 1; index < transactionGoodsQuantities[goodId].Count; index++)
                {
                    var checkingTransactionQuantityPrice = transactionGoodsQuantities[goodId][index];
                    //The checking Quantity parts represent the adjustment pricing parts which should not be greater in quantity than regular QuantityPricing parts.

                    if (finalEffectiveQuantities[goodId].Last().QuantitySign == checkingTransactionQuantityPrice.QuantitySign)
                    {
                        finalEffectiveQuantities[goodId].Add(checkingTransactionQuantityPrice);
                    }
                    else
                    {
                        var lastAddedTransactionQuantityPrice = finalEffectiveQuantities[goodId].Last();
                        lastAddedTransactionQuantityPrice.SignedQuantity -= checkingTransactionQuantityPrice.SignedQuantity;

                        if (lastAddedTransactionQuantityPrice.QuantitySign != validSignOfEachElementBasedOnTransactionNature)
                            //For Issue Transactions the valid sign of each element is minus (-) and for Receipt Transactions the valid sign is plus (+).
                            //If this "if" statement evaluates to true, it means that an adjustment part is reached which leads to changing the type of transaction to its opposite type.
                            throw new BusinessRuleException("", "Invalid good quantity in adjustment part.");
                    }
                }
            }

            var result = new Dictionary<long, decimal>();

            foreach (var goodId in finalEffectiveQuantities.Keys)
            {
                //Sort and clear result.
                result.Add(goodId, finalEffectiveQuantities[goodId].Sum(v => v.SignedQuantity));
            }

            return result;
        }

        private Dictionary<long, List<GoodQuantity>> createEntityFinalValues(DataContainer dbContext, FuelReport fuelReport, int inventoryOperationId, Dictionary<long, decimal> goodsConsumption)
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
                            InventoryQuantityUnitId = (int)getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                            TransactionId = inventoryOperationId
                        }
                    }
                );
            }

            return result;
        }

        private List<Inventory_Transaction> getRecieptsWithGivenTransactionAsPricingRefrence(DataContainer dataContainer, long transactionId, long warehouseId)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == (byte)InventoryOperationType.Receipt
                                                                && c.PricingReferenceId == transactionId
                                                                && c.WarehouseId == warehouseId).AsNoTracking().ToList();
        }

        private List<Inventory_Transaction> getIssuesWithGivenTransactionAsPricingRefrence(DataContainer dataContainer, long transactionId, long warehouseId)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == (byte)InventoryOperationType.Issue
                                                                && c.PricingReferenceId == transactionId
                                                                && c.WarehouseId == warehouseId).AsNoTracking().ToList();
        }

        private List<Inventory_Transaction> findIssuesWithFIFOPricingAfterGivenDateTime(DataContainer dataContainer, DateTime camparingDateTime, long warehouseId, List<long> changedGoodsIdList)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == (byte)InventoryOperationType.Issue
                                                            && c.PricingReferenceId == null
                                                            && c.WarehouseId == warehouseId
                                                            && c.RegistrationDate >= camparingDateTime
                                                            && c.Inventory_TransactionItem.Any(item => changedGoodsIdList.Contains(item.GoodId))).AsNoTracking().ToList();
        }

        private List<Inventory_Transaction> getIssuesWithFIFOPricingAfterGivenTransaction(DataContainer dataContainer, Inventory_Transaction baseInventoryTransaction, List<long> changedGoodsIdList)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == (byte)InventoryOperationType.Issue
                                                            && c.PricingReferenceId == null
                                                            && c.WarehouseId == baseInventoryTransaction.WarehouseId
                                                            && c.RegistrationDate >= baseInventoryTransaction.RegistrationDate
                                                            && c.Code.Value > baseInventoryTransaction.Code.Value
                                                            && c.Inventory_TransactionItem.Any(item => changedGoodsIdList.Contains(item.GoodId))).AsNoTracking().ToList();
        }

        public List<InventoryOperation> ManageFuelReportDetailReceive(FuelReportDetail fuelReportDetail, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var result = new List<InventoryOperation>();

                    if (fuelReportDetail.Receive.HasValue)
                    {
                        //TODO: Receive OK Except for Trust
                        #region Receive

                        var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();

                        string transactionCode;
                        string transactionMessage;

                        var operationReference =
                            receipt(
                                dbContext,
                                (int)fuelReportDetail.FuelReport.VesselInCompany.CompanyId,
                                (int)fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id,
                                getTimeBucketId(dbContext, fuelReportDetail.FuelReport.EventDate),
                                fuelReportDetail.FuelReport.EventDate,
                                convertFuelReportReceiveTypeToStoreType(dbContext, fuelReportDetail),
                                null,
                                null,
                                InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE,
                                transactionReferenceNumber,
                                userId,
                                out transactionCode,
                                out transactionMessage);

                        string transactionItemMessage;

                        var transactionItem = new List<Inventory_TransactionItem>();
                        transactionItem.Add(new Inventory_TransactionItem()
                                            {
                                                GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                                                CreateDate = DateTime.Now,
                                                Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                                                QuantityAmount = (decimal?)fuelReportDetail.Receive.Value,
                                                QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                                                TransactionId = (int)operationReference.OperationId,
                                                UserCreatorId = userId
                                            });

                        addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                        updateContext(dbContext);

                        var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                        result.Add(createInventoryOperationResult(createdTransaction));
                        #endregion
                    }

                    //transaction.Commit();

                    return result;
                }
            }
            return null;
        }

        private List<Inventory_OperationReference> getFuelReportDetailReceiveReference(DataContainer dbContext, FuelReportDetail fuelReportDetail)
        {
            var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE, transactionReferenceNumber);
            return references;
        }

        public List<InventoryOperation> ManageFuelReportDetailTransfer(FuelReportDetail fuelReportDetail, long? pricingReferenceId, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                var inventoryQuantity = getInventoryQuantity(dbContext, fuelReportDetail.Good.SharedGoodId, fuelReportDetail.MeasuringUnit.Abbreviation, fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id, null);

                if (inventoryQuantity - fuelReportDetail.Transfer.Value < 0)
                    throw new BusinessRuleException("", "Transfer value causes inconsistency in Inventory.");

                var issuesWithFIFOPricingAfterCurrentEntity = findIssuesWithFIFOPricingAfterGivenDateTime(dbContext, fuelReportDetail.FuelReport.EventDate, fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id, new List<long>() { fuelReportDetail.Good.SharedGoodId });

                if (issuesWithFIFOPricingAfterCurrentEntity.Count(t => t.Status == (byte)TransactionState.FullPriced || t.Status == (byte)TransactionState.Vouchered) > 0)
                    throw new BusinessRuleException("", "There are some Issues with FIFO pricing in Inventory that should be reverted.");


                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var result = new List<InventoryOperation>();

                    if (fuelReportDetail.Transfer.HasValue)
                    {
                        //TODO: Transfer
                        #region Transfer

                        var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
                        var transactionReferenceType = InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER;

                        string transactionCode;
                        string transactionMessage;

                        var operationReference =
                            issue(
                                dbContext,
                                (int)fuelReportDetail.FuelReport.VesselInCompany.CompanyId,
                                (int)fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id,
                                getTimeBucketId(dbContext, fuelReportDetail.FuelReport.EventDate),
                                fuelReportDetail.FuelReport.EventDate,
                                convertFuelReportTransferTypeToStoreType(dbContext, fuelReportDetail),
                                (int?)pricingReferenceId,
                                null,
                                transactionReferenceType,
                                transactionReferenceNumber,
                                userId,
                                out transactionCode,
                                out transactionMessage);

                        string transactionItemMessage;

                        var transactionItem = new List<Inventory_TransactionItem>();
                        transactionItem.Add(new Inventory_TransactionItem()
                        {
                            GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                            CreateDate = DateTime.Now,
                            Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                            QuantityAmount = (decimal?)fuelReportDetail.Transfer.Value,
                            QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                            TransactionId = (int)operationReference.OperationId,
                            UserCreatorId = userId
                        });

                        var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);


                        string pricingMessage;

                        //todo: oza kheili kharabe xxxxxx 
                        try
                        {
                            if (pricingReferenceId.HasValue)
                            {
                                priceSuspendedTransactionUsingReference(dbContext, (int)operationReference.OperationId, transactionReferenceType + " Pricing by Reference", null, userId, out pricingMessage,
                                    transactionReferenceType, transactionReferenceNumber);
                            }
                            else
                            {
                                priceIssuedItemInFIFO(dbContext, transactionItemIds[0], transactionReferenceType + " FIFO Pricing", userId,
                                              out pricingMessage,
                                              transactionReferenceType,
                                              transactionReferenceNumber);
                            }
                        }
                        catch
                        {


                        }

                        updateContext(dbContext);

                        var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                        result.Add(createInventoryOperationResult(createdTransaction));

                        #endregion
                    }

                    //transaction.Commit();

                    return result;
                }
            }
            return null;
        }

        private List<Inventory_OperationReference> getFuelReportDetailTransferReference(DataContainer dbContext, FuelReportDetail fuelReportDetail)
        {
            var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER, transactionReferenceNumber);
            return references;
        }

        public Inventory_OperationReference GetFuelReportDetailTransferReference(FuelReportDetail fuelReportDetail)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getFuelReportDetailTransferReference(dbContext, fuelReportDetail).FirstOrDefault();
            }
        }

        //private decimal? calculateCorrectionAmount(FuelReportDetail detail)
        //{
        //    if (detail.Correction.HasValue && detail.CorrectionType.HasValue)
        //    {
        //        return (decimal?)((detail.CorrectionType.Value == CorrectionTypes.Minus ? -1 : 1) * detail.Correction.Value);
        //    }

        //    return null;
        //}

        private int convertFuelReportReceiveTypeToStoreType(DataContainer dbContext, FuelReportDetail detail)
        {
            short code = 0;

            switch (detail.ReceiveType)
            {
                case ReceiveTypes.Trust:
                    code = (short)InventoryStoreTypesCode.FuelReport_Receive_Trust;
                    break;

                case ReceiveTypes.InternalTransfer:
                    code = (short)InventoryStoreTypesCode.FuelReport_Receive_InternalTransfer;
                    break;

                case ReceiveTypes.TransferPurchase:
                    code = (short)InventoryStoreTypesCode.FuelReport_Receive_TransferPurchase;
                    break;

                case ReceiveTypes.Purchase:
                    code = (short)InventoryStoreTypesCode.FuelReport_Receive_Purchase;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }
        private int convertFuelReportTransferTypeToStoreType(DataContainer dbContext, FuelReportDetail detail)
        {
            short code = 0;

            switch (detail.TransferType)
            {
                case TransferTypes.InternalTransfer:
                    code = (short)InventoryStoreTypesCode.FuelReport_Transfer_InternalTransfer;
                    break;

                case TransferTypes.TransferSale:
                    code = (short)InventoryStoreTypesCode.FuelReport_Transfer_TransferSale;
                    break;

                case TransferTypes.Rejected:
                    code = (short)InventoryStoreTypesCode.FuelReport_Transfer_Rejected;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }
        private int convertFuelReportCorrectionTypeToStoreType(DataContainer dbContext, FuelReportDetail detail)
        {
            short code = 0;

            switch (detail.CorrectionType)
            {
                case CorrectionTypes.Plus:
                    if (detail.CorrectionPricingType == CorrectionPricingTypes.LastIssuedConsumption)
                        code = (short)InventoryStoreTypesCode.FuelReport_Incremental_Correction_for_Issued_Voyage;
                    else
                        code = (short)InventoryStoreTypesCode.FuelReport_Incremental_Correction_Inventory_Adjustment;  //Inventory Adjustments
                    break;

                case CorrectionTypes.Minus:
                    if (detail.CorrectionPricingType == CorrectionPricingTypes.LastIssuedConsumption)
                        code = (short)InventoryStoreTypesCode.FuelReport_Decremental_Correction_for_Issued_Voyage;
                    else
                        code = (short)InventoryStoreTypesCode.FuelReport_Decremental_Correction_Inventory_Adjustment;  //Inventory Adjustments
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }
        private int convertFuelReportConsumptionTypeToStoreType(DataContainer dbContext, FuelReport fuelReport)
        {
            short code = 0;

            if (fuelReport.IsEndOfYearReport())
                code = (short)InventoryStoreTypesCode.EOY_Consumption;
            else
            {
                switch (fuelReport.FuelReportType)
                {
                    case FuelReportTypes.EndOfVoyage:
                        code = (short)InventoryStoreTypesCode.EOV_Consumption;
                        break;

                    //case FuelReportTypes.EndOfMonth:
                    //    code = (short)InventoryStoreTypesCode.;
                    //    break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }

        private int getIssueTrustReceivesStoreType(DataContainer dbContext)
        {
            short code = (short)InventoryStoreTypesCode.Issue_Total_Received_Trust;

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }

        private int getEndOfVoyageConsumptionStoreType(DataContainer dbContext)
        {
            short code = (short)InventoryStoreTypesCode.EOV_Consumption;

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }

        //================================================================================

        public List<InventoryOperation> ManageFuelReportDetailDecrementalCorrection(FuelReportDetail fuelReportDetail, int userId)
        {
            //if (fuelReportDetail.Correction.HasValue && 
            //    fuelReportDetail.CorrectionType.HasValue &&
            //    fuelReportDetail.CorrectionPrice.HasValue &&
            //    !string.IsNullOrWhiteSpace(fuelReportDetail.CorrectionPriceCurrencyISOCode))

            //if (fuelReportDetail.CorrectionType.Value == CorrectionTypes.Minus)
            //{
            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var result = new List<InventoryOperation>();

                    //TODO: Decremental Correction
                    #region Decremental Correction

                    var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION;

                    string transactionCode;
                    string transactionMessage;

                    var operationReference =
                        issue(
                              dbContext,
                              (int)fuelReportDetail.FuelReport.VesselInCompany.CompanyId,
                              (int)fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id,
                                getTimeBucketId(dbContext, fuelReportDetail.FuelReport.EventDate),
                                fuelReportDetail.FuelReport.EventDate,
                              convertFuelReportCorrectionTypeToStoreType(dbContext, fuelReportDetail),
                              null,
                            null,
                            transactionReferenceType,
                              transactionReferenceNumber,
                              userId,
                              out transactionCode,
                              out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();
                    transactionItem.Add(new Inventory_TransactionItem()
                    {
                        GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                        CreateDate = DateTime.Now,
                        Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                        QuantityAmount = (decimal?)fuelReportDetail.Correction,
                        QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                        TransactionId = (int)operationReference.OperationId,
                        UserCreatorId = userId
                    });

                    var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    string pricingMessage;
                    //int? notPricedTransactionId;

                    try
                    {
                        priceIssuedItemInFIFO(dbContext, transactionItemIds[0], transactionReferenceType + " FIFO Pricing", userId,
                                              out pricingMessage, transactionReferenceType,
                                              transactionReferenceNumber);
                    }
                    catch
                    {
                    }

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    #endregion

                    //transaction.Commit();

                    return result;
                }
            }
        }

        private List<Inventory_OperationReference> getFuelReportDetailDecrementalCorrectionReference(DataContainer dbContext, FuelReportDetail fuelReportDetail)
        {
            var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION, transactionReferenceNumber);
            return references;
        }
        public Inventory_OperationReference GetFuelReportDetailDecrementalCorrectionReference(FuelReportDetail fuelReportDetail)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getFuelReportDetailDecrementalCorrectionReference(dbContext, fuelReportDetail).FirstOrDefault();
            }
        }
        public List<InventoryOperation> ManageFuelReportDetailDecrementalCorrectionWithoutPricing(FuelReportDetail fuelReportDetail, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                var result = new List<InventoryOperation>();

                #region Decremental Correction

                var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();

                string transactionCode;
                string transactionMessage;

                var operationReference =
                    issue(
                          dbContext,
                          (int)fuelReportDetail.FuelReport.VesselInCompany.CompanyId,
                          (int)fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id,
                            getTimeBucketId(dbContext, fuelReportDetail.FuelReport.EventDate),
                            fuelReportDetail.FuelReport.EventDate,
                          convertFuelReportCorrectionTypeToStoreType(dbContext, fuelReportDetail),
                          null,
                        null,
                          InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION,
                          transactionReferenceNumber,
                          userId,
                          out transactionCode,
                          out transactionMessage);

                string transactionItemMessage;

                var transactionItem = new List<Inventory_TransactionItem>();
                transactionItem.Add(new Inventory_TransactionItem()
                {
                    GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                    CreateDate = DateTime.Now,
                    Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                    QuantityAmount = (decimal?)fuelReportDetail.Correction,
                    QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                    TransactionId = (int)operationReference.OperationId,
                    UserCreatorId = userId
                });

                var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                updateContext(dbContext);

                var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                result.Add(createInventoryOperationResult(createdTransaction));

                #endregion

                return result;
            }
        }

        public long PriceFuelReportDetailDecrementalCorrectionDefaultPricing(FuelReportDetail fuelReportDetail, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                var result = new List<InventoryOperation>();

                var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
                var transactionReferenceType = InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION;
                var reference = getFuelReportDetailDecrementalCorrectionReference(dbContext, fuelReportDetail);


                if (reference != null)
                {
                    var inventoryTransactionId = (int)reference.First().OperationId;
                    var issueTransactionItem = dbContext.Inventory_TransactionItem.Single(
                            tip =>
                                tip.TransactionId == inventoryTransactionId &&
                                tip.GoodId == (int)fuelReportDetail.Good.SharedGoodId);


                    if (issueTransactionItem.Inventory_Transaction.Status == 3 || issueTransactionItem.Inventory_Transaction.Status == 4)
                    {
                    }
                    else
                    {
                        try
                        {
                            string pricingMessage;
                            priceIssuedItemInFIFO(dbContext, issueTransactionItem.Id, transactionReferenceType + " FIFO Pricing", userId,
                                                  out pricingMessage, transactionReferenceType,
                                                  transactionReferenceNumber);

                            return reference.First().OperationId;
                        }
                        catch
                        {
                        }
                    }

                    return reference.First().OperationId;
                }

                return -1;
            }
        }

        public long PriceFuelReportDetailDecrementalCorrectionUsingPricingReference(FuelReportDetail fuelReportDetail, long pricingReferenceId, string pricingReferenceType, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                var result = new List<InventoryOperation>();

                #region Incremental Correction

                var transactionReferenceNumber = fuelReportDetail.Id.ToString();

                var reference = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION, transactionReferenceNumber);

                if (reference != null)
                {
                    //string transactionCode;
                    //string transactionMessage;

                    var transaction = dbContext.Inventory_Transaction.Single(t => t.Id == (int)reference.First().OperationId);

                    if (transaction.Status == 3 || transaction.Status == 4)
                    {
                    }
                    else
                    {
                        transaction.PricingReferenceId = (int)pricingReferenceId;

                        dbContext.SaveChanges();

                        string pricingMessage;

                        priceSuspendedTransactionUsingReference(dbContext, (int)reference.First().OperationId, pricingReferenceType, null, userId, out pricingMessage, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION, transactionReferenceNumber);
                    }

                    return reference.First().OperationId;
                }
                else
                {
                    //throw new InvalidOperation("FR Decremental Correction Edit", "FueReport  Incremental Correction edit is invalid");
                    //var transactionItem = dbContext.Inventory_TransactionItem.Where(ti => ti.TransactionId == reference.OperationId);
                }

                return -1;

                #endregion
            }
        }

        //================================================================================

        public List<InventoryOperation> ManageScrap(Scrap scrap, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var transactionReferenceNumber = scrap.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.SCRAP_ISSUE;

                    var result = new List<InventoryOperation>();

                    string transactionCode;
                    string transactionMessage;

                    var operationReference = issue(
                        dbContext,
                        (int)scrap.VesselInCompany.CompanyId,
                        (int)scrap.VesselInCompany.VesselInInventory.Id,
                        getTimeBucketId(dbContext, scrap.ScrapDate),
                        scrap.ScrapDate,
                        getScrapStoreType(dbContext),
                        null,
                        null,
                        transactionReferenceType,
                        transactionReferenceNumber,
                        userId,
                        out transactionCode,
                        out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();

                    foreach (var scrapDetail in scrap.ScrapDetails)
                    {
                        transactionItem.Add(new Inventory_TransactionItem()
                        {
                            GoodId = (int)scrapDetail.Good.SharedGoodId,
                            CreateDate = DateTime.Now,
                            Description = "Scrap > " + scrapDetail.Good.Code,
                            QuantityAmount = new decimal(scrapDetail.ROB),
                            QuantityUnitId = getMeasurementUnitId(dbContext, scrapDetail.Unit.Abbreviation),
                            TransactionId = (int)operationReference.OperationId,
                            UserCreatorId = userId
                        });
                    }

                    var registeredTransactionIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    string issuedItemsPricingMessage;

                    var pricingTransactionIds = registeredTransactionIds.Select(id => new Inventory_TransactionItemPricingId() { Id = id, Description = "Scrap FIFO Pricing" });

                    priceIssuedItemsInFIFO(dbContext, pricingTransactionIds, userId, out issuedItemsPricingMessage, transactionReferenceType, transactionReferenceNumber);

                    deactivateWarehouse(dbContext, (int)scrap.VesselInCompany.VesselInInventory.Id, scrap.ScrapDate, userId);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    //transaction.Commit();

                    return result;
                }
            }
        }

        private List<Inventory_OperationReference> getScrapReference(DataContainer dbContext, Scrap scrap)
        {
            var transactionReferenceNumber = scrap.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.SCRAP_ISSUE, transactionReferenceNumber);
            return references;
        }

        public Inventory_OperationReference GetScrapReference(Scrap scrap)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getScrapReference(dbContext, scrap).FirstOrDefault();
            }
        }

        //================================================================================

        private int getVesselActivationStoreType(DataContainer dbContext)
        {
            return dbContext.Inventory_StoreType.Single(s => s.Code == (short)InventoryStoreTypesCode.Vessel_Activation).Id;
            //return 18;
        }

        //================================================================================

        private int getScrapStoreType(DataContainer dbContext)
        {
            return dbContext.Inventory_StoreType.Single(s => s.Code == (short)InventoryStoreTypesCode.Scrap).Id;
            //return 18;
        }

        //================================================================================

        public InventoryOperation ManageOrderItemBalance(OrderItemBalance orderItemBalance, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    //Finding relevant Receipt Item
                    //var receiptReferenceNumber = orderItemBalance.FuelReportDetail.GetInventoryTransactionReferenceNumber();
                    var receiptReferences = getFuelReportDetailReceiveReference(dbContext, orderItemBalance.FuelReportDetail);

                    var receiptReference = receiptReferences.OrderBy(r => r.RegistrationDate).LastOrDefault();

                    var receiptReferenceTransactionItem = dbContext.Inventory_TransactionItem.Single(
                        tip =>
                            tip.TransactionId == (int)receiptReference.OperationId &&
                            tip.GoodId == (int)orderItemBalance.FuelReportDetail.Good.SharedGoodId);

                    var receiptPriceReferenceNumber = orderItemBalance.GetOrderItemBalancePricingReferenceNumber();

                    decimal price = orderItemBalance.InvoiceItem.Price;

                    if (orderItemBalance.PairingInvoiceItem != null)
                        if (orderItemBalance.InvoiceItem.Invoice.CurrencyId == orderItemBalance.PairingInvoiceItem.Invoice.CurrencyId)
                        {
                            price += orderItemBalance.PairingInvoiceItem.Price;
                        }
                        else
                        {
                            var rate = getExchangeRate(dbContext, orderItemBalance.PairingInvoiceItem.Invoice.Currency.Abbreviation, orderItemBalance.InvoiceItem.Invoice.Currency.Abbreviation, orderItemBalance.PairingInvoiceItem.Invoice.InvoiceDate);

                            price += orderItemBalance.PairingInvoiceItem.Price * rate;
                        }

                    var transactionItemPrice = new Inventory_TransactionItemPrice()
                    {
                        TransactionItemId = receiptReferenceTransactionItem.Id,
                        QuantityUnitId = getMeasurementUnitId(dbContext, orderItemBalance.UnitCode),
                        QuantityAmount = orderItemBalance.QuantityAmountInMainUnit,
                        PriceUnitId = getCurrencyId(dbContext, orderItemBalance.InvoiceItem.Invoice.Currency.Abbreviation),
                        Fee = price / orderItemBalance.QuantityAmountInMainUnit,
                        RegistrationDate = receiptReferenceTransactionItem.Inventory_Transaction.RegistrationDate,
                        Description = "Received Good Pricing > " + orderItemBalance.FuelReportDetail.Good.Code,
                        UserCreatorId = userId
                    };

                    string pricingMessage;

                    priceTransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE, receiptPriceReferenceNumber);

                    var pricingOperationReference = getFuelReportDetailReceivePricingReference(dbContext, orderItemBalance);

                    var result = new InventoryOperation(
                        pricingOperationReference.First().OperationId,
                           actionNumber: string.Format("{0}/{1}/Invoice|{2}", (InventoryOperationType)pricingOperationReference.First().OperationType, pricingOperationReference.First().OperationId, orderItemBalance.InvoiceItem.Invoice.InvoiceNumber),
                           actionDate: DateTime.Now,
                           actionType: InventoryActionType.Pricing);

                    //transaction.Commit();

                    return result;
                }
            }
        }

        //================================================================================

        private List<Inventory_OperationReference> getFuelReportDetailReceivePricingReference(DataContainer dbContext, OrderItemBalance orderItemBalance)
        {
            var receiptPriceReferenceNumber = orderItemBalance.GetOrderItemBalancePricingReferenceNumber();
            var pricingOperationReferences = findInventoryOperationReference(dbContext, InventoryOperationType.Pricing, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE, receiptPriceReferenceNumber);
            return pricingOperationReferences;
        }
        public Inventory_OperationReference GetFuelReportDetailReceivePricingReference(OrderItemBalance orderItemBalance)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getFuelReportDetailReceivePricingReference(dbContext, orderItemBalance).FirstOrDefault();
            }
        }

        //================================================================================

        private List<Inventory_OperationReference> getFuelReportDetailCorrectionPricingReference(DataContainer dbContext, FuelReportDetail fuelReportDetail)
        {
            if (!fuelReportDetail.CorrectionType.HasValue) return null;

            var pricingReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
            var pricingOperationReferences = findInventoryOperationReference(dbContext, InventoryOperationType.Pricing,
                fuelReportDetail.CorrectionType.Value == CorrectionTypes.Plus ?
                InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION :
                InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION, pricingReferenceNumber);
            return pricingOperationReferences;
        }
        public Inventory_OperationReference GetFuelReportDetailCorrectionPricingReference(FuelReportDetail fuelReportDetail)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getFuelReportDetailCorrectionPricingReference(dbContext, fuelReportDetail).FirstOrDefault();
            }
        }

        //================================================================================

        private int convertCharterInTypeToStoreType(DataContainer dbContext, CharterIn charterIn)
        {
            short code = 0;

            switch (charterIn.CharterType)
            {
                case CharterType.Start:
                    code = (short)InventoryStoreTypesCode.Charter_In_Start;
                    break;

                case CharterType.End:
                    code = (short)InventoryStoreTypesCode.Charter_In_End;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }

        private int convertCharterOutTypeToStoreType(DataContainer dbContext, CharterOut charterOut)
        {
            short code = 0;

            switch (charterOut.CharterType)
            {
                case CharterType.Start:
                    code = (short)InventoryStoreTypesCode.Charter_Out_Start;
                    break;

                case CharterType.End:
                    code = (short)InventoryStoreTypesCode.Charter_Out_End;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dbContext.Inventory_StoreType.Single(s => s.Code == code).Id;
        }


        public int getAdjustmentReceiptType(DataContainer dbContainer)
        {
            return dbContainer.Inventory_StoreType.Single(c => c.Code == (short)InventoryStoreTypesCode.Adjustment_Receipt).Id;
            //return dbContainer.Inventory_StoreType.Single(c => c.Code == (short)InventoryStoreTypesCode.Charter_In_Start_Adjustment_Receipt).Id;
        }
        public int getAdjustmentIssueType(DataContainer dbContainer)
        {
            return dbContainer.Inventory_StoreType.Single(c => c.Code == (short)InventoryStoreTypesCode.Adjustment_Issue).Id;
            //return dbContainer.Inventory_StoreType.Single(c => c.Code == (short)InventoryStoreTypesCode.Charter_In_Start_Adjustment_Issue).Id;
        }

        //================================================================================

        public List<InventoryOperation> ManageCharterInStart(CharterIn charterInStart, int userId)
        {
            if (charterInStart.CharterType != CharterType.Start)
                throw new InvalidArgument("The given entity is not Charter In Start", "charterInStart");

            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var transactionReferenceNumber = charterInStart.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT;

                    var result = new List<InventoryOperation>();

                    this.activateWarehouse(dbContext, charterInStart.VesselInCompany.VesselInInventory.Id, charterInStart.ActionDate, userId);

                    string transactionCode;
                    string transactionMessage;

                    var operationReference = receipt(
                              dbContext,
                            (int)charterInStart.VesselInCompany.CompanyId,
                            (int)charterInStart.VesselInCompany.VesselInInventory.Id,
                            getTimeBucketId(dbContext, charterInStart.ActionDate),
                            charterInStart.ActionDate,
                              convertCharterInTypeToStoreType(dbContext, charterInStart),
                                null,
                        null,
                        transactionReferenceType,
                              transactionReferenceNumber,
                              userId,
                              out transactionCode,
                              out transactionMessage);

                    //TODO: Items
                    string transactionItemMessage;

                    var transactionItems = new List<Inventory_TransactionItem>();

                    foreach (var charterItem in charterInStart.CharterItems)
                    {
                        transactionItems.Add(new Inventory_TransactionItem()
                        {
                            GoodId = (int)charterItem.Good.SharedGoodId,
                            CreateDate = DateTime.Now,
                            Description = "Charter In Start > " + charterItem.Good.Code,
                            QuantityAmount = charterItem.Rob,
                            QuantityUnitId = getMeasurementUnitId(dbContext, charterItem.GoodUnit.Abbreviation),
                            TransactionId = (int)operationReference.OperationId,
                            UserCreatorId = userId
                        });
                    }

                    addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItems, userId, out transactionItemMessage);

                    //Manual Items Pricing
                    var registeredTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == (int)operationReference.OperationId);


                    var transactionItemPrices = new List<Inventory_TransactionItemPrice>();

                    foreach (var charterItem in charterInStart.CharterItems)
                    {
                        var registeredTransactionItem = registeredTransaction.Inventory_TransactionItem.Single(ti => ti.GoodId == charterItem.Good.SharedGoodId);

                        var transactionItemPrice = new Inventory_TransactionItemPrice()
                        {
                            TransactionItemId = registeredTransactionItem.Id,
                            QuantityUnitId = getMeasurementUnitId(dbContext, charterItem.GoodUnit.Abbreviation),
                            QuantityAmount = charterItem.Rob,
                            PriceUnitId = getCurrencyId(dbContext, charterInStart.Currency.Abbreviation),
                            Fee = charterItem.Fee,
                            RegistrationDate = charterInStart.ActionDate,
                            Description = "Charter In Start Pricing > " + charterItem.Good.Code,
                            UserCreatorId = userId
                        };

                        transactionItemPrices.Add(transactionItemPrice);
                        //priceInventory_TransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage, CHARTER_IN_START_RECEIPT_PRICING, charterItem.Id.ToString());
                    }

                    string pricingMessage;
                    priceTransactionItemsManually(dbContext, transactionItemPrices, userId, out pricingMessage, transactionReferenceType, transactionReferenceNumber);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    //transaction.Commit();

                    return result;
                }
            }
        }

        private List<Inventory_OperationReference> getCharterInStartReference(DataContainer dbContext, CharterIn charterInStart)
        {
            var transactionReferenceNumber = charterInStart.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT, transactionReferenceNumber);
            return references;
        }
        public Inventory_OperationReference GetCharterInStartReference(CharterIn charterInStart)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterInStartReference(dbContext, charterInStart).FirstOrDefault();
            }
        }

        //================================================================================

        public List<InventoryOperation> ManageCharterInEnd(CharterIn charterInEnd, int userId, bool inventoryShouldBeDeactivated /*,long? lastIssuedVoyageInventoryOperationId*/)
        {
            if (charterInEnd.CharterType != CharterType.End)
                throw new InvalidArgument("The given entity is not Charter In End", "charterInEnd");

            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var transactionReferenceNumber = charterInEnd.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE;

                    var result = new List<InventoryOperation>();

                    string transactionCode;
                    string transactionMessage;


                    var goodsIncrementalOperationQuantities = new List<GoodOperationQuantity>();
                    var goodsDecrementalOperationQuantities = new List<GoodOperationQuantity>();


                    foreach (var charterItem in charterInEnd.CharterItems)
                    {
                        var goodInventoryAvailableQuantity = this.getInventoryQuantity(dbContext, charterItem.Good.SharedGoodId, charterItem.GoodUnit.Abbreviation, charterInEnd.VesselInCompany.VesselInInventory.Id, charterInEnd.ActionDate);

                        if (goodInventoryAvailableQuantity > charterItem.Rob)
                        {

                            goodsDecrementalOperationQuantities.Add(
                                new GoodOperationQuantity()
                                {
                                    Good = charterItem.Good,
                                    QuantityAmount = goodInventoryAvailableQuantity - charterItem.Rob,
                                    MeasurementUnit = charterItem.GoodUnit
                                }
                            );

                            result.AddRange(performDecrementalAdjustmentInFIFO(dbContext, charterInEnd.ChartererId.Value, charterInEnd.VesselInCompany.VesselInInventory.Id,
                                getCharterInEndDecrementalAdjustmentStoreType(dbContext),
                                new List<GoodOperationQuantity>()
                                    {
                                        new GoodOperationQuantity()
                                        {
                                            Good = charterItem.Good,
                                            QuantityAmount = goodInventoryAvailableQuantity - charterItem.Rob,
                                            MeasurementUnit = charterItem.GoodUnit
                                        }
                                    },
                                InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION,
                                transactionReferenceNumber,
                                //InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION,
                                charterInEnd.ActionDate,
                                userId,
                                charterInEnd.VesselInCompany.Code));
                        }
                        else if (goodInventoryAvailableQuantity < charterItem.Rob)
                        {
                            //if (!lastIssuedVoyageInventoryOperationId.HasValue)
                            //    throw new BusinessRuleException("", "No Voyage is issued yet to be used for CharterInEnd correction.");

                            goodsIncrementalOperationQuantities.Add(
                               new GoodOperationQuantity()
                               {
                                   Good = charterItem.Good,
                                   QuantityAmount = charterItem.Rob - goodInventoryAvailableQuantity,
                                   MeasurementUnit = charterItem.GoodUnit
                               }
                            );

                            Inventory_Transaction lastIssuedVoyageInventoryOperation = this.getLastEOVInventoryTransaction(dbContext, charterItem.Good.SharedGoodId, charterInEnd.VesselInCompany.VesselInInventory.Id);

                            if (lastIssuedVoyageInventoryOperation == null)
                                throw new BusinessRuleException("", "No Voyage is issued yet to be used for CharterInEnd '" + charterItem.Good.Code + "' correction.");

                            result.AddRange(performIncrementalAdjustmentByPricingReference(dbContext, charterInEnd.ChartererId.Value, charterInEnd.VesselInCompany.VesselInInventory.Id,
                                getCharterInEndIncrementalAdjustmentStoreType(dbContext),
                                new List<GoodOperationQuantity>(){new GoodOperationQuantity()
                                       {
                                           Good = charterItem.Good,
                                           QuantityAmount = charterItem.Rob - goodInventoryAvailableQuantity,
                                           MeasurementUnit = charterItem.GoodUnit
                                       }},
                                InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION,
                                transactionReferenceNumber,
                                //InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION,
                                lastIssuedVoyageInventoryOperation.Id,
                                charterInEnd.ActionDate,
                                userId,
                                charterInEnd.VesselInCompany.Code));
                        }
                    }

                    //if (goodsDecrementalOperationQuantities.Count > 0)
                    //    result.AddRange(
                    //            performDecrementalAdjustmentInFIFO(dbContext, charterInEnd.ChartererId.Value, charterInEnd.VesselInCompany.VesselInInventory.Id,
                    //                getCharterInEndDecrementalAdjustmentStoreType(dbContext),
                    //                goodsDecrementalOperationQuantities,
                    //                InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION,
                    //                transactionReferenceNumber,
                    //                InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION_PRICING,
                    //                charterInEnd.ActionDate,
                    //                userId,
                    //                charterInEnd.VesselInCompany.Code)
                    //            );

                    //if (goodsIncrementalOperationQuantities.Count > 0)
                    //    result.AddRange(
                    //           performIncrementalAdjustmentByPricingReference(dbContext, charterInEnd.ChartererId.Value, charterInEnd.VesselInCompany.VesselInInventory.Id,
                    //                getCharterInEndIncrementalAdjustmentStoreType(dbContext),
                    //                goodsIncrementalOperationQuantities,
                    //                InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION,
                    //                transactionReferenceNumber,
                    //                InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION_PRICING,
                    //                lastIssuedVoyageInventoryOperationId.Value,
                    //                charterInEnd.ActionDate,
                    //                userId,
                    //                charterInEnd.VesselInCompany.Code)
                    //           );


                    var operationReference = issue(
                        dbContext,
                        (int)charterInEnd.VesselInCompany.CompanyId,
                        (int)charterInEnd.VesselInCompany.VesselInInventory.Id,
                        getTimeBucketId(dbContext, charterInEnd.ActionDate),
                        charterInEnd.ActionDate,
                        convertCharterInTypeToStoreType(dbContext, charterInEnd),
                        null,
                        null,
                        transactionReferenceType,
                        transactionReferenceNumber,
                        userId,
                        out transactionCode,
                        out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();

                    foreach (var charterItem in charterInEnd.CharterItems)
                    {
                        transactionItem.Add(new Inventory_TransactionItem()
                        {
                            GoodId = (int)charterItem.Good.SharedGoodId,
                            CreateDate = DateTime.Now,
                            Description = "Charter In End > " + charterItem.Good.Code,
                            QuantityAmount = charterItem.Rob,
                            QuantityUnitId = getMeasurementUnitId(dbContext, charterItem.GoodUnit.Abbreviation),
                            TransactionId = (int)operationReference.OperationId,
                            UserCreatorId = userId
                        });
                    }

                    var registeredTransactionIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    string issuedItemsPricingMessage;

                    var pricingTransactionIds = registeredTransactionIds.Select(id => new Inventory_TransactionItemPricingId() { Id = id, Description = "Charter-In End FIFO Pricing" });

                    try
                    {
                        priceIssuedItemsInFIFO(dbContext, pricingTransactionIds, userId, out issuedItemsPricingMessage, transactionReferenceType, transactionReferenceNumber);

                    }
                    catch
                    {

                    }
                    if (inventoryShouldBeDeactivated)
                        deactivateWarehouse(dbContext, charterInEnd.VesselInCompany.VesselInInventory.Id, charterInEnd.ActionDate, userId);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    //transaction.Commit();

                    return result;
                }
            }
        }

        private List<Inventory_OperationReference> getCharterInEndReference(DataContainer dbContext, CharterIn charterInEnd)
        {
            var transactionReferenceNumber = charterInEnd.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE, transactionReferenceNumber);
            return references;
        }

        public List<Inventory_OperationReference> GetCharterInEndReference(CharterIn charterInEnd)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterInEndReference(dbContext, charterInEnd).OrderBy(o => o.RegistrationDate).ThenBy(o => o.Id).ToList();
            }
        }
        //================================================================================

        private List<Inventory_OperationReference> getCharterInEndIssueTrustReceiptsReference(DataContainer dbContext, CharterIn charterInEnd)
        {
            var transactionReferenceNumber = charterInEnd.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.ISSUE_TOTAL_RECEIVED_TRUST, transactionReferenceNumber);
            return references;
        }
        public Inventory_OperationReference GetCharterInEndIssueTrustReceiptsReference(CharterIn charterInEnd)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterInEndIssueTrustReceiptsReference(dbContext, charterInEnd).FirstOrDefault();
            }
        }

        private List<Inventory_OperationReference> getCharterInEndIncrementalCorrectionReference(DataContainer dbContext, CharterIn charterInEnd)
        {
            var transactionReferenceNumber = charterInEnd.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION, transactionReferenceNumber);
            return references;
        }
        public List<Inventory_OperationReference> GetCharterInEndIncrementalCorrectionReference(CharterIn charterInEnd)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterInEndIncrementalCorrectionReference(dbContext, charterInEnd).OrderBy(o => o.RegistrationDate).ThenBy(o => o.Id).ToList();
            }
        }

        private List<Inventory_OperationReference> getCharterInEndDecrementalCorrectionReference(DataContainer dbContext, CharterIn charterInEnd)
        {
            var transactionReferenceNumber = charterInEnd.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION, transactionReferenceNumber);
            return references;
        }
        public List<Inventory_OperationReference> GetCharterInEndDecrementalCorrectionReference(CharterIn charterInEnd)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterInEndDecrementalCorrectionReference(dbContext, charterInEnd).OrderBy(o => o.RegistrationDate).ThenBy(o => o.Id).ToList();
            }
        }


        public List<InventoryOperation> ManageCharterOutStart(CharterOut charterOutStart, int userId, bool inventoryShouldBeDeactivated /*, long? lastIssuedVoyageInventoryOperationId*/)
        {
            if (charterOutStart.CharterType != CharterType.Start)
                throw new InvalidArgument("The given entity is not Charter Out Start", "charterOutStart");

            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    List<InventoryOperation> result = new List<InventoryOperation>();

                    var transactionReferenceNumber = charterOutStart.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.CHARTER_OUT_START_ISSUE;

                    string transactionCode;
                    string transactionMessage;

                    var goodsIncrementalOperationQuantities = new List<GoodOperationQuantity>();
                    var goodsDecrementalOperationQuantities = new List<GoodOperationQuantity>();

                    foreach (var charterItem in charterOutStart.CharterItems)
                    {
                        var goodInventoryAvailableQuantity = this.getInventoryQuantity(dbContext, charterItem.Good.SharedGoodId, charterItem.GoodUnit.Abbreviation, charterOutStart.VesselInCompany.VesselInInventory.Id, charterOutStart.ActionDate);

                        if (goodInventoryAvailableQuantity > charterItem.Rob)
                        {
                            goodsDecrementalOperationQuantities.Add(
                                new GoodOperationQuantity()
                                {
                                    Good = charterItem.Good,
                                    QuantityAmount = goodInventoryAvailableQuantity - charterItem.Rob,
                                    MeasurementUnit = charterItem.GoodUnit
                                }
                            );

                            result.AddRange(performDecrementalAdjustmentInFIFO(dbContext, charterOutStart.OwnerId.Value, charterOutStart.VesselInCompany.VesselInInventory.Id,
                                getCharterOutStartDecrementalAdjustmentStoreType(dbContext),
                                new List<GoodOperationQuantity>(){new GoodOperationQuantity()
                                    {
                                        Good = charterItem.Good,
                                        QuantityAmount = goodInventoryAvailableQuantity - charterItem.Rob,
                                        MeasurementUnit = charterItem.GoodUnit
                                    }},
                                InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION,
                                transactionReferenceNumber,
                                //InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION_PRICING,
                                charterOutStart.ActionDate.Subtract(new TimeSpan(0, 0, 0, 1)),
                                userId,
                                charterOutStart.VesselInCompany.Code));
                        }
                        else if (goodInventoryAvailableQuantity < charterItem.Rob)
                        {
                            //if (!lastIssuedVoyageInventoryOperationId.HasValue)
                            //    throw new BusinessRuleException("", "No Voyage is issued yet to be used for CharterOutStart correction.");

                            goodsIncrementalOperationQuantities.Add(
                               new GoodOperationQuantity()
                               {
                                   Good = charterItem.Good,
                                   QuantityAmount = charterItem.Rob - goodInventoryAvailableQuantity,
                                   MeasurementUnit = charterItem.GoodUnit
                               }
                            );

                            Inventory_Transaction lastIssuedVoyageInventoryOperation = this.getLastEOVInventoryTransaction(dbContext, charterItem.Good.SharedGoodId, charterOutStart.VesselInCompany.VesselInInventory.Id);

                            if (lastIssuedVoyageInventoryOperation == null)
                                throw new BusinessRuleException("", "No Voyage is issued yet to be used for CharterOutStart '" + charterItem.Good.Code + "' correction.");

                            result.AddRange(performIncrementalAdjustmentByPricingReference(dbContext, charterOutStart.OwnerId.Value, charterOutStart.VesselInCompany.VesselInInventory.Id,
                                getCharterOutStartIncrementalAdjustmentStoreType(dbContext),
                                new List<GoodOperationQuantity>()
                                    {
                                        new GoodOperationQuantity()
                                        {
                                            Good = charterItem.Good,
                                            QuantityAmount = charterItem.Rob - goodInventoryAvailableQuantity,
                                            MeasurementUnit = charterItem.GoodUnit
                                        }
                                    },
                                InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION,
                                transactionReferenceNumber,
                                //InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION_PRICING,
                                lastIssuedVoyageInventoryOperation.Id,
                                charterOutStart.ActionDate.Subtract(new TimeSpan(0, 0, 0, 1)),
                                userId,
                                charterOutStart.VesselInCompany.Code));
                        }
                    }


                    //if (goodsDecrementalOperationQuantities.Count > 0)
                    //    result.AddRange(
                    //            performDecrementalAdjustmentInFIFO(dbContext, charterOutStart.OwnerId.Value, charterOutStart.VesselInCompany.VesselInInventory.Id,
                    //                getCharterOutStartDecrementalAdjustmentStoreType(dbContext),
                    //                goodsDecrementalOperationQuantities,
                    //                InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION,
                    //                transactionReferenceNumber,
                    //                InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION_PRICING,
                    //                charterOutStart.ActionDate.Subtract(new TimeSpan(0,0,0,1)),
                    //                userId,
                    //                charterOutStart.VesselInCompany.Code)
                    //            );

                    //if (goodsIncrementalOperationQuantities.Count > 0)
                    //    result.AddRange(
                    //           performIncrementalAdjustmentByPricingReference(dbContext, charterOutStart.OwnerId.Value, charterOutStart.VesselInCompany.VesselInInventory.Id,
                    //                getCharterOutStartIncrementalAdjustmentStoreType(dbContext),
                    //                goodsIncrementalOperationQuantities,
                    //                InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION,
                    //                transactionReferenceNumber,
                    //                InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION_PRICING,
                    //                lastIssuedVoyageInventoryOperationId.Value,
                    //                charterOutStart.ActionDate.Subtract(new TimeSpan(0, 0, 0, 1)),
                    //                userId,
                    //                charterOutStart.VesselInCompany.Code)
                    //           );


                    var operationReference = issue(
                        dbContext,
                        (int)charterOutStart.VesselInCompany.CompanyId,
                        (int)charterOutStart.VesselInCompany.VesselInInventory.Id,
                        getTimeBucketId(dbContext, charterOutStart.ActionDate),
                        charterOutStart.ActionDate,
                        convertCharterOutTypeToStoreType(dbContext, charterOutStart),
                        null,
                        null,
                        transactionReferenceType,
                        transactionReferenceNumber,
                        userId,
                        out transactionCode,
                        out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();

                    foreach (var charterItem in charterOutStart.CharterItems)
                    {
                        transactionItem.Add(new Inventory_TransactionItem()
                        {
                            GoodId = (int)charterItem.Good.SharedGoodId,
                            CreateDate = DateTime.Now,
                            Description = "Charter Out Start > " + charterItem.Good.Code,
                            QuantityAmount = charterItem.Rob,
                            QuantityUnitId = getMeasurementUnitId(dbContext, charterItem.GoodUnit.Abbreviation),
                            TransactionId = (int)operationReference.OperationId,
                            UserCreatorId = userId
                        });
                    }

                    var registeredTransactionIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    string issuedItemsPricingMessage;

                    var pricingTransactionIds = registeredTransactionIds.Select(id => new Inventory_TransactionItemPricingId() { Id = id, Description = "Charter-Out Start FIFO Pricing" });
                    try
                    {
                        priceIssuedItemsInFIFO(dbContext, pricingTransactionIds, userId, out issuedItemsPricingMessage, transactionReferenceType, transactionReferenceNumber);
                    }
                    catch
                    {

                    }
                    if (inventoryShouldBeDeactivated)
                        deactivateWarehouse(dbContext, charterOutStart.VesselInCompany.VesselInInventory.Id, charterOutStart.ActionDate, userId);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    //transaction.Commit();

                    return result;
                }
            }
        }

        private List<Inventory_OperationReference> getCharterOutStartReference(DataContainer dbContext, CharterOut charterOutStart)
        {
            var transactionReferenceNumber = charterOutStart.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.CHARTER_OUT_START_ISSUE, transactionReferenceNumber);
            return references;
        }
        public List<Inventory_OperationReference> GetCharterOutStartReference(CharterOut charterOutStart)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterOutStartReference(dbContext, charterOutStart).OrderBy(o => o.RegistrationDate).ThenBy(o => o.Id).ToList();
            }
        }

        private List<Inventory_OperationReference> getCharterOutStartIssueTrustReceiptsReference(DataContainer dbContext, CharterOut charterOutStart)
        {
            var transactionReferenceNumber = charterOutStart.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.ISSUE_TOTAL_RECEIVED_TRUST, transactionReferenceNumber);
            return references;
        }

        public Inventory_OperationReference GetCharterOutStartIssueTrustReceiptsReference(CharterOut charterOutStart)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterOutStartIssueTrustReceiptsReference(dbContext, charterOutStart).FirstOrDefault();
            }
        }

        private List<Inventory_OperationReference> getCharterOutStartIncrementalCorrectionReference(DataContainer dbContext, CharterOut charterOutStart)
        {
            var transactionReferenceNumber = charterOutStart.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION, transactionReferenceNumber);
            return references;
        }
        public List<Inventory_OperationReference> GetCharterOutStartIncrementalCorrectionReference(CharterOut charterOutStart)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterOutStartIncrementalCorrectionReference(dbContext, charterOutStart).OrderBy(o => o.RegistrationDate).ThenBy(o => o.Id).ToList();
            }
        }

        private List<Inventory_OperationReference> getCharterOutStartDecrementalCorrectionReference(DataContainer dbContext, CharterOut charterOutStart)
        {
            var transactionReferenceNumber = charterOutStart.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Issue, InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION, transactionReferenceNumber);
            return references;
        }
        public List<Inventory_OperationReference> GetCharterOutStartDecrementalCorrectionReference(CharterOut charterOutStart)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterOutStartDecrementalCorrectionReference(dbContext, charterOutStart).OrderBy(o => o.RegistrationDate).ThenBy(o => o.Id).ToList();
            }
        }

        private Inventory_Transaction getLastEOVInventoryTransaction(DataContainer dbContext, long sharedGoodId, long warehouseId)
        {
            var eovStoreTypeId = getEndOfVoyageConsumptionStoreType(dbContext);

            return dbContext.Inventory_Transaction
                .Where(
                isTransactionFullyPriced.Predicate.And(
                    i => i.WarehouseId == warehouseId && i.StoreTypesId == eovStoreTypeId && i.Action == (byte)TransactionType.Issue).Compile())
                .OrderBy(i => i.Code).ThenBy(i => i.RegistrationDate)
                .LastOrDefault(i => i.Inventory_TransactionItem.Any(it => it.GoodId == sharedGoodId));
        }

        //================================================================================

        //TODO : vessel deactivation
        public List<InventoryOperation> ManageCharterOutEnd(CharterOut charterOutEnd, int userId)
        {
            if (charterOutEnd.CharterType != CharterType.End)
                throw new InvalidArgument("The given entity is not Charter Out End", "charterOutEnd");

            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var transactionReferenceNumber = charterOutEnd.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.CHARTER_OUT_END_RECEIPT;

                    var result = new List<InventoryOperation>();

                    this.activateWarehouse(dbContext, charterOutEnd.VesselInCompany.VesselInInventory.Id, charterOutEnd.ActionDate, userId);

                    string transactionCode;
                    string transactionMessage;

                    var operationReference = receipt(
                            dbContext,
                            (int)charterOutEnd.VesselInCompany.CompanyId,
                            (int)charterOutEnd.VesselInCompany.VesselInInventory.Id,
                            getTimeBucketId(dbContext, charterOutEnd.ActionDate),
                            charterOutEnd.ActionDate,
                            convertCharterOutTypeToStoreType(dbContext, charterOutEnd),
                            null,
                            null,
                            transactionReferenceType,
                            transactionReferenceNumber,
                            userId,
                            out transactionCode,
                            out transactionMessage);

                    //TODO: Items
                    string transactionItemMessage;

                    var transactionItems = new List<Inventory_TransactionItem>();

                    foreach (var charterItem in charterOutEnd.CharterItems)
                    {
                        transactionItems.Add(new Inventory_TransactionItem()
                        {
                            GoodId = (int)charterItem.Good.SharedGoodId,
                            CreateDate = DateTime.Now,
                            Description = "Charter Out End > " + charterItem.Good.Code,
                            QuantityAmount = charterItem.Rob,
                            QuantityUnitId = getMeasurementUnitId(dbContext, charterItem.GoodUnit.Abbreviation),
                            TransactionId = (int)operationReference.OperationId,
                            UserCreatorId = userId
                        });
                    }

                    addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItems, userId, out transactionItemMessage);

                    //Manual Items Pricing
                    var registeredTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == (int)operationReference.OperationId);


                    var transactionItemPrices = new List<Inventory_TransactionItemPrice>();

                    foreach (var charterItem in charterOutEnd.CharterItems)
                    {
                        var registeredTransactionItem = registeredTransaction.Inventory_TransactionItem.Single(ti => ti.GoodId == charterItem.Good.SharedGoodId);

                        var transactionItemPrice = new Inventory_TransactionItemPrice()
                        {
                            TransactionItemId = registeredTransactionItem.Id,
                            QuantityUnitId = getMeasurementUnitId(dbContext, charterItem.GoodUnit.Abbreviation),
                            QuantityAmount = charterItem.Rob,
                            PriceUnitId = getCurrencyId(dbContext, charterOutEnd.Currency.Abbreviation),
                            Fee = charterItem.Fee,
                            RegistrationDate = charterOutEnd.ActionDate,
                            Description = "Charter Out End Pricing > " + charterItem.Good.Code,
                            UserCreatorId = userId
                        };

                        transactionItemPrices.Add(transactionItemPrice);
                        //priceInventory_TransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage, CHARTER_OUT_END_RECEIPT_PRICING, charterItem.Id.ToString());
                    }

                    string pricingMessage;
                    priceTransactionItemsManually(dbContext, transactionItemPrices, userId, out pricingMessage, transactionReferenceType, transactionReferenceNumber);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    //transaction.Commit();

                    return result;
                }
            }
        }

        private List<Inventory_OperationReference> getCharterOutEndReference(DataContainer dbContext, CharterOut charterOutEnd)
        {
            var transactionReferenceNumber = charterOutEnd.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.CHARTER_OUT_END_RECEIPT, transactionReferenceNumber);
            return references;
        }
        public Inventory_OperationReference GetCharterOutEndReference(CharterOut charterOutEnd)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getCharterOutEndReference(dbContext, charterOutEnd).FirstOrDefault();
            }
        }

        public long GetMeasurementUnitId(string unitAbbreviation)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return this.getMeasurementUnitId(dbContext, unitAbbreviation);
            }
        }

        public long GetCurrencyId(string currencyAbbreviation)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return this.getCurrencyId(dbContext, currencyAbbreviation);
            }
        }

        //================================================================================

        public Inventory_Transaction GetTransaction(long transactionId, InventoryOperationType operationType)
        {
            TransactionType transactionType = mapOperationTypeToTransactionActionType(operationType);

            var dbContext = new DataContainer();

            //using (var dbContext = new DataContainer())
            {
                //dbContext.Configuration.LazyLoadingEnabled = false;
                //dbContext.Configuration.ProxyCreationEnabled = false;

                var foundTransaction = dbContext.Inventory_Transaction.FirstOrDefault(t => t.Id == transactionId && t.Action == (byte)transactionType);

                if (foundTransaction == null)
                    throw new ObjectNotFound("InventoryTransaction", transactionId);

                return foundTransaction;
            }
        }

        //================================================================================

        public Inventory_Transaction GetLastTransactionBefore(long transactionId, InventoryOperationType findingOperationType, string referenceType)
        {
            TransactionType transactionType = mapOperationTypeToTransactionActionType(findingOperationType);

            var dbContext = new DataContainer();

            var originalTransaction = dbContext.Inventory_Transaction.SingleOrDefault(t => t.Id == transactionId);

            if (originalTransaction == null)
                throw new ObjectNotFound("InventoryTransaction", transactionId);

            var foundTransaction = dbContext.Inventory_Transaction.Where(
                t =>
                    originalTransaction.RegistrationDate > t.RegistrationDate
                    && t.Code < originalTransaction.Code
                    && t.WarehouseId == originalTransaction.WarehouseId
                    && t.Action == (byte)transactionType
                    && t.ReferenceType == referenceType).OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate).LastOrDefault();

            return foundTransaction;
        }

        //================================================================================

        public Inventory_Transaction GetLastTransactionBefore(DateTime dateTime, long warehouseId, InventoryOperationType findingOperationType, string referenceType)
        {
            TransactionType transactionType = mapOperationTypeToTransactionActionType(findingOperationType);

            var dbContext = new DataContainer();

            var foundTransaction = dbContext.Inventory_Transaction.Where(t => t.WarehouseId == warehouseId && dateTime > t.RegistrationDate && t.Action == (byte)transactionType).OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate).LastOrDefault();

            return foundTransaction;
        }

        //================================================================================

        public Inventory_OperationReference GetFuelReportDetailReceiveReference(FuelReportDetail fuelReportDetail)
        {
            using (var dbContext = new DataContainer())
            {
                //var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
                //var operationReference = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE, transactionReferenceNumber);
                var operationReferences = this.getFuelReportDetailReceiveReference(dbContext, fuelReportDetail);

                return operationReferences.FirstOrDefault();
            }
        }

        //================================================================================

        public List<Inventory_OperationReference> GetFueReportDetailsReceiveOperationReference(List<FuelReportDetail> fuelReportDetails)
        {
            var result = new List<Inventory_OperationReference>();

            using (var dbContext = new DataContainer())
            {
                foreach (var fuelReportDetail in fuelReportDetails)
                {
                    //var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
                    //var operationReference = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE, transactionReferenceNumber);

                    var operationReference = getFuelReportDetailReceiveReference(dbContext, fuelReportDetail);

                    if (operationReference != null)
                        result.AddRange(operationReference);
                }

                return result;
            }
        }

        //================================================================================

        private TransactionType mapOperationTypeToTransactionActionType(InventoryOperationType operationType)
        {
            switch (operationType)
            {
                case InventoryOperationType.Receipt:
                    return TransactionType.Receipt;
                case InventoryOperationType.Issue:
                    return TransactionType.Issue;
                default:
                    throw new ArgumentOutOfRangeException("operationType");
            }
        }

        //================================================================================

        public decimal GetAverageFee(long transactionId, TransactionType actionType, long goodId, long unitId)
        {
            using (var dbContext = new DataContainer())
            {
                var transaction = dbContext.Inventory_Transaction.SingleOrDefault(
                    isTransactionFullyPriced.Predicate.And(
                    t => t.Action == (byte)actionType &&
                        t.Id == transactionId).Compile());

                if (transaction == null)
                    throw new ObjectNotFound("FullPricedTransaction", transactionId);

                var transactionItem = transaction.Inventory_TransactionItem.SingleOrDefault(ti => ti.GoodId == goodId);

                if (transactionItem == null)
                    throw new ObjectNotFound("FullPricedTransactionItem", goodId);

                var transactionItemPrices = transactionItem.Inventory_TransactionItemPrice;

                if (!transactionItemPrices.Any())
                    return 0;

                var totalTransactionQuantity = transactionItemPrices.Sum(tip => tip.QuantityAmount.Value);

                var totalTransactionPrice = transactionItemPrices.Sum(tip => tip.QuantityAmount.Value * tip.FeeInMainCurrency.Value);

                return totalTransactionPrice / totalTransactionQuantity;
            }
        }

        public decimal GetInventoryQuantity(long sharedGoodId, string unitAbbreviation, long vesselInInventoryId, DateTime? requestDateTime)
        {
            using (var dbContext = new DataContainer())
            {
                return this.getInventoryQuantity(dbContext, sharedGoodId, unitAbbreviation, vesselInInventoryId, requestDateTime);
            }
        }

        private decimal getInventoryQuantity(DataContainer dbContext, long sharedGoodId, string unitAbbreviation, long vesselInInventoryId, DateTime? requestDateTime)
        {
            var unitId = getMeasurementUnitId(dbContext, unitAbbreviation);

            return getInventoryQuantity(dbContext, sharedGoodId, unitId, vesselInInventoryId, requestDateTime);
        }

        private decimal getInventoryQuantity(DataContainer dbContext, long sharedGoodId, long inventoryQuantityUnitId, long vesselInInventoryId, DateTime? requestDateTime)
        {
            return dbContext.Database.SqlQuery<decimal>(
                "SELECT [Inventory].[GetInventoryQuantity](@GoodId, @WarehouseId, @RequestDateTime) AS InventoryQuantity",
                    new SqlParameter("@GoodId", sharedGoodId),
                    new SqlParameter("@WarehouseId", vesselInInventoryId),
                //new SqlParameter("@UnitId", inventoryQuantityUnitId),
                    new SqlParameter("@RequestDateTime", requestDateTime.HasValue ? requestDateTime as object : DBNull.Value)).Single();
        }

        //================================================================================

        public void PriceAllSuspendedIssuedItems(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int userId, out string message)
        {
            using (var dbContext = new DataContainer())
            {
                priceAllSuspendedIssuedItems(dbContext, companyId, warehouseId, fromDate, toDate, userId, out message);
            }
        }

        //================================================================================

        public void PriceAllSuspendedTransactionItemsUsingReference(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate,
            int userId, TransactionType? action, out string message)
        {
            using (var dbContext = new DataContainer())
            {
                priceAllSuspendedTransactionItemsUsingReference(dbContext, companyId, warehouseId, fromDate, toDate,
                    userId, action, out message);
            }
        }

        public void PriceAllSuspendedTransactions(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int userId, out string message)
        {
            message = string.Empty;

            using (var dbContext = new DataContainer())
            {
                var transactions = dbContext.Inventory_Transaction.Where(
                    t => (t.Action == (byte)InventoryOperationType.Issue || t.PricingReferenceId.HasValue)
                        && (t.Status != (byte)TransactionState.FullPriced && t.Status != (byte)TransactionState.Vouchered)
                        && (!companyId.HasValue || t.Inventory_Warehouse.CompanyId == companyId)
                        && (!warehouseId.HasValue || t.WarehouseId == warehouseId)
                        && (!toDate.HasValue || t.RegistrationDate <= toDate)
                    ).OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate).ToList();

                foreach (var transaction in transactions)
                {
                    var ts = ServiceLocator.Current.GetInstance<ITransactionScopeFactory>();

                    try
                    {
                        using (var transactionScope = ts.Create())
                        {
                            if (transaction.PricingReferenceId.HasValue)
                            {
                                priceSuspendedTransactionUsingReference(dbContext, transaction.Id, transaction.Description + " Pricing By Referernce", null, userId, out message, transaction.ReferenceType, transaction.ReferenceNo);
                            }
                            else
                            {   //Price in FIFO
                                var pricingIds = transaction.Inventory_TransactionItem.Select(ti => new Inventory_TransactionItemPricingId() { Id = ti.Id, Description = "FIFO Pricing > " + ti.Inventory_Good.Code });

                                priceIssuedItemsInFIFO(dbContext, pricingIds, userId, out message, transaction.ReferenceType, transaction.ReferenceNo);
                            }
                            transactionScope.Complete();
                        }
                    }
                    finally
                    {
                        ServiceLocator.Current.Release(ts);
                    }
                }
            }
        }

        public void TryGetFuelPurchasePricingReferences(long pricingOperationId, out long fuelReportDetailId, out long[] invoiceItemId)
        {
            using (var dbContext = new DataContainer())
            {
                var operationReference = dbContext.Inventory_OperationReference.Single(o => o.OperationType == (int)InventoryOperationType.Pricing && o.OperationId == pricingOperationId && o.ReferenceType == InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE);

                var referenceNumber = operationReference.ReferenceNumber;

                var ids = referenceNumber.Split(',');

                fuelReportDetailId = long.Parse(ids[0]);
                invoiceItemId = ids[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            }
        }

        public List<InventoryOperation> UpdateCountSubmitedReciptFlow<T, L>(IUpdateCountSubmitedReciptFactory<T, L> updateCountSubmitedReciptFactory, IGoodRepository goodRepository, Voyage voyage, long userId, decimal diffQuantity, decimal? oldFee) where T : class
        {
            throw new NotImplementedException();
        }

        public void RegisterInventory(long id, long companyId, string vesselCode, string name, string description, int userId)
        {
            var warehouseRepository = ServiceLocator.Current.GetInstance<IRepository<Inventory_Warehouse>>();

            Inventory_Warehouse inventory_Warehouse = new Inventory_Warehouse()
            {
                Code = vesselCode,
                CompanyId = companyId,
                CreateDate = DateTime.Now,
                Id = id,
                UserCreatorId = userId,
                IsActive = false,
                Name = name
            };

            warehouseRepository.Add(inventory_Warehouse);
        }

        //================================================================================

        public List<InventoryOperation> ManageFuelReportDetailIncrementalCorrectionUsingPricingReference(FuelReportDetail fuelReportDetail, long pricingReferenceId, string pricingReferenceType, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var result = new List<InventoryOperation>();

                    #region Incremental Correction

                    var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION;

                    string transactionCode;
                    string transactionMessage;

                    var operationReference =
                        receipt(
                            dbContext,
                            (int)fuelReportDetail.FuelReport.VesselInCompany.CompanyId,
                            (int)fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id,
                            getTimeBucketId(dbContext, fuelReportDetail.FuelReport.EventDate),
                            fuelReportDetail.FuelReport.EventDate,
                            convertFuelReportCorrectionTypeToStoreType(dbContext, fuelReportDetail),
                            (int)pricingReferenceId,
                            null,
                            transactionReferenceType,
                            transactionReferenceNumber,
                            userId,
                            out transactionCode,
                            out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();
                    transactionItem.Add(new Inventory_TransactionItem()
                    {
                        GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                        CreateDate = DateTime.Now,
                        Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                        QuantityAmount = (decimal?)fuelReportDetail.Correction,
                        QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                        TransactionId = (int)operationReference.OperationId,
                        UserCreatorId = userId
                    });

                    var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    string pricingMessage;

                    priceSuspendedTransactionUsingReference(dbContext, (int)operationReference.OperationId, pricingReferenceType, null, userId, out pricingMessage, transactionReferenceType, transactionReferenceNumber);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    #endregion

                    //transaction.Commit();

                    return result;
                }
            }
        }

        private List<Inventory_OperationReference> getFuelReportDetailIncrementalCorrectionReference(DataContainer dbContext, FuelReportDetail fuelReportDetail)
        {
            var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
            var references = findInventoryOperationReference(dbContext, InventoryOperationType.Receipt, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION, transactionReferenceNumber);
            return references;
        }

        public Inventory_OperationReference GetFuelReportDetailIncrementalCorrectionReference(FuelReportDetail fuelReportDetail)
        {
            using (DataContainer dbContext = new DataContainer())
            {
                return getFuelReportDetailIncrementalCorrectionReference(dbContext, fuelReportDetail).FirstOrDefault();
            }
        }

        //================================================================================

        public List<InventoryOperation> ManageFuelReportDetailIncrementalCorrectionDirectPricing(FuelReportDetail fuelReportDetail, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var result = new List<InventoryOperation>();

                    #region Incremental Correction

                    var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();
                    var transactionReferenceType = InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION;

                    string transactionCode;
                    string transactionMessage;

                    var operationReference =
                        receipt(
                              dbContext,
                              (int)fuelReportDetail.FuelReport.VesselInCompany.CompanyId,
                              (int)fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id,
                              getTimeBucketId(dbContext, fuelReportDetail.FuelReport.EventDate),
                              fuelReportDetail.FuelReport.EventDate,
                              convertFuelReportCorrectionTypeToStoreType(dbContext, fuelReportDetail),
                                null,
                            null,
                            transactionReferenceType,
                              transactionReferenceNumber,
                              userId,
                              out transactionCode,
                              out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();
                    transactionItem.Add(new Inventory_TransactionItem()
                    {
                        GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                        CreateDate = DateTime.Now,
                        Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                        QuantityAmount = (decimal?)fuelReportDetail.Correction,
                        QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                        TransactionId = (int)operationReference.OperationId,
                        UserCreatorId = userId
                    });

                    var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    //TODO: Items Pricing.

                    var transactionItemPrice = new Inventory_TransactionItemPrice()
                    {
                        TransactionItemId = transactionItemIds[0],
                        QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                        QuantityAmount = (decimal?)fuelReportDetail.Correction,
                        PriceUnitId = getCurrencyId(dbContext, fuelReportDetail.CorrectionPriceCurrency.Abbreviation),
                        Fee = fuelReportDetail.CorrectionPrice,
                        RegistrationDate = fuelReportDetail.FuelReport.EventDate,
                        Description = "Incremental Correction Direct Pricing > " + fuelReportDetail.Good.Code,
                        UserCreatorId = userId
                    };

                    string pricingMessage;

                    priceTransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage, transactionReferenceType, transactionReferenceNumber);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    #endregion

                    //transaction.Commit();

                    return result;
                }
            }
        }

        //================================================================================

        public List<InventoryOperation> ManageFuelReportDetailIncrementalCorrectionWithoutPricing(FuelReportDetail fuelReportDetail, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var result = new List<InventoryOperation>();

                    #region Incremental Correction

                    var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();

                    string transactionCode;
                    string transactionMessage;

                    var operationReference =
                        receipt(
                                dbContext,
                                (int)fuelReportDetail.FuelReport.VesselInCompany.CompanyId,
                                (int)fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id,
                                getTimeBucketId(dbContext, fuelReportDetail.FuelReport.EventDate),
                                fuelReportDetail.FuelReport.EventDate,
                                convertFuelReportCorrectionTypeToStoreType(dbContext, fuelReportDetail),
                                null,
                                null,
                                InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION,
                                transactionReferenceNumber,
                                userId,
                                out transactionCode,
                                out transactionMessage);

                    string transactionItemMessage;

                    var transactionItem = new List<Inventory_TransactionItem>();
                    transactionItem.Add(new Inventory_TransactionItem()
                                        {
                                            GoodId = (int)fuelReportDetail.Good.SharedGoodId,
                                            CreateDate = DateTime.Now,
                                            Description = fuelReportDetail.FuelReport.FuelReportType.ToString(),
                                            QuantityAmount = (decimal?)fuelReportDetail.Correction,
                                            QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                                            TransactionId = (int)operationReference.OperationId,
                                            UserCreatorId = userId
                                        });

                    var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                    updateContext(dbContext);

                    var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                    result.Add(createInventoryOperationResult(createdTransaction));

                    #endregion

                    //transaction.Commit();

                    return result;
                }
            }
        }

        public long PriceFuelReportDetailIncrementalCorrectionUsingPricingReference(FuelReportDetail fuelReportDetail, long pricingReferenceId, string pricingReferenceType, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                var result = new List<InventoryOperation>();

                #region Incremental Correction

                var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();

                var reference = getFuelReportDetailIncrementalCorrectionReference(dbContext, fuelReportDetail);

                if (reference != null)
                {
                    //string transactionCode;
                    //string transactionMessage;
                    var transactionId = (int)reference.FirstOrDefault().OperationId;

                    var transaction = dbContext.Inventory_Transaction.Single(t => t.Id == transactionId);

                    if (transaction.Status == 3 || transaction.Status == 4)
                    {
                    }
                    else
                    {
                        transaction.PricingReferenceId = (int)pricingReferenceId;

                        dbContext.SaveChanges();

                        string pricingMessage;

                        priceSuspendedTransactionUsingReference(dbContext, transactionId, pricingReferenceType, null, userId, out pricingMessage, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION, transactionReferenceNumber);
                    }

                    //var receiptReferenceTransactionItem = dbContext.Inventory_TransactionItem.Single(
                    //    tip =>
                    //        tip.TransactionId == (int)reference.OperationId &&
                    //        tip.GoodId == (int)fuelReportDetail.Good.SharedGoodId);

                    //result.Add(new InventoryOperation(
                    //               inventoryOperationId: reference.OperationId,
                    //               actionNumber: InventoryExtensions.BuildActionNumber(TransactionType.Receipt, fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id, decimal.Parse(transactionCode), fuelReportDetail.FuelReport.VesselInCompany.Code),
                    //               actionDate: DateTime.Now,
                    //               actionType: InventoryActionType.Pricing,
                    //               fuelReportDetailId: fuelReportDetail.Id,
                    //               charterId: null));

                    return transactionId;
                }
                else
                {
                    //throw new InvalidOperation("FR Incremental Correction Edit", "FueReport  Incremental Correction edit is invalid");
                    //var transactionItem = dbContext.Inventory_TransactionItem.Where(ti => ti.TransactionId == reference.OperationId);
                }

                return -1;

                #endregion
            }
        }

        public long PriceFuelReportDetailIncrementalCorrectionDirectPricing(FuelReportDetail fuelReportDetail, int userId)
        {
            using (var dbContext = new DataContainer())
            {
                var result = new List<InventoryOperation>();

                #region Incremental Correction

                var transactionReferenceNumber = fuelReportDetail.GetInventoryTransactionReferenceNumber();

                var reference = getFuelReportDetailIncrementalCorrectionReference(dbContext, fuelReportDetail);

                if (reference != null)
                {
                    //string transactionCode;
                    //string transactionMessage;

                    var transactionId = (int)reference.FirstOrDefault().OperationId;

                    var receiptReferenceTransactionItem = dbContext.Inventory_TransactionItem.Single(
                            ti =>
                                ti.TransactionId == transactionId &&
                                ti.GoodId == (int)fuelReportDetail.Good.SharedGoodId);

                    if (receiptReferenceTransactionItem.Inventory_Transaction.Status == 3 || receiptReferenceTransactionItem.Inventory_Transaction.Status == 4)
                    {

                    }
                    else
                    {
                        var transactionItemPrice = new Inventory_TransactionItemPrice()
                                                   {
                                                       TransactionItemId = receiptReferenceTransactionItem.Id,
                                                       QuantityUnitId = getMeasurementUnitId(dbContext, fuelReportDetail.MeasuringUnit.Abbreviation),
                                                       QuantityAmount = fuelReportDetail.Correction,
                                                       PriceUnitId = getCurrencyId(dbContext, fuelReportDetail.CorrectionPriceCurrency.Abbreviation),
                                                       Fee = fuelReportDetail.CorrectionPrice,
                                                       RegistrationDate = fuelReportDetail.FuelReport.EventDate,
                                                       Description = "Incremental Correction Direct Pricing > " + fuelReportDetail.Good.Code,
                                                       UserCreatorId = userId
                                                   };

                        string pricingMessage;

                        priceTransactionItemManually(dbContext, transactionItemPrice, userId, out pricingMessage, InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION, transactionReferenceNumber);
                    }

                    //result.Add(new InventoryOperation(
                    //               inventoryOperationId: operationReference.OperationId,
                    //               actionNumber: InventoryExtensions.BuildActionNumber(TransactionType.Receipt, fuelReportDetail.FuelReport.VesselInCompany.VesselInInventory.Id, decimal.Parse(transactionCode), fuelReportDetail.FuelReport.VesselInCompany.Code),
                    //               actionDate: DateTime.Now,
                    //               actionType: InventoryActionType.Receipt,
                    //               fuelReportDetailId: fuelReportDetail.Id,
                    //               charterId: null));

                    return transactionId;
                }
                else
                {
                    //throw new InvalidOperation("FR Incremental Correction Edit", "FueReport  Incremental Correction edit is invalid");
                    //var transactionItem = dbContext.Inventory_TransactionItem.Where(ti => ti.TransactionId == reference.OperationId);
                }

                return -1;

                #endregion
            }
        }

        //================================================================================

        private int getCharterInEndIncrementalAdjustmentStoreType(DataContainer dbContext)
        {
            return dbContext.Inventory_StoreType.Single(s => s.Code == (short)InventoryStoreTypesCode.Charter_In_End_Incremental_Adjustment).Id;
        }

        private int getCharterInEndDecrementalAdjustmentStoreType(DataContainer dbContext)
        {
            return dbContext.Inventory_StoreType.Single(s => s.Code == (short)InventoryStoreTypesCode.Charter_In_End_Decremental_Adjustment).Id;
        }

        private int getCharterOutStartDecrementalAdjustmentStoreType(DataContainer dbContext)
        {
            return dbContext.Inventory_StoreType.Single(s => s.Code == (short)InventoryStoreTypesCode.Charter_Out_Start_Decremental_Adjustment).Id;
        }

        private int getCharterOutStartIncrementalAdjustmentStoreType(DataContainer dbContext)
        {
            return dbContext.Inventory_StoreType.Single(s => s.Code == (short)InventoryStoreTypesCode.Charter_Out_Start_Incremental_Adjustment).Id;
        }

        //================================================================================

        private List<InventoryOperation> performIncrementalAdjustmentByPricingReference(
            DataContainer dbContext,
            long companyId,
            long vesselInInventoryId,
            int storeTypesId,
            List<GoodOperationQuantity> goodsIncrementalOperationQuantities,
            string transactionReferenceType,
            string transactionReferenceNumber,
            //string transactionReferencePricingType,
            long pricingReferenceId, DateTime operationDateTime, int userId, string vesselCode)
        {
            var result = new List<InventoryOperation>();

            #region Incremental Correction

            string transactionCode;
            string transactionMessage;

            var operationReference = receipt(
                    dbContext,
                    (int)companyId,
                    (int)vesselInInventoryId,
                    getTimeBucketId(dbContext, operationDateTime),
                    operationDateTime,
                    storeTypesId,
                    (int)pricingReferenceId,
                    null,
                    transactionReferenceType, //InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION,
                    transactionReferenceNumber,
                    userId,
                    out transactionCode,
                    out transactionMessage);

            string transactionItemMessage;

            var transactionItem = new List<Inventory_TransactionItem>();
            transactionItem.AddRange(
                goodsIncrementalOperationQuantities.Select(g =>
                new Inventory_TransactionItem()
                    {
                        GoodId = g.Good.SharedGoodId,
                        CreateDate = DateTime.Now,
                        Description = transactionReferenceType + " Pricing > " + g.Good.Code, //InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION,
                        QuantityAmount = g.QuantityAmount,
                        QuantityUnitId = getMeasurementUnitId(dbContext, g.MeasurementUnit.Abbreviation),
                        TransactionId = (int)operationReference.OperationId,
                        UserCreatorId = userId
                    }));

            var transactionItemIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

            string pricingMessage;

            priceSuspendedTransactionUsingReference(dbContext, (int)operationReference.OperationId, "Incremental Adjustment Pricing by Last Issued Voyage", null, userId, out pricingMessage,
                transactionReferenceType,
                transactionReferenceNumber);

            updateContext(dbContext);

            var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

            result.Add(createInventoryOperationResult(createdTransaction));


            #endregion


            return result;

        }

        //================================================================================

        private List<InventoryOperation> performDecrementalAdjustmentInFIFO(
            DataContainer dbContext,
            long companyId,
            long vesselInInventoryId,
            int storeTypesId,
            List<GoodOperationQuantity> goodsDecrementalOperationQuantities,
            //long sharedGoodId,
            //decimal quantityAmount,
            //string measurementUnitAbbreviation,
            string transactionReferenceType,
            string transactionReferenceNumber,
            //string transactionReferencePricingType,
            DateTime operationDateTime,
            int userId,
            string vesselCode)
        {

            var result = new List<InventoryOperation>();

            #region Decremental Correction

            string transactionCode;
            string transactionMessage;

            var operationReference = issue(
                      dbContext,
                      (int)companyId,
                      (int)vesselInInventoryId,
                      getTimeBucketId(dbContext, operationDateTime),
                      operationDateTime,
                      storeTypesId,
                      null,
                      null,
                      transactionReferenceType, //InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION,
                      transactionReferenceNumber,
                      userId,
                      out transactionCode,
                      out transactionMessage);

            string transactionItemMessage;

            var transactionItem = new List<Inventory_TransactionItem>();
            transactionItem.AddRange(
                goodsDecrementalOperationQuantities.Select(g =>
                    new Inventory_TransactionItem()
                    {
                        GoodId = g.Good.SharedGoodId,
                        CreateDate = DateTime.Now,
                        Description = transactionReferenceType + " > " + g.Good.Code, //InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION,
                        QuantityAmount = g.QuantityAmount,
                        QuantityUnitId = getMeasurementUnitId(dbContext, g.MeasurementUnit.Abbreviation),
                        TransactionId = (int)operationReference.OperationId,
                        UserCreatorId = userId
                    }));

            var registeredTransactionIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

            string pricingMessage;
            //int? notPricedTransactionId;

            var pricingTransactionIds = registeredTransactionIds.Select(id => new Inventory_TransactionItemPricingId() { Id = id, Description = "Decremental Adjustment Pricing in FIFO" });

            priceIssuedItemsInFIFO(dbContext, pricingTransactionIds, userId,
                                  out pricingMessage, transactionReferenceType, //InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION_PRICING,
                                  transactionReferenceNumber);

            updateContext(dbContext);

            var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

            result.Add(createInventoryOperationResult(createdTransaction));

            #endregion

            return result;

        }

        //================================================================================

        public InventoryOperation ManageTrustReceivesIssue(CharterOut charterOutStart, List<GoodTrustReceiveData> goodsTrustReceiveData, int userId)
        {
            var charterId = charterOutStart.Id;

            var companyId = charterOutStart.OwnerId.Value;
            var warehouseId = charterOutStart.VesselInCompany.VesselInInventory.Id;
            var actionDate = charterOutStart.ActionDate;
            var vesselCode = charterOutStart.VesselInCompany.Code;

            return issueTotalTrustReceives(goodsTrustReceiveData, userId, charterId, charterOutStart.GetInventoryTransactionReferenceNumber(), companyId, warehouseId, actionDate, vesselCode);
        }

        public InventoryOperation ManageTrustReceivesIssue(CharterIn charterInEnd, List<GoodTrustReceiveData> goodsTrustReceiveData, int userId)
        {
            var charterId = charterInEnd.Id;

            var companyId = charterInEnd.ChartererId.Value;
            var warehouseId = charterInEnd.VesselInCompany.VesselInInventory.Id;
            var actionDate = charterInEnd.ActionDate;
            var vesselCode = charterInEnd.VesselInCompany.Code;

            return issueTotalTrustReceives(goodsTrustReceiveData, userId, charterId, charterInEnd.GetInventoryTransactionReferenceNumber(), companyId, warehouseId, actionDate, vesselCode);
        }

        private InventoryOperation issueTotalTrustReceives(List<GoodTrustReceiveData> goodsTrustReceiveData, int userId, long charterId, string inventoryTransactionReferenceNumber, long companyId, long warehouseId, DateTime actionDate, string vesselCode)
        {
            if (goodsTrustReceiveData == null || goodsTrustReceiveData.Count == 0) return null;

            using (var dbContext = new DataContainer())
            {
                InventoryOperation result = null;

                #region Trust Receives Issue

                string transactionCode;
                string transactionMessage;

                var operationReference =
                    issue(
                            dbContext,
                            (int)companyId,
                            (int)warehouseId,
                            getTimeBucketId(dbContext, actionDate),
                            actionDate,
                            getIssueTrustReceivesStoreType(dbContext),
                            null,
                            null,
                            InventoryOperationReferenceTypes.ISSUE_TOTAL_RECEIVED_TRUST,
                            inventoryTransactionReferenceNumber,
                            userId,
                            out transactionCode,
                            out transactionMessage);

                string transactionItemMessage;

                var transactionItem = new List<Inventory_TransactionItem>();

                foreach (var goodTotalTrustData in goodsTrustReceiveData)
                {
                    transactionItem.Add(new Inventory_TransactionItem()
                    {
                        GoodId = (int)goodTotalTrustData.Good.SharedGoodId,
                        CreateDate = DateTime.Now,
                        Description = "Issue Total Trust Receive for" + goodTotalTrustData.Good.Code,
                        QuantityAmount = goodTotalTrustData.Quantity,
                        QuantityUnitId = getMeasurementUnitId(dbContext, goodTotalTrustData.GoodUnit.Abbreviation),
                        TransactionId = (int)operationReference.OperationId,
                        UserCreatorId = userId
                    });
                }

                var registeredTransactionIds = addTransactionItems(dbContext, (int)operationReference.OperationId, transactionItem, userId, out transactionItemMessage);

                updateContext(dbContext);

                var createdTransaction = dbContext.Inventory_Transaction.Single(t => t.Id == operationReference.OperationId);

                result = createInventoryOperationResult(createdTransaction);

                #endregion

                //transaction.Commit();

                return result;
            }
        }

        public Inventory_Transaction GetLastIssuedTrustReceivesTransaction(string warehouseCode, DateTime comparingDateTime)
        {
            var dbContext = new DataContainer();

            var issueTrustReceivesStoreType = getIssueTrustReceivesStoreType(dbContext);

            return dbContext.Inventory_Transaction.Where(t => t.Inventory_Warehouse.Code == warehouseCode && t.RegistrationDate <= comparingDateTime && t.StoreTypesId == issueTrustReceivesStoreType).OrderBy(t => t.Code).ThenBy(t => t.RegistrationDate).LastOrDefault();
        }

        //================================================================================

        private class Inventory_TransactionItemPricingId
        {
            public int Id;

            public string Description;
        }

        private struct GoodOperationQuantity
        {
            public Good Good;

            public decimal QuantityAmount;

            public GoodUnit MeasurementUnit;
        }
    }
}
