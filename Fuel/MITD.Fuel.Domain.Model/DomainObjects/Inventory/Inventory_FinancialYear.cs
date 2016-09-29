using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_FinancialYear
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public DateTime StartDate { get; set; } // StartDate
        public DateTime EndDate { get; set; } // EndDate
        public int UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_TimeBucket> Inventory_TimeBucket { get; set; } // TimeBucket.FK_TimeBucket_FinancialYearId

        // Foreign keys
        public virtual Inventory_User Inventory_User { get; set; } // FK_FinancialYear_UserCreatorId

        public Inventory_FinancialYear()
        {
            CreateDate = System.DateTime.Now;
            Inventory_TimeBucket = new List<Inventory_TimeBucket>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}