using System;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_UnitConvert
    {
        public int Id { get; set; } // Id (Primary key)
        public long UnitId { get; set; } // UnitId
        public long SubUnitId { get; set; } // SubUnitId
        public decimal Coefficient { get; set; } // Coefficient
        public DateTime? EffectiveDateStart { get; set; } // EffectiveDateStart
        public DateTime? EffectiveDateEnd { get; set; } // EffectiveDateEnd
        public int UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Foreign keys
        public virtual Inventory_Unit Inventory_Unit_SubUnitId { get; set; } // FK_UnitConverts_SubUnitId
        public virtual Inventory_Unit Inventory_Unit_UnitId { get; set; } // FK_UnitConverts_UnitId
        public virtual Inventory_User Inventory_User { get; set; } // FK_UnitConverts_UserCreatorId

        public Inventory_UnitConvert()
        {
            EffectiveDateStart = System.DateTime.Now;
            CreateDate = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }
}