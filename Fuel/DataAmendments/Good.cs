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
    
    public partial class Good
    {
        public Good()
        {
            this.TransactionItems = new HashSet<TransactionItem>();
        }
    
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public long MainUnitId { get; set; }
        public Nullable<int> UserCreatorId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual Unit Unit { get; set; }
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }
    }
}
