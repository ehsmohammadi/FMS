using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Core;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.ACL.Contracts.AutomaticVoucher;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.DomainObjects.InvoiceAggreate.Enums;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.Extensions;
using MITD.Fuel.Domain.Model.Factories;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Domain.Model.Specifications;
using MITD.Fuel.Integration.Inventory.Infrastructure;

namespace MITD.Fuel.Domain.Model.DomainServices
{
    public class InventoryTransactionDomainService : IInventoryTransactionDomainService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IRepository<Inventory_TransactionItem> transactionDetailRepository;
        private readonly IRepository<Inventory_TransactionItemPrice> transactionDetailPriceRepository;
        private readonly IAddCharterInStartReceiptVoucher _addCharterInStartReceiptVoucher;
        private readonly IAddCharterOutStartIssueVoucher _addCharterOutStartIssueVoucher;
        private readonly IAddCharterInEndIssueVoucher _addCharterInEndIssueVoucher;
        private readonly IAddCharterOutEndReceiptVoucher _addCharterOutEndReceiptVoucher;
        private readonly IAddConsumptionIssueVoucher _addConsumptionIssueVoucher;
        private readonly IAddPurchesInvoiceVoucher _addPurchaseInvoiceVoucher;
        private readonly IAddTransferBarjingInvoiceVoucher addTransferBarjingInvoiceVoucher;
        private readonly IInventoryOperationManager inventoryOperationManager;
        private readonly ICharterInRepository _charterInRepository;
        private readonly ICharterOutRepository _charterOutRepository;
        private readonly IFuelReportRepository _fuelReportRepository;
        private readonly IRepository<FuelReportDetail> _fuelReportDetailRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IScrapRepository _scrapRepository;
        private readonly ICurrencyDomainService _currencyDomainService;
        private readonly IGoodRepository goodRepository;
        private readonly IVoyageDomainService voyageDomainService;
        private readonly ICharterInDomainService charterInDomainService;
        private readonly ICharterOutDomainService charterOutDomainService;
        private readonly IAddSaleTransitionIssueVoucher addSaleTransitionIssueVoucher;

        private readonly IAddCharterInEndBackReciptVoucher addCharterInEndBackReceiptVoucher;
        private readonly IAddCharterInEndConsumptionIssueVoucher addCharterInEndConsumptionIssueVoucher;

        private readonly IAddCharterOutStartBackReceiptVoucher addCharterOutStartBackReceiptVoucher;
        private readonly IAddCharterOutStartConsumptionIssueVoucher addCharterOutStartConsumptionIssueVoucher;

        private readonly IAddPlusCorrectionReceiptVoucher addPlusCorrectionReceiptVoucher;
        private readonly IAddMinusCorrectionReceiptVoucher addMinusCorrectionReceiptVoucher;


        public InventoryTransactionDomainService(ITransactionRepository transactionRepository,
            IRepository<Inventory_TransactionItem> transactionDetailRepository,
            IRepository<Inventory_TransactionItemPrice> transactionDetailPriceRepository,
            IAddCharterInStartReceiptVoucher addCharterInStartReceiptVoucher,
            IAddCharterOutStartIssueVoucher addCharterOutStartIssueVoucher,
            IAddCharterInEndIssueVoucher addCharterInEndIssueVoucher,
            IAddCharterOutEndReceiptVoucher addCharterOutEndReceiptVoucher,
            IAddConsumptionIssueVoucher addConsumptionIssueVoucher,
            IAddPurchesInvoiceVoucher addPurchaseInvoiceVoucher,
            IInventoryOperationManager inventoryOperationManager,
            ICharterInRepository charterInRepository,
            ICharterOutRepository charterOutRepository,
            IFuelReportRepository fuelReportRepository,
            IRepository<FuelReportDetail> fuelReportDetailRepository,
            IInvoiceRepository invoiceRepository,
            IScrapRepository scrapRepository,
            ICurrencyDomainService currencyDomainService,
            IGoodRepository goodRepository, IVoyageDomainService voyageDomainService, ICharterInDomainService charterInDomainService, ICharterOutDomainService charterOutDomainService, IAddCharterInEndBackReciptVoucher addCharterInEndBackReceiptVoucher, IAddCharterInEndConsumptionIssueVoucher addCharterInEndConsumptionIssueVoucher, IAddCharterOutStartBackReceiptVoucher addCharterOutStartBackReceiptVoucher, IAddCharterOutStartConsumptionIssueVoucher addCharterOutStartConsumptionIssueVoucher, IAddSaleTransitionIssueVoucher addSaleTransitionIssueVoucher, IAddTransferBarjingInvoiceVoucher addTransferBarjingInvoiceVoucher, IAddPlusCorrectionReceiptVoucher addPlusCorrectionReceiptVoucher, IAddMinusCorrectionReceiptVoucher addMinusCorrectionReceiptVoucher)
        {
            this.transactionRepository = transactionRepository;
            this.transactionDetailRepository = transactionDetailRepository;
            this.transactionDetailPriceRepository = transactionDetailPriceRepository;
            _addCharterInStartReceiptVoucher = addCharterInStartReceiptVoucher;
            _addCharterOutStartIssueVoucher = addCharterOutStartIssueVoucher;
            _addCharterInEndIssueVoucher = addCharterInEndIssueVoucher;
            _addCharterOutEndReceiptVoucher = addCharterOutEndReceiptVoucher;
            _addConsumptionIssueVoucher = addConsumptionIssueVoucher;
            _addPurchaseInvoiceVoucher = addPurchaseInvoiceVoucher;
            this.inventoryOperationManager = inventoryOperationManager;
            _charterInRepository = charterInRepository;
            _charterOutRepository = charterOutRepository;
            _fuelReportRepository = fuelReportRepository;
            _fuelReportDetailRepository = fuelReportDetailRepository;
            _invoiceRepository = invoiceRepository;
            _scrapRepository = scrapRepository;
            _currencyDomainService = currencyDomainService;
            this.goodRepository = goodRepository;
            this.voyageDomainService = voyageDomainService;
            this.charterInDomainService = charterInDomainService;
            this.charterOutDomainService = charterOutDomainService;
            this.addCharterInEndBackReceiptVoucher = addCharterInEndBackReceiptVoucher;
            this.addCharterInEndConsumptionIssueVoucher = addCharterInEndConsumptionIssueVoucher;
            this.addCharterOutStartBackReceiptVoucher = addCharterOutStartBackReceiptVoucher;
            this.addCharterOutStartConsumptionIssueVoucher = addCharterOutStartConsumptionIssueVoucher;

            this.addSaleTransitionIssueVoucher = addSaleTransitionIssueVoucher;
            this.addTransferBarjingInvoiceVoucher = addTransferBarjingInvoiceVoucher;
            this.addPlusCorrectionReceiptVoucher = addPlusCorrectionReceiptVoucher;
            this.addMinusCorrectionReceiptVoucher = addMinusCorrectionReceiptVoucher;
        }

