using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class OffhireManagementSystemDto
    {
        private long referenceNumber;
        public long ReferenceNumber
        {
            get { return referenceNumber; }
            set { this.SetField(p => p.ReferenceNumber, ref this.referenceNumber, value); }
        }

        private ActivityLocationDto offhireLocation;
        public ActivityLocationDto OffhireLocation
        {
            get { return offhireLocation; }
            set { this.SetField(p => p.OffhireLocation, ref this.offhireLocation, value); }
        }

        private DateTime startDateTime;
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set { this.SetField(p => p.StartDateTime, ref this.startDateTime, value); }
        }

        private DateTime endDateTime;
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set { this.SetField(p => p.EndDateTime, ref this.endDateTime, value); }
        }

        private VesselInCompanyDto vesselInCompany;
        public VesselInCompanyDto VesselInCompany
        {
            get { return vesselInCompany; }
            set { this.SetField(p => p.VesselInCompany, ref this.vesselInCompany, value); }
        }

        private ObservableCollection<OffhireManagementSystemDetailDto> offhireDetails;
        public ObservableCollection<OffhireManagementSystemDetailDto> OffhireDetails
        {
            get { return this.offhireDetails; }
            set { this.SetField(p => p.OffhireDetails, ref this.offhireDetails, value); }
        }
    }

    public partial class OffhireManagementSystemDetailDto
    {
        private decimal quantity;
        public decimal Quantity
        {
            get { return quantity; }
            set { this.SetField(p => p.Quantity, ref quantity, value); }
        }

        private GoodDto good;
        public GoodDto Good
        {
            get { return good; }
            set { this.SetField(p => p.Good, ref good, value); }
        }

        private GoodUnitDto unit;
        public GoodUnitDto Unit
        {
            get { return unit; }
            set { this.SetField(p => p.Unit, ref unit, value); }
        }

        private TankDto tank;
        public TankDto Tank
        {
            get { return tank; }
            set { this.SetField(p => p.Tank, ref tank, value); }
        }
    }
}