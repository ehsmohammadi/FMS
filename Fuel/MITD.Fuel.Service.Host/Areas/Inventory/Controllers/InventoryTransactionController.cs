using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Inventory.Controllers
{
    public class InventoryTransactionController : ApiController
    {
        #region props

        private IInventoryTransactionFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public InventoryTransactionController(IInventoryTransactionFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public Inventory_TransactionDto Get(long id)
        {
            var result = this.FacadeService.GetById(id);
            return result;
        }


        public PageResultDto<Inventory_TransactionDto> Get(int pageSize, int pageIndex)
        {
            var result = this.FacadeService.GetPagedData(pageSize, pageIndex);
            return result;
        }

        public PageResultDto<Inventory_TransactionDto> Get(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType, byte? status, decimal? inventoryCode, int pageSize, int pageIndex)
        {
            var result = this.FacadeService.GetPagedDataByFilter(companyId, warehouseId, fromDate, toDate,
                transactionType, status, inventoryCode, pageSize, pageIndex);
            return result;
        }

        [HttpPut]
        [ActionName("TransactionPricing")]
        public void PutTransactionPricing(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            this.FacadeService.PricingTransaction(companyId, warehouseId,
                fromDate, toDate, transactionType);
        }

        [HttpPut]
        [ActionName("RegisterVoucher")]
        public void PutRegisterVoucher(long? companyId, long? warehouseId,
            DateTime? fromDate, DateTime? toDate, byte? transactionType)
        {
            this.FacadeService.CreateVoucherForTransactions(companyId, warehouseId,
                fromDate, toDate, transactionType);
        }

        #endregion
    }
}
