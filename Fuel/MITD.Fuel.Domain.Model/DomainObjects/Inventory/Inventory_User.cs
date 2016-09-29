using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Fuel.Domain.Model.DomainObjects.Factories;
using MITD.Fuel.Domain.Model.DomainObjects.ScrapStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.Exceptions;
using MITD.Fuel.Domain.Model.IDomainServices;
using MITD.Fuel.Domain.Model.IDomainServices.Events.InventoryOperations;
using MITD.Fuel.Domain.Model.Specifications;

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    [Serializable]
    public partial class Inventory_User
    {
        public int Id { get; set; } // Id (Primary key)
        public int Code { get; set; } // Code
        public string Name { get; set; } // Name
        public string UserName { get; set; } // User_Name
        public string Password { get; set; } // Password
        public bool? IsActive { get; set; } // IsActive
        public string EmailAddress { get; set; } // Email_Address
        public string IpAddress { get; set; } // IPAddress
        public bool? Login { get; set; } // Login
        public string SessionId { get; set; } // SessionId
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_Company> Inventory_Company { get; set; } // Companies.FK_Companies_UserCreatorId
        public virtual ICollection<Inventory_FinancialYear> Inventory_FinancialYear { get; set; } // FinancialYear.FK_FinancialYear_UserCreatorId
        public virtual ICollection<Inventory_Good> Inventory_Good { get; set; } // Goods.FK_Goods_UserCreatorId
        public virtual ICollection<Inventory_StoreType> Inventory_StoreType { get; set; } // StoreTypes.FK_StoreTypes_UserCreatorId
        public virtual ICollection<Inventory_TimeBucket> Inventory_TimeBucket { get; set; } // TimeBucket.FK_TimeBucket_UserCreatorId
        public virtual ICollection<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions.FK_Transaction_UserCreatorId
        public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_UserCreatorId
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_UserCreatorId
        public virtual ICollection<Inventory_Unit> Inventory_Unit { get; set; } // Units.FK_Units_UserCreatorId
        public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert { get; set; } // UnitConverts.FK_UnitConverts_UserCreatorId
        public virtual ICollection<Inventory_User> Inventory_User2 { get; set; } // Users.FK_Users_UserCreatorId
        public virtual ICollection<Inventory_Warehouse> Inventory_Warehouse { get; set; } // Warehouse.FK_Warehouse_UserCreatorId

        // Foreign keys
        public virtual Inventory_User Inventory_User1 { get; set; } // FK_Users_UserCreatorId

        public Inventory_User()
        {
            IsActive = true;
            Login = false;
            CreateDate = System.DateTime.Now;
            Inventory_Company = new List<Inventory_Company>();
            Inventory_FinancialYear = new List<Inventory_FinancialYear>();
            Inventory_Good = new List<Inventory_Good>();
            Inventory_StoreType = new List<Inventory_StoreType>();
            Inventory_TimeBucket = new List<Inventory_TimeBucket>();
            Inventory_TransactionItemPrice = new List<Inventory_TransactionItemPrice>();
            Inventory_TransactionItem = new List<Inventory_TransactionItem>();
            Inventory_Transaction = new List<Inventory_Transaction>();
            Inventory_UnitConvert = new List<Inventory_UnitConvert>();
            Inventory_Unit = new List<Inventory_Unit>();
            Inventory_User2 = new List<Inventory_User>();
            Inventory_Warehouse = new List<Inventory_Warehouse>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
