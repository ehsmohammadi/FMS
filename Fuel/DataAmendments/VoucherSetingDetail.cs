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
    
    public partial class VoucherSetingDetail
    {
        public VoucherSetingDetail()
        {
            this.AsgnSegmentTypeVoucherSetingDetails = new HashSet<AsgnSegmentTypeVoucherSetingDetail>();
            this.AsgnVoucherAconts = new HashSet<AsgnVoucherAcont>();
        }
    
        public long Id { get; set; }
        public long VoucherSetingId { get; set; }
        public string VoucherCeditRefDescription { get; set; }
        public string VoucherDebitDescription { get; set; }
        public string VoucherDebitRefDescription { get; set; }
        public string VoucherCreditDescription { get; set; }
        public long GoodId { get; set; }
        public bool IsDelete { get; set; }
    
        public virtual ICollection<AsgnSegmentTypeVoucherSetingDetail> AsgnSegmentTypeVoucherSetingDetails { get; set; }
        public virtual ICollection<AsgnVoucherAcont> AsgnVoucherAconts { get; set; }
        public virtual VoucherSeting VoucherSeting { get; set; }
    }
}
