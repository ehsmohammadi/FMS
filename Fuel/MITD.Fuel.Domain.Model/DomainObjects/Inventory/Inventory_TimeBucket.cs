using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_TimeBucket
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public DateTime? StartDate { get; set; } // StartDate
        public DateTime? EndDate { get; set; } // EndDate
        public int FinancialYearId { get; set; } // FinancialYearId
        public int UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate
        public bool? IsActive { get; set; } // IsActive

        // Foreign keys
        public virtual Inventory_FinancialYear Inventory_FinancialYear { get; set; } // FK_TimeBucket_FinancialYearId
        public virtual Inventory_User Inventory_User { get; set; } // FK_TimeBucket_UserCreatorId

        public virtual ICollection<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions.FK_Transaction_TimeBucketId
        public Inventory_TimeBucket()
        {
            StartDate = System.DateTime.Now;
            EndDate = System.DateTime.Now;
            CreateDate = System.DateTime.Now;
            IsActive = false;
            InitializePartial();
        }
        partial void InitializePartial();
    }
}