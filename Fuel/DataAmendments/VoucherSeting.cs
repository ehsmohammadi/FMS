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
    
    public partial class VoucherSeting
    {
        public VoucherSeting()
        {
            this.VoucherSetingDetails = new HashSet<VoucherSetingDetail>();
        }
    
        public long Id { get; set; }
        public string VoucherMainRefDescription { get; set; }
        public string VoucherMainDescription { get; set; }
        public long CompanyId { get; set; }
        public int VoucherDetailTypeId { get; set; }
        public byte[] TimeStamp { get; set; }
        public int VoucherTypeId { get; set; }
    
        public virtual ICollection<VoucherSetingDetail> VoucherSetingDetails { get; set; }
    }
}
