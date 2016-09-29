using System;
using System.Collections.ObjectModel;

namespace MITD.Fuel.Domain.Model.Commands
{
    public partial class OffhireManagementSystem
    {
        public string ReferenceNumber { get; set; }

        public long LocationId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public VesselDto Vessel { get; set; }

        public ObservableCollection<OffhireManagementSystemDetailDto> OffhireDetails { get; set; }
    }

    public partial class OffhireManagementSystemDetail
    {
        public decimal Quantity { get; set; }
        public virtual GoodDto Good { get; set; }
        public virtual GoodUnitDto Unit { get; set; }
        public virtual TankDto Tank { get; set; }
    }
}