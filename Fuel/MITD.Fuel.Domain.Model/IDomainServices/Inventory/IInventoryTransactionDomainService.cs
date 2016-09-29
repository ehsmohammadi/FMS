using System;
using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Domain.Repository;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IInventoryTransactionDomainService : IDomainService
    {
        Inventory_Transaction Get(long transactionId);

        PageResult<Inventory_Transaction> GetPagedData(int pageSize, int pageIndex);

        PageResult<Inventory_Transaction> GetPagedDataByFilter(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, byte? status, decimal? inventoryCode, int pageSize, int pageIndex);

        Inventory_TransactionItem GetDetailDataByFilter(long transactionId, long transactionDetailId);

        PageResult<Inventory_TransactionItem> GetPagedDetailDataByFilter(long transactionId, int pageSize,
            int pageIndex);

        Inventory_TransactionItemPrice GetDetailPriceDataByFilter(long transactionId, long transactionDetailId,long TransactionDetailPriceId);

        PageResult<Inventory_TransactionItemPrice> GetPagedDetailPriceDataByFilter(
            long transactionId, long transactionDetailId, int pageSize, int pageIndex);

        List<Inventory_Transaction> GetNotCompletePricedTransactions(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);

        List<Inventory_Transaction> GetNotVoucherdTransactions(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);

        void PricingTransaction(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType);

        void CreateVoucherForTransactions(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, long userId);

        void CreateVoucherForTransaction(Inventory_Transaction inventoryTransaction, long userId);
    }
}