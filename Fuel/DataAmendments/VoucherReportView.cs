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
    
    public partial class VoucherReportView
    {
        public Nullable<long> VoucherId { get; set; }
        public long JournalEntryId { get; set; }
        public string VoucherCompany { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> FinancialVoucherDate { get; set; }
        public Nullable<System.DateTime> LocalVoucherDate { get; set; }
        public string LocalVoucherNo { get; set; }
        public string ReferenceNo { get; set; }
        public string VoucherRef { get; set; }
        public Nullable<int> ReferenceTypeId { get; set; }
        public Nullable<int> VoucherDetailTypeId { get; set; }
        public Nullable<int> VoucherTypeId { get; set; }
        public string AccountNo { get; set; }
        public string JournalEntryDescription { get; set; }
        public string JournalEntryVoucherRef { get; set; }
        public decimal ForeignAmount { get; set; }
        public decimal IrrAmount { get; set; }
        public int JournalEntryType { get; set; }
        public string JournalEntryCurrency { get; set; }
        public string SegmentName { get; set; }
        public string SegmentCode { get; set; }
        public string SegmentTypeName { get; set; }
        public string SegmentTypeCode { get; set; }
        public string UserName { get; set; }
    }
}
