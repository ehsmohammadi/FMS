using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Services.Facade;
using MITD.Fuel.ACL.StorageSpace.InventoryServiceReference;

namespace MITD.Fuel.ACL.StorageSpace.Mappers.Inventory.Contracts
{
    public interface IInventoryOperationToInventoryOperationDtoMapper : IFacadeMapper<InventoryOperation, FuelReportInventoryOperationDto>
    {
    }
}
