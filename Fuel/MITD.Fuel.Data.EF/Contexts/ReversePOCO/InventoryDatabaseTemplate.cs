

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Configuration file:     "Fuel\MITD.Fuel.Data.EF\App.config"
//     Connection String Name: "DataContainer"
//     Connection String:      "Data Source=ali;Initial Catalog=StorageSpace;Integrated Security=True"

using MITD.Fuel.Domain.Model.DomainObjects;
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
	// ************************************************************************
	// Unit of work
	public interface IDataContainer : IDisposable
	{
		IDbSet<Inventory_Company> Inventory_Company { get; set; } // Companies
		// IDbSet<Inventory_ErrorMessage> Inventory_ErrorMessage { get; set; } // ErrorMessages
		IDbSet<Inventory_FinancialYear> Inventory_FinancialYear { get; set; } // FinancialYear
		IDbSet<Inventory_Good> Inventory_Good { get; set; } // Goods
		IDbSet<Inventory_OperationReference> Inventory_OperationReference { get; set; } // OperationReference
		IDbSet<Inventory_StoreType> Inventory_StoreType { get; set; } // StoreTypes
		IDbSet<Inventory_TimeBucket> Inventory_TimeBucket { get; set; } // TimeBucket
		IDbSet<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions
		IDbSet<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems
		IDbSet<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; } // TransactionItemPrices
		IDbSet<Inventory_Unit> Inventory_Unit { get; set; } // Units
		IDbSet<Inventory_UnitConvert> Inventory_UnitConvert { get; set; } // UnitConverts
		IDbSet<Inventory_User> Inventory_User { get; set; } // Users
		IDbSet<Inventory_Warehouse> Inventory_Warehouse { get; set; } // Warehouse

		int SaveChanges();
	}

	// ************************************************************************
	// Database context
	public partial class DataContainer : DbContext, IDataContainer
	{
		public IDbSet<Inventory_Company> Inventory_Company { get; set; } // Companies
		// public IDbSet<Inventory_ErrorMessage> Inventory_ErrorMessage { get; set; } // ErrorMessages
		public IDbSet<Inventory_FinancialYear> Inventory_FinancialYear { get; set; } // FinancialYear
		public IDbSet<Inventory_Good> Inventory_Good { get; set; } // Goods
		public IDbSet<Inventory_OperationReference> Inventory_OperationReference { get; set; } // OperationReference
		public IDbSet<Inventory_StoreType> Inventory_StoreType { get; set; } // StoreTypes
		public IDbSet<Inventory_TimeBucket> Inventory_TimeBucket { get; set; } // TimeBucket
		public IDbSet<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions
		public IDbSet<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems
		public IDbSet<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; } // TransactionItemPrices
		public IDbSet<Inventory_Unit> Inventory_Unit { get; set; } // Units
		public IDbSet<Inventory_UnitConvert> Inventory_UnitConvert { get; set; } // UnitConverts
		public IDbSet<Inventory_User> Inventory_User { get; set; } // Users
		public IDbSet<Inventory_Warehouse> Inventory_Warehouse { get; set; } // Warehouse

		//Commented By Hatefi
		/*static DataContainer()
		{
			Database.SetInitializer<DataContainer>(null);
		}

		public DataContainer()
			: base("Name=DataContainer")
		{
        InitializePartial();
		}

		public DataContainer(string connectionString) : base(connectionString)
		{
        InitializePartial();
		}

		public DataContainer(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
		{
        InitializePartial();
		}*/

		protected void OnInventoryModelCreating(DbModelBuilder modelBuilder)
		//protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Configurations.Add(new Inventory_CompanyConfiguration());
            //modelBuilder.Configurations.Add(new Inventory_ErrorMessageConfiguration());
			modelBuilder.Configurations.Add(new Inventory_FinancialYearConfiguration());
			modelBuilder.Configurations.Add(new Inventory_GoodConfiguration());
			modelBuilder.Configurations.Add(new Inventory_OperationReferenceConfiguration());
			modelBuilder.Configurations.Add(new Inventory_StoreTypeConfiguration());
			modelBuilder.Configurations.Add(new Inventory_TimeBucketConfiguration());
			modelBuilder.Configurations.Add(new Inventory_TransactionConfiguration());
			modelBuilder.Configurations.Add(new Inventory_TransactionItemConfiguration());
			modelBuilder.Configurations.Add(new Inventory_TransactionItemPriceConfiguration());
			modelBuilder.Configurations.Add(new Inventory_UnitConfiguration());
			modelBuilder.Configurations.Add(new Inventory_UnitConvertConfiguration());
			modelBuilder.Configurations.Add(new Inventory_UserConfiguration());
			modelBuilder.Configurations.Add(new Inventory_WarehouseConfiguration());
        OnModelCreatingPartial(modelBuilder);
		}

		public static DbModelBuilder CreateInventoryModel(DbModelBuilder modelBuilder, string schema)
		//public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
		{
			modelBuilder.Configurations.Add(new Inventory_CompanyConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_ErrorMessageConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_FinancialYearConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_GoodConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_OperationReferenceConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_StoreTypeConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_TimeBucketConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_TransactionConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_TransactionItemConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_TransactionItemPriceConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_UnitConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_UnitConvertConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_UserConfiguration(schema));
			modelBuilder.Configurations.Add(new Inventory_WarehouseConfiguration(schema));
			return modelBuilder;
		}

		partial void InitializePartial();
		partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
	}

	// ************************************************************************
	// POCO classes

    //// Companies
    //public partial class Inventory_Company
    //{
    //    public long Id { get; set; } // Id (Primary key)
    //    public string Code { get; set; } // Code
    //    public string Name { get; set; } // Name
    //    public bool? IsActive { get; set; } // IsActive
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_Warehouse> Inventory_Warehouse { get; set; } // Warehouse.FK_Warehouse_CompanyId

    //    // Foreign keys
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_Companies_UserCreatorId

    //    public Inventory_Company()
    //    {
    //        IsActive = true;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_Warehouse = new List<Inventory_Warehouse>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// ErrorMessages
    //public partial class Inventory_ErrorMessage
    //{
    //    public string ErrorMessage { get; set; } // ErrorMessage (Primary key)
    //    public string TextMessage { get; set; } // TextMessage (Primary key)
    //    public string Action { get; set; } // Action (Primary key)
    //}

    //// FinancialYear
    //public partial class Inventory_FinancialYear
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public string Name { get; set; } // Name
    //    public DateTime StartDate { get; set; } // StartDate
    //    public DateTime EndDate { get; set; } // EndDate
    //    public int UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_TimeBucket> Inventory_TimeBucket { get; set; } // TimeBucket.FK_TimeBucket_FinancialYearId

    //    // Foreign keys
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_FinancialYear_UserCreatorId

    //    public Inventory_FinancialYear()
    //    {
    //        CreateDate = System.DateTime.Now;
    //        Inventory_TimeBucket = new List<Inventory_TimeBucket>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// Goods
    //public partial class Inventory_Good
    //{
    //    public long Id { get; set; } // Id (Primary key)
    //    public string Code { get; set; } // Code
    //    public string Name { get; set; } // Name
    //    public bool IsActive { get; set; } // IsActive
    //    public long MainUnitId { get; set; } // MainUnitId
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_GoodId

    //    // Foreign keys
    //    public virtual Inventory_Unit Inventory_Unit { get; set; } // FK_Goods_MainUnitId
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_Goods_UserCreatorId

    //    public Inventory_Good()
    //    {
    //        IsActive = true;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_TransactionItem = new List<Inventory_TransactionItem>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// OperationReference
    //public partial class Inventory_OperationReference
    //{
    //    public long Id { get; set; } // Id (Primary key)
    //    public long OperationId { get; set; } // OperationId
    //    public int OperationType { get; set; } // OperationType
    //    public string ReferenceType { get; set; } // ReferenceType
    //    public string ReferenceNumber { get; set; } // ReferenceNumber
    //    public DateTime RegistrationDate { get; set; } // RegistrationDate

    //    public Inventory_OperationReference()
    //    {
    //        RegistrationDate = System.DateTime.Now;
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// StoreTypes
    //public partial class Inventory_StoreType
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public short Code { get; set; } // Code
    //    public byte Type { get; set; } // Type
    //    public string InputName { get; set; } // InputName
    //    public string OutputName { get; set; } // OutputName
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions.FK_Transaction_StoreTypesId

    //    // Foreign keys
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_StoreTypes_UserCreatorId

    //    public Inventory_StoreType()
    //    {
    //        Type = 0;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_Transaction = new List<Inventory_Transaction>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// TimeBucket
    //public partial class Inventory_TimeBucket
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public string Name { get; set; } // Name
    //    public DateTime? StartDate { get; set; } // StartDate
    //    public DateTime? EndDate { get; set; } // EndDate
    //    public int FinancialYearId { get; set; } // FinancialYearId
    //    public int UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate
    //    public bool? IsActive { get; set; } // IsActive

    //    // Foreign keys
    //    public virtual Inventory_FinancialYear Inventory_FinancialYear { get; set; } // FK_TimeBucket_FinancialYearId
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_TimeBucket_UserCreatorId

    //    public Inventory_TimeBucket()
    //    {
    //        StartDate = System.DateTime.Now;
    //        EndDate = System.DateTime.Now;
    //        CreateDate = System.DateTime.Now;
    //        IsActive = false;
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// Transactions
    //public partial class Inventory_Transaction
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public byte Action { get; set; } // Action
    //    public decimal? Code { get; set; } // Code
    //    public string Description { get; set; } // Description
    //    public int? PricingReferenceId { get; set; } // PricingReferenceId
    //    public long WarehouseId { get; set; } // WarehouseId
    //    public int StoreTypesId { get; set; } // StoreTypesId
    //    public int TimeBucketId { get; set; } // TimeBucketId
    //    public byte? Status { get; set; } // Status
    //    public DateTime? RegistrationDate { get; set; } // RegistrationDate
    //    public int? SenderReciver { get; set; } // SenderReciver
    //    public string HardCopyNo { get; set; } // HardCopyNo
    //    public string ReferenceType { get; set; } // ReferenceType
    //    public string ReferenceNo { get; set; } // ReferenceNo
    //    public DateTime? ReferenceDate { get; set; } // ReferenceDate
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_Transaction> Inventory_Transaction2 { get; set; } // Transactions.FK_Transaction_PricingReferenceId
    //    public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_TransactionId

    //    // Foreign keys
    //    public virtual Inventory_StoreType Inventory_StoreType { get; set; } // FK_Transaction_StoreTypesId
    //    public virtual Inventory_Transaction Inventory_Transaction1 { get; set; } // FK_Transaction_PricingReferenceId
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_Transaction_UserCreatorId
    //    public virtual Inventory_Warehouse Inventory_Warehouse { get; set; } // FK_Transaction_WarehouseId

    //    public Inventory_Transaction()
    //    {
    //        Status = 1;
    //        RegistrationDate = System.DateTime.Now;
    //        ReferenceDate = System.DateTime.Now;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_TransactionItem = new List<Inventory_TransactionItem>();
    //        Inventory_Transaction2 = new List<Inventory_Transaction>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// TransactionItems
    //public partial class Inventory_TransactionItem
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public short RowVersion { get; set; } // RowVersion
    //    public int TransactionId { get; set; } // TransactionId
    //    public long GoodId { get; set; } // GoodId
    //    public long QuantityUnitId { get; set; } // QuantityUnitId
    //    public decimal? QuantityAmount { get; set; } // QuantityAmount
    //    public string Description { get; set; } // Description
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_TransactionItemsId

    //    // Foreign keys
    //    public virtual Inventory_Good Inventory_Good { get; set; } // FK_TransactionItems_GoodId
    //    public virtual Inventory_Transaction Inventory_Transaction { get; set; } // FK_TransactionItems_TransactionId
    //    public virtual Inventory_Unit Inventory_Unit { get; set; } // FK_TransactionItems_QuantityUnitId
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_TransactionItems_UserCreatorId

    //    public Inventory_TransactionItem()
    //    {
    //        QuantityAmount = 0m;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_TransactionItemPrice = new List<Inventory_TransactionItemPrice>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// TransactionItemPrices
    //public partial class Inventory_TransactionItemPrice
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public short RowVersion { get; set; } // RowVersion
    //    public int TransactionId { get; set; } // TransactionId
    //    public int TransactionItemId { get; set; } // TransactionItemId
    //    public string Description { get; set; } // Description
    //    public long QuantityUnitId { get; set; } // QuantityUnitId
    //    public decimal? QuantityAmount { get; set; } // QuantityAmount
    //    public long PriceUnitId { get; set; } // PriceUnitId
    //    public decimal? Fee { get; set; } // Fee
    //    public long MainCurrencyUnitId { get; set; } // MainCurrencyUnitId
    //    public decimal? FeeInMainCurrency { get; set; } // FeeInMainCurrency
    //    public DateTime? RegistrationDate { get; set; } // RegistrationDate
    //    public decimal? QuantityAmountUseFifo { get; set; } // QuantityAmountUseFIFO
    //    public int? TransactionReferenceId { get; set; } // TransactionReferenceId
    //    public string IssueReferenceIds { get; set; } // IssueReferenceIds
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate
    //    public string OtherSystemReferenceNo { get; set; } // OtherSystemReferenceNo

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice2 { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_TransactionReferenceId

    //    // Foreign keys
    //    public virtual Inventory_TransactionItem Inventory_TransactionItem { get; set; } // FK_TransactionItemPrices_TransactionItemsId
    //    public virtual Inventory_TransactionItemPrice Inventory_TransactionItemPrice1 { get; set; } // FK_TransactionItemPrices_TransactionReferenceId
    //    public virtual Inventory_Unit Inventory_Unit_MainCurrencyUnitId { get; set; } // FK_TransactionItemPrices_MainCurrencyUnitId
    //    public virtual Inventory_Unit Inventory_Unit_PriceUnitId { get; set; } // FK_TransactionItemPrices_PriceUnitId
    //    public virtual Inventory_Unit Inventory_Unit_QuantityUnitId { get; set; } // FK_TransactionItemPrices_QuantityUnitId
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_TransactionItemPrices_UserCreatorId

    //    public Inventory_TransactionItemPrice()
    //    {
    //        QuantityAmount = 0m;
    //        Fee = 0m;
    //        FeeInMainCurrency = 0m;
    //        RegistrationDate = System.DateTime.Now;
    //        QuantityAmountUseFifo = 0m;
    //        IssueReferenceIds = "N''";
    //        CreateDate = System.DateTime.Now;
    //        Inventory_TransactionItemPrice2 = new List<Inventory_TransactionItemPrice>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// Units
    //public partial class Inventory_Unit
    //{
    //    public long Id { get; set; } // Id (Primary key)
    //    public string Abbreviation { get; set; } // Abbreviation
    //    public string Name { get; set; } // Name
    //    public bool? IsCurrency { get; set; } // IsCurrency
    //    public bool? IsBaseCurrency { get; set; } // IsBaseCurrency
    //    public bool IsActive { get; set; } // IsActive
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_Good> Inventory_Good { get; set; } // Goods.FK_Goods_MainUnitId
    //    public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_QuantityUnitId
    //    public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_MainCurrencyUnitId { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_MainCurrencyUnitId
    //    public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_PriceUnitId { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_PriceUnitId
    //    public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_QuantityUnitId { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_QuantityUnitId
    //    public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert_SubUnitId { get; set; } // UnitConverts.FK_UnitConverts_SubUnitId
    //    public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert_UnitId { get; set; } // UnitConverts.FK_UnitConverts_UnitId

    //    // Foreign keys
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_Units_UserCreatorId

    //    public Inventory_Unit()
    //    {
    //        IsCurrency = false;
    //        IsBaseCurrency = false;
    //        IsActive = true;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_Good = new List<Inventory_Good>();
    //        Inventory_TransactionItemPrice_MainCurrencyUnitId = new List<Inventory_TransactionItemPrice>();
    //        Inventory_TransactionItemPrice_PriceUnitId = new List<Inventory_TransactionItemPrice>();
    //        Inventory_TransactionItemPrice_QuantityUnitId = new List<Inventory_TransactionItemPrice>();
    //        Inventory_TransactionItem = new List<Inventory_TransactionItem>();
    //        Inventory_UnitConvert_SubUnitId = new List<Inventory_UnitConvert>();
    //        Inventory_UnitConvert_UnitId = new List<Inventory_UnitConvert>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// UnitConverts
    //public partial class Inventory_UnitConvert
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public long UnitId { get; set; } // UnitId
    //    public long SubUnitId { get; set; } // SubUnitId
    //    public decimal Coefficient { get; set; } // Coefficient
    //    public DateTime? EffectiveDateStart { get; set; } // EffectiveDateStart
    //    public DateTime? EffectiveDateEnd { get; set; } // EffectiveDateEnd
    //    public short RowVersion { get; set; } // RowVersion
    //    public int UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Foreign keys
    //    public virtual Inventory_Unit Inventory_Unit_SubUnitId { get; set; } // FK_UnitConverts_SubUnitId
    //    public virtual Inventory_Unit Inventory_Unit_UnitId { get; set; } // FK_UnitConverts_UnitId
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_UnitConverts_UserCreatorId

    //    public Inventory_UnitConvert()
    //    {
    //        EffectiveDateStart = System.DateTime.Now;
    //        CreateDate = System.DateTime.Now;
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// Users
    //public partial class Inventory_User
    //{
    //    public int Id { get; set; } // Id (Primary key)
    //    public int Code { get; set; } // Code
    //    public string Name { get; set; } // Name
    //    public string UserName { get; set; } // User_Name
    //    public string Password { get; set; } // Password
    //    public bool? IsActive { get; set; } // IsActive
    //    public string EmailAddress { get; set; } // Email_Address
    //    public string IpAddress { get; set; } // IPAddress
    //    public bool? Login { get; set; } // Login
    //    public string SessionId { get; set; } // SessionId
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_Company> Inventory_Company { get; set; } // Companies.FK_Companies_UserCreatorId
    //    public virtual ICollection<Inventory_FinancialYear> Inventory_FinancialYear { get; set; } // FinancialYear.FK_FinancialYear_UserCreatorId
    //    public virtual ICollection<Inventory_Good> Inventory_Good { get; set; } // Goods.FK_Goods_UserCreatorId
    //    public virtual ICollection<Inventory_StoreType> Inventory_StoreType { get; set; } // StoreTypes.FK_StoreTypes_UserCreatorId
    //    public virtual ICollection<Inventory_TimeBucket> Inventory_TimeBucket { get; set; } // TimeBucket.FK_TimeBucket_UserCreatorId
    //    public virtual ICollection<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions.FK_Transaction_UserCreatorId
    //    public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_UserCreatorId
    //    public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_UserCreatorId
    //    public virtual ICollection<Inventory_Unit> Inventory_Unit { get; set; } // Units.FK_Units_UserCreatorId
    //    public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert { get; set; } // UnitConverts.FK_UnitConverts_UserCreatorId
    //    public virtual ICollection<Inventory_User> Inventory_User2 { get; set; } // Users.FK_Users_UserCreatorId
    //    public virtual ICollection<Inventory_Warehouse> Inventory_Warehouse { get; set; } // Warehouse.FK_Warehouse_UserCreatorId

    //    // Foreign keys
    //    public virtual Inventory_User Inventory_User1 { get; set; } // FK_Users_UserCreatorId

    //    public Inventory_User()
    //    {
    //        IsActive = true;
    //        Login = false;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_Company = new List<Inventory_Company>();
    //        Inventory_FinancialYear = new List<Inventory_FinancialYear>();
    //        Inventory_Good = new List<Inventory_Good>();
    //        Inventory_StoreType = new List<Inventory_StoreType>();
    //        Inventory_TimeBucket = new List<Inventory_TimeBucket>();
    //        Inventory_TransactionItemPrice = new List<Inventory_TransactionItemPrice>();
    //        Inventory_TransactionItem = new List<Inventory_TransactionItem>();
    //        Inventory_Transaction = new List<Inventory_Transaction>();
    //        Inventory_UnitConvert = new List<Inventory_UnitConvert>();
    //        Inventory_Unit = new List<Inventory_Unit>();
    //        Inventory_User2 = new List<Inventory_User>();
    //        Inventory_Warehouse = new List<Inventory_Warehouse>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}

    //// Warehouse
    //public partial class Inventory_Warehouse
    //{
    //    public long Id { get; set; } // Id (Primary key)
    //    public string Code { get; set; } // Code
    //    public string Name { get; set; } // Name
    //    public long CompanyId { get; set; } // CompanyId
    //    public bool? IsActive { get; set; } // IsActive
    //    public int? UserCreatorId { get; set; } // UserCreatorId
    //    public DateTime? CreateDate { get; set; } // CreateDate

    //    // Reverse navigation
    //    public virtual ICollection<Inventory_Transaction> Inventory_Transaction { get; set; } // Transactions.FK_Transaction_WarehouseId

    //    // Foreign keys
    //    public virtual Inventory_Company Inventory_Company { get; set; } // FK_Warehouse_CompanyId
    //    public virtual Inventory_User Inventory_User { get; set; } // FK_Warehouse_UserCreatorId

    //    public Inventory_Warehouse()
    //    {
    //        IsActive = true;
    //        CreateDate = System.DateTime.Now;
    //        Inventory_Transaction = new List<Inventory_Transaction>();
    //        InitializePartial();
    //    }
    //    partial void InitializePartial();
    //}


	// ************************************************************************
	// POCO Configuration

    // Companies
	internal partial class Inventory_CompanyConfiguration : EntityTypeConfiguration<Inventory_Company>
	{
		public Inventory_CompanyConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".Companies");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(256);
			Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
			Property(x => x.IsActive).HasColumnName("IsActive").IsOptional();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Company).HasForeignKey(c => c.UserCreatorId); // FK_Companies_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // ErrorMessages
	internal partial class Inventory_ErrorMessageConfiguration : EntityTypeConfiguration<Inventory_ErrorMessage>
	{
		public Inventory_ErrorMessageConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".ErrorMessages");
 		   HasKey(x => new { x.ErrorMessage, x.TextMessage, x.Action });

			Property(x => x.ErrorMessage).HasColumnName("ErrorMessage").IsRequired().HasMaxLength(200).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(x => x.TextMessage).HasColumnName("TextMessage").IsRequired().HasMaxLength(200).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(20).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // FinancialYear
	internal partial class Inventory_FinancialYearConfiguration : EntityTypeConfiguration<Inventory_FinancialYear>
	{
		public Inventory_FinancialYearConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".FinancialYear");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
			Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
			Property(x => x.EndDate).HasColumnName("EndDate").IsRequired();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsRequired();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasRequired(a => a.Inventory_User).WithMany(b => b.Inventory_FinancialYear).HasForeignKey(c => c.UserCreatorId); // FK_FinancialYear_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // Goods
	internal partial class Inventory_GoodConfiguration : EntityTypeConfiguration<Inventory_Good>
	{
		public Inventory_GoodConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".Goods");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(100);
			Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
			Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
			Property(x => x.MainUnitId).HasColumnName("MainUnitId").IsRequired();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasRequired(a => a.Inventory_Unit).WithMany(b => b.Inventory_Good).HasForeignKey(c => c.MainUnitId).WillCascadeOnDelete(false); // FK_Goods_MainUnitId
			HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Good).HasForeignKey(c => c.UserCreatorId); // FK_Goods_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // OperationReference
	internal partial class Inventory_OperationReferenceConfiguration : EntityTypeConfiguration<Inventory_OperationReference>
	{
		public Inventory_OperationReferenceConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".OperationReference");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.OperationId).HasColumnName("OperationId").IsRequired();
			Property(x => x.OperationType).HasColumnName("OperationType").IsRequired();
			Property(x => x.ReferenceType).HasColumnName("ReferenceType").IsRequired().HasMaxLength(256);
			Property(x => x.ReferenceNumber).HasColumnName("ReferenceNumber").IsRequired().HasMaxLength(256);
			Property(x => x.RegistrationDate).HasColumnName("RegistrationDate").IsRequired();
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // StoreTypes
	internal partial class Inventory_StoreTypeConfiguration : EntityTypeConfiguration<Inventory_StoreType>
	{
		public Inventory_StoreTypeConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".StoreTypes");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.Code).HasColumnName("Code").IsRequired();
			Property(x => x.Type).HasColumnName("Type").IsRequired();
			Property(x => x.InputName).HasColumnName("InputName").IsOptional().HasMaxLength(100);
			Property(x => x.OutputName).HasColumnName("OutputName").IsOptional().HasMaxLength(100);
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_StoreType).HasForeignKey(c => c.UserCreatorId); // FK_StoreTypes_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // TimeBucket
	internal partial class Inventory_TimeBucketConfiguration : EntityTypeConfiguration<Inventory_TimeBucket>
	{
		public Inventory_TimeBucketConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".TimeBucket");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
			Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
			Property(x => x.EndDate).HasColumnName("EndDate").IsOptional();
			Property(x => x.FinancialYearId).HasColumnName("FinancialYearId").IsRequired();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsRequired();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();
			Property(x => x.IsActive).HasColumnName("IsActive").IsOptional();

            // Foreign keys
			HasRequired(a => a.Inventory_FinancialYear).WithMany(b => b.Inventory_TimeBucket).HasForeignKey(c => c.FinancialYearId); // FK_TimeBucket_FinancialYearId
			HasRequired(a => a.Inventory_User).WithMany(b => b.Inventory_TimeBucket).HasForeignKey(c => c.UserCreatorId).WillCascadeOnDelete(false); // FK_TimeBucket_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // Transactions
	internal partial class Inventory_TransactionConfiguration : EntityTypeConfiguration<Inventory_Transaction>
	{
		public Inventory_TransactionConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".Transactions");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.Action).HasColumnName("Action").IsRequired();
            Property(x => x.Code).HasColumnName("Code").IsOptional().HasPrecision(20, 2);
			Property(x => x.Description).HasColumnName("Description").IsOptional();
			Property(x => x.PricingReferenceId).HasColumnName("PricingReferenceId").IsOptional();
			Property(x => x.WarehouseId).HasColumnName("WarehouseId").IsRequired();
			Property(x => x.StoreTypesId).HasColumnName("StoreTypesId").IsRequired();
			Property(x => x.TimeBucketId).HasColumnName("TimeBucketId").IsRequired();
			Property(x => x.Status).HasColumnName("Status").IsOptional();
			Property(x => x.RegistrationDate).HasColumnName("RegistrationDate").IsOptional();
			Property(x => x.SenderReciver).HasColumnName("SenderReciver").IsOptional();
			Property(x => x.HardCopyNo).HasColumnName("HardCopyNo").IsOptional().HasMaxLength(10);
			Property(x => x.ReferenceType).HasColumnName("ReferenceType").IsOptional().HasMaxLength(100);
			Property(x => x.ReferenceNo).HasColumnName("ReferenceNo").IsOptional().HasMaxLength(100);
			Property(x => x.ReferenceDate).HasColumnName("ReferenceDate").IsOptional();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();
		    Property(x => x.AdjustmentForTransactionId);

            // Foreign keys
			HasOptional(a => a.Inventory_TransactionPricingReference).WithMany(b => b.Inventory_TransactionPricedTransactions).HasForeignKey(c => c.PricingReferenceId); // FK_Transaction_PricingReferenceId
			HasRequired(a => a.Inventory_Warehouse).WithMany(b => b.Inventory_Transaction).HasForeignKey(c => c.WarehouseId); // FK_Transaction_WarehouseId
			HasRequired(a => a.Inventory_StoreType).WithMany(b => b.Inventory_Transaction).HasForeignKey(c => c.StoreTypesId); // FK_Transaction_StoreTypesId
            HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Transaction).HasForeignKey(c => c.UserCreatorId); // FK_Transaction_UserCreatorId
            HasRequired(a => a.Inventory_TimeBucket).WithMany(b => b.Inventory_Transaction).HasForeignKey(c => c.TimeBucketId); // 
		    HasOptional(p => p.Inventory_TransactionAdjustmentForTransaction).WithMany(p => p.Inventory_TransactionAdjustments).HasForeignKey(p => p.AdjustmentForTransactionId);
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // TransactionItems
	internal partial class Inventory_TransactionItemConfiguration : EntityTypeConfiguration<Inventory_TransactionItem>
	{
		public Inventory_TransactionItemConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".TransactionItems");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired();
			Property(x => x.TransactionId).HasColumnName("TransactionId").IsRequired();
			Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
			Property(x => x.QuantityUnitId).HasColumnName("QuantityUnitId").IsRequired();
            Property(x => x.QuantityAmount).HasColumnName("QuantityAmount").IsOptional().HasPrecision(20, 3);
			Property(x => x.Description).HasColumnName("Description").IsOptional();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasRequired(a => a.Inventory_Transaction).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.TransactionId); // FK_TransactionItems_TransactionId
			HasRequired(a => a.Inventory_Good).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.GoodId); // FK_TransactionItems_GoodId
            HasRequired(a => a.Inventory_Unit).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.QuantityUnitId).WillCascadeOnDelete(false); // FK_TransactionItems_QuantityUnitId
			HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.UserCreatorId); // FK_TransactionItems_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // TransactionItemPrices
	internal partial class Inventory_TransactionItemPriceConfiguration : EntityTypeConfiguration<Inventory_TransactionItemPrice>
	{
		public Inventory_TransactionItemPriceConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".TransactionItemPrices");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired();
			Property(x => x.TransactionId).HasColumnName("TransactionId").IsRequired();
			Property(x => x.TransactionItemId).HasColumnName("TransactionItemId").IsRequired();
			Property(x => x.Description).HasColumnName("Description").IsOptional();
			Property(x => x.QuantityUnitId).HasColumnName("QuantityUnitId").IsRequired();
            Property(x => x.QuantityAmount).HasColumnName("QuantityAmount").IsOptional().HasPrecision(20, 3);
			Property(x => x.PriceUnitId).HasColumnName("PriceUnitId").IsRequired();
            Property(x => x.Fee).HasColumnName("Fee").IsOptional().HasPrecision(20, 3);
			Property(x => x.MainCurrencyUnitId).HasColumnName("MainCurrencyUnitId").IsRequired();
            Property(x => x.FeeInMainCurrency).HasColumnName("FeeInMainCurrency").IsOptional().HasPrecision(20, 3);
			Property(x => x.RegistrationDate).HasColumnName("RegistrationDate").IsOptional();
            Property(x => x.QuantityAmountUseFifo).HasColumnName("QuantityAmountUseFIFO").IsOptional().HasPrecision(20, 3);
			Property(x => x.TransactionReferenceId).HasColumnName("TransactionReferenceId").IsOptional();
			Property(x => x.IssueReferenceIds).HasColumnName("IssueReferenceIds").IsOptional();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();
	

            // Foreign keys
			HasRequired(a => a.Inventory_TransactionItem).WithMany(b => b.Inventory_TransactionItemPrice).HasForeignKey(c => c.TransactionItemId); // FK_TransactionItemPrices_TransactionItemsId
            HasRequired(a => a.Inventory_Unit_QuantityUnit).WithMany(b => b.Inventory_TransactionItemPrice_QuantityUnit).HasForeignKey(c => c.QuantityUnitId).WillCascadeOnDelete(false); // FK_TransactionItemPrices_QuantityUnitId
            HasRequired(a => a.Inventory_Unit_PriceUnit).WithMany(b => b.Inventory_TransactionItemPrice_PriceUnit).HasForeignKey(c => c.PriceUnitId).WillCascadeOnDelete(false); // FK_TransactionItemPrices_PriceUnitId
            HasRequired(a => a.Inventory_Unit_MainCurrencyUnit).WithMany(b => b.Inventory_TransactionItemPrice_MainCurrencyUnit).HasForeignKey(c => c.MainCurrencyUnitId).WillCascadeOnDelete(false); // FK_TransactionItemPrices_MainCurrencyUnitId
			HasOptional(a => a.Inventory_TransactionItemPriceFIFOReference).WithMany(b => b.Inventory_TransactionItemPriceFIFODestination).HasForeignKey(c => c.TransactionReferenceId); // FK_TransactionItemPrices_TransactionReferenceId
			HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_TransactionItemPrice).HasForeignKey(c => c.UserCreatorId); // FK_TransactionItemPrices_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // Units
	internal partial class Inventory_UnitConfiguration : EntityTypeConfiguration<Inventory_Unit>
	{
		public Inventory_UnitConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".Units");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.Abbreviation).HasColumnName("Abbreviation").IsRequired().HasMaxLength(256);
			Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
			Property(x => x.IsCurrency).HasColumnName("IsCurrency").IsOptional();
			Property(x => x.IsBaseCurrency).HasColumnName("IsBaseCurrency").IsOptional();
			Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Unit).HasForeignKey(c => c.UserCreatorId); // FK_Units_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // UnitConverts
	internal partial class Inventory_UnitConvertConfiguration : EntityTypeConfiguration<Inventory_UnitConvert>
	{
		public Inventory_UnitConvertConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".UnitConverts");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.UnitId).HasColumnName("UnitId").IsRequired();
			Property(x => x.SubUnitId).HasColumnName("SubUnitId").IsRequired();
            Property(x => x.Coefficient).HasColumnName("Coefficient").IsRequired().HasPrecision(18, 3);
			Property(x => x.EffectiveDateStart).HasColumnName("EffectiveDateStart").IsOptional();
			Property(x => x.EffectiveDateEnd).HasColumnName("EffectiveDateEnd").IsOptional();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsRequired();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
            HasRequired(a => a.Inventory_Unit_UnitId).WithMany(b => b.Inventory_UnitConvert_UnitId).HasForeignKey(c => c.UnitId).WillCascadeOnDelete(false); // FK_UnitConverts_UnitId
            HasRequired(a => a.Inventory_Unit_SubUnitId).WithMany(b => b.Inventory_UnitConvert_SubUnitId).HasForeignKey(c => c.SubUnitId).WillCascadeOnDelete(false); // FK_UnitConverts_SubUnitId
			HasRequired(a => a.Inventory_User).WithMany(b => b.Inventory_UnitConvert).HasForeignKey(c => c.UserCreatorId); // FK_UnitConverts_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // Users
	internal partial class Inventory_UserConfiguration : EntityTypeConfiguration<Inventory_User>
	{
		public Inventory_UserConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".Users");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(x => x.Code).HasColumnName("Code").IsRequired();
			Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
			Property(x => x.UserName).HasColumnName("User_Name").IsRequired().HasMaxLength(256);
			Property(x => x.Password).HasColumnName("Password").IsRequired().HasMaxLength(100);
			Property(x => x.IsActive).HasColumnName("IsActive").IsOptional();
			Property(x => x.EmailAddress).HasColumnName("Email_Address").IsOptional().HasMaxLength(256);
			Property(x => x.IpAddress).HasColumnName("IPAddress").IsOptional().HasMaxLength(15);
			Property(x => x.Login).HasColumnName("Login").IsOptional();
			Property(x => x.SessionId).HasColumnName("SessionId").IsOptional().HasMaxLength(88);
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasOptional(a => a.Inventory_User1).WithMany(b => b.Inventory_User2).HasForeignKey(c => c.UserCreatorId); // FK_Users_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }

    // Warehouse
	internal partial class Inventory_WarehouseConfiguration : EntityTypeConfiguration<Inventory_Warehouse>
	{
		public Inventory_WarehouseConfiguration(string schema = "Inventory")
		{
 		   ToTable(schema + ".Warehouse");
 		   HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(128);
			Property(x => x.Name).HasColumnName("Name").IsOptional().HasMaxLength(256);
			Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
			Property(x => x.IsActive).HasColumnName("IsActive").IsOptional();
			Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
			Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
			HasRequired(a => a.Inventory_Company).WithMany(b => b.Inventory_Warehouse).HasForeignKey(c => c.CompanyId); // FK_Warehouse_CompanyId
			HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Warehouse).HasForeignKey(c => c.UserCreatorId); // FK_Warehouse_UserCreatorId
			InitializePartial();
        }
		partial void InitializePartial();
    }




}

