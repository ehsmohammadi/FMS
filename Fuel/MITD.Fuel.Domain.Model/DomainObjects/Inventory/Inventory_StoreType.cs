using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_StoreType
    {
        public int Id { get; set; } // Id (Primary key)
        public short Code { get; set; } // Code
        public byte Type { get; set; } // Type
        public bool IsAdjustment { get; set; }
        public string InputName { get; set; } // InputName
        public string OutputName { get; set; } // OutputName
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions.FK_Transaction_StoreTypesId

        // Foreign keys
        public virtual Inventory_User Inventory_User { get; set; } // FK_StoreTypes_UserCreatorId

        public Inventory_StoreType()
        {
            Type = 0;
            CreateDate = System.DateTime.Now;
            Inventory_Transaction = new List<Inventory_Transaction>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}