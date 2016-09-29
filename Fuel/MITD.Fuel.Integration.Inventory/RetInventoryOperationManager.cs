using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MITD.Core;
using MITD.Domain.Model;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Repositories;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Integration.Inventory.Infrastructure;
using NHibernate.Linq;

namespace MITD.Fuel.Integration.Inventory
{
    public class RetInventoryOperationManager
    {
        #region Main Flow
        public List<InventoryOperation> UpdateCountSubmitedReciptFlow<T, L>(
                                                 IUpdateCountSubmitedReciptFactory<T, L> updateCountSubmitedReciptFactory,
                                                 IGoodRepository goodRepository,
                                                 Voyage voyage,
                                                 long userId,
                                                 decimal diffQuantity,
                                                 decimal? oldFee
                                                 ) where T : class
        {



            string inventoryOperationReferenceTypes = updateCountSubmitedReciptFactory.GetOperationReferenceType();
            var refreceNumber = updateCountSubmitedReciptFactory.GetRefreceNumber();
            var good = updateCountSubmitedReciptFactory.Getgood();
            var headerEntityId = updateCountSubmitedReciptFactory.GetEntityId();
            var rob = updateCountSubmitedReciptFactory.GetEntityItemRob();
            var voucherPublisher = updateCountSubmitedReciptFactory.CreateReciptVoucher();
            var inventoryRevertOperationReferenceTypes = updateCountSubmitedReciptFactory.GetRevertOperationReferenceType();
            var deleteVoucher = ServiceLocator.Current.GetInstance<IDeleteVoucher>();
            var checkVoucher = ServiceLocator.Current.GetInstance<ICheckVoucher>();
            var entity = updateCountSubmitedReciptFactory.GetEntity();



            using (var dbContext = new DataContainer())
            {

                var refrenceInventoryOPeration = getReference(dbContext, headerEntityId.ToString(),
                    InventoryOperationType.Receipt, inventoryOperationReferenceTypes);

                var recipt = dbContext.Inventory_Transaction.SingleOrDefault(c => c.Id == refrenceInventoryOPeration.OperationId);
                var reciptItem =
                    recipt.Inventory_TransactionItem.Single(C => C.GoodId == good.SharedGoodId);

                #region Update Count Recipt

                #region  Recipt Is Pricing Step 1
                if (IsReciptPriced(recipt))
                {
                    #region  Fuel Sys  Has Voucher Step 5
                    if (HasVoucher(recipt, good.SharedGoodId, checkVoucher))
                    {
                        #region Send To Financial Step 7
                        if (IsSendToFinancial())
                        {
                            #region Adjustment Is Positive Step 13
                            if (IsPositiveAdjustment(recipt, good.SharedGoodId, rob))
                            {

                            }
                            #endregion
                            #region Adjustment Is Not Positive Step 13
                            else
                            {
                                #region Amount Recipt Greater than Adjustment Step 12
                                if (IsAmountReciptGreaterthanAdjustment(dbContext, recipt, rob, good.SharedGoodId))
                                {

                                    #region Pre Publish Revert Issue Step 11
                                    var resPreStep11 = ReformPricingScan(
                                        dbContext,
                                        recipt.RegistrationDate.Value,
                                        recipt.Id,
                                        userId,
                                        recipt.WarehouseId);
                                    #endregion

                                    #region Publish Revert Issue Step 11
                                    var resStep11 = PublishRevertIssue(dbContext,
                                                      (int)recipt.Inventory_Warehouse.CompanyId,
                                                      (int)recipt.WarehouseId,
                                                       recipt.Id,
                                                       inventoryRevertOperationReferenceTypes,
                                                       refreceNumber,
                                                       (int)userId,
                                                       good.SharedGoodId,
                                                       diffQuantity,
                                                       good.SharedGood.MainUnit.Abbreviation,
                                                       recipt.RegistrationDate.Value
                                                                                     );
                                    #endregion

                                    #region Publish Voucher Step 20
                                    PublishVoucher(

                                         resStep11,
                                         InventoryOperationType.Issue,
                                         entity,
                                         userId,
                                         voyage.VoyageNumber,
                                         ""
                                         );
                                    #endregion

                                    #region Re Pricing Step 22

                                    var resStep22 = RePricing(
                                     resPreStep11.Item1,
                                     resPreStep11.Item2,
                                     dbContext,
                                     userId,
                                     headerEntityId
                                     );

                                    #endregion

                                    #region Publish Voucher Corrective Step 29

                                    PublishVoucherCorrective(dbContext, resStep22.Item2, resStep22.Item1,
                                     good.SharedGoodId, userId);

                                    #endregion
                                }
                                #endregion
                                #region Amount Recipt Is Not Greater than Adjustment Step 12
                                else
                                {

                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                        #region Not Send To Financial Step 7
                        else
                        {

                        }
                        #endregion
                    }
                    #endregion
                    #region Fuel Sys  Has not Voucher Step 5
                    else
                    {

                    }
                    #endregion

                }
                #endregion
                #region  Recipt Is not Pricing Step 1
                else
                {

                }
                #endregion

                #endregion

            }


            return new List<InventoryOperation>();

        }
        #endregion

        #region Sub Flow

        //private Func<bool> Removing(DataContainer container, DateTime recieptDate
        //  , long recieptId, long userid, long warehouseId)
        //{


        //    return () =>
        //    {
        //        var step1 = new ActivityChain<Inventory_Transaction>("PricingScan_Step1", GetRelatedTransaction(container, recieptDate, recieptId, warehouseId));
        //        var memorizedTransaction = step1.OutPutsList;
        //        var step2 = new ActivityChain<Inventory_Transaction>("PricingScan_Step2", SortRelatedTransaction(memorizedTransaction));
        //        memorizedTransaction = step2.OutPutsList;
        //        var step3 = new ActivityChain<object>("PricingScan_Step3", RemovePricing(container, (int)userid, memorizedTransaction));
        //        var step4 = new ActivityChain<object>("PricingScan_Step4", RefreshContext(container));

        //        step1.SetChain(step2);
        //        step2.SetChain(step3);
        //        step3.SetChain(step4);
        //        step4.SetChain(null);

        //        step1.HandleRequest();
        //        return true;
        //    };
        //}

        public System.Tuple<List<Inventory_Transaction>, List<Inventory_Transaction>> RePricing(
              List<Inventory_Transaction> oldList,
            List<Inventory_Transaction> newList,
          DataContainer container,
            long userid,
            long charterId
                 )
        {

            var memorizedTransaction = oldList;

            foreach (var inventoryTransaction in memorizedTransaction)
            {
                #region  Transaction Has Pricing Refrence
                if (HasTransactionPricingRefrence(inventoryTransaction))
                {
                    PriceSuspendedTransUsingRefrence(container, inventoryTransaction,
                        "", (int)userid, charterId.ToString());
                }
                #endregion
                #region Transaction Has not Pricing Refrence
                else
                {
                    PriceIssueItemInFIFO(container, inventoryTransaction, "", (int)userid, charterId.ToString());
                }
                #endregion
            }
            RefreshContext(container);

            return new System.Tuple<List<Inventory_Transaction>, List<Inventory_Transaction>>(newList,
                       oldList);

        }

        public System.Tuple<List<Inventory_Transaction>, List<Inventory_Transaction>> ReformPricingScan(
          DataContainer container,
          DateTime recieptDate,
          long recieptId,
          long userid,
          long warehouseId)
        {
            var memorizedTransaction = GetRelatedTransaction(container, recieptDate, recieptId, warehouseId);
            var sortTransaction = SortRelatedTransaction(memorizedTransaction);
            var cloneTransaction = CloneList(sortTransaction);
            RemovePricing(container, (int)userid, memorizedTransaction);
            RefreshContext(container);
             memorizedTransaction = GetRelatedTransaction(container, recieptDate, recieptId, warehouseId);
             sortTransaction = SortRelatedTransaction(memorizedTransaction);
            return new System.Tuple<List<Inventory_Transaction>, List<Inventory_Transaction>>(cloneTransaction,
                        sortTransaction);

        }




        #endregion

        #region Main Flow Method

        //1
        bool IsReciptPriced(Inventory_Transaction transaction)
        {
            if (transaction == null)
                throw new BusinessRuleException("Inv Operation Ref Is Null");
            else
            {
                return (transaction.Status == 3 || transaction.Status == 4);
            }
        }



        //5
        bool HasVoucher(Inventory_Transaction inventoryTransaction, long goodId,
                            ICheckVoucher checkVoucher)
        {
            var res = inventoryTransaction.Inventory_TransactionItem.Single(c => c.GoodId == goodId);
            var headerNo = inventoryTransaction.GetActionNumber(inventoryTransaction.Inventory_Warehouse.Code);
            return checkVoucher.HasVoucher(headerNo, res.Id);
        }


        //7
        bool IsSendToFinancial()
        {

            return true;

        }
        bool IsSendToFinancial(Voucher voucher)
        {

            return true;

        }



        //11
        List<Inventory_OperationReference> PublishRevertIssue(DataContainer dbContext
            , int companyId, int warehouseId,
              int? pricingReferenceId, string referenceType,
              string referenceNumber, int userId, long goodId, decimal diffValue, string goodUnitAbbreviation, DateTime effectiveDate)
        {

            var timeBucketId = getTimeBucketId(dbContext, effectiveDate);
            string code;
            string message;
            var res = new List<Inventory_OperationReference>();
            var iom = new InventoryOperationManager();
            var resIssue = issue(dbContext,
                companyId,
                warehouseId,
                timeBucketId,
                effectiveDate,
                iom.getAdjustmentIssueType(dbContext),
                pricingReferenceId,
                null,
                referenceType, //InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE,
                referenceNumber,
                userId,
                out code,
                out message);

            var transactionItem = new List<Inventory_TransactionItem>();
            transactionItem.Add(new Inventory_TransactionItem()
            {
                GoodId = (int)goodId,
                CreateDate = DateTime.Now,
                Description = "Charter In Start Adjustment> ",
                QuantityAmount = diffValue,
                QuantityUnitId = iom.getMeasurementUnitId(dbContext, goodUnitAbbreviation),
                TransactionId = (int)resIssue.OperationId,
                UserCreatorId = userId
            });

            string transactionItemMessage;
            var transactionItemIds = iom.addTransactionItems(dbContext, (int)resIssue.OperationId,
                transactionItem, userId, out transactionItemMessage);


            string pricingMessage;


            iom.priceSuspendedTransactionUsingReference(
                 dbContext,
                 (int)resIssue.OperationId,
                 InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_ISSUE
                 , userId,
                 out pricingMessage,
                 InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_ISSUE,
                 referenceNumber);



            res.Add(resIssue);
            return res;

        }

        //19 12
        bool IsAmountReciptGreaterthanAdjustment(DataContainer container, Inventory_Transaction transaction, decimal newValue, long goodId)
        {

            var transactions =
                container.Inventory_Transaction.Where(c => c.RegistrationDate > transaction.RegistrationDate && c.WarehouseId == transaction.WarehouseId)
                    .ToList();
            decimal? issueValue = 0;
            decimal? recieptValue = 0;
            transactions.ForEach(f =>
            {
                var item = f.Inventory_TransactionItem.SingleOrDefault(c => c.GoodId == goodId);
                if (item != null)
                    if (f.Action == 1)
                    {
                        recieptValue += item.QuantityAmount;
                    }
                    else if (f.Action == 2)
                    {
                        issueValue += item.QuantityAmount;
                    }

            });

            return (newValue + recieptValue) - issueValue > 0;


        }

        //15 13
        bool IsPositiveAdjustment(Inventory_Transaction transaction, long goodId, decimal newvalue)
        {

            var transItem = transaction.Inventory_TransactionItem.Single(c => c.GoodId == goodId);
            return transItem.QuantityAmount < newvalue;

        }

        //20
        bool PublishVoucher(

                                List<Inventory_OperationReference> inventoryOperationReference,
                                InventoryOperationType inventoryOperationType,
                                object entity,
                                long userId,
                                string lineCode,
                                string voyageCode)
        {

            var iom = new InventoryOperationManager();
            if (entity is CharterIn)
            {
                var goodRepository = ServiceLocator.Current.GetInstance<IGoodRepository>();
                var transactionId = inventoryOperationReference[0].OperationId;
                var trans = iom.GetTransaction(transactionId, inventoryOperationType);
                var issues = trans.CreateIssueDataForFinanceArticles(((CharterIn)entity).ChartererId.Value, goodRepository);

                var voucher = ServiceLocator.Current.GetInstance<IAddCharterInStartIssueDiffVoucher>();

                voucher.Execute(
                   ((CharterIn)entity),
                    issues,
                    trans.WarehouseId.ToString(),
                    trans.GetActionNumber(trans.Inventory_Warehouse.Code),
                    userId,
                    lineCode,
                    voyageCode);
            }


            return true;

        }


        //29 //22
        bool PublishVoucherCorrective(DataContainer container,
            List<Inventory_Transaction> oldList,
            List<Inventory_Transaction> newList,
            long shareGoodId,
            long userId)
        {

            oldList.ForEach(c =>
            {
                var headerNo = c.GetActionNumber(c.Inventory_Warehouse.Code);
                var deleteVoucher = ServiceLocator.Current.GetInstance<IDeleteVoucher>();
                var newTrans = newList.Single(d => d.Id == c.Id);
                var vouchers = container.Vouchers.Where(x => x.ReferenceNo == headerNo).ToList();

                if (!CheckSameVoucher(c, newTrans))
                {
                    if (c.Status == 4)
                    {
                        vouchers.ForEach(d =>
                        {
                            if (!IsSendToFinancial(d))
                            {

                                deleteVoucher.Done(0, headerNo);
                            }

                        });

                    }

                    if (newTrans.Status == 3)
                    {

                        vouchers.ForEach(v =>
                        {
                            if (IsSendToFinancial(v))
                            {
                                Utitlity.ExecuteAutomaticVoucher(
                                    c, newTrans,
                                    userId,
                                    shareGoodId,
                                    true,
                                    null

                                    );
                            }
                            else
                            {

                                Utitlity.ExecuteAutomaticVoucher(
                                    c, newTrans,
                                    userId,
                                    shareGoodId,
                                    false,
                                    null

                                    );
                            }
                        });
                    }
                }

            });

            return true;
        }


        #endregion

        #region Sub Flow Method

        List<Inventory_Transaction> GetFifoIssues(DataContainer dataContainer, DateTime recieptDate, long warehouseId)
        {
            var res = dataContainer.Inventory_Transaction.Where(c => c.Action == 2
                                                            && c.PricingReferenceId == null
                                                            && c.WarehouseId == warehouseId
                                                            && c.RegistrationDate > recieptDate
                                                            && !c.Inventory_StoreType.IsAdjustment)
                                                            .Include("Inventory_TransactionItem.Inventory_TransactionItemPrice")
                    .ToList();

            return res;
        }

        List<Inventory_Transaction> GetRecieptWithFifoIssueRefrence(DataContainer dataContainer, DateTime recieptDate,
            List<int> pricingRefrenceIds, long warehouseId)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == 1
                                                                && pricingRefrenceIds.Contains(c.PricingReferenceId.Value)
                                                                && c.WarehouseId == warehouseId
                                                                && c.RegistrationDate > recieptDate
                                                                && !c.Inventory_StoreType.IsAdjustment)
                                                                  .Include("Inventory_TransactionItem.Inventory_TransactionItemPrice")
                    .ToList();
        }
        List<Inventory_Transaction> GetRecieptWithCurrentRecieptRefrence(DataContainer dataContainer, DateTime recieptDate,
            long currentRecieptId, long warehouseId)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == 2
                                                                && c.PricingReferenceId == currentRecieptId
                                                                && c.WarehouseId == warehouseId
                                                                && c.RegistrationDate > recieptDate
                                                                 && !c.Inventory_StoreType.IsAdjustment
                                                                )
                                                                  .Include("Inventory_TransactionItem.Inventory_TransactionItemPrice")
                    .ToList();
        }
        List<Inventory_Transaction> GetIssuesWithCurrentRecieptRefrence(DataContainer dataContainer, DateTime recieptDate,
            long currentRecieptId, long warehouseId)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == 1
                                                                && c.PricingReferenceId == currentRecieptId
                                                                 && c.WarehouseId == warehouseId
                                                                && c.RegistrationDate > recieptDate
                                                                 && !c.Inventory_StoreType.IsAdjustment)
                                                                   .Include("Inventory_TransactionItem.Inventory_TransactionItemPrice")
                    .ToList();
        }

        List<Inventory_Transaction> CloneList(List<Inventory_Transaction> inventoryTransactions)
        {
            var res = inventoryTransactions.Clone();
            return res;
        }

        List<Inventory_Transaction> GetRelatedTransaction(DataContainer container, DateTime recieptDate, long recieptId, long warehouseId)
        {

            var res = new List<Inventory_Transaction>();
            res.AddRange(GetRecieptWithCurrentRecieptRefrence(container, recieptDate, recieptId, warehouseId));
            res.AddRange(GetIssuesWithCurrentRecieptRefrence(container, recieptDate, recieptId, warehouseId));

            var res1 = GetFifoIssues(container, recieptDate, warehouseId);
            res.AddRange(res1);
            res.AddRange(GetRecieptWithFifoIssueRefrence(container, recieptDate, res1.Select(c => c.Id).ToList(), warehouseId));


            return res;

        }

        List<Inventory_Transaction> SortRelatedTransaction(IEnumerable<Inventory_Transaction> inventoryTransactions)
        {

            return
                inventoryTransactions.OrderBy(c => c.RegistrationDate).ToList();

        }

        bool RemovePricing(DataContainer container, int userid, List<Inventory_Transaction> inventoryTransactions)
        {

            var message = "";
            var lst = new List<Inventory_Transaction>();
            var iom = new InventoryOperationManager();
            inventoryTransactions.ForEach(lst.Add);
            lst.OrderByDescending(c => c.RegistrationDate).ToList().ForEach(c =>
            {
                foreach (var inventoryTransactionItem in c.Inventory_TransactionItem)
                {
                    iom.removeTransactionItemPrices(container, inventoryTransactionItem.Id, userid, out message);
                }

            });

            return true;

        }

        void RefreshContext(DataContainer container)
        {
            refreshContext(container);

        }

        bool HasTransactionPricingRefrence(Inventory_Transaction inventoryTransaction)
        {

            if (inventoryTransaction != null)
                return inventoryTransaction.PricingReferenceId != null;
            else
                return false;

        }

        void PriceSuspendedTransUsingRefrence(DataContainer container, Inventory_Transaction inventoryTransaction, string desc, int userId, string charterId)
        {

            var iom = new InventoryOperationManager();
            int transactionId = inventoryTransaction.Id;
            string message = "";
            iom.priceSuspendedTransactionUsingReference(container, transactionId, desc, userId, out message,
                InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT, charterId.ToString());



        }


        void PriceIssueItemInFIFO(DataContainer container, Inventory_Transaction inventoryTransaction, string desc, int userId, string charterId)
        {

            string message = "";
            foreach (var inventoryTransactionItem in inventoryTransaction.Inventory_TransactionItem)
            {
                try
                {
                    var iom = new InventoryOperationManager();
                    iom.priceIssuedItemInFIFO(container, inventoryTransactionItem.Id, desc, userId, out message,
                                          inventoryTransaction.ReferenceType, inventoryTransaction.ReferenceNo);
                }
                catch
                {
                }
            }


        }


        #endregion


        #region Utill Method

        bool CheckSameVoucher(Inventory_Transaction old, Inventory_Transaction nw)
        {
            var flag = true;
            var transItemOld = old.Inventory_TransactionItem;
            var transItemNew = nw.Inventory_TransactionItem;

            foreach (var inventoryTransactionItemold in transItemOld)
            {
                var res = transItemNew.Single(
                     c =>
                         c.GoodId == inventoryTransactionItemold.GoodId &&
                         c.QuantityAmount == inventoryTransactionItemold.QuantityAmount);
                if (res == null)
                {
                    flag = false;
                }
                else
                {
                    foreach (var transactionItemPrice in inventoryTransactionItemold.Inventory_TransactionItemPrice)
                    {
                        if (
                            res.Inventory_TransactionItemPrice.Count(c => c.QuantityAmount == transactionItemPrice.QuantityAmount &&
                                                                          c.Fee == transactionItemPrice.Fee) ==
                            inventoryTransactionItemold.Inventory_TransactionItemPrice.Count)
                            flag = true;
                        else
                        {
                            flag = false;
                        }
                    }
                }
            }

            return flag;
        }
        private void refreshContext(DataContainer context)
        {
            context.ChangeTracker.DetectChanges();

            var objContext = ((IObjectContextAdapter)context).ObjectContext;
            objContext.Refresh(RefreshMode.ClientWins, context.Inventory_Transaction);
            objContext.Refresh(RefreshMode.ClientWins, context.Inventory_TransactionItem);
            objContext.Refresh(RefreshMode.ClientWins, context.Inventory_TransactionItemPrice);
        }

        Inventory_OperationReference getReference(DataContainer dbContext, string transactionReferenceNumber, InventoryOperationType inventoryOperationType, string inventoryOperationReferenceTypes)
        {

            var res = dbContext.Inventory_OperationReference.Where(c =>
                        c.ReferenceNumber == transactionReferenceNumber &&
                        c.OperationType == (int)inventoryOperationType &&
                        c.ReferenceType == inventoryOperationReferenceTypes).
                        OrderBy(c => c.RegistrationDate).ToList().LastOrDefault();

            return res;

        }

        private int getTimeBucketId(DataContainer dbContext, DateTime dateTime)
        {
            var iom = new InventoryOperationManager();
            return iom.getTimeBucketId(dbContext, dateTime);

        }

        private Inventory_OperationReference issue(DataContainer dbContext, int companyId, int warehouseId, int timeBucketId, DateTime operationDateTime, int storeTypesId, int? pricingReferenceId, int? adjustmentForTransactionId, string referenceType, string referenceNumber, int userId, out string code, out string message)
        {
            var iom = new InventoryOperationManager();
            return iom.issue(dbContext, companyId, warehouseId,
                  timeBucketId, operationDateTime,
                  storeTypesId, pricingReferenceId, adjustmentForTransactionId, referenceType, referenceNumber, userId,
                 out code, out message);
        }

        private Inventory_OperationReference receipt(DataContainer dbContext, int companyId, int warehouseId, int timeBucketId, DateTime operationDateTime, int storeTypesId, int? pricingReferenceId, int? adjustmentForTransactionId, string referenceType, string referenceNumber, int userId, out string code, out string message)
        {
            var iom = new InventoryOperationManager();
            return iom.receipt(dbContext, companyId, warehouseId,
             timeBucketId, operationDateTime,
             storeTypesId, pricingReferenceId, adjustmentForTransactionId, referenceType, referenceNumber, userId,
            out  code, out  message)
            ;
        }

        #endregion



    }
}
