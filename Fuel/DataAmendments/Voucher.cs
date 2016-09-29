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
    
    public partial class Voucher
    {
        public Voucher()
        {
            this.JournalEntries = new HashSet<JournalEntry>();
        }
    
        public long Id { get; set; }
        public string Description { get; set; }
        public System.DateTime FinancialVoucherDate { get; set; }
        public System.DateTime LocalVoucherDate { get; set; }
        public string LocalVoucherNo { get; set; }
        public string ReferenceNo { get; set; }
        public string VoucherRef { get; set; }
        public int ReferenceTypeId { get; set; }
        public byte[] TimeStamp { get; set; }
        public long CompanyId { get; set; }
        public int VoucherDetailTypeId { get; set; }
        public int VoucherTypeId { get; set; }
        public long UserId { get; set; }
    
        public virtual ICollection<JournalEntry> JournalEntries { get; set; }
    }
}