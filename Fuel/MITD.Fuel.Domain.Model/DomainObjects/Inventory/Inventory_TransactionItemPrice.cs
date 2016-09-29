using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_TransactionItemPrice
    {
        public int Id { get; set; } // Id (Primary key)
        public short RowVersion { get; set; } // RowVersion
        public int TransactionId { get; set; } // TransactionId
        public int TransactionItemId { get; set; } // TransactionItemId
        public string Description { get; set; } // Description
        public long QuantityUnitId { get; set; } // QuantityUnitId
        public decimal? QuantityAmount { get; set; } // QuantityAmount
        public long PriceUnitId { get; set; } // PriceUnitId
        public decimal? Fee { get; set; } // Fee
        public long MainCurrencyUnitId { get; set; } // MainCurrencyUnitId
        public decimal? FeeInMainCurrency { get; set; } // FeeInMainCurrency
        public DateTime? RegistrationDate { get; set; } // RegistrationDate
        public decimal? QuantityAmountUseFifo { get; set; } // QuantityAmountUseFIFO
        public int? TransactionReferenceId { get; set; } // TransactionReferenceId
        public string IssueReferenceIds { get; set; } // IssueReferenceIds
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate
     //   public string OtherSystemReferenceNo { get; set; }

        // Reverse navigation
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPriceFIFODestination { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_TransactionReferenceId

        // Foreign keys
        public virtual Inventory_TransactionItem Inventory_TransactionItem { get; set; } // FK_TransactionItemPrices_TransactionItemsId
        public virtual Inventory_TransactionItemPrice Inventory_TransactionItemPriceFIFOReference { get; set; } // FK_TransactionItemPrices_TransactionReferenceId
        public virtual Inventory_Unit Inventory_Unit_PriceUnit { get; set; } // FK_TransactionItemPrices_PriceUnitId
        public virtual Inventory_Unit Inventory_Unit_MainCurrencyUnit { get; set; } // FK_TransactionItemPrices_MainCurrencyUnit
        public virtual Inventory_Unit Inventory_Unit_QuantityUnit { get; set; } // FK_TransactionItemPrices_QuantityUnitId
        public virtual Inventory_User Inventory_User { get; set; } // FK_TransactionItemPrices_UserCreatorId

        public Inventory_TransactionItemPrice()
        {
            QuantityAmount = 0m;
            Fee = 0m;
            FeeInMainCurrency = 0m;
            RegistrationDate = System.DateTime.Now;
            QuantityAmountUseFifo = 0m;
            IssueReferenceIds = "N''";
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItemPriceFIFODestination = new List<Inventory_TransactionItemPrice>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}