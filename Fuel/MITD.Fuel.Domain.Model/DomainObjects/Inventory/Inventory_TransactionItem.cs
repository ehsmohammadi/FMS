using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_TransactionItem
    {
        public int Id { get; set; } // Id (Primary key)
        public short RowVersion { get; set; } // RowVersion
        public int TransactionId { get; set; } // TransactionId
        public long GoodId { get; set; } // GoodId
        public long QuantityUnitId { get; set; } // QuantityUnitId
        public decimal? QuantityAmount { get; set; } // QuantityAmount
        public string Description { get; set; } // Description
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_TransactionItemsId

        // Foreign keys
        public virtual Inventory_Good Inventory_Good { get; set; } // FK_TransactionItems_GoodId
        public virtual Inventory_Transaction Inventory_Transaction { get; set; } // FK_TransactionItems_TransactionId
        public virtual Inventory_Unit Inventory_Unit { get; set; } // FK_TransactionItems_QuantityUnitId
        public virtual Inventory_User Inventory_User { get; set; } // FK_TransactionItems_UserCreatorId

        public Inventory_TransactionItem()
        {
            QuantityAmount = 0m;
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItemPrice = new List<Inventory_TransactionItemPrice>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}