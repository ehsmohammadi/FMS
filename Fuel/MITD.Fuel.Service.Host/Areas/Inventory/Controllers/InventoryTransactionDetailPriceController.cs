using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Inventory.Controllers
{
    public class InventoryTransactionDetailPriceController : ApiController
    {
        #region props

        private IInventoryTransactionFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public InventoryTransactionDetailPriceController(IInventoryTransactionFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public Inventory_TransactionDetailPriceDto Get(long id, long detailId,
            long detailPriceId)
        {
            var result = this.FacadeService.GetTransactionDetailPriceDataByFilter(id, detailId,
                detailPriceId);
            return result;
        }

        public PageResultDto<Inventory_TransactionDetailPriceDto> Get(long id, long detailId,
            int pageSize,
            int pageIndex)
        {
            var result = this.FacadeService.GetPagedTransactionDetailPriceDataByFilter(id,
                detailId, pageSize, pageIndex);
            return result;
        }

        #endregion
    }
}
