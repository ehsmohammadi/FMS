using System.Collections.Generic;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    /// <summary>
    /// This class is used by FuelManagment Services (Fuel Domain) to retrieve Inventory data, via InventoryManagement service.
    /// </summary>
    public class InventoryResult
    {
        public long Id { get; set; }

        /// <summary>
        /// Inventory Operation Number in format like 'Receipt/[WarehouseId]/[OperationCode]/[VesselCode]'
        /// </summary>
        public string Number { get; set; }

        public InventoryActionType ActionType { get; set; }

        public List<InventoryResultItem> InventoryResultItems { get; set; }
    }
}
