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
    
    public partial class WorkflowLog
    {
        public long Id { get; set; }
        public int WorkflowEntity { get; set; }
        public System.DateTime ActionDate { get; set; }
        public Nullable<int> WorkflowAction { get; set; }
        public long ActorUserId { get; set; }
        public string Remark { get; set; }
        public bool Active { get; set; }
        public long CurrentWorkflowStepId { get; set; }
        public Nullable<long> InvoiceId { get; set; }
        public Nullable<long> OrderId { get; set; }
        public Nullable<long> CharterId { get; set; }
        public Nullable<long> FuelReportId { get; set; }
        public Nullable<long> OffhireId { get; set; }
        public Nullable<long> ScrapId { get; set; }
        public string Discriminator { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Charter Charter { get; set; }
        public virtual FuelReport FuelReport { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Offhire Offhire { get; set; }
        public virtual Order Order { get; set; }
        public virtual Scrap Scrap { get; set; }
        public virtual WorkflowStep WorkflowStep { get; set; }
    }
}