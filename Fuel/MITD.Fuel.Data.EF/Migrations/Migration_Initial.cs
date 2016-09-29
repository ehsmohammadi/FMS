using System;
using System.Data;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(1)]
    public class Migration_Initial : Migration
    {
        public const string FUEL_SCHEMA = "Fuel";
        public const string BASIC_INFO_SCHEMA = "BasicInfo";
        public const string SECURITY_SCHEMA = "dbo";

        public override void Up()
        {
            if (!Schema.Schema(BASIC_INFO_SCHEMA).Exists())
            {
                Create.Schema(BASIC_INFO_SCHEMA);
            }


            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create SAPID-HAFIZ Voyages Views.sql");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create ActivityLocations Table-Data.sql");

            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\BasicInfoViews_Create.sql");

            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create SAPID-HAFIZ Rotation Linked Servers.sql");

            createInventoryBasicInfoViews();

            createActionsTable();
            createUsersTable();

            if (!Schema.Schema(FUEL_SCHEMA).Exists())
                Create.Schema(FUEL_SCHEMA);

            Create.Table("Vessel").InSchema(FUEL_SCHEMA)
                    .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                    .WithColumn("Code").AsString(50).NotNullable().Indexed()
                    .WithColumn("OwnerId").AsInt64().NotNullable().Indexed()
                    .WithColumn("RowVersion").AsCustom("RowVersion");

            Create.Table("VesselInCompany").InSchema(FUEL_SCHEMA)
                    .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                    .WithColumn("Name").AsString(200).Nullable().Indexed()
                    .WithColumn("Description").AsString(2000).Nullable().Indexed()
                    .WithColumn("CompanyId").AsInt64().NotNullable().Indexed()
                    .WithColumn("VesselId").AsInt64().NotNullable()
                        .ForeignKey("FK_VesselInCompany_VesselId_Vessel_Id", FUEL_SCHEMA, "Vessel", "Id")
                    .WithColumn("VesselStateCode").AsInt32().NotNullable()
                    .WithColumn("RowVersion").AsCustom("RowVersion");

            Create.Table("Voyage").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("VoyageNumber").AsString(200)
                  .WithColumn("Description").AsString(200)
                  .WithColumn("VesselInCompanyId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_Voyage_VesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id")
                  .WithColumn("CompanyId").AsInt64().NotNullable().Indexed()
                  .WithColumn("StartDate").AsDateTime().NotNullable()
                  .WithColumn("EndDate").AsDateTime().Nullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable();


            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\CreateVoyagesView.sql");


            Create.Table("ApproveFlowConfig").InSchema(FUEL_SCHEMA)
                    .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                    .WithColumn("ActorUserId").AsInt64().NotNullable().Indexed()
                    .WithColumn("NextWorkflowStepId").AsInt64().Nullable().Indexed()
                        .ForeignKey("FK_ApproveFlowConfig_NextWorkflowStepId_ApproveFlowConfig_Id", FUEL_SCHEMA, "ApproveFlowConfig", "Id")
                    .WithColumn("WithWorkflowAction").AsInt32().NotNullable()
                    .WithColumn("State").AsInt32().NotNullable()
                    .WithColumn("WorkflowEntity").AsInt32().NotNullable()
                    .WithColumn("CurrentWorkflowStage").AsInt32().NotNullable()
                    .WithColumn("RowVersion").AsCustom("RowVersion");



            Create.Table("Invoice").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("InvoiceDate").AsDateTime().NotNullable()
                  .WithColumn("CurrencyId").AsInt64().NotNullable().Indexed()
                  .WithColumn("State").AsInt32().NotNullable()
                  .WithColumn("Description").AsString().Nullable()
                  .WithColumn("DivisionMethod").AsInt32().NotNullable()
                  .WithColumn("InvoiceNumber").AsString().NotNullable()
                  .WithColumn("AccountingType").AsInt32().NotNullable()
                  .WithColumn("InvoiceRefrenceId").AsInt64().Nullable().Indexed()
                        .ForeignKey("FK_Invoice_InvoiceRefrenceId_Invoice_Id", FUEL_SCHEMA, "Invoice", "Id")
                  .WithColumn("InvoiceType").AsInt32().NotNullable()
                  .WithColumn("TransporterId").AsInt64().Nullable().Indexed()
                  .WithColumn("SupplierId").AsInt64().Nullable().Indexed()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("OwnerId").AsInt64().NotNullable().Indexed()
                  .WithColumn("IsCreditor").AsBoolean().NotNullable()
                  .WithColumn("TotalOfDivisionPrice").AsDecimal(18, 2).NotNullable();



            Create.Table("EffectiveFactor").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("Name").AsString(200)
                  .WithColumn("EffectiveFactorType").AsInt32().NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion");


            Create.Table("InvoiceAdditionalPrices").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("EffectiveFactorId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_InvoiceAdditionalPrices_EffectiveFactorId_EffectiveFactor_Id", FUEL_SCHEMA, "EffectiveFactor", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("Price").AsDecimal(18, 2).NotNullable()
                  .WithColumn("Description").AsString().Nullable()
                  .WithColumn("Divisionable").AsBoolean().NotNullable()
                  .WithColumn("InvoiceId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_InvoiceAdditionalPrices_InvoiceId_Invoice_Id", FUEL_SCHEMA, "Invoice", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("TimeStamp").AsCustom("RowVersion");


            Create.Table("InvoiceItems").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("Quantity").AsDecimal(18, 2).NotNullable()
                  .WithColumn("Fee").AsDecimal(18, 2).NotNullable()
                  .WithColumn("InvoiceId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_InvoiceItems_InvoiceId_Invoice_Id", FUEL_SCHEMA, "Invoice", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("GoodId").AsInt64().NotNullable().Indexed()
                  .WithColumn("MeasuringUnitId").AsInt64().NotNullable().Indexed()
                  .WithColumn("Description").AsString().Nullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("DivisionPrice").AsDecimal(18, 2).NotNullable();



            Create.Table("FuelReport").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("Code").AsString(200)
                  .WithColumn("Description").AsString(2000)
                  .WithColumn("EventDate").AsDateTime().NotNullable()
                  .WithColumn("ReportDate").AsDateTime().NotNullable()
                  .WithColumn("VesselInCompanyId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_FuelReport_VesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id")
                  .WithColumn("VoyageId").AsInt64().Nullable().Indexed()
                //.ForeignKey("FK_FuelReport_VoyageId_Voyage_Id", FUEL_SCHEMA, "Voyage", "Id")
                  .WithColumn("FuelReportType").AsInt32().NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("State").AsInt32().NotNullable();



            Create.Table("FuelReportDetail").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("FuelReportId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_FuelReportDetail_FuelReportId_FuelReport_Id", FUEL_SCHEMA, "FuelReport", "Id")
                  .WithColumn("GoodId").AsInt64().NotNullable().Indexed()
                  .WithColumn("TankId").AsInt64().Nullable().Indexed()
                  .WithColumn("Consumption").AsDouble().NotNullable()
                  .WithColumn("ROB").AsDouble().NotNullable()
                  .WithColumn("ROBUOM").AsString(50).NotNullable()
                  .WithColumn("Receive").AsDouble().Nullable()
                  .WithColumn("ReceiveType").AsInt32().Nullable()
                  .WithColumn("ReceiveReference_ReferenceType").AsInt32().Nullable()
                  .WithColumn("ReceiveReference_ReferenceId").AsInt64().Nullable()
                  .WithColumn("ReceiveReference_Code").AsString().Nullable()
                  .WithColumn("Transfer").AsDouble().Nullable()
                  .WithColumn("TransferType").AsInt32().Nullable()
                  .WithColumn("TransferReference_ReferenceType").AsInt32().Nullable()
                  .WithColumn("TransferReference_ReferenceId").AsInt64().Nullable()
                  .WithColumn("TransferReference_Code").AsString().Nullable()
                  .WithColumn("Correction").AsDouble().Nullable()
                  .WithColumn("CorrectionPrice").AsDecimal(18, 2).Nullable()
                  .WithColumn("CorrectionType").AsInt32().Nullable()
                  .WithColumn("CorrectionReference_ReferenceType").AsInt32().Nullable()
                  .WithColumn("CorrectionReference_ReferenceId").AsInt64().Nullable()
                  .WithColumn("CorrectionReference_Code").AsString().Nullable()
                  .WithColumn("CorrectionPriceCurrencyId").AsInt64().Nullable().Indexed()
                  .WithColumn("CorrectionPriceCurrencyISOCode").AsString(20).Nullable()
                  .WithColumn("MeasuringUnitId").AsInt64().NotNullable().Indexed()
                  .WithColumn("TimeStamp").AsCustom("RowVersion");



            Create.Table("Order").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("Code").AsString()
                  .WithColumn("Description").AsString().Nullable()
                  .WithColumn("SupplierId").AsInt64().Nullable().Indexed()
                  .WithColumn("ReceiverId").AsInt64().Nullable().Indexed()
                  .WithColumn("TransporterId").AsInt64().Nullable().Indexed()
                  .WithColumn("OwnerId").AsInt64().NotNullable().Indexed()
                  .WithColumn("OrderType").AsInt32().NotNullable()
                  .WithColumn("OrderDate").AsDateTime().NotNullable()
                  .WithColumn("FromVesselInCompanyId").AsInt64().Nullable().Indexed()
                        .ForeignKey("FK_Order_FromVesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id")
                  .WithColumn("ToVesselInCompanyId").AsInt64().Nullable().Indexed()
                        .ForeignKey("FK_Order_ToVesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id")
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("State").AsInt32().NotNullable();


            Create.Table("OrderItems").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("Description").AsString().Nullable()
                  .WithColumn("Quantity").AsDecimal(18, 2).NotNullable()
                  .WithColumn("OrderId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_OrderItems_OrderId_Order_Id", FUEL_SCHEMA, "Order", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("GoodId").AsInt64().NotNullable().Indexed()
                  .WithColumn("MeasuringUnitId").AsInt64().NotNullable().Indexed()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("InvoicedInMainUnit").AsDecimal(18, 2).NotNullable()
                  .WithColumn("ReceivedInMainUnit").AsDecimal(18, 2).NotNullable();



            Create.Table("OrderItemBalances").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("OrderId").AsInt64().NotNullable()
                  .WithColumn("OrderItemId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_OrderItemBalances_OrderItemId_OrderItems_Id", FUEL_SCHEMA, "OrderItems", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("QuantityAmountInMainUnit").AsDecimal(18, 2).NotNullable()
                  .WithColumn("UnitCode").AsString(50).NotNullable()
                  .WithColumn("FuelReportDetailId").AsInt64().Indexed().NotNullable()
                        .ForeignKey("FK_OrderItemBalances_FuelReportDetailId_FuelReportDetail_Id", FUEL_SCHEMA, "FuelReportDetail", "Id")
                  .WithColumn("InvoiceItemId").AsInt64().Indexed().NotNullable()
                        .ForeignKey("FK_OrderItemBalances_InvoiceItemId_InvoiceItems_Id", FUEL_SCHEMA, "InvoiceItems", "Id")
                  .WithColumn("TimeStamp").AsCustom("RowVersion");


            Create.Table("Charter").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("CurrentState").AsInt32().NotNullable()
                  .WithColumn("ActionDate").AsDateTime().NotNullable()
                  .WithColumn("CharterType").AsInt32().NotNullable()
                  .WithColumn("CharterEndType").AsInt32().NotNullable()
                  .WithColumn("ChartererId").AsInt64().Indexed()
                  .WithColumn("OwnerId").AsInt64().Indexed()
                  .WithColumn("VesselInCompanyId").AsInt64().Indexed()
                        .ForeignKey("FK_Charter_VesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id")
                  .WithColumn("CurrencyId").AsInt64().Indexed()
                  .WithColumn("TimeStamp").AsCustom("RowVersion");


            Create.Table("CharterItem").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("CharterId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_CharterItem_CharterId_Charter_Id", FUEL_SCHEMA, "Charter", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("GoodUnitId").AsInt64().NotNullable().Indexed()
                  .WithColumn("GoodId").AsInt64().NotNullable().Indexed()
                  .WithColumn("TankId").AsInt64().Nullable().Indexed()
                  .WithColumn("Rob").AsDecimal(18, 2).NotNullable()
                  .WithColumn("Fee").AsDecimal(18, 2).NotNullable()
                  .WithColumn("OffhireFee").AsDecimal(18, 2).NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion");



            Create.Table("Offhire").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("ReferenceNumber").AsInt64().NotNullable()
                  .WithColumn("StartDateTime").AsDateTime().NotNullable()
                  .WithColumn("EndDateTime").AsDateTime().NotNullable()
                  .WithColumn("IntroducerType").AsInt32().NotNullable()
                  .WithColumn("VoucherDate").AsDateTime().NotNullable()
                  .WithColumn("VoucherCurrencyId").AsInt64().NotNullable()
                  .WithColumn("PricingReference_Number").AsString()
                  .WithColumn("PricingReference_Type").AsInt32().NotNullable()
                  .WithColumn("State").AsInt32().NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("IntroducerId").AsInt64().NotNullable().Indexed()
                  .WithColumn("OffhireLocationId").AsInt64().NotNullable().Indexed()
                  .WithColumn("VesselInCompanyId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_Offhire_VesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id")
                  .WithColumn("VoyageId").AsInt64().Nullable().Indexed();
            //.ForeignKey("FK_Offhire_Voyage_Id_Voyage_Id", FUEL_SCHEMA, "Voyage", "Id");



            Create.Table("OffhireDetail").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("Quantity").AsDecimal(18, 2).NotNullable()
                  .WithColumn("FeeInVoucherCurrency").AsDecimal(18, 2).NotNullable()
                  .WithColumn("FeeInMainCurrency").AsDecimal(18, 2).NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("GoodId").AsInt64().NotNullable().Indexed()
                  .WithColumn("OffhireId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_OffhireDetail_Offhire_Id_Offhire_Id", FUEL_SCHEMA, "Offhire", "Id")
                  .WithColumn("TankId").AsInt64().Nullable().Indexed()
                  .WithColumn("UnitId").AsInt64().NotNullable().Indexed();



            Create.Table("Scrap").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("ScrapDate").AsDateTime().NotNullable()
                  .WithColumn("State").AsInt32().NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("SecondPartyId").AsInt64().NotNullable().Indexed()
                  .WithColumn("VesselInCompanyId").AsInt64().NotNullable().Indexed()
                      .ForeignKey("FK_Scrap_VesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id");


            Create.Table("ScrapDetail").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("ROB").AsDouble().NotNullable()
                  .WithColumn("Price").AsDouble().NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("CurrencyId").AsInt64().NotNullable().Indexed()
                  .WithColumn("GoodId").AsInt64().NotNullable().Indexed()
                  .WithColumn("TankId").AsInt64().Nullable().Indexed()
                  .WithColumn("UnitId").AsInt64().NotNullable().Indexed()
                  .WithColumn("ScrapId").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_ScrapDetail_Scrap_Id_Scrap_Id", FUEL_SCHEMA, "Scrap", "Id").OnDeleteOrUpdate(Rule.Cascade);


            Create.Table("VoyageLog").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("ReferencedVoyageId").AsInt64().NotNullable().Indexed()
                //.ForeignKey("FK_VoyageLog_ReferencedVoyageId_Voyage_Id", FUEL_SCHEMA, "Voyage", "Id")
                  .WithColumn("ChangeDate").AsDateTime().NotNullable()
                  .WithColumn("VoyageNumber").AsString(200)
                  .WithColumn("Description").AsString(200)
                  .WithColumn("StartDate").AsDateTime().NotNullable()
                  .WithColumn("EndDate").AsDateTime().NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable()
                  .WithColumn("CompanyId").AsInt64().NotNullable().Indexed()
                  .WithColumn("VesselInCompanyId").AsInt64().NotNullable().Indexed()
                      .ForeignKey("FK_VoyageLog_VesselInCompanyId_VesselInCompany_Id", FUEL_SCHEMA, "VesselInCompany", "Id");



            Create.Table("InvoiceOrders").InSchema(FUEL_SCHEMA)
                  .WithColumn("Invoice_Id").AsInt64().NotNullable().PrimaryKey().Indexed()
                        .ForeignKey("FK_InvoiceOrders_Invoice_Id_Invoice_Id", FUEL_SCHEMA, "Invoice", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("Order_Id").AsInt64().NotNullable().PrimaryKey().Indexed()
                        .ForeignKey("FK_InvoiceOrders_Order_Id_Order_Id", FUEL_SCHEMA, "Order", "Id").OnDeleteOrUpdate(Rule.Cascade);


            Create.Table("CharterIn").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_CharterIn_Id_Charter_Id", FUEL_SCHEMA, "Charter", "Id")
                  .WithColumn("OffHirePricingType").AsInt32().NotNullable();



            Create.Table("CharterOut").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Indexed()
                        .ForeignKey("FK_CharterOut_Id_Charter_Id", FUEL_SCHEMA, "Charter", "Id")
                  .WithColumn("OffHirePricingType").AsInt32().NotNullable();

            Create.Table("InventoryOperation").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("ActionNumber").AsString(200)
                  .WithColumn("ActionDate").AsDateTime().NotNullable()
                  .WithColumn("ActionType").AsInt32().NotNullable()
                  .WithColumn("TimeStamp").AsCustom("RowVersion")
                  .WithColumn("FuelReportDetailId").AsInt64().Indexed().Nullable()
                        .ForeignKey("FK_InventoryOperation_FuelReportDetailId_FuelReportDetail_Id", FUEL_SCHEMA, "FuelReportDetail", "Id")
                  .WithColumn("CharterId").AsInt64().Indexed().Nullable()
                        .ForeignKey("FK_InventoryOperation_CharterId_Charter_Id", FUEL_SCHEMA, "Charter", "Id")
                  .WithColumn("Scrap_Id").AsInt64().Indexed().Nullable()
                        .ForeignKey("FK_InventoryOperation_Scrap_Id_Scrap_Id", FUEL_SCHEMA, "Scrap", "Id");


            Create.Table("WorkflowLog").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                  .WithColumn("WorkflowEntity").AsInt32().NotNullable()
                  .WithColumn("ActionDate").AsDateTime().NotNullable()
                  .WithColumn("WorkflowAction").AsInt32().Nullable()
                  .WithColumn("ActorUserId").AsInt64().NotNullable().Indexed()
                  .WithColumn("Remark").AsString().Nullable()
                  .WithColumn("Active").AsBoolean().NotNullable()
                  .WithColumn("CurrentWorkflowStepId").AsInt64().NotNullable().Indexed()
                    .ForeignKey("FK_WorkflowLog_CurrentWorkflowStepId_ApproveFlowConfig_Id", FUEL_SCHEMA, "ApproveFlowConfig", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("InvoiceId").AsInt64().Indexed().Nullable()
                    .ForeignKey("FK_WorkflowLog_InvoiceId_Invoice_Id", FUEL_SCHEMA, "Invoice", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("OrderId").AsInt64().Indexed().Nullable()
                    .ForeignKey("FK_WorkflowLog_OrderId_Order_Id", FUEL_SCHEMA, "Order", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("CharterId").AsInt64().Indexed().Nullable()
                    .ForeignKey("FK_WorkflowLog_CharterId_Charter_Id", FUEL_SCHEMA, "Charter", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("FuelReportId").AsInt64().Indexed().Nullable()
                    .ForeignKey("FK_WorkflowLog_FuelReportId_FuelReport_Id", FUEL_SCHEMA, "FuelReport", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("OffhireId").AsInt64().Indexed().Nullable()
                    .ForeignKey("FK_WorkflowLog_OffhireId_Offhire_Id", FUEL_SCHEMA, "Offhire", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("ScrapId").AsInt64().Indexed().Nullable()
                    .ForeignKey("FK_WorkflowLog_ScrapId_Scrap_Id", FUEL_SCHEMA, "Scrap", "Id").OnDeleteOrUpdate(Rule.Cascade)
                  .WithColumn("Discriminator").AsString(128).NotNullable()
                  .WithColumn("RowVersion").AsCustom("RowVersion");

            CreateLog();

        }

        private void createInventoryBasicInfoViews()
        {
            Execute.Sql(@"IF OBJECT_ID ( '[BasicInfo].[UnitView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[UnitView];
	GO
CREATE VIEW [BasicInfo].[UnitView]
AS
	SELECT u.Id, u.Abbreviation, u.Name
	FROM [Inventory].Units u
	WHERE u.IsCurrency=0 AND u.IsActive=1 
GO	
-----------------------------------------------------------------------------------
IF OBJECT_ID ( '[BasicInfo].[CurrencyView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[CurrencyView];
	GO
CREATE VIEW [BasicInfo].[CurrencyView]
AS
	SELECT u.Id, u.Abbreviation, u.Name
	FROM [Inventory].Units u
	WHERE u.IsCurrency=1 AND u.IsActive=1 
GO	
-----------------------------------------------------------------------------------
IF OBJECT_ID ( '[BasicInfo].[SharedGoodView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[SharedGoodView];
	GO
CREATE VIEW [BasicInfo].[SharedGoodView]
AS
	SELECT g.Id, g.Code, g.Name, g.MainUnitId,u.Abbreviation AS MainUnitCode
	FROM [Inventory].Goods g
	INNER JOIN [Inventory].Units u ON u.Id = g.MainUnitId 
GO	
-----------------------------------------------------------------------------------
IF OBJECT_ID ( '[BasicInfo].[CompanyView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[CompanyView];
	GO
CREATE VIEW [BasicInfo].[CompanyView]
AS
	SELECT c.Id, c.Code, c.Name
	FROM [Inventory].Companies c
GO	
-----------------------------------------------------------------------------------
IF OBJECT_ID ( '[BasicInfo].[CompanyVesselView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[CompanyVesselView];
	GO
CREATE VIEW [BasicInfo].[CompanyVesselView]
AS
	SELECT w.Id, w.Code, w.Name, w.CompanyId, w.IsActive
	FROM [Inventory].Warehouse w
GO	
-----------------------------------------------------------------------------------
IF OBJECT_ID ( '[BasicInfo].[CompanyVesselTankView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[CompanyVesselTankView];
	GO
CREATE VIEW [BasicInfo].[CompanyVesselTankView]
AS
	SELECT (w.Id * 1000 + 1) AS Id,
       w.Id AS VesselInInventoryId,
       '---' AS Name,
       '---' AS Description
	FROM [Inventory].Warehouse w
GO	
-----------------------------------------------------------------------------------
IF OBJECT_ID ( '[BasicInfo].[CompanyGoodView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[CompanyGoodView];
	GO
CREATE VIEW [BasicInfo].[CompanyGoodView]
AS
	SELECT c.Id * 1000 + g.Id AS Id,
	       g.Id AS SharedGoodId,
	       c.Id AS CompanyId,
	       g.Name,
	       g.Code
	FROM [Inventory].Companies c
	CROSS JOIN [Inventory].Goods g
GO	
-----------------------------------------------------------------------------------
IF OBJECT_ID ( '[BasicInfo].[CompanyGoodUnitView]', 'view' ) IS NOT NULL 
		DROP VIEW [BasicInfo].[CompanyGoodUnitView];
	GO
CREATE VIEW [BasicInfo].[CompanyGoodUnitView]
AS
	SELECT cgv.Id * 100 + uv.Id AS Id,
	       cgv.Id AS CompanyGoodId,
	       cgv.SharedGoodId,
	       cgv.CompanyId,
	       uv.Name,
	       uv.Abbreviation,
	       ISNULL(toU.Abbreviation,uv.Abbreviation) AS [To],
	       CAST(ISNULL(uc.Coefficient,1) AS DECIMAL(19,5)) AS Coefficient,
	       CAST(0 AS DECIMAL(19,5)) AS Offset,
		   CAST(NULL AS BIGINT) AS ParentId
	FROM BasicInfo.CompanyGoodView cgv
		CROSS JOIN BasicInfo.UnitView uv
		LEFT JOIN [Inventory].UnitConverts uc ON uv.Id = uc.UnitId 
		LEFT JOIN [Inventory].Units toU ON uc.SubUnitId = toU.Id
GO	


");
        }

        public override void Down()
        {
            Delete.Table("WorkflowLog").InSchema(FUEL_SCHEMA);
            Delete.Table("InventoryOperation").InSchema(FUEL_SCHEMA);
            Delete.Table("CharterOut").InSchema(FUEL_SCHEMA);
            Delete.Table("CharterIn").InSchema(FUEL_SCHEMA);
            Delete.Table("InvoiceOrders").InSchema(FUEL_SCHEMA);
            Delete.Table("VoyageLog").InSchema(FUEL_SCHEMA);
            Delete.Table("ScrapDetail").InSchema(FUEL_SCHEMA);
            Delete.Table("Scrap").InSchema(FUEL_SCHEMA);
            Delete.Table("OffhireDetail").InSchema(FUEL_SCHEMA);
            Delete.Table("Offhire").InSchema(FUEL_SCHEMA);
            Delete.Table("CharterItem").InSchema(FUEL_SCHEMA);
            Delete.Table("Charter").InSchema(FUEL_SCHEMA);
            Delete.Table("OrderItemBalances").InSchema(FUEL_SCHEMA);
            Delete.Table("OrderItems").InSchema(FUEL_SCHEMA);
            Delete.Table("Order").InSchema(FUEL_SCHEMA);
            Delete.Table("FuelReportDetail").InSchema(FUEL_SCHEMA);
            Delete.Table("FuelReport").InSchema(FUEL_SCHEMA);
            Delete.Table("InvoiceItems").InSchema(FUEL_SCHEMA);
            Delete.Table("InvoiceAdditionalPrices").InSchema(FUEL_SCHEMA);
            Delete.Table("EffectiveFactor").InSchema(FUEL_SCHEMA);
            Delete.Table("Invoice").InSchema(FUEL_SCHEMA);
            Delete.Table("ApproveFlowConfig").InSchema(FUEL_SCHEMA);
            Delete.Table("ExceptionLogs").InSchema(FUEL_SCHEMA);
            Delete.Table("EventLogs").InSchema(FUEL_SCHEMA);

            Delete.Table("Logs").InSchema(FUEL_SCHEMA);

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\DropVoyagesView.sql");
            Delete.Table("Voyage").InSchema(FUEL_SCHEMA);
            Delete.Table("VesselInCompany").InSchema(FUEL_SCHEMA);
            Delete.Table("Vessel").InSchema(FUEL_SCHEMA);

            Delete.Schema(FUEL_SCHEMA);

            Delete.Table("Users_Groups").InSchema(SECURITY_SCHEMA);
            Delete.Table("Parties_CustomActions").InSchema(SECURITY_SCHEMA);
            Delete.Table("Users").InSchema(SECURITY_SCHEMA);
            Delete.Table("Groups").InSchema(SECURITY_SCHEMA);
            Delete.Table("Parties").InSchema(SECURITY_SCHEMA);
            Delete.Table("ActionTypes").InSchema(SECURITY_SCHEMA);


            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop Inventory BasicInfo Views.sql");

            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop SAPID-HAFIZ Rotation Linked Servers.sql");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\BasicInfoViews_Drop.sql");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop ActivityLocations Table-Data.sql");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop SAPID-HAFIZ Voyages Views.sql");
        }

        private void createUsersTable()
        {
            Create.Table("Parties").InSchema(SECURITY_SCHEMA)
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("PartyName").AsString(100).NotNullable().Unique()
                .WithColumn("RowVersion").AsCustom("RowVersion");

            Create.Table("Users").InSchema(SECURITY_SCHEMA)
                .WithColumn("Id").AsInt64().PrimaryKey()
                    .ForeignKey("Parties", "Id")
                .WithColumn("FirstName").AsString(100).Nullable()
                .WithColumn("UserName").AsString(100).Nullable()
                .WithColumn("LastName").AsString(100).Nullable()
                .WithColumn("Email").AsString(100).Nullable()
                .WithColumn("Active").AsBoolean().NotNullable()
                .WithColumn("CompanyId").AsInt64()
                ;

            Create.Table("Groups").InSchema(SECURITY_SCHEMA)
                .WithColumn("Id").AsInt64().PrimaryKey()
                    .ForeignKey("Parties", "Id")
                .WithColumn("Description").AsString(256).Nullable();

            Create.Table("Users_Groups").InSchema(SECURITY_SCHEMA)
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("UserId").AsInt64().NotNullable()
                    .ForeignKey("Users", "Id")
                .WithColumn("GroupId").AsInt64().NotNullable()
                    .ForeignKey("Groups", "Id");

            //Create.Table("Users_WorkListUsers")
            //    .WithColumn("Id").AsInt64().Identity().PrimaryKey()
            //    .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            //    .WithColumn("WorkListUserId").AsInt64().NotNullable().ForeignKey("Users", "Id");

            Create.Table("Parties_CustomActions").InSchema(SECURITY_SCHEMA)
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("PartyId").AsInt64().NotNullable()
                    .ForeignKey("Parties", "Id")
                .WithColumn("ActionTypeId").AsInt32().NotNullable()
                    .ForeignKey("ActionTypes", "Id")
                .WithColumn("IsGranted").AsBoolean().NotNullable();
        }

        private void createActionsTable()
        {
            Create.Table("ActionTypes").InSchema(SECURITY_SCHEMA)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(512).Unique().NotNullable()
                .WithColumn("Description").AsString(2000).Unique().NotNullable();
        }

        private void CreateLog()
        {

            Create.Table("Logs").InSchema(FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                //.WithColumn("Guid").AsGuid().Unique()
                  .WithColumn("RowVersion").AsCustom("rowversion")
                  .WithColumn("Code").AsString(100).NotNullable()
                  .WithColumn("LogLevelId").AsInt32().NotNullable()
                  .WithColumn("PartyId").AsInt64()
                  .WithColumn("UserName").AsString(100).Nullable()
                  .WithColumn("ClassName").AsString(200).Nullable()
                  .WithColumn("MethodName").AsString(200).Nullable()
                  .WithColumn("LogDate").AsDateTime().NotNullable()
                  .WithColumn("Title").AsString(200).NotNullable()
                  .WithColumn("Messages").AsString(4000).Nullable();


            Create.Table("ExceptionLogs").InSchema(FUEL_SCHEMA)
                .WithColumn("Id").AsInt64().NotNullable().Indexed()
                .ForeignKey("FK_ExceptionLogs_Id_Logs_Id", FUEL_SCHEMA, "Logs", "Id");

            Create.Table("EventLogs").InSchema(FUEL_SCHEMA)
                .WithColumn("Id").AsInt64().NotNullable().Indexed()
                .ForeignKey("FK_EventLogs_Id_Logs_Id", FUEL_SCHEMA, "Logs", "Id");


        }
    }
}
