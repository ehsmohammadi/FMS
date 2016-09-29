using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Integration.Inventory.Infrastructure;

namespace MITD.Fuel.Integration.Inventory
{
    public static class Utitlity
    {
        public static void ExecuteAutomaticVoucher(
            Inventory_Transaction oldInventoryTransaction,
            Inventory_Transaction newInventoryTransaction,
            long userId,
            long shardGoodId,
            bool isReform,InvoiceTypes? invoiceTypes

            )
        {
            IAutomaticVoucher automaticVoucher = null;
            var charterInRepository = ServiceLocator.Current.GetInstance<ICharterInRepository>();
            var charterInDomainService = ServiceLocator.Current.GetInstance<ICharterInDomainService>();
             var charterOutDomainService = ServiceLocator.Current.GetInstance<ICharterOutDomainService>();
            var charterOutRepository = ServiceLocator.Current.GetInstance<ICharterOutRepository>();
            var fuelReportRepository = ServiceLocator.Current.GetInstance<IFuelReportRepository>();
            var orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();
            var goodRepository = ServiceLocator.Current.GetInstance<IGoodRepository>();
            var invoiceRepository = ServiceLocator.Current.GetInstance<IInvoiceRepository>();
            var currencyDomainService = ServiceLocator.Current.GetInstance<ICurrencyDomainService>();
            string refType = oldInventoryTransaction.ReferenceType;
            long refNo =long.Parse(oldInventoryTransaction.ReferenceNo);
            string inventoryActionNumber =
                oldInventoryTransaction.GetActionNumber(oldInventoryTransaction.Inventory_Warehouse.Code);
            var lineCode = "";
            var voyageCode = "";
            
            switch (refType)
            {
                #region CHARTER_IN_START_RECEIPT
                case InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterInStartReceiptVoucher>();
                        var entityFuel = charterInRepository.FindByKey(refNo);
                        var receipts = new List<Receipt>();
                        var charterInStartVoyage = charterInDomainService.GetVoyageCharterInStart(entityFuel.ChartererId.Value, entityFuel.VesselInCompanyId.Value, entityFuel.ActionDate);
                        lineCode = charterInStartVoyage.VoyageNumber;
                        
                        if (isReform)
                        {
                            receipts.Add(newInventoryTransaction.CreateDifferentialReceiptDataForFinanceArticles
                                (oldInventoryTransaction,
                                 entityFuel.ChartererId.Value
                                 ,shardGoodId
                                 ,goodRepository));
                        }
                        else
                        {
                            receipts = newInventoryTransaction.CreateReceiptDataForFinanceArticles(
                                entityFuel.ChartererId.Value, goodRepository);
                        }
                        
                       
                        ((IAddCharterInStartReceiptVoucher)automaticVoucher).Execute(
                           (CharterIn)entityFuel,
                           receipts,
                           oldInventoryTransaction.Inventory_Warehouse.Code,
                           inventoryActionNumber,
                           userId,
                           lineCode,
                           voyageCode);
                        break;
                    }
                #endregion
                #region CHARTER_IN_START_RECEIPT_PRICING
                //case InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT_PRICING:
                case InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_ISSUE:
                case InventoryOperationReferenceTypes.CHARTER_IN_START_ADJUSTMENT_RECEIPT:
                    {
                        // automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterInStartReceiptVoucher>();
                        break;
                    }
                #endregion
                #region CHARTER_IN_END_ISSUE
                case InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterInEndIssueVoucher>();
                        var entityFuel = charterInRepository.FindByKey(refNo);
                        var issues = new List<Issue>();
                        var charterInEndVoyage = charterInDomainService.GetVoyageCharterInEnd(entityFuel.ChartererId.Value, entityFuel.VesselInCompanyId.Value, entityFuel.ActionDate);
                        lineCode = charterInEndVoyage.VoyageNumber;
                        
                        if (isReform)
                        {
                          issues.Add(newInventoryTransaction.CreateDifferentialIssueDataForFinanceArticles(
                              oldInventoryTransaction,
                              entityFuel.ChartererId.Value,
                              shardGoodId,
                              goodRepository
                              )); 
                        }
                        else
                        {
                            issues = newInventoryTransaction.CreateIssueDataForFinanceArticles(
                                 entityFuel.ChartererId.Value, goodRepository);
                        }
                        ((IAddCharterInEndIssueVoucher)automaticVoucher).Execute(
                          (CharterIn)entityFuel,
                          issues,
                          oldInventoryTransaction.Inventory_Warehouse.Code,
                         inventoryActionNumber,
                         userId,
                          lineCode,
                          voyageCode
                          );
                        break;
                    }
                #endregion
                #region CHARTER_IN_END_ISSUE_PRICING
                //case InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE_PRICING:
                //    {
                //        //  automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterInEndIssueVoucher>();
                //        break;
                //    }
                #endregion
                #region CHARTER_IN_END_DECREMENTAL_CORRECTION
                case InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterInEndConsumptionIssueVoucher>();
                        break;
                    }
                #endregion
                #region CHARTER_IN_END_INCREMENTAL_CORRECTION
                case InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterInEndBackReciptVoucher>();
                        var entityFuel = charterInRepository.FindByKey(refNo);
                        var receipts = new List<Receipt>();
                        var charterInEndIncrementalCorrectionVoyage = charterInDomainService.GetVoyageCharterInEnd(entityFuel.ChartererId.Value, entityFuel.VesselInCompanyId.Value, entityFuel.ActionDate);
                        lineCode = charterInEndIncrementalCorrectionVoyage.VoyageNumber;
                        if (isReform)
                        {
                            receipts.Add(newInventoryTransaction.CreateDifferentialReceiptDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.ChartererId.Value,
                                shardGoodId,
                                goodRepository));
                        }
                        else
                        {
                            receipts =
                                newInventoryTransaction.CreateReceiptDataForFinanceArticles(
                                    entityFuel.ChartererId.Value, goodRepository);
                        }

                        
                        ((IAddCharterInEndBackReciptVoucher)automaticVoucher).Execute(
                          (CharterIn)entityFuel,
                          receipts,
                          oldInventoryTransaction.Inventory_Warehouse.Code,
                          inventoryActionNumber,
                          userId,
                          lineCode,
                          voyageCode
                          );

                        break;
                    }
                #endregion
                #region CHARTER_IN_END_DECREMENTAL_CORRECTION_PRICING
                //case InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION_PRICING:
                //case InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION_PRICING:
                //    {
                //        //automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterInEndBackReciptVoucher>();
                //        break;
                //    }
                #endregion
                #region CHARTER_OUT_START_ISSUE
                case InventoryOperationReferenceTypes.CHARTER_OUT_START_ISSUE:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterOutStartIssueVoucher>();
                        var entityFuel = charterOutRepository.FindByKey(refNo);
                        var issues = new List<Issue>();
                         var charterOutStartVoyage = charterOutDomainService.GetVoyageCharterInEnd(entityFuel.OwnerId.Value, entityFuel.VesselInCompanyId.Value, entityFuel.ActionDate);
                        lineCode = charterOutStartVoyage.VoyageNumber;
                        if (isReform)
                        {
                            issues.Add(newInventoryTransaction.CreateDifferentialIssueDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.OwnerId.Value,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            issues = newInventoryTransaction.CreateIssueDataForFinanceArticles(
                                 entityFuel.OwnerId.Value, goodRepository);
                        }
                        
                        ((IAddCharterOutStartIssueVoucher)automaticVoucher).Execute(
                           (CharterOut)entityFuel,
                          issues,
                          oldInventoryTransaction.Inventory_Warehouse.Code,
                          inventoryActionNumber,
                          userId,
                          lineCode,
                          voyageCode
                           );


                        break;
                    }
                #endregion
                #region CHARTER_OUT_START_ISSUE_PRICING
                //case InventoryOperationReferenceTypes.CHARTER_OUT_START_ISSUE_PRICING:
                //case InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION_PRICING:
                //case InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION_PRICING:
                //    {
                //        //automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterOutStartIssueVoucher>();

                //        break;
                //    }
                #endregion
                #region CHARTER_OUT_START_INCREMENTAL_CORRECTION
                case InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterOutStartBackReceiptVoucher>();
                        var entityFuel = charterOutRepository.FindByKey(refNo);
                        var receipts = new List<Receipt>();
                        var charterOutStartIncrementalCorrectionVoyage = charterOutDomainService.GetVoyageCharterInEnd(entityFuel.OwnerId.Value, entityFuel.VesselInCompanyId.Value, entityFuel.ActionDate);
                        lineCode = charterOutStartIncrementalCorrectionVoyage.VoyageNumber;
                        if (isReform)
                        {
                            receipts.Add(newInventoryTransaction.CreateDifferentialReceiptDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.OwnerId.Value,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            receipts = newInventoryTransaction.CreateReceiptDataForFinanceArticles(
                                 entityFuel.OwnerId.Value, goodRepository);
                        }
                        
                        
                        ((IAddCharterOutStartBackReceiptVoucher)automaticVoucher).Execute(
                         (CharterOut)entityFuel,
                        receipts,
                        oldInventoryTransaction.Inventory_Warehouse.Code,
                        inventoryActionNumber,
                        userId,
                         lineCode,
                         voyageCode
                      );
                        break;
                    }
                #endregion
                #region CHARTER_OUT_START_DECREMENTAL_CORRECTION
                case InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterOutStartConsumptionIssueVoucher>();
                        var entityFuel = charterOutRepository.FindByKey(refNo);
                        var issues = new List<Issue>();
                        var charterOutStartDecrementalCorrectionVoyage = charterOutDomainService.GetVoyageCharterInEnd(entityFuel.OwnerId.Value, entityFuel.VesselInCompanyId.Value, entityFuel.ActionDate);
                        lineCode = charterOutStartDecrementalCorrectionVoyage.VoyageNumber;
                        if (isReform)
                        {
                            issues.Add(newInventoryTransaction.CreateDifferentialIssueDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.OwnerId.Value,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            issues = newInventoryTransaction.CreateIssueDataForFinanceArticles(
                                 entityFuel.OwnerId.Value, goodRepository);
                        }
                        
                        ((IAddCharterOutStartConsumptionIssueVoucher)automaticVoucher).Execute(
                          (CharterOut)entityFuel,
                          issues,
                          oldInventoryTransaction.Inventory_Warehouse.Code,
                          inventoryActionNumber,
                          userId,
                          lineCode,
                          voyageCode
                          );
                        break;
                    }
                #endregion
                #region CHARTER_OUT_END_RECEIPT
                case InventoryOperationReferenceTypes.CHARTER_OUT_END_RECEIPT:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterOutEndReceiptVoucher>();
                        var entityFuel = charterOutRepository.FindByKey(refNo);
                        var receipts = new List<Receipt>();
                        var charterOutEndVoyage = charterOutDomainService.GetVoyageCharterInEnd(entityFuel.OwnerId.Value, entityFuel.VesselInCompanyId.Value, entityFuel.ActionDate);
                        lineCode = charterOutEndVoyage.VoyageNumber;
                        if (isReform)
                        {
                            receipts.Add(newInventoryTransaction.CreateDifferentialReceiptDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.OwnerId.Value,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            receipts = newInventoryTransaction.CreateReceiptDataForFinanceArticles(
                                 entityFuel.OwnerId.Value, goodRepository);
                        }
                        
                        
                        ((IAddCharterOutEndReceiptVoucher)automaticVoucher).Execute(
                       (CharterOut)entityFuel,
                      receipts,
                       oldInventoryTransaction.Inventory_Warehouse.Code,
                      inventoryActionNumber,
                    userId,
                       lineCode,
                       voyageCode
                       );
                        break;
                    }
                #endregion
                #region CHARTER_OUT_END_RECEIPT_PRICING
                //case InventoryOperationReferenceTypes.CHARTER_OUT_END_RECEIPT_PRICING:
                //    {
                //        // automaticVoucher = ServiceLocator.Current.GetInstance<IAddCharterOutEndReceiptVoucher>();

                //        break;
                //    }
                #endregion
                #region EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION
                case InventoryOperationReferenceTypes.EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddConsumptionIssueVoucher>();
                        var entityFuel = fuelReportRepository.FindByKey(refNo);
                        var issues = new List<Issue>();
                        if (isReform)
                        {
                            issues.Add(newInventoryTransaction.CreateDifferentialIssueDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.VesselInCompany.CompanyId,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            issues = newInventoryTransaction.CreateIssueDataForFinanceArticles(
                                   entityFuel.VesselInCompany.CompanyId, goodRepository);
                        }
                        ((IAddConsumptionIssueVoucher)automaticVoucher).Execute(
                       issues,
                        entityFuel,
                         entityFuel.VesselInCompany.Vessel.Code,
                        inventoryActionNumber,
                         userId
                     );
                        break;
                    }
                #endregion
                #region EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION_PRICING
                //case InventoryOperationReferenceTypes.EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION_PRICING:
                //    {
                //        //automaticVoucher = ServiceLocator.Current.GetInstance<IAddConsumptionIssueVoucher>();

                //        break;
                //    }
                #endregion
                #region FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION
                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddMinusCorrectionReceiptVoucher>();
                        var entityFuel = fuelReportRepository.FindByKey(refNo);
                        var issues = new List<Issue>();
                        if (isReform)
                        {
                            issues.Add(newInventoryTransaction.CreateDifferentialIssueDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.VesselInCompany.CompanyId,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            issues = newInventoryTransaction.CreateIssueDataForFinanceArticles(
                                   entityFuel.VesselInCompany.CompanyId, goodRepository);
                        }
                        ((IAddMinusCorrectionReceiptVoucher)automaticVoucher).Execute(
                          entityFuel,
                            issues,
                           entityFuel.VesselInCompany.Vessel.Code,
                           inventoryActionNumber,
                         userId
                           );
                        break;
                    }
                #endregion
                #region FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION
                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddPlusCorrectionReceiptVoucher>();
                        var entityFuel = fuelReportRepository.FindByKey(refNo);
                        var receipts = new List<Receipt>();
                        if (isReform)
                        {
                            receipts.Add(newInventoryTransaction.CreateDifferentialReceiptDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.VesselInCompany.CompanyId,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            receipts = newInventoryTransaction.CreateReceiptDataForFinanceArticles(
                                   entityFuel.VesselInCompany.CompanyId, goodRepository);
                        }
                        ((IAddPlusCorrectionReceiptVoucher)automaticVoucher).Execute(
                          entityFuel,
                           receipts,
                          entityFuel.VesselInCompany.Vessel.Code,
                          inventoryActionNumber,
                          userId
                          );
                        break;
                    }
                #endregion
                #region FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION_PRICING
                //case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION_PRICING:
                //case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION_PRICING:
                //    {
                //        //  automaticVoucher = ServiceLocator.Current.GetInstance<IAddPlusCorrectionReceiptVoucher>();

                //        break;
                //    }
                #endregion
                #region FUEL_REPORT_DETAIL_RECEIVE
                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE:
                    {
                        //Todo get second entity 
                       // var secondEntityFuel = fuelReportRepository.FindByKey(secondRefNo);
                        var secondEntityFuel = fuelReportRepository.FindByKey(0);
                        var entityFuel = invoiceRepository.FindByKey(refNo);
                        var exchangeRate = currencyDomainService.GetCurrencyToMainCurrencyRate(entityFuel.CurrencyId,
                                                entityFuel.InvoiceDate);
                        var receipts = new List<Receipt>();
                        if (isReform)
                        {
                            receipts.Add(newInventoryTransaction.CreateDifferentialReceiptDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.OwnerId,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            receipts = newInventoryTransaction.CreateReceiptDataForFinanceArticles(
                                   entityFuel.OwnerId, goodRepository);
                        }



                        if (invoiceTypes == InvoiceTypes.Purchase)
                        {
                         automaticVoucher = ServiceLocator.Current.GetInstance<IAddPurchesInvoiceVoucher>();
                         ((IAddPurchesInvoiceVoucher)automaticVoucher).Execute(
                         entityFuel,
                         receipts,
                         oldInventoryTransaction.Inventory_Warehouse.Code,
                         exchangeRate,
                         secondEntityFuel,
                         inventoryActionNumber,
                         userId
                        );
                        }
                        if (invoiceTypes == InvoiceTypes.PurchaseOperations)
                        {
                            automaticVoucher = ServiceLocator.Current.GetInstance<IAddTransferBarjingInvoiceVoucher>();
                            ((IAddTransferBarjingInvoiceVoucher)automaticVoucher).Execute(
                        entityFuel,
                         receipts,
                         oldInventoryTransaction.Inventory_Warehouse.Code,
                         exchangeRate,
                         secondEntityFuel,
                         inventoryActionNumber,
                         userId
                        );
                        }
                        break;
                    }
                #endregion
                #region FUEL_REPORT_DETAIL_RECEIVE_PRICING
                //case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE_PRICING:
                //    {
                //        //  automaticVoucher = ServiceLocator.Current.GetInstance<IAddTransferBarjingInvoiceVoucher>();
                //        break;
                //    }
                #endregion
                #region FUEL_REPORT_DETAIL_TRANSFER
                case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER:
                    {
                        automaticVoucher = ServiceLocator.Current.GetInstance<IAddSaleTransitionIssueVoucher>();
                        var entityFuel = fuelReportRepository.FindByKey(refNo);
                        //Todo Get Second entity
                        //var secondEntityFuel = orderRepository.FindByKey(secondRefNo);
                        var secondEntityFuel = orderRepository.FindByKey(0);
                        var issues = new List<Issue>();
                        if (isReform)
                        {
                            issues.Add(newInventoryTransaction.CreateDifferentialIssueDataForFinanceArticles(
                                oldInventoryTransaction,
                                entityFuel.VesselInCompany.CompanyId,
                                shardGoodId,
                                goodRepository
                                ));
                        }
                        else
                        {
                            issues = newInventoryTransaction.CreateIssueDataForFinanceArticles(
                                   entityFuel.VesselInCompany.CompanyId, goodRepository);
                        }


                        ((IAddSaleTransitionIssueVoucher)automaticVoucher).Execute(
                       issues,
                       entityFuel,
                       oldInventoryTransaction.Inventory_Warehouse.Code,
                       secondEntityFuel.ToVesselInCompany.Code,
                       inventoryActionNumber,
                       userId
                    
                  );
                        break;
                    }
                #endregion
                #region FUEL_REPORT_DETAIL_TRANSFER_PRICING
                //case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER_PRICING:
                //    {
                //        // automaticVoucher = ServiceLocator.Current.GetInstance<IAddSaleTransitionIssueVoucher>();
                //        break;
                //    }
                #endregion
                #region INVENTORY_INITIATION
                case InventoryOperationReferenceTypes.INVENTORY_INITIATION:
                    {
                        // automaticVoucher = ServiceLocator.Current.GetInstance<IAddSaleTransitionIssueVoucher>();
                        break;
                    }
                #endregion
                #region SCRAP_ISSUE
                case InventoryOperationReferenceTypes.SCRAP_ISSUE:
                    {
                        // automaticVoucher = ServiceLocator.Current.GetInstance<IAdd>();
                        break;
                    }
                #endregion
            }

          
        }
    }
}
