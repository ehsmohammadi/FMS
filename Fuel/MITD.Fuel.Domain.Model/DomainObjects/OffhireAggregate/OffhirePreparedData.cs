using System;
using System.Collections.Generic;
namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class OffhirePreparedData
    {
        public long ReferenceNumber { get; set; }

        public ActivityLocation Location { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public Company Introducer { get; set; }

        public VesselInCompany VesselInCompany { get; set; }

        public Voyage Voyage { get; set; }

        public bool HasVoucher { get; set; }

        public List<OffhirePreparedDataDetail> OffhireDetails { get; set; }
    }

    public class OffhirePreparedDataDetail
    {
        public decimal Quantity { get; set; }
        public virtual Good Good { get; set; }
        public virtual GoodUnit Unit { get; set; }
        public virtual Tank Tank { get; set; }
        public decimal? FeeInMainCurrency { get; set; }
    }
}