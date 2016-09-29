using System;
using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Contracts.SL.ServiceWrapper
{
    public interface IInventoryCompanyServiceWrapper : IServiceWrapper
    {
        void GetAll(Action<List<Inventory_CompanyDto>, Exception> action, bool filterByUser);

        void GetById(Action<Inventory_CompanyDto, Exception> action, int id);

        void GetWarehouse(Action<List<Inventory_WarehouseDto>, Exception> action, long companyId);

        void GetWarehouseById(Action<Inventory_WarehouseDto, Exception> action, long companyId, long id);

  
    }
}
