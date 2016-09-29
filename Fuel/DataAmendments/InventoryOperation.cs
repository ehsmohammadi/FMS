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
    
    public partial class InventoryOperation
    {
        public InventoryOperation()
        {
            this.OrderItemBalances = new HashSet<OrderItemBalance>();
        }
    
        public long Id { get; set; }
        public string ActionNumber { get; set; }
        public System.DateTime ActionDate { get; set; }
        public int ActionType { get; set; }
        public byte[] TimeStamp { get; set; }
        public Nullable<long> FuelReportDetailId { get; set; }
        public Nullable<long> CharterId { get; set; }
        public Nullable<long> Scrap_Id { get; set; }
        public Nullable<long> FuelReport_Id { get; set; }
        public long InventoryOperationId { get; set; }
    
        public virtual Charter Charter { get; set; }
        public virtual FuelReport FuelReport { get; set; }
        public virtual FuelReportDetail FuelReportDetail { get; set; }
        public virtual Scrap Scrap { get; set; }
        public virtual ICollection<OrderItemBalance> OrderItemBalances { get; set; }
    }
}