using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.Fuel.Application.Facade.Contracts.Mappers;
using MITD.Fuel.Application.Service.Contracts;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Application.Facade
{
    [Interceptor(typeof (SecurityInterception))]
    public class InventoryTransactionFacadeService : IInventoryTransactionFacadeService
    {
        //private readonly ITransactionApplicationService transactionApplicationService;
        private readonly IInventoryTransactionDomainService transactionDomainService;
        private readonly ITransactionToTransactionDtoMapper transactionDtoMapper;
        private readonly ITransactionDetailToTransactionDetailDtoMapper transactionDetailDtoMapper;
        private readonly ITransactionDetailPriceToTransactionDetailPriceDtoMapper transactionDetailPriceDtoMapper;
        private readonly IFuelUserDomainService fuelUserDomainService;

        public InventoryTransactionFacadeService(
            //ITransactionApplicationService transactionApplicationService,
            IInventoryTransactionDomainService transactionDomainService,
            ITransactionToTransactionDtoMapper transactionDtoMapper,
            ITransactionDetailToTransactionDetailDtoMapper transactionDetailDtoMapper,
            ITransactionDetailPriceToTransactionDetailPriceDtoMapper transactionDetailPriceDtoMapper,
            IFuelUserDomainService fuelUserDomainService)
        {
            //this.transactionApplicationService = transactionApplicationService;
            this.transactionDomainService = transactionDomainService;
            this.transactionDtoMapper = transactionDtoMapper;
            this.transactionDetailDtoMapper = transactionDetailDtoMapper;
            this.transactionDetailPriceDtoMapper = transactionDetailPriceDtoMapper;
            this.fuelUserDomainService = fuelUserDomainService;
        }

        //================================================================================

        public Inventory_TransactionDto GetById(long id)
        {
            var transaction = transactionDomainService.Get(id);

            var transactionDto = transactionDtoMapper.MapToModel(transaction);

            return transactionDto;
        }

        //================================================================================

        public PageResultDto<Inventory_TransactionDto> GetPagedData(int pageSize, int pageIndex)
        {
            var pageResult = transactionDomainService.GetPagedData(pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        public PageResultDto<Inventory_TransactionDto> GetPagedDataByFilter(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, byte? transactionType, byte? status, decimal? inventoryCode, int pageSize, int pageIndex)
        {
            var pageResult = transactionDomainService.GetPagedDataByFilter(companyId, warehouseId, fromDate, toDate,
                transactionType, status, inventoryCode, pageSize, pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        private PageResultDto<Inventory_TransactionDto> mapPageResult(PageResult<Inventory_Transaction> pageResult)
        {
            return new PageResultDto<Inventory_TransactionDto>()
                   {
                       CurrentPage = pageResult.CurrentPage,
                       PageSize = pageResult.PageSize,
                       TotalCount = pageResult.TotalCount,
                       TotalPages = pageResult.TotalPages,
                       Result = transactionDtoMapper.MapToModel(pageResult.Result).ToList()
                   };
        }

        //================================================================================

        private PageResultDto<Inventory_TransactionDetailDto> mapPageResult(
            PageResult<Inventory_TransactionItem> pageResult)
        {
            return new PageResultDto<Inventory_TransactionDetailDto>()
                   {
                       CurrentPage = pageResult.CurrentPage,
                       PageSize = pageResult.PageSize,
                       TotalCount = pageResult.TotalCount,
                       TotalPages = pageResult.TotalPages,
                       Result = transactionDetailDtoMapper.MapToModel(pageResult.Result).ToList()
                   };
        }

        //================================================================================

        private PageResultDto<Inventory_TransactionDetailPriceDto> mapPageResult(
            PageResult<Inventory_TransactionItemPrice> pageResult)
        {
            return new PageResultDto<Inventory_TransactionDetailPriceDto>()
                   {
                       CurrentPage = pageResult.CurrentPage,
                       PageSize = pageResult.PageSize,
                       TotalCount = pageResult.TotalCount,
                       TotalPages = pageResult.TotalPages,
                       Result = transactionDetailPriceDtoMapper.MapToModel(pageResult.Result, true).ToList()
                   };
        }

        ////================================================================================

        //public ScrapDto ScrapVessel(ScrapDto dto)
        //{
        //    var result = scrapApplicationService.ScrapVessel(dto.Vessel.Id, dto.SecondParty.Id, dto.ScrapDate);

        //    return scrapDtoMapper.MapToModel(result, this.setEditProperties);
        //}

        //================================================================================

        public Inventory_TransactionDetailDto GetTransactionDetailDataByFilter(long transactionId, long transactionDetailId)
        {
            var transactionDetail = transactionDomainService.GetDetailDataByFilter(transactionId, transactionDetailId);

            var transactionDetailDto = transactionDetailDtoMapper.MapToModel(transactionDetail);

            return transactionDetailDto;
        }

        //================================================================================

        public PageResultDto<Inventory_TransactionDetailDto> GetPagedTransactionDetailDataByFilter(long transactionId,
            int pageSize,
            int pageIndex)
        {
            var pageResult = transactionDomainService.GetPagedDetailDataByFilter(transactionId, pageSize,
                pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        public Inventory_TransactionDetailPriceDto GetTransactionDetailPriceDataByFilter(long transactionId, long transactionDetailId, long transactionDetailPriceId)
        {
            var transactionDetail = transactionDomainService.GetDetailPriceDataByFilter(transactionId, transactionDetailId,transactionDetailPriceId);

            var transactionDetailDto = transactionDetailPriceDtoMapper.MapToModel(transactionDetail);

            return transactionDetailDto;
        }

        //================================================================================

        public PageResultDto<Inventory_TransactionDetailPriceDto> GetPagedTransactionDetailPriceDataByFilter(
            long transactionId, long transactionDetailId, int pageSize, int pageIndex)
        {
            var pageResult = transactionDomainService.GetPagedDetailPriceDataByFilter(transactionId, transactionDetailId, pageSize,
                pageIndex);

            return mapPageResult(pageResult);
        }

        //================================================================================

        public List<Inventory_TransactionDto> GetNotCompletePricedTransaction(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            var transaction = transactionDomainService.GetNotCompletePricedTransactions(companyId, warehouseId, fromDate,
                toDate, transactionType);

            var transactionDto = transactionDtoMapper.MapToModel(transaction);

            return transactionDto.ToList();
        }

        //================================================================================

        public List<Inventory_TransactionDto> GetNotVoucherdTransaction(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            var transaction = transactionDomainService.GetNotVoucherdTransactions(companyId, warehouseId, fromDate,
                toDate, transactionType);

            var transactionDto = transactionDtoMapper.MapToModel(transaction);

            return transactionDto.ToList();
        }

        //================================================================================

        public void PricingTransaction(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            transactionDomainService.PricingTransaction(companyId, warehouseId,
                fromDate, toDate, transactionType);
        }

        //================================================================================

        public void CreateVoucherForTransactions(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            transactionDomainService.CreateVoucherForTransactions(companyId, warehouseId,
                fromDate, toDate, transactionType, this.fuelUserDomainService.GetCurrentFuelUserId());
        }
    }
}