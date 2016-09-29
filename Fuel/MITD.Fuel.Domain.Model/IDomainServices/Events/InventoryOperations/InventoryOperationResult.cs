using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects;

namespace MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations
{
    public class InventoryOperationResult
    {
        public List<long> RemovedTransactionIds { get; set; }
        public List<InventoryOperation> CreatedInventoryOperations { get; set; }

        public InventoryOperationResult()
        {
            this.RemovedTransactionIds = new List<long>();
            this.CreatedInventoryOperations = new List<InventoryOperation>();
        }

        public void Merge(InventoryOperationResult anotherResult)
        {
            this.RemovedTransactionIds.AddRange(anotherResult.RemovedTransactionIds);
            this.CreatedInventoryOperations.AddRange(anotherResult.CreatedInventoryOperations);
        }
    }
}
