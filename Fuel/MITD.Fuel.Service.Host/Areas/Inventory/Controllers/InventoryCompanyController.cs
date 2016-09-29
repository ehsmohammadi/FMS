using System;
using System.Collections.Generic;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Inventory.Controllers
{
    public class InventoryCompanyController : ApiController
    {
        #region props
        private IInventoryCompanyFacadeService FacadeService { get; set; }
        #endregion

        #region ctor

        public InventoryCompanyController()
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
        public InventoryCompanyController(IInventoryCompanyFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }
        #endregion

        #region methods

        public Inventory_CompanyDto Get(long id)
        {
            var data = this.FacadeService.Get(id);

            return data;
        }

        public List<Inventory_CompanyDto> Get(bool filterByUser)
        {
            List<Inventory_CompanyDto> result = 
                filterByUser
                    ? this.FacadeService.GetByCurrentUser()
                    : this.FacadeService.Get();
            return result;
        }

        #endregion
    }
}
