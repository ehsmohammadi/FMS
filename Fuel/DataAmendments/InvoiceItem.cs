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
    
    public partial class InvoiceItem
    {
        public InvoiceItem()
        {
            this.OrderItemBalances = new HashSet<OrderItemBalance>();
            this.OrderItemBalances1 = new HashSet<OrderItemBalance>();
        }
    
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal Fee { get; set; }
        public long InvoiceId { get; set; }
        public long GoodId { get; set; }
        public long MeasuringUnitId { get; set; }
        public string Description { get; set; }
        public byte[] TimeStamp { get; set; }
        public decimal DivisionPrice { get; set; }
    
        public virtual Invoice Invoice { get; set; }
        public virtual ICollection<OrderItemBalance> OrderItemBalances { get; set; }
        public virtual ICollection<OrderItemBalance> OrderItemBalances1 { get; set; }
    }
}
