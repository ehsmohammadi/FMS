#region

using System.Collections.Generic;
using MITD.Domain.Model;
using MITD.Fuel.Domain.Model.DomainObjects;

#endregion

namespace MITD.Fuel.Domain.Model.IDomainServices
{
    public interface IInventoryCompanyDomainService : IDomainService
    {
        Inventory_Company Get(long id);

        List<Inventory_Company> Get();

        List<Inventory_Company> GetCurrentUserCompanies();

        Inventory_Warehouse Get(long companyId, long warehouseId);

        List<Inventory_Warehouse> GetWarehouse(long companyId);
    }
}