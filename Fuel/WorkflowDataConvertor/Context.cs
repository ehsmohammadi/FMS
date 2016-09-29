

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Configuration file:     "WorkflowDataConvertor\App.config"
//     Connection String Name: "MyDbContext"
//     Connection String:      "Data Source=.\sqlexpress;Initial Catalog=StorageSpace_WF_Test;Integrated Security=True"

// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.ModelConfiguration;
using System.Threading;
using System.Threading.Tasks;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace WorkflowDataConvertor
{
    // ************************************************************************
    // Unit of work
    internal interface IMyDbContext : IDisposable
    {
        IDbSet<ActionType> ActionTypes { get; set; } // ActionTypes
        IDbSet<BasicInfo_AccountListView> BasicInfo_AccountListView { get; set; } // AccountListView
        IDbSet<BasicInfo_ActivityLocation> BasicInfo_ActivityLocation { get; set; } // ActivityLocation
        IDbSet<BasicInfo_CompanyGoodUnitView> BasicInfo_CompanyGoodUnitView { get; set; } // CompanyGoodUnitView
        IDbSet<BasicInfo_CompanyGoodView> BasicInfo_CompanyGoodView { get; set; } // CompanyGoodView
        IDbSet<BasicInfo_CompanyVesselTankView> BasicInfo_CompanyVesselTankView { get; set; } // CompanyVesselTankView
        IDbSet<BasicInfo_CompanyVesselView> BasicInfo_CompanyVesselView { get; set; } // CompanyVesselView
        IDbSet<BasicInfo_CompanyView> BasicInfo_CompanyView { get; set; } // CompanyView
        IDbSet<BasicInfo_CurrencyView> BasicInfo_CurrencyView { get; set; } // CurrencyView
        IDbSet<BasicInfo_SharedGoodView> BasicInfo_SharedGoodView { get; set; } // SharedGoodView
        IDbSet<BasicInfo_UnitView> BasicInfo_UnitView { get; set; } // UnitView
        IDbSet<BasicInfo_UserView> BasicInfo_UserView { get; set; } // UserView
        IDbSet<BasicInfo_VoyagesView> BasicInfo_VoyagesView { get; set; } // __VoyagesView
        IDbSet<BasicInfo_VoyagesView1> BasicInfo_VoyagesView1 { get; set; } // VoyagesView
        IDbSet<Fuel_Account> Fuel_Account { get; set; } // Accounts
        IDbSet<Fuel_ActivityFlow> Fuel_ActivityFlow { get; set; } // ActivityFlow
        IDbSet<Fuel_ApproveFlowConfig> Fuel_ApproveFlowConfig { get; set; } // ApproveFlowConfig
        IDbSet<Fuel_ApproveFlowConfigValidFuelUser> Fuel_ApproveFlowConfigValidFuelUser { get; set; } // ApproveFlowConfigValidFuelUsers
        IDbSet<Fuel_AsgnSegmentTypeVoucherSetingDetail> Fuel_AsgnSegmentTypeVoucherSetingDetail { get; set; } // AsgnSegmentTypeVoucherSetingDetail
        IDbSet<Fuel_AsgnVoucherAcont> Fuel_AsgnVoucherAcont { get; set; } // AsgnVoucherAconts
        IDbSet<Fuel_Attachment> Fuel_Attachment { get; set; } // Attachments
        IDbSet<Fuel_Charter> Fuel_Charter { get; set; } // Charter
        IDbSet<Fuel_CharterIn> Fuel_CharterIn { get; set; } // CharterIn
        IDbSet<Fuel_CharterItem> Fuel_CharterItem { get; set; } // CharterItem
        IDbSet<Fuel_CharterItemHistory> Fuel_CharterItemHistory { get; set; } // CharterItemHistory
        IDbSet<Fuel_CharterOut> Fuel_CharterOut { get; set; } // CharterOut
        IDbSet<Fuel_EffectiveFactor> Fuel_EffectiveFactor { get; set; } // EffectiveFactor
        IDbSet<Fuel_EovReportsView> Fuel_EovReportsView { get; set; } // EOVReportsView
        IDbSet<Fuel_EventLog> Fuel_EventLog { get; set; } // EventLogs
        IDbSet<Fuel_ExceptionLog> Fuel_ExceptionLog { get; set; } // ExceptionLogs
        IDbSet<Fuel_FreeAccount> Fuel_FreeAccount { get; set; } // FreeAccounts
        IDbSet<Fuel_FuelReport> Fuel_FuelReport { get; set; } // FuelReport
        IDbSet<Fuel_FuelReportDetail> Fuel_FuelReportDetail { get; set; } // FuelReportDetail
        IDbSet<Fuel_InventoryOperation> Fuel_InventoryOperation { get; set; } // InventoryOperation
        IDbSet<Fuel_Invoice> Fuel_Invoice { get; set; } // Invoice
        IDbSet<Fuel_InvoiceAdditionalPrice> Fuel_InvoiceAdditionalPrice { get; set; } // InvoiceAdditionalPrices
        IDbSet<Fuel_InvoiceItem> Fuel_InvoiceItem { get; set; } // InvoiceItems
        IDbSet<Fuel_JournalEntry> Fuel_JournalEntry { get; set; } // JournalEntries
        IDbSet<Fuel_Log> Fuel_Log { get; set; } // Logs
        IDbSet<Fuel_Offhire> Fuel_Offhire { get; set; } // Offhire
        IDbSet<Fuel_OffhireDetail> Fuel_OffhireDetail { get; set; } // OffhireDetail
        IDbSet<Fuel_Order> Fuel_Order { get; set; } // Order
        IDbSet<Fuel_OrderItem> Fuel_OrderItem { get; set; } // OrderItems
        IDbSet<Fuel_OrderItemBalance> Fuel_OrderItemBalance { get; set; } // OrderItemBalances
        IDbSet<Fuel_Scrap> Fuel_Scrap { get; set; } // Scrap
        IDbSet<Fuel_ScrapDetail> Fuel_ScrapDetail { get; set; } // ScrapDetail
        IDbSet<Fuel_Segment> Fuel_Segment { get; set; } // Segments
        IDbSet<Fuel_UserInCompany> Fuel_UserInCompany { get; set; } // UserInCompany
        IDbSet<Fuel_Vessel> Fuel_Vessel { get; set; } // Vessel
        IDbSet<Fuel_VesselInCompany> Fuel_VesselInCompany { get; set; } // VesselInCompany
        IDbSet<Fuel_Voucher> Fuel_Voucher { get; set; } // Vouchers
        IDbSet<Fuel_VoucherLog> Fuel_VoucherLog { get; set; } // VoucherLogs
        IDbSet<Fuel_VoucherReportView> Fuel_VoucherReportView { get; set; } // VoucherReportView
        IDbSet<Fuel_VoucherSeting> Fuel_VoucherSeting { get; set; } // VoucherSetings
        IDbSet<Fuel_VoucherSetingDetail> Fuel_VoucherSetingDetail { get; set; } // VoucherSetingDetails
        IDbSet<Fuel_Voyage> Fuel_Voyage { get; set; } // Voyage
        IDbSet<Fuel_VoyageLog> Fuel_VoyageLog { get; set; } // VoyageLog
        IDbSet<Fuel_Workflow> Fuel_Workflow { get; set; } // Workflow
        IDbSet<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog
        IDbSet<Fuel_WorkflowLogOld> Fuel_WorkflowLogOld { get; set; } // WorkflowLog_Old
        IDbSet<Fuel_WorkflowStep> Fuel_WorkflowStep { get; set; } // WorkflowStep
        IDbSet<Group> Groups { get; set; } // Groups
        IDbSet<HafezAccountListView> HafezAccountListViews { get; set; } // HAFEZAccountListView
        IDbSet<HafezVoyagesView> HafezVoyagesViews { get; set; } // HAFEZVoyagesView
        IDbSet<HafizAccountListView> HafizAccountListViews { get; set; } // HAFIZAccountListView
        IDbSet<HafizVoyagesView> HafizVoyagesViews { get; set; } // HAFIZVoyagesView
        IDbSet<Inventory_Company> Inventory_Company { get; set; } // Companies
        IDbSet<Inventory_ErrorMessage> Inventory_ErrorMessage { get; set; } // ErrorMessages
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
        IDbSet<Offhire_OffhireFuelTypeFuelGoodCode> Offhire_OffhireFuelTypeFuelGoodCode { get; set; } // OffhireFuelTypeFuelGoodCode
        IDbSet<Offhire_OffhireMeasureTypeFuelMeasureCode> Offhire_OffhireMeasureTypeFuelMeasureCode { get; set; } // OffhireMeasureTypeFuelMeasureCode
        IDbSet<PartiesCustomActions> PartiesCustomActions { get; set; } // Parties_CustomActions
        IDbSet<Party> Parties { get; set; } // Parties
        IDbSet<SapidAccountListView> SapidAccountListViews { get; set; } // SAPIDAccountListView
        IDbSet<SapidVoyagesView> SapidVoyagesViews { get; set; } // SAPIDVoyagesView
        IDbSet<Sysdiagram> Sysdiagrams { get; set; } // sysdiagrams
        IDbSet<User> Users { get; set; } // Users
        IDbSet<UsersGroups> UsersGroups { get; set; } // Users_Groups
        IDbSet<VersionInfo> VersionInfoes { get; set; } // VersionInfo
        IDbSet<Vessel_FuelReportCommandLog> Vessel_FuelReportCommandLog { get; set; } // FuelReportCommandLog
        IDbSet<Vessel_FuelReportCommandLogDetail> Vessel_FuelReportCommandLogDetail { get; set; } // FuelReportCommandLogDetail

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
        // Stored Procedures
        int Fuel_GetFuelOriginalData(string code);
        int Fuel_GetVesselReportData(string shipCode, string voyageNo, DateTime? fromDate, DateTime? toDate, double? portTime, double? portTimeMol, string locationType);
        int Fuel_GetVesselReportShipNameData();
        int Fuel_GetVesselReportVoyageData(string shipCode);
        List<Fuel_PeriodicalFuelStatisticsReturnModel> Fuel_PeriodicalFuelStatistics(long? companyId, long? warehouseId, long? quantityUnitId, long? goodId, DateTime? from, DateTime? to, out int procResult);
        int Inventory_ActivateWarehouseIncludingRecieptsOperation(string description, long? warehouseId, int? timeBucketId, int? storeTypesId, DateTime? registrationDate, string referenceType, string referenceNo, string transactionItems, int? userCreatorId);
        List<Inventory_CardexReturnModel> Inventory_Cardex(long? warehouseId, long? goodId, DateTime? startDate, DateTime? endDate, out int procResult);
        int Inventory_ChangeCoefficientAndUpdatePriceByUnitConvertId(int? unitConvertId, out string message);
        List<Inventory_ChangeWarehouseStatusReturnModel> Inventory_ChangeWarehouseStatus(bool? isActive, long? warehouseId, int? userCreatorId, out int procResult);
        int Inventory_ErrorHandling();
        List<Inventory_IsValidTransactionCodeReturnModel> Inventory_IsValidTransactionCode(byte? action, decimal? code, long? warehouseId, DateTime? registrationDate, int? timeBucketId, out int procResult);
        int Inventory_PriceAllSuspendedIssuedItems(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int? userCreatorId, out string transactionItemPriceIds, out string message);
        int Inventory_PriceAllSuspendedTransactionItemsUsingReference(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int? userCreatorId, byte? action, out string transactionItemPriceIds, out string message);
        int Inventory_PriceGivenIssuedItems(int? userCreatorId, string issueItemIds, out string transactionItemPriceIds, out string message, out int? notPricedTransactionId);
        int Inventory_PriceSuspendedTransactionUsingReference(int? transactionId, string description, int? userCreatorId, out string transactionItemPriceIds, out string message);
        int Inventory_RemoveTransactionItemPrices(int? transactionItemId, int? userId, out string message);
        List<Inventory_TransactionItemPricesGetAllReturnModel> Inventory_TransactionItemPricesGetAll( out int procResult);
        int Inventory_TransactionItemPricesOperation(string action, int? userCreatorId, string transactionItemPrices, out string transactionItemPriceIds, out string message);
        List<Inventory_TransactionItemsGetAllReturnModel> Inventory_TransactionItemsGetAll( out int procResult);
        int Inventory_TransactionItemsOperation(string action, int? transactionId, int? userCreatorId, string transactionItems, out string transactionItemsId, out string message);
        int Inventory_TransactionOperation(string action, int? id, byte? transactionAction, string description, long? companyId, long? warehouseId, int? timeBucketId, int? storeTypesId, int? pricingReferenceId, byte? status, DateTime? registrationDate, string referenceType, string referenceNo, int? userCreatorId, out int? transactionId, out decimal? code, out string message);
        List<Inventory_TransactionsGetAllReturnModel> Inventory_TransactionsGetAll( out int procResult);
        List<Inventory_UnitConvertsOperationReturnModel> Inventory_UnitConvertsOperation(string action, int? id, long? unitId, long? subUnitId, decimal? coefficient, int? fiscalYearId, DateTime? effectiveDateStart, DateTime? effectiveDateEnd, int? userCreatorId, out string message, out int procResult);
    }

    // ************************************************************************
    // Database context
    internal class MyDbContext : DbContext, IMyDbContext
    {
        public IDbSet<ActionType> ActionTypes { get; set; } // ActionTypes
        public IDbSet<BasicInfo_AccountListView> BasicInfo_AccountListView { get; set; } // AccountListView
        public IDbSet<BasicInfo_ActivityLocation> BasicInfo_ActivityLocation { get; set; } // ActivityLocation
        public IDbSet<BasicInfo_CompanyGoodUnitView> BasicInfo_CompanyGoodUnitView { get; set; } // CompanyGoodUnitView
        public IDbSet<BasicInfo_CompanyGoodView> BasicInfo_CompanyGoodView { get; set; } // CompanyGoodView
        public IDbSet<BasicInfo_CompanyVesselTankView> BasicInfo_CompanyVesselTankView { get; set; } // CompanyVesselTankView
        public IDbSet<BasicInfo_CompanyVesselView> BasicInfo_CompanyVesselView { get; set; } // CompanyVesselView
        public IDbSet<BasicInfo_CompanyView> BasicInfo_CompanyView { get; set; } // CompanyView
        public IDbSet<BasicInfo_CurrencyView> BasicInfo_CurrencyView { get; set; } // CurrencyView
        public IDbSet<BasicInfo_SharedGoodView> BasicInfo_SharedGoodView { get; set; } // SharedGoodView
        public IDbSet<BasicInfo_UnitView> BasicInfo_UnitView { get; set; } // UnitView
        public IDbSet<BasicInfo_UserView> BasicInfo_UserView { get; set; } // UserView
        public IDbSet<BasicInfo_VoyagesView> BasicInfo_VoyagesView { get; set; } // __VoyagesView
        public IDbSet<BasicInfo_VoyagesView1> BasicInfo_VoyagesView1 { get; set; } // VoyagesView
        public IDbSet<Fuel_Account> Fuel_Account { get; set; } // Accounts
        public IDbSet<Fuel_ActivityFlow> Fuel_ActivityFlow { get; set; } // ActivityFlow
        public IDbSet<Fuel_ApproveFlowConfig> Fuel_ApproveFlowConfig { get; set; } // ApproveFlowConfig
        public IDbSet<Fuel_ApproveFlowConfigValidFuelUser> Fuel_ApproveFlowConfigValidFuelUser { get; set; } // ApproveFlowConfigValidFuelUsers
        public IDbSet<Fuel_AsgnSegmentTypeVoucherSetingDetail> Fuel_AsgnSegmentTypeVoucherSetingDetail { get; set; } // AsgnSegmentTypeVoucherSetingDetail
        public IDbSet<Fuel_AsgnVoucherAcont> Fuel_AsgnVoucherAcont { get; set; } // AsgnVoucherAconts
        public IDbSet<Fuel_Attachment> Fuel_Attachment { get; set; } // Attachments
        public IDbSet<Fuel_Charter> Fuel_Charter { get; set; } // Charter
        public IDbSet<Fuel_CharterIn> Fuel_CharterIn { get; set; } // CharterIn
        public IDbSet<Fuel_CharterItem> Fuel_CharterItem { get; set; } // CharterItem
        public IDbSet<Fuel_CharterItemHistory> Fuel_CharterItemHistory { get; set; } // CharterItemHistory
        public IDbSet<Fuel_CharterOut> Fuel_CharterOut { get; set; } // CharterOut
        public IDbSet<Fuel_EffectiveFactor> Fuel_EffectiveFactor { get; set; } // EffectiveFactor
        public IDbSet<Fuel_EovReportsView> Fuel_EovReportsView { get; set; } // EOVReportsView
        public IDbSet<Fuel_EventLog> Fuel_EventLog { get; set; } // EventLogs
        public IDbSet<Fuel_ExceptionLog> Fuel_ExceptionLog { get; set; } // ExceptionLogs
        public IDbSet<Fuel_FreeAccount> Fuel_FreeAccount { get; set; } // FreeAccounts
        public IDbSet<Fuel_FuelReport> Fuel_FuelReport { get; set; } // FuelReport
        public IDbSet<Fuel_FuelReportDetail> Fuel_FuelReportDetail { get; set; } // FuelReportDetail
        public IDbSet<Fuel_InventoryOperation> Fuel_InventoryOperation { get; set; } // InventoryOperation
        public IDbSet<Fuel_Invoice> Fuel_Invoice { get; set; } // Invoice
        public IDbSet<Fuel_InvoiceAdditionalPrice> Fuel_InvoiceAdditionalPrice { get; set; } // InvoiceAdditionalPrices
        public IDbSet<Fuel_InvoiceItem> Fuel_InvoiceItem { get; set; } // InvoiceItems
        public IDbSet<Fuel_JournalEntry> Fuel_JournalEntry { get; set; } // JournalEntries
        public IDbSet<Fuel_Log> Fuel_Log { get; set; } // Logs
        public IDbSet<Fuel_Offhire> Fuel_Offhire { get; set; } // Offhire
        public IDbSet<Fuel_OffhireDetail> Fuel_OffhireDetail { get; set; } // OffhireDetail
        public IDbSet<Fuel_Order> Fuel_Order { get; set; } // Order
        public IDbSet<Fuel_OrderItem> Fuel_OrderItem { get; set; } // OrderItems
        public IDbSet<Fuel_OrderItemBalance> Fuel_OrderItemBalance { get; set; } // OrderItemBalances
        public IDbSet<Fuel_Scrap> Fuel_Scrap { get; set; } // Scrap
        public IDbSet<Fuel_ScrapDetail> Fuel_ScrapDetail { get; set; } // ScrapDetail
        public IDbSet<Fuel_Segment> Fuel_Segment { get; set; } // Segments
        public IDbSet<Fuel_UserInCompany> Fuel_UserInCompany { get; set; } // UserInCompany
        public IDbSet<Fuel_Vessel> Fuel_Vessel { get; set; } // Vessel
        public IDbSet<Fuel_VesselInCompany> Fuel_VesselInCompany { get; set; } // VesselInCompany
        public IDbSet<Fuel_Voucher> Fuel_Voucher { get; set; } // Vouchers
        public IDbSet<Fuel_VoucherLog> Fuel_VoucherLog { get; set; } // VoucherLogs
        public IDbSet<Fuel_VoucherReportView> Fuel_VoucherReportView { get; set; } // VoucherReportView
        public IDbSet<Fuel_VoucherSeting> Fuel_VoucherSeting { get; set; } // VoucherSetings
        public IDbSet<Fuel_VoucherSetingDetail> Fuel_VoucherSetingDetail { get; set; } // VoucherSetingDetails
        public IDbSet<Fuel_Voyage> Fuel_Voyage { get; set; } // Voyage
        public IDbSet<Fuel_VoyageLog> Fuel_VoyageLog { get; set; } // VoyageLog
        public IDbSet<Fuel_Workflow> Fuel_Workflow { get; set; } // Workflow
        public IDbSet<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog
        public IDbSet<Fuel_WorkflowLogOld> Fuel_WorkflowLogOld { get; set; } // WorkflowLog_Old
        public IDbSet<Fuel_WorkflowStep> Fuel_WorkflowStep { get; set; } // WorkflowStep
        public IDbSet<Group> Groups { get; set; } // Groups
        public IDbSet<HafezAccountListView> HafezAccountListViews { get; set; } // HAFEZAccountListView
        public IDbSet<HafezVoyagesView> HafezVoyagesViews { get; set; } // HAFEZVoyagesView
        public IDbSet<HafizAccountListView> HafizAccountListViews { get; set; } // HAFIZAccountListView
        public IDbSet<HafizVoyagesView> HafizVoyagesViews { get; set; } // HAFIZVoyagesView
        public IDbSet<Inventory_Company> Inventory_Company { get; set; } // Companies
        public IDbSet<Inventory_ErrorMessage> Inventory_ErrorMessage { get; set; } // ErrorMessages
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
        public IDbSet<Offhire_OffhireFuelTypeFuelGoodCode> Offhire_OffhireFuelTypeFuelGoodCode { get; set; } // OffhireFuelTypeFuelGoodCode
        public IDbSet<Offhire_OffhireMeasureTypeFuelMeasureCode> Offhire_OffhireMeasureTypeFuelMeasureCode { get; set; } // OffhireMeasureTypeFuelMeasureCode
        public IDbSet<PartiesCustomActions> PartiesCustomActions { get; set; } // Parties_CustomActions
        public IDbSet<Party> Parties { get; set; } // Parties
        public IDbSet<SapidAccountListView> SapidAccountListViews { get; set; } // SAPIDAccountListView
        public IDbSet<SapidVoyagesView> SapidVoyagesViews { get; set; } // SAPIDVoyagesView
        public IDbSet<Sysdiagram> Sysdiagrams { get; set; } // sysdiagrams
        public IDbSet<User> Users { get; set; } // Users
        public IDbSet<UsersGroups> UsersGroups { get; set; } // Users_Groups
        public IDbSet<VersionInfo> VersionInfoes { get; set; } // VersionInfo
        public IDbSet<Vessel_FuelReportCommandLog> Vessel_FuelReportCommandLog { get; set; } // FuelReportCommandLog
        public IDbSet<Vessel_FuelReportCommandLogDetail> Vessel_FuelReportCommandLogDetail { get; set; } // FuelReportCommandLogDetail
        
        static MyDbContext()
        {
            Database.SetInitializer<MyDbContext>(null);
        }

        public MyDbContext()
            : base("Name=MyDbContext")
        {
        }

        public MyDbContext(string connectionString) : base(connectionString)
        {
        }

        public MyDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ActionTypeConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_AccountListViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_ActivityLocationConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_CompanyGoodUnitViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_CompanyGoodViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_CompanyVesselTankViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_CompanyVesselViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_CompanyViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_CurrencyViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_SharedGoodViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_UnitViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_UserViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_VoyagesViewConfiguration());
            modelBuilder.Configurations.Add(new BasicInfo_VoyagesView1Configuration());
            modelBuilder.Configurations.Add(new Fuel_AccountConfiguration());
            modelBuilder.Configurations.Add(new Fuel_ActivityFlowConfiguration());
            modelBuilder.Configurations.Add(new Fuel_ApproveFlowConfigConfiguration());
            modelBuilder.Configurations.Add(new Fuel_ApproveFlowConfigValidFuelUserConfiguration());
            modelBuilder.Configurations.Add(new Fuel_AsgnSegmentTypeVoucherSetingDetailConfiguration());
            modelBuilder.Configurations.Add(new Fuel_AsgnVoucherAcontConfiguration());
            modelBuilder.Configurations.Add(new Fuel_AttachmentConfiguration());
            modelBuilder.Configurations.Add(new Fuel_CharterConfiguration());
            modelBuilder.Configurations.Add(new Fuel_CharterInConfiguration());
            modelBuilder.Configurations.Add(new Fuel_CharterItemConfiguration());
            modelBuilder.Configurations.Add(new Fuel_CharterItemHistoryConfiguration());
            modelBuilder.Configurations.Add(new Fuel_CharterOutConfiguration());
            modelBuilder.Configurations.Add(new Fuel_EffectiveFactorConfiguration());
            modelBuilder.Configurations.Add(new Fuel_EovReportsViewConfiguration());
            modelBuilder.Configurations.Add(new Fuel_EventLogConfiguration());
            modelBuilder.Configurations.Add(new Fuel_ExceptionLogConfiguration());
            modelBuilder.Configurations.Add(new Fuel_FreeAccountConfiguration());
            modelBuilder.Configurations.Add(new Fuel_FuelReportConfiguration());
            modelBuilder.Configurations.Add(new Fuel_FuelReportDetailConfiguration());
            modelBuilder.Configurations.Add(new Fuel_InventoryOperationConfiguration());
            modelBuilder.Configurations.Add(new Fuel_InvoiceConfiguration());
            modelBuilder.Configurations.Add(new Fuel_InvoiceAdditionalPriceConfiguration());
            modelBuilder.Configurations.Add(new Fuel_InvoiceItemConfiguration());
            modelBuilder.Configurations.Add(new Fuel_JournalEntryConfiguration());
            modelBuilder.Configurations.Add(new Fuel_LogConfiguration());
            modelBuilder.Configurations.Add(new Fuel_OffhireConfiguration());
            modelBuilder.Configurations.Add(new Fuel_OffhireDetailConfiguration());
            modelBuilder.Configurations.Add(new Fuel_OrderConfiguration());
            modelBuilder.Configurations.Add(new Fuel_OrderItemConfiguration());
            modelBuilder.Configurations.Add(new Fuel_OrderItemBalanceConfiguration());
            modelBuilder.Configurations.Add(new Fuel_ScrapConfiguration());
            modelBuilder.Configurations.Add(new Fuel_ScrapDetailConfiguration());
            modelBuilder.Configurations.Add(new Fuel_SegmentConfiguration());
            modelBuilder.Configurations.Add(new Fuel_UserInCompanyConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VesselConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VesselInCompanyConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VoucherConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VoucherLogConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VoucherReportViewConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VoucherSetingConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VoucherSetingDetailConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VoyageConfiguration());
            modelBuilder.Configurations.Add(new Fuel_VoyageLogConfiguration());
            modelBuilder.Configurations.Add(new Fuel_WorkflowConfiguration());
            modelBuilder.Configurations.Add(new Fuel_WorkflowLogConfiguration());
            modelBuilder.Configurations.Add(new Fuel_WorkflowLogOldConfiguration());
            modelBuilder.Configurations.Add(new Fuel_WorkflowStepConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new HafezAccountListViewConfiguration());
            modelBuilder.Configurations.Add(new HafezVoyagesViewConfiguration());
            modelBuilder.Configurations.Add(new HafizAccountListViewConfiguration());
            modelBuilder.Configurations.Add(new HafizVoyagesViewConfiguration());
            modelBuilder.Configurations.Add(new Inventory_CompanyConfiguration());
            modelBuilder.Configurations.Add(new Inventory_ErrorMessageConfiguration());
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
            modelBuilder.Configurations.Add(new Offhire_OffhireFuelTypeFuelGoodCodeConfiguration());
            modelBuilder.Configurations.Add(new Offhire_OffhireMeasureTypeFuelMeasureCodeConfiguration());
            modelBuilder.Configurations.Add(new PartiesCustomActionsConfiguration());
            modelBuilder.Configurations.Add(new PartyConfiguration());
            modelBuilder.Configurations.Add(new SapidAccountListViewConfiguration());
            modelBuilder.Configurations.Add(new SapidVoyagesViewConfiguration());
            modelBuilder.Configurations.Add(new SysdiagramConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UsersGroupsConfiguration());
            modelBuilder.Configurations.Add(new VersionInfoConfiguration());
            modelBuilder.Configurations.Add(new Vessel_FuelReportCommandLogConfiguration());
            modelBuilder.Configurations.Add(new Vessel_FuelReportCommandLogDetailConfiguration());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new ActionTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_AccountListViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_ActivityLocationConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_CompanyGoodUnitViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_CompanyGoodViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_CompanyVesselTankViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_CompanyVesselViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_CompanyViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_CurrencyViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_SharedGoodViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_UnitViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_UserViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_VoyagesViewConfiguration(schema));
            modelBuilder.Configurations.Add(new BasicInfo_VoyagesView1Configuration(schema));
            modelBuilder.Configurations.Add(new Fuel_AccountConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_ActivityFlowConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_ApproveFlowConfigConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_ApproveFlowConfigValidFuelUserConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_AsgnSegmentTypeVoucherSetingDetailConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_AsgnVoucherAcontConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_AttachmentConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_CharterConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_CharterInConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_CharterItemConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_CharterItemHistoryConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_CharterOutConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_EffectiveFactorConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_EovReportsViewConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_EventLogConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_ExceptionLogConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_FreeAccountConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_FuelReportConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_FuelReportDetailConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_InventoryOperationConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_InvoiceConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_InvoiceAdditionalPriceConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_InvoiceItemConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_JournalEntryConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_LogConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_OffhireConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_OffhireDetailConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_OrderConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_OrderItemConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_OrderItemBalanceConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_ScrapConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_ScrapDetailConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_SegmentConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_UserInCompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VesselConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VesselInCompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VoucherConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VoucherLogConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VoucherReportViewConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VoucherSetingConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VoucherSetingDetailConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VoyageConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_VoyageLogConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_WorkflowConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_WorkflowLogConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_WorkflowLogOldConfiguration(schema));
            modelBuilder.Configurations.Add(new Fuel_WorkflowStepConfiguration(schema));
            modelBuilder.Configurations.Add(new GroupConfiguration(schema));
            modelBuilder.Configurations.Add(new HafezAccountListViewConfiguration(schema));
            modelBuilder.Configurations.Add(new HafezVoyagesViewConfiguration(schema));
            modelBuilder.Configurations.Add(new HafizAccountListViewConfiguration(schema));
            modelBuilder.Configurations.Add(new HafizVoyagesViewConfiguration(schema));
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
            modelBuilder.Configurations.Add(new Offhire_OffhireFuelTypeFuelGoodCodeConfiguration(schema));
            modelBuilder.Configurations.Add(new Offhire_OffhireMeasureTypeFuelMeasureCodeConfiguration(schema));
            modelBuilder.Configurations.Add(new PartiesCustomActionsConfiguration(schema));
            modelBuilder.Configurations.Add(new PartyConfiguration(schema));
            modelBuilder.Configurations.Add(new SapidAccountListViewConfiguration(schema));
            modelBuilder.Configurations.Add(new SapidVoyagesViewConfiguration(schema));
            modelBuilder.Configurations.Add(new SysdiagramConfiguration(schema));
            modelBuilder.Configurations.Add(new UserConfiguration(schema));
            modelBuilder.Configurations.Add(new UsersGroupsConfiguration(schema));
            modelBuilder.Configurations.Add(new VersionInfoConfiguration(schema));
            modelBuilder.Configurations.Add(new Vessel_FuelReportCommandLogConfiguration(schema));
            modelBuilder.Configurations.Add(new Vessel_FuelReportCommandLogDetailConfiguration(schema));
            return modelBuilder;
        }
        
        // Stored Procedures
        public int Fuel_GetFuelOriginalData(string code)
        {
            var codeParam = new SqlParameter { ParameterName = "@Code", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = code, Size = 50 };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Fuel].[GetFuelOriginalData] @Code", codeParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int Fuel_GetVesselReportData(string shipCode, string voyageNo, DateTime? fromDate, DateTime? toDate, double? portTime, double? portTimeMol, string locationType)
        {
            var shipCodeParam = new SqlParameter { ParameterName = "@ShipCode", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = shipCode, Size = 10 };
            var voyageNoParam = new SqlParameter { ParameterName = "@VoyageNo", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = voyageNo, Size = 50 };
            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            var portTimeParam = new SqlParameter { ParameterName = "@PortTime", SqlDbType = SqlDbType.Float, Direction = ParameterDirection.Input, Value = portTime.GetValueOrDefault() };
            var portTimeMolParam = new SqlParameter { ParameterName = "@PortTimeMOL", SqlDbType = SqlDbType.Float, Direction = ParameterDirection.Input, Value = portTimeMol.GetValueOrDefault() };
            var locationTypeParam = new SqlParameter { ParameterName = "@LocationType", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = locationType, Size = 10 };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Fuel].[GetVesselReportData] @ShipCode, @VoyageNo, @FromDate, @ToDate, @PortTime, @PortTimeMOL, @LocationType", shipCodeParam, voyageNoParam, fromDateParam, toDateParam, portTimeParam, portTimeMolParam, locationTypeParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int Fuel_GetVesselReportShipNameData()
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Fuel].[GetVesselReportShipNameData] ", procResultParam);
 
            return (int) procResultParam.Value;
        }

        public int Fuel_GetVesselReportVoyageData(string shipCode)
        {
            var shipCodeParam = new SqlParameter { ParameterName = "@ShipCode", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = shipCode, Size = 50 };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Fuel].[GetVesselReportVoyageData] @ShipCode", shipCodeParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<Fuel_PeriodicalFuelStatisticsReturnModel> Fuel_PeriodicalFuelStatistics(long? companyId, long? warehouseId, long? quantityUnitId, long? goodId, DateTime? from, DateTime? to, out int procResult)
        {
            var companyIdParam = new SqlParameter { ParameterName = "@CompanyId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = companyId.GetValueOrDefault() };
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var quantityUnitIdParam = new SqlParameter { ParameterName = "@QuantityUnitId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = quantityUnitId.GetValueOrDefault() };
            var goodIdParam = new SqlParameter { ParameterName = "@GoodId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = goodId.GetValueOrDefault() };
            var fromParam = new SqlParameter { ParameterName = "@From", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = from.GetValueOrDefault() };
            var toParam = new SqlParameter { ParameterName = "@To", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = to.GetValueOrDefault() };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Fuel_PeriodicalFuelStatisticsReturnModel>("EXEC @procResult = [Fuel].[PeriodicalFuelStatistics] @CompanyId, @WarehouseId, @QuantityUnitId, @GoodId, @From, @To", companyIdParam, warehouseIdParam, quantityUnitIdParam, goodIdParam, fromParam, toParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int Inventory_ActivateWarehouseIncludingRecieptsOperation(string description, long? warehouseId, int? timeBucketId, int? storeTypesId, DateTime? registrationDate, string referenceType, string referenceNo, string transactionItems, int? userCreatorId)
        {
            var descriptionParam = new SqlParameter { ParameterName = "@Description", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = description };
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var timeBucketIdParam = new SqlParameter { ParameterName = "@TimeBucketId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = timeBucketId.GetValueOrDefault() };
            var storeTypesIdParam = new SqlParameter { ParameterName = "@StoreTypesId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeTypesId.GetValueOrDefault() };
            var registrationDateParam = new SqlParameter { ParameterName = "@RegistrationDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = registrationDate.GetValueOrDefault() };
            var referenceTypeParam = new SqlParameter { ParameterName = "@ReferenceType", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = referenceType, Size = 100 };
            var referenceNoParam = new SqlParameter { ParameterName = "@ReferenceNo", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = referenceNo, Size = 100 };
            var transactionItemsParam = new SqlParameter { ParameterName = "@TransactionItems", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = transactionItems };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[ActivateWarehouseIncludingRecieptsOperation] @Description, @WarehouseId, @TimeBucketId, @StoreTypesId, @RegistrationDate, @ReferenceType, @ReferenceNo, @TransactionItems, @UserCreatorId", descriptionParam, warehouseIdParam, timeBucketIdParam, storeTypesIdParam, registrationDateParam, referenceTypeParam, referenceNoParam, transactionItemsParam, userCreatorIdParam, procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<Inventory_CardexReturnModel> Inventory_Cardex(long? warehouseId, long? goodId, DateTime? startDate, DateTime? endDate, out int procResult)
        {
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var goodIdParam = new SqlParameter { ParameterName = "@GoodId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = goodId.GetValueOrDefault() };
            var startDateParam = new SqlParameter { ParameterName = "@StartDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = startDate.GetValueOrDefault() };
            var endDateParam = new SqlParameter { ParameterName = "@EndDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = endDate.GetValueOrDefault() };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Inventory_CardexReturnModel>("EXEC @procResult = [Inventory].[Cardex] @WarehouseId, @GoodId, @StartDate, @EndDate", warehouseIdParam, goodIdParam, startDateParam, endDateParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int Inventory_ChangeCoefficientAndUpdatePriceByUnitConvertId(int? unitConvertId, out string message)
        {
            var unitConvertIdParam = new SqlParameter { ParameterName = "@UnitConvertId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = unitConvertId.GetValueOrDefault() };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[ChangeCoefficientAndUpdatePriceByUnitConvertId] @UnitConvertId, @Message OUTPUT", unitConvertIdParam, messageParam, procResultParam);
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public List<Inventory_ChangeWarehouseStatusReturnModel> Inventory_ChangeWarehouseStatus(bool? isActive, long? warehouseId, int? userCreatorId, out int procResult)
        {
            var isActiveParam = new SqlParameter { ParameterName = "@IsActive", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Input, Value = isActive.GetValueOrDefault() };
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Inventory_ChangeWarehouseStatusReturnModel>("EXEC @procResult = [Inventory].[ChangeWarehouseStatus] @IsActive, @WarehouseId, @UserCreatorId", isActiveParam, warehouseIdParam, userCreatorIdParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int Inventory_ErrorHandling()
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[ErrorHandling] ", procResultParam);
 
            return (int) procResultParam.Value;
        }

        public List<Inventory_IsValidTransactionCodeReturnModel> Inventory_IsValidTransactionCode(byte? action, decimal? code, long? warehouseId, DateTime? registrationDate, int? timeBucketId, out int procResult)
        {
            var actionParam = new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.TinyInt, Direction = ParameterDirection.Input, Value = action.GetValueOrDefault() };
            var codeParam = new SqlParameter { ParameterName = "@Code", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input, Value = code.GetValueOrDefault() };
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var registrationDateParam = new SqlParameter { ParameterName = "@RegistrationDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = registrationDate.GetValueOrDefault() };
            var timeBucketIdParam = new SqlParameter { ParameterName = "@TimeBucketId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = timeBucketId.GetValueOrDefault() };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Inventory_IsValidTransactionCodeReturnModel>("EXEC @procResult = [Inventory].[IsValidTransactionCode] @Action, @Code, @WarehouseId, @RegistrationDate, @TimeBucketId", actionParam, codeParam, warehouseIdParam, registrationDateParam, timeBucketIdParam, procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int Inventory_PriceAllSuspendedIssuedItems(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int? userCreatorId, out string transactionItemPriceIds, out string message)
        {
            var companyIdParam = new SqlParameter { ParameterName = "@CompanyId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = companyId.GetValueOrDefault() };
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var transactionItemPriceIdsParam = new SqlParameter { ParameterName = "@TransactionItemPriceIds", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[PriceAllSuspendedIssuedItems] @CompanyId, @WarehouseId, @FromDate, @ToDate, @UserCreatorId, @TransactionItemPriceIds OUTPUT, @Message OUTPUT", companyIdParam, warehouseIdParam, fromDateParam, toDateParam, userCreatorIdParam, transactionItemPriceIdsParam, messageParam, procResultParam);
            transactionItemPriceIds = (string) transactionItemPriceIdsParam.Value;
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public int Inventory_PriceAllSuspendedTransactionItemsUsingReference(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int? userCreatorId, byte? action, out string transactionItemPriceIds, out string message)
        {
            var companyIdParam = new SqlParameter { ParameterName = "@CompanyId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = companyId.GetValueOrDefault() };
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var fromDateParam = new SqlParameter { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = fromDate.GetValueOrDefault() };
            var toDateParam = new SqlParameter { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = toDate.GetValueOrDefault() };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var actionParam = new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.TinyInt, Direction = ParameterDirection.Input, Value = action.GetValueOrDefault() };
            var transactionItemPriceIdsParam = new SqlParameter { ParameterName = "@TransactionItemPriceIds", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[PriceAllSuspendedTransactionItemsUsingReference] @CompanyId, @WarehouseId, @FromDate, @ToDate, @UserCreatorId, @Action, @TransactionItemPriceIds OUTPUT, @Message OUTPUT", companyIdParam, warehouseIdParam, fromDateParam, toDateParam, userCreatorIdParam, actionParam, transactionItemPriceIdsParam, messageParam, procResultParam);
            transactionItemPriceIds = (string) transactionItemPriceIdsParam.Value;
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public int Inventory_PriceGivenIssuedItems(int? userCreatorId, string issueItemIds, out string transactionItemPriceIds, out string message, out int? notPricedTransactionId)
        {
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var issueItemIdsParam = new SqlParameter { ParameterName = "@IssueItemIds", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = issueItemIds };
            var transactionItemPriceIdsParam = new SqlParameter { ParameterName = "@TransactionItemPriceIds", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var notPricedTransactionIdParam = new SqlParameter { ParameterName = "@NotPricedTransactionId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[PriceGivenIssuedItems] @UserCreatorId, @IssueItemIds, @TransactionItemPriceIds OUTPUT, @Message OUTPUT, @NotPricedTransactionId OUTPUT", userCreatorIdParam, issueItemIdsParam, transactionItemPriceIdsParam, messageParam, notPricedTransactionIdParam, procResultParam);
            transactionItemPriceIds = (string) transactionItemPriceIdsParam.Value;
            message = (string) messageParam.Value;
            notPricedTransactionId = (int) notPricedTransactionIdParam.Value;
 
            return (int) procResultParam.Value;
        }

        public int Inventory_PriceSuspendedTransactionUsingReference(int? transactionId, string description, int? userCreatorId, out string transactionItemPriceIds, out string message)
        {
            var transactionIdParam = new SqlParameter { ParameterName = "@TransactionId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = transactionId.GetValueOrDefault() };
            var descriptionParam = new SqlParameter { ParameterName = "@Description", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = description };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var transactionItemPriceIdsParam = new SqlParameter { ParameterName = "@TransactionItemPriceIds", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[PriceSuspendedTransactionUsingReference] @TransactionId, @Description, @UserCreatorId, @TransactionItemPriceIds OUTPUT, @Message OUTPUT", transactionIdParam, descriptionParam, userCreatorIdParam, transactionItemPriceIdsParam, messageParam, procResultParam);
            transactionItemPriceIds = (string) transactionItemPriceIdsParam.Value;
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public int Inventory_RemoveTransactionItemPrices(int? transactionItemId, int? userId, out string message)
        {
            var transactionItemIdParam = new SqlParameter { ParameterName = "@TransactionItemId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = transactionItemId.GetValueOrDefault() };
            var userIdParam = new SqlParameter { ParameterName = "@UserId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userId.GetValueOrDefault() };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[RemoveTransactionItemPrices] @TransactionItemId, @UserId, @Message OUTPUT", transactionItemIdParam, userIdParam, messageParam, procResultParam);
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public List<Inventory_TransactionItemPricesGetAllReturnModel> Inventory_TransactionItemPricesGetAll( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Inventory_TransactionItemPricesGetAllReturnModel>("EXEC @procResult = [Inventory].[TransactionItemPricesGetAll] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int Inventory_TransactionItemPricesOperation(string action, int? userCreatorId, string transactionItemPrices, out string transactionItemPriceIds, out string message)
        {
            var actionParam = new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = action, Size = 10 };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var transactionItemPricesParam = new SqlParameter { ParameterName = "@TransactionItemPrices", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = transactionItemPrices };
            var transactionItemPriceIdsParam = new SqlParameter { ParameterName = "@TransactionItemPriceIds", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[TransactionItemPricesOperation] @Action, @UserCreatorId, @TransactionItemPrices, @TransactionItemPriceIds OUTPUT, @Message OUTPUT", actionParam, userCreatorIdParam, transactionItemPricesParam, transactionItemPriceIdsParam, messageParam, procResultParam);
            transactionItemPriceIds = (string) transactionItemPriceIdsParam.Value;
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public List<Inventory_TransactionItemsGetAllReturnModel> Inventory_TransactionItemsGetAll( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Inventory_TransactionItemsGetAllReturnModel>("EXEC @procResult = [Inventory].[TransactionItemsGetAll] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public int Inventory_TransactionItemsOperation(string action, int? transactionId, int? userCreatorId, string transactionItems, out string transactionItemsId, out string message)
        {
            var actionParam = new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = action, Size = 10 };
            var transactionIdParam = new SqlParameter { ParameterName = "@TransactionId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = transactionId.GetValueOrDefault() };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var transactionItemsParam = new SqlParameter { ParameterName = "@TransactionItems", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = transactionItems };
            var transactionItemsIdParam = new SqlParameter { ParameterName = "@TransactionItemsId", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output, Size = 256 };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[TransactionItemsOperation] @Action, @TransactionId, @UserCreatorId, @TransactionItems, @TransactionItemsId OUTPUT, @Message OUTPUT", actionParam, transactionIdParam, userCreatorIdParam, transactionItemsParam, transactionItemsIdParam, messageParam, procResultParam);
            transactionItemsId = (string) transactionItemsIdParam.Value;
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public int Inventory_TransactionOperation(string action, int? id, byte? transactionAction, string description, long? companyId, long? warehouseId, int? timeBucketId, int? storeTypesId, int? pricingReferenceId, byte? status, DateTime? registrationDate, string referenceType, string referenceNo, int? userCreatorId, out int? transactionId, out decimal? code, out string message)
        {
            var actionParam = new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = action, Size = 10 };
            var idParam = new SqlParameter { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = id.GetValueOrDefault() };
            var transactionActionParam = new SqlParameter { ParameterName = "@TransactionAction", SqlDbType = SqlDbType.TinyInt, Direction = ParameterDirection.Input, Value = transactionAction.GetValueOrDefault() };
            var descriptionParam = new SqlParameter { ParameterName = "@Description", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = description };
            var companyIdParam = new SqlParameter { ParameterName = "@CompanyId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = companyId.GetValueOrDefault() };
            var warehouseIdParam = new SqlParameter { ParameterName = "@WarehouseId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = warehouseId.GetValueOrDefault() };
            var timeBucketIdParam = new SqlParameter { ParameterName = "@TimeBucketId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = timeBucketId.GetValueOrDefault() };
            var storeTypesIdParam = new SqlParameter { ParameterName = "@StoreTypesId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = storeTypesId.GetValueOrDefault() };
            var pricingReferenceIdParam = new SqlParameter { ParameterName = "@PricingReferenceId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = pricingReferenceId.GetValueOrDefault() };
            var statusParam = new SqlParameter { ParameterName = "@Status", SqlDbType = SqlDbType.TinyInt, Direction = ParameterDirection.Input, Value = status.GetValueOrDefault() };
            var registrationDateParam = new SqlParameter { ParameterName = "@RegistrationDate", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = registrationDate.GetValueOrDefault() };
            var referenceTypeParam = new SqlParameter { ParameterName = "@ReferenceType", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = referenceType, Size = 100 };
            var referenceNoParam = new SqlParameter { ParameterName = "@ReferenceNo", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = referenceNo, Size = 100 };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var transactionIdParam = new SqlParameter { ParameterName = "@TransactionId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var codeParam = new SqlParameter { ParameterName = "@Code", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Output };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            Database.ExecuteSqlCommand("EXEC @procResult = [Inventory].[TransactionOperation] @Action, @Id, @TransactionAction, @Description, @CompanyId, @WarehouseId, @TimeBucketId, @StoreTypesId, @PricingReferenceId, @Status, @RegistrationDate, @ReferenceType, @ReferenceNo, @UserCreatorId, @TransactionId OUTPUT, @Code OUTPUT, @Message OUTPUT", actionParam, idParam, transactionActionParam, descriptionParam, companyIdParam, warehouseIdParam, timeBucketIdParam, storeTypesIdParam, pricingReferenceIdParam, statusParam, registrationDateParam, referenceTypeParam, referenceNoParam, userCreatorIdParam, transactionIdParam, codeParam, messageParam, procResultParam);
            transactionId = (int) transactionIdParam.Value;
            code = (decimal) codeParam.Value;
            message = (string) messageParam.Value;
 
            return (int) procResultParam.Value;
        }

        public List<Inventory_TransactionsGetAllReturnModel> Inventory_TransactionsGetAll( out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Inventory_TransactionsGetAllReturnModel>("EXEC @procResult = [Inventory].[TransactionsGetAll] ", procResultParam).ToList();
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public List<Inventory_UnitConvertsOperationReturnModel> Inventory_UnitConvertsOperation(string action, int? id, long? unitId, long? subUnitId, decimal? coefficient, int? fiscalYearId, DateTime? effectiveDateStart, DateTime? effectiveDateEnd, int? userCreatorId, out string message, out int procResult)
        {
            var actionParam = new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = action, Size = 10 };
            var idParam = new SqlParameter { ParameterName = "@Id", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = id.GetValueOrDefault() };
            var unitIdParam = new SqlParameter { ParameterName = "@UnitId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = unitId.GetValueOrDefault() };
            var subUnitIdParam = new SqlParameter { ParameterName = "@SubUnitId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = subUnitId.GetValueOrDefault() };
            var coefficientParam = new SqlParameter { ParameterName = "@Coefficient", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input, Value = coefficient.GetValueOrDefault() };
            var fiscalYearIdParam = new SqlParameter { ParameterName = "@Fiscal_Year_ID", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = fiscalYearId.GetValueOrDefault() };
            var effectiveDateStartParam = new SqlParameter { ParameterName = "@EffectiveDateStart", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = effectiveDateStart.GetValueOrDefault() };
            var effectiveDateEndParam = new SqlParameter { ParameterName = "@EffectiveDateEnd", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, Value = effectiveDateEnd.GetValueOrDefault() };
            var userCreatorIdParam = new SqlParameter { ParameterName = "@UserCreatorId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = userCreatorId.GetValueOrDefault() };
            var messageParam = new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Output };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
 
            var procResultData = Database.SqlQuery<Inventory_UnitConvertsOperationReturnModel>("EXEC @procResult = [Inventory].[UnitConvertsOperation] @Action, @Id, @UnitId, @SubUnitId, @Coefficient, @Fiscal_Year_ID, @EffectiveDateStart, @EffectiveDateEnd, @UserCreatorId, @Message OUTPUT", actionParam, idParam, unitIdParam, subUnitIdParam, coefficientParam, fiscalYearIdParam, effectiveDateStartParam, effectiveDateEndParam, userCreatorIdParam, messageParam, procResultParam).ToList();
            message = default(string);
 
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

    }

    // ************************************************************************
    // Fake Database context
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class FakeMyDbContext : IMyDbContext
    {
        public IDbSet<ActionType> ActionTypes { get; set; }
        public IDbSet<BasicInfo_AccountListView> BasicInfo_AccountListView { get; set; }
        public IDbSet<BasicInfo_ActivityLocation> BasicInfo_ActivityLocation { get; set; }
        public IDbSet<BasicInfo_CompanyGoodUnitView> BasicInfo_CompanyGoodUnitView { get; set; }
        public IDbSet<BasicInfo_CompanyGoodView> BasicInfo_CompanyGoodView { get; set; }
        public IDbSet<BasicInfo_CompanyVesselTankView> BasicInfo_CompanyVesselTankView { get; set; }
        public IDbSet<BasicInfo_CompanyVesselView> BasicInfo_CompanyVesselView { get; set; }
        public IDbSet<BasicInfo_CompanyView> BasicInfo_CompanyView { get; set; }
        public IDbSet<BasicInfo_CurrencyView> BasicInfo_CurrencyView { get; set; }
        public IDbSet<BasicInfo_SharedGoodView> BasicInfo_SharedGoodView { get; set; }
        public IDbSet<BasicInfo_UnitView> BasicInfo_UnitView { get; set; }
        public IDbSet<BasicInfo_UserView> BasicInfo_UserView { get; set; }
        public IDbSet<BasicInfo_VoyagesView> BasicInfo_VoyagesView { get; set; }
        public IDbSet<BasicInfo_VoyagesView1> BasicInfo_VoyagesView1 { get; set; }
        public IDbSet<Fuel_Account> Fuel_Account { get; set; }
        public IDbSet<Fuel_ActivityFlow> Fuel_ActivityFlow { get; set; }
        public IDbSet<Fuel_ApproveFlowConfig> Fuel_ApproveFlowConfig { get; set; }
        public IDbSet<Fuel_ApproveFlowConfigValidFuelUser> Fuel_ApproveFlowConfigValidFuelUser { get; set; }
        public IDbSet<Fuel_AsgnSegmentTypeVoucherSetingDetail> Fuel_AsgnSegmentTypeVoucherSetingDetail { get; set; }
        public IDbSet<Fuel_AsgnVoucherAcont> Fuel_AsgnVoucherAcont { get; set; }
        public IDbSet<Fuel_Attachment> Fuel_Attachment { get; set; }
        public IDbSet<Fuel_Charter> Fuel_Charter { get; set; }
        public IDbSet<Fuel_CharterIn> Fuel_CharterIn { get; set; }
        public IDbSet<Fuel_CharterItem> Fuel_CharterItem { get; set; }
        public IDbSet<Fuel_CharterItemHistory> Fuel_CharterItemHistory { get; set; }
        public IDbSet<Fuel_CharterOut> Fuel_CharterOut { get; set; }
        public IDbSet<Fuel_EffectiveFactor> Fuel_EffectiveFactor { get; set; }
        public IDbSet<Fuel_EovReportsView> Fuel_EovReportsView { get; set; }
        public IDbSet<Fuel_EventLog> Fuel_EventLog { get; set; }
        public IDbSet<Fuel_ExceptionLog> Fuel_ExceptionLog { get; set; }
        public IDbSet<Fuel_FreeAccount> Fuel_FreeAccount { get; set; }
        public IDbSet<Fuel_FuelReport> Fuel_FuelReport { get; set; }
        public IDbSet<Fuel_FuelReportDetail> Fuel_FuelReportDetail { get; set; }
        public IDbSet<Fuel_InventoryOperation> Fuel_InventoryOperation { get; set; }
        public IDbSet<Fuel_Invoice> Fuel_Invoice { get; set; }
        public IDbSet<Fuel_InvoiceAdditionalPrice> Fuel_InvoiceAdditionalPrice { get; set; }
        public IDbSet<Fuel_InvoiceItem> Fuel_InvoiceItem { get; set; }
        public IDbSet<Fuel_JournalEntry> Fuel_JournalEntry { get; set; }
        public IDbSet<Fuel_Log> Fuel_Log { get; set; }
        public IDbSet<Fuel_Offhire> Fuel_Offhire { get; set; }
        public IDbSet<Fuel_OffhireDetail> Fuel_OffhireDetail { get; set; }
        public IDbSet<Fuel_Order> Fuel_Order { get; set; }
        public IDbSet<Fuel_OrderItem> Fuel_OrderItem { get; set; }
        public IDbSet<Fuel_OrderItemBalance> Fuel_OrderItemBalance { get; set; }
        public IDbSet<Fuel_Scrap> Fuel_Scrap { get; set; }
        public IDbSet<Fuel_ScrapDetail> Fuel_ScrapDetail { get; set; }
        public IDbSet<Fuel_Segment> Fuel_Segment { get; set; }
        public IDbSet<Fuel_UserInCompany> Fuel_UserInCompany { get; set; }
        public IDbSet<Fuel_Vessel> Fuel_Vessel { get; set; }
        public IDbSet<Fuel_VesselInCompany> Fuel_VesselInCompany { get; set; }
        public IDbSet<Fuel_Voucher> Fuel_Voucher { get; set; }
        public IDbSet<Fuel_VoucherLog> Fuel_VoucherLog { get; set; }
        public IDbSet<Fuel_VoucherReportView> Fuel_VoucherReportView { get; set; }
        public IDbSet<Fuel_VoucherSeting> Fuel_VoucherSeting { get; set; }
        public IDbSet<Fuel_VoucherSetingDetail> Fuel_VoucherSetingDetail { get; set; }
        public IDbSet<Fuel_Voyage> Fuel_Voyage { get; set; }
        public IDbSet<Fuel_VoyageLog> Fuel_VoyageLog { get; set; }
        public IDbSet<Fuel_Workflow> Fuel_Workflow { get; set; }
        public IDbSet<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; }
        public IDbSet<Fuel_WorkflowLogOld> Fuel_WorkflowLogOld { get; set; }
        public IDbSet<Fuel_WorkflowStep> Fuel_WorkflowStep { get; set; }
        public IDbSet<Group> Groups { get; set; }
        public IDbSet<HafezAccountListView> HafezAccountListViews { get; set; }
        public IDbSet<HafezVoyagesView> HafezVoyagesViews { get; set; }
        public IDbSet<HafizAccountListView> HafizAccountListViews { get; set; }
        public IDbSet<HafizVoyagesView> HafizVoyagesViews { get; set; }
        public IDbSet<Inventory_Company> Inventory_Company { get; set; }
        public IDbSet<Inventory_ErrorMessage> Inventory_ErrorMessage { get; set; }
        public IDbSet<Inventory_FinancialYear> Inventory_FinancialYear { get; set; }
        public IDbSet<Inventory_Good> Inventory_Good { get; set; }
        public IDbSet<Inventory_OperationReference> Inventory_OperationReference { get; set; }
        public IDbSet<Inventory_StoreType> Inventory_StoreType { get; set; }
        public IDbSet<Inventory_TimeBucket> Inventory_TimeBucket { get; set; }
        public IDbSet<Inventory_Transaction> Inventory_Transaction { get; set; }
        public IDbSet<Inventory_TransactionItem> Inventory_TransactionItem { get; set; }
        public IDbSet<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; }
        public IDbSet<Inventory_Unit> Inventory_Unit { get; set; }
        public IDbSet<Inventory_UnitConvert> Inventory_UnitConvert { get; set; }
        public IDbSet<Inventory_User> Inventory_User { get; set; }
        public IDbSet<Inventory_Warehouse> Inventory_Warehouse { get; set; }
        public IDbSet<Offhire_OffhireFuelTypeFuelGoodCode> Offhire_OffhireFuelTypeFuelGoodCode { get; set; }
        public IDbSet<Offhire_OffhireMeasureTypeFuelMeasureCode> Offhire_OffhireMeasureTypeFuelMeasureCode { get; set; }
        public IDbSet<PartiesCustomActions> PartiesCustomActions { get; set; }
        public IDbSet<Party> Parties { get; set; }
        public IDbSet<SapidAccountListView> SapidAccountListViews { get; set; }
        public IDbSet<SapidVoyagesView> SapidVoyagesViews { get; set; }
        public IDbSet<Sysdiagram> Sysdiagrams { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<UsersGroups> UsersGroups { get; set; }
        public IDbSet<VersionInfo> VersionInfoes { get; set; }
        public IDbSet<Vessel_FuelReportCommandLog> Vessel_FuelReportCommandLog { get; set; }
        public IDbSet<Vessel_FuelReportCommandLogDetail> Vessel_FuelReportCommandLogDetail { get; set; }

        public FakeMyDbContext()
        {
            ActionTypes = new FakeDbSet<ActionType>();
            BasicInfo_AccountListView = new FakeDbSet<BasicInfo_AccountListView>();
            BasicInfo_ActivityLocation = new FakeDbSet<BasicInfo_ActivityLocation>();
            BasicInfo_CompanyGoodUnitView = new FakeDbSet<BasicInfo_CompanyGoodUnitView>();
            BasicInfo_CompanyGoodView = new FakeDbSet<BasicInfo_CompanyGoodView>();
            BasicInfo_CompanyVesselTankView = new FakeDbSet<BasicInfo_CompanyVesselTankView>();
            BasicInfo_CompanyVesselView = new FakeDbSet<BasicInfo_CompanyVesselView>();
            BasicInfo_CompanyView = new FakeDbSet<BasicInfo_CompanyView>();
            BasicInfo_CurrencyView = new FakeDbSet<BasicInfo_CurrencyView>();
            BasicInfo_SharedGoodView = new FakeDbSet<BasicInfo_SharedGoodView>();
            BasicInfo_UnitView = new FakeDbSet<BasicInfo_UnitView>();
            BasicInfo_UserView = new FakeDbSet<BasicInfo_UserView>();
            BasicInfo_VoyagesView = new FakeDbSet<BasicInfo_VoyagesView>();
            BasicInfo_VoyagesView1 = new FakeDbSet<BasicInfo_VoyagesView1>();
            Fuel_Account = new FakeDbSet<Fuel_Account>();
            Fuel_ActivityFlow = new FakeDbSet<Fuel_ActivityFlow>();
            Fuel_ApproveFlowConfig = new FakeDbSet<Fuel_ApproveFlowConfig>();
            Fuel_ApproveFlowConfigValidFuelUser = new FakeDbSet<Fuel_ApproveFlowConfigValidFuelUser>();
            Fuel_AsgnSegmentTypeVoucherSetingDetail = new FakeDbSet<Fuel_AsgnSegmentTypeVoucherSetingDetail>();
            Fuel_AsgnVoucherAcont = new FakeDbSet<Fuel_AsgnVoucherAcont>();
            Fuel_Attachment = new FakeDbSet<Fuel_Attachment>();
            Fuel_Charter = new FakeDbSet<Fuel_Charter>();
            Fuel_CharterIn = new FakeDbSet<Fuel_CharterIn>();
            Fuel_CharterItem = new FakeDbSet<Fuel_CharterItem>();
            Fuel_CharterItemHistory = new FakeDbSet<Fuel_CharterItemHistory>();
            Fuel_CharterOut = new FakeDbSet<Fuel_CharterOut>();
            Fuel_EffectiveFactor = new FakeDbSet<Fuel_EffectiveFactor>();
            Fuel_EovReportsView = new FakeDbSet<Fuel_EovReportsView>();
            Fuel_EventLog = new FakeDbSet<Fuel_EventLog>();
            Fuel_ExceptionLog = new FakeDbSet<Fuel_ExceptionLog>();
            Fuel_FreeAccount = new FakeDbSet<Fuel_FreeAccount>();
            Fuel_FuelReport = new FakeDbSet<Fuel_FuelReport>();
            Fuel_FuelReportDetail = new FakeDbSet<Fuel_FuelReportDetail>();
            Fuel_InventoryOperation = new FakeDbSet<Fuel_InventoryOperation>();
            Fuel_Invoice = new FakeDbSet<Fuel_Invoice>();
            Fuel_InvoiceAdditionalPrice = new FakeDbSet<Fuel_InvoiceAdditionalPrice>();
            Fuel_InvoiceItem = new FakeDbSet<Fuel_InvoiceItem>();
            Fuel_JournalEntry = new FakeDbSet<Fuel_JournalEntry>();
            Fuel_Log = new FakeDbSet<Fuel_Log>();
            Fuel_Offhire = new FakeDbSet<Fuel_Offhire>();
            Fuel_OffhireDetail = new FakeDbSet<Fuel_OffhireDetail>();
            Fuel_Order = new FakeDbSet<Fuel_Order>();
            Fuel_OrderItem = new FakeDbSet<Fuel_OrderItem>();
            Fuel_OrderItemBalance = new FakeDbSet<Fuel_OrderItemBalance>();
            Fuel_Scrap = new FakeDbSet<Fuel_Scrap>();
            Fuel_ScrapDetail = new FakeDbSet<Fuel_ScrapDetail>();
            Fuel_Segment = new FakeDbSet<Fuel_Segment>();
            Fuel_UserInCompany = new FakeDbSet<Fuel_UserInCompany>();
            Fuel_Vessel = new FakeDbSet<Fuel_Vessel>();
            Fuel_VesselInCompany = new FakeDbSet<Fuel_VesselInCompany>();
            Fuel_Voucher = new FakeDbSet<Fuel_Voucher>();
            Fuel_VoucherLog = new FakeDbSet<Fuel_VoucherLog>();
            Fuel_VoucherReportView = new FakeDbSet<Fuel_VoucherReportView>();
            Fuel_VoucherSeting = new FakeDbSet<Fuel_VoucherSeting>();
            Fuel_VoucherSetingDetail = new FakeDbSet<Fuel_VoucherSetingDetail>();
            Fuel_Voyage = new FakeDbSet<Fuel_Voyage>();
            Fuel_VoyageLog = new FakeDbSet<Fuel_VoyageLog>();
            Fuel_Workflow = new FakeDbSet<Fuel_Workflow>();
            Fuel_WorkflowLog = new FakeDbSet<Fuel_WorkflowLog>();
            Fuel_WorkflowLogOld = new FakeDbSet<Fuel_WorkflowLogOld>();
            Fuel_WorkflowStep = new FakeDbSet<Fuel_WorkflowStep>();
            Groups = new FakeDbSet<Group>();
            HafezAccountListViews = new FakeDbSet<HafezAccountListView>();
            HafezVoyagesViews = new FakeDbSet<HafezVoyagesView>();
            HafizAccountListViews = new FakeDbSet<HafizAccountListView>();
            HafizVoyagesViews = new FakeDbSet<HafizVoyagesView>();
            Inventory_Company = new FakeDbSet<Inventory_Company>();
            Inventory_ErrorMessage = new FakeDbSet<Inventory_ErrorMessage>();
            Inventory_FinancialYear = new FakeDbSet<Inventory_FinancialYear>();
            Inventory_Good = new FakeDbSet<Inventory_Good>();
            Inventory_OperationReference = new FakeDbSet<Inventory_OperationReference>();
            Inventory_StoreType = new FakeDbSet<Inventory_StoreType>();
            Inventory_TimeBucket = new FakeDbSet<Inventory_TimeBucket>();
            Inventory_Transaction = new FakeDbSet<Inventory_Transaction>();
            Inventory_TransactionItem = new FakeDbSet<Inventory_TransactionItem>();
            Inventory_TransactionItemPrice = new FakeDbSet<Inventory_TransactionItemPrice>();
            Inventory_Unit = new FakeDbSet<Inventory_Unit>();
            Inventory_UnitConvert = new FakeDbSet<Inventory_UnitConvert>();
            Inventory_User = new FakeDbSet<Inventory_User>();
            Inventory_Warehouse = new FakeDbSet<Inventory_Warehouse>();
            Offhire_OffhireFuelTypeFuelGoodCode = new FakeDbSet<Offhire_OffhireFuelTypeFuelGoodCode>();
            Offhire_OffhireMeasureTypeFuelMeasureCode = new FakeDbSet<Offhire_OffhireMeasureTypeFuelMeasureCode>();
            PartiesCustomActions = new FakeDbSet<PartiesCustomActions>();
            Parties = new FakeDbSet<Party>();
            SapidAccountListViews = new FakeDbSet<SapidAccountListView>();
            SapidVoyagesViews = new FakeDbSet<SapidVoyagesView>();
            Sysdiagrams = new FakeDbSet<Sysdiagram>();
            Users = new FakeDbSet<User>();
            UsersGroups = new FakeDbSet<UsersGroups>();
            VersionInfoes = new FakeDbSet<VersionInfo>();
            Vessel_FuelReportCommandLog = new FakeDbSet<Vessel_FuelReportCommandLog>();
            Vessel_FuelReportCommandLogDetail = new FakeDbSet<Vessel_FuelReportCommandLogDetail>();
        }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        
        // Stored Procedures
        public int Fuel_GetFuelOriginalData(string code)
        {
 
            return 0;
        }

        public int Fuel_GetVesselReportData(string shipCode, string voyageNo, DateTime? fromDate, DateTime? toDate, double? portTime, double? portTimeMol, string locationType)
        {
 
            return 0;
        }

        public int Fuel_GetVesselReportShipNameData()
        {
 
            return 0;
        }

        public int Fuel_GetVesselReportVoyageData(string shipCode)
        {
 
            return 0;
        }

        public List<Fuel_PeriodicalFuelStatisticsReturnModel> Fuel_PeriodicalFuelStatistics(long? companyId, long? warehouseId, long? quantityUnitId, long? goodId, DateTime? from, DateTime? to, out int procResult)
        {
 
            procResult = 0;
            return new List<Fuel_PeriodicalFuelStatisticsReturnModel>();
        }

        public int Inventory_ActivateWarehouseIncludingRecieptsOperation(string description, long? warehouseId, int? timeBucketId, int? storeTypesId, DateTime? registrationDate, string referenceType, string referenceNo, string transactionItems, int? userCreatorId)
        {
 
            return 0;
        }

        public List<Inventory_CardexReturnModel> Inventory_Cardex(long? warehouseId, long? goodId, DateTime? startDate, DateTime? endDate, out int procResult)
        {
 
            procResult = 0;
            return new List<Inventory_CardexReturnModel>();
        }

        public int Inventory_ChangeCoefficientAndUpdatePriceByUnitConvertId(int? unitConvertId, out string message)
        {
            message = default(string);
 
            return 0;
        }

        public List<Inventory_ChangeWarehouseStatusReturnModel> Inventory_ChangeWarehouseStatus(bool? isActive, long? warehouseId, int? userCreatorId, out int procResult)
        {
 
            procResult = 0;
            return new List<Inventory_ChangeWarehouseStatusReturnModel>();
        }

        public int Inventory_ErrorHandling()
        {
 
            return 0;
        }

        public List<Inventory_IsValidTransactionCodeReturnModel> Inventory_IsValidTransactionCode(byte? action, decimal? code, long? warehouseId, DateTime? registrationDate, int? timeBucketId, out int procResult)
        {
 
            procResult = 0;
            return new List<Inventory_IsValidTransactionCodeReturnModel>();
        }

        public int Inventory_PriceAllSuspendedIssuedItems(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int? userCreatorId, out string transactionItemPriceIds, out string message)
        {
            transactionItemPriceIds = default(string);
            message = default(string);
 
            return 0;
        }

        public int Inventory_PriceAllSuspendedTransactionItemsUsingReference(long? companyId, long? warehouseId, DateTime? fromDate, DateTime? toDate, int? userCreatorId, byte? action, out string transactionItemPriceIds, out string message)
        {
            transactionItemPriceIds = default(string);
            message = default(string);
 
            return 0;
        }

        public int Inventory_PriceGivenIssuedItems(int? userCreatorId, string issueItemIds, out string transactionItemPriceIds, out string message, out int? notPricedTransactionId)
        {
            transactionItemPriceIds = default(string);
            message = default(string);
            notPricedTransactionId = default(int);
 
            return 0;
        }

        public int Inventory_PriceSuspendedTransactionUsingReference(int? transactionId, string description, int? userCreatorId, out string transactionItemPriceIds, out string message)
        {
            transactionItemPriceIds = default(string);
            message = default(string);
 
            return 0;
        }

        public int Inventory_RemoveTransactionItemPrices(int? transactionItemId, int? userId, out string message)
        {
            message = default(string);
 
            return 0;
        }

        public List<Inventory_TransactionItemPricesGetAllReturnModel> Inventory_TransactionItemPricesGetAll( out int procResult)
        {
 
            procResult = 0;
            return new List<Inventory_TransactionItemPricesGetAllReturnModel>();
        }

        public int Inventory_TransactionItemPricesOperation(string action, int? userCreatorId, string transactionItemPrices, out string transactionItemPriceIds, out string message)
        {
            transactionItemPriceIds = default(string);
            message = default(string);
 
            return 0;
        }

        public List<Inventory_TransactionItemsGetAllReturnModel> Inventory_TransactionItemsGetAll( out int procResult)
        {
 
            procResult = 0;
            return new List<Inventory_TransactionItemsGetAllReturnModel>();
        }

        public int Inventory_TransactionItemsOperation(string action, int? transactionId, int? userCreatorId, string transactionItems, out string transactionItemsId, out string message)
        {
            transactionItemsId = default(string);
            message = default(string);
 
            return 0;
        }

        public int Inventory_TransactionOperation(string action, int? id, byte? transactionAction, string description, long? companyId, long? warehouseId, int? timeBucketId, int? storeTypesId, int? pricingReferenceId, byte? status, DateTime? registrationDate, string referenceType, string referenceNo, int? userCreatorId, out int? transactionId, out decimal? code, out string message)
        {
            transactionId = default(int);
            code = default(decimal);
            message = default(string);
 
            return 0;
        }

        public List<Inventory_TransactionsGetAllReturnModel> Inventory_TransactionsGetAll( out int procResult)
        {
 
            procResult = 0;
            return new List<Inventory_TransactionsGetAllReturnModel>();
        }

        public List<Inventory_UnitConvertsOperationReturnModel> Inventory_UnitConvertsOperation(string action, int? id, long? unitId, long? subUnitId, decimal? coefficient, int? fiscalYearId, DateTime? effectiveDateStart, DateTime? effectiveDateEnd, int? userCreatorId, out string message, out int procResult)
        {
            message = default(string);
 
            procResult = 0;
            return new List<Inventory_UnitConvertsOperationReturnModel>();
        }

    }

    // ************************************************************************
    // Fake DbSet
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class FakeDbSet<T> : IDbSet<T> where T : class
    {
        private readonly HashSet<T> _data;

        public FakeDbSet()
        {
            _data = new HashSet<T>();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public void Detach(T item)
        {
            _data.Remove(item);
        }

        Type IQueryable.ElementType
        {
            get { return _data.AsQueryable().ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _data.AsQueryable().Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _data.AsQueryable().Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public ObservableCollection<T> Local
        {
            get
            {
                return new ObservableCollection<T>(_data);
            }
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }
    }

    // ************************************************************************
    // POCO classes

    // ActionTypes
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class ActionType
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<Fuel_ActivityFlow> Fuel_ActivityFlow { get; set; } // ActivityFlow.FK_Fuel.ActivityFlow_ActionTypeId_dbo.ActionTypes_Id
        public virtual ICollection<PartiesCustomActions> PartiesCustomActions { get; set; } // Parties_CustomActions.FK_Parties_CustomActions_ActionTypeId_ActionTypes_Id
        
        public ActionType()
        {
            Fuel_ActivityFlow = new List<Fuel_ActivityFlow>();
            PartiesCustomActions = new List<PartiesCustomActions>();
        }
    }

    // AccountListView
    internal class BasicInfo_AccountListView
    {
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
    }

    // ActivityLocation
    internal class BasicInfo_ActivityLocation
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
        public string Abbreviation { get; set; } // Abbreviation
        public double? Latitude { get; set; } // Latitude
        public double? Longitude { get; set; } // Longitude
        public string CountryName { get; set; } // CountryName
        
        public BasicInfo_ActivityLocation()
        {
            CountryName = "";
        }
    }

    // CompanyGoodUnitView
    internal class BasicInfo_CompanyGoodUnitView
    {
        public long? Id { get; set; } // Id
        public long? CompanyGoodId { get; set; } // CompanyGoodId
        public long SharedGoodId { get; set; } // SharedGoodId
        public long CompanyId { get; set; } // CompanyId
        public string Name { get; set; } // Name
        public string Abbreviation { get; set; } // Abbreviation
        public string To { get; set; } // To
        public decimal? Coefficient { get; set; } // Coefficient
        public decimal? Offset { get; set; } // Offset
        public long? ParentId { get; set; } // ParentId
    }

    // CompanyGoodView
    internal class BasicInfo_CompanyGoodView
    {
        public long? Id { get; set; } // Id
        public long SharedGoodId { get; set; } // SharedGoodId
        public long CompanyId { get; set; } // CompanyId
        public string Name { get; set; } // Name
        public string Code { get; set; } // Code
    }

    // CompanyVesselTankView
    internal class BasicInfo_CompanyVesselTankView
    {
        public long? Id { get; set; } // Id
        public long VesselInInventoryId { get; set; } // VesselInInventoryId
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
    }

    // CompanyVesselView
    internal class BasicInfo_CompanyVesselView
    {
        public long Id { get; set; } // Id
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
        public long CompanyId { get; set; } // CompanyId
        public bool? IsActive { get; set; } // IsActive
    }

    // CompanyView
    internal class BasicInfo_CompanyView
    {
        public long Id { get; set; } // Id
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
    }

    // CurrencyView
    internal class BasicInfo_CurrencyView
    {
        public long Id { get; set; } // Id
        public string Abbreviation { get; set; } // Abbreviation
        public string Name { get; set; } // Name
    }

    // SharedGoodView
    internal class BasicInfo_SharedGoodView
    {
        public long Id { get; set; } // Id
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
        public long MainUnitId { get; set; } // MainUnitId
        public string MainUnitCode { get; set; } // MainUnitCode
    }

    // UnitView
    internal class BasicInfo_UnitView
    {
        public long Id { get; set; } // Id
        public string Abbreviation { get; set; } // Abbreviation
        public string Name { get; set; } // Name
    }

    // UserView
    internal class BasicInfo_UserView
    {
        public long Id { get; set; } // Id
        public long IdentityId { get; set; } // IdentityId
        public string Name { get; set; } // Name
        public long CompanyId { get; set; } // CompanyId
        public bool? IsFrApprover { get; set; } // IsFRApprover
    }

    // __VoyagesView
    internal class BasicInfo_VoyagesView
    {
        public long? Id { get; set; } // Id
        public string VoyageNumber { get; set; } // VoyageNumber
        public string Description { get; set; } // Description
        public long? CompanyId { get; set; } // CompanyId
        public long VesselInCompanyId { get; set; } // VesselInCompanyId
        public DateTime? StartDate { get; set; } // StartDate
        public DateTime? EndDate { get; set; } // EndDate
        public bool IsActive { get; set; } // IsActive
    }

    // VoyagesView
    internal class BasicInfo_VoyagesView1
    {
        public long? Id { get; set; } // Id
        public string VoyageNumber { get; set; } // VoyageNumber
        public string Description { get; set; } // Description
        public long? CompanyId { get; set; } // CompanyId
        public long VesselInCompanyId { get; set; } // VesselInCompanyId
        public DateTime? StartDate { get; set; } // StartDate
        public DateTime? EndDate { get; set; } // EndDate
        public bool IsActive { get; set; } // IsActive
    }

    // Accounts
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Account
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public byte[] TimeStamp { get; set; } // TimeStamp
        public string Code { get; set; } // Code

        // Reverse navigation
        public virtual ICollection<Fuel_AsgnVoucherAcont> Fuel_AsgnVoucherAcont { get; set; } // AsgnVoucherAconts.FK_Account_AsgnVoucherAcont
        public virtual ICollection<Fuel_EffectiveFactor> Fuel_EffectiveFactor { get; set; } // EffectiveFactor.FK_EffectiveFactor_Account
        
        public Fuel_Account()
        {
            Fuel_AsgnVoucherAcont = new List<Fuel_AsgnVoucherAcont>();
            Fuel_EffectiveFactor = new List<Fuel_EffectiveFactor>();
        }
    }

    // ActivityFlow
    internal class Fuel_ActivityFlow
    {
        public long Id { get; set; } // Id (Primary key)
        public long WorkflowStepId { get; set; } // WorkflowStepId
        public long WorkflowNextStepId { get; set; } // WorkflowNextStepId
        public int WorkflowAction { get; set; } // WorkflowAction
        public int ActionTypeId { get; set; } // ActionTypeId

        // Foreign keys
        public virtual ActionType ActionType { get; set; } // FK_Fuel.ActivityFlow_ActionTypeId_dbo.ActionTypes_Id
        public virtual Fuel_WorkflowStep Fuel_WorkflowStep_WorkflowNextStepId { get; set; } // FK_Fuel.ActivityFlow_WorkflowNextStepId_Fuel.WorkflowStep_Id
        public virtual Fuel_WorkflowStep Fuel_WorkflowStep_WorkflowStepId { get; set; } // FK_Fuel.ActivityFlow_WorkflowStepId_Fuel.WorkflowStep_Id
    }

    // ApproveFlowConfig
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_ApproveFlowConfig
    {
        public long Id { get; set; } // Id (Primary key)
        public long ActorUserId { get; set; } // ActorUserId
        public long? NextWorkflowStepId { get; set; } // NextWorkflowStepId
        public int WithWorkflowAction { get; set; } // WithWorkflowAction
        public int State { get; set; } // State
        public int WorkflowEntity { get; set; } // WorkflowEntity
        public int CurrentWorkflowStage { get; set; } // CurrentWorkflowStage
        public byte[] RowVersion { get; set; } // RowVersion

        // Reverse navigation
        public virtual ICollection<Fuel_ApproveFlowConfig> Fuel_ApproveFlowConfig2 { get; set; } // ApproveFlowConfig.FK_ApproveFlowConfig_NextWorkflowStepId_ApproveFlowConfig_Id
        public virtual ICollection<Fuel_ApproveFlowConfigValidFuelUser> Fuel_ApproveFlowConfigValidFuelUser { get; set; } // Many to many mapping

        // Foreign keys
        public virtual Fuel_ApproveFlowConfig Fuel_ApproveFlowConfig1 { get; set; } // FK_ApproveFlowConfig_NextWorkflowStepId_ApproveFlowConfig_Id
        
        public Fuel_ApproveFlowConfig()
        {
            Fuel_ApproveFlowConfig2 = new List<Fuel_ApproveFlowConfig>();
            Fuel_ApproveFlowConfigValidFuelUser = new List<Fuel_ApproveFlowConfigValidFuelUser>();
        }
    }

    // ApproveFlowConfigValidFuelUsers
    internal class Fuel_ApproveFlowConfigValidFuelUser
    {
        public long ApproveFlowConfigId { get; set; } // ApproveFlowConfigId (Primary key)
        public long FuelUserId { get; set; } // FuelUserId (Primary key)

        // Foreign keys
        public virtual Fuel_ApproveFlowConfig Fuel_ApproveFlowConfig { get; set; } // FK_ApproveFlowConfigValidFuelUsers_ApproveFlowConfigId_ApproveFlowConfig_Id
    }

    // AsgnSegmentTypeVoucherSetingDetail
    internal class Fuel_AsgnSegmentTypeVoucherSetingDetail
    {
        public int Id { get; set; } // Id (Primary key)
        public int Type { get; set; } // Type
        public long VoucherSetingDetailId { get; set; } // VoucherSetingDetailId
        public int SegmentTypeId { get; set; } // SegmentTypeId

        // Foreign keys
        public virtual Fuel_VoucherSetingDetail Fuel_VoucherSetingDetail { get; set; } // FK_VoucherSetingDetail_AsgnSegmentTypeVoucherSetingDetail
    }

    // AsgnVoucherAconts
    internal class Fuel_AsgnVoucherAcont
    {
        public int Id { get; set; } // Id (Primary key)
        public long VoucherSetingDetailId { get; set; } // VoucherSetingDetailId
        public int AccountId { get; set; } // AccountId
        public int Type { get; set; } // Type
        public byte[] TimeStamp { get; set; } // TimeStamp

        // Foreign keys
        public virtual Fuel_Account Fuel_Account { get; set; } // FK_Account_AsgnVoucherAcont
        public virtual Fuel_VoucherSetingDetail Fuel_VoucherSetingDetail { get; set; } // FK_VoucherSetingDetail_AsgnVoucherAconts
    }

    // Attachments
    internal class Fuel_Attachment
    {
        public int RowId { get; set; } // RowID (Primary key)
        public byte[] AttachmentContent { get; set; } // AttachmentContent
        public string AttachmentName { get; set; } // AttachmentName
        public string AttachmentExt { get; set; } // AttachmentExt
        public long EntityId { get; set; } // EntityId
        public int EntityType { get; set; } // EntityType
        public Guid RowGuid { get; set; } // RowGUID
        
        public Fuel_Attachment()
        {
            RowGuid = System.Guid.NewGuid();
        }
    }

    // Charter
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Charter
    {
        public long Id { get; set; } // Id (Primary key)
        public int CurrentState { get; set; } // CurrentState
        public DateTime ActionDate { get; set; } // ActionDate
        public int CharterType { get; set; } // CharterType
        public int CharterEndType { get; set; } // CharterEndType
        public long ChartererId { get; set; } // ChartererId
        public long OwnerId { get; set; } // OwnerId
        public long VesselInCompanyId { get; set; } // VesselInCompanyId
        public long CurrencyId { get; set; } // CurrencyId
        public byte[] TimeStamp { get; set; } // TimeStamp

        // Reverse navigation
        public virtual ICollection<Fuel_CharterIn> Fuel_CharterIn { get; set; } // Many to many mapping
        public virtual ICollection<Fuel_CharterItem> Fuel_CharterItem { get; set; } // CharterItem.FK_CharterItem_CharterId_Charter_Id
        public virtual ICollection<Fuel_CharterOut> Fuel_CharterOut { get; set; } // Many to many mapping
        public virtual ICollection<Fuel_FuelReport> Fuel_FuelReport { get; set; } // FuelReport.FK_FuelReport_CreatedCharterId_Charter_Id
        public virtual ICollection<Fuel_InventoryOperation> Fuel_InventoryOperation { get; set; } // InventoryOperation.FK_InventoryOperation_CharterId_Charter_Id
        public virtual ICollection<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog.FK_WorkflowLog_CharterId_Charter_Id

        // Foreign keys
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany { get; set; } // FK_Charter_VesselInCompanyId_VesselInCompany_Id
        
        public Fuel_Charter()
        {
            Fuel_CharterIn = new List<Fuel_CharterIn>();
            Fuel_CharterItem = new List<Fuel_CharterItem>();
            Fuel_CharterOut = new List<Fuel_CharterOut>();
            Fuel_FuelReport = new List<Fuel_FuelReport>();
            Fuel_InventoryOperation = new List<Fuel_InventoryOperation>();
            Fuel_WorkflowLog = new List<Fuel_WorkflowLog>();
        }
    }

    // CharterIn
    internal class Fuel_CharterIn
    {
        public long Id { get; set; } // Id
        public int OffHirePricingType { get; set; } // OffHirePricingType

        // Foreign keys
        public virtual Fuel_Charter Fuel_Charter { get; set; } // FK_CharterIn_Id_Charter_Id
    }

    // CharterItem
    internal class Fuel_CharterItem
    {
        public long Id { get; set; } // Id (Primary key)
        public long CharterId { get; set; } // CharterId
        public long GoodUnitId { get; set; } // GoodUnitId
        public long GoodId { get; set; } // GoodId
        public long? TankId { get; set; } // TankId
        public decimal Rob { get; set; } // Rob
        public decimal Fee { get; set; } // Fee
        public decimal OffhireFee { get; set; } // OffhireFee
        public byte[] TimeStamp { get; set; } // TimeStamp

        // Foreign keys
        public virtual Fuel_Charter Fuel_Charter { get; set; } // FK_CharterItem_CharterId_Charter_Id
    }

    // CharterItemHistory
    internal class Fuel_CharterItemHistory
    {
        public long Id { get; set; } // Id (Primary key)
        public long CharterId { get; set; } // CharterId
        public long CharterItemId { get; set; } // CharterItemId
        public long GoodUnitId { get; set; } // GoodUnitId
        public int CharterStateId { get; set; } // CharterStateId
        public long GoodId { get; set; } // GoodId
        public long? TankId { get; set; } // TankId
        public decimal Rob { get; set; } // Rob
        public decimal Fee { get; set; } // Fee
        public decimal OffhireFee { get; set; } // OffhireFee
        public DateTime DateRegisterd { get; set; } // DateRegisterd
        public byte[] TimeStamp { get; set; } // TimeStamp
    }

    // CharterOut
    internal class Fuel_CharterOut
    {
        public long Id { get; set; } // Id
        public int OffHirePricingType { get; set; } // OffHirePricingType

        // Foreign keys
        public virtual Fuel_Charter Fuel_Charter { get; set; } // FK_CharterOut_Id_Charter_Id
    }

    // EffectiveFactor
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_EffectiveFactor
    {
        public long Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public int EffectiveFactorType { get; set; } // EffectiveFactorType
        public byte[] TimeStamp { get; set; } // TimeStamp
        public int? AccountId { get; set; } // AccountId
        public string VoucherDescription { get; set; } // VoucherDescription
        public string VoucherRefDescription { get; set; } // VoucherRefDescription

        // Reverse navigation
        public virtual ICollection<Fuel_InvoiceAdditionalPrice> Fuel_InvoiceAdditionalPrice { get; set; } // InvoiceAdditionalPrices.FK_InvoiceAdditionalPrices_EffectiveFactorId_EffectiveFactor_Id
        public virtual ICollection<Fuel_Segment> Fuel_Segment { get; set; } // Segments.FK_Segment_EffectiveFactor

        // Foreign keys
        public virtual Fuel_Account Fuel_Account { get; set; } // FK_EffectiveFactor_Account
        
        public Fuel_EffectiveFactor()
        {
            Fuel_InvoiceAdditionalPrice = new List<Fuel_InvoiceAdditionalPrice>();
            Fuel_Segment = new List<Fuel_Segment>();
        }
    }

    // EOVReportsView
    internal class Fuel_EovReportsView
    {
        public int DraftId { get; set; } // DraftID
        public int? ShipId { get; set; } // ShipID
        public int? ConsNo { get; set; } // ConsNo
        public string ShipName { get; set; } // ShipName
        public string VoyageNo { get; set; } // VoyageNo
        public int? Year { get; set; } // Year
        public int? Month { get; set; } // Month
        public int? Day { get; set; } // Day
        public TimeSpan? Time { get; set; } // Time
        public int FuelReportType { get; set; } // FuelReportType
        public string PortName { get; set; } // PortName
        public int PortTime { get; set; } // PortTime
        public int AtSeaLatitudeDegree { get; set; } // AtSeaLatitudeDegree
        public int AtSeaLatitudeMinute { get; set; } // AtSeaLatitudeMinute
        public int AtSeaLongitudeDegree { get; set; } // AtSeaLongitudeDegree
        public int AtSeaLongitudeMinute { get; set; } // AtSeaLongitudeMinute
        public int ObsDist { get; set; } // ObsDist
        public int EngDist { get; set; } // EngDist
        public int SteamTime { get; set; } // SteamTime
        public int AvObsSpeed { get; set; } // AvObsSpeed
        public int AvEngSpeed { get; set; } // AvEngSpeed
        public int Rpm { get; set; } // RPM
        public int Slip { get; set; } // Slip
        public int WindDir { get; set; } // WindDir
        public int WindForce { get; set; } // WindForce
        public int SeaDir { get; set; } // SeaDir
        public int SeaForce { get; set; } // SeaForce
        public int Robho { get; set; } // ROBHO
        public int Robdo { get; set; } // ROBDO
        public int Robmgo { get; set; } // ROBMGO
        public int Robfw { get; set; } // ROBFW
        public int ConsInPortHo { get; set; } // ConsInPortHO
        public int ConsInPortDo { get; set; } // ConsInPortDO
        public int ConsInPortMgo { get; set; } // ConsInPortMGO
        public int ConsInPortFw { get; set; } // ConsInPortFW
        public int ConsAtSeaHo { get; set; } // ConsAtSeaHO
        public int ConsAtSeaDo { get; set; } // ConsAtSeaDO
        public int ConsAtSeaMgo { get; set; } // ConsAtSeaMGO
        public int ConsAtSeaFw { get; set; } // ConsAtSeaFW
        public int ReceivedHo { get; set; } // ReceivedHO
        public int ReceivedDo { get; set; } // ReceivedDO
        public int ReceivedMgo { get; set; } // ReceivedMGO
        public int ReceivedFw { get; set; } // ReceivedFW
        public string EtaPort { get; set; } // ETAPort
        public string EtaDate { get; set; } // ETADate
        public DateTime DateIn { get; set; } // DateIn
        public int IsSm { get; set; } // IsSM
        public int InPortOrAtSea { get; set; } // InPortOrAtSea
        public string ImportDate { get; set; } // ImportDate
        public string ShipCode { get; set; } // ShipCode
        public long? VoyageId { get; set; } // VoyageId
    }

    // EventLogs
    internal class Fuel_EventLog
    {
        public long Id { get; set; } // Id

        // Foreign keys
        public virtual Fuel_Log Fuel_Log { get; set; } // FK_EventLogs_Id_Logs_Id
    }

    // ExceptionLogs
    internal class Fuel_ExceptionLog
    {
        public long Id { get; set; } // Id

        // Foreign keys
        public virtual Fuel_Log Fuel_Log { get; set; } // FK_ExceptionLogs_Id_Logs_Id
    }

    // FreeAccounts
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_FreeAccount
    {
        public long Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Code { get; set; } // Code

        // Reverse navigation
        public virtual ICollection<Fuel_Segment> Fuel_Segment { get; set; } // Segments.FK_Segment_FreeAccount
        
        public Fuel_FreeAccount()
        {
            Fuel_Segment = new List<Fuel_Segment>();
        }
    }

    // FuelReport
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_FuelReport
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Description { get; set; } // Description
        public DateTime EventDate { get; set; } // EventDate
        public DateTime ReportDate { get; set; } // ReportDate
        public long VesselInCompanyId { get; set; } // VesselInCompanyId
        public long? VoyageId { get; set; } // VoyageId
        public int FuelReportType { get; set; } // FuelReportType
        public byte[] TimeStamp { get; set; } // TimeStamp
        public int State { get; set; } // State
        public long? CreatedCharterId { get; set; } // CreatedCharterId

        // Reverse navigation
        public virtual ICollection<Fuel_FuelReportDetail> Fuel_FuelReportDetail { get; set; } // FuelReportDetail.FK_FuelReportDetail_FuelReportId_FuelReport_Id
        public virtual ICollection<Fuel_InventoryOperation> Fuel_InventoryOperation { get; set; } // InventoryOperation.FK_InventoryOperation_Id_FuelReport_Id
        public virtual ICollection<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog.FK_WorkflowLog_FuelReportId_FuelReport_Id

        // Foreign keys
        public virtual Fuel_Charter Fuel_Charter { get; set; } // FK_FuelReport_CreatedCharterId_Charter_Id
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany { get; set; } // FK_FuelReport_VesselInCompanyId_VesselInCompany_Id
        
        public Fuel_FuelReport()
        {
            Fuel_FuelReportDetail = new List<Fuel_FuelReportDetail>();
            Fuel_InventoryOperation = new List<Fuel_InventoryOperation>();
            Fuel_WorkflowLog = new List<Fuel_WorkflowLog>();
        }
    }

    // FuelReportDetail
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_FuelReportDetail
    {
        public long Id { get; set; } // Id (Primary key)
        public long FuelReportId { get; set; } // FuelReportId
        public long GoodId { get; set; } // GoodId
        public long? TankId { get; set; } // TankId
        public decimal Consumption { get; set; } // Consumption
        public decimal Rob { get; set; } // ROB
        public string Robuom { get; set; } // ROBUOM
        public decimal? Receive { get; set; } // Receive
        public int? ReceiveType { get; set; } // ReceiveType
        public int? ReceiveReferenceReferenceType { get; set; } // ReceiveReference_ReferenceType
        public long? ReceiveReferenceReferenceId { get; set; } // ReceiveReference_ReferenceId
        public string ReceiveReferenceCode { get; set; } // ReceiveReference_Code
        public decimal? Transfer { get; set; } // Transfer
        public int? TransferType { get; set; } // TransferType
        public int? TransferReferenceReferenceType { get; set; } // TransferReference_ReferenceType
        public long? TransferReferenceReferenceId { get; set; } // TransferReference_ReferenceId
        public string TransferReferenceCode { get; set; } // TransferReference_Code
        public decimal? Correction { get; set; } // Correction
        public decimal? CorrectionPrice { get; set; } // CorrectionPrice
        public int? CorrectionType { get; set; } // CorrectionType
        public int? CorrectionReferenceReferenceType { get; set; } // CorrectionReference_ReferenceType
        public long? CorrectionReferenceReferenceId { get; set; } // CorrectionReference_ReferenceId
        public string CorrectionReferenceCode { get; set; } // CorrectionReference_Code
        public long? CorrectionPriceCurrencyId { get; set; } // CorrectionPriceCurrencyId
        public string CorrectionPriceCurrencyIsoCode { get; set; } // CorrectionPriceCurrencyISOCode
        public long MeasuringUnitId { get; set; } // MeasuringUnitId
        public byte[] TimeStamp { get; set; } // TimeStamp
        public int? CorrectionPricingType { get; set; } // CorrectionPricingType

        // Reverse navigation
        public virtual ICollection<Fuel_InventoryOperation> Fuel_InventoryOperation { get; set; } // InventoryOperation.FK_InventoryOperation_FuelReportDetailId_FuelReportDetail_Id
        public virtual ICollection<Fuel_OrderItemBalance> Fuel_OrderItemBalance { get; set; } // OrderItemBalances.FK_OrderItemBalances_FuelReportDetailId_FuelReportDetail_Id

        // Foreign keys
        public virtual Fuel_FuelReport Fuel_FuelReport { get; set; } // FK_FuelReportDetail_FuelReportId_FuelReport_Id
        
        public Fuel_FuelReportDetail()
        {
            Fuel_InventoryOperation = new List<Fuel_InventoryOperation>();
            Fuel_OrderItemBalance = new List<Fuel_OrderItemBalance>();
        }
    }

    // InventoryOperation
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_InventoryOperation
    {
        public long Id { get; set; } // Id (Primary key)
        public string ActionNumber { get; set; } // ActionNumber
        public DateTime ActionDate { get; set; } // ActionDate
        public int ActionType { get; set; } // ActionType
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long? FuelReportDetailId { get; set; } // FuelReportDetailId
        public long? CharterId { get; set; } // CharterId
        public long? ScrapId { get; set; } // Scrap_Id
        public long? FuelReportId { get; set; } // FuelReport_Id
        public long InventoryOperationId { get; set; } // InventoryOperationId

        // Reverse navigation
        public virtual ICollection<Fuel_OrderItemBalance> Fuel_OrderItemBalance { get; set; } // OrderItemBalances.FK_OrderItemBalances_InventoryOperationId_InventoryOperation_Id

        // Foreign keys
        public virtual Fuel_Charter Fuel_Charter { get; set; } // FK_InventoryOperation_CharterId_Charter_Id
        public virtual Fuel_FuelReport Fuel_FuelReport { get; set; } // FK_InventoryOperation_Id_FuelReport_Id
        public virtual Fuel_FuelReportDetail Fuel_FuelReportDetail { get; set; } // FK_InventoryOperation_FuelReportDetailId_FuelReportDetail_Id
        public virtual Fuel_Scrap Fuel_Scrap { get; set; } // FK_InventoryOperation_Scrap_Id_Scrap_Id
        
        public Fuel_InventoryOperation()
        {
            InventoryOperationId = -1;
            Fuel_OrderItemBalance = new List<Fuel_OrderItemBalance>();
        }
    }

    // Invoice
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Invoice
    {
        public long Id { get; set; } // Id (Primary key)
        public DateTime InvoiceDate { get; set; } // InvoiceDate
        public long CurrencyId { get; set; } // CurrencyId
        public int State { get; set; } // State
        public string Description { get; set; } // Description
        public int DivisionMethod { get; set; } // DivisionMethod
        public string InvoiceNumber { get; set; } // InvoiceNumber
        public int AccountingType { get; set; } // AccountingType
        public long? InvoiceRefrenceId { get; set; } // InvoiceRefrenceId
        public int InvoiceType { get; set; } // InvoiceType
        public long? TransporterId { get; set; } // TransporterId
        public long? SupplierId { get; set; } // SupplierId
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long OwnerId { get; set; } // OwnerId
        public bool IsCreditor { get; set; } // IsCreditor
        public decimal TotalOfDivisionPrice { get; set; } // TotalOfDivisionPrice

        // Reverse navigation
        public virtual ICollection<Fuel_Invoice> Fuel_Invoice2 { get; set; } // Invoice.FK_Invoice_InvoiceRefrenceId_Invoice_Id
        public virtual ICollection<Fuel_InvoiceAdditionalPrice> Fuel_InvoiceAdditionalPrice { get; set; } // InvoiceAdditionalPrices.FK_InvoiceAdditionalPrices_InvoiceId_Invoice_Id
        public virtual ICollection<Fuel_InvoiceItem> Fuel_InvoiceItem { get; set; } // InvoiceItems.FK_InvoiceItems_InvoiceId_Invoice_Id
        public virtual ICollection<Fuel_Order> Fuel_Order { get; set; } // Many to many mapping
        public virtual ICollection<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog.FK_WorkflowLog_InvoiceId_Invoice_Id

        // Foreign keys
        public virtual Fuel_Invoice Fuel_Invoice1 { get; set; } // FK_Invoice_InvoiceRefrenceId_Invoice_Id
        
        public Fuel_Invoice()
        {
            Fuel_Invoice2 = new List<Fuel_Invoice>();
            Fuel_InvoiceAdditionalPrice = new List<Fuel_InvoiceAdditionalPrice>();
            Fuel_InvoiceItem = new List<Fuel_InvoiceItem>();
            Fuel_WorkflowLog = new List<Fuel_WorkflowLog>();
            Fuel_Order = new List<Fuel_Order>();
        }
    }

    // InvoiceAdditionalPrices
    internal class Fuel_InvoiceAdditionalPrice
    {
        public long Id { get; set; } // Id (Primary key)
        public long EffectiveFactorId { get; set; } // EffectiveFactorId
        public decimal Price { get; set; } // Price
        public string Description { get; set; } // Description
        public bool Divisionable { get; set; } // Divisionable
        public long InvoiceId { get; set; } // InvoiceId
        public byte[] TimeStamp { get; set; } // TimeStamp

        // Foreign keys
        public virtual Fuel_EffectiveFactor Fuel_EffectiveFactor { get; set; } // FK_InvoiceAdditionalPrices_EffectiveFactorId_EffectiveFactor_Id
        public virtual Fuel_Invoice Fuel_Invoice { get; set; } // FK_InvoiceAdditionalPrices_InvoiceId_Invoice_Id
    }

    // InvoiceItems
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_InvoiceItem
    {
        public long Id { get; set; } // Id (Primary key)
        public decimal Quantity { get; set; } // Quantity
        public decimal Fee { get; set; } // Fee
        public long InvoiceId { get; set; } // InvoiceId
        public long GoodId { get; set; } // GoodId
        public long MeasuringUnitId { get; set; } // MeasuringUnitId
        public string Description { get; set; } // Description
        public byte[] TimeStamp { get; set; } // TimeStamp
        public decimal DivisionPrice { get; set; } // DivisionPrice

        // Reverse navigation
        public virtual ICollection<Fuel_OrderItemBalance> Fuel_OrderItemBalance_InvoiceItemId { get; set; } // OrderItemBalances.FK_OrderItemBalances_InvoiceItemId_InvoiceItems_Id
        public virtual ICollection<Fuel_OrderItemBalance> Fuel_OrderItemBalance_PairingInvoiceItemId { get; set; } // OrderItemBalances.FK_OrderItemBalances_PairingInvoiceItemId_InvoiceItems_Id

        // Foreign keys
        public virtual Fuel_Invoice Fuel_Invoice { get; set; } // FK_InvoiceItems_InvoiceId_Invoice_Id
        
        public Fuel_InvoiceItem()
        {
            Fuel_OrderItemBalance_InvoiceItemId = new List<Fuel_OrderItemBalance>();
            Fuel_OrderItemBalance_PairingInvoiceItemId = new List<Fuel_OrderItemBalance>();
        }
    }

    // JournalEntries
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_JournalEntry
    {
        public long Id { get; set; } // Id (Primary key)
        public long VoucherId { get; set; } // VoucherId
        public string AccountNo { get; set; } // AccountNo
        public long CurrencyId { get; set; } // CurrencyId
        public string Description { get; set; } // Description
        public byte[] TimeStamp { get; set; } // TimeStamp
        public string VoucherRef { get; set; } // VoucherRef
        public decimal ForeignAmount { get; set; } // ForeignAmount
        public decimal IrrAmount { get; set; } // IrrAmount
        public int Typ { get; set; } // Typ
        public long? InventoryItemId { get; set; } // InventoryItemId

        // Reverse navigation
        public virtual ICollection<Fuel_Segment> Fuel_Segment { get; set; } // Segments.FK_Segment_JournalEntries

        // Foreign keys
        public virtual Fuel_Voucher Fuel_Voucher { get; set; } // FK_Voucher_JournalEntries_
        
        public Fuel_JournalEntry()
        {
            Fuel_Segment = new List<Fuel_Segment>();
        }
    }

    // Logs
    internal class Fuel_Log
    {
        public long Id { get; set; } // Id (Primary key)
        public byte[] RowVersion { get; set; } // RowVersion
        public string Code { get; set; } // Code
        public int LogLevelId { get; set; } // LogLevelId
        public long PartyId { get; set; } // PartyId
        public string UserName { get; set; } // UserName
        public string ClassName { get; set; } // ClassName
        public string MethodName { get; set; } // MethodName
        public DateTime LogDate { get; set; } // LogDate
        public string Title { get; set; } // Title
        public string Messages { get; set; } // Messages

        // Reverse navigation
        public virtual Fuel_EventLog Fuel_EventLog { get; set; } // EventLogs.FK_EventLogs_Id_Logs_Id
        public virtual Fuel_ExceptionLog Fuel_ExceptionLog { get; set; } // ExceptionLogs.FK_ExceptionLogs_Id_Logs_Id
    }

    // Offhire
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Offhire
    {
        public long Id { get; set; } // Id (Primary key)
        public long ReferenceNumber { get; set; } // ReferenceNumber
        public DateTime StartDateTime { get; set; } // StartDateTime
        public DateTime EndDateTime { get; set; } // EndDateTime
        public int IntroducerType { get; set; } // IntroducerType
        public DateTime VoucherDate { get; set; } // VoucherDate
        public long VoucherCurrencyId { get; set; } // VoucherCurrencyId
        public string PricingReferenceNumber { get; set; } // PricingReference_Number
        public int PricingReferenceType { get; set; } // PricingReference_Type
        public int State { get; set; } // State
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long IntroducerId { get; set; } // IntroducerId
        public long OffhireLocationId { get; set; } // OffhireLocationId
        public long VesselInCompanyId { get; set; } // VesselInCompanyId
        public long? VoyageId { get; set; } // VoyageId

        // Reverse navigation
        public virtual ICollection<Fuel_OffhireDetail> Fuel_OffhireDetail { get; set; } // OffhireDetail.FK_OffhireDetail_Offhire_Id_Offhire_Id
        public virtual ICollection<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog.FK_WorkflowLog_OffhireId_Offhire_Id

        // Foreign keys
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany { get; set; } // FK_Offhire_VesselInCompanyId_VesselInCompany_Id
        
        public Fuel_Offhire()
        {
            Fuel_OffhireDetail = new List<Fuel_OffhireDetail>();
            Fuel_WorkflowLog = new List<Fuel_WorkflowLog>();
        }
    }

    // OffhireDetail
    internal class Fuel_OffhireDetail
    {
        public long Id { get; set; } // Id (Primary key)
        public decimal Quantity { get; set; } // Quantity
        public decimal FeeInVoucherCurrency { get; set; } // FeeInVoucherCurrency
        public decimal FeeInMainCurrency { get; set; } // FeeInMainCurrency
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long GoodId { get; set; } // GoodId
        public long OffhireId { get; set; } // OffhireId
        public long? TankId { get; set; } // TankId
        public long UnitId { get; set; } // UnitId

        // Foreign keys
        public virtual Fuel_Offhire Fuel_Offhire { get; set; } // FK_OffhireDetail_Offhire_Id_Offhire_Id
    }

    // Order
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Order
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Description { get; set; } // Description
        public long? SupplierId { get; set; } // SupplierId
        public long? ReceiverId { get; set; } // ReceiverId
        public long? TransporterId { get; set; } // TransporterId
        public long OwnerId { get; set; } // OwnerId
        public int OrderType { get; set; } // OrderType
        public DateTime OrderDate { get; set; } // OrderDate
        public long? FromVesselInCompanyId { get; set; } // FromVesselInCompanyId
        public long? ToVesselInCompanyId { get; set; } // ToVesselInCompanyId
        public byte[] TimeStamp { get; set; } // TimeStamp
        public int State { get; set; } // State

        // Reverse navigation
        public virtual ICollection<Fuel_Invoice> Fuel_Invoice { get; set; } // Many to many mapping
        public virtual ICollection<Fuel_OrderItem> Fuel_OrderItem { get; set; } // OrderItems.FK_OrderItems_OrderId_Order_Id
        public virtual ICollection<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog.FK_WorkflowLog_OrderId_Order_Id

        // Foreign keys
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany_FromVesselInCompanyId { get; set; } // FK_Order_FromVesselInCompanyId_VesselInCompany_Id
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany_ToVesselInCompanyId { get; set; } // FK_Order_ToVesselInCompanyId_VesselInCompany_Id
        
        public Fuel_Order()
        {
            Fuel_OrderItem = new List<Fuel_OrderItem>();
            Fuel_WorkflowLog = new List<Fuel_WorkflowLog>();
            Fuel_Invoice = new List<Fuel_Invoice>();
        }
    }

    // OrderItems
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_OrderItem
    {
        public long Id { get; set; } // Id (Primary key)
        public string Description { get; set; } // Description
        public decimal Quantity { get; set; } // Quantity
        public long OrderId { get; set; } // OrderId
        public long GoodId { get; set; } // GoodId
        public long MeasuringUnitId { get; set; } // MeasuringUnitId
        public byte[] TimeStamp { get; set; } // TimeStamp
        public decimal InvoicedInMainUnit { get; set; } // InvoicedInMainUnit
        public decimal ReceivedInMainUnit { get; set; } // ReceivedInMainUnit

        // Reverse navigation
        public virtual ICollection<Fuel_OrderItemBalance> Fuel_OrderItemBalance { get; set; } // OrderItemBalances.FK_OrderItemBalances_OrderItemId_OrderItems_Id

        // Foreign keys
        public virtual Fuel_Order Fuel_Order { get; set; } // FK_OrderItems_OrderId_Order_Id
        
        public Fuel_OrderItem()
        {
            Fuel_OrderItemBalance = new List<Fuel_OrderItemBalance>();
        }
    }

    // OrderItemBalances
    internal class Fuel_OrderItemBalance
    {
        public long Id { get; set; } // Id (Primary key)
        public long OrderId { get; set; } // OrderId
        public long OrderItemId { get; set; } // OrderItemId
        public decimal QuantityAmountInMainUnit { get; set; } // QuantityAmountInMainUnit
        public string UnitCode { get; set; } // UnitCode
        public long FuelReportDetailId { get; set; } // FuelReportDetailId
        public long InvoiceItemId { get; set; } // InvoiceItemId
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long? InventoryOperationId { get; set; } // InventoryOperationId
        public long? PairingInvoiceItemId { get; set; } // PairingInvoiceItemId

        // Foreign keys
        public virtual Fuel_FuelReportDetail Fuel_FuelReportDetail { get; set; } // FK_OrderItemBalances_FuelReportDetailId_FuelReportDetail_Id
        public virtual Fuel_InventoryOperation Fuel_InventoryOperation { get; set; } // FK_OrderItemBalances_InventoryOperationId_InventoryOperation_Id
        public virtual Fuel_InvoiceItem Fuel_InvoiceItem_InvoiceItemId { get; set; } // FK_OrderItemBalances_InvoiceItemId_InvoiceItems_Id
        public virtual Fuel_InvoiceItem Fuel_InvoiceItem_PairingInvoiceItemId { get; set; } // FK_OrderItemBalances_PairingInvoiceItemId_InvoiceItems_Id
        public virtual Fuel_OrderItem Fuel_OrderItem { get; set; } // FK_OrderItemBalances_OrderItemId_OrderItems_Id
    }

    // Scrap
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Scrap
    {
        public long Id { get; set; } // Id (Primary key)
        public DateTime ScrapDate { get; set; } // ScrapDate
        public int State { get; set; } // State
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long SecondPartyId { get; set; } // SecondPartyId
        public long VesselInCompanyId { get; set; } // VesselInCompanyId

        // Reverse navigation
        public virtual ICollection<Fuel_InventoryOperation> Fuel_InventoryOperation { get; set; } // InventoryOperation.FK_InventoryOperation_Scrap_Id_Scrap_Id
        public virtual ICollection<Fuel_ScrapDetail> Fuel_ScrapDetail { get; set; } // ScrapDetail.FK_ScrapDetail_Scrap_Id_Scrap_Id
        public virtual ICollection<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog.FK_WorkflowLog_ScrapId_Scrap_Id

        // Foreign keys
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany { get; set; } // FK_Scrap_VesselInCompanyId_VesselInCompany_Id
        
        public Fuel_Scrap()
        {
            Fuel_InventoryOperation = new List<Fuel_InventoryOperation>();
            Fuel_ScrapDetail = new List<Fuel_ScrapDetail>();
            Fuel_WorkflowLog = new List<Fuel_WorkflowLog>();
        }
    }

    // ScrapDetail
    internal class Fuel_ScrapDetail
    {
        public long Id { get; set; } // Id (Primary key)
        public double Rob { get; set; } // ROB
        public double Price { get; set; } // Price
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long CurrencyId { get; set; } // CurrencyId
        public long GoodId { get; set; } // GoodId
        public long? TankId { get; set; } // TankId
        public long UnitId { get; set; } // UnitId
        public long ScrapId { get; set; } // ScrapId

        // Foreign keys
        public virtual Fuel_Scrap Fuel_Scrap { get; set; } // FK_ScrapDetail_Scrap_Id_Scrap_Id
    }

    // Segments
    internal class Fuel_Segment
    {
        public long Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Code { get; set; } // Code
        public byte[] TimeStamp { get; set; } // TimeStamp
        public int SegmentTypeId { get; set; } // SegmentTypeId
        public long JournalEntryId { get; set; } // JournalEntryId
        public long? FreeAccountId { get; set; } // FreeAccountId
        public long? EffectiveFactorId { get; set; } // EffectiveFactorId
        public string SegmentTypeCode { get; set; } // SegmentTypeCode
        public string SegmentTypeName { get; set; } // SegmentTypeName

        // Foreign keys
        public virtual Fuel_EffectiveFactor Fuel_EffectiveFactor { get; set; } // FK_Segment_EffectiveFactor
        public virtual Fuel_FreeAccount Fuel_FreeAccount { get; set; } // FK_Segment_FreeAccount
        public virtual Fuel_JournalEntry Fuel_JournalEntry { get; set; } // FK_Segment_JournalEntries
    }

    // UserInCompany
    internal class Fuel_UserInCompany
    {
        public long Id { get; set; } // Id (Primary key)
        public long UserId { get; set; } // UserId
        public long CompanyId { get; set; } // CompanyId

        // Foreign keys
        public virtual User User { get; set; } // FK_UserInCompany_UserId_SecurityUser_Id
    }

    // Vessel
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Vessel
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public long OwnerId { get; set; } // OwnerId
        public byte[] RowVersion { get; set; } // RowVersion

        // Reverse navigation
        public virtual ICollection<Fuel_VesselInCompany> Fuel_VesselInCompany { get; set; } // VesselInCompany.FK_VesselInCompany_VesselId_Vessel_Id
        
        public Fuel_Vessel()
        {
            Fuel_VesselInCompany = new List<Fuel_VesselInCompany>();
        }
    }

    // VesselInCompany
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_VesselInCompany
    {
        public long Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public long CompanyId { get; set; } // CompanyId
        public long VesselId { get; set; } // VesselId
        public int VesselStateCode { get; set; } // VesselStateCode
        public byte[] RowVersion { get; set; } // RowVersion

        // Reverse navigation
        public virtual ICollection<Fuel_Charter> Fuel_Charter { get; set; } // Charter.FK_Charter_VesselInCompanyId_VesselInCompany_Id
        public virtual ICollection<Fuel_FuelReport> Fuel_FuelReport { get; set; } // FuelReport.FK_FuelReport_VesselInCompanyId_VesselInCompany_Id
        public virtual ICollection<Fuel_Offhire> Fuel_Offhire { get; set; } // Offhire.FK_Offhire_VesselInCompanyId_VesselInCompany_Id
        public virtual ICollection<Fuel_Order> Fuel_Order_FromVesselInCompanyId { get; set; } // Order.FK_Order_FromVesselInCompanyId_VesselInCompany_Id
        public virtual ICollection<Fuel_Order> Fuel_Order_ToVesselInCompanyId { get; set; } // Order.FK_Order_ToVesselInCompanyId_VesselInCompany_Id
        public virtual ICollection<Fuel_Scrap> Fuel_Scrap { get; set; } // Scrap.FK_Scrap_VesselInCompanyId_VesselInCompany_Id
        public virtual ICollection<Fuel_Voyage> Fuel_Voyage { get; set; } // Voyage.FK_Voyage_VesselInCompanyId_VesselInCompany_Id
        public virtual ICollection<Fuel_VoyageLog> Fuel_VoyageLog { get; set; } // VoyageLog.FK_VoyageLog_VesselInCompanyId_VesselInCompany_Id

        // Foreign keys
        public virtual Fuel_Vessel Fuel_Vessel { get; set; } // FK_VesselInCompany_VesselId_Vessel_Id
        
        public Fuel_VesselInCompany()
        {
            Fuel_Charter = new List<Fuel_Charter>();
            Fuel_FuelReport = new List<Fuel_FuelReport>();
            Fuel_Offhire = new List<Fuel_Offhire>();
            Fuel_Order_FromVesselInCompanyId = new List<Fuel_Order>();
            Fuel_Order_ToVesselInCompanyId = new List<Fuel_Order>();
            Fuel_Scrap = new List<Fuel_Scrap>();
            Fuel_Voyage = new List<Fuel_Voyage>();
            Fuel_VoyageLog = new List<Fuel_VoyageLog>();
        }
    }

    // Vouchers
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Voucher
    {
        public long Id { get; set; } // Id (Primary key)
        public string Description { get; set; } // Description
        public DateTime FinancialVoucherDate { get; set; } // FinancialVoucherDate
        public DateTime LocalVoucherDate { get; set; } // LocalVoucherDate
        public string LocalVoucherNo { get; set; } // LocalVoucherNo
        public string ReferenceNo { get; set; } // ReferenceNo
        public string VoucherRef { get; set; } // VoucherRef
        public int ReferenceTypeId { get; set; } // ReferenceTypeId
        public byte[] TimeStamp { get; set; } // TimeStamp
        public long CompanyId { get; set; } // CompanyId
        public int VoucherDetailTypeId { get; set; } // VoucherDetailTypeId
        public int VoucherTypeId { get; set; } // VoucherTypeId
        public long UserId { get; set; } // UserId

        // Reverse navigation
        public virtual ICollection<Fuel_JournalEntry> Fuel_JournalEntry { get; set; } // JournalEntries.FK_Voucher_JournalEntries_
        
        public Fuel_Voucher()
        {
            Fuel_JournalEntry = new List<Fuel_JournalEntry>();
        }
    }

    // VoucherLogs
    internal class Fuel_VoucherLog
    {
        public long Id { get; set; } // Id (Primary key)
        public string ExceptionMessage { get; set; } // ExceptionMessage
        public string StackTrace { get; set; } // StackTrace
        public string VoucherType { get; set; } // VoucherType
        public string RefrenceNo { get; set; } // RefrenceNo
    }

    // VoucherReportView
    internal class Fuel_VoucherReportView
    {
        public long? VoucherId { get; set; } // VoucherId
        public long JournalEntryId { get; set; } // JournalEntryId
        public string VoucherCompany { get; set; } // VoucherCompany
        public string Description { get; set; } // Description
        public DateTime? FinancialVoucherDate { get; set; } // FinancialVoucherDate
        public DateTime? LocalVoucherDate { get; set; } // LocalVoucherDate
        public string LocalVoucherNo { get; set; } // LocalVoucherNo
        public string ReferenceNo { get; set; } // ReferenceNo
        public string VoucherRef { get; set; } // VoucherRef
        public int? ReferenceTypeId { get; set; } // ReferenceTypeId
        public int? VoucherDetailTypeId { get; set; } // VoucherDetailTypeId
        public int? VoucherTypeId { get; set; } // VoucherTypeId
        public string AccountNo { get; set; } // AccountNo
        public string JournalEntryDescription { get; set; } // JournalEntryDescription
        public string JournalEntryVoucherRef { get; set; } // JournalEntryVoucherRef
        public decimal ForeignAmount { get; set; } // ForeignAmount
        public decimal IrrAmount { get; set; } // IrrAmount
        public int JournalEntryType { get; set; } // JournalEntryType
        public string JournalEntryCurrency { get; set; } // JournalEntryCurrency
        public string SegmentName { get; set; } // SegmentName
        public string SegmentCode { get; set; } // SegmentCode
        public string SegmentTypeName { get; set; } // SegmentTypeName
        public string SegmentTypeCode { get; set; } // SegmentTypeCode
        public string UserName { get; set; } // UserName
    }

    // VoucherSetings
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_VoucherSeting
    {
        public long Id { get; set; } // Id (Primary key)
        public string VoucherMainRefDescription { get; set; } // VoucherMainRefDescription
        public string VoucherMainDescription { get; set; } // VoucherMainDescription
        public long CompanyId { get; set; } // CompanyId
        public int VoucherDetailTypeId { get; set; } // VoucherDetailTypeId
        public byte[] TimeStamp { get; set; } // TimeStamp
        public int VoucherTypeId { get; set; } // VoucherTypeId

        // Reverse navigation
        public virtual ICollection<Fuel_VoucherSetingDetail> Fuel_VoucherSetingDetail { get; set; } // VoucherSetingDetails.FK_VoucherSeting
        
        public Fuel_VoucherSeting()
        {
            Fuel_VoucherSetingDetail = new List<Fuel_VoucherSetingDetail>();
        }
    }

    // VoucherSetingDetails
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_VoucherSetingDetail
    {
        public long Id { get; set; } // Id (Primary key)
        public long VoucherSetingId { get; set; } // VoucherSetingId
        public string VoucherCeditRefDescription { get; set; } // VoucherCeditRefDescription
        public string VoucherDebitDescription { get; set; } // VoucherDebitDescription
        public string VoucherDebitRefDescription { get; set; } // VoucherDebitRefDescription
        public string VoucherCreditDescription { get; set; } // VoucherCreditDescription
        public long GoodId { get; set; } // GoodId
        public bool IsDelete { get; set; } // IsDelete

        // Reverse navigation
        public virtual ICollection<Fuel_AsgnSegmentTypeVoucherSetingDetail> Fuel_AsgnSegmentTypeVoucherSetingDetail { get; set; } // AsgnSegmentTypeVoucherSetingDetail.FK_VoucherSetingDetail_AsgnSegmentTypeVoucherSetingDetail
        public virtual ICollection<Fuel_AsgnVoucherAcont> Fuel_AsgnVoucherAcont { get; set; } // AsgnVoucherAconts.FK_VoucherSetingDetail_AsgnVoucherAconts

        // Foreign keys
        public virtual Fuel_VoucherSeting Fuel_VoucherSeting { get; set; } // FK_VoucherSeting
        
        public Fuel_VoucherSetingDetail()
        {
            IsDelete = false;
            Fuel_AsgnSegmentTypeVoucherSetingDetail = new List<Fuel_AsgnSegmentTypeVoucherSetingDetail>();
            Fuel_AsgnVoucherAcont = new List<Fuel_AsgnVoucherAcont>();
        }
    }

    // Voyage
    internal class Fuel_Voyage
    {
        public long Id { get; set; } // Id (Primary key)
        public string VoyageNumber { get; set; } // VoyageNumber
        public string Description { get; set; } // Description
        public long VesselInCompanyId { get; set; } // VesselInCompanyId
        public long CompanyId { get; set; } // CompanyId
        public DateTime StartDate { get; set; } // StartDate
        public DateTime? EndDate { get; set; } // EndDate
        public bool IsActive { get; set; } // IsActive

        // Foreign keys
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany { get; set; } // FK_Voyage_VesselInCompanyId_VesselInCompany_Id
    }

    // VoyageLog
    internal class Fuel_VoyageLog
    {
        public long Id { get; set; } // Id (Primary key)
        public long ReferencedVoyageId { get; set; } // ReferencedVoyageId
        public DateTime ChangeDate { get; set; } // ChangeDate
        public string VoyageNumber { get; set; } // VoyageNumber
        public string Description { get; set; } // Description
        public DateTime StartDate { get; set; } // StartDate
        public DateTime EndDate { get; set; } // EndDate
        public bool IsActive { get; set; } // IsActive
        public long CompanyId { get; set; } // CompanyId
        public long VesselInCompanyId { get; set; } // VesselInCompanyId

        // Foreign keys
        public virtual Fuel_VesselInCompany Fuel_VesselInCompany { get; set; } // FK_VoyageLog_VesselInCompanyId_VesselInCompany_Id
    }

    // Workflow
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_Workflow
    {
        public long Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public int WorkflowEntity { get; set; } // WorkflowEntity
        public long CompanyId { get; set; } // CompanyId

        // Reverse navigation
        public virtual ICollection<Fuel_WorkflowStep> Fuel_WorkflowStep { get; set; } // WorkflowStep.FK_Fuel.WorkflowStep_WorkflowId_Fuel.Workflow_Id
        
        public Fuel_Workflow()
        {
            Fuel_WorkflowStep = new List<Fuel_WorkflowStep>();
        }
    }

    // WorkflowLog
    internal class Fuel_WorkflowLog
    {
        public long Id { get; set; } // Id (Primary key)
        public int WorkflowEntity { get; set; } // WorkflowEntity
        public DateTime ActionDate { get; set; } // ActionDate
        public int? WorkflowAction { get; set; } // WorkflowAction
        public long ActorUserId { get; set; } // ActorUserId
        public string Remark { get; set; } // Remark
        public bool Active { get; set; } // Active
        public long CurrentWorkflowStepId { get; set; } // CurrentWorkflowStepId
        public long? InvoiceId { get; set; } // InvoiceId
        public long? OrderId { get; set; } // OrderId
        public long? CharterId { get; set; } // CharterId
        public long? FuelReportId { get; set; } // FuelReportId
        public long? OffhireId { get; set; } // OffhireId
        public long? ScrapId { get; set; } // ScrapId
        public string Discriminator { get; set; } // Discriminator
        public byte[] RowVersion { get; set; } // RowVersion

        // Foreign keys
        public virtual Fuel_Charter Fuel_Charter { get; set; } // FK_WorkflowLog_CharterId_Charter_Id
        public virtual Fuel_FuelReport Fuel_FuelReport { get; set; } // FK_WorkflowLog_FuelReportId_FuelReport_Id
        public virtual Fuel_Invoice Fuel_Invoice { get; set; } // FK_WorkflowLog_InvoiceId_Invoice_Id
        public virtual Fuel_Offhire Fuel_Offhire { get; set; } // FK_WorkflowLog_OffhireId_Offhire_Id
        public virtual Fuel_Order Fuel_Order { get; set; } // FK_WorkflowLog_OrderId_Order_Id
        public virtual Fuel_Scrap Fuel_Scrap { get; set; } // FK_WorkflowLog_ScrapId_Scrap_Id
        public virtual Fuel_WorkflowStep Fuel_WorkflowStep { get; set; } // FK_WorkflowLog_CurrentWorkflowStepId_WorkflowStep_Id
    }

    // WorkflowLog_Old
    internal class Fuel_WorkflowLogOld
    {
        public long Id { get; set; } // Id
        public int WorkflowEntity { get; set; } // WorkflowEntity
        public DateTime ActionDate { get; set; } // ActionDate
        public int? WorkflowAction { get; set; } // WorkflowAction
        public long ActorUserId { get; set; } // ActorUserId
        public string Remark { get; set; } // Remark
        public bool Active { get; set; } // Active
        public long CurrentWorkflowStepId { get; set; } // CurrentWorkflowStepId
        public long? InvoiceId { get; set; } // InvoiceId
        public long? OrderId { get; set; } // OrderId
        public long? CharterId { get; set; } // CharterId
        public long? FuelReportId { get; set; } // FuelReportId
        public long? OffhireId { get; set; } // OffhireId
        public long? ScrapId { get; set; } // ScrapId
        public string Discriminator { get; set; } // Discriminator
        public byte[] RowVersion { get; set; } // RowVersion
    }

    // WorkflowStep
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Fuel_WorkflowStep
    {
        public long Id { get; set; } // Id (Primary key)
        public long WorkflowId { get; set; } // WorkflowId
        public int State { get; set; } // State
        public int CurrentWorkflowStage { get; set; } // CurrentWorkflowStage

        // Reverse navigation
        public virtual ICollection<Fuel_ActivityFlow> Fuel_ActivityFlow_WorkflowNextStepId { get; set; } // ActivityFlow.FK_Fuel.ActivityFlow_WorkflowNextStepId_Fuel.WorkflowStep_Id
        public virtual ICollection<Fuel_ActivityFlow> Fuel_ActivityFlow_WorkflowStepId { get; set; } // ActivityFlow.FK_Fuel.ActivityFlow_WorkflowStepId_Fuel.WorkflowStep_Id
        public virtual ICollection<Fuel_WorkflowLog> Fuel_WorkflowLog { get; set; } // WorkflowLog.FK_WorkflowLog_CurrentWorkflowStepId_WorkflowStep_Id

        // Foreign keys
        public virtual Fuel_Workflow Fuel_Workflow { get; set; } // FK_Fuel.WorkflowStep_WorkflowId_Fuel.Workflow_Id
        
        public Fuel_WorkflowStep()
        {
            Fuel_ActivityFlow_WorkflowNextStepId = new List<Fuel_ActivityFlow>();
            Fuel_ActivityFlow_WorkflowStepId = new List<Fuel_ActivityFlow>();
            Fuel_WorkflowLog = new List<Fuel_WorkflowLog>();
        }
    }

    // Groups
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Group
    {
        public long Id { get; set; } // Id (Primary key)
        public string Description { get; set; } // Description

        // Reverse navigation
        public virtual ICollection<UsersGroups> UsersGroups { get; set; } // Users_Groups.FK_Users_Groups_GroupId_Groups_Id

        // Foreign keys
        public virtual Party Party { get; set; } // FK_Groups_Id_Parties_Id
        
        public Group()
        {
            UsersGroups = new List<UsersGroups>();
        }
    }

    // HAFEZAccountListView
    internal class HafezAccountListView
    {
        public int AccountListId { get; set; } // AccountListID
        public int? ParentId { get; set; } // ParentID
        public string AccountCode { get; set; } // AccountCode
        public bool Nature { get; set; } // Nature
        public short LevelCode { get; set; } // LevelCode
        public string Name { get; set; } // Name
        public string NameL { get; set; } // NameL
        public bool Disabled { get; set; } // Disabled
    }

    // HAFEZVoyagesView
    internal class HafezVoyagesView
    {
        public int Id { get; set; } // Id
        public string VesselCode { get; set; } // VesselCode
        public short ShipOwnerId { get; set; } // ShipOwnerId
        public string VoyageNumber { get; set; } // VoyageNumber
        public DateTime? StartDateTime { get; set; } // StartDateTime
        public DateTime? EndDateTime { get; set; } // EndDateTime
        public byte TripType { get; set; } // TripType
        public bool IsActive { get; set; } // IsActive
    }

    // HAFIZAccountListView
    internal class HafizAccountListView
    {
        public int AccountListId { get; set; } // AccountListID
        public int? ParentId { get; set; } // ParentID
        public string AccountCode { get; set; } // AccountCode
        public bool Nature { get; set; } // Nature
        public short LevelCode { get; set; } // LevelCode
        public string Name { get; set; } // Name
        public string NameL { get; set; } // NameL
        public bool Disabled { get; set; } // Disabled
    }

    // HAFIZVoyagesView
    internal class HafizVoyagesView
    {
        public int Id { get; set; } // Id
        public string VesselCode { get; set; } // VesselCode
        public short ShipOwnerId { get; set; } // ShipOwnerId
        public string VoyageNumber { get; set; } // VoyageNumber
        public DateTime? StartDateTime { get; set; } // StartDateTime
        public DateTime? EndDateTime { get; set; } // EndDateTime
        public byte TripType { get; set; } // TripType
        public bool IsActive { get; set; } // IsActive
    }

    // Companies
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_Company
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
        }
    }

    // ErrorMessages
    internal class Inventory_ErrorMessage
    {
        public string ErrorMessage { get; set; } // ErrorMessage (Primary key)
        public string TextMessage { get; set; } // TextMessage (Primary key)
        public string Action { get; set; } // Action (Primary key)
    }

    // FinancialYear
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_FinancialYear
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
        }
    }

    // Goods
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_Good
    {
        public long Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string Name { get; set; } // Name
        public bool IsActive { get; set; } // IsActive
        public long MainUnitId { get; set; } // MainUnitId
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_GoodId

        // Foreign keys
        public virtual Inventory_Unit Inventory_Unit { get; set; } // FK_Goods_MainUnitId
        public virtual Inventory_User Inventory_User { get; set; } // FK_Goods_UserCreatorId
        
        public Inventory_Good()
        {
            IsActive = true;
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItem = new List<Inventory_TransactionItem>();
        }
    }

    // OperationReference
    internal class Inventory_OperationReference
    {
        public long Id { get; set; } // Id (Primary key)
        public long OperationId { get; set; } // OperationId
        public int OperationType { get; set; } // OperationType
        public string ReferenceType { get; set; } // ReferenceType
        public string ReferenceNumber { get; set; } // ReferenceNumber
        public DateTime RegistrationDate { get; set; } // RegistrationDate
        
        public Inventory_OperationReference()
        {
            RegistrationDate = System.DateTime.Now;
        }
    }

    // StoreTypes
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_StoreType
    {
        public int Id { get; set; } // Id (Primary key)
        public short Code { get; set; } // Code
        public byte Type { get; set; } // Type
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
        }
    }

    // TimeBucket
    internal class Inventory_TimeBucket
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
        
        public Inventory_TimeBucket()
        {
            StartDate = System.DateTime.Now;
            EndDate = System.DateTime.Now;
            CreateDate = System.DateTime.Now;
            IsActive = false;
        }
    }

    // Transactions
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_Transaction
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

        // Reverse navigation
        public virtual ICollection<Inventory_Transaction> Inventory_Transaction2 { get; set; } // Transactions.FK_Transaction_PricingReferenceId
        public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_TransactionId

        // Foreign keys
        public virtual Inventory_StoreType Inventory_StoreType { get; set; } // FK_Transaction_StoreTypesId
        public virtual Inventory_Transaction Inventory_Transaction1 { get; set; } // FK_Transaction_PricingReferenceId
        public virtual Inventory_User Inventory_User { get; set; } // FK_Transaction_UserCreatorId
        public virtual Inventory_Warehouse Inventory_Warehouse { get; set; } // FK_Transaction_WarehouseId
        
        public Inventory_Transaction()
        {
            Status = 1;
            RegistrationDate = System.DateTime.Now;
            ReferenceDate = System.DateTime.Now;
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItem = new List<Inventory_TransactionItem>();
            Inventory_Transaction2 = new List<Inventory_Transaction>();
        }
    }

    // TransactionItems
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_TransactionItem
    {
        public int Id { get; set; } // Id (Primary key)
        public short RowVersion { get; set; } // RowVersion
        public int TransactionId { get; set; } // TransactionId
        public long GoodId { get; set; } // GoodId
        public long QuantityUnitId { get; set; } // QuantityUnitId
        public decimal? QuantityAmount { get; set; } // QuantityAmount
        public string Description { get; set; } // Description
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_TransactionItemsId

        // Foreign keys
        public virtual Inventory_Good Inventory_Good { get; set; } // FK_TransactionItems_GoodId
        public virtual Inventory_Transaction Inventory_Transaction { get; set; } // FK_TransactionItems_TransactionId
        public virtual Inventory_Unit Inventory_Unit { get; set; } // FK_TransactionItems_QuantityUnitId
        public virtual Inventory_User Inventory_User { get; set; } // FK_TransactionItems_UserCreatorId
        
        public Inventory_TransactionItem()
        {
            QuantityAmount = 0m;
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItemPrice = new List<Inventory_TransactionItemPrice>();
        }
    }

    // TransactionItemPrices
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_TransactionItemPrice
    {
        public int Id { get; set; } // Id (Primary key)
        public short RowVersion { get; set; } // RowVersion
        public int TransactionId { get; set; } // TransactionId
        public int TransactionItemId { get; set; } // TransactionItemId
        public string Description { get; set; } // Description
        public long QuantityUnitId { get; set; } // QuantityUnitId
        public decimal? QuantityAmount { get; set; } // QuantityAmount
        public long PriceUnitId { get; set; } // PriceUnitId
        public decimal? Fee { get; set; } // Fee
        public long MainCurrencyUnitId { get; set; } // MainCurrencyUnitId
        public decimal? FeeInMainCurrency { get; set; } // FeeInMainCurrency
        public DateTime? RegistrationDate { get; set; } // RegistrationDate
        public decimal? QuantityAmountUseFifo { get; set; } // QuantityAmountUseFIFO
        public int? TransactionReferenceId { get; set; } // TransactionReferenceId
        public string IssueReferenceIds { get; set; } // IssueReferenceIds
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice2 { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_TransactionReferenceId

        // Foreign keys
        public virtual Inventory_TransactionItem Inventory_TransactionItem { get; set; } // FK_TransactionItemPrices_TransactionItemsId
        public virtual Inventory_TransactionItemPrice Inventory_TransactionItemPrice1 { get; set; } // FK_TransactionItemPrices_TransactionReferenceId
        public virtual Inventory_Unit Inventory_Unit_MainCurrencyUnitId { get; set; } // FK_TransactionItemPrices_MainCurrencyUnitId
        public virtual Inventory_Unit Inventory_Unit_PriceUnitId { get; set; } // FK_TransactionItemPrices_PriceUnitId
        public virtual Inventory_Unit Inventory_Unit_QuantityUnitId { get; set; } // FK_TransactionItemPrices_QuantityUnitId
        public virtual Inventory_User Inventory_User { get; set; } // FK_TransactionItemPrices_UserCreatorId
        
        public Inventory_TransactionItemPrice()
        {
            QuantityAmount = 0m;
            Fee = 0m;
            FeeInMainCurrency = 0m;
            RegistrationDate = System.DateTime.Now;
            QuantityAmountUseFifo = 0m;
            IssueReferenceIds = "N''";
            CreateDate = System.DateTime.Now;
            Inventory_TransactionItemPrice2 = new List<Inventory_TransactionItemPrice>();
        }
    }

    // Units
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_Unit
    {
        public long Id { get; set; } // Id (Primary key)
        public string Abbreviation { get; set; } // Abbreviation
        public string Name { get; set; } // Name
        public bool? IsCurrency { get; set; } // IsCurrency
        public bool? IsBaseCurrency { get; set; } // IsBaseCurrency
        public bool IsActive { get; set; } // IsActive
        public int? UserCreatorId { get; set; } // UserCreatorId
        public DateTime? CreateDate { get; set; } // CreateDate

        // Reverse navigation
        public virtual ICollection<Inventory_Good> Inventory_Good { get; set; } // Goods.FK_Goods_MainUnitId
        public virtual ICollection<Inventory_TransactionItem> Inventory_TransactionItem { get; set; } // TransactionItems.FK_TransactionItems_QuantityUnitId
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_MainCurrencyUnitId { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_MainCurrencyUnitId
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_PriceUnitId { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_PriceUnitId
        public virtual ICollection<Inventory_TransactionItemPrice> Inventory_TransactionItemPrice_QuantityUnitId { get; set; } // TransactionItemPrices.FK_TransactionItemPrices_QuantityUnitId
        public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert_SubUnitId { get; set; } // UnitConverts.FK_UnitConverts_SubUnitId
        public virtual ICollection<Inventory_UnitConvert> Inventory_UnitConvert_UnitId { get; set; } // UnitConverts.FK_UnitConverts_UnitId

        // Foreign keys
        public virtual Inventory_User Inventory_User { get; set; } // FK_Units_UserCreatorId
        
        public Inventory_Unit()
        {
            IsCurrency = false;
            IsBaseCurrency = false;
            IsActive = true;
            CreateDate = System.DateTime.Now;
            Inventory_Good = new List<Inventory_Good>();
            Inventory_TransactionItemPrice_MainCurrencyUnitId = new List<Inventory_TransactionItemPrice>();
            Inventory_TransactionItemPrice_PriceUnitId = new List<Inventory_TransactionItemPrice>();
            Inventory_TransactionItemPrice_QuantityUnitId = new List<Inventory_TransactionItemPrice>();
            Inventory_TransactionItem = new List<Inventory_TransactionItem>();
            Inventory_UnitConvert_SubUnitId = new List<Inventory_UnitConvert>();
            Inventory_UnitConvert_UnitId = new List<Inventory_UnitConvert>();
        }
    }

    // UnitConverts
    internal class Inventory_UnitConvert
    {
        public int Id { get; set; } // Id (Primary key)
        public long UnitId { get; set; } // UnitId
        public long SubUnitId { get; set; } // SubUnitId
        public decimal Coefficient { get; set; } // Coefficient
        public DateTime? EffectiveDateStart { get; set; } // EffectiveDateStart
        public DateTime? EffectiveDateEnd { get; set; } // EffectiveDateEnd
        public short RowVersion { get; set; } // RowVersion
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
        }
    }

    // Users
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_User
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
        }
    }

    // Warehouse
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Inventory_Warehouse
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
        }
    }

    // OffhireFuelTypeFuelGoodCode
    internal class Offhire_OffhireFuelTypeFuelGoodCode
    {
        public long Id { get; set; } // Id (Primary key)
        public string OffhireFuelType { get; set; } // OffhireFuelType
        public string FuelGoodCode { get; set; } // FuelGoodCode
        public DateTime? ActiveFrom { get; set; } // ActiveFrom
        public DateTime? ActiveTo { get; set; } // ActiveTo
    }

    // OffhireMeasureTypeFuelMeasureCode
    internal class Offhire_OffhireMeasureTypeFuelMeasureCode
    {
        public long Id { get; set; } // Id (Primary key)
        public string OffhireMeasureType { get; set; } // OffhireMeasureType
        public string FuelMeasureCode { get; set; } // FuelMeasureCode
        public DateTime? ActiveFrom { get; set; } // ActiveFrom
        public DateTime? ActiveTo { get; set; } // ActiveTo
    }

    // Parties_CustomActions
    internal class PartiesCustomActions
    {
        public long Id { get; set; } // Id (Primary key)
        public long PartyId { get; set; } // PartyId
        public int ActionTypeId { get; set; } // ActionTypeId
        public bool IsGranted { get; set; } // IsGranted

        // Foreign keys
        public virtual ActionType ActionType { get; set; } // FK_Parties_CustomActions_ActionTypeId_ActionTypes_Id
        public virtual Party Party { get; set; } // FK_Parties_CustomActions_PartyId_Parties_Id
    }

    // Parties
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Party
    {
        public long Id { get; set; } // Id (Primary key)
        public string PartyName { get; set; } // PartyName
        public byte[] RowVersion { get; set; } // RowVersion

        // Reverse navigation
        public virtual Group Group { get; set; } // Groups.FK_Groups_Id_Parties_Id
        public virtual ICollection<PartiesCustomActions> PartiesCustomActions { get; set; } // Parties_CustomActions.FK_Parties_CustomActions_PartyId_Parties_Id
        public virtual User User { get; set; } // Users.FK_Users_Id_Parties_Id
        
        public Party()
        {
            PartiesCustomActions = new List<PartiesCustomActions>();
        }
    }

    // SAPIDAccountListView
    internal class SapidAccountListView
    {
        public int AccountListId { get; set; } // AccountListID
        public int? ParentId { get; set; } // ParentID
        public string AccountCode { get; set; } // AccountCode
        public bool Nature { get; set; } // Nature
        public short LevelCode { get; set; } // LevelCode
        public string Name { get; set; } // Name
        public string NameL { get; set; } // NameL
        public bool Disabled { get; set; } // Disabled
    }

    // SAPIDVoyagesView
    internal class SapidVoyagesView
    {
        public int Id { get; set; } // Id
        public string VesselCode { get; set; } // VesselCode
        public short ShipOwnerId { get; set; } // ShipOwnerId
        public string VoyageNumber { get; set; } // VoyageNumber
        public DateTime? StartDateTime { get; set; } // StartDateTime
        public DateTime? EndDateTime { get; set; } // EndDateTime
        public byte TripType { get; set; } // TripType
        public bool IsActive { get; set; } // IsActive
    }

    // sysdiagrams
    internal class Sysdiagram
    {
        public string Name { get; set; } // name
        public int PrincipalId { get; set; } // principal_id
        public int DiagramId { get; set; } // diagram_id (Primary key)
        public int? Version { get; set; } // version
        public byte[] Definition { get; set; } // definition
    }

    // Users
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class User
    {
        public long Id { get; set; } // Id (Primary key)
        public string FirstName { get; set; } // FirstName
        public string UserName { get; set; } // UserName
        public string LastName { get; set; } // LastName
        public string Email { get; set; } // Email
        public bool Active { get; set; } // Active
        public long CompanyId { get; set; } // CompanyId

        // Reverse navigation
        public virtual ICollection<Fuel_UserInCompany> Fuel_UserInCompany { get; set; } // UserInCompany.FK_UserInCompany_UserId_SecurityUser_Id
        public virtual ICollection<UsersGroups> UsersGroups { get; set; } // Users_Groups.FK_Users_Groups_UserId_Users_Id

        // Foreign keys
        public virtual Party Party { get; set; } // FK_Users_Id_Parties_Id
        
        public User()
        {
            Fuel_UserInCompany = new List<Fuel_UserInCompany>();
            UsersGroups = new List<UsersGroups>();
        }
    }

    // Users_Groups
    internal class UsersGroups
    {
        public long Id { get; set; } // Id (Primary key)
        public long UserId { get; set; } // UserId
        public long GroupId { get; set; } // GroupId

        // Foreign keys
        public virtual Group Group { get; set; } // FK_Users_Groups_GroupId_Groups_Id
        public virtual User User { get; set; } // FK_Users_Groups_UserId_Users_Id
    }

    // VersionInfo
    internal class VersionInfo
    {
        public long Version { get; set; } // Version (Primary key via unique index UC_Version)
        public DateTime? AppliedOn { get; set; } // AppliedOn
        public string Description { get; set; } // Description
    }

    // FuelReportCommandLog
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.13.1.0")]
    internal class Vessel_FuelReportCommandLog
    {
        public long Id { get; set; } // Id (Primary key)
        public string VesselReportReference { get; set; } // VesselReportReference
        public string VesselCode { get; set; } // VesselCode
        public int FuelReportType { get; set; } // FuelReportType
        public DateTime ReportDate { get; set; } // ReportDate
        public DateTime EventDate { get; set; } // EventDate
        public string VoyageNumber { get; set; } // VoyageNumber
        public bool IsActive { get; set; } // IsActive
        public int ProcessState { get; set; } // ProcessState
        public DateTime? ProcessDate { get; set; } // ProcessDate
        public string Remark { get; set; } // Remark
        public byte[] RowVersion { get; set; } // RowVersion

        // Reverse navigation
        public virtual ICollection<Vessel_FuelReportCommandLogDetail> Vessel_FuelReportCommandLogDetail { get; set; } // FuelReportCommandLogDetail.FuelReportCommandLogDetail_FuelReportCommandLogId_FuelReportCommandLog_Id
        
        public Vessel_FuelReportCommandLog()
        {
            Vessel_FuelReportCommandLogDetail = new List<Vessel_FuelReportCommandLogDetail>();
        }
    }

    // FuelReportCommandLogDetail
    internal class Vessel_FuelReportCommandLogDetail
    {
        public long Id { get; set; } // Id (Primary key)
        public long FuelReportCommandLogId { get; set; } // FuelReportCommandLogId
        public string FuelType { get; set; } // FuelType
        public decimal Consumption { get; set; } // Consumption
        public decimal Rob { get; set; } // ROB
        public decimal? Transfer { get; set; } // Transfer
        public decimal? Receive { get; set; } // Receive
        public decimal? Correction { get; set; } // Correction
        public string TankCode { get; set; } // TankCode
        public string MeasuringUnitCode { get; set; } // MeasuringUnitCode
        public byte[] RowVersion { get; set; } // RowVersion

        // Foreign keys
        public virtual Vessel_FuelReportCommandLog Vessel_FuelReportCommandLog { get; set; } // FuelReportCommandLogDetail_FuelReportCommandLogId_FuelReportCommandLog_Id
    }


    // ************************************************************************
    // POCO Configuration

    // ActionTypes
    internal class ActionTypeConfiguration : EntityTypeConfiguration<ActionType>
    {
        public ActionTypeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ActionTypes");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(512);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(2000);
        }
    }

    // AccountListView
    internal class BasicInfo_AccountListViewConfiguration : EntityTypeConfiguration<BasicInfo_AccountListView>
    {
        public BasicInfo_AccountListViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".AccountListView");
            HasKey(x => new { x.Code, x.Name });

            Property(x => x.Code).HasColumnName("Code").IsRequired().IsUnicode(false).HasMaxLength(12);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(60);
        }
    }

    // ActivityLocation
    internal class BasicInfo_ActivityLocationConfiguration : EntityTypeConfiguration<BasicInfo_ActivityLocation>
    {
        public BasicInfo_ActivityLocationConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".ActivityLocation");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
            Property(x => x.Name).HasColumnName("Name").IsOptional().HasMaxLength(200);
            Property(x => x.Abbreviation).HasColumnName("Abbreviation").IsOptional().HasMaxLength(50);
            Property(x => x.Latitude).HasColumnName("Latitude").IsOptional();
            Property(x => x.Longitude).HasColumnName("Longitude").IsOptional();
            Property(x => x.CountryName).HasColumnName("CountryName").IsRequired().HasMaxLength(200);
        }
    }

    // CompanyGoodUnitView
    internal class BasicInfo_CompanyGoodUnitViewConfiguration : EntityTypeConfiguration<BasicInfo_CompanyGoodUnitView>
    {
        public BasicInfo_CompanyGoodUnitViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".CompanyGoodUnitView");
            HasKey(x => new { x.SharedGoodId, x.CompanyId, x.Name, x.Abbreviation, x.To });

            Property(x => x.Id).HasColumnName("Id").IsOptional();
            Property(x => x.CompanyGoodId).HasColumnName("CompanyGoodId").IsOptional();
            Property(x => x.SharedGoodId).HasColumnName("SharedGoodId").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
            Property(x => x.Abbreviation).HasColumnName("Abbreviation").IsRequired().HasMaxLength(256);
            Property(x => x.To).HasColumnName("To").IsRequired().HasMaxLength(256);
            Property(x => x.Coefficient).HasColumnName("Coefficient").IsOptional().HasPrecision(19,5);
            Property(x => x.Offset).HasColumnName("Offset").IsOptional().HasPrecision(19,5);
            Property(x => x.ParentId).HasColumnName("ParentId").IsOptional();
        }
    }

    // CompanyGoodView
    internal class BasicInfo_CompanyGoodViewConfiguration : EntityTypeConfiguration<BasicInfo_CompanyGoodView>
    {
        public BasicInfo_CompanyGoodViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".CompanyGoodView");
            HasKey(x => new { x.SharedGoodId, x.CompanyId, x.Name, x.Code });

            Property(x => x.Id).HasColumnName("Id").IsOptional();
            Property(x => x.SharedGoodId).HasColumnName("SharedGoodId").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(100);
        }
    }

    // CompanyVesselTankView
    internal class BasicInfo_CompanyVesselTankViewConfiguration : EntityTypeConfiguration<BasicInfo_CompanyVesselTankView>
    {
        public BasicInfo_CompanyVesselTankViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".CompanyVesselTankView");
            HasKey(x => new { x.VesselInInventoryId, x.Name, x.Description });

            Property(x => x.Id).HasColumnName("Id").IsOptional();
            Property(x => x.VesselInInventoryId).HasColumnName("VesselInInventoryId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().IsUnicode(false).HasMaxLength(3);
            Property(x => x.Description).HasColumnName("Description").IsRequired().IsUnicode(false).HasMaxLength(3);
        }
    }

    // CompanyVesselView
    internal class BasicInfo_CompanyVesselViewConfiguration : EntityTypeConfiguration<BasicInfo_CompanyVesselView>
    {
        public BasicInfo_CompanyVesselViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".CompanyVesselView");
            HasKey(x => new { x.Id, x.Code, x.CompanyId });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(128);
            Property(x => x.Name).HasColumnName("Name").IsOptional().HasMaxLength(256);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.IsActive).HasColumnName("IsActive").IsOptional();
        }
    }

    // CompanyView
    internal class BasicInfo_CompanyViewConfiguration : EntityTypeConfiguration<BasicInfo_CompanyView>
    {
        public BasicInfo_CompanyViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".CompanyView");
            HasKey(x => new { x.Id, x.Code, x.Name });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(256);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
        }
    }

    // CurrencyView
    internal class BasicInfo_CurrencyViewConfiguration : EntityTypeConfiguration<BasicInfo_CurrencyView>
    {
        public BasicInfo_CurrencyViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".CurrencyView");
            HasKey(x => new { x.Id, x.Abbreviation, x.Name });

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Abbreviation).HasColumnName("Abbreviation").IsRequired().HasMaxLength(256);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
        }
    }

    // SharedGoodView
    internal class BasicInfo_SharedGoodViewConfiguration : EntityTypeConfiguration<BasicInfo_SharedGoodView>
    {
        public BasicInfo_SharedGoodViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".SharedGoodView");
            HasKey(x => new { x.Id, x.Code, x.Name, x.MainUnitId, x.MainUnitCode });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(100);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
            Property(x => x.MainUnitId).HasColumnName("MainUnitId").IsRequired();
            Property(x => x.MainUnitCode).HasColumnName("MainUnitCode").IsRequired().HasMaxLength(256);
        }
    }

    // UnitView
    internal class BasicInfo_UnitViewConfiguration : EntityTypeConfiguration<BasicInfo_UnitView>
    {
        public BasicInfo_UnitViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".UnitView");
            HasKey(x => new { x.Id, x.Abbreviation, x.Name });

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Abbreviation).HasColumnName("Abbreviation").IsRequired().HasMaxLength(256);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
        }
    }

    // UserView
    internal class BasicInfo_UserViewConfiguration : EntityTypeConfiguration<BasicInfo_UserView>
    {
        public BasicInfo_UserViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".UserView");
            HasKey(x => new { x.Id, x.IdentityId, x.CompanyId });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.IdentityId).HasColumnName("IdentityId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsOptional().HasMaxLength(202);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.IsFrApprover).HasColumnName("IsFRApprover").IsOptional();
        }
    }

    // __VoyagesView
    internal class BasicInfo_VoyagesViewConfiguration : EntityTypeConfiguration<BasicInfo_VoyagesView>
    {
        public BasicInfo_VoyagesViewConfiguration(string schema = "BasicInfo")
        {
            ToTable(schema + ".__VoyagesView");
            HasKey(x => new { x.VoyageNumber, x.Description, x.VesselInCompanyId, x.IsActive });

            Property(x => x.Id).HasColumnName("Id").IsOptional();
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsRequired().IsUnicode(false).HasMaxLength(10);
            Property(x => x.Description).HasColumnName("Description").IsRequired().IsUnicode(false).HasMaxLength(10);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsOptional();
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.EndDate).HasColumnName("EndDate").IsOptional();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
        }
    }

    // VoyagesView
    internal class BasicInfo_VoyagesView1Configuration : EntityTypeConfiguration<BasicInfo_VoyagesView1>
    {
        public BasicInfo_VoyagesView1Configuration(string schema = "BasicInfo")
        {
            ToTable(schema + ".VoyagesView");
            HasKey(x => new { x.VesselInCompanyId, x.IsActive });

            Property(x => x.Id).HasColumnName("Id").IsOptional();
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsOptional().HasMaxLength(200);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(200);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsOptional();
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.EndDate).HasColumnName("EndDate").IsOptional();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
        }
    }

    // Accounts
    internal class Fuel_AccountConfiguration : EntityTypeConfiguration<Fuel_Account>
    {
        public Fuel_AccountConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Accounts");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
        }
    }

    // ActivityFlow
    internal class Fuel_ActivityFlowConfiguration : EntityTypeConfiguration<Fuel_ActivityFlow>
    {
        public Fuel_ActivityFlowConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".ActivityFlow");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.WorkflowStepId).HasColumnName("WorkflowStepId").IsRequired();
            Property(x => x.WorkflowNextStepId).HasColumnName("WorkflowNextStepId").IsRequired();
            Property(x => x.WorkflowAction).HasColumnName("WorkflowAction").IsRequired();
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();

            // Foreign keys
            HasRequired(a => a.ActionType).WithMany(b => b.Fuel_ActivityFlow).HasForeignKey(c => c.ActionTypeId); // FK_Fuel.ActivityFlow_ActionTypeId_dbo.ActionTypes_Id
            HasRequired(a => a.Fuel_WorkflowStep_WorkflowNextStepId).WithMany(b => b.Fuel_ActivityFlow_WorkflowNextStepId).HasForeignKey(c => c.WorkflowNextStepId); // FK_Fuel.ActivityFlow_WorkflowNextStepId_Fuel.WorkflowStep_Id
            HasRequired(a => a.Fuel_WorkflowStep_WorkflowStepId).WithMany(b => b.Fuel_ActivityFlow_WorkflowStepId).HasForeignKey(c => c.WorkflowStepId); // FK_Fuel.ActivityFlow_WorkflowStepId_Fuel.WorkflowStep_Id
        }
    }

    // ApproveFlowConfig
    internal class Fuel_ApproveFlowConfigConfiguration : EntityTypeConfiguration<Fuel_ApproveFlowConfig>
    {
        public Fuel_ApproveFlowConfigConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".ApproveFlowConfig");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ActorUserId).HasColumnName("ActorUserId").IsRequired();
            Property(x => x.NextWorkflowStepId).HasColumnName("NextWorkflowStepId").IsOptional();
            Property(x => x.WithWorkflowAction).HasColumnName("WithWorkflowAction").IsRequired();
            Property(x => x.State).HasColumnName("State").IsRequired();
            Property(x => x.WorkflowEntity).HasColumnName("WorkflowEntity").IsRequired();
            Property(x => x.CurrentWorkflowStage).HasColumnName("CurrentWorkflowStage").IsRequired();
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasOptional(a => a.Fuel_ApproveFlowConfig1).WithMany(b => b.Fuel_ApproveFlowConfig2).HasForeignKey(c => c.NextWorkflowStepId); // FK_ApproveFlowConfig_NextWorkflowStepId_ApproveFlowConfig_Id
        }
    }

    // ApproveFlowConfigValidFuelUsers
    internal class Fuel_ApproveFlowConfigValidFuelUserConfiguration : EntityTypeConfiguration<Fuel_ApproveFlowConfigValidFuelUser>
    {
        public Fuel_ApproveFlowConfigValidFuelUserConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".ApproveFlowConfigValidFuelUsers");
            HasKey(x => new { x.ApproveFlowConfigId, x.FuelUserId });

            Property(x => x.ApproveFlowConfigId).HasColumnName("ApproveFlowConfigId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FuelUserId).HasColumnName("FuelUserId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.Fuel_ApproveFlowConfig).WithMany(b => b.Fuel_ApproveFlowConfigValidFuelUser).HasForeignKey(c => c.ApproveFlowConfigId); // FK_ApproveFlowConfigValidFuelUsers_ApproveFlowConfigId_ApproveFlowConfig_Id
        }
    }

    // AsgnSegmentTypeVoucherSetingDetail
    internal class Fuel_AsgnSegmentTypeVoucherSetingDetailConfiguration : EntityTypeConfiguration<Fuel_AsgnSegmentTypeVoucherSetingDetail>
    {
        public Fuel_AsgnSegmentTypeVoucherSetingDetailConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".AsgnSegmentTypeVoucherSetingDetail");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.VoucherSetingDetailId).HasColumnName("VoucherSetingDetailId").IsRequired();
            Property(x => x.SegmentTypeId).HasColumnName("SegmentTypeId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_VoucherSetingDetail).WithMany(b => b.Fuel_AsgnSegmentTypeVoucherSetingDetail).HasForeignKey(c => c.VoucherSetingDetailId); // FK_VoucherSetingDetail_AsgnSegmentTypeVoucherSetingDetail
        }
    }

    // AsgnVoucherAconts
    internal class Fuel_AsgnVoucherAcontConfiguration : EntityTypeConfiguration<Fuel_AsgnVoucherAcont>
    {
        public Fuel_AsgnVoucherAcontConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".AsgnVoucherAconts");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.VoucherSetingDetailId).HasColumnName("VoucherSetingDetailId").IsRequired();
            Property(x => x.AccountId).HasColumnName("AccountId").IsRequired();
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasRequired(a => a.Fuel_Account).WithMany(b => b.Fuel_AsgnVoucherAcont).HasForeignKey(c => c.AccountId); // FK_Account_AsgnVoucherAcont
            HasRequired(a => a.Fuel_VoucherSetingDetail).WithMany(b => b.Fuel_AsgnVoucherAcont).HasForeignKey(c => c.VoucherSetingDetailId); // FK_VoucherSetingDetail_AsgnVoucherAconts
        }
    }

    // Attachments
    internal class Fuel_AttachmentConfiguration : EntityTypeConfiguration<Fuel_Attachment>
    {
        public Fuel_AttachmentConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Attachments");
            HasKey(x => x.RowId);

            Property(x => x.RowId).HasColumnName("RowID").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.AttachmentContent).HasColumnName("AttachmentContent").IsOptional();
            Property(x => x.AttachmentName).HasColumnName("AttachmentName").IsRequired().HasMaxLength(150);
            Property(x => x.AttachmentExt).HasColumnName("AttachmentExt").IsOptional().HasMaxLength(10);
            Property(x => x.EntityId).HasColumnName("EntityId").IsRequired();
            Property(x => x.EntityType).HasColumnName("EntityType").IsRequired();
            Property(x => x.RowGuid).HasColumnName("RowGUID").IsRequired();
        }
    }

    // Charter
    internal class Fuel_CharterConfiguration : EntityTypeConfiguration<Fuel_Charter>
    {
        public Fuel_CharterConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Charter");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CurrentState).HasColumnName("CurrentState").IsRequired();
            Property(x => x.ActionDate).HasColumnName("ActionDate").IsRequired();
            Property(x => x.CharterType).HasColumnName("CharterType").IsRequired();
            Property(x => x.CharterEndType).HasColumnName("CharterEndType").IsRequired();
            Property(x => x.ChartererId).HasColumnName("ChartererId").IsRequired();
            Property(x => x.OwnerId).HasColumnName("OwnerId").IsRequired();
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();
            Property(x => x.CurrencyId).HasColumnName("CurrencyId").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasRequired(a => a.Fuel_VesselInCompany).WithMany(b => b.Fuel_Charter).HasForeignKey(c => c.VesselInCompanyId); // FK_Charter_VesselInCompanyId_VesselInCompany_Id
        }
    }

    // CharterIn
    internal class Fuel_CharterInConfiguration : EntityTypeConfiguration<Fuel_CharterIn>
    {
        public Fuel_CharterInConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".CharterIn");
            HasKey(x => new { x.Id, x.OffHirePricingType });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.OffHirePricingType).HasColumnName("OffHirePricingType").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_Charter).WithMany(b => b.Fuel_CharterIn).HasForeignKey(c => c.Id); // FK_CharterIn_Id_Charter_Id
        }
    }

    // CharterItem
    internal class Fuel_CharterItemConfiguration : EntityTypeConfiguration<Fuel_CharterItem>
    {
        public Fuel_CharterItemConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".CharterItem");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CharterId).HasColumnName("CharterId").IsRequired();
            Property(x => x.GoodUnitId).HasColumnName("GoodUnitId").IsRequired();
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.TankId).HasColumnName("TankId").IsOptional();
            Property(x => x.Rob).HasColumnName("Rob").IsRequired().HasPrecision(18,3);
            Property(x => x.Fee).HasColumnName("Fee").IsRequired().HasPrecision(18,2);
            Property(x => x.OffhireFee).HasColumnName("OffhireFee").IsRequired().HasPrecision(18,2);
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasRequired(a => a.Fuel_Charter).WithMany(b => b.Fuel_CharterItem).HasForeignKey(c => c.CharterId); // FK_CharterItem_CharterId_Charter_Id
        }
    }

    // CharterItemHistory
    internal class Fuel_CharterItemHistoryConfiguration : EntityTypeConfiguration<Fuel_CharterItemHistory>
    {
        public Fuel_CharterItemHistoryConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".CharterItemHistory");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CharterId).HasColumnName("CharterId").IsRequired();
            Property(x => x.CharterItemId).HasColumnName("CharterItemId").IsRequired();
            Property(x => x.GoodUnitId).HasColumnName("GoodUnitId").IsRequired();
            Property(x => x.CharterStateId).HasColumnName("CharterStateId").IsRequired();
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.TankId).HasColumnName("TankId").IsOptional();
            Property(x => x.Rob).HasColumnName("Rob").IsRequired().HasPrecision(18,3);
            Property(x => x.Fee).HasColumnName("Fee").IsRequired().HasPrecision(18,3);
            Property(x => x.OffhireFee).HasColumnName("OffhireFee").IsRequired().HasPrecision(18,3);
            Property(x => x.DateRegisterd).HasColumnName("DateRegisterd").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
        }
    }

    // CharterOut
    internal class Fuel_CharterOutConfiguration : EntityTypeConfiguration<Fuel_CharterOut>
    {
        public Fuel_CharterOutConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".CharterOut");
            HasKey(x => new { x.Id, x.OffHirePricingType });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.OffHirePricingType).HasColumnName("OffHirePricingType").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_Charter).WithMany(b => b.Fuel_CharterOut).HasForeignKey(c => c.Id); // FK_CharterOut_Id_Charter_Id
        }
    }

    // EffectiveFactor
    internal class Fuel_EffectiveFactorConfiguration : EntityTypeConfiguration<Fuel_EffectiveFactor>
    {
        public Fuel_EffectiveFactorConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".EffectiveFactor");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
            Property(x => x.EffectiveFactorType).HasColumnName("EffectiveFactorType").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.AccountId).HasColumnName("AccountId").IsOptional();
            Property(x => x.VoucherDescription).HasColumnName("VoucherDescription").IsOptional().HasMaxLength(255);
            Property(x => x.VoucherRefDescription).HasColumnName("VoucherRefDescription").IsOptional().HasMaxLength(255);

            // Foreign keys
            HasOptional(a => a.Fuel_Account).WithMany(b => b.Fuel_EffectiveFactor).HasForeignKey(c => c.AccountId); // FK_EffectiveFactor_Account
        }
    }

    // EOVReportsView
    internal class Fuel_EovReportsViewConfiguration : EntityTypeConfiguration<Fuel_EovReportsView>
    {
        public Fuel_EovReportsViewConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".EOVReportsView");
            HasKey(x => new { x.DraftId, x.FuelReportType, x.PortName, x.PortTime, x.AtSeaLatitudeDegree, x.AtSeaLatitudeMinute, x.AtSeaLongitudeDegree, x.AtSeaLongitudeMinute, x.ObsDist, x.EngDist, x.SteamTime, x.AvObsSpeed, x.AvEngSpeed, x.Rpm, x.Slip, x.WindDir, x.WindForce, x.SeaDir, x.SeaForce, x.Robho, x.Robdo, x.Robmgo, x.Robfw, x.ConsInPortHo, x.ConsInPortDo, x.ConsInPortMgo, x.ConsInPortFw, x.ConsAtSeaHo, x.ConsAtSeaDo, x.ConsAtSeaMgo, x.ConsAtSeaFw, x.ReceivedHo, x.ReceivedDo, x.ReceivedMgo, x.ReceivedFw, x.EtaPort, x.DateIn, x.IsSm, x.InPortOrAtSea, x.ShipCode });

            Property(x => x.DraftId).HasColumnName("DraftID").IsRequired();
            Property(x => x.ShipId).HasColumnName("ShipID").IsOptional();
            Property(x => x.ConsNo).HasColumnName("ConsNo").IsOptional();
            Property(x => x.ShipName).HasColumnName("ShipName").IsOptional().IsUnicode(false).HasMaxLength(50);
            Property(x => x.VoyageNo).HasColumnName("VoyageNo").IsOptional().HasMaxLength(200);
            Property(x => x.Year).HasColumnName("Year").IsOptional();
            Property(x => x.Month).HasColumnName("Month").IsOptional();
            Property(x => x.Day).HasColumnName("Day").IsOptional();
            Property(x => x.Time).HasColumnName("Time").IsOptional();
            Property(x => x.FuelReportType).HasColumnName("FuelReportType").IsRequired();
            Property(x => x.PortName).HasColumnName("PortName").IsRequired().IsUnicode(false).HasMaxLength(8);
            Property(x => x.PortTime).HasColumnName("PortTime").IsRequired();
            Property(x => x.AtSeaLatitudeDegree).HasColumnName("AtSeaLatitudeDegree").IsRequired();
            Property(x => x.AtSeaLatitudeMinute).HasColumnName("AtSeaLatitudeMinute").IsRequired();
            Property(x => x.AtSeaLongitudeDegree).HasColumnName("AtSeaLongitudeDegree").IsRequired();
            Property(x => x.AtSeaLongitudeMinute).HasColumnName("AtSeaLongitudeMinute").IsRequired();
            Property(x => x.ObsDist).HasColumnName("ObsDist").IsRequired();
            Property(x => x.EngDist).HasColumnName("EngDist").IsRequired();
            Property(x => x.SteamTime).HasColumnName("SteamTime").IsRequired();
            Property(x => x.AvObsSpeed).HasColumnName("AvObsSpeed").IsRequired();
            Property(x => x.AvEngSpeed).HasColumnName("AvEngSpeed").IsRequired();
            Property(x => x.Rpm).HasColumnName("RPM").IsRequired();
            Property(x => x.Slip).HasColumnName("Slip").IsRequired();
            Property(x => x.WindDir).HasColumnName("WindDir").IsRequired();
            Property(x => x.WindForce).HasColumnName("WindForce").IsRequired();
            Property(x => x.SeaDir).HasColumnName("SeaDir").IsRequired();
            Property(x => x.SeaForce).HasColumnName("SeaForce").IsRequired();
            Property(x => x.Robho).HasColumnName("ROBHO").IsRequired();
            Property(x => x.Robdo).HasColumnName("ROBDO").IsRequired();
            Property(x => x.Robmgo).HasColumnName("ROBMGO").IsRequired();
            Property(x => x.Robfw).HasColumnName("ROBFW").IsRequired();
            Property(x => x.ConsInPortHo).HasColumnName("ConsInPortHO").IsRequired();
            Property(x => x.ConsInPortDo).HasColumnName("ConsInPortDO").IsRequired();
            Property(x => x.ConsInPortMgo).HasColumnName("ConsInPortMGO").IsRequired();
            Property(x => x.ConsInPortFw).HasColumnName("ConsInPortFW").IsRequired();
            Property(x => x.ConsAtSeaHo).HasColumnName("ConsAtSeaHO").IsRequired();
            Property(x => x.ConsAtSeaDo).HasColumnName("ConsAtSeaDO").IsRequired();
            Property(x => x.ConsAtSeaMgo).HasColumnName("ConsAtSeaMGO").IsRequired();
            Property(x => x.ConsAtSeaFw).HasColumnName("ConsAtSeaFW").IsRequired();
            Property(x => x.ReceivedHo).HasColumnName("ReceivedHO").IsRequired();
            Property(x => x.ReceivedDo).HasColumnName("ReceivedDO").IsRequired();
            Property(x => x.ReceivedMgo).HasColumnName("ReceivedMGO").IsRequired();
            Property(x => x.ReceivedFw).HasColumnName("ReceivedFW").IsRequired();
            Property(x => x.EtaPort).HasColumnName("ETAPort").IsRequired().IsUnicode(false).HasMaxLength(8);
            Property(x => x.EtaDate).HasColumnName("ETADate").IsOptional().HasMaxLength(15);
            Property(x => x.DateIn).HasColumnName("DateIn").IsRequired();
            Property(x => x.IsSm).HasColumnName("IsSM").IsRequired();
            Property(x => x.InPortOrAtSea).HasColumnName("InPortOrAtSea").IsRequired();
            Property(x => x.ImportDate).HasColumnName("ImportDate").IsOptional().HasMaxLength(15);
            Property(x => x.ShipCode).HasColumnName("ShipCode").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(4);
            Property(x => x.VoyageId).HasColumnName("VoyageId").IsOptional();
        }
    }

    // EventLogs
    internal class Fuel_EventLogConfiguration : EntityTypeConfiguration<Fuel_EventLog>
    {
        public Fuel_EventLogConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".EventLogs");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_Log).WithOptional(b => b.Fuel_EventLog); // FK_EventLogs_Id_Logs_Id
        }
    }

    // ExceptionLogs
    internal class Fuel_ExceptionLogConfiguration : EntityTypeConfiguration<Fuel_ExceptionLog>
    {
        public Fuel_ExceptionLogConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".ExceptionLogs");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_Log).WithOptional(b => b.Fuel_ExceptionLog); // FK_ExceptionLogs_Id_Logs_Id
        }
    }

    // FreeAccounts
    internal class Fuel_FreeAccountConfiguration : EntityTypeConfiguration<Fuel_FreeAccount>
    {
        public Fuel_FreeAccountConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".FreeAccounts");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
        }
    }

    // FuelReport
    internal class Fuel_FuelReportConfiguration : EntityTypeConfiguration<Fuel_FuelReport>
    {
        public Fuel_FuelReportConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".FuelReport");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(200);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(2000);
            Property(x => x.EventDate).HasColumnName("EventDate").IsRequired();
            Property(x => x.ReportDate).HasColumnName("ReportDate").IsRequired();
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();
            Property(x => x.VoyageId).HasColumnName("VoyageId").IsOptional();
            Property(x => x.FuelReportType).HasColumnName("FuelReportType").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.State).HasColumnName("State").IsRequired();
            Property(x => x.CreatedCharterId).HasColumnName("CreatedCharterId").IsOptional();

            // Foreign keys
            HasOptional(a => a.Fuel_Charter).WithMany(b => b.Fuel_FuelReport).HasForeignKey(c => c.CreatedCharterId); // FK_FuelReport_CreatedCharterId_Charter_Id
            HasRequired(a => a.Fuel_VesselInCompany).WithMany(b => b.Fuel_FuelReport).HasForeignKey(c => c.VesselInCompanyId); // FK_FuelReport_VesselInCompanyId_VesselInCompany_Id
        }
    }

    // FuelReportDetail
    internal class Fuel_FuelReportDetailConfiguration : EntityTypeConfiguration<Fuel_FuelReportDetail>
    {
        public Fuel_FuelReportDetailConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".FuelReportDetail");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.FuelReportId).HasColumnName("FuelReportId").IsRequired();
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.TankId).HasColumnName("TankId").IsOptional();
            Property(x => x.Consumption).HasColumnName("Consumption").IsRequired().HasPrecision(18,3);
            Property(x => x.Rob).HasColumnName("ROB").IsRequired().HasPrecision(18,3);
            Property(x => x.Robuom).HasColumnName("ROBUOM").IsRequired().HasMaxLength(50);
            Property(x => x.Receive).HasColumnName("Receive").IsOptional().HasPrecision(18,3);
            Property(x => x.ReceiveType).HasColumnName("ReceiveType").IsOptional();
            Property(x => x.ReceiveReferenceReferenceType).HasColumnName("ReceiveReference_ReferenceType").IsOptional();
            Property(x => x.ReceiveReferenceReferenceId).HasColumnName("ReceiveReference_ReferenceId").IsOptional();
            Property(x => x.ReceiveReferenceCode).HasColumnName("ReceiveReference_Code").IsOptional().HasMaxLength(255);
            Property(x => x.Transfer).HasColumnName("Transfer").IsOptional().HasPrecision(18,3);
            Property(x => x.TransferType).HasColumnName("TransferType").IsOptional();
            Property(x => x.TransferReferenceReferenceType).HasColumnName("TransferReference_ReferenceType").IsOptional();
            Property(x => x.TransferReferenceReferenceId).HasColumnName("TransferReference_ReferenceId").IsOptional();
            Property(x => x.TransferReferenceCode).HasColumnName("TransferReference_Code").IsOptional().HasMaxLength(255);
            Property(x => x.Correction).HasColumnName("Correction").IsOptional().HasPrecision(18,3);
            Property(x => x.CorrectionPrice).HasColumnName("CorrectionPrice").IsOptional().HasPrecision(18,2);
            Property(x => x.CorrectionType).HasColumnName("CorrectionType").IsOptional();
            Property(x => x.CorrectionReferenceReferenceType).HasColumnName("CorrectionReference_ReferenceType").IsOptional();
            Property(x => x.CorrectionReferenceReferenceId).HasColumnName("CorrectionReference_ReferenceId").IsOptional();
            Property(x => x.CorrectionReferenceCode).HasColumnName("CorrectionReference_Code").IsOptional().HasMaxLength(255);
            Property(x => x.CorrectionPriceCurrencyId).HasColumnName("CorrectionPriceCurrencyId").IsOptional();
            Property(x => x.CorrectionPriceCurrencyIsoCode).HasColumnName("CorrectionPriceCurrencyISOCode").IsOptional().HasMaxLength(20);
            Property(x => x.MeasuringUnitId).HasColumnName("MeasuringUnitId").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.CorrectionPricingType).HasColumnName("CorrectionPricingType").IsOptional();

            // Foreign keys
            HasRequired(a => a.Fuel_FuelReport).WithMany(b => b.Fuel_FuelReportDetail).HasForeignKey(c => c.FuelReportId); // FK_FuelReportDetail_FuelReportId_FuelReport_Id
        }
    }

    // InventoryOperation
    internal class Fuel_InventoryOperationConfiguration : EntityTypeConfiguration<Fuel_InventoryOperation>
    {
        public Fuel_InventoryOperationConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".InventoryOperation");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ActionNumber).HasColumnName("ActionNumber").IsRequired().HasMaxLength(200);
            Property(x => x.ActionDate).HasColumnName("ActionDate").IsRequired();
            Property(x => x.ActionType).HasColumnName("ActionType").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.FuelReportDetailId).HasColumnName("FuelReportDetailId").IsOptional();
            Property(x => x.CharterId).HasColumnName("CharterId").IsOptional();
            Property(x => x.ScrapId).HasColumnName("Scrap_Id").IsOptional();
            Property(x => x.FuelReportId).HasColumnName("FuelReport_Id").IsOptional();
            Property(x => x.InventoryOperationId).HasColumnName("InventoryOperationId").IsRequired();

            // Foreign keys
            HasOptional(a => a.Fuel_Charter).WithMany(b => b.Fuel_InventoryOperation).HasForeignKey(c => c.CharterId); // FK_InventoryOperation_CharterId_Charter_Id
            HasOptional(a => a.Fuel_FuelReport).WithMany(b => b.Fuel_InventoryOperation).HasForeignKey(c => c.FuelReportId); // FK_InventoryOperation_Id_FuelReport_Id
            HasOptional(a => a.Fuel_FuelReportDetail).WithMany(b => b.Fuel_InventoryOperation).HasForeignKey(c => c.FuelReportDetailId); // FK_InventoryOperation_FuelReportDetailId_FuelReportDetail_Id
            HasOptional(a => a.Fuel_Scrap).WithMany(b => b.Fuel_InventoryOperation).HasForeignKey(c => c.ScrapId); // FK_InventoryOperation_Scrap_Id_Scrap_Id
        }
    }

    // Invoice
    internal class Fuel_InvoiceConfiguration : EntityTypeConfiguration<Fuel_Invoice>
    {
        public Fuel_InvoiceConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Invoice");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.InvoiceDate).HasColumnName("InvoiceDate").IsRequired();
            Property(x => x.CurrencyId).HasColumnName("CurrencyId").IsRequired();
            Property(x => x.State).HasColumnName("State").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.DivisionMethod).HasColumnName("DivisionMethod").IsRequired();
            Property(x => x.InvoiceNumber).HasColumnName("InvoiceNumber").IsRequired().HasMaxLength(255);
            Property(x => x.AccountingType).HasColumnName("AccountingType").IsRequired();
            Property(x => x.InvoiceRefrenceId).HasColumnName("InvoiceRefrenceId").IsOptional();
            Property(x => x.InvoiceType).HasColumnName("InvoiceType").IsRequired();
            Property(x => x.TransporterId).HasColumnName("TransporterId").IsOptional();
            Property(x => x.SupplierId).HasColumnName("SupplierId").IsOptional();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.OwnerId).HasColumnName("OwnerId").IsRequired();
            Property(x => x.IsCreditor).HasColumnName("IsCreditor").IsRequired();
            Property(x => x.TotalOfDivisionPrice).HasColumnName("TotalOfDivisionPrice").IsRequired().HasPrecision(18,2);

            // Foreign keys
            HasOptional(a => a.Fuel_Invoice1).WithMany(b => b.Fuel_Invoice2).HasForeignKey(c => c.InvoiceRefrenceId); // FK_Invoice_InvoiceRefrenceId_Invoice_Id
            HasMany(t => t.Fuel_Order).WithMany(t => t.Fuel_Invoice).Map(m => 
            {
                m.ToTable("InvoiceOrders", schema);
                m.MapLeftKey("Invoice_Id");
                m.MapRightKey("Order_Id");
            });
        }
    }

    // InvoiceAdditionalPrices
    internal class Fuel_InvoiceAdditionalPriceConfiguration : EntityTypeConfiguration<Fuel_InvoiceAdditionalPrice>
    {
        public Fuel_InvoiceAdditionalPriceConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".InvoiceAdditionalPrices");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.EffectiveFactorId).HasColumnName("EffectiveFactorId").IsRequired();
            Property(x => x.Price).HasColumnName("Price").IsRequired().HasPrecision(18,2);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.Divisionable).HasColumnName("Divisionable").IsRequired();
            Property(x => x.InvoiceId).HasColumnName("InvoiceId").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasRequired(a => a.Fuel_EffectiveFactor).WithMany(b => b.Fuel_InvoiceAdditionalPrice).HasForeignKey(c => c.EffectiveFactorId); // FK_InvoiceAdditionalPrices_EffectiveFactorId_EffectiveFactor_Id
            HasRequired(a => a.Fuel_Invoice).WithMany(b => b.Fuel_InvoiceAdditionalPrice).HasForeignKey(c => c.InvoiceId); // FK_InvoiceAdditionalPrices_InvoiceId_Invoice_Id
        }
    }

    // InvoiceItems
    internal class Fuel_InvoiceItemConfiguration : EntityTypeConfiguration<Fuel_InvoiceItem>
    {
        public Fuel_InvoiceItemConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".InvoiceItems");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired().HasPrecision(18,3);
            Property(x => x.Fee).HasColumnName("Fee").IsRequired().HasPrecision(18,2);
            Property(x => x.InvoiceId).HasColumnName("InvoiceId").IsRequired();
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.MeasuringUnitId).HasColumnName("MeasuringUnitId").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.DivisionPrice).HasColumnName("DivisionPrice").IsRequired().HasPrecision(18,2);

            // Foreign keys
            HasRequired(a => a.Fuel_Invoice).WithMany(b => b.Fuel_InvoiceItem).HasForeignKey(c => c.InvoiceId); // FK_InvoiceItems_InvoiceId_Invoice_Id
        }
    }

    // JournalEntries
    internal class Fuel_JournalEntryConfiguration : EntityTypeConfiguration<Fuel_JournalEntry>
    {
        public Fuel_JournalEntryConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".JournalEntries");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.VoucherId).HasColumnName("VoucherId").IsRequired();
            Property(x => x.AccountNo).HasColumnName("AccountNo").IsRequired().HasMaxLength(250);
            Property(x => x.CurrencyId).HasColumnName("CurrencyId").IsRequired();
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(250);
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.VoucherRef).HasColumnName("VoucherRef").IsRequired().HasMaxLength(250);
            Property(x => x.ForeignAmount).HasColumnName("ForeignAmount").IsRequired().HasPrecision(19,5);
            Property(x => x.IrrAmount).HasColumnName("IrrAmount").IsRequired().HasPrecision(19,5);
            Property(x => x.Typ).HasColumnName("Typ").IsRequired();
            Property(x => x.InventoryItemId).HasColumnName("InventoryItemId").IsOptional();

            // Foreign keys
            HasRequired(a => a.Fuel_Voucher).WithMany(b => b.Fuel_JournalEntry).HasForeignKey(c => c.VoucherId); // FK_Voucher_JournalEntries_
        }
    }

    // Logs
    internal class Fuel_LogConfiguration : EntityTypeConfiguration<Fuel_Log>
    {
        public Fuel_LogConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Logs");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(100);
            Property(x => x.LogLevelId).HasColumnName("LogLevelId").IsRequired();
            Property(x => x.PartyId).HasColumnName("PartyId").IsRequired();
            Property(x => x.UserName).HasColumnName("UserName").IsOptional().HasMaxLength(100);
            Property(x => x.ClassName).HasColumnName("ClassName").IsOptional().HasMaxLength(200);
            Property(x => x.MethodName).HasColumnName("MethodName").IsOptional().HasMaxLength(200);
            Property(x => x.LogDate).HasColumnName("LogDate").IsRequired();
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(200);
            Property(x => x.Messages).HasColumnName("Messages").IsOptional().HasMaxLength(4000);
        }
    }

    // Offhire
    internal class Fuel_OffhireConfiguration : EntityTypeConfiguration<Fuel_Offhire>
    {
        public Fuel_OffhireConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Offhire");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ReferenceNumber).HasColumnName("ReferenceNumber").IsRequired();
            Property(x => x.StartDateTime).HasColumnName("StartDateTime").IsRequired();
            Property(x => x.EndDateTime).HasColumnName("EndDateTime").IsRequired();
            Property(x => x.IntroducerType).HasColumnName("IntroducerType").IsRequired();
            Property(x => x.VoucherDate).HasColumnName("VoucherDate").IsRequired();
            Property(x => x.VoucherCurrencyId).HasColumnName("VoucherCurrencyId").IsRequired();
            Property(x => x.PricingReferenceNumber).HasColumnName("PricingReference_Number").IsRequired().HasMaxLength(255);
            Property(x => x.PricingReferenceType).HasColumnName("PricingReference_Type").IsRequired();
            Property(x => x.State).HasColumnName("State").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.IntroducerId).HasColumnName("IntroducerId").IsRequired();
            Property(x => x.OffhireLocationId).HasColumnName("OffhireLocationId").IsRequired();
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();
            Property(x => x.VoyageId).HasColumnName("VoyageId").IsOptional();

            // Foreign keys
            HasRequired(a => a.Fuel_VesselInCompany).WithMany(b => b.Fuel_Offhire).HasForeignKey(c => c.VesselInCompanyId); // FK_Offhire_VesselInCompanyId_VesselInCompany_Id
        }
    }

    // OffhireDetail
    internal class Fuel_OffhireDetailConfiguration : EntityTypeConfiguration<Fuel_OffhireDetail>
    {
        public Fuel_OffhireDetailConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".OffhireDetail");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired().HasPrecision(18,3);
            Property(x => x.FeeInVoucherCurrency).HasColumnName("FeeInVoucherCurrency").IsRequired().HasPrecision(18,2);
            Property(x => x.FeeInMainCurrency).HasColumnName("FeeInMainCurrency").IsRequired().HasPrecision(18,2);
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.OffhireId).HasColumnName("OffhireId").IsRequired();
            Property(x => x.TankId).HasColumnName("TankId").IsOptional();
            Property(x => x.UnitId).HasColumnName("UnitId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_Offhire).WithMany(b => b.Fuel_OffhireDetail).HasForeignKey(c => c.OffhireId); // FK_OffhireDetail_Offhire_Id_Offhire_Id
        }
    }

    // Order
    internal class Fuel_OrderConfiguration : EntityTypeConfiguration<Fuel_Order>
    {
        public Fuel_OrderConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Order");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.SupplierId).HasColumnName("SupplierId").IsOptional();
            Property(x => x.ReceiverId).HasColumnName("ReceiverId").IsOptional();
            Property(x => x.TransporterId).HasColumnName("TransporterId").IsOptional();
            Property(x => x.OwnerId).HasColumnName("OwnerId").IsRequired();
            Property(x => x.OrderType).HasColumnName("OrderType").IsRequired();
            Property(x => x.OrderDate).HasColumnName("OrderDate").IsRequired();
            Property(x => x.FromVesselInCompanyId).HasColumnName("FromVesselInCompanyId").IsOptional();
            Property(x => x.ToVesselInCompanyId).HasColumnName("ToVesselInCompanyId").IsOptional();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.State).HasColumnName("State").IsRequired();

            // Foreign keys
            HasOptional(a => a.Fuel_VesselInCompany_FromVesselInCompanyId).WithMany(b => b.Fuel_Order_FromVesselInCompanyId).HasForeignKey(c => c.FromVesselInCompanyId); // FK_Order_FromVesselInCompanyId_VesselInCompany_Id
            HasOptional(a => a.Fuel_VesselInCompany_ToVesselInCompanyId).WithMany(b => b.Fuel_Order_ToVesselInCompanyId).HasForeignKey(c => c.ToVesselInCompanyId); // FK_Order_ToVesselInCompanyId_VesselInCompany_Id
        }
    }

    // OrderItems
    internal class Fuel_OrderItemConfiguration : EntityTypeConfiguration<Fuel_OrderItem>
    {
        public Fuel_OrderItemConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".OrderItems");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired().HasPrecision(18,3);
            Property(x => x.OrderId).HasColumnName("OrderId").IsRequired();
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.MeasuringUnitId).HasColumnName("MeasuringUnitId").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.InvoicedInMainUnit).HasColumnName("InvoicedInMainUnit").IsRequired().HasPrecision(18,3);
            Property(x => x.ReceivedInMainUnit).HasColumnName("ReceivedInMainUnit").IsRequired().HasPrecision(18,3);

            // Foreign keys
            HasRequired(a => a.Fuel_Order).WithMany(b => b.Fuel_OrderItem).HasForeignKey(c => c.OrderId); // FK_OrderItems_OrderId_Order_Id
        }
    }

    // OrderItemBalances
    internal class Fuel_OrderItemBalanceConfiguration : EntityTypeConfiguration<Fuel_OrderItemBalance>
    {
        public Fuel_OrderItemBalanceConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".OrderItemBalances");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OrderId).HasColumnName("OrderId").IsRequired();
            Property(x => x.OrderItemId).HasColumnName("OrderItemId").IsRequired();
            Property(x => x.QuantityAmountInMainUnit).HasColumnName("QuantityAmountInMainUnit").IsRequired().HasPrecision(18,3);
            Property(x => x.UnitCode).HasColumnName("UnitCode").IsRequired().HasMaxLength(50);
            Property(x => x.FuelReportDetailId).HasColumnName("FuelReportDetailId").IsRequired();
            Property(x => x.InvoiceItemId).HasColumnName("InvoiceItemId").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.InventoryOperationId).HasColumnName("InventoryOperationId").IsOptional();
            Property(x => x.PairingInvoiceItemId).HasColumnName("PairingInvoiceItemId").IsOptional();

            // Foreign keys
            HasOptional(a => a.Fuel_InventoryOperation).WithMany(b => b.Fuel_OrderItemBalance).HasForeignKey(c => c.InventoryOperationId); // FK_OrderItemBalances_InventoryOperationId_InventoryOperation_Id
            HasOptional(a => a.Fuel_InvoiceItem_PairingInvoiceItemId).WithMany(b => b.Fuel_OrderItemBalance_PairingInvoiceItemId).HasForeignKey(c => c.PairingInvoiceItemId); // FK_OrderItemBalances_PairingInvoiceItemId_InvoiceItems_Id
            HasRequired(a => a.Fuel_FuelReportDetail).WithMany(b => b.Fuel_OrderItemBalance).HasForeignKey(c => c.FuelReportDetailId); // FK_OrderItemBalances_FuelReportDetailId_FuelReportDetail_Id
            HasRequired(a => a.Fuel_InvoiceItem_InvoiceItemId).WithMany(b => b.Fuel_OrderItemBalance_InvoiceItemId).HasForeignKey(c => c.InvoiceItemId); // FK_OrderItemBalances_InvoiceItemId_InvoiceItems_Id
            HasRequired(a => a.Fuel_OrderItem).WithMany(b => b.Fuel_OrderItemBalance).HasForeignKey(c => c.OrderItemId); // FK_OrderItemBalances_OrderItemId_OrderItems_Id
        }
    }

    // Scrap
    internal class Fuel_ScrapConfiguration : EntityTypeConfiguration<Fuel_Scrap>
    {
        public Fuel_ScrapConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Scrap");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ScrapDate).HasColumnName("ScrapDate").IsRequired();
            Property(x => x.State).HasColumnName("State").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.SecondPartyId).HasColumnName("SecondPartyId").IsRequired();
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_VesselInCompany).WithMany(b => b.Fuel_Scrap).HasForeignKey(c => c.VesselInCompanyId); // FK_Scrap_VesselInCompanyId_VesselInCompany_Id
        }
    }

    // ScrapDetail
    internal class Fuel_ScrapDetailConfiguration : EntityTypeConfiguration<Fuel_ScrapDetail>
    {
        public Fuel_ScrapDetailConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".ScrapDetail");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Rob).HasColumnName("ROB").IsRequired();
            Property(x => x.Price).HasColumnName("Price").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.CurrencyId).HasColumnName("CurrencyId").IsRequired();
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.TankId).HasColumnName("TankId").IsOptional();
            Property(x => x.UnitId).HasColumnName("UnitId").IsRequired();
            Property(x => x.ScrapId).HasColumnName("ScrapId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_Scrap).WithMany(b => b.Fuel_ScrapDetail).HasForeignKey(c => c.ScrapId); // FK_ScrapDetail_Scrap_Id_Scrap_Id
        }
    }

    // Segments
    internal class Fuel_SegmentConfiguration : EntityTypeConfiguration<Fuel_Segment>
    {
        public Fuel_SegmentConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Segments");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.SegmentTypeId).HasColumnName("SegmentTypeId").IsRequired();
            Property(x => x.JournalEntryId).HasColumnName("JournalEntryId").IsRequired();
            Property(x => x.FreeAccountId).HasColumnName("FreeAccountId").IsOptional();
            Property(x => x.EffectiveFactorId).HasColumnName("EffectiveFactorId").IsOptional();
            Property(x => x.SegmentTypeCode).HasColumnName("SegmentTypeCode").IsOptional().HasMaxLength(50);
            Property(x => x.SegmentTypeName).HasColumnName("SegmentTypeName").IsOptional().HasMaxLength(200);

            // Foreign keys
            HasOptional(a => a.Fuel_EffectiveFactor).WithMany(b => b.Fuel_Segment).HasForeignKey(c => c.EffectiveFactorId); // FK_Segment_EffectiveFactor
            HasOptional(a => a.Fuel_FreeAccount).WithMany(b => b.Fuel_Segment).HasForeignKey(c => c.FreeAccountId); // FK_Segment_FreeAccount
            HasRequired(a => a.Fuel_JournalEntry).WithMany(b => b.Fuel_Segment).HasForeignKey(c => c.JournalEntryId); // FK_Segment_JournalEntries
        }
    }

    // UserInCompany
    internal class Fuel_UserInCompanyConfiguration : EntityTypeConfiguration<Fuel_UserInCompany>
    {
        public Fuel_UserInCompanyConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".UserInCompany");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();

            // Foreign keys
            HasRequired(a => a.User).WithMany(b => b.Fuel_UserInCompany).HasForeignKey(c => c.UserId); // FK_UserInCompany_UserId_SecurityUser_Id
        }
    }

    // Vessel
    internal class Fuel_VesselConfiguration : EntityTypeConfiguration<Fuel_Vessel>
    {
        public Fuel_VesselConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Vessel");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
            Property(x => x.OwnerId).HasColumnName("OwnerId").IsRequired();
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
        }
    }

    // VesselInCompany
    internal class Fuel_VesselInCompanyConfiguration : EntityTypeConfiguration<Fuel_VesselInCompany>
    {
        public Fuel_VesselInCompanyConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".VesselInCompany");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsOptional().HasMaxLength(200);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(2000);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.VesselId).HasColumnName("VesselId").IsRequired();
            Property(x => x.VesselStateCode).HasColumnName("VesselStateCode").IsRequired();
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasRequired(a => a.Fuel_Vessel).WithMany(b => b.Fuel_VesselInCompany).HasForeignKey(c => c.VesselId); // FK_VesselInCompany_VesselId_Vessel_Id
        }
    }

    // Vouchers
    internal class Fuel_VoucherConfiguration : EntityTypeConfiguration<Fuel_Voucher>
    {
        public Fuel_VoucherConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Vouchers");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(250);
            Property(x => x.FinancialVoucherDate).HasColumnName("FinancialVoucherDate").IsRequired();
            Property(x => x.LocalVoucherDate).HasColumnName("LocalVoucherDate").IsRequired();
            Property(x => x.LocalVoucherNo).HasColumnName("LocalVoucherNo").IsRequired().HasMaxLength(50);
            Property(x => x.ReferenceNo).HasColumnName("ReferenceNo").IsRequired().HasMaxLength(512);
            Property(x => x.VoucherRef).HasColumnName("VoucherRef").IsRequired().HasMaxLength(50);
            Property(x => x.ReferenceTypeId).HasColumnName("ReferenceTypeId").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.VoucherDetailTypeId).HasColumnName("VoucherDetailTypeId").IsRequired();
            Property(x => x.VoucherTypeId).HasColumnName("VoucherTypeId").IsRequired();
            Property(x => x.UserId).HasColumnName("UserId").IsRequired();
        }
    }

    // VoucherLogs
    internal class Fuel_VoucherLogConfiguration : EntityTypeConfiguration<Fuel_VoucherLog>
    {
        public Fuel_VoucherLogConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".VoucherLogs");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ExceptionMessage).HasColumnName("ExceptionMessage").IsRequired().HasMaxLength(1000);
            Property(x => x.StackTrace).HasColumnName("StackTrace").IsRequired().HasMaxLength(1073741823);
            Property(x => x.VoucherType).HasColumnName("VoucherType").IsRequired().HasMaxLength(30);
            Property(x => x.RefrenceNo).HasColumnName("RefrenceNo").IsRequired().HasMaxLength(512);
        }
    }

    // VoucherReportView
    internal class Fuel_VoucherReportViewConfiguration : EntityTypeConfiguration<Fuel_VoucherReportView>
    {
        public Fuel_VoucherReportViewConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".VoucherReportView");
            HasKey(x => new { x.JournalEntryId, x.AccountNo, x.JournalEntryDescription, x.JournalEntryVoucherRef, x.ForeignAmount, x.IrrAmount, x.JournalEntryType });

            Property(x => x.VoucherId).HasColumnName("VoucherId").IsOptional();
            Property(x => x.JournalEntryId).HasColumnName("JournalEntryId").IsRequired();
            Property(x => x.VoucherCompany).HasColumnName("VoucherCompany").IsOptional().HasMaxLength(256);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(250);
            Property(x => x.FinancialVoucherDate).HasColumnName("FinancialVoucherDate").IsOptional();
            Property(x => x.LocalVoucherDate).HasColumnName("LocalVoucherDate").IsOptional();
            Property(x => x.LocalVoucherNo).HasColumnName("LocalVoucherNo").IsOptional().HasMaxLength(50);
            Property(x => x.ReferenceNo).HasColumnName("ReferenceNo").IsOptional().HasMaxLength(50);
            Property(x => x.VoucherRef).HasColumnName("VoucherRef").IsOptional().HasMaxLength(50);
            Property(x => x.ReferenceTypeId).HasColumnName("ReferenceTypeId").IsOptional();
            Property(x => x.VoucherDetailTypeId).HasColumnName("VoucherDetailTypeId").IsOptional();
            Property(x => x.VoucherTypeId).HasColumnName("VoucherTypeId").IsOptional();
            Property(x => x.AccountNo).HasColumnName("AccountNo").IsRequired().HasMaxLength(250);
            Property(x => x.JournalEntryDescription).HasColumnName("JournalEntryDescription").IsRequired().HasMaxLength(250);
            Property(x => x.JournalEntryVoucherRef).HasColumnName("JournalEntryVoucherRef").IsRequired().HasMaxLength(250);
            Property(x => x.ForeignAmount).HasColumnName("ForeignAmount").IsRequired().HasPrecision(19,5);
            Property(x => x.IrrAmount).HasColumnName("IrrAmount").IsRequired().HasPrecision(19,5);
            Property(x => x.JournalEntryType).HasColumnName("JournalEntryType").IsRequired();
            Property(x => x.JournalEntryCurrency).HasColumnName("JournalEntryCurrency").IsOptional().HasMaxLength(256);
            Property(x => x.SegmentName).HasColumnName("SegmentName").IsOptional().HasMaxLength(50);
            Property(x => x.SegmentCode).HasColumnName("SegmentCode").IsOptional().HasMaxLength(50);
            Property(x => x.SegmentTypeName).HasColumnName("SegmentTypeName").IsOptional().HasMaxLength(200);
            Property(x => x.SegmentTypeCode).HasColumnName("SegmentTypeCode").IsOptional().HasMaxLength(50);
            Property(x => x.UserName).HasColumnName("UserName").IsOptional().HasMaxLength(202);
        }
    }

    // VoucherSetings
    internal class Fuel_VoucherSetingConfiguration : EntityTypeConfiguration<Fuel_VoucherSeting>
    {
        public Fuel_VoucherSetingConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".VoucherSetings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.VoucherMainRefDescription).HasColumnName("VoucherMainRefDescription").IsRequired().HasMaxLength(250);
            Property(x => x.VoucherMainDescription).HasColumnName("VoucherMainDescription").IsRequired().HasMaxLength(250);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.VoucherDetailTypeId).HasColumnName("VoucherDetailTypeId").IsRequired();
            Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
            Property(x => x.VoucherTypeId).HasColumnName("VoucherTypeId").IsRequired();
        }
    }

    // VoucherSetingDetails
    internal class Fuel_VoucherSetingDetailConfiguration : EntityTypeConfiguration<Fuel_VoucherSetingDetail>
    {
        public Fuel_VoucherSetingDetailConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".VoucherSetingDetails");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.VoucherSetingId).HasColumnName("VoucherSetingId").IsRequired();
            Property(x => x.VoucherCeditRefDescription).HasColumnName("VoucherCeditRefDescription").IsRequired().HasMaxLength(250);
            Property(x => x.VoucherDebitDescription).HasColumnName("VoucherDebitDescription").IsRequired().HasMaxLength(250);
            Property(x => x.VoucherDebitRefDescription).HasColumnName("VoucherDebitRefDescription").IsRequired().HasMaxLength(250);
            Property(x => x.VoucherCreditDescription).HasColumnName("VoucherCreditDescription").IsRequired().HasMaxLength(250);
            Property(x => x.GoodId).HasColumnName("GoodId").IsRequired();
            Property(x => x.IsDelete).HasColumnName("IsDelete").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_VoucherSeting).WithMany(b => b.Fuel_VoucherSetingDetail).HasForeignKey(c => c.VoucherSetingId); // FK_VoucherSeting
        }
    }

    // Voyage
    internal class Fuel_VoyageConfiguration : EntityTypeConfiguration<Fuel_Voyage>
    {
        public Fuel_VoyageConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Voyage");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsRequired().HasMaxLength(200);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(200);
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
            Property(x => x.EndDate).HasColumnName("EndDate").IsOptional();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_VesselInCompany).WithMany(b => b.Fuel_Voyage).HasForeignKey(c => c.VesselInCompanyId); // FK_Voyage_VesselInCompanyId_VesselInCompany_Id
        }
    }

    // VoyageLog
    internal class Fuel_VoyageLogConfiguration : EntityTypeConfiguration<Fuel_VoyageLog>
    {
        public Fuel_VoyageLogConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".VoyageLog");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ReferencedVoyageId).HasColumnName("ReferencedVoyageId").IsRequired();
            Property(x => x.ChangeDate).HasColumnName("ChangeDate").IsRequired();
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsRequired().HasMaxLength(200);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(200);
            Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
            Property(x => x.EndDate).HasColumnName("EndDate").IsRequired();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.VesselInCompanyId).HasColumnName("VesselInCompanyId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_VesselInCompany).WithMany(b => b.Fuel_VoyageLog).HasForeignKey(c => c.VesselInCompanyId); // FK_VoyageLog_VesselInCompanyId_VesselInCompany_Id
        }
    }

    // Workflow
    internal class Fuel_WorkflowConfiguration : EntityTypeConfiguration<Fuel_Workflow>
    {
        public Fuel_WorkflowConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".Workflow");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
            Property(x => x.WorkflowEntity).HasColumnName("WorkflowEntity").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
        }
    }

    // WorkflowLog
    internal class Fuel_WorkflowLogConfiguration : EntityTypeConfiguration<Fuel_WorkflowLog>
    {
        public Fuel_WorkflowLogConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".WorkflowLog");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.WorkflowEntity).HasColumnName("WorkflowEntity").IsRequired();
            Property(x => x.ActionDate).HasColumnName("ActionDate").IsRequired();
            Property(x => x.WorkflowAction).HasColumnName("WorkflowAction").IsOptional();
            Property(x => x.ActorUserId).HasColumnName("ActorUserId").IsRequired();
            Property(x => x.Remark).HasColumnName("Remark").IsOptional().HasMaxLength(255);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CurrentWorkflowStepId).HasColumnName("CurrentWorkflowStepId").IsRequired();
            Property(x => x.InvoiceId).HasColumnName("InvoiceId").IsOptional();
            Property(x => x.OrderId).HasColumnName("OrderId").IsOptional();
            Property(x => x.CharterId).HasColumnName("CharterId").IsOptional();
            Property(x => x.FuelReportId).HasColumnName("FuelReportId").IsOptional();
            Property(x => x.OffhireId).HasColumnName("OffhireId").IsOptional();
            Property(x => x.ScrapId).HasColumnName("ScrapId").IsOptional();
            Property(x => x.Discriminator).HasColumnName("Discriminator").IsRequired().HasMaxLength(128);
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasOptional(a => a.Fuel_Charter).WithMany(b => b.Fuel_WorkflowLog).HasForeignKey(c => c.CharterId); // FK_WorkflowLog_CharterId_Charter_Id
            HasOptional(a => a.Fuel_FuelReport).WithMany(b => b.Fuel_WorkflowLog).HasForeignKey(c => c.FuelReportId); // FK_WorkflowLog_FuelReportId_FuelReport_Id
            HasOptional(a => a.Fuel_Invoice).WithMany(b => b.Fuel_WorkflowLog).HasForeignKey(c => c.InvoiceId); // FK_WorkflowLog_InvoiceId_Invoice_Id
            HasOptional(a => a.Fuel_Offhire).WithMany(b => b.Fuel_WorkflowLog).HasForeignKey(c => c.OffhireId); // FK_WorkflowLog_OffhireId_Offhire_Id
            HasOptional(a => a.Fuel_Order).WithMany(b => b.Fuel_WorkflowLog).HasForeignKey(c => c.OrderId); // FK_WorkflowLog_OrderId_Order_Id
            HasOptional(a => a.Fuel_Scrap).WithMany(b => b.Fuel_WorkflowLog).HasForeignKey(c => c.ScrapId); // FK_WorkflowLog_ScrapId_Scrap_Id
            HasRequired(a => a.Fuel_WorkflowStep).WithMany(b => b.Fuel_WorkflowLog).HasForeignKey(c => c.CurrentWorkflowStepId); // FK_WorkflowLog_CurrentWorkflowStepId_WorkflowStep_Id
        }
    }

    // WorkflowLog_Old
    internal class Fuel_WorkflowLogOldConfiguration : EntityTypeConfiguration<Fuel_WorkflowLogOld>
    {
        public Fuel_WorkflowLogOldConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".WorkflowLog_Old");
            HasKey(x => new { x.Id, x.WorkflowEntity, x.ActionDate, x.ActorUserId, x.Active, x.CurrentWorkflowStepId, x.Discriminator, x.RowVersion });

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.WorkflowEntity).HasColumnName("WorkflowEntity").IsRequired();
            Property(x => x.ActionDate).HasColumnName("ActionDate").IsRequired();
            Property(x => x.WorkflowAction).HasColumnName("WorkflowAction").IsOptional();
            Property(x => x.ActorUserId).HasColumnName("ActorUserId").IsRequired();
            Property(x => x.Remark).HasColumnName("Remark").IsOptional().HasMaxLength(255);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CurrentWorkflowStepId).HasColumnName("CurrentWorkflowStepId").IsRequired();
            Property(x => x.InvoiceId).HasColumnName("InvoiceId").IsOptional();
            Property(x => x.OrderId).HasColumnName("OrderId").IsOptional();
            Property(x => x.CharterId).HasColumnName("CharterId").IsOptional();
            Property(x => x.FuelReportId).HasColumnName("FuelReportId").IsOptional();
            Property(x => x.OffhireId).HasColumnName("OffhireId").IsOptional();
            Property(x => x.ScrapId).HasColumnName("ScrapId").IsOptional();
            Property(x => x.Discriminator).HasColumnName("Discriminator").IsRequired().HasMaxLength(128);
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
        }
    }

    // WorkflowStep
    internal class Fuel_WorkflowStepConfiguration : EntityTypeConfiguration<Fuel_WorkflowStep>
    {
        public Fuel_WorkflowStepConfiguration(string schema = "Fuel")
        {
            ToTable(schema + ".WorkflowStep");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.WorkflowId).HasColumnName("WorkflowId").IsRequired();
            Property(x => x.State).HasColumnName("State").IsRequired();
            Property(x => x.CurrentWorkflowStage).HasColumnName("CurrentWorkflowStage").IsRequired();

            // Foreign keys
            HasRequired(a => a.Fuel_Workflow).WithMany(b => b.Fuel_WorkflowStep).HasForeignKey(c => c.WorkflowId); // FK_Fuel.WorkflowStep_WorkflowId_Fuel.Workflow_Id
        }
    }

    // Groups
    internal class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Groups");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(256);

            // Foreign keys
            HasRequired(a => a.Party).WithOptional(b => b.Group); // FK_Groups_Id_Parties_Id
        }
    }

    // HAFEZAccountListView
    internal class HafezAccountListViewConfiguration : EntityTypeConfiguration<HafezAccountListView>
    {
        public HafezAccountListViewConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".HAFEZAccountListView");
            HasKey(x => new { x.AccountListId, x.AccountCode, x.Nature, x.LevelCode, x.Name, x.NameL, x.Disabled });

            Property(x => x.AccountListId).HasColumnName("AccountListID").IsRequired();
            Property(x => x.ParentId).HasColumnName("ParentID").IsOptional();
            Property(x => x.AccountCode).HasColumnName("AccountCode").IsRequired().IsUnicode(false).HasMaxLength(12);
            Property(x => x.Nature).HasColumnName("Nature").IsRequired();
            Property(x => x.LevelCode).HasColumnName("LevelCode").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(60);
            Property(x => x.NameL).HasColumnName("NameL").IsRequired().HasMaxLength(60);
            Property(x => x.Disabled).HasColumnName("Disabled").IsRequired();
        }
    }

    // HAFEZVoyagesView
    internal class HafezVoyagesViewConfiguration : EntityTypeConfiguration<HafezVoyagesView>
    {
        public HafezVoyagesViewConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".HAFEZVoyagesView");
            HasKey(x => new { x.Id, x.VesselCode, x.ShipOwnerId, x.TripType, x.IsActive });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.VesselCode).HasColumnName("VesselCode").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(4);
            Property(x => x.ShipOwnerId).HasColumnName("ShipOwnerId").IsRequired();
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsOptional().IsUnicode(false).HasMaxLength(12);
            Property(x => x.StartDateTime).HasColumnName("StartDateTime").IsOptional();
            Property(x => x.EndDateTime).HasColumnName("EndDateTime").IsOptional();
            Property(x => x.TripType).HasColumnName("TripType").IsRequired();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
        }
    }

    // HAFIZAccountListView
    internal class HafizAccountListViewConfiguration : EntityTypeConfiguration<HafizAccountListView>
    {
        public HafizAccountListViewConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".HAFIZAccountListView");
            HasKey(x => new { x.AccountListId, x.AccountCode, x.Nature, x.LevelCode, x.Name, x.NameL, x.Disabled });

            Property(x => x.AccountListId).HasColumnName("AccountListID").IsRequired();
            Property(x => x.ParentId).HasColumnName("ParentID").IsOptional();
            Property(x => x.AccountCode).HasColumnName("AccountCode").IsRequired().IsUnicode(false).HasMaxLength(12);
            Property(x => x.Nature).HasColumnName("Nature").IsRequired();
            Property(x => x.LevelCode).HasColumnName("LevelCode").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(60);
            Property(x => x.NameL).HasColumnName("NameL").IsRequired().HasMaxLength(60);
            Property(x => x.Disabled).HasColumnName("Disabled").IsRequired();
        }
    }

    // HAFIZVoyagesView
    internal class HafizVoyagesViewConfiguration : EntityTypeConfiguration<HafizVoyagesView>
    {
        public HafizVoyagesViewConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".HAFIZVoyagesView");
            HasKey(x => new { x.Id, x.VesselCode, x.ShipOwnerId, x.TripType, x.IsActive });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.VesselCode).HasColumnName("VesselCode").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(4);
            Property(x => x.ShipOwnerId).HasColumnName("ShipOwnerId").IsRequired();
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsOptional().IsUnicode(false).HasMaxLength(12);
            Property(x => x.StartDateTime).HasColumnName("StartDateTime").IsOptional();
            Property(x => x.EndDateTime).HasColumnName("EndDateTime").IsOptional();
            Property(x => x.TripType).HasColumnName("TripType").IsRequired();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
        }
    }

    // Companies
    internal class Inventory_CompanyConfiguration : EntityTypeConfiguration<Inventory_Company>
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
        }
    }

    // ErrorMessages
    internal class Inventory_ErrorMessageConfiguration : EntityTypeConfiguration<Inventory_ErrorMessage>
    {
        public Inventory_ErrorMessageConfiguration(string schema = "Inventory")
        {
            ToTable(schema + ".ErrorMessages");
            HasKey(x => new { x.ErrorMessage, x.TextMessage, x.Action });

            Property(x => x.ErrorMessage).HasColumnName("ErrorMessage").IsRequired().HasMaxLength(200).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.TextMessage).HasColumnName("TextMessage").IsRequired().HasMaxLength(200).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(20).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }

    // FinancialYear
    internal class Inventory_FinancialYearConfiguration : EntityTypeConfiguration<Inventory_FinancialYear>
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
        }
    }

    // Goods
    internal class Inventory_GoodConfiguration : EntityTypeConfiguration<Inventory_Good>
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
            HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Good).HasForeignKey(c => c.UserCreatorId); // FK_Goods_UserCreatorId
            HasRequired(a => a.Inventory_Unit).WithMany(b => b.Inventory_Good).HasForeignKey(c => c.MainUnitId); // FK_Goods_MainUnitId
        }
    }

    // OperationReference
    internal class Inventory_OperationReferenceConfiguration : EntityTypeConfiguration<Inventory_OperationReference>
    {
        public Inventory_OperationReferenceConfiguration(string schema = "Inventory")
        {
            ToTable(schema + ".OperationReference");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OperationId).HasColumnName("OperationId").IsRequired();
            Property(x => x.OperationType).HasColumnName("OperationType").IsRequired();
            Property(x => x.ReferenceType).HasColumnName("ReferenceType").IsRequired().IsUnicode(false).HasMaxLength(256);
            Property(x => x.ReferenceNumber).HasColumnName("ReferenceNumber").IsRequired().IsUnicode(false).HasMaxLength(256);
            Property(x => x.RegistrationDate).HasColumnName("RegistrationDate").IsRequired();
        }
    }

    // StoreTypes
    internal class Inventory_StoreTypeConfiguration : EntityTypeConfiguration<Inventory_StoreType>
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
        }
    }

    // TimeBucket
    internal class Inventory_TimeBucketConfiguration : EntityTypeConfiguration<Inventory_TimeBucket>
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
            HasRequired(a => a.Inventory_User).WithMany(b => b.Inventory_TimeBucket).HasForeignKey(c => c.UserCreatorId); // FK_TimeBucket_UserCreatorId
        }
    }

    // Transactions
    internal class Inventory_TransactionConfiguration : EntityTypeConfiguration<Inventory_Transaction>
    {
        public Inventory_TransactionConfiguration(string schema = "Inventory")
        {
            ToTable(schema + ".Transactions");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Action).HasColumnName("Action").IsRequired();
            Property(x => x.Code).HasColumnName("Code").IsOptional().HasPrecision(20,2);
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

            // Foreign keys
            HasOptional(a => a.Inventory_Transaction1).WithMany(b => b.Inventory_Transaction2).HasForeignKey(c => c.PricingReferenceId); // FK_Transaction_PricingReferenceId
            HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Transaction).HasForeignKey(c => c.UserCreatorId); // FK_Transaction_UserCreatorId
            HasRequired(a => a.Inventory_StoreType).WithMany(b => b.Inventory_Transaction).HasForeignKey(c => c.StoreTypesId); // FK_Transaction_StoreTypesId
            HasRequired(a => a.Inventory_Warehouse).WithMany(b => b.Inventory_Transaction).HasForeignKey(c => c.WarehouseId); // FK_Transaction_WarehouseId
        }
    }

    // TransactionItems
    internal class Inventory_TransactionItemConfiguration : EntityTypeConfiguration<Inventory_TransactionItem>
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
            Property(x => x.QuantityAmount).HasColumnName("QuantityAmount").IsOptional().HasPrecision(20,3);
            Property(x => x.Description).HasColumnName("Description").IsOptional();
            Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
            HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.UserCreatorId); // FK_TransactionItems_UserCreatorId
            HasRequired(a => a.Inventory_Good).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.GoodId); // FK_TransactionItems_GoodId
            HasRequired(a => a.Inventory_Transaction).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.TransactionId); // FK_TransactionItems_TransactionId
            HasRequired(a => a.Inventory_Unit).WithMany(b => b.Inventory_TransactionItem).HasForeignKey(c => c.QuantityUnitId); // FK_TransactionItems_QuantityUnitId
        }
    }

    // TransactionItemPrices
    internal class Inventory_TransactionItemPriceConfiguration : EntityTypeConfiguration<Inventory_TransactionItemPrice>
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
            Property(x => x.QuantityAmount).HasColumnName("QuantityAmount").IsOptional().HasPrecision(20,3);
            Property(x => x.PriceUnitId).HasColumnName("PriceUnitId").IsRequired();
            Property(x => x.Fee).HasColumnName("Fee").IsOptional().HasPrecision(20,3);
            Property(x => x.MainCurrencyUnitId).HasColumnName("MainCurrencyUnitId").IsRequired();
            Property(x => x.FeeInMainCurrency).HasColumnName("FeeInMainCurrency").IsOptional().HasPrecision(20,3);
            Property(x => x.RegistrationDate).HasColumnName("RegistrationDate").IsOptional();
            Property(x => x.QuantityAmountUseFifo).HasColumnName("QuantityAmountUseFIFO").IsOptional().HasPrecision(20,3);
            Property(x => x.TransactionReferenceId).HasColumnName("TransactionReferenceId").IsOptional();
            Property(x => x.IssueReferenceIds).HasColumnName("IssueReferenceIds").IsOptional();
            Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
            HasOptional(a => a.Inventory_TransactionItemPrice1).WithMany(b => b.Inventory_TransactionItemPrice2).HasForeignKey(c => c.TransactionReferenceId); // FK_TransactionItemPrices_TransactionReferenceId
            HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_TransactionItemPrice).HasForeignKey(c => c.UserCreatorId); // FK_TransactionItemPrices_UserCreatorId
            HasRequired(a => a.Inventory_TransactionItem).WithMany(b => b.Inventory_TransactionItemPrice).HasForeignKey(c => c.TransactionItemId); // FK_TransactionItemPrices_TransactionItemsId
            HasRequired(a => a.Inventory_Unit_MainCurrencyUnitId).WithMany(b => b.Inventory_TransactionItemPrice_MainCurrencyUnitId).HasForeignKey(c => c.MainCurrencyUnitId); // FK_TransactionItemPrices_MainCurrencyUnitId
            HasRequired(a => a.Inventory_Unit_PriceUnitId).WithMany(b => b.Inventory_TransactionItemPrice_PriceUnitId).HasForeignKey(c => c.PriceUnitId); // FK_TransactionItemPrices_PriceUnitId
            HasRequired(a => a.Inventory_Unit_QuantityUnitId).WithMany(b => b.Inventory_TransactionItemPrice_QuantityUnitId).HasForeignKey(c => c.QuantityUnitId); // FK_TransactionItemPrices_QuantityUnitId
        }
    }

    // Units
    internal class Inventory_UnitConfiguration : EntityTypeConfiguration<Inventory_Unit>
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
        }
    }

    // UnitConverts
    internal class Inventory_UnitConvertConfiguration : EntityTypeConfiguration<Inventory_UnitConvert>
    {
        public Inventory_UnitConvertConfiguration(string schema = "Inventory")
        {
            ToTable(schema + ".UnitConverts");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UnitId).HasColumnName("UnitId").IsRequired();
            Property(x => x.SubUnitId).HasColumnName("SubUnitId").IsRequired();
            Property(x => x.Coefficient).HasColumnName("Coefficient").IsRequired().HasPrecision(18,3);
            Property(x => x.EffectiveDateStart).HasColumnName("EffectiveDateStart").IsOptional();
            Property(x => x.EffectiveDateEnd).HasColumnName("EffectiveDateEnd").IsOptional();
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired();
            Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsRequired();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
            HasRequired(a => a.Inventory_Unit_SubUnitId).WithMany(b => b.Inventory_UnitConvert_SubUnitId).HasForeignKey(c => c.SubUnitId); // FK_UnitConverts_SubUnitId
            HasRequired(a => a.Inventory_Unit_UnitId).WithMany(b => b.Inventory_UnitConvert_UnitId).HasForeignKey(c => c.UnitId); // FK_UnitConverts_UnitId
            HasRequired(a => a.Inventory_User).WithMany(b => b.Inventory_UnitConvert).HasForeignKey(c => c.UserCreatorId); // FK_UnitConverts_UserCreatorId
        }
    }

    // Users
    internal class Inventory_UserConfiguration : EntityTypeConfiguration<Inventory_User>
    {
        public Inventory_UserConfiguration(string schema = "Inventory")
        {
            ToTable(schema + ".Users");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Code).HasColumnName("Code").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            Property(x => x.UserName).HasColumnName("User_Name").IsRequired().IsUnicode(false).HasMaxLength(256);
            Property(x => x.Password).HasColumnName("Password").IsRequired().HasMaxLength(100);
            Property(x => x.IsActive).HasColumnName("IsActive").IsOptional();
            Property(x => x.EmailAddress).HasColumnName("Email_Address").IsOptional().IsUnicode(false).HasMaxLength(256);
            Property(x => x.IpAddress).HasColumnName("IPAddress").IsOptional().IsUnicode(false).HasMaxLength(15);
            Property(x => x.Login).HasColumnName("Login").IsOptional();
            Property(x => x.SessionId).HasColumnName("SessionId").IsOptional().HasMaxLength(88);
            Property(x => x.UserCreatorId).HasColumnName("UserCreatorId").IsOptional();
            Property(x => x.CreateDate).HasColumnName("CreateDate").IsOptional();

            // Foreign keys
            HasOptional(a => a.Inventory_User1).WithMany(b => b.Inventory_User2).HasForeignKey(c => c.UserCreatorId); // FK_Users_UserCreatorId
        }
    }

    // Warehouse
    internal class Inventory_WarehouseConfiguration : EntityTypeConfiguration<Inventory_Warehouse>
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
            HasOptional(a => a.Inventory_User).WithMany(b => b.Inventory_Warehouse).HasForeignKey(c => c.UserCreatorId); // FK_Warehouse_UserCreatorId
            HasRequired(a => a.Inventory_Company).WithMany(b => b.Inventory_Warehouse).HasForeignKey(c => c.CompanyId); // FK_Warehouse_CompanyId
        }
    }

    // OffhireFuelTypeFuelGoodCode
    internal class Offhire_OffhireFuelTypeFuelGoodCodeConfiguration : EntityTypeConfiguration<Offhire_OffhireFuelTypeFuelGoodCode>
    {
        public Offhire_OffhireFuelTypeFuelGoodCodeConfiguration(string schema = "Offhire")
        {
            ToTable(schema + ".OffhireFuelTypeFuelGoodCode");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OffhireFuelType).HasColumnName("OffhireFuelType").IsRequired().HasMaxLength(50);
            Property(x => x.FuelGoodCode).HasColumnName("FuelGoodCode").IsRequired().HasMaxLength(50);
            Property(x => x.ActiveFrom).HasColumnName("ActiveFrom").IsOptional();
            Property(x => x.ActiveTo).HasColumnName("ActiveTo").IsOptional();
        }
    }

    // OffhireMeasureTypeFuelMeasureCode
    internal class Offhire_OffhireMeasureTypeFuelMeasureCodeConfiguration : EntityTypeConfiguration<Offhire_OffhireMeasureTypeFuelMeasureCode>
    {
        public Offhire_OffhireMeasureTypeFuelMeasureCodeConfiguration(string schema = "Offhire")
        {
            ToTable(schema + ".OffhireMeasureTypeFuelMeasureCode");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OffhireMeasureType).HasColumnName("OffhireMeasureType").IsRequired().HasMaxLength(50);
            Property(x => x.FuelMeasureCode).HasColumnName("FuelMeasureCode").IsRequired().HasMaxLength(50);
            Property(x => x.ActiveFrom).HasColumnName("ActiveFrom").IsOptional();
            Property(x => x.ActiveTo).HasColumnName("ActiveTo").IsOptional();
        }
    }

    // Parties_CustomActions
    internal class PartiesCustomActionsConfiguration : EntityTypeConfiguration<PartiesCustomActions>
    {
        public PartiesCustomActionsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Parties_CustomActions");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PartyId).HasColumnName("PartyId").IsRequired();
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.IsGranted).HasColumnName("IsGranted").IsRequired();

            // Foreign keys
            HasRequired(a => a.ActionType).WithMany(b => b.PartiesCustomActions).HasForeignKey(c => c.ActionTypeId); // FK_Parties_CustomActions_ActionTypeId_ActionTypes_Id
            HasRequired(a => a.Party).WithMany(b => b.PartiesCustomActions).HasForeignKey(c => c.PartyId); // FK_Parties_CustomActions_PartyId_Parties_Id
        }
    }

    // Parties
    internal class PartyConfiguration : EntityTypeConfiguration<Party>
    {
        public PartyConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Parties");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.PartyName).HasColumnName("PartyName").IsRequired().HasMaxLength(100);
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
        }
    }

    // SAPIDAccountListView
    internal class SapidAccountListViewConfiguration : EntityTypeConfiguration<SapidAccountListView>
    {
        public SapidAccountListViewConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".SAPIDAccountListView");
            HasKey(x => new { x.AccountListId, x.AccountCode, x.Nature, x.LevelCode, x.Name, x.NameL, x.Disabled });

            Property(x => x.AccountListId).HasColumnName("AccountListID").IsRequired();
            Property(x => x.ParentId).HasColumnName("ParentID").IsOptional();
            Property(x => x.AccountCode).HasColumnName("AccountCode").IsRequired().IsUnicode(false).HasMaxLength(12);
            Property(x => x.Nature).HasColumnName("Nature").IsRequired();
            Property(x => x.LevelCode).HasColumnName("LevelCode").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(60);
            Property(x => x.NameL).HasColumnName("NameL").IsRequired().HasMaxLength(60);
            Property(x => x.Disabled).HasColumnName("Disabled").IsRequired();
        }
    }

    // SAPIDVoyagesView
    internal class SapidVoyagesViewConfiguration : EntityTypeConfiguration<SapidVoyagesView>
    {
        public SapidVoyagesViewConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".SAPIDVoyagesView");
            HasKey(x => new { x.Id, x.VesselCode, x.ShipOwnerId, x.TripType, x.IsActive });

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.VesselCode).HasColumnName("VesselCode").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(4);
            Property(x => x.ShipOwnerId).HasColumnName("ShipOwnerId").IsRequired();
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsOptional().IsUnicode(false).HasMaxLength(12);
            Property(x => x.StartDateTime).HasColumnName("StartDateTime").IsOptional();
            Property(x => x.EndDateTime).HasColumnName("EndDateTime").IsOptional();
            Property(x => x.TripType).HasColumnName("TripType").IsRequired();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
        }
    }

    // sysdiagrams
    internal class SysdiagramConfiguration : EntityTypeConfiguration<Sysdiagram>
    {
        public SysdiagramConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".sysdiagrams");
            HasKey(x => x.DiagramId);

            Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(128);
            Property(x => x.PrincipalId).HasColumnName("principal_id").IsRequired();
            Property(x => x.DiagramId).HasColumnName("diagram_id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Version).HasColumnName("version").IsOptional();
            Property(x => x.Definition).HasColumnName("definition").IsOptional();
        }
    }

    // Users
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Users");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FirstName).HasColumnName("FirstName").IsOptional().HasMaxLength(100);
            Property(x => x.UserName).HasColumnName("UserName").IsOptional().HasMaxLength(100);
            Property(x => x.LastName).HasColumnName("LastName").IsOptional().HasMaxLength(100);
            Property(x => x.Email).HasColumnName("Email").IsOptional().HasMaxLength(100);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Party).WithOptional(b => b.User); // FK_Users_Id_Parties_Id
        }
    }

    // Users_Groups
    internal class UsersGroupsConfiguration : EntityTypeConfiguration<UsersGroups>
    {
        public UsersGroupsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Users_Groups");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired();
            Property(x => x.GroupId).HasColumnName("GroupId").IsRequired();

            // Foreign keys
            HasRequired(a => a.Group).WithMany(b => b.UsersGroups).HasForeignKey(c => c.GroupId); // FK_Users_Groups_GroupId_Groups_Id
            HasRequired(a => a.User).WithMany(b => b.UsersGroups).HasForeignKey(c => c.UserId); // FK_Users_Groups_UserId_Users_Id
        }
    }

    // VersionInfo
    internal class VersionInfoConfiguration : EntityTypeConfiguration<VersionInfo>
    {
        public VersionInfoConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".VersionInfo");
            HasKey(x => x.Version);

            Property(x => x.Version).HasColumnName("Version").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.AppliedOn).HasColumnName("AppliedOn").IsOptional();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(1024);
        }
    }

    // FuelReportCommandLog
    internal class Vessel_FuelReportCommandLogConfiguration : EntityTypeConfiguration<Vessel_FuelReportCommandLog>
    {
        public Vessel_FuelReportCommandLogConfiguration(string schema = "Vessel")
        {
            ToTable(schema + ".FuelReportCommandLog");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.VesselReportReference).HasColumnName("VesselReportReference").IsRequired().HasMaxLength(255);
            Property(x => x.VesselCode).HasColumnName("VesselCode").IsRequired().HasMaxLength(255);
            Property(x => x.FuelReportType).HasColumnName("FuelReportType").IsRequired();
            Property(x => x.ReportDate).HasColumnName("ReportDate").IsRequired();
            Property(x => x.EventDate).HasColumnName("EventDate").IsRequired();
            Property(x => x.VoyageNumber).HasColumnName("VoyageNumber").IsOptional().HasMaxLength(255);
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
            Property(x => x.ProcessState).HasColumnName("ProcessState").IsRequired();
            Property(x => x.ProcessDate).HasColumnName("ProcessDate").IsOptional();
            Property(x => x.Remark).HasColumnName("Remark").IsOptional().HasMaxLength(255);
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();
        }
    }

    // FuelReportCommandLogDetail
    internal class Vessel_FuelReportCommandLogDetailConfiguration : EntityTypeConfiguration<Vessel_FuelReportCommandLogDetail>
    {
        public Vessel_FuelReportCommandLogDetailConfiguration(string schema = "Vessel")
        {
            ToTable(schema + ".FuelReportCommandLogDetail");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.FuelReportCommandLogId).HasColumnName("FuelReportCommandLogId").IsRequired();
            Property(x => x.FuelType).HasColumnName("FuelType").IsRequired().HasMaxLength(255);
            Property(x => x.Consumption).HasColumnName("Consumption").IsRequired().HasPrecision(19,5);
            Property(x => x.Rob).HasColumnName("ROB").IsRequired().HasPrecision(19,5);
            Property(x => x.Transfer).HasColumnName("Transfer").IsOptional().HasPrecision(19,5);
            Property(x => x.Receive).HasColumnName("Receive").IsOptional().HasPrecision(19,5);
            Property(x => x.Correction).HasColumnName("Correction").IsOptional().HasPrecision(19,5);
            Property(x => x.TankCode).HasColumnName("TankCode").IsOptional().HasMaxLength(255);
            Property(x => x.MeasuringUnitCode).HasColumnName("MeasuringUnitCode").IsRequired().HasMaxLength(255);
            Property(x => x.RowVersion).HasColumnName("RowVersion").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasRequired(a => a.Vessel_FuelReportCommandLog).WithMany(b => b.Vessel_FuelReportCommandLogDetail).HasForeignKey(c => c.FuelReportCommandLogId); // FuelReportCommandLogDetail_FuelReportCommandLogId_FuelReportCommandLog_Id
        }
    }


    // ************************************************************************
    // Stored procedure return models

    internal class Fuel_PeriodicalFuelStatisticsReturnModel
    {
        public String Type { get; set; }
        public Int64? GoodId { get; set; }
        public String GoodCode { get; set; }
        public String GoodName { get; set; }
        public String QuantityUnit { get; set; }
        public String MainCurrencyUnit { get; set; }
        public Decimal? QuantityAmount { get; set; }
        public Decimal? TotalPrice { get; set; }
        public Int64? CompanyId { get; set; }
        public Int64? WarehouseId { get; set; }
        public String Status { get; set; }
        public Int32? TypeId { get; set; }
        public Int32? TypeDisplayOrder { get; set; }
        public String CompanyName { get; set; }
        public String WarehouseName { get; set; }
    }

    internal class Inventory_CardexReturnModel
    {
        public DateTime? RegistrationDate { get; set; }
        public String Description { get; set; }
        public Decimal? Code { get; set; }
        public Byte? Action { get; set; }
        public Int16? SignAction { get; set; }
        public Int64? QuantityUnitId { get; set; }
        public String QuantityUnitAbbreviation { get; set; }
        public String QuantityUnitName { get; set; }
        public Int64? PriceUnitId { get; set; }
        public String PriceUnitAbbreviation { get; set; }
        public String PriceUnitName { get; set; }
        public Int64? MainCurrencyUnitId { get; set; }
        public String MainCurrencyAbbreviation { get; set; }
        public String MainCurrencyName { get; set; }
        public Decimal? QuantityAmount { get; set; }
        public Decimal? QuantityAmountPriced { get; set; }
        public Decimal? FeeInMainCurrency { get; set; }
        public Decimal? TotalPrice { get; set; }
    }

    internal class Inventory_ChangeWarehouseStatusReturnModel
    {
        public String Column1 { get; set; }
    }

    internal class Inventory_IsValidTransactionCodeReturnModel
    {
        public Boolean? Column1 { get; set; }
    }

    internal class Inventory_TransactionItemPricesGetAllReturnModel
    {
        public Int32 tipId { get; set; }
        public Int16 tipRowVersion { get; set; }
        public String tipDescription { get; set; }
        public Int64 tipQuantityUnitId { get; set; }
        public Decimal? tipQuantityAmount { get; set; }
        public Int64 tipPriceUnitId { get; set; }
        public Decimal? tipFee { get; set; }
        public Int64 tipMainCurrencyUnitId { get; set; }
        public Decimal? tipFeeInMainCurrency { get; set; }
        public DateTime? tipRegistrationDate { get; set; }
        public Decimal? tipQuantityAmountUseFIFO { get; set; }
        public Int32? tipTransactionReferenceId { get; set; }
        public String tipIssueReferenceIds { get; set; }
        public Int32? tipUserCreatorId { get; set; }
        public DateTime? tipCreateDate { get; set; }
        public Int32 tiId { get; set; }
        public Int16 tiRowVersion { get; set; }
        public Int64 tiGoodId { get; set; }
        public Int64 tiQuantityUnitId { get; set; }
        public Decimal? tiQuantityAmount { get; set; }
        public String tiDescription { get; set; }
        public Int32? tiUserCreatorId { get; set; }
        public DateTime? tiCreateDate { get; set; }
        public Int32 tId { get; set; }
        public Byte tAction { get; set; }
        public Decimal? tCode { get; set; }
        public String tDescription { get; set; }
        public Int32? tPricingReferenceId { get; set; }
        public Int64 tWarehouseId { get; set; }
        public Int32 tStoreTypesId { get; set; }
        public Int32 tTimeBucketId { get; set; }
        public Byte? tStatus { get; set; }
        public DateTime? tRegistrationDate { get; set; }
        public Int32? tSenderReciver { get; set; }
        public String tHardCopyNo { get; set; }
        public String tReferenceType { get; set; }
        public String tReferenceNo { get; set; }
        public DateTime? tReferenceDate { get; set; }
        public Int32? tUserCreatorId { get; set; }
        public DateTime? tCreateDate { get; set; }
        public Int16 StoreTypeCode { get; set; }
        public String StoreTypeInputName { get; set; }
        public String StoreTypeOutputName { get; set; }
        public String WarehouseCode { get; set; }
        public String WarehouseName { get; set; }
        public Int64 CompanyId { get; set; }
        public Boolean? WarehouseStatus { get; set; }
        public String CompanyCode { get; set; }
        public String CompanyName { get; set; }
        public Boolean? CompanyStatus { get; set; }
        public String GoodCode { get; set; }
        public String GoodName { get; set; }
        public Boolean GoodStatus { get; set; }
        public String QuantityUnitAbbreviation { get; set; }
        public String QuantityUnitName { get; set; }
        public Boolean? QuantityUnitStatus { get; set; }
        public String PriceUnitAbbreviation { get; set; }
        public String PriceUnitName { get; set; }
        public Boolean? PriceUnitStatus { get; set; }
        public String MainCurrencyAbbreviation { get; set; }
        public String MainCurrencyName { get; set; }
        public Boolean? BaseUnitStatus { get; set; }
    }

    internal class Inventory_TransactionItemsGetAllReturnModel
    {
        public Int32 tiId { get; set; }
        public Int16 tiRowVersion { get; set; }
        public Int64 tiGoodId { get; set; }
        public Int64 tiQuantityUnitId { get; set; }
        public Decimal? tiQuantityAmount { get; set; }
        public String tiDescription { get; set; }
        public Int32? tiUserCreatorId { get; set; }
        public DateTime? tiCreateDate { get; set; }
        public Int32 tId { get; set; }
        public Byte tAction { get; set; }
        public Decimal? tCode { get; set; }
        public String tDescription { get; set; }
        public Int32? tPricingReferenceId { get; set; }
        public Int64 tWarehouseId { get; set; }
        public Int32 tStoreTypesId { get; set; }
        public Int32 tTimeBucketId { get; set; }
        public Byte? tStatus { get; set; }
        public DateTime? tRegistrationDate { get; set; }
        public Int32? tSenderReciver { get; set; }
        public String tHardCopyNo { get; set; }
        public String tReferenceType { get; set; }
        public String tReferenceNo { get; set; }
        public DateTime? tReferenceDate { get; set; }
        public Int32? tUserCreatorId { get; set; }
        public DateTime? tCreateDate { get; set; }
        public Int16 StoreTypeCode { get; set; }
        public String StoreTypeInputName { get; set; }
        public String StoreTypeOutputName { get; set; }
        public String WarehouseCode { get; set; }
        public String WarehouseName { get; set; }
        public Int64 CompanyId { get; set; }
        public Boolean? WarehouseStatus { get; set; }
        public String CompanyCode { get; set; }
        public String CompanyName { get; set; }
        public Boolean? CompanyStatus { get; set; }
        public String GoodCode { get; set; }
        public String GoodName { get; set; }
        public Boolean GoodStatus { get; set; }
        public String UnitAbbreviation { get; set; }
        public String UnitName { get; set; }
        public Boolean? UnitIsCurrency { get; set; }
        public Boolean? UnitIsBaseCurrency { get; set; }
        public Boolean UnitStatus { get; set; }
    }

    internal class Inventory_TransactionsGetAllReturnModel
    {
        public Int32 Id { get; set; }
        public Byte Action { get; set; }
        public Decimal? Code { get; set; }
        public String Description { get; set; }
        public Int64 WarehouseId { get; set; }
        public Int32 StoreTypesId { get; set; }
        public Int32 TimeBucketId { get; set; }
        public Byte? Status { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public Int32? SenderReciver { get; set; }
        public String HardCopyNo { get; set; }
        public String ReferenceType { get; set; }
        public String ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public Int32? UserCreatorId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Int16 StoreTypeCode { get; set; }
        public String StoreTypeInputName { get; set; }
        public String StoreTypeOutputName { get; set; }
        public String WarehouseCode { get; set; }
        public String WarehouseName { get; set; }
        public Int64 CompanyId { get; set; }
        public Boolean? WarehouseStatus { get; set; }
        public String CompanyCode { get; set; }
        public String CompanyName { get; set; }
        public Boolean? CompanyStatus { get; set; }
    }

    internal class Inventory_UnitConvertsOperationReturnModel
    {
        public String Column1 { get; set; }
    }

}

