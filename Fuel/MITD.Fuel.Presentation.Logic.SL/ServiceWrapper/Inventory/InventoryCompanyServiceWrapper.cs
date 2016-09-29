using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.SL.Infrastructure;
using MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper;
using MITD.Fuel.Presentation.Logic.SL.Infrastructure;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.FuelApp.Logic.SL.ServiceWrapper
{
    public class InventoryCompanyServiceWrapper : IInventoryCompanyServiceWrapper
    {
        private readonly string companyAddressFormatString;
        private readonly string companyWarehouseAddressFormatString;

        public InventoryCompanyServiceWrapper()
        {
            companyAddressFormatString = Path.Combine(ApiConfig.HostAddress, "apiarea/Inventory/InventoryCompany/{0}");
            companyWarehouseAddressFormatString = string.Concat(companyAddressFormatString, "/Warehouse/{1}");
        }

        #region methods

        public void GetAll(Action<List<Inventory_CompanyDto>, Exception> action, bool filterByUser)
        {
            var url = string.Format(companyAddressFormatString, string.Empty) + "?filterByUser=" + filterByUser;

            WebClientHelper.Get<List<Inventory_CompanyDto>>(new Uri(url, UriKind.Absolute),
                                                                    (res, exp) => action(res, exp),
                                                                    WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetById(Action<Inventory_CompanyDto, Exception> action, int id)
        {
            var url = string.Format(companyAddressFormatString, id);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute),
                                                     action,
                                                     WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetWarehouse(Action<List<Inventory_WarehouseDto>, Exception> action, long companyId)
        {
            var url = string.Format(companyWarehouseAddressFormatString, companyId, string.Empty);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute), action, WebClientHelper.MessageFormat.Json,ApiConfig.Headers);
        }

        public void GetWarehouseById(Action<Inventory_WarehouseDto, Exception> action, long companyId, long id)
        {
            var url = string.Format(companyWarehouseAddressFormatString, companyId, id);

            WebClientHelper.Get(new Uri(url, UriKind.Absolute),action,
                                                                    WebClientHelper.MessageFormat.Json, ApiConfig.Headers);
        }

        #endregion

    }
}
