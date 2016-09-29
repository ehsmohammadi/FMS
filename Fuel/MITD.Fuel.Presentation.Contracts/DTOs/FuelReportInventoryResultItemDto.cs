using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public class FuelReportInventoryResultItemDto
    {
        private long fuelReportDetailId;

        public long FuelReportDetailId
        {
            get { return fuelReportDetailId; }
            set { fuelReportDetailId = value; }
        }

        private string actionNumber;

        public string ActionNumber
        {
            get { return actionNumber; }
            set { actionNumber = value; }
        }

        private DateTime actionDate;

        public DateTime ActionDate
        {
            get { return actionDate; }
            set { actionDate = value; }
        }

        private InventoryResultDtoActionType actionType;

        public InventoryResultDtoActionType ActionType
        {
            get { return actionType; }
            set { actionType = value; }
        }

    }
}
