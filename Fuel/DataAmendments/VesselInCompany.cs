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
    
    public partial class VesselInCompany
    {
        public VesselInCompany()
        {
            this.Charters = new HashSet<Charter>();
            this.FuelReports = new HashSet<FuelReport>();
            this.Offhires = new HashSet<Offhire>();
            this.Orders = new HashSet<Order>();
            this.Orders1 = new HashSet<Order>();
            this.Scraps = new HashSet<Scrap>();
            this.Voyages = new HashSet<Voyage>();
            this.VoyageLogs = new HashSet<VoyageLog>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CompanyId { get; set; }
        public long VesselId { get; set; }
        public int VesselStateCode { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual ICollection<Charter> Charters { get; set; }
        public virtual ICollection<FuelReport> FuelReports { get; set; }
        public virtual ICollection<Offhire> Offhires { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Order> Orders1 { get; set; }
        public virtual ICollection<Scrap> Scraps { get; set; }
        public virtual Vessel Vessel { get; set; }
        public virtual ICollection<Voyage> Voyages { get; set; }
        public virtual ICollection<VoyageLog> VoyageLogs { get; set; }
    }
}
