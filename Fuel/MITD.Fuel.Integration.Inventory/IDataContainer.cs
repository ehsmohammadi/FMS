// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace MITD.Fuel.Data.EF.Context
{
    public interface IDataContainer : IDisposable
    {
        IDbSet<Company> Companies { get; set; } // Companies
        IDbSet<ErrorMessage> ErrorMessages { get; set; } // ErrorMessages
        IDbSet<FinancialYear> FinancialYears { get; set; } // FinancialYear
        IDbSet<Good> Goods { get; set; } // Goods
        IDbSet<Inventory_Arman> Inventory_Arman { get; set; } // Arman
        IDbSet<OperationReference> OperationReferences { get; set; } // OperationReference
        IDbSet<StoreType> StoreTypes { get; set; } // StoreTypes
        IDbSet<TimeBucket> TimeBuckets { get; set; } // TimeBucket
        IDbSet<Transaction> Transactions { get; set; } // Transactions
        IDbSet<TransactionItem> TransactionItems { get; set; } // TransactionItems
        IDbSet<TransactionItemPrice> TransactionItemPrices { get; set; } // TransactionItemPrices
        IDbSet<Unit> Units { get; set; } // Units
        IDbSet<UnitConvert> UnitConverts { get; set; } // UnitConverts
        IDbSet<User> Users { get; set; } // Users
        IDbSet<Warehouse> Warehouses { get; set; } // Warehouse

        int SaveChanges();
    }

}
