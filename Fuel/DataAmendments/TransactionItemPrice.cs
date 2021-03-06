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
    
    public partial class TransactionItemPrice
    {
        public TransactionItemPrice()
        {
            this.TransactionItemPrices1 = new HashSet<TransactionItemPrice>();
        }
    
        public int Id { get; set; }
        public short RowVersion { get; set; }
        public int TransactionId { get; set; }
        public int TransactionItemId { get; set; }
        public string Description { get; set; }
        public long QuantityUnitId { get; set; }
        public Nullable<decimal> QuantityAmount { get; set; }
        public long PriceUnitId { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public long MainCurrencyUnitId { get; set; }
        public Nullable<decimal> FeeInMainCurrency { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<decimal> QuantityAmountUseFIFO { get; set; }
        public Nullable<int> TransactionReferenceId { get; set; }
        public string IssueReferenceIds { get; set; }
        public Nullable<int> UserCreatorId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual Unit Unit { get; set; }
        public virtual Unit Unit1 { get; set; }
        public virtual Unit Unit2 { get; set; }
        public virtual TransactionItem TransactionItem { get; set; }
        public virtual ICollection<TransactionItemPrice> TransactionItemPrices1 { get; set; }
        public virtual TransactionItemPrice TransactionItemPrice1 { get; set; }
    }
}
