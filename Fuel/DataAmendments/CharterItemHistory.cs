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
    
    public partial class CharterItemHistory
    {
        public long Id { get; set; }
        public long CharterId { get; set; }
        public long CharterItemId { get; set; }
        public long GoodUnitId { get; set; }
        public int CharterStateId { get; set; }
        public long GoodId { get; set; }
        public Nullable<long> TankId { get; set; }
        public decimal Rob { get; set; }
        public decimal Fee { get; set; }
        public decimal OffhireFee { get; set; }
        public System.DateTime DateRegisterd { get; set; }
        public byte[] TimeStamp { get; set; }
    }
}
