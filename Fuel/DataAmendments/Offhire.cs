//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAmendments
{
    using System;
    using System.Collections.Generic;
    
    public partial class Offhire
    {
        public Offhire()
        {
            this.OffhireDetails = new HashSet<OffhireDetail>();
            this.WorkflowLogs = new HashSet<WorkflowLog>();
        }
    
        public long Id { get; set; }
        public long ReferenceNumber { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public int IntroducerType { get; set; }
        public System.DateTime VoucherDate { get; set; }
        public long VoucherCurrencyId { get; set; }
        public string PricingReference_Number { get; set; }
        public int PricingReference_Type { get; set; }
        public int State { get; set; }
        public byte[] TimeStamp { get; set; }
        public long IntroducerId { get; set; }
        public long OffhireLocationId { get; set; }
        public long VesselInCompanyId { get; set; }
        public Nullable<long> VoyageId { get; set; }
    
        public virtual VesselInCompany VesselInCompany { get; set; }
        public virtual ICollection<OffhireDetail> OffhireDetails { get; set; }
        public virtual ICollection<WorkflowLog> WorkflowLogs { get; set; }
    }
}
