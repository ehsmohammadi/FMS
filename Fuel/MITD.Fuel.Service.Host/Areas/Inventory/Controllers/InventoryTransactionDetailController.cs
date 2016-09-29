using System;
using System.Web.Http;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;


namespace MITD.Fuel.Service.Host.Areas.Inventory.Controllers
{
    public class InventoryTransactionDetailController : ApiController
    {
        #region props

        private IInventoryTransactionFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public InventoryTransactionDetailController(IInventoryTransactionFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods


        public Inventory_TransactionDetailDto Get(long id, long detailId)
        {
            var result = this.FacadeService.GetTransactionDetailDataByFilter(id, detailId);
            return result;
        }

        public PageResultDto<Inventory_TransactionDetailDto> Get(long id,
            int pageSize,
            int pageIndex)
        {
            var result = this.FacadeService.GetPagedTransactionDetailDataByFilter(id, pageSize, pageIndex);
            return result;
        }

        #endregion
    }
}