        //================================================================================

        public Inventory_Transaction Get(long transactionId)
        {
            var transaction = transactionRepository.Single(e => e.Id == transactionId);

            if (transaction == null)
                throw new ObjectNotFound("transaction", transactionId);

            return transaction;
        }

        //================================================================================

        public PageResult<Inventory_Transaction> GetPagedData(int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Inventory_Transaction>()
                .Include(t => t.Inventory_StoreType)
                .Include(t => t.Inventory_TransactionItem)
                .Include(t => t.Inventory_Warehouse)
                .Include(t => t.Inventory_TimeBucket)
                .OrderBy(t => t.RegistrationDate)
                .WithPaging(pageSize, pageNumber);

            transactionRepository.GetAll(fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public PageResult<Inventory_Transaction> GetPagedDataByFilter(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, byte? status, decimal? inventoryCode, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Inventory_Transaction>()
                .Include(t => t.Inventory_StoreType)
                .Include(t => t.Inventory_TransactionItem)
                .Include(t => t.Inventory_Warehouse)
                .Include(t => t.Inventory_TimeBucket)
                .OrderBy(t => t.Code)
                .WithPaging(pageSize, pageNumber);

            transactionRepository.Find(
                t => (!companyId.HasValue || companyId == 0 || t.Inventory_Warehouse.CompanyId == companyId) &&
                     (!warehouseId.HasValue || warehouseId == 0 || t.WarehouseId == warehouseId) &&
                     (!fromDate.HasValue || t.RegistrationDate >= fromDate) &&
                     (!toDate.HasValue || t.RegistrationDate <= toDate) &&
                     (!transactionType.HasValue || transactionType == 0 || t.Action == transactionType) &&
                     (!status.HasValue || status == 0 || t.Status == status) &&
                     (!inventoryCode.HasValue || t.Code == inventoryCode)
                , fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public Inventory_TransactionItem GetDetailDataByFilter(long transactionId, long transactionDetailId)
        {
            var transactionDetail =
                transactionDetailRepository.Single(
                    ti => ti.TransactionId == transactionId && ti.Id == transactionDetailId);

            if (transactionDetail == null)
                throw new ObjectNotFound("transactionDetail", transactionDetailId);

            return transactionDetail;
        }

        //================================================================================

        public PageResult<Inventory_TransactionItem> GetPagedDetailDataByFilter(long transactionId, int pageSize,
            int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Inventory_TransactionItem>()
                .Include(ti => ti.Inventory_Good)
                .Include(ti => ti.Inventory_Transaction)
                .Include(ti => ti.Inventory_TransactionItemPrice)
                .Include(ti => ti.Inventory_Unit)
                .WithPaging(pageSize, pageNumber);

            transactionDetailRepository.Find(ti => ti.TransactionId == transactionId, fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;

        }

        //================================================================================

        public Inventory_TransactionItemPrice GetDetailPriceDataByFilter(long transactionId, long transactionDetailId,
            long TransactionDetailPriceId)
        {
            var transactionDetailPrice =
                transactionDetailPriceRepository.Single(
                    tip =>
                        tip.TransactionId == transactionId && tip.TransactionItemId == transactionDetailId &&
                        tip.Id == TransactionDetailPriceId);

            if (transactionDetailPrice == null)
                throw new ObjectNotFound("transactionDetailPrice", TransactionDetailPriceId);

            return transactionDetailPrice;
        }

        //================================================================================

        public PageResult<Inventory_TransactionItemPrice> GetPagedDetailPriceDataByFilter(
            long transactionId, long transactionDetailId, int pageSize, int pageIndex)
        {
            var pageNumber = pageIndex + 1;

            var fetchStrategy = new ListFetchStrategy<Inventory_TransactionItemPrice>()
                .Include(tip => tip.Inventory_TransactionItem)
                .Include(tip => tip.Inventory_Unit_PriceUnit)
                .Include(tip => tip.Inventory_Unit_QuantityUnit)
                .WithPaging(pageSize, pageNumber);

            transactionDetailPriceRepository.Find(
                tip => tip.TransactionId == transactionId && tip.TransactionItemId == transactionDetailId, fetchStrategy);

            return fetchStrategy.PageCriteria.PageResult;
        }

        //================================================================================

        public List<Inventory_Transaction> GetNotCompletePricedTransactions(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {

            var fetchStrategy = new ListFetchStrategy<Inventory_Transaction>()
                .Include(tip => tip.Inventory_TransactionItem)
                .Include(tip => tip.Inventory_Warehouse)
                .Include(tip => tip.Inventory_TransactionItem.SelectMany(t => t.Inventory_TransactionItemPrice))
                .OrderBy(t => t.Code);

            var result = transactionRepository.Find(
                //isTransactionCompletePriced.Predicate.Not()
                //.And
                (t => (!companyId.HasValue || companyId == 0 || t.Inventory_Warehouse.CompanyId == companyId) &&
                      (!warehouseId.HasValue || warehouseId == 0 || t.WarehouseId == warehouseId) &&
                      (!fromDate.HasValue || t.RegistrationDate >= fromDate) &&
                      (!toDate.HasValue || t.RegistrationDate <= toDate) &&
                      (!transactionType.HasValue || transactionType == 0 || t.Action == transactionType) &&
                      (t.Status == (byte)TransactionState.JustRegistered ||
                       t.Status == (byte)TransactionState.PartialPriced)
                    ), fetchStrategy);

            return result.ToList();
        }

        //================================================================================

        public List<Inventory_Transaction> GetNotVoucherdTransactions(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            var fetchStrategy = new ListFetchStrategy<Inventory_Transaction>()
                .Include(t => t.Inventory_TransactionItem)
                .Include(t => t.Inventory_TransactionItem.SelectMany(ti => ti.Inventory_TransactionItemPrice))
                .Include(t => t.Inventory_Warehouse)
                .OrderBy(t => t.Code);

            var result = transactionRepository.Find(
                //isTransactionVoucherd.Predicate.Not()
                //.And
                (t => (!companyId.HasValue || companyId == 0 || t.Inventory_Warehouse.CompanyId == companyId) &&
                      (!warehouseId.HasValue || warehouseId == 0 || t.WarehouseId == warehouseId) &&
                      (!fromDate.HasValue || t.RegistrationDate >= fromDate) &&
                      (!toDate.HasValue || t.RegistrationDate <= toDate) &&
                      (!transactionType.HasValue || transactionType == 0 || t.Action == transactionType) &&
                      t.Status == (byte)TransactionState.FullPriced), fetchStrategy);

            return result.ToList();
        }

        //================================================================================

        private List<Inventory_Transaction> getNotPricedTransactions(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate)
        {
            var fetchStrategy = new ListFetchStrategy<Inventory_Transaction>()
                .Include(t => t.Inventory_TransactionItem)
                .Include(t => t.Inventory_TransactionItem.SelectMany(ti => ti.Inventory_TransactionItemPrice))
                .Include(t => t.Inventory_Warehouse)
                .OrderBy(t => t.Code);

            var result = transactionRepository.Find(
                (t => (!companyId.HasValue || companyId == 0 || t.Inventory_Warehouse.CompanyId == companyId) &&
                      (!warehouseId.HasValue || warehouseId == 0 || t.WarehouseId == warehouseId) &&
                      (!fromDate.HasValue || t.RegistrationDate >= fromDate) &&
                      (!toDate.HasValue || t.RegistrationDate <= toDate) &&
                      (t.PricingReferenceId.HasValue || (t.Action == (byte)TransactionType.Issue)) &&
                      (t.Status == (byte)TransactionState.JustRegistered || t.Status == (byte)TransactionState.PartialPriced)), fetchStrategy);

            return result.OrderBy(t => t.RegistrationDate).ToList();
        }

        //================================================================================

        public void PricingTransaction(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            if (!toDate.HasValue)
                throw new BusinessRuleException("", "'To Date' must be selected.");

            checkIsThereAnyFuelReportWithCorrectionButNotRevisedByFinance(companyId, warehouseId, toDate.Value);

            string message;

            inventoryOperationManager.PriceAllSuspendedTransactions(companyId, warehouseId, fromDate, toDate, (int)FuelUserDomainService.GetCurrentUserId(), out message);
            //inventoryOperationManager.PriceAllSuspendedIssuedItems(companyId, warehouseId, fromDate, toDate, (int)FuelUserDomainService.GetCurrentUserId(), out message);
            //inventoryOperationManager.PriceAllSuspendedTransactionItemsUsingReference(companyId, warehouseId, fromDate, toDate, (int)FuelUserDomainService.GetCurrentUserId(), (TransactionType?)transactionType, out message);
        }

        private void checkIsThereAnyFuelReportWithCorrectionButNotRevisedByFinance(long? companyId, long? warehouseId, DateTime toDate)
        {
            var warehouseRepository = ServiceLocator.Current.GetInstance<IRepository<Inventory_Warehouse>>();

            var warehouseCode = null as string;
            if (warehouseId.HasValue)
                warehouseCode = warehouseRepository.Single(w => w.Id == warehouseId.Value).Code;

            var fuelReportDomainService = ServiceLocator.Current.GetInstance<IFuelReportDomainService>();
            var fuelReportsWithCorrectionButNotRevised = fuelReportDomainService.GetFuelReportsWithCorrectionButNotRevisedByFinance(companyId, warehouseCode, toDate);

            if (fuelReportsWithCorrectionButNotRevised.Count > 0)
                throw new BusinessRuleException("",
                    "There are some Fuel Reports with Correction, which are not manipulated by relevant financial operator.\nThe Ids are:\n\n" +
                    string.Join("\n", fuelReportsWithCorrectionButNotRevised));
        }


        public void CreateVoucherForTransactions(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, long userId)
        {
            if (!toDate.HasValue)
                throw new BusinessRuleException("", "'To Date' must be selected.");

            checkIsThereAnyFuelReportWithCorrectionButNotRevisedByFinance(companyId, warehouseId, toDate.Value);

            List<Inventory_Transaction> transactionNotVoucherd = GetNotVoucherdTransactions(companyId, warehouseId,
                fromDate, toDate, transactionType);

            foreach (var inventoryTransaction in transactionNotVoucherd)
            {
                this.CreateVoucherForTransaction(inventoryTransaction, userId);
            }
        }

        public void CreateVoucherForTransaction(Inventory_Transaction inventoryTransaction, long userId)
        {
                if (inventoryTransaction.Status.Value == (byte)MITD.Fuel.Domain.Model.Enums.TransactionState.FullPriced)
                {
                    //var actionNumber = string.Format("{0}/{1}", (TransactionType)inventoryTransaction.Action,
                    //    inventoryTransaction.Code);

                    var actionNumber = (string)null;

                    switch (inventoryTransaction.ReferenceType)
                    {
                        case InventoryOperationReferenceTypes.CHARTER_IN_START_RECEIPT:

                            long charterInStartId = long.Parse(inventoryTransaction.ReferenceNo);

                            var charterInStart =
                                _charterInRepository.GetQueryInclude().Single(ci => ci.Id == charterInStartId);

                        actionNumber = inventoryTransaction.GetActionNumber();
                            //var charterInStartArticleToFinance = new List<Receipt>();

                            //foreach (var transactionItem in inventoryTransaction.Inventory_TransactionItem)
                            //{

                            //    var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == charterInStart.ChartererId);

                            //    charterInStartArticleToFinance.AddRange(
                            //        transactionItem.Inventory_TransactionItemPrice.Select(
                            //            tip =>
                            //                new Receipt(0, good.Id,
                            //                    tip.QuantityAmount.Value, tip.Fee.Value,
                            //                    tip.FeeInMainCurrency.Value/tip.Fee.Value,
                            //                    tip.Inventory_Unit_QuantityUnit.Name,
                            //                    transactionItem.Inventory_Good.Name,
                            //                    inventoryTransaction.RegistrationDate.Value, tip.PriceUnitId,
                            //                    tip.Inventory_Unit_PriceUnit.Name)).ToList());
                            //}

                            var charterInStartVoyage = charterInDomainService.GetVoyageCharterInStart(charterInStart.ChartererId.Value, charterInStart.VesselInCompanyId.Value, charterInStart.ActionDate);

                            var charterInStartReceiptDataForFinance = inventoryTransaction.CreateReceiptDataForFinanceArticles(charterInStart.ChartererId.Value, goodRepository);

                            try
                            {
                            _addCharterInStartReceiptVoucher.Execute(charterInStart, charterInStartReceiptDataForFinance, inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterInStartVoyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }

                            break;

                        case InventoryOperationReferenceTypes.CHARTER_IN_END_ISSUE:

                            long charterInEndId = long.Parse(inventoryTransaction.ReferenceNo);

                            var charterInEnd = _charterInRepository.GetQueryInclude().Single(ci => ci.Id == charterInEndId);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            //var charterInEndArticleToFinance = new List<Issue>();

                            //foreach (var transactionItem in inventoryTransaction.Inventory_TransactionItem)
                            //{
                            //    var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == charterInEnd.ChartererId);

                            //    charterInEndArticleToFinance.AddRange(
                            //        transactionItem.Inventory_TransactionItemPrice.Select(
                            //            tip =>
                            //                new Issue(0, good.Id,
                            //                    tip.QuantityAmount.Value, tip.Fee.Value,
                            //                    tip.FeeInMainCurrency.Value/tip.Fee.Value,
                            //                    tip.Inventory_Unit_QuantityUnit.Name,
                            //                    transactionItem.Inventory_Good.Name,
                            //                    inventoryTransaction.RegistrationDate.Value, tip.PriceUnitId,
                            //                    tip.Inventory_Unit_PriceUnit.Name)).ToList());
                            //}

                            try
                            {
                                var charterInEndVoyage = charterInDomainService.GetVoyageCharterInEnd(charterInEnd.ChartererId.Value, charterInEnd.VesselInCompanyId.Value, charterInEnd.ActionDate);

                                var charterInEndIssueDataForFinance = inventoryTransaction.CreateIssueDataForFinanceArticles(charterInEnd.ChartererId.Value, goodRepository);


                            _addCharterInEndIssueVoucher.Execute(charterInEnd, charterInEndIssueDataForFinance, inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterInEndVoyage.VoyageNumber, "");
                            }
                            catch
                            { }
                            break;

                        case InventoryOperationReferenceTypes.CHARTER_OUT_END_RECEIPT:

                            long charterOutEndId = long.Parse(inventoryTransaction.ReferenceNo);

                            var charterOutEnd = _charterOutRepository.GetQueryInclude().Single(ci => ci.Id == charterOutEndId);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            //var charterOutEndArticleToFinance = new List<Receipt>();

                            //foreach (var transactionItem in inventoryTransaction.Inventory_TransactionItem)
                            //{
                            //    var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == charterOutEnd.OwnerId);

                            //    charterOutEndArticleToFinance.AddRange(
                            //        transactionItem.Inventory_TransactionItemPrice.Select(
                            //            tip =>
                            //                new Receipt(0, good.Id,
                            //                    tip.QuantityAmount.Value, tip.Fee.Value,
                            //                    tip.FeeInMainCurrency.Value/tip.Fee.Value,
                            //                    tip.Inventory_Unit_QuantityUnit.Name,
                            //                    transactionItem.Inventory_Good.Name,
                            //                    inventoryTransaction.RegistrationDate.Value, tip.PriceUnitId,
                            //                    tip.Inventory_Unit_PriceUnit.Name)).ToList());
                            //}

                            try
                            {
                                var charterOutEndVoyage = charterOutDomainService.GetVoyageCharterInEnd(charterOutEnd.OwnerId.Value, charterOutEnd.VesselInCompanyId.Value, charterOutEnd.ActionDate);

                                var charterOutEndReceiptDataForFinance = inventoryTransaction.CreateReceiptDataForFinanceArticles(charterOutEnd.OwnerId.Value, goodRepository);


                            _addCharterOutEndReceiptVoucher.Execute(charterOutEnd, charterOutEndReceiptDataForFinance, inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterOutEndVoyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }

                            break;

                        case InventoryOperationReferenceTypes.CHARTER_OUT_START_ISSUE:

                            long charterOutStartId = long.Parse(inventoryTransaction.ReferenceNo);

                            var charterOutStart =
                                _charterOutRepository.GetQueryInclude().Single(ci => ci.Id == charterOutStartId);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            //var charterOutStartArticleToFinance = new List<Issue>();

                            //foreach (var transactionItem in inventoryTransaction.Inventory_TransactionItem)
                            //{
                            //    var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == charterOutStart.OwnerId);

                            //    charterOutStartArticleToFinance.AddRange(
                            //        transactionItem.Inventory_TransactionItemPrice.Select(
                            //            tip =>
                            //                new Issue(0, good.Id,
                            //                    tip.QuantityAmount.Value, tip.Fee.Value,
                            //                    tip.FeeInMainCurrency.Value/tip.Fee.Value,
                            //                    tip.Inventory_Unit_QuantityUnit.Name,
                            //                    transactionItem.Inventory_Good.Name,
                            //                    inventoryTransaction.RegistrationDate.Value, tip.PriceUnitId,
                            //                    tip.Inventory_Unit_PriceUnit.Name)).ToList());
                            //}

                            try
                            {
                                var charterOutStartVoyage = charterOutDomainService.GetVoyageCharterInEnd(charterOutStart.OwnerId.Value, charterOutStart.VesselInCompanyId.Value, charterOutStart.ActionDate);

                                var charterOutStartIssueDataForFinance = inventoryTransaction.CreateIssueDataForFinanceArticles(charterOutStart.OwnerId.Value, goodRepository);


                            _addCharterOutStartIssueVoucher.Execute(charterOutStart, charterOutStartIssueDataForFinance, inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterOutStartVoyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }

                            break;

                        case InventoryOperationReferenceTypes.EOV_EOM_EOY_FUEL_REPORT_CONSUMPTION:

                            long EOV_EOM_EOY_FuelReportId = long.Parse(inventoryTransaction.ReferenceNo);

                            var EOV_EOM_EOY_FuelReport =
                                _fuelReportRepository.Single(ci => ci.Id == EOV_EOM_EOY_FuelReportId);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            //var EOV_EOM_EOY_Fuel_ReportArticleToFinance = new List<Issue>();

                            //foreach (var transactionItem in inventoryTransaction.Inventory_TransactionItem)
                            //{
                            //    var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == EOV_EOM_EOY_Fuel_Report.VesselInCompany.CompanyId);

                            //    EOV_EOM_EOY_Fuel_ReportArticleToFinance.AddRange(
                            //        transactionItem.Inventory_TransactionItemPrice.Select(
                            //            tip =>
                            //                new Issue(0, good.Id,
                            //                    tip.QuantityAmount.Value, tip.Fee.Value,
                            //                    tip.FeeInMainCurrency.Value/tip.Fee.Value,
                            //                    tip.Inventory_Unit_QuantityUnit.Name,
                            //                    transactionItem.Inventory_Good.Name,
                            //                    inventoryTransaction.RegistrationDate.Value, tip.PriceUnitId,
                            //                    tip.Inventory_Unit_PriceUnit.Name)).ToList());
                            //}


                            var EOV_EOM_EOY_FuelReportIssueDataForFinance = inventoryTransaction.CreateIssueDataForFinanceArticles(EOV_EOM_EOY_FuelReport.VesselInCompany.CompanyId, goodRepository);

                            try
                            {
                            _addConsumptionIssueVoucher.Execute(EOV_EOM_EOY_FuelReportIssueDataForFinance, EOV_EOM_EOY_FuelReport, inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId);
                            }
                            catch
                            {
                            }
                            break;

                        case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_RECEIVE:

                            long fuel_Report_Detail_ReceiveId = long.Parse(inventoryTransaction.ReferenceNo);

                            var fuel_Report_Detail_Receive = _fuelReportDetailRepository.Single(ci => ci.Id == fuel_Report_Detail_ReceiveId);

                            foreach (var transactionItem in inventoryTransaction.Inventory_TransactionItem)
                            {
                                foreach (var inventoryTransactionItemPrice in transactionItem.Inventory_TransactionItemPrice)
                                {
                                    var fuelReportDetailReceivePricingArticleToFinance = new List<Receipt>();

                                    var good = goodRepository.Single(g => g.SharedGoodId == transactionItem.GoodId && g.CompanyId == fuel_Report_Detail_Receive.FuelReport.VesselInCompany.CompanyId);


                                    fuelReportDetailReceivePricingArticleToFinance.Add(
                                        new Receipt(0, good.Id,
                                            inventoryTransactionItemPrice.QuantityAmount.Value,
                                            inventoryTransactionItemPrice.Fee.Value,
                                            inventoryTransactionItemPrice.FeeInMainCurrency.Value /
                                            inventoryTransactionItemPrice.Fee.Value,
                                            inventoryTransactionItemPrice.Inventory_Unit_QuantityUnit.Name,
                                            transactionItem.Inventory_Good.Name,
                                            inventoryTransaction.RegistrationDate.Value,
                                            inventoryTransactionItemPrice.PriceUnitId,
                                            inventoryTransactionItemPrice.Inventory_Unit_PriceUnit.Name, transactionItem.Id));

                                    long fuelReportDetailId;
                                    long[] invoiceItemIds;

                                    inventoryOperationManager.TryGetFuelPurchasePricingReferences(
                                        inventoryTransactionItemPrice.Id, out fuelReportDetailId, out invoiceItemIds);

                                    var fuelReport = _fuelReportRepository.Single(fr => fr.FuelReportDetails.Any(frd => frd.Id == fuelReportDetailId));

                                    var invoices = _invoiceRepository.Find(i => i.InvoiceItems.Any(it => invoiceItemIds.Contains(it.Id)));

                                actionNumber = inventoryTransaction.GetActionNumber();

                                    foreach (var invoice in invoices)
                                    {
                                        var exchangeRate =
                                            _currencyDomainService.GetCurrencyToMainCurrencyRate(invoice.CurrencyId,
                                                invoice.InvoiceDate);

                                        if (invoice.InvoiceType == InvoiceTypes.Purchase)
                                        {
                                            try
                                            {
                                                _addPurchaseInvoiceVoucher.Execute(invoice,
                                                                                   fuelReportDetailReceivePricingArticleToFinance,
                                                                                   inventoryTransaction.Inventory_Warehouse.Code, exchangeRate, fuelReport, actionNumber, userId);
                                            }
                                            catch (BusinessRuleException businessRuleException)
                                            {
                                                throw;
                                            }
                                            catch { }

                                            //TODO: The vouchers for attachments should be created.
                                        }
                                        else if (invoice.InvoiceType == InvoiceTypes.PurchaseOperations)
                                        {
                                            try
                                            {
                                                addTransferBarjingInvoiceVoucher.Execute(invoice,
                                                    fuelReportDetailReceivePricingArticleToFinance,
                                                    inventoryTransaction.Inventory_Warehouse.Code, exchangeRate, fuelReport, actionNumber, userId);
                                            }
                                            catch (BusinessRuleException businessRuleException)
                                            {
                                                throw;
                                            }
                                            catch { }

                                            //TODO: The vouchers for attachments should be created.
                                        }

                                        //if (orderItemBalance.PairingInvoiceItem != null)
                                        //{
                                        //    if (orderItemBalance.PairingInvoiceItem.Invoice.InvoiceType == InvoiceTypes.Purchase)
                                        //        this.addPurchaseInvoiceVoucher.Execute(orderItemBalance.PairingInvoiceItem.Invoice, receiptDataForFinance, operationTransaction.Inventory_Warehouse.Code, exchangeRate, orderItemBalance.FuelReportDetail.FuelReport, inventoryActionNumber, userId);
                                        //    else if (orderItemBalance.PairingInvoiceItem.Invoice.InvoiceType == InvoiceTypes.Transfer)
                                        //        this.addTransferBarjingInvoiceVoucher.Execute(orderItemBalance.PairingInvoiceItem.Invoice, receiptDataForFinance, operationTransaction.Inventory_Warehouse.Code, exchangeRate, orderItemBalance.FuelReportDetail.FuelReport, inventoryActionNumber, userId);
                                        //}
                                    }

                                    //<A.H - 1393-02-15>
                                    inventoryOperationManager.SetInventoryTransactionStatusForRegisteredVoucher(actionNumber);

                                }
                            }

                            break;

                        case InventoryOperationReferenceTypes.CHARTER_IN_END_INCREMENTAL_CORRECTION:

                            long charterInEndIncrementalCorrectionId = long.Parse(inventoryTransaction.ReferenceNo);

                            var releatedCharterInEndIncrementalCorrection = _charterInRepository.GetQueryInclude().Single(ci => ci.Id == charterInEndIncrementalCorrectionId);

                            var charterInEndIncrementalCorrectionVoyage = charterInDomainService.GetVoyageCharterInEnd(releatedCharterInEndIncrementalCorrection.ChartererId.Value, releatedCharterInEndIncrementalCorrection.VesselInCompanyId.Value, releatedCharterInEndIncrementalCorrection.ActionDate);

                            var charterInEndIncrementalCorrectionReceiptDataForFinance = inventoryTransaction.CreateReceiptDataForFinanceArticles(releatedCharterInEndIncrementalCorrection.ChartererId.Value, goodRepository);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            try
                            {
                                addCharterInEndBackReceiptVoucher.Execute(releatedCharterInEndIncrementalCorrection, charterInEndIncrementalCorrectionReceiptDataForFinance,
                                                    inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterInEndIncrementalCorrectionVoyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }

                            break;

                        case InventoryOperationReferenceTypes.CHARTER_IN_END_DECREMENTAL_CORRECTION:

                            long charterInEndDecrementalCorrectionId = long.Parse(inventoryTransaction.ReferenceNo);

                            var releatedCharterInEndDecrementalCorrection = _charterInRepository.GetQueryInclude()
                                .Single(ci => ci.Id == charterInEndDecrementalCorrectionId);

                            var charterInEndDecrementalCorrectionVoyage = charterInDomainService.GetVoyageCharterInEnd(releatedCharterInEndDecrementalCorrection.ChartererId.Value, releatedCharterInEndDecrementalCorrection.VesselInCompanyId.Value, releatedCharterInEndDecrementalCorrection.ActionDate);

                            var charterInEndDecrementalCorrectionIssueDataForFinance = inventoryTransaction.CreateIssueDataForFinanceArticles(releatedCharterInEndDecrementalCorrection.ChartererId.Value, goodRepository);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            try
                            {
                                this.addCharterInEndConsumptionIssueVoucher.Execute(releatedCharterInEndDecrementalCorrection, charterInEndDecrementalCorrectionIssueDataForFinance,
                                    inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterInEndDecrementalCorrectionVoyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }


                            break;

                        case InventoryOperationReferenceTypes.CHARTER_OUT_START_INCREMENTAL_CORRECTION:

                            long charterOutStartIncrementalCorrectionId = long.Parse(inventoryTransaction.ReferenceNo);

                            var relatedCharterOutStartIncrementalCorrection = _charterOutRepository.GetQueryInclude()
                                .Single(ci => ci.Id == charterOutStartIncrementalCorrectionId);

                            var charterOutStartIncrementalCorrectionVoyage = charterOutDomainService.GetVoyageCharterInEnd(relatedCharterOutStartIncrementalCorrection.OwnerId.Value, relatedCharterOutStartIncrementalCorrection.VesselInCompanyId.Value, relatedCharterOutStartIncrementalCorrection.ActionDate);

                            var charterOutStartIncrementalCorrectionReceiptDataForFinance = inventoryTransaction.CreateReceiptDataForFinanceArticles(relatedCharterOutStartIncrementalCorrection.OwnerId.Value, goodRepository);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            try
                            {
                                this.addCharterOutStartBackReceiptVoucher.Execute(relatedCharterOutStartIncrementalCorrection, charterOutStartIncrementalCorrectionReceiptDataForFinance,
                                                    inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterOutStartIncrementalCorrectionVoyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }

                            break;

                        case InventoryOperationReferenceTypes.CHARTER_OUT_START_DECREMENTAL_CORRECTION:

                            long charterOutStartDecrementalCorrectionId = long.Parse(inventoryTransaction.ReferenceNo);

                            var relatedCharterOutStartDecrementalCorrection = _charterOutRepository.GetQueryInclude()
                                .Single(ci => ci.Id == charterOutStartDecrementalCorrectionId);

                            var charterOutStartDecrementalCorrectionVoyage = charterOutDomainService.GetVoyageCharterInEnd(relatedCharterOutStartDecrementalCorrection.OwnerId.Value, relatedCharterOutStartDecrementalCorrection.VesselInCompanyId.Value, relatedCharterOutStartDecrementalCorrection.ActionDate);

                            var charterOutStartDecrementalCorrectionIssueDataForFinance = inventoryTransaction.CreateIssueDataForFinanceArticles(relatedCharterOutStartDecrementalCorrection.OwnerId.Value, goodRepository);

                        actionNumber = inventoryTransaction.GetActionNumber();

                            try
                            {
                                this.addCharterOutStartConsumptionIssueVoucher.Execute(relatedCharterOutStartDecrementalCorrection, charterOutStartDecrementalCorrectionIssueDataForFinance,
                                                    inventoryTransaction.Inventory_Warehouse.Code, actionNumber, userId, charterOutStartDecrementalCorrectionVoyage.VoyageNumber, "");
                            }
                            catch
                            {
                            }

                            break;

                        case InventoryOperationReferenceTypes.INVENTORY_INITIATION:

                            break;


                        case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER:
                            {
                                var fuelReportDetailTransferId = long.Parse(inventoryTransaction.ReferenceNo);

                                var fuelReportDetailTransfer = _fuelReportDetailRepository.Single(ci => ci.Id == fuelReportDetailTransferId);

                                if (inventoryTransaction.ReferenceType == InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_TRANSFER)
                                {
                                    var fuelReprotDetailTransferIssueDataForFinance = inventoryTransaction.CreateIssueDataForFinanceArticles(fuelReportDetailTransfer.FuelReport.VesselInCompany.CompanyId, goodRepository);

                                    var orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();

                                actionNumber = inventoryTransaction.GetActionNumber();

                                    try
                                    {
                                        if (fuelReportDetailTransfer.TransferType.Value == TransferTypes.TransferSale)
                                        {
                                            var saleTransferOrder = orderRepository.Single(o => o.Id == fuelReportDetailTransfer.TransferReference.ReferenceId);

                                            this.addSaleTransitionIssueVoucher.Execute(fuelReprotDetailTransferIssueDataForFinance,
                                                                                       fuelReportDetailTransfer.FuelReport,
                                                                                       inventoryTransaction.Inventory_Warehouse.Code,
                                                                                       saleTransferOrder.ToVesselInCompany.Code,
                                                                                       actionNumber, userId);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            break;

                        case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_DECREMENTAL_CORRECTION:
                            {
                                var fuelReportDetailId = long.Parse(inventoryTransaction.ReferenceNo);

                                var fuelReportDetail = _fuelReportDetailRepository.Single(ci => ci.Id == fuelReportDetailId);

                                var issueDataForFinance = inventoryTransaction.CreateIssueDataForFinanceArticles(fuelReportDetail.FuelReport.VesselInCompany.CompanyId, goodRepository);

                            actionNumber = inventoryTransaction.GetActionNumber();

                                try
                                {
                                    if (fuelReportDetail.CorrectionType.Value == CorrectionTypes.Minus)
                                    {
                                        this.addMinusCorrectionReceiptVoucher.Execute(fuelReportDetail.FuelReport,
                                                                                      issueDataForFinance,
                                                                                      inventoryTransaction.Inventory_Warehouse.Code,
                                                                                      actionNumber, userId);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            break;

                        case InventoryOperationReferenceTypes.FUEL_REPORT_DETAIL_INCREMENTAL_CORRECTION:
                            {
                                var fuelReportDetailId = long.Parse(inventoryTransaction.ReferenceNo);

                                var fuelReportDetail = _fuelReportDetailRepository.Single(ci => ci.Id == fuelReportDetailId);

                                var incrementalCorrectionReceiptDataForFinance = inventoryTransaction.CreateReceiptDataForFinanceArticles(fuelReportDetail.FuelReport.VesselInCompany.CompanyId, goodRepository);

                            actionNumber = inventoryTransaction.GetActionNumber();

                                try
                                {
                                    if (fuelReportDetail.CorrectionType.Value == CorrectionTypes.Plus)
                                    {
                                        this.addPlusCorrectionReceiptVoucher.Execute(fuelReportDetail.FuelReport,
                                                                                     incrementalCorrectionReceiptDataForFinance,
                                                                                     inventoryTransaction.Inventory_Warehouse.Code,
                                                                                     actionNumber, userId);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            break;

                        //case InventoryOperationReferenceTypes.SCRAP_ISSUE:

                        //    long scrapId = long.Parse(inventoryTransaction.ReferenceNo);

                        //    var scrap =
                        //        _scrapRepository.Single(s => s.Id == scrapId);

                        //    var scrapArticleToFinance = new List<Issue>();

                        //    foreach (var transactionItem in inventoryTransaction.Inventory_TransactionItem)
                        //    {
                        //        scrapArticleToFinance.AddRange(
                        //            transactionItem.Inventory_TransactionItemPrice.Select(
                        //                tip =>
                        //                    new Issue(0, transactionItem.GoodId,
                        //                        tip.QuantityAmount.Value, tip.Fee.Value,
                        //                        tip.FeeInMainCurrency.Value / tip.Fee.Value,
                        //                        tip.Inventory_Unit_QuantityUnit.Name,
                        //                        transactionItem.Inventory_Good.Name,
                        //                        inventoryTransaction.RegistrationDate.Value, tip.PriceUnitId,
                        //                        tip.Inventory_Unit_PriceUnit.Name)).ToList());
                        //    }

                        //    _add.Execute(scrap, scrapArticleToFinance,
                        //        inventoryTransaction.Inventory_Warehouse.Code, actionNumber);

                        //    break;


                    }
            }
        }

        //================================================================================
    }
}