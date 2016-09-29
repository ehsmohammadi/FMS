using System;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.Factories
{
    public class InventoryOperationFactory : IInventoryOperationFactory
    {
        public DomainObjects.InventoryOperation Create(FuelReportDetail fuelReportDetail, long inventoryOperationId, string actionNumber, Enums.InventoryActionType actionType, DateTime actionDate)
        {
            InventoryOperation result = new InventoryOperation(
                inventoryOperationId,        
                actionNumber,
                actionDate,
                actionType);

            return result;
        }
    }
}
