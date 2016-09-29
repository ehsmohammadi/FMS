using System;
using MITD.Presentation;
using MITD.Presentation.Contracts;
using MITD.Fuel.Presentation.Contracts.DTOs;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IInventoryTransactionServiceWrapper : IServiceWrapper
    {
        void GetById(Action<Inventory_TransactionDto, Exception> action, long id);

        void GetPagedTransactionData(Action<PageResultDto<Inventory_TransactionDto>, Exception> action, int pageSize,
            int pageIndex);

        void GetPagedTransactionDataByFilter(Action<PageResultDto<Inventory_TransactionDto>, Exception> action, long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, byte? status, decimal? inventoryCode, int pageSize, int pageIndex);

        void GetTransactionDetailDataByFilter(Action<Inventory_TransactionDetailDto, Exception> action,
            long transactionId, long transactionDetailId);

        void GetPagedTransactionDetailDataByFilter(
            Action<PageResultDto<Inventory_TransactionDetailDto>, Exception> action, long transactionId, int pageSize,
            int pageIndex);

        void GetTransactionDetailPriceDataByFilter(Action<Inventory_TransactionDetailPriceDto, Exception> action,
            long transactionId, long transactionDetailId, long transactionDetailPriceId);

        void GetPagedTransactionDetailPriceDataByFilter(
            Action<PageResultDto<Inventory_TransactionDetailPriceDto>, Exception> action, long transactionId, long transactionDetailId,
            int pageSize, int pageIndex);

        void PricingTransaction(Action<string, Exception> action, long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);

        void CreateVoucherForTransactions(Action<string, Exception> action, long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);
    }
}