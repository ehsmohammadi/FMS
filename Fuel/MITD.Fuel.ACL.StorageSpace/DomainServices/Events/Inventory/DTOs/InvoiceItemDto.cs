using System.Runtime.Serialization;
using MITD.Presentation;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class InvoiceItemDto : DTOBase
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


        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }

        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { if ((object.ReferenceEquals(this.Description, value) != true)) {this.description= value;}}
        }

        //[CustomValidation(typeof (ValidationDto), "IsGreaterZero")]
        [DataMember]
        public decimal Quantity
        {
            get { return this.quantity; }
            set
            {
                if ((object.ReferenceEquals(this.Quantity, value) != true)) {this.quantity= value;}
            }
        }

        [DataMember]
        public decimal Fee
        {
            get { return this.fee; }
            set
            {
                if ((object.ReferenceEquals(this.Fee, value) != true)) {this.fee= value;}
            }
        }

        [DataMember]
        public decimal Price
        {
            get { return this.price; }
            set { if ((object.ReferenceEquals(this.Price, value) != true)) {this.price= value;}}
        }

        [DataMember]
        public decimal DivisionPrice
        {
            get { return this.divisionPrice; }
            set { if ((object.ReferenceEquals(this.DivisionPrice, value) != true)) {this.divisionPrice= value;}}
        }


        [DataMember]
        public decimal TotalPrice
        {

            get { return this.totalPrice; }
            set { if ((object.ReferenceEquals(this.TotalPrice, value) != true)) {this.totalPrice= value;}}
        }

        [DataMember]
        public long GoodId
        {
            get { return this.goodId; }
            set { if ((object.ReferenceEquals(this.GoodId, value) != true)) {this.goodId= value;}}
        }


        [DataMember]
        public long MeasuringUnitId
        {
            get { return this.measuringUnitId; }
            set { if ((object.ReferenceEquals(this.MeasuringUnitId, value) != true)) {this.measuringUnitId= value;}}
        }

        [DataMember]
        public long InvoiceId
        {
            get { return this.invoiceId; }
            set { if ((object.ReferenceEquals(this.InvoiceId, value) != true)) {this.invoiceId= value;}}
        }

        [DataMember]
        public string GoodName
        {
            get { return this.goodName; }
            set { if ((object.ReferenceEquals(this.GoodName, value) != true)) {this.goodName= value;}}
        }

        [DataMember]
        public string GoodCode
        {
            get { return this.goodName; }
            set { if ((object.ReferenceEquals(this.GoodName, value) != true)) {this.goodName= value;}}
        }

        [DataMember]
        public string MeasuringUnitName
        {
            get { return this.measuringUnitName; }
            set { if ((object.ReferenceEquals(this.MeasuringUnitName, value) != true)) {this.measuringUnitName= value;}}
        }

        [DataMember]
        public decimal DivisionPriceInMainCurrency
        {
            get { return this.divisionPriceInMainCurrency; }
            set { if ((object.ReferenceEquals(this.DivisionPriceInMainCurrency, value) != true)) {this.divisionPriceInMainCurrency= value;}}
        }


        [DataMember]
        public long DivisionPercent
        {
            get { return this.divisionPercent; }
            set { if ((object.ReferenceEquals(this.DivisionPercent, value) != true)) {this.divisionPercent= value;}}
        }


        [DataMember]
        public decimal PriceInMainCurrency
        {
            get { return this.priceInMainCurrency; }
            set { if ((object.ReferenceEquals(this.PriceInMainCurrency, value) != true)) {this.priceInMainCurrency= value;}}
        }


        [DataMember]
        public decimal FeeInMainCurrency
        {
            get { return this.feeInMainCurrency; }
            set { if ((object.ReferenceEquals(this.FeeInMainCurrency, value) != true)) {this.feeInMainCurrency= value;}}
        }


        [DataMember]
        public decimal QuantityWithMainUnit
        {
            get { return this.quantityWithMainUnit; }
            set { if ((object.ReferenceEquals(this.QuantityWithMainUnit, value) != true)) {this.quantityWithMainUnit= value;}}
        }

        [DataMember]
        public string MainUnitName
        {
            get { return this.mainUnitName; }
            set { if ((object.ReferenceEquals(this.MainUnitName, value) != true)) {this.mainUnitName= value;}}
        }

        [DataMember]
        public decimal CurrencyToMainCurrencyRate
        {
            get { return this.currencyToMainCurrencyRate; }
            set { if ((object.ReferenceEquals(this.CurrencyToMainCurrencyRate, value) != true)) {this.currencyToMainCurrencyRate= value;}}

        }


    }
}