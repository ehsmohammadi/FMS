using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.DomainObjects.CharterAggregate;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class InventoryOperation
    {
        public long Id { get; private set; }

        public long InventoryOperationId { get; private set; }

        public string ActionNumber { get; private set; }

        public DateTime ActionDate { get; private set; }

        public InventoryActionType ActionType { get; private set; }

        public byte[] TimeStamp { get; set; }

        public long? FuelReportDetailId { get; private set; }

        public long? CharterId { get; private set; }

        public virtual Charter Charter { get; private set; }

        public virtual FuelReportDetail FuelReportDetail { get; set; }
        public virtual Scrap Scrap { get; private set; }

        public virtual FuelReport FuelReport { get; set; }

        public long? Scrap_Id { get; private set; }
        public long? FuelReport_Id { get; private set; }

        public InventoryOperation()
        {
        }

        public InventoryOperation(
            long inventoryOperationId,
            string actionNumber,
            DateTime actionDate,
            InventoryActionType actionType)
        {
            this.InventoryOperationId = inventoryOperationId;
            this.ActionNumber = actionNumber;
            this.ActionDate = actionDate;
            this.ActionType = actionType;
        }
    }
}
