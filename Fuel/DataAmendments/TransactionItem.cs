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
    
    public partial class TransactionItem
    {
        public TransactionItem()
        {
            this.TransactionItemPrices = new HashSet<TransactionItemPrice>();
        }
    
        public int Id { get; set; }
        public short RowVersion { get; set; }
        public int TransactionId { get; set; }
        public long GoodId { get; set; }
        public long QuantityUnitId { get; set; }
        public Nullable<decimal> QuantityAmount { get; set; }
        public string Description { get; set; }
        public Nullable<int> UserCreatorId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual Good Good { get; set; }
        public virtual ICollection<TransactionItemPrice> TransactionItemPrices { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
