using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Factories
{
    public interface IInventoryOperationFactory : IFactory
    {
        InventoryOperation Create(FuelReportDetail fuelReportDetail, long inventoryOperationId, string actionNumber, InventoryActionType actionType, DateTime actionDate);
    }
}
