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
    
    public partial class HAFEZVoyagesView
    {
        public int Id { get; set; }
        public string VesselCode { get; set; }
        public short ShipOwnerId { get; set; }
        public string VoyageNumber { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public byte TripType { get; set; }
        public bool IsActive { get; set; }
    }
}