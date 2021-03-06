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
    
    public partial class EffectiveFactor
    {
        public EffectiveFactor()
        {
            this.InvoiceAdditionalPrices = new HashSet<InvoiceAdditionalPrice>();
            this.Segments = new HashSet<Segment>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public int EffectiveFactorType { get; set; }
        public byte[] TimeStamp { get; set; }
        public Nullable<int> AccountId { get; set; }
        public string VoucherDescription { get; set; }
        public string VoucherRefDescription { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual ICollection<InvoiceAdditionalPrice> InvoiceAdditionalPrices { get; set; }
        public virtual ICollection<Segment> Segments { get; set; }
    }
}
