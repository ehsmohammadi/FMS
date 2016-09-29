using System;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class InventoryResultItemDto
    {
        private long id;
        private GoodDto good;
        private CurrencyDto currency;
        private decimal fee;
        private decimal quantity;
        private long? transactionId;

        public long Id
        {
            get { return this.id; }
            set { this.SetField(p => p.Id, ref this.id, value); }
        }

        public GoodDto Good
        {
            get { return this.good; }
            set { this.SetField(p => p.Good, ref this.good, value); }
        }

        public CurrencyDto Currency
        {
            get { return this.currency; }
            set { this.SetField(p => p.Currency, ref this.currency, value); }
        }

        public Decimal Fee
        {
            get { return this.fee; }
            set { this.SetField(p => p.Fee, ref this.fee, value); }
        }

        public Decimal Quantity
        {
            get { return this.quantity; }
            set { this.SetField(p => p.Quantity, ref this.quantity, value); }
        }

        public long? TransactionId
        {
            get { return this.transactionId; }
            set { this.SetField(p => p.TransactionId, ref this.transactionId, value); }
        }
    }
}
