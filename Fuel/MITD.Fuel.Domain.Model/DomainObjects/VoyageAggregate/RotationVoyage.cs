using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace MITD.Fuel.Domain.Model.DomainObjects
{
    /// <summary>
    /// There are no comments for MITD.Fuel.Domain.Model.DomainObjects.Voyage in the schema.
    /// </summary>
    public class RotationVoyage
    {
        public RotationVoyage()
        {
        }

        public long Id { get; set; }
        public string VoyageNumber { get; set; }
        public string Description { get; set; }
        public long VesselInCompanyId { get; set; }
        public long CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}