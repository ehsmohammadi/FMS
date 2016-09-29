#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Fuel.Presentation.Contracts.Infrastructure;
using MITD.Presentation;

#endregion

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class InvoiceItemDto
    {
        private string description;
        private long divisionPercent;
        private decimal divisionPrice;
        private decimal divisionPriceInMainCurrency;
        private decimal fee;
        private decimal feeInMainCurrency;
        private string goodCode;
        private long goodId;
        private string goodName;
        private long id;
        private long invoiceId;
        private string mainUnitName;
        private long measuringUnitId;
        private string measuringUnitName;
        private decimal price;
        private decimal priceInMainCurrency;
        private decimal quantity;
        private decimal quantityWithMainUnit;
        private decimal currencyToMainCurrencyRate;
        private decimal totalPrice;


        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }

        [CustomValidation(typeof (ValidationDto), "IsGreaterZero")]
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                this.SetField(p => p.Quantity, ref quantity, value);
            }
        }

        public decimal Fee
        {
            get { return fee; }
            set
            {
                this.SetField(p => p.Fee, ref fee, value);
            }
        }

        public decimal Price
        {
            get { return price; }
            set { this.SetField(p => p.Price, ref price, value); }
        }

        public decimal DivisionPrice
        {
            get { return divisionPrice; }
            set { this.SetField(p => p.DivisionPrice, ref divisionPrice, value); }
        }


        public decimal TotalPrice
        {

            get { return totalPrice; }
            set { this.SetField(p => p.TotalPrice, ref totalPrice, value); }
        }

        public long GoodId
        {
            get { return goodId; }
            set { this.SetField(p => p.GoodId, ref goodId, value); }
        }


        public long MeasuringUnitId
        {
            get { return measuringUnitId; }
            set { this.SetField(p => p.MeasuringUnitId, ref measuringUnitId, value); }
        }

        public long InvoiceId
        {
            get { return invoiceId; }
            set { this.SetField(p => p.InvoiceId, ref invoiceId, value); }
        }

        public string GoodName
        {
            get { return goodName; }
            set { this.SetField(p => p.GoodName, ref goodName, value); }
        }

        public string GoodCode
        {
            get { return goodName; }
            set { this.SetField(p => p.GoodName, ref goodName, value); }
        }

        public string MeasuringUnitName
        {
            get { return measuringUnitName; }
            set { this.SetField(p => p.MeasuringUnitName, ref measuringUnitName, value); }
        }

        public decimal DivisionPriceInMainCurrency
        {
            get { return divisionPriceInMainCurrency; }
            set { this.SetField(p => p.DivisionPriceInMainCurrency, ref divisionPriceInMainCurrency, value); }
        }


        public long DivisionPercent
        {
            get { return divisionPercent; }
            set { this.SetField(p => p.DivisionPercent, ref divisionPercent, value); }
        }


        public decimal PriceInMainCurrency
        {
            get { return priceInMainCurrency; }
            set { this.SetField(p => p.PriceInMainCurrency, ref priceInMainCurrency, value); }
        }


        public decimal FeeInMainCurrency
        {
            get { return feeInMainCurrency; }
            set { this.SetField(p => p.FeeInMainCurrency, ref feeInMainCurrency, value); }
        }


        public decimal QuantityWithMainUnit
        {
            get { return quantityWithMainUnit; }
            set { this.SetField(p => p.QuantityWithMainUnit, ref quantityWithMainUnit, value); }
        }

        public string MainUnitName
        {
            get { return mainUnitName; }
            set { this.SetField(p => p.MainUnitName, ref mainUnitName, value); }
        }

        public decimal CurrencyToMainCurrencyRate
        {
            get { return currencyToMainCurrencyRate; }
            set { this.SetField(p => p.CurrencyToMainCurrencyRate, ref currencyToMainCurrencyRate, value); }

        }


        protected override void OnPropertyChanged(string propertyName)
        {

            switch (propertyName)
            {
                case "CurrencyToMainCurrencyRate":
                case "Fee":
                case "Quantity":
                case "DivisionPrice":
                case "Price":

                    TotalPrice = DivisionPrice + Price;
                    FeeInMainCurrency = decimal.Round(CurrencyToMainCurrencyRate*Fee, 2);
                    Price = Fee*Quantity;
                    PriceInMainCurrency = decimal.Round(CurrencyToMainCurrencyRate*Price, 2);
                    DivisionPriceInMainCurrency = decimal.Round(CurrencyToMainCurrencyRate*DivisionPrice, 2);
                    break;
            }
            base.OnPropertyChanged(propertyName);
        }
    }
}