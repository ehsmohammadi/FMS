using System;
using System.Collections.Generic;
namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class OffhireManagementSystemEntity
    {
        public long ReferenceNumber { get; set; }

        public ActivityLocation Location { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public VesselInCompany VesselInCompany { get; set; }

        public bool HasVoucher { get; set; }

        public List<OffhireManagementSystemEntityDetail> OffhireDetails { get; set; }
    }

    public class OffhireManagementSystemEntityDetail
    {
        public decimal QuantityAmount { get; set; }
        public virtual Good Good { get; set; }
        public virtual GoodUnit Unit { get; set; }
        public virtual Tank Tank { get; set; }
    }


}