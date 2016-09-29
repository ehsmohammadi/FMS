using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.Fuel.Domain.Model.DomainObjects;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Enums.Inventory;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.Repositories;

namespace MITD.Fuel.Domain.Model.Extensions
{
    public static class InventoryExtensions
    {
        public static string GetActionNumber(this Inventory_Transaction source, string vesselCode = null)
        {
            return BuildActionNumber((TransactionType)source.Action, source.WarehouseId, source.Code.Value, vesselCode ?? source.Inventory_Warehouse.Code);
        }

        public static string BuildActionNumber(TransactionType transactionType, long inventoryWarehouseId, decimal code, string vesselCode)
        {
            return string.Format("{0}/{1}/{2}/{3}", transactionType, inventoryWarehouseId, code, vesselCode);
        }

        public static void ExtractActionNumberValues(string inventoryReferenceActionNumber, out TransactionType transactionType, out long inventoryWarehouseId, out decimal code, out string vesselCode)
        {
            var parts = inventoryReferenceActionNumber.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), parts[0]);

            inventoryWarehouseId = long.Parse(parts[1]);

            code = decimal.Parse(parts[2]);

            vesselCode = parts[3];
        }

        public static void MergeInventoryOperationResult(this List<InventoryOperation> source, InventoryOperationResult inventoryOperationResult)
        {
            var inventoryOperationRepository = ServiceLocator.Current.GetInstance<IInventoryOperationRepository>();

            for (int index = 0; index < source.Count; )
            {
                if (inventoryOperationResult.RemovedTransactionIds.Contains(source[index].InventoryOperationId))
                    inventoryOperationRepository.Delete(source[index]);
                else
                    index++;
            }

            source.AddRange(inventoryOperationResult.CreatedInventoryOperations);
        }
    }
}
