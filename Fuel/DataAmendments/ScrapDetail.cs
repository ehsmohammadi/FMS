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
    
    public partial class ScrapDetail
    {
        public long Id { get; set; }
        public double ROB { get; set; }
        public double Price { get; set; }
        public byte[] TimeStamp { get; set; }
        public long CurrencyId { get; set; }
        public long GoodId { get; set; }
        public Nullable<long> TankId { get; set; }
        public long UnitId { get; set; }
        public long ScrapId { get; set; }
    
        public virtual Scrap Scrap { get; set; }
    }
}