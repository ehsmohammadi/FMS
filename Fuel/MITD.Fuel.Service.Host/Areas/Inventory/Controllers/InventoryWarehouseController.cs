using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Inventory.Controllers
{
    public class InventoryWarehouseController : ApiController
    {
        #region props

        private IInventoryCompanyFacadeService FacadeService { get; set; }

        #endregion

        #region ctor

        public InventoryWarehouseController()
        {
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IInventoryCompanyFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public InventoryWarehouseController(IInventoryCompanyFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }

        #endregion

        #region methods

        public List<Inventory_WarehouseDto> Get(long id)
        {
            var data = this.FacadeService.GetWarehouse(id);
            return data;
        }

        public Inventory_WarehouseDto Get(long id, long warehouseId)
        {
            var data = this.FacadeService.Get(id, warehouseId);
            return data;
        }


        #endregion
    }
}
