using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MITD.Core;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Service.Host.Areas.Fuel.Controllers
{
    public class CurrentUserController : ApiController
    {
        #region props
        private IUserFacadeService FacadeService { get; set; }
        #endregion

        #region ctor

        public CurrentUserController()
        {
            try
            {
                this.FacadeService = ServiceLocator.Current.GetInstance<IUserFacadeService>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public CurrentUserController(IUserFacadeService facadeService)
        {
            if (facadeService == null)
                throw new Exception(" facade service can not be null");

            this.FacadeService = facadeService;
        }
        #endregion

        #region methods

        public  FuelUserDto GetCurrentFuelUser()
        {
            return this.FacadeService.GetCurrentFuelUser();
        }

        public long GetCurrentFuelUserCompanyId()
        {
            return this.FacadeService.GetCurrentFuelUserCompanyId();
        }


        public bool GetCurrentFuelUserAccessToHolding()
        {
            return this.FacadeService.GetCurrentFuelUserAccessToHolding();
        }

        #endregion
    }
}
