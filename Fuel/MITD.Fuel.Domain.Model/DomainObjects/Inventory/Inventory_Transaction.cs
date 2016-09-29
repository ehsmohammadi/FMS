using System;
using System.Collections.Generic;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_Transaction
    {
        public int Id { get; set; } // Id (Primary key)
        public byte Action { get; set; } // Action
        public decimal? Code { get; set; } // Code
        public string Description { get; set; } // Description
        public int? PricingReferenceId { get; set; } // PricingReferenceId
        public long WarehouseId { get; set; } // WarehouseId
        public int StoreTypesId { get; set; } // StoreTypesId
        public int TimeBucketId { get; set; } // TimeBucketId
        public byte? Status { get; set; } // Status
        public DateTime? RegistrationDate { get; set; } // RegistrationDate
        public int? SenderReciver { get; set; } // SenderReciver
        public string HardCopyNo { get; set; } // HardCopyNo
        public string ReferenceType { get; set; } // ReferenceType
        public string ReferenceNo { get; set; } // ReferenceNo
        public DateTime? ReferenceDate { get; set; } // ReferenceDate
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        public int? AdjustmentForTransactionId { get; set; } 

        // Reverse navigation
        public virtual ICollection<Inventory_Transaction> Inventory_TransactionPricedTransactions { get; set; } // Transactions.FK_Transaction_PricingReferenceId
        public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_TransactionId

        // Foreign keys
        public virtual Inventory_StoreType Inventory_StoreType { get; set; } // FK_Transaction_StoreTypesId
        public virtual Inventory_Transaction Inventory_TransactionPricingReference { get; set; } // FK_Transaction_PricingReferenceId
        public virtual Inventory_User Inventory_User { get; set; } // FK_Transaction_UserCreatorId
        public virtual Inventory_Warehouse Inventory_Warehouse { get; set; } // FK_Transaction_WarehouseId
        public virtual Inventory_TimeBucket Inventory_TimeBucket { get; set; } // FK_Inventory_TimeBucketId
        public virtual Inventory_Transaction Inventory_TransactionAdjustmentForTransaction { get; set; }
        public virtual ICollection<Inventory_Transaction> Inventory_TransactionAdjustments { get; set; } // Transactions.FK_Transaction_PricingReferenceId

        public Inventory_Transaction()
        {
            Status = 1;
            RegistrationDate = System.DateTime.Now;
            ReferenceDate = System.DateTime.Now;
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItem = new List<Inventory_TransactionItem>();
            Inventory_TransactionPricedTransactions = new List<Inventory_Transaction>();
            this.Inventory_TransactionAdjustments = new List<Inventory_Transaction>();
            InitializePartial();
        }
        partial void InitializePartial();
    }
}