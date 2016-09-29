using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public class FuelReportInventoryResultDto
    {
        private long fuelReportId;

        public long FuelReportId
        {
            get { return fuelReportId; }
            set { fuelReportId = value; }
        }

        private List<FuelReportInventoryResultItemDto> items;

        public List<FuelReportInventoryResultItemDto> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
