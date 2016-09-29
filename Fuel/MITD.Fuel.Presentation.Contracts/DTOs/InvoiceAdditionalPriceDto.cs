#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;

#endregion

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class InvoiceAdditionalPriceDto
    {
        #region props

        private string description;
        private bool divisionable;
        private EffectiveFactorTypeEnum effectiveFactorType;
        private long effectiveFactorId;
        private string effectiveFactorName;
        private long id;

        private InvoiceDto invoice;
        private decimal price;

        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }


        public decimal Price
        {
            get { return price; }
            set
            {
                this.SetField(p => p.Price, ref price, value);
                base.OnPropertyChanged(this.GetPropertyName(p => p.PriceInMainCurrency));                
            }
        }
        //decimal priceInMainCurrency;
        private decimal currencyToMainCurrencyRate;

        public decimal PriceInMainCurrency
        {
            //get { return priceInMainCurrency; }
            //set
            //{
            //    this.SetField(p => p.PriceInMainCurrency, ref priceInMainCurrency, value);
            //}

            get { return decimal.Round(CurrencyToMainCurrencyRate * Price, 2); }
        }

        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }


        public bool Divisionable
        {
            get { return divisionable; }
            set { this.SetField(p => p.Divisionable, ref divisionable, value); }
        }


        public InvoiceDto Invoice
        {
            get { return invoice; }
            set { this.SetField(p => p.Invoice, ref invoice, value); }
        }


        public long EffectiveFactorId
        {
            get { return effectiveFactorId; }
            set { this.SetField(p => p.EffectiveFactorId, ref effectiveFactorId, value); }
        }

        public string EffectiveFactorName
        {
            get { return effectiveFactorName; }
            set { this.SetField(p => p.EffectiveFactorName, ref effectiveFactorName, value); }
        }

        public EffectiveFactorTypeEnum EffectiveFactorType
        {
            get { return effectiveFactorType; }
            set { this.SetField(p => p.EffectiveFactorType, ref effectiveFactorType, value); }
        }

        public decimal CurrencyToMainCurrencyRate
        {
            get { return currencyToMainCurrencyRate; }
            set
            {
                this.SetField(p => p.CurrencyToMainCurrencyRate, ref currencyToMainCurrencyRate, value);
                base.OnPropertyChanged(this.GetPropertyName(p => p.PriceInMainCurrency));
            }
        }
        //protected override void OnPropertyChanged(string propertyName)
        //{
        //    PriceInMainCurrency = decimal.Round(CurrencyToMainCurrencyRate*Price, 2);
        //    base.OnPropertyChanged(propertyName);
        //}

        #endregion
    }

}