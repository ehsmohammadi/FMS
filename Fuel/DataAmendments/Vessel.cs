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
    
    public partial class Vessel
    {
        public Vessel()
        {
            this.VesselInCompanies = new HashSet<VesselInCompany>();
        }
    
        public long Id { get; set; }
        public string Code { get; set; }
        public long OwnerId { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual ICollection<VesselInCompany> VesselInCompanies { get; set; }
    }
}
