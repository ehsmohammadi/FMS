using System.Collections.Generic;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Presentation.Contracts;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
    public interface IInventoryCompanyFacadeService : IFacadeService
    {

        Inventory_CompanyDto Get(long id);

        List<Inventory_CompanyDto> GetByCurrentUser();

        List<Inventory_CompanyDto> Get();

        Inventory_WarehouseDto Get(long companyId, long warehouseId);

        List<Inventory_WarehouseDto> GetWarehouse(long companyId);
        
    }
}
