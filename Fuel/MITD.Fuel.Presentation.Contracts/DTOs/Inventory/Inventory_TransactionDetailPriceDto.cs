using System;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class Inventory_TransactionDetailPriceDto
    {
        public Inventory_TransactionDetailPriceDto()
        {
            //TransactionDetail=new Inventory_TransactionDetailDto();
            //TransactionReference=new Inventory_TransactionDto();
            //QuantityUnit=new Inventory_UnitDto();
            //PriceUnit=new Inventory_UnitDto();
            //MainCurrencyUnit=new Inventory_UnitDto();
            //UserCreator=new Inventory_UserDto();
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        private short rowVersion;

        public short RowVersion
        {
            get { return rowVersion; }
            set { this.SetField(p => p.RowVersion, ref rowVersion, value); }
        }

        private int transactionId;

        public int TransactionId
        {
            get { return transactionId; }
            set { this.SetField(p => p.TransactionId, ref transactionId, value); }
        }

        private int transactionItemId;

        public int TransactionItemId
        {
            get { return transactionItemId; }
            set { this.SetField(p => p.TransactionItemId, ref transactionItemId, value); }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }

        private long quantityUnitId;

        public long QuantityUnitId
        {
            get { return quantityUnitId; }
            set { this.SetField(p => p.QuantityUnitId, ref quantityUnitId, value); }
        }

        private decimal? quantityAmount;

        public decimal? QuantityAmount
        {
            get { return quantityAmount; }
            set { this.SetField(p => p.QuantityAmount, ref quantityAmount, value); }
        }

        private long priceUnitId;

        public long PriceUnitId
        {
            get { return priceUnitId; }
            set { this.SetField(p => p.PriceUnitId, ref priceUnitId, value); }
        }

        private decimal? fee;

        public decimal? Fee
        {
            get { return fee; }
            set { this.SetField(p => p.Fee, ref fee, value); }
        }

        private int mainCurrencyUnitId;

        public int MainCurrencyUnitId
        {
            get { return mainCurrencyUnitId; }
            set { this.SetField(p => p.MainCurrencyUnitId, ref mainCurrencyUnitId, value); }
        }

        private decimal? feeInMainCurrency;

        public decimal? FeeInMainCurrency
        {
            get { return feeInMainCurrency; }
            set { this.SetField(p => p.FeeInMainCurrency, ref feeInMainCurrency, value); }
        }

        public decimal? PriceInMainCurrency { get { return FeeInMainCurrency * QuantityAmount; } }
        public decimal? Price { get { return Fee * QuantityAmount; } }

        private DateTime? registrationDate;

        public DateTime? RegistrationDate
        {
            get { return this.registrationDate; }
            set { this.SetField(p => p.RegistrationDate, ref this.registrationDate, value); }
        }

        private decimal? quantityAmountUseFifo;

        public decimal? QuantityAmountUseFifo
        {
            get { return quantityAmountUseFifo; }
            set { this.SetField(p => p.QuantityAmountUseFifo, ref quantityAmountUseFifo, value); }
        }

        private int? transactionReferenceId;

        public int? TransactionReferenceId
        {
            get { return transactionReferenceId; }
            set { this.SetField(p => p.TransactionReferenceId, ref transactionReferenceId, value); }
        }

        private string issueReferenceIds;

        public string IssueReferenceIds
        {
            get { return issueReferenceIds; }
            set { this.SetField(p => p.IssueReferenceIds, ref issueReferenceIds, value); }
        }

        private int? userCreatorId;

        public int? UserCreatorId
        {
            get { return userCreatorId; }
            set { this.SetField(p => p.UserCreatorId, ref userCreatorId, value); }
        }

        private DateTime? createDate;

        public DateTime? CreateDate
        {
            get { return this.createDate; }
            set { this.SetField(p => p.CreateDate, ref this.createDate, value); }
        }

        private Inventory_TransactionDetailDto transactionDetail;
        public Inventory_TransactionDetailDto TransactionDetail
        {
            get { return transactionDetail; }
            set { this.SetField(p => p.TransactionDetail, ref transactionDetail, value); }
        }

        private Inventory_TransactionDto transactionReference;
        public Inventory_TransactionDto TransactionReference
        {
            get { return transactionReference; }
            set { this.SetField(p => p.TransactionReference, ref transactionReference, value); }
        }
        
        private Inventory_UnitDto quantityUnit;
        public Inventory_UnitDto QuantityUnit
        {
            get { return quantityUnit; }
            set { this.SetField(p => p.QuantityUnit, ref quantityUnit, value); }
        }

        private Inventory_UnitDto priceUnit;
        public Inventory_UnitDto PriceUnit
        {
            get { return priceUnit; }
            set { this.SetField(p => p.PriceUnit, ref priceUnit, value); }
        }


        private Inventory_UnitDto mainCurrencyUnit;
        public Inventory_UnitDto MainCurrencyUnit
        {
            get { return mainCurrencyUnit; }
            set { this.SetField(p => p.MainCurrencyUnit, ref mainCurrencyUnit, value); }
        }

        private Inventory_UserDto userCreator;
        public Inventory_UserDto UserCreator
        {
            get { return userCreator; }
            set { this.SetField(p => p.UserCreator, ref userCreator, value); }
        }

        private string pricingReferenceNumber;

        public string PricingReferenceNumber
        {
            get { return pricingReferenceNumber; }
            set { this.SetField(p => p.PricingReferenceNumber, ref pricingReferenceNumber, value); }
        }

        private string pricingReferenceType;

        public string PricingReferenceType
        {
            get { return pricingReferenceType; }
            set { this.SetField(p => p.PricingReferenceType, ref pricingReferenceType, value); }
        }

    }
}
