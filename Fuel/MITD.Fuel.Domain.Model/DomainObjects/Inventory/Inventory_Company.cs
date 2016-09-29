using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_Company
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
        public bool? IsActive { get; set; } // IsActive
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_Warehouse> Inventory_Warehouse { get; set; } // Warehouse.FK_Warehouse_CompanyId

        // Foreign keys
        public virtual Inventory_User Inventory_User { get; set; } // FK_Companies_UserCreatorId

        public Inventory_Company()
        {
            IsActive = true;
            CreateDate = System.DateTime.Now;
            Inventory_Warehouse = new List<Inventory_Warehouse>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}