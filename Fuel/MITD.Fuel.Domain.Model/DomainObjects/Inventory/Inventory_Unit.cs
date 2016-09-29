using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_Unit
    {
        public long Id { get; set; } // Id (Primary key)
        public string Abbreviation { get; set; } // Abbreviation
        public string Name { get; set; } // Name
        public bool? IsCurrency { get; set; } // IsCurrency
        public bool? IsBaseCurrency { get; set; } // IsBaseCurrency
        public bool IsActive { get; set; } // IsActive
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_Good> Inventory_Good { get; set; } // Goods.FK_Goods_MainUnitId
        public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_QuantityUnitId
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_PriceUnit { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_PriceUnitId
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_MainCurrencyUnit { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_MainUnitId
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_QuantityUnit { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_QuantityUnitId
        public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert_SubUnitId { get; set; } // UnitConverts.FK_UnitConverts_SubUnitId
        public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert_UnitId { get; set; } // UnitConverts.FK_UnitConverts_UnitId

        // Foreign keys
        public virtual Inventory_User Inventory_User { get; set; } // FK_Units_UserCreatorId

        public Inventory_Unit()
        {
            IsCurrency = false;
            IsBaseCurrency = false;
            IsActive = true;
            CreateDate = System.DateTime.Now;
            Inventory_Good = new List<Inventory_Good>();
            Inventory_TransactionItemPrice_PriceUnit = new List<Inventory_TransactionItemPrice>();
            Inventory_TransactionItemPrice_QuantityUnit = new List<Inventory_TransactionItemPrice>();
            Inventory_TransactionItem = new List<Inventory_TransactionItem>();
            Inventory_UnitConvert_SubUnitId = new List<Inventory_UnitConvert>();
            Inventory_UnitConvert_UnitId = new List<Inventory_UnitConvert>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}