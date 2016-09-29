using System.Runtime.Serialization;
using MITD.Fuel.Presentation.Contracts.Enums;
using MITD.Presentation;
using EffectiveFactorTypeEnum = MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums.EffectiveFactorTypeEnum;

namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.DTOs
{
    [DataContract]
    public partial class InvoiceAdditionalPriceDto : DTOBase
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

        [DataMember]
        public long Id
        {
            get { return this.id; }
            set { if ((object.ReferenceEquals(this.Id, value) != true)) {this.id= value;}}
        }


        [DataMember]
        public decimal Price
        {
            get { return this.price; }
            set { if ((object.ReferenceEquals(this.Price, value) != true)) {this.price= value;}}
        }
        decimal priceInMainCurrency;
        private decimal currencyToMainCurrencyRate;

        [DataMember]
        public decimal PriceInMainCurrency
        {
            get { return this.priceInMainCurrency; }
            set
            {
                if ((object.ReferenceEquals(this.PriceInMainCurrency, value) != true)) {this.priceInMainCurrency= value;}
            }
        }

        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { if ((object.ReferenceEquals(this.Description, value) != true)) {this.description= value;}}
        }


        [DataMember]
        public bool Divisionable
        {
            get { return this.divisionable; }
            set { if ((object.ReferenceEquals(this.Divisionable, value) != true)) {this.divisionable= value;}}
        }


        [DataMember]
        public InvoiceDto Invoice
        {
            get { return this.invoice; }
            set { if ((object.ReferenceEquals(this.Invoice, value) != true)) {this.invoice= value;}}
        }


        [DataMember]
        public long EffectiveFactorId
        {
            get { return this.effectiveFactorId; }
            set { if ((object.ReferenceEquals(this.EffectiveFactorId, value) != true)) {this.effectiveFactorId= value;}}
        }

        [DataMember]
        public string EffectiveFactorName
        {
            get { return this.effectiveFactorName; }
            set { if ((object.ReferenceEquals(this.EffectiveFactorName, value) != true)) {this.effectiveFactorName= value;}}
        }

        [DataMember]
        public EffectiveFactorTypeEnum EffectiveFactorType
        {
            get { return this.effectiveFactorType; }
            set { if ((object.ReferenceEquals(this.EffectiveFactorType, value) != true)) {this.effectiveFactorType= value;}}
        }

        [DataMember]
        public decimal CurrencyToMainCurrencyRate
        {
            get { return this.currencyToMainCurrencyRate; }
            set { if ((object.ReferenceEquals(this.CurrencyToMainCurrencyRate, value) != true)) {this.currencyToMainCurrencyRate= value;}}
        }

        #endregion
    }

}