using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_Warehouse
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
        public long CompanyId { get; set; } // CompanyId
        public bool? IsActive { get; set; } // IsActive
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions.FK_Transaction_WarehouseId

        // Foreign keys
        public virtual Inventory_Company Inventory_Company { get; set; } // FK_Warehouse_CompanyId
        public virtual Inventory_User Inventory_User { get; set; } // FK_Warehouse_UserCreatorId

        public Inventory_Warehouse()
        {
            IsActive = true;
            CreateDate = System.DateTime.Now;
            Inventory_Transaction = new List<Inventory_Transaction>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}