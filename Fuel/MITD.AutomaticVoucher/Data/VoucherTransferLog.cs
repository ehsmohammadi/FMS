//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MITD.AutomaticVoucher.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class VoucherTransferLog
    {
        public long Id { get; set; }
        public string FinancialExceptionMessage { get; set; }
        public long UserId { get; set; }
        public string VoucherIds { get; set; }
        public System.DateTime SendDate { get; set; }
        public string ConfigDate { get; set; }
        public string ConfigCode { get; set; }
    }
}