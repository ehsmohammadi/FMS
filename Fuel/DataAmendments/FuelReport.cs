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
    
    public partial class FuelReport
    {
        public FuelReport()
        {
            this.FuelReportDetails = new HashSet<FuelReportDetail>();
            this.InventoryOperations = new HashSet<InventoryOperation>();
            this.WorkflowLogs = new HashSet<WorkflowLog>();
        }
    
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public System.DateTime EventDate { get; set; }
        public System.DateTime ReportDate { get; set; }
        public long VesselInCompanyId { get; set; }
        public Nullable<long> VoyageId { get; set; }
        public int FuelReportType { get; set; }
        public byte[] TimeStamp { get; set; }
        public int State { get; set; }
        public Nullable<long> CreatedCharterId { get; set; }
    
        public virtual Charter Charter { get; set; }
        public virtual VesselInCompany VesselInCompany { get; set; }
        public virtual ICollection<FuelReportDetail> FuelReportDetails { get; set; }
        public virtual ICollection<InventoryOperation> InventoryOperations { get; set; }
        public virtual ICollection<WorkflowLog> WorkflowLogs { get; set; }
    }
}