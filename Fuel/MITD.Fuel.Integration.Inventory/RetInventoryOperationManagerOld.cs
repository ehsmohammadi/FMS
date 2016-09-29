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
    public partial class InventoryOperationManager
    {

        #region Flow
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
            var inventoryRevertOperationReferenceTypes =
                updateCountSubmitedReciptFactory.GetRevertOperationReferenceType();

            UpdateCountableSubmitedReciptFlow(good,
                                              voyage,
                                              userId,
                                              goodRepository,
                                              diffQuantity,
                                              inventoryOperationReferenceTypes,
                                              inventoryRevertOperationReferenceTypes,
                                              voucherPublisher,
                                              refreceNumber,
                                              rob,
                                              headerEntityId,
                                              updateCountSubmitedReciptFactory.GetEntity());



            return new List<InventoryOperation>();

        }

        private List<InventoryOperation> UpdateCountableSubmitedReciptFlow(
                                                                            Good good,
                                                                            Voyage voyage,
                                                                            long userId,
                                                                            IGoodRepository goodRepository,
                                                                            decimal diffQuantity,
                                                                            string inventoryOperationReferenceTypes,
                                                                            string inventoryRevertOperationReferenceTypes,
                                                                            IAutomaticVoucher voucherPublisher,
                                                                            string refreceNumber,
                                                                            decimal rob,
                                                                            long headerEntityId,
                                                                            object entity
                                                                   )
        {

            var deleteVoucher =
            ServiceLocator.Current.GetInstance<IDeleteVoucher>();
            var checkVoucher =
              ServiceLocator.Current.GetInstance<ICheckVoucher>();

            using (var dbContext = new DataContainer())
            {
                //using (var transaction = dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var refrenceInventoryOPeration = getReference(dbContext, headerEntityId.ToString(),
                        InventoryOperationType.Receipt, inventoryOperationReferenceTypes);

                    var recipt = dbContext.Inventory_Transaction.SingleOrDefault(c => c.Id == refrenceInventoryOPeration.OperationId);
                    var reciptItem =
                        recipt.Inventory_TransactionItem.Single(C => C.GoodId == good.SharedGoodId);

                    #region Setup

                    var step1 = new ConditionChain<object>("Step1", IsReciptPriced(recipt));
                    var step2 = new ActivityChain<object>("Step2", CheckCountReciptGreaterThanCountIssue(dbContext,
                                                                                                     good.SharedGoodId,
                                                                                                     recipt.WarehouseId,
                                                                                                     refrenceInventoryOPeration.OperationId,
                                                                                                     rob));
                    var step3 = new ActivityChain<Inventory_Transaction>("Step3", UpdateTransactionItemQuantity(dbContext, recipt, rob, good.SharedGoodId, (int)userId));
                    var step4 = new ActivityChain<object>("Step4", RenewedPricingCurrentReciept(dbContext,
                                                                   step3.OutPutsList,
                                                                   "",
                                                                   (int)userId,
                                                                    headerEntityId.ToString()));
                    var step41 = new ActivityChain<object>("Step41", PublishVoucherCurrentReciept(step3.OutPutsList, voucherPublisher, entity, voyage, userId));
                    var step42 = new ActivityChain<object>("Step42", PricingScanPublishVoucher(dbContext,
                                                                                recipt.RegistrationDate.Value,
                                                                                recipt.Id,
                                                                                userId,
                                                                                headerEntityId,
                                                                                reciptItem.Id,
                                                                                recipt.WarehouseId,
                                                                                recipt.PricingReferenceId));

                    var step5 = new ConditionChain<object>("Step5", HasVoucher(recipt, good.SharedGoodId, checkVoucher));
                    var step6 = new ConditionChain<object>("Step6", IsQuantityIssueFromCurrentRecieptGreaterthanZero(dbContext, reciptItem));
                    var step7 = new ConditionChain<object>("Step7", IsSendToFinancial());
                    var step8 = new ActivityChain<object>("Step8", DeleteVoucher(reciptItem.Id, recipt, deleteVoucher));
                    var step10 = new ExceptionChain(new BusinessRuleException("Error Step 10"));
                    var step11_0 = new ActivityChain<Inventory_Transaction>("Step11_0", PreReformPricingScanAndVoucherPublish(
                                                                                dbContext,
                                                                                recipt.RegistrationDate.Value,
                                                                                recipt.Id,
                                                                                userId,
                                                                                recipt.WarehouseId));
                    var step11 = new ActivityChain<Inventory_OperationReference>("step11", PublishRevertIssue(dbContext,
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
                                                                                         ));
                    var step12 = new ConditionChain<object>("step12", IsAmountReciptGreaterthanAdjustment(dbContext, recipt, rob, good.SharedGoodId));
                    var step13 = new ConditionChain<object>("Step13", IsPositiveAdjustment(recipt, good.SharedGoodId, rob));
                    var step14 = new ConditionChain<object>("Step14", IsQuantityAmountCurrentGreaterthanZero(dbContext, reciptItem));
                    var step15 = new ConditionChain<object>("Step15", IsPositiveAdjustment(recipt, good.SharedGoodId, rob));
                    var step16 = new ExceptionChain(new BusinessRuleException("Error Step 16"));
                    var step171 = new ActivityChain<Inventory_Transaction>("step171", UpdateTransactionItemQuantity(dbContext, recipt, rob, good.SharedGoodId, (int)userId));
                    var step172 = new ActivityChain<object>("step172", RenewedPricingCurrentReciept(dbContext,
                                                                       new List<Inventory_Transaction>() { recipt }, inventoryOperationReferenceTypes,
                                                                       (int)userId, headerEntityId.ToString()));
                    var step173 = new ActivityChain<object>("step173", PublishVoucherCurrentReciept(new List<Inventory_Transaction>() { recipt },
                                                                                                    voucherPublisher,
                                                                                                     entity,
                                                                                                     voyage,
                                                                                                     (int)userId));
                    var step18 = new ConditionChain<object>("step18", HasIssueAfterIssueCurrentReciept(dbContext, recipt.WarehouseId, good.SharedGoodId, recipt.RegistrationDate.Value));

                    var step19 = new ConditionChain<object>("step19", IsAmountReciptGreaterthanAdjustment(dbContext,
                                                                                                          recipt,
                                                                                                          rob,
                                                                                                          good.SharedGoodId));
                    var step191 = new ActivityChain<object>("step191", Removing(dbContext,
                                                                                    recipt.RegistrationDate.Value,
                                                                                    recipt.Id,
                                                                                    userId,
                                                                                     recipt.WarehouseId));

                    var step20 = new ActivityChain<object>("step20", PublishVoucher(

                                                                                    step11.OutPutsList,
                                                                                    InventoryOperationType.Issue,
                                                                                    entity,
                                                                                    userId,
                                                                                    voyage.VoyageNumber,
                                                                                    ""
                                                                                    ));

                    var step21 = new ActivityChain<object>("Step21", PublishReciptPricing(
                                                                                dbContext,
                                                                                (int)recipt.Inventory_Warehouse.CompanyId,
                                                                                (int)recipt.WarehouseId,
                                                                                refreceNumber,
                                                                                recipt.Id,
                                                                                (int)userId,
                                                                                "",
                                                                                "",
                                                                                good.SharedGoodId,
                                                                                good.Code,
                                                                                rob,
                                                                                good.SharedGood.MainUnit.Abbreviation
                                                                                ));

                    var step22 = new ActivityChain<Inventory_Transaction>("Step22", ReformPricingScanAndVoucherPublish(
                                                                                 step11_0.OutPutsList,
                                                                                 step11_0.OutPutsList1,
                                                                                 dbContext,
                                                                                  userId,
                                                                                 headerEntityId
                                                                                ));
                    var step23 = new ConditionChain<object>("step23", HasIssueAfterIssueCurrentReciept(dbContext, recipt.WarehouseId, good.SharedGoodId, recipt.RegistrationDate.Value));
                    var step24 = new ConditionChain<object>("Step24", IsQuantityAmountCurrentGreaterthanZero(dbContext,
                                                                                                             reciptItem));
                    var step25 = new ActivityChain<object>("Step25", PricingScanPublishVoucher(dbContext,
                                                                                recipt.RegistrationDate.Value,
                                                                                recipt.Id,
                                                                                userId,
                                                                                headerEntityId,
                                                                                reciptItem.Id,
                                                                                 recipt.WarehouseId,
                                                                                 recipt.PricingReferenceId));

                    var step26 = new ActivityChain<Inventory_Transaction>("Step26", UpdateTransactionItemQuantity(dbContext, recipt, rob, good.SharedGoodId, (int)userId));
                    //    var step27 = new ActivityChain<object>("Step27",IsSendToFinancial());
                    var step28 = new ActivityChain<object>("Step28", PublishVoucherSendFinancial());
                    var step29 = new ActivityChain<object>("step29", PublishVoucherCorrective(dbContext,step22.OutPutsList1,step22.OutPutsList,good.SharedGoodId,userId));

                    var step43 = new ActivityChain<object>("Step0043", RenewedPricingCurrentReciept(dbContext,
                                                                step26.OutPutsList,
                                                                "",
                                                                (int)userId,
                                                                 headerEntityId.ToString()));
                    var step44 = new ActivityChain<object>("Step0044", PublishVoucherCurrentReciept(step26.OutPutsList, voucherPublisher, entity, voyage, userId));

                    #endregion

                    #region Sequance

                    step1.SetChain(step5, step2);
                    step2.SetChain(step3);
                    step3.SetChain(step4);
                    step4.SetChain(step41);
                    step41.SetChain(step42);
                    step42.SetChain(null);
                    step43.SetChain(step44);
                    step44.SetChain(step42);
                    step5.SetChain(step7, step6);
                    step6.SetChain(step15, step3);
                    step7.SetChain(step13, step8);
                    step8.SetChain(step6);
                    step11_0.SetChain(step11);
                    step11.SetChain(step20);
                   
                   // step12.SetChain(step11, step16);
                    step12.SetChain(step11_0, step16);
                    step13.SetChain(step21, step12);
                    step14.SetChain(step171, step18);
                    step15.SetChain(step14, step19);
                    step171.SetChain(step172);
                    step172.SetChain(step173);
                    step173.SetChain(null);
                    step18.SetChain(step25, null);
                    //   step19.SetChain(step26, step10);
                    step19.SetChain(step191, step10);
                    step191.SetChain(step26);
                    step20.SetChain(step22);
                    step21.SetChain(step24);
                    step22.SetChain(step29);
                    step23.SetChain(step22, null);
                    step24.SetChain(step28, step23);
                    step25.SetChain(null);
                    step26.SetChain(step43);

                    // step27.SetChain(step29);
                    step28.SetChain(null);
                    step29.SetChain(null);
                    #endregion


                    step1.HandleRequest();
                    //   transaction.Commit();
                }
            }


            return new List<InventoryOperation>();
        }




        public List<InventoryOperation> UpdatePriceSubmitedReciptFlow<T, L>(
            IUpdatePriceSubmitedReciptFactory<T, L> updateCountSubmitedReciptFactory,
            IGoodRepository goodRepository,
            Voyage voyage,
            long userId,
            decimal newPrice
            ) where T : class
        {
            string inventoryOperationReferenceTypes = updateCountSubmitedReciptFactory.GetOperationReferenceType();
            var refreceNumber = updateCountSubmitedReciptFactory.GetRefreceNumber();
            var good = updateCountSubmitedReciptFactory.Getgood();
            var headerEntityId = updateCountSubmitedReciptFactory.GetEntityId();
            var newPrice1 = newPrice;
            var voucherPublisher = updateCountSubmitedReciptFactory.CreateReciptVoucher();
            var inventoryRevertOperationReferenceTypes =
                updateCountSubmitedReciptFactory.GetRevertOperationReferenceType();

            UpdatePriceSubmitedReciptFlow(good,
                                              voyage,
                                              userId,
                                              goodRepository,
                                              inventoryOperationReferenceTypes,
                                              inventoryRevertOperationReferenceTypes,
                                              voucherPublisher,
                                              refreceNumber,
                                              headerEntityId,
                                              updateCountSubmitedReciptFactory.GetEntity(),
                                              newPrice1);

            return new List<InventoryOperation>();
        }

        private void UpdatePriceSubmitedReciptFlow(Good good,
                                                   Voyage voyage,
                                                   long userId,
                                                   IGoodRepository goodRepository,
                                                   string inventoryOperationReferenceTypes,
                                                   string inventoryRevertOperationReferenceTypes,
                                                   IAutomaticVoucher voucherPublisher,
                                                   string refreceNumber,
                                                   long headerEntityId,
                                                   object entity,
                                                   decimal newPrice
                                            )
        {

            var deleteVoucher =
            ServiceLocator.Current.GetInstance<IDeleteVoucher>();
            var checkVoucher =
                ServiceLocator.Current.GetInstance<ICheckVoucher>();


            using (var dbContext = new DataContainer())
            {
                using (var transaction = dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var refrenceInventoryOPeration = getReference(dbContext, headerEntityId.ToString(),
                        InventoryOperationType.Receipt, inventoryOperationReferenceTypes);

                    var recipt =
                        dbContext.Inventory_Transaction.SingleOrDefault(
                            c => c.Id == refrenceInventoryOPeration.OperationId);
                    var reciptItem =
                        recipt.Inventory_TransactionItem.Single(C => C.GoodId == good.SharedGoodId);

                    #region Setup
                    var step1 = new ConditionChain<object>("Step1", HasVoucher(recipt, good.SharedGoodId, checkVoucher));
                    var step2 = new ActivityChain<Inventory_Transaction>("Step2",
                       UpdateTransactionItemPrice(dbContext, recipt, newPrice, good.SharedGoodId));
                    var step3 = new ConditionChain<object>("step3", HasIssueRegistered(dbContext, reciptItem));
                    var step4 = new ConditionChain<object>("Step4", IsSendToFinancial());
                    var step5 = new ConditionChain<object>("step5", HasIssueRegistered(dbContext, reciptItem));
                    var step6 = new ActivityChain<Inventory_Transaction>("Step6",
                      UpdateTransactionItemPrice(dbContext, recipt, newPrice, good.SharedGoodId));
                    var step7 = new ActivityChain<object>("Step7", PricingScanPublishVoucher(dbContext,
                        recipt.RegistrationDate.Value,
                        recipt.Id,
                        userId,
                        headerEntityId,
                        reciptItem.Id,
                        recipt.WarehouseId,
                        recipt.PricingReferenceId));
                    var step8 = new ActivityChain<object>("Step8", () => true);
                    var step9 = new ConditionChain<object>("step9", HasIssueRegistered(dbContext, reciptItem));
                    var step10 = new ActivityChain<Inventory_Transaction>("Step10",
                      UpdateTransactionItemPrice(dbContext, recipt, newPrice, good.SharedGoodId));
                    var step11 = new ActivityChain<Inventory_Transaction>("Step11",
                     UpdateTransactionItemPrice(dbContext, recipt, newPrice, good.SharedGoodId));
                    var step12 = new ActivityChain<object>("Step12", () => true);
                    var step131 = new ActivityChain<object>("Step131",
                       DeleteVoucher(reciptItem.Id, recipt, deleteVoucher));
                    var step132 = new ActivityChain<object>("Step132", PublishVoucherSendFinancial());
                    var step14 = new ActivityChain<object>("Step14", () => true);
                    var step15 = new ActivityChain<Inventory_Transaction>("Step15",
                     UpdateTransactionItemPrice(dbContext, recipt, newPrice, good.SharedGoodId));
                    var step16 = new ActivityChain<object>("step16",IsSendToFinancial());
                    var step17 = new ActivityChain<object>("Step17", RenewedPricingCurrentReciept(dbContext,
                       null,
                       "",
                       (int)userId,
                       headerEntityId.ToString()));


                    #endregion

                    #region Sequance

                    step1.SetChain(step4, step2);
                    step2.SetChain(step3);
                    step3.SetChain(step7, null);
                    step4.SetChain(step9, step5);
                    step5.SetChain(step6, step10);
                    step6.SetChain(step7);
                    step7.SetChain(step14);
                    step8.SetChain(step11);
                    step9.SetChain(step12, step8);
                    step10.SetChain(step131);
                    step11.SetChain(step16);
                    step12.SetChain(step15);
                    step131.SetChain(step132);
                    step132.SetChain(null);
                    step14.SetChain(null);
                    step15.SetChain(step17);
                    step16.SetChain(null);
                    step17.SetChain(step16);

                    #endregion


                    step1.HandleRequest();
                    transaction.Commit();
                }

            }

        }

        public void UpdateCountSubmitedIssueFlow()
        {
            #region Setup

            //var step1 = new ConditionChain("Step1", IsCharterOutStartOrCharterInEnd());
            //var step2 = new ActivityChain("Step2", RegisterRevertReciptCountPrevIssueForChInEndChOtStrt());
            //var step3 = new ActivityChain("Step3", PublishRevertReciptVoucher());
            //var step4 = new ConditionChain("Step4", IsFinalVoucherIssue());
            //var step5 = new ConditionChain("Step5", IsNewValueGreaterThanPrevValue());
            //var step6 = new ActivityChain("Step6", RegisterRevertReciptDiffValueLastVoyage());
            //var step7 = new ActivityChain("Step7", PublishRevertReciptVoucher());
            //var step8 = new ActivityChain("Step8", RegisterIssueChInEdOrChOtSt());
            //var step9 = new ActivityChain("step9", PublishIssueVoucher());
            //var step10 = new ConditionChain("Step10", IsReceivedSalesInvoice());
            //var step11 = new ActivityChain("step11", PublishDebtorSalesInvoiceForDiff());
            //var step12 = new ConditionChain("Step12", IsFinalVoucherIssue());
            //var step13 = new ConditionChain("Step13", IsNewValueGreaterThanPrevValue());
            //var step14 = new ConditionChain("step14", IsAmountIssueGreaterThanQuantityNewIssue());
            //var step15 = new ActivityChain("step15", RegisterIssueDampingForPrevIssue());
            //var step16 = new ActivityChain("step16", PublishIssueDampingVoucher());
            //var step17 = new ActivityChain("step17", RenewedPricingIssueReciptSavePrevValue());
            //var step18 = new ActivityChain("step18", PublishVoucherCorrective());
            //var step19 = new ConditionChain("Step19", IsNewValueGreaterThanPrevValue());
            //var step20 = new ConditionChain("step20", IsAmountIssueGreaterThanQuantityNewIssue());
            //var step21 = new ActivityChain("step21", UpdateIssueAndVoucher());
            //var step22 = new ActivityChain("step22", RenewedPricingIssueOrPricingIssue());
            //var step23 = new ActivityChain("step23", RegisterReciptDampingForPricedIssue());
            //var step24 = new ActivityChain("step24", RegisterVoucherReciptDamping());
            //var step25 = new ExceptionChain(new BusinessRuleException("Error Step 25 "));
            //var step27 = new ActivityChain("step27", RegisterIssueLastVoyageForDiff());
            //var step28 = new ActivityChain("step28", RegisterVoucherIssueDampingVoyageConsumption());
            //var step30 = new ActivityChain("step30", PublishIssueVoucher());
            //var step31 = new ConditionChain("step31", IsNewValueGreaterThanPrevValue());
            //var step32 = new ActivityChain("step32", RegisterRetriveReciptForLastVoyage());
            //var step33 = new ActivityChain("step33", PublishRevertReciptVoucher());
            //var step34 = new ActivityChain("step34", UpdateIssueAndVoucher());
            //var step35 = new ActivityChain("step35", RegisterIssueLastVoyageForDiff());
            //var step36 = new ActivityChain("step36", RegisterVoucherIssue());
            //var step37 = new ConditionChain("Step37", IsReceivedSalesInvoice());
            //var step38 = new ActivityChain("step38", PublishCreditorSalesInvoiceForDiff());
            //var step39 = new ActivityChain("step39", UpdateSalesInvoiceForNewValue());

            #endregion

            #region Sequance

            //step1.SetChain(step2, step12);
            //step2.SetChain(step3);
            //step3.SetChain(step4);
            //step4.SetChain(step5, step31);
            //step5.SetChain(step6, step27);
            //step6.SetChain(step7);
            //step7.SetChain(step8);
            //step8.SetChain(step9);
            //step9.SetChain(step10);
            //step10.SetChain(step11, step39);
            //step11.SetChain(null);
            //step12.SetChain(step13, step19);
            //step13.SetChain(step14, step23);
            //step14.SetChain(step15, step25);
            //step15.SetChain(step16);
            //step16.SetChain(step17);
            //step17.SetChain(step18);
            //step18.SetChain(null);
            //step19.SetChain(step20, step21);
            //step20.SetChain(step21, step25);
            //step21.SetChain(step22);
            //step22.SetChain(null);
            //step23.SetChain(step24);
            //step24.SetChain(step17);
            //step27.SetChain(step28);
            //step28.SetChain(step8);
            //step30.SetChain(step37);
            //step31.SetChain(step32, step35);
            //step32.SetChain(step33);
            //step33.SetChain(step34);
            //step34.SetChain(step10);
            //step35.SetChain(step36);
            //step36.SetChain(step37);
            //step37.SetChain(step38, step39);
            //step38.SetChain(null);
            //step39.SetChain(null);
            #endregion

            //  step1.HandleRequest();
        }


        public Func<System.Tuple<bool, List<Inventory_Transaction>, List<Inventory_Transaction>>>
          PreReformPricingScanAndVoucherPublish(
          DataContainer container,
          DateTime recieptDate,
          long recieptId,
          long userid,
          long warehouseId)
        {
            return () =>
            {

                #region setup

                var step1 = new ActivityChain<Inventory_Transaction>("PricingScan_Step1",
                    GetRelatedTransaction(container, recieptDate, recieptId, warehouseId));
                var memorizedTransaction = step1.OutPutsList;
                var step2 = new ActivityChain<Inventory_Transaction>("PricingScan_Step2",
                    SortRelatedTransaction(memorizedTransaction));
                memorizedTransaction = step2.OutPutsList;

               var step21 = new ActivityChain<Inventory_Transaction>("PricingScan_Step21", CloneList(step2.OutPutsList));

                var step3 = new ActivityChain<object>("PricingScan_Step3",
                    RemovePricing(container, (int)userid, memorizedTransaction));
                var step4 = new ActivityChain<object>("PricingScan_Step4", RefreshContext(container));
           


                #endregion

                #region Sequence

                step1.SetChain(step2);
                step2.SetChain(step21);
                step21.SetChain(step3);
                step3.SetChain(step4);
                step4.SetChain(null);
               
                #endregion

                step1.HandleRequest();

                return new System.Tuple<bool, List<Inventory_Transaction>, List<Inventory_Transaction>>(true, step21.OutPutsList,
                        step2.OutPutsList);
            };
        }



        //public Func<System.Tuple<bool, List<Inventory_Transaction>, List<Inventory_Transaction>>>
        //    ReformPricingScanAndVoucherPublish(
        //    DataContainer container,
        //    DateTime recieptDate,
        //    long recieptId,
        //    long userid,
        //    long charterId,
        //    long transItemId,
        //    long warehouseId)
        public Func<System.Tuple<bool, List<Inventory_Transaction>, List<Inventory_Transaction>>>
          ReformPricingScanAndVoucherPublish(
              List<Inventory_Transaction> oldList,
            List<Inventory_Transaction> newList,
          DataContainer container,
            long userid,
            long charterId
                 )
       {
            return () =>
            {

                #region setup

             //   var step1 = new ActivityChain<Inventory_Transaction>("PricingScan_Step1",
             //       GetRelatedTransaction(container, recieptDate, recieptId, warehouseId));
             //   var memorizedTransaction = step1.OutPutsList;
             //   var step2 = new ActivityChain<Inventory_Transaction>("PricingScan_Step2",
             //       SortRelatedTransaction(memorizedTransaction));
             //   memorizedTransaction = step2.OutPutsList;

             ////   var res = memorizedTransaction.Clone();

                var memorizedTransaction = oldList;
                //var step21 = new ActivityChain<Inventory_Transaction>("PricingScan_Step21", CloneList(step2.OutPutsList));

                //var step3 = new ActivityChain<object>("PricingScan_Step3",
                //    RemovePricing(container, (int) userid, memorizedTransaction));
                //var step4 = new ActivityChain<object>("PricingScan_Step4", RefreshContext(container));
                var count = 0;
              //  var step5 = new ConditionChain<object>("PricingScan_Step5", IsTransactionRemaining(memorizedTransaction, count));

               // var step51 = new ActivityChain<int>("PricingScan_Step51", IsTransactionRemaining(memorizedTransaction));
                var step6 = new ConditionChain<Inventory_Transaction>("PricingScan_Step6", GetNext(memorizedTransaction, count));
                var step7 = new ConditionChain<object>("PricingScan_Step7",
                    HasTransactionPricingRefrence(step6.OutPutsList));
                var step8 = new ActivityChain<object>("PricingScan_Step8",
                    PriceSuspendedTransUsingRefrence(container, step6.OutPutsList,
                        "", (int) userid, charterId.ToString()));
                var step9 = new ActivityChain<object>("PricingScan_Step9",
                    PriceIssueItemInFIFO(container, step6.OutPutsList, "", (int)userid, charterId.ToString()));
                var step10 = new ActivityChain<object>("PricingScan_Step10", RefreshContext(container));

                #endregion

                #region Sequence

                //step1.SetChain(step2);
                //step2.SetChain(step21);
                //step21.SetChain(step3);
                //step3.SetChain(step4);
                //step4.SetChain(step6);
                step6.SetChain(step7, step10);
                //step51.SetChain(step6);
               // step6.SetChain(step7);
                step7.SetChain(step8, step9);
                step8.SetChain(step6);
                step9.SetChain(step6);
                step10.SetChain(null);
                #endregion

                step6.HandleRequest();

                return new System.Tuple<bool, List<Inventory_Transaction>, List<Inventory_Transaction>>(true, newList,
                       oldList);
            };
        }


        public void PricingScan(DataContainer container, DateTime recieptDate, long recieptId, long userid, long charterId, long transItemId, long warehouseId)
        {



            #region setup
            var step1 = new ActivityChain<Inventory_Transaction>("PricingScan_Step1", GetRelatedTransaction(container, recieptDate, recieptId, warehouseId));
            var memorizedTransaction = step1.OutPutsList;
            var step2 = new ActivityChain<Inventory_Transaction>("PricingScan_Step2", SortRelatedTransaction(memorizedTransaction));
            memorizedTransaction = step2.OutPutsList;




            //  var step3 = new ActivityChain<object>("PricingScan_Step3", RemovePricing(container, (int)userid, memorizedTransaction));
            var step4 = new ActivityChain<object>("PricingScan_Step4", RefreshContext(container));
            var count = 0;
           // var step5 = new ConditionChain<object>("PricingScan_Step5", IsTransactionRemaining(memorizedTransaction, count));


            var step6 = new ConditionChain<Inventory_Transaction>("PricingScan_Step6", GetNext(memorizedTransaction, count));
            var step7 = new ConditionChain<object>("PricingScan_Step7", HasTransactionPricingRefrence(step6.OutPutsList));
            var step8 = new ActivityChain<object>("PricingScan_Step8", PriceSuspendedTransUsingRefrence(container, step6.OutPutsList,
                "", (int)userid, charterId.ToString()));
            var step9 = new ActivityChain<object>("PricingScan_Step9",
                PriceIssueItemInFIFO(container, step6.OutPutsList, "", (int)userid, charterId.ToString()));
            var step10= new ActivityChain<object>("PricingScan_Step10", RefreshContext(container));
            var step11 = new ConditionChain<Inventory_Transaction>("PricingScan_Step11", IsChangeTransaction(container, step6.OutPutsList));
            var step12 = new ConditionChain<object>("PricingScan_Step12", CheckFullPriced(step6.OutPutsList));
            var step13 = new ConditionChain<object>("PricingScan_Step13", HasPrevVoucher(step6.OutPutsList, 1));
            var step14 = new ConditionChain<object>("PricingScan_Step14", IsSendToFinancial());
            var step15 = new ConditionChain<object>("PricingScan_Step15", IsSendToFinancial());
            var step16 = new ActivityChain<object>("PricingScan_Step16", DeletePrevVoucher(step11.OutPutsList));
            var step17 = new ActivityChain<object>("PricingScan_Step17", PublishNewVoucher());
            var step18 = new ActivityChain<object>("PricingScan_Step18", SendTofinancial());
            var step19 = new ActivityChain<object>("PricingScan_Step19", PublishDiffVoucher());
            var step20 = new ActivityChain<object>("PricingScan_Step20", SendTofinancial());
            var step21 = new ActivityChain<object>("PricingScan_Step21", DeletePrevVoucher(step11.OutPutsList));
            var step22 = new ActivityChain<object>("PricingScan_Step22", PublishFinancialVoucher());
            var step23 = new ActivityChain<object>("PricingScan_Step23", SendTofinancial());
            #endregion

            #region Sequence

            step1.SetChain(step2);
            //step2.SetChain(step3);
            //step3.SetChain(step4);
            step2.SetChain(step4);

            step4.SetChain(step6);
            step6.SetChain(step10, step7);
           // step6.SetChain(step7);
            step7.SetChain(step8, step9);
            step8.SetChain(step6);
            step9.SetChain(step6);
            step10.SetChain(null);
            // step8.SetChain(step11);
            // step9.SetChain(step11);
            step11.SetChain(step12, null);
            step12.SetChain(step14, step13);
            step13.SetChain(step15, null);
            step14.SetChain(step19, step16);
            step15.SetChain(step22, step21);
            step16.SetChain(step17);
            step17.SetChain(step18);
            step18.SetChain(null);
            step19.SetChain(step20);
            step20.SetChain(null);
            step21.SetChain(null);
            step22.SetChain(step23);

            #endregion

            step1.HandleRequest();


        }




        #endregion


        #region Ctor



        #endregion

        #region Method

   
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

        
        List<Inventory_Transaction> GetFifoIssues(DataContainer dataContainer, DateTime recieptDate, long warehouseId)
        {
            return
                dataContainer.Inventory_Transaction.Where(c => c.Action == 2
                                                            && c.PricingReferenceId == null
                                                            && c.WarehouseId == warehouseId
                                                            && c.RegistrationDate > recieptDate
                                                            && !c.Inventory_StoreType.IsAdjustment)
                    .ToList();
        }
        List<Inventory_Transaction> GetRecieptWithFifoIssueRefrence(DataContainer dataContainer, DateTime recieptDate,
            List<int> pricingRefrenceIds, long warehouseId)
        {
            return dataContainer.Inventory_Transaction.Where(c => c.Action == 1
                                                                && pricingRefrenceIds.Contains(c.PricingReferenceId.Value)
                                                                && c.WarehouseId == warehouseId
                                                                && c.RegistrationDate > recieptDate
                                                                && !c.Inventory_StoreType.IsAdjustment)
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
                    .ToList();
        }



        #endregion

        #region Flow Method

        #region UpdateCountSubmitedReciptFlow


        private Func<bool> Removing(DataContainer container, DateTime recieptDate
            , long recieptId, long userid, long warehouseId)
        {


            return () =>
            {
                var step1 = new ActivityChain<Inventory_Transaction>("PricingScan_Step1", GetRelatedTransaction(container, recieptDate, recieptId, warehouseId));
                var memorizedTransaction = step1.OutPutsList;
                var step2 = new ActivityChain<Inventory_Transaction>("PricingScan_Step2", SortRelatedTransaction(memorizedTransaction));
                memorizedTransaction = step2.OutPutsList;
                var step3 = new ActivityChain<object>("PricingScan_Step3", RemovePricing(container, (int)userid, memorizedTransaction));
                var step4 = new ActivityChain<object>("PricingScan_Step4", RefreshContext(container));

                step1.SetChain(step2);
                step2.SetChain(step3);
                step3.SetChain(step4);
                step4.SetChain(null);

                step1.HandleRequest();
                return true;
            };
        }

        //1
        Func<bool> IsReciptPriced(Inventory_Transaction transaction)
        {

            return () =>
            {
                if (transaction == null)
                    throw new BusinessRuleException("Inv Operation Ref Is Null");
                else
                {
                    return (transaction.Status == 3 || transaction.Status == 4);
                }
            };


        }

        //2
        Func<bool> CheckCountReciptGreaterThanCountIssue(DataContainer container, long goodId,
            long wareHouseId, long transactionId, decimal newValue)
        {

            return () =>
            {
                decimal? sumIssue = 0;
                decimal? sumRecipt = 0;
                var transactions = container.Inventory_Transaction.Where(c => c.WarehouseId == wareHouseId).Include(c => c.Inventory_TransactionItem).ToList();
                transactions.ForEach(c =>
                {
                    if (c.Action == 2)
                    {
                        sumIssue += c.Inventory_TransactionItem.Where(e => e.GoodId == goodId).Sum(d => d.QuantityAmount);
                    }
                    else if ((c.Action == 1) && (c.Id != transactionId))
                    {
                        sumRecipt += c.Inventory_TransactionItem.Where(e => e.GoodId == goodId).Sum(d => d.QuantityAmount);
                    }

                });

                sumRecipt += newValue;
                return (sumRecipt - sumIssue) > 0;
            };
        }

        //3 & 26
        Func<System.Tuple<bool, List<Inventory_Transaction>>> UpdateTransactionItemQuantity(DataContainer container, Inventory_Transaction inventoryTransaction, decimal rob, long goodId, int userid)
        {
            return () =>
            {
                //<A.H> Commented by Hatefi due to new implementation of it at the bottom of method.

                //var res = new List<Inventory_Transaction>();
                //var transItem = inventoryTransaction.Inventory_TransactionItem.Single(c => c.GoodId == goodId);
                //transItem.QuantityAmount = rob;

                //if (inventoryTransaction.PricingReferenceId != null)
                //{
                //    string m = "";
                //    removeTransactionItemPrices(container, transItem.Id, userid, out m);
                //}

                //else if (inventoryTransaction.PricingReferenceId == null)
                //{

                //    if (!transItem.Inventory_TransactionItemPrice.Any(c => c.TransactionReferenceId != null))
                //        if (transItem.Inventory_TransactionItemPrice.ToList()[0] != null)
                //            transItem.Inventory_TransactionItemPrice.ToList()[0].QuantityAmount = rob;

                //}
                //else
                //{

                //}

                //res.Add(inventoryTransaction);


                //<A.H> Added by Hatefi on 1394-02-27 (Begin).
                var res = new List<Inventory_Transaction>();

                var tran = container.Inventory_Transaction.Single(t => t.Id == inventoryTransaction.Id);

                var transItem = tran.Inventory_TransactionItem.Single(c => c.GoodId == goodId);

                if (tran.PricingReferenceId != null)
                {
                    string m = "";
                    removeTransactionItemPrices(container, transItem.Id, userid, out m);
                }

                else if (tran.PricingReferenceId == null)
                {

                    if (!transItem.Inventory_TransactionItemPrice.Any(c => c.TransactionReferenceId != null))
                        if (transItem.Inventory_TransactionItemPrice.ToList()[0] != null)
                            transItem.Inventory_TransactionItemPrice.ToList()[0].QuantityAmount = rob;

                }
                else
                {

                }

                container.SaveChanges();
                refreshContext(container);

                tran = container.Inventory_Transaction.Single(t => t.Id == inventoryTransaction.Id);
                transItem = tran.Inventory_TransactionItem.Single(c => c.GoodId == goodId);
                transItem.QuantityAmount = rob;

                container.SaveChanges();
                refreshContext(container);

                res.Add(tran);
                //Added by Hatefi on 1394-02-27 (End).

                return new System.Tuple<bool, List<Inventory_Transaction>>(true, res);
            };
        }

        //4
        Func<bool> RenewedPricingCurrentReciept(DataContainer container, List<Inventory_Transaction> inventoryTransaction,
                                                string desc, int userId, string charterId)
        {
            return () =>
            {

                if (inventoryTransaction[0].PricingReferenceId != null)
                {
                    int transactionId = inventoryTransaction[0].Id;
                    string message = "";
                    priceSuspendedTransactionUsingReference(container, transactionId, desc, userId, out message,
                        InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT, charterId.ToString());
                }
                //else
                //{
                //    inventoryTransaction[0].Inventory_TransactionItem.ForEach(c =>
                //    {
                //        c.Inventory_TransactionItemPrice.ToList()[0].QuantityAmount = c.QuantityAmount;
                //    });
                //}
                return true;
            };
        }

        //41
        Func<bool> PublishVoucherCurrentReciept(List<Inventory_Transaction> inventoryTransaction, IAutomaticVoucher automaticVoucher,
            object entity, Voyage voyage, long userId)
        {
            return () =>
            {
                //  if (inventoryTransaction[0].PricingReferenceId != null)
                //  {
                var goodRepository = ServiceLocator.Current.GetInstance<IGoodRepository>();

                if (entity is CharterIn)
                {
                    if (inventoryTransaction[0].Status == 3 || inventoryTransaction[0].Status == 4)
                    {
                        var lst = inventoryTransaction[0].CreateReceiptDataForFinanceArticles(
                       (entity as CharterIn).ChartererId.Value
                       , goodRepository);
                        var voucher = (IAddCharterInStartReceiptVoucher)automaticVoucher;
                        voucher.Execute((entity as CharterIn),
                            lst,
                            inventoryTransaction[0].Inventory_Warehouse.Code,
                            inventoryTransaction[0].GetActionNumber(inventoryTransaction[0].Inventory_Warehouse.Code),
                            userId,
                            voyage.VoyageNumber,
                            "", false);
                    }


                }
                // }

                return true;
            };
        }

        //5
        Func<bool> HasVoucher(Inventory_Transaction inventoryTransaction, long goodId,
                            ICheckVoucher checkVoucher)
        {
            return () =>
            {
                var res = inventoryTransaction.Inventory_TransactionItem.Single(c => c.GoodId == goodId);
                var headerNo = inventoryTransaction.GetActionNumber(inventoryTransaction.Inventory_Warehouse.Code);
                return checkVoucher.HasVoucher(headerNo, res.Id);

            };
        }

        //6
        Func<bool> IsQuantityIssueFromCurrentRecieptGreaterthanZero(DataContainer container, Inventory_TransactionItem transactionItem)
        {
            return () =>
            {
                var res = transactionItem.Inventory_TransactionItemPrice.Sum(c => c.QuantityAmountUseFifo);

                return res > 0;
            };
        }

        //7
        Func<bool> IsSendToFinancial()
        {
            return () =>
            {
                return true;
            };
        }
        Func<bool> IsSendToFinancial(Voucher voucher)
        {
            return () =>
            {
                return false;
            };
        }
        //8
        Func<bool> DeleteVoucher(long inventoryItemId, Inventory_Transaction inventoryTransaction, IDeleteVoucher deleteVoucher)
        {
            return () =>
            {
                deleteVoucher.Done(inventoryItemId, inventoryTransaction.GetActionNumber(inventoryTransaction.Inventory_Warehouse.Code));
                return true;
            };
        }

        //11
        Func<System.Tuple<bool, List<Inventory_OperationReference>>> PublishRevertIssue(DataContainer dbContext
            , int companyId, int warehouseId,
              int? pricingReferenceId, string referenceType,
              string referenceNumber, int userId, long goodId, decimal diffValue, string goodUnitAbbreviation,DateTime effectiveDate)
        {
            return () =>
            {
                var timeBucketId = getTimeBucketId(dbContext, effectiveDate);
                string code;
                string message;
                var res = new List<Inventory_OperationReference>();

                var resIssue = issue(dbContext,
                    companyId,
                    warehouseId,
                    timeBucketId,
                   effectiveDate,
                    getAdjustmentIssueType(dbContext),
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
                    QuantityUnitId = getMeasurementUnitId(dbContext, goodUnitAbbreviation),
                    TransactionId = (int)resIssue.OperationId,
                    UserCreatorId = userId
                });

                string transactionItemMessage;
                var transactionItemIds = addTransactionItems(dbContext, (int)resIssue.OperationId,
                    transactionItem, userId, out transactionItemMessage);


                string pricingMessage;


                priceSuspendedTransactionUsingReference(
                    dbContext,
                    (int)resIssue.OperationId,
                    InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_ISSUE
                    , userId,
                    out pricingMessage,
                    InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_ISSUE,
                    referenceNumber);



                res.Add(resIssue);
                return new System.Tuple<bool, List<Inventory_OperationReference>>(true, res); ;
            };
        }

        //19 12
        Func<bool> IsAmountReciptGreaterthanAdjustment(DataContainer container, Inventory_Transaction transaction, decimal newValue, long goodId)
        {
            return () =>
            {
                //var transactionItem = transaction.Inventory_TransactionItem.Single(c => c.GoodId == goodId);
                //var deltaValue = transactionItem.QuantityAmount - newValue;
                //var val = transactionItem.Inventory_TransactionItemPrice.Sum(c => c.QuantityAmount);
                //return val > deltaValue;

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

            };
        }

        //15 13
        Func<bool> IsPositiveAdjustment(Inventory_Transaction transaction, long goodId, decimal newvalue)
        {

            return () =>
            {
                var transItem = transaction.Inventory_TransactionItem.Single(c => c.GoodId == goodId);
                return transItem.QuantityAmount < newvalue;
            };
        }

        //14 
        Func<bool> IsQuantityAmountCurrentGreaterthanZero(DataContainer container, Inventory_TransactionItem transactionItem)
        {
            return () =>
            {

                return transactionItem.QuantityAmount > 0;
            };
        }


        //18
        Func<bool> HasIssueAfterIssueCurrentReciept(DataContainer container, long wareHouseId, long goodId, DateTime dateTime)
        {
            return () =>
            {
                var res = container.Inventory_Transaction.Where(c =>
                    c.Action == 2 &&
                    c.WarehouseId == wareHouseId &&
                    c.ReferenceDate > dateTime
                    ).Select(d => d);


                return Enumerable.Any(res, inventoryTransaction => inventoryTransaction.Inventory_TransactionItem.Any(c => c.GoodId == goodId));
            };
        }

        //20
        Func<bool> PublishVoucher(

                                List<Inventory_OperationReference> inventoryOperationReference,
                                InventoryOperationType inventoryOperationType,
                                object entity,
                                long userId,
                                string lineCode,
                                string voyageCode)
        {
            return () =>
            {

                if (entity is CharterIn)
                {
                    var goodRepository = ServiceLocator.Current.GetInstance<IGoodRepository>();
                    var transactionId = inventoryOperationReference[0].OperationId;
                    var trans = GetTransaction(transactionId, inventoryOperationType);
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
            };
        }

        //21
        Func<bool> PublishReciptPricing(DataContainer container,
                                        int companyId,
                                        int warehouseId,
                                        string referenceNumber,
                                        int transactionId,
                                        int userId,
                                        string code,
                                        string message,
                                        long goodId,
                                        string goodCode,
                                        decimal rob,
                                        string goodUnitAbbreviation
                                      )
        {
            return () =>
            {
                var timeBucketId = getTimeBucketId(container, DateTime.Now);
                var res = receipt(
                     container,
                     companyId,
                     warehouseId,
                     timeBucketId,
                     DateTime.Now,
                     getAdjustmentReceiptType(container),
                     transactionId,
                     null,
                     InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_RECEIPT,
                     referenceNumber,
                     userId,
                     out code,
                     out message);
                string message1;

                var transactionItems = new List<Inventory_TransactionItem>();
                transactionItems.Add(new Inventory_TransactionItem()
                {
                    GoodId = (int)goodId,
                    CreateDate = DateTime.Now,
                    Description = "Charter In Start Adjustment> " + goodCode,
                    QuantityAmount = rob,//diffrent
                    QuantityUnitId = getMeasurementUnitId(container, goodUnitAbbreviation),
                    TransactionId = (int)res.OperationId,
                    UserCreatorId = userId
                });
                addTransactionItems(container, (int)res.OperationId, transactionItems, userId, out message1);
                priceSuspendedTransactionUsingReference(container, (int)res.OperationId, "Charter In Start Adjustment", userId, out message1, InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_RECEIPT, referenceNumber);

                return true;
            };
        }

        //22 & 25
        Func<bool> PricingScanPublishVoucher(DataContainer container, DateTime recieptDate, long recieptId, long userid, long charterId, long transItemId, long warehouseId, int? pricingReferenceId)
        {
            return () =>
            {
                // if (pricingReferenceId != null)
                //{
                PricingScan(container, recieptDate, recieptId, userid, charterId, transItemId, warehouseId);
                // }
                return true;
            };
        }

        //27
        Func<System.Tuple<bool, List<Inventory_Transaction>>> CalculationDiff(DataContainer container)//,List<Inventory_Transaction> oldList)
        {
            return () =>
            {
                var newList = new List<Inventory_Transaction>();

                //oldList.ForEach(c =>
                //{
                //    var res = container.Inventory_Transaction.Single(d=>d.Id==c.Id);

                //    foreach (var inventoryTransactionItem in res.Inventory_TransactionItem)
                //    {

                //    }


                //  newList.Add(res);



                //   });

                return new System.Tuple<bool, List<Inventory_Transaction>>(false, new List<Inventory_Transaction>());
            };
        }

        //28
        Func<bool> PublishVoucherSendFinancial()
        {
            return () => true;
        }

        //29 //22
        Func<bool> PublishVoucherCorrective(DataContainer container,
            List<Inventory_Transaction> oldList,
            List<Inventory_Transaction> newList,
            long shareGoodId,
            long userId)
        {
            return () =>
            {
                oldList.ForEach(c =>
                {
                    var headerNo = c.GetActionNumber(c.Inventory_Warehouse.Code);
                    var deleteVoucher = ServiceLocator.Current.GetInstance<IDeleteVoucher>();
                    var newTrans = newList.Single(d => d.Id == c.Id);
                    var vouchers = container.Vouchers.Where(x => x.ReferenceNo == headerNo).ToList();

                   // if (c.Status == 3)
                 if (c.Status == 4)
                    {
                        vouchers.ForEach(d =>
                        {
                            if (!IsSendToFinancial(d).Invoke())
                            {
                                deleteVoucher.Done(0, headerNo);
                            }
                        
                        });
                      
                    }
                   // if (newTrans.Status == 4)
                  if (newTrans.Status == 3)
                    {
                        
                        vouchers.ForEach(v =>
                            {
                                if (IsSendToFinancial(v).Invoke())
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
                                   // deleteVoucher.Done(0, headerNo);
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





                });

                return true;
            };

        }


        #endregion

        #region UpdatePriceSubmitedReciptFlow

        // 2 
        Func<System.Tuple<bool, List<Inventory_Transaction>>> UpdateTransactionItemPrice(DataContainer container, Inventory_Transaction inventoryTransaction, decimal newprice, long goodId)
        {
            return () =>
            {
                var res = new List<Inventory_Transaction>();
                var transItem = inventoryTransaction.Inventory_TransactionItem.Single(c => c.GoodId == goodId);

                //is manual
                if (inventoryTransaction.PricingReferenceId != null)
                {
                    if (transItem.Inventory_TransactionItemPrice.ToList()[0] != null)
                    {
                        transItem.Inventory_TransactionItemPrice.ToList()[0].FeeInMainCurrency =
                            getExchangeRate(container,
                                            transItem.Inventory_TransactionItemPrice.ToList()[0].Inventory_Unit_PriceUnit.Abbreviation,
                                            transItem.Inventory_TransactionItemPrice.ToList()[0].Inventory_Unit_MainCurrencyUnit.Abbreviation,
                                            inventoryTransaction.ReferenceDate.Value
                                                      ) * newprice;
                        transItem.Inventory_TransactionItemPrice.ToList()[0].Fee = newprice;
                    }


                }

                res.Add(inventoryTransaction);

                return new System.Tuple<bool, List<Inventory_Transaction>>(true, res);
            };
        }

        //3
        Func<bool> HasIssueRegistered(DataContainer container, Inventory_TransactionItem transactionItem)
        {

            return () =>
            {
                var issuedQuantity = transactionItem.Inventory_TransactionItemPrice.Sum(c => c.QuantityAmountUseFifo);

                // var itemPrices =
                //container.Inventory_TransactionItemPrice.Where(c => c.TransactionReferenceId == transactionItemId)
                //.Include(d => d.Inventory_TransactionItem).Include(d => d.Inventory_TransactionItem.Inventory_Transaction)
                //.Select(d => d).ToList();


                // var trans =
                //     itemPrices.Where(c => c.Inventory_TransactionItem.Inventory_Transaction.Action == 2).Select(d => d);

                return (issuedQuantity > 0);
            };
        }
        #endregion


        #region PricingScan

        Func<System.Tuple<bool, List<Inventory_Transaction>>> CloneList(List<Inventory_Transaction> inventoryTransactions)
        {
            return () =>
            {
               var res= inventoryTransactions.Clone();

               return new System.Tuple<bool, List<Inventory_Transaction>>(true, res);
            };
        }

        Func<System.Tuple<bool, List<Inventory_Transaction>>> GetRelatedTransaction(DataContainer container, DateTime recieptDate, long recieptId, long warehouseId)
        {
            return () =>
            {
                var res = new List<Inventory_Transaction>();
                res.AddRange(GetRecieptWithCurrentRecieptRefrence(container, recieptDate, recieptId, warehouseId));
                res.AddRange(GetIssuesWithCurrentRecieptRefrence(container, recieptDate, recieptId, warehouseId));

                var res1 = GetFifoIssues(container, recieptDate, warehouseId);
                res.AddRange(res1);
                res.AddRange(GetRecieptWithFifoIssueRefrence(container, recieptDate, res1.Select(c => c.Id).ToList(), warehouseId));


                return new System.Tuple<bool, List<Inventory_Transaction>>(true, res);
            };
        }

        Func<System.Tuple<bool, List<Inventory_Transaction>>> SortRelatedTransaction(IEnumerable<Inventory_Transaction> inventoryTransactions)
        {
            return () =>
            {
                return new System.Tuple<bool, List<Inventory_Transaction>>(true,
                    inventoryTransactions.OrderBy(c => c.RegistrationDate).ToList());
            };
        }

        Func<bool> RemovePricing(DataContainer container, int userid, List<Inventory_Transaction> inventoryTransactions)
        {
            return () =>
            {
                var message = "";
                var lst = new List<Inventory_Transaction>();
                inventoryTransactions.ForEach(lst.Add);
                lst.OrderByDescending(c => c.RegistrationDate).ToList().ForEach(c =>
                {
                    foreach (var inventoryTransactionItem in c.Inventory_TransactionItem)
                    {
                        removeTransactionItemPrices(container, inventoryTransactionItem.Id, userid, out message);
                    }

                });

                return true;
            };
        }
        Func<bool> RefreshContext(DataContainer container)
        {
            return () =>
            {

                refreshContext(container);
                return true;
            };
        }

        Func<bool> IsTransactionRemaining(List<Inventory_Transaction> inventoryTransactions, int count)
        {
        
            return () =>
            {

                return count<inventoryTransactions.Count;
            };
        }

        Func<System.Tuple<bool, List<Inventory_Transaction>>> GetNext(IEnumerable<Inventory_Transaction> inventoryTransactions, int count)
        {
            return () =>
            {
                if (count < inventoryTransactions.Count())
                {
                    var res = new Inventory_Transaction();
                   
                        res = inventoryTransactions.ToList()[count];
                        count++;
                        return new System.Tuple<bool, List<Inventory_Transaction>>(true,
                            new List<Inventory_Transaction>()
                            {
                                res
                            });
                   
                   
                }
                return new System.Tuple<bool, List<Inventory_Transaction>>(false,
                    new List<Inventory_Transaction>());
            };
        }

        Func<bool> HasTransactionPricingRefrence(List<Inventory_Transaction> inventoryTransaction)
        {
            return () =>
            {
                if (inventoryTransaction != null && inventoryTransaction.Count > 0)
                    return inventoryTransaction[0].PricingReferenceId != null;
                else
                    return false;
            };
        }

        Func<bool> PriceSuspendedTransUsingRefrence(DataContainer container, List<Inventory_Transaction> inventoryTransaction, string desc, int userId, string charterId)
        {
            return () =>
            {
              
                int transactionId = inventoryTransaction[0].Id;
                string message = "";
                priceSuspendedTransactionUsingReference(container, transactionId, desc, userId, out message,
                    InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT, charterId.ToString());

                return true;
            };
        }

        //Func<bool> PriceIssueItemInFIFO(DataContainer container, int transactionItemId, string desc, int userId, string charterId)
        Func<bool> PriceIssueItemInFIFO(DataContainer container, List<Inventory_Transaction> inventoryTransaction, string desc, int userId, string charterId)
        {
            return () =>
            {
                string message = "";
                foreach (var inventoryTransactionItem in inventoryTransaction[0].Inventory_TransactionItem)
                {
                    try
                    {
                        priceIssuedItemInFIFO(container, inventoryTransactionItem.Id, desc, userId, out message,
                                              inventoryTransaction[0].ReferenceType, inventoryTransaction[0].ReferenceNo);
                    }
                    catch
                    {
                    }
                }
                return true;
            };
        }

        //11
        Func<bool> IsChangeTransaction(DataContainer container, List<Inventory_Transaction> inventoryTransactions)
        {
            return () =>
            {

                var flag = false;
                var res = container.Inventory_Transaction.SingleOrDefault(c => c.Id == inventoryTransactions[0].Id);
                res.Inventory_TransactionItem.ForEach(r =>
                {
                    var rVal = r.Inventory_TransactionItemPrice.Sum(c => c.FeeInMainCurrency * c.QuantityAmount);
                    var tranVal = inventoryTransactions[0].Inventory_TransactionItem.SingleOrDefault(d => d.Id == r.Id)
                          .Inventory_TransactionItemPrice.Sum(c => c.FeeInMainCurrency * c.QuantityAmount);
                    if (rVal != tranVal)
                        flag = true;
                });
                return flag;

            };
        }

        //12
        Func<bool> CheckFullPriced(List<Inventory_Transaction> inventoryTransactions)
        {
            return () => (inventoryTransactions[0].Status == 3 || inventoryTransactions[0].Status == 4);
        }

        //13
        Func<bool> HasPrevVoucher(List<Inventory_Transaction> inventoryTransactions, long goodId)
        {
            return () =>
            {

                var ivch = ServiceLocator.Current.GetInstance<ICheckVoucher>();
                return HasVoucher(inventoryTransactions[0], goodId, ivch).Invoke();

            };
        }

        //16
        Func<bool> DeletePrevVoucher(List<Inventory_Transaction> inventoryTransactions)
        {
            return () =>
            {
                var idel = ServiceLocator.Current.GetInstance<IDeleteVoucher>();
                return DeleteVoucher(0, inventoryTransactions[0], idel).Invoke();
            };
        }

        //17
        Func<bool> PublishNewVoucher()
        {
            return () => true;
        }


        //18
        Func<bool> SendTofinancial()
        {
            return () => true;
        }

        //19
        Func<bool> PublishDiffVoucher()
        {
            return () => true;
        }

        //22
        Func<bool> PublishFinancialVoucher()
        {
            return () => true;
        }





        #endregion


        #region Other
        Func<bool> IsCharterOutStartOrCharterInEnd()
        {
            return () => true;
        }


        private Func<bool> createRepriceParticipantTransactionsStepFunction(DataContainer context, List<Inventory_Transaction> copiedInventoryTransactions)
        {
            return () =>
            {

                Inventory_Transaction processingCopiedTransaction = null;

                #region Steps Definition

                var priceTransactionStep = new ActivityChain<Inventory_Transaction>("priceTransactionStep", createPriceTransactionStepFunction(context, processingCopiedTransaction));

                var partialPriceToPartialPriceChangeConditionStep = new ConditionChain<object>("partialPriceToPartialPriceChangeConditionStep", createPartialPriceToPartialPriceChangeConditionStep(processingCopiedTransaction, priceTransactionStep.OutPutsList));
                var partialPriceToFullPriceChangeConditionStep = new ConditionChain<object>("partialPriceToFullPriceChangeConditionStep", createPartialPriceToFullPriceChangeConditionStep(processingCopiedTransaction, priceTransactionStep.OutPutsList));
                var fullPriceToPartialPriceChangeConditionStep = new ConditionChain<object>("fullPriceToPartialPriceChangeConditionStep", createFullPriceToPartialPriceChangeConditionStep(processingCopiedTransaction, priceTransactionStep.OutPutsList));
                var fullPriceToFullPriceChangeConditionStep = new ConditionChain<object>("fullPriceToFullPriceChangeConditionStep", createFullPriceToFullPriceChangeConditionStep(processingCopiedTransaction, priceTransactionStep.OutPutsList));


                var createSingleTransactionVoucherStep = new ActivityChain<Inventory_Transaction>("createSingleTransactionVoucherStep", createCreateSingleTransactionVoucherStepFunction(context, priceTransactionStep.OutPutsList));

                //var makeCopyOfParticipantTransactionsStep = new ActivityChain<Inventory_Transaction>("makeCopyOfParticipantTransactionsStep", createMakeCopyOfParticipantTransactionsStepFunction(context, findParticipantTransactionsStep.OutPutsList));
                //var removePricingOfParticipantTransactionsStep = new ActivityChain<Inventory_Transaction>("removePricingOfParticipantTransactionsStep", createRemovePricingOfParticipantTransactionsStepFunction(context, findParticipantTransactionsStep.OutPutsList));
                //var repriceParticipantTransactionsStep = new ActivityChain<Inventory_Transaction>("repriceParticipantTransactionsStep", createRepriceParticipantTransactionsStepFunction(context, makeCopyOfParticipantTransactionsStep.OutPutsList));



                //var findParticipantTransactionsStep = new ActivityChain<object>("");

                #endregion

                #region Steps Flow Configuration

                #endregion

                #region Start Flow

                //findInitiatingTransactionsStep.HandleRequest();

                #endregion

                return true;
            };
        }

        private Func<bool> createCreateSingleTransactionVoucherStepFunction(DataContainer context, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }

        private Func<bool> createFullPriceToFullPriceChangeConditionStep(Inventory_Transaction processingCopiedTransaction, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }

        private Func<bool> createFullPriceToPartialPriceChangeConditionStep(Inventory_Transaction processingCopiedTransaction, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }

        private Func<bool> createPartialPriceToFullPriceChangeConditionStep(Inventory_Transaction processingCopiedTransaction, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }

        private Func<bool> createPartialPriceToPartialPriceChangeConditionStep(Inventory_Transaction processingCopiedTransaction, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }

        private Func<System.Tuple<bool, List<Inventory_Transaction>>> createPriceTransactionStepFunction(DataContainer context, Inventory_Transaction processingCopiedTransaction)
        {
            throw new NotImplementedException();
        }

        private Func<System.Tuple<bool, List<Inventory_Transaction>>> createRemovePricingOfParticipantTransactionsStepFunction(DataContainer context, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }

        private Func<System.Tuple<bool, List<Inventory_Transaction>>> createMakeCopyOfParticipantTransactionsStepFunction(DataContainer context, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }

        private Func<System.Tuple<bool, List<Inventory_Transaction>>> createFindInitiatingTransactionsStepFunction(DataContainer context, Inventory_Transaction startingFromInventoryTransaction)
        {
            throw new NotImplementedException();
        }

        private Func<System.Tuple<bool, List<Inventory_Transaction>>> createFindParticipantTransactionsStepFunction(DataContainer context, List<Inventory_Transaction> outPutsList)
        {
            throw new NotImplementedException();
        }


        #endregion

        #endregion





    }
}
