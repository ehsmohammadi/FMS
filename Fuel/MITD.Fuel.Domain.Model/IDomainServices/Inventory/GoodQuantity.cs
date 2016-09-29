using System;
using MITD.Fuel.Domain.Model.Exceptions;

namespace MITD.Fuel.Domain.Model.IDomainServices.Inventory
{
    public struct GoodQuantity
    {
        public int TransactionId { get; set; }
        public int TransactionItemId { get; set; }
        public long InventoryGoodId { get; set; }
        public decimal SignedQuantity { get; set; }
        public long InventoryQuantityUnitId { get; set; }

        public Sign QuantitySign
        {
            get
            {
                if (this.SignedQuantity > 0) return Sign.Positive;
                if (this.SignedQuantity < 0) return Sign.Negative;

                throw new InvalidOperation("Detect Sign", "Invalid signed quantity.");
            }
        }
    }

    public struct GoodQuantityPricing
    {
        public int TransactionId { get; set; }
        public int TransactionItemId { get; set; }
        public int TransactionItemPriceId { get; set; }
        public long InventoryGoodId { get; set; }
        public decimal SignedQuantity { get; set; }
        public long InventoryQuantityUnitId { get; set; }
        public decimal FeeInMainCurrency { get; set; }
        public long InventoryMainCurrencyUnitId { get; set; }
        public DateTime? DateTimeForRevertAdjustment { get; set; }
        public long PricingReferenceTransactionIdForRevertAdjustment { get; set; }
        public decimal Fee { get; set; }
        public long FeeInventoryCurrencyUnitId { get; set; }
        public string Description { get; set; }
        public decimal UnsignedQuantity { get { return Math.Abs(this.SignedQuantity); } }
        public Sign QuantitySign
        {
            get
            {
                if (this.SignedQuantity > 0) return Sign.Positive;
                if (this.SignedQuantity < 0) return Sign.Negative;

                throw new InvalidOperation("Detect Sign", "Invalid signed quantity.");
            }
        }

        public string PricingReferenceType { get; set; }
        public string PricingReferenceNumber { get; set; }
    }

    public enum Sign
    {
        Positive,
        Negative
    }
}
