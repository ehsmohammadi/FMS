using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_Good
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
        public bool IsActive { get; set; } // IsActive
        public long MainUnitId { get; set; } // MainUnitId
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_GoodId

        // Foreign keys
        public virtual Inventory_Unit Inventory_Unit { get; set; } // FK_Goods_MainUnitId
        public virtual Inventory_User Inventory_User { get; set; } // FK_Goods_UserCreatorId

        public Inventory_Good()
        {
            IsActive = true;
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItem = new List<Inventory_TransactionItem>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}