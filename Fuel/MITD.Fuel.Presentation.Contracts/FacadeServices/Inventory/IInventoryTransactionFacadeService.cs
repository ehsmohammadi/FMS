using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IInventoryTransactionFacadeService : IFacadeService
    {
        Inventory_TransactionDto GetById(long id);
        PageResultDto<Inventory_TransactionDto> GetPagedData(int pageSize, int pageIndex);

        PageResultDto<Inventory_TransactionDto> GetPagedDataByFilter(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, byte? status, decimal? inventoryCode, int pageSize, int pageIndex);

        Inventory_TransactionDetailDto GetTransactionDetailDataByFilter(long transactionId, long transactionDetailId);

        PageResultDto<Inventory_TransactionDetailDto> GetPagedTransactionDetailDataByFilter(long transactionId,
            int pageSize,
            int pageIndex);

        Inventory_TransactionDetailPriceDto GetTransactionDetailPriceDataByFilter(long transactionId, long transactionDetailId,long transactionDetailPriceId);

        PageResultDto<Inventory_TransactionDetailPriceDto> GetPagedTransactionDetailPriceDataByFilter(long transactionId,
            long transactionDetailId, int pageSize, int pageIndex);

        List<Inventory_TransactionDto> GetNotCompletePricedTransaction(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);

        List<Inventory_TransactionDto> GetNotVoucherdTransaction(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);

        void PricingTransaction(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);

        void CreateVoucherForTransactions(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType);

    }
}