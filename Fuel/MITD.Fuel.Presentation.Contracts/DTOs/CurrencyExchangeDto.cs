using System;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class CurrencyExchangeDto
    {
        private int id;
        private long fromCurrencyId;
        private long toCurrencyId;
        private decimal coefficient;
        private DateTime? effectiveDateStart;
        private DateTime? effectiveDateEnd;
        private DateTime createDate;
        private CurrencyDto fromCurrency;
        private CurrencyDto toCurrency;

        public int Id
        {
            get { return this.id; }
            set { this.SetField(p => p.Id, ref this.id, value); }
        }

        public long FromCurrencyId
        {
            get { return this.fromCurrencyId; }
            set { this.SetField(p => p.FromCurrencyId, ref this.fromCurrencyId, value); }
        }

        public long ToCurrencyId
        {
            get { return this.toCurrencyId; }
            set { this.SetField(p => p.ToCurrencyId, ref this.toCurrencyId, value); }
        }

        public decimal Coefficient
        {
            get { return this.coefficient; }
            set { this.SetField(p => p.Coefficient, ref this.coefficient, value); }
        }

        public DateTime? EffectiveDateStart
        {
            get { return this.effectiveDateStart; }
            set { this.SetField(p => p.EffectiveDateStart, ref this.effectiveDateStart, value); }
        }

        public DateTime? EffectiveDateEnd
        {
            get { return this.effectiveDateEnd; }
            set { this.SetField(p => p.EffectiveDateEnd, ref this.effectiveDateEnd, value); }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
            set { this.SetField(p => p.CreateDate, ref this.createDate, value); }
        }

        public virtual CurrencyDto FromCurrency
        {
            get { return this.fromCurrency; }
            set { this.SetField(p => p.FromCurrency, ref this.fromCurrency, value); }
        }

        public virtual CurrencyDto ToCurrency
        {
            get { return this.toCurrency; }
            set { this.SetField(p => p.ToCurrency, ref this.toCurrency, value); }
        }
    }
}