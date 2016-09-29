using System.Data;
using System.Linq;
using FluentMigrator;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Migrations;
using WorkflowDataConvertor;

namespace MITD.Data.EF.Migrations
{
    [Migration(26)]
   public class Migration_V26:Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            setPartiesIdColumnAsIndentity();

            Create.Table("Workflow").InSchema(Migration_Initial.FUEL_SCHEMA)
                .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("WorkflowEntity").AsInt32().NotNullable()
                .WithColumn("CompanyId").AsInt64().NotNullable().Indexed();

            Create.Table("WorkflowStep").InSchema(Migration_Initial.FUEL_SCHEMA)
                .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                .WithColumn("WorkflowId").AsInt64().NotNullable().Indexed()
                    .ForeignKey("FK_Fuel.WorkflowStep_WorkflowId_Fuel.Workflow_Id", Migration_Initial.FUEL_SCHEMA, "Workflow", "Id").OnDeleteOrUpdate(Rule.None)
                .WithColumn("State").AsInt32().NotNullable()
                .WithColumn("CurrentWorkflowStage").AsInt32().NotNullable();

            Create.Table("ActivityFlow").InSchema(Migration_Initial.FUEL_SCHEMA)
                .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                .WithColumn("WorkflowStepId").AsInt64().NotNullable()
                    .ForeignKey("FK_Fuel.ActivityFlow_WorkflowStepId_Fuel.WorkflowStep_Id", Migration_Initial.FUEL_SCHEMA, "WorkflowStep", "Id").OnDeleteOrUpdate(Rule.None)
                .WithColumn("WorkflowNextStepId").AsInt64().NotNullable().Indexed()
                    .ForeignKey("FK_Fuel.ActivityFlow_WorkflowNextStepId_Fuel.WorkflowStep_Id", Migration_Initial.FUEL_SCHEMA, "WorkflowStep", "Id").OnDeleteOrUpdate(Rule.None)
                .WithColumn("WorkflowAction").AsInt32().NotNullable()
                .WithColumn("ActionTypeId").AsInt32().NotNullable()
                    .ForeignKey("FK_Fuel.ActivityFlow_ActionTypeId_dbo.ActionTypes_Id", "dbo", "ActionTypes", "Id").OnDeleteOrUpdate(Rule.None);

            Create.UniqueConstraint("UC_WorkflowStep_ActionType").OnTable("ActivityFlow").WithSchema(Migration_Initial.FUEL_SCHEMA).Columns(new[] { "WorkflowStepId", "ActionTypeId" });


            removeCurrentWorkflowLogRelations();

            Rename.Table("WorkflowLog").InSchema(Migration_Initial.FUEL_SCHEMA).To("WorkflowLog_Old").InSchema(Migration_Initial.FUEL_SCHEMA);

            Create.Table("WorkflowLog").InSchema(Migration_Initial.FUEL_SCHEMA)
                .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
                .WithColumn("WorkflowEntity").AsInt32().NotNullable()
                .WithColumn("ActionDate").AsDateTime().NotNullable()
                .WithColumn("WorkflowAction").AsInt32().Nullable()
                .WithColumn("ActorUserId").AsInt64().NotNullable().Indexed()
                .WithColumn("Remark").AsString().Nullable()
                .WithColumn("Active").AsBoolean().NotNullable()
                .WithColumn("CurrentWorkflowStepId").AsInt64().NotNullable().Indexed()
                  .ForeignKey("FK_WorkflowLog_CurrentWorkflowStepId_WorkflowStep_Id", Migration_Initial.FUEL_SCHEMA, "WorkflowStep", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("InvoiceId").AsInt64().Indexed().Nullable()
                  .ForeignKey("FK_WorkflowLog_InvoiceId_Invoice_Id", Migration_Initial.FUEL_SCHEMA, "Invoice", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("OrderId").AsInt64().Indexed().Nullable()
                  .ForeignKey("FK_WorkflowLog_OrderId_Order_Id", Migration_Initial.FUEL_SCHEMA, "Order", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("CharterId").AsInt64().Indexed().Nullable()
                  .ForeignKey("FK_WorkflowLog_CharterId_Charter_Id", Migration_Initial.FUEL_SCHEMA, "Charter", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("FuelReportId").AsInt64().Indexed().Nullable()
                  .ForeignKey("FK_WorkflowLog_FuelReportId_FuelReport_Id", Migration_Initial.FUEL_SCHEMA, "FuelReport", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("OffhireId").AsInt64().Indexed().Nullable()
                  .ForeignKey("FK_WorkflowLog_OffhireId_Offhire_Id", Migration_Initial.FUEL_SCHEMA, "Offhire", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("ScrapId").AsInt64().Indexed().Nullable()
                  .ForeignKey("FK_WorkflowLog_ScrapId_Scrap_Id", Migration_Initial.FUEL_SCHEMA, "Scrap", "Id").OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("Discriminator").AsString(128).NotNullable()
                .WithColumn("RowVersion").AsCustom("RowVersion");

            createVoyageEventReportsView();

            using (DataContainer context = new DataContainer())
            {
                DataConfigurationProvider.AddOrUpdateActionTypes(context);

                foreach (var company in context.Companies.ToList())
                {
                    DataConfigurationProvider.InsertWorkflowConfigs(context, company.Id);
                }

                WFDataConvertor.ConcvertOldData(this.ConnectionString);
            }
        }

        private void createVoyageEventReportsView()
        {
            this.Execute.Sql(@"
CREATE SCHEMA [Report] AUTHORIZATION [dbo]
GO
GRANT EXECUTE ON SCHEMA::[Report] TO [public]
GO
GRANT SELECT ON SCHEMA::[Report] TO [public]
GO

/****** Object:  View [dbo].[VesselEventReportsView]    Script Date: 6/14/2015 12:15:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [Report].[VesselEventReportsView]
AS
SELECT   er.ID, er.DraftID, RIGHT('0000' + er.[ShipCode], 4) AS ShipCode, er.ConsNo, er.ShipName, er.VoyageNo, er.Year, er.Month, er.Day, er.PortName, er.PortTime, er.AtSeaLatitudeDegree, er.AtSeaLatitudeMinute, 
                er.AtSeaLongitudeDegree, er.AtSeaLongitudeMinute, er.ObsDist, er.EngDist, er.SteamTime, er.AvObsSpeed, er.AvEngSpeed, er.RPM, er.Slip, er.WindDir, er.WindForce, 
                er.SeaDir, er.SeaForce, er.ROBHO, er.ROBDO, er.ROBMGO, er.ROBFW, er.ConsInPortHO, er.ConsInPortDO, er.ConsInPortMGO, er.ConsInPortFW, er.ConsAtSeaHO, 
                er.ConsAtSeaDO, er.ConsAtSeaMGO, er.ConsAtSeaFW, er.ReceivedHO, er.ReceivedDO, er.ReceivedMGO, er.ReceivedFW, er.ETAPort, er.ETADate, er.Date, er.DateIn, 
                er.DailyFuelCons, er.Speed, er.IsSM, er.InPortOrAtSea, er.ImportDate, er.TransferHo, er.TransferDo, er.TransferFW, er.TransferMGOLS, er.CorrectionHo, er.CorrectionDo, 
                er.CorrectionFW, er.CorrectionMGOLS, er.CorrectionTypeHo, er.CorrectionTypeDo, er.CorrectionTypeFW, er.CorrectionTypeMGOLS, er.Time, er.FuelReportType, er.State, 
                ert.Name AS ReportTypeName, lt.Name AS LocationTypeName
FROM      EventReport.dbo.EventReport AS er LEFT OUTER JOIN
                EventReport.dbo.EventReportTypes AS ert ON er.FuelReportType = ert.Type LEFT OUTER JOIN
                EventReport.dbo.LocationTypes AS lt ON er.InPortOrAtSea = lt.Type

GO

ALTER AUTHORIZATION ON [Report].[VesselEventReportsView] TO  SCHEMA OWNER 
GO
GRANT SELECT ON [Report].[VesselEventReportsView] TO [public] AS [dbo]
");
        }

        private void removeCurrentWorkflowLogRelations()
        {
            Execute.Sql(@"
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON


GO
ALTER TABLE Fuel.WorkflowLog
	DROP CONSTRAINT FK_WorkflowLog_ScrapId_Scrap_Id
GO
ALTER TABLE Fuel.Scrap SET (LOCK_ESCALATION = TABLE)
GO


GO
ALTER TABLE Fuel.WorkflowLog
	DROP CONSTRAINT FK_WorkflowLog_OffhireId_Offhire_Id
GO
ALTER TABLE Fuel.Offhire SET (LOCK_ESCALATION = TABLE)
GO

GO
ALTER TABLE Fuel.WorkflowLog
	DROP CONSTRAINT FK_WorkflowLog_FuelReportId_FuelReport_Id
GO
ALTER TABLE Fuel.FuelReport SET (LOCK_ESCALATION = TABLE)
GO

GO
ALTER TABLE Fuel.WorkflowLog
	DROP CONSTRAINT FK_WorkflowLog_CharterId_Charter_Id
GO
ALTER TABLE Fuel.Charter SET (LOCK_ESCALATION = TABLE)
GO
GO
ALTER TABLE Fuel.WorkflowLog
	DROP CONSTRAINT FK_WorkflowLog_OrderId_Order_Id
GO
ALTER TABLE Fuel.[Order] SET (LOCK_ESCALATION = TABLE)
GO

GO
ALTER TABLE Fuel.WorkflowLog
	DROP CONSTRAINT FK_WorkflowLog_InvoiceId_Invoice_Id
GO
ALTER TABLE Fuel.Invoice SET (LOCK_ESCALATION = TABLE)

GO
ALTER TABLE Fuel.WorkflowLog
	DROP CONSTRAINT FK_WorkflowLog_CurrentWorkflowStepId_ApproveFlowConfig_Id
GO
ALTER TABLE Fuel.ApproveFlowConfig SET (LOCK_ESCALATION = TABLE)
GO

GO
DROP INDEX IX_WorkflowLog_ScrapId ON Fuel.WorkflowLog
GO
DROP INDEX IX_WorkflowLog_OffhireId ON Fuel.WorkflowLog
GO
DROP INDEX IX_WorkflowLog_FuelReportId ON Fuel.WorkflowLog
GO
DROP INDEX IX_WorkflowLog_CharterId ON Fuel.WorkflowLog
GO
DROP INDEX IX_WorkflowLog_OrderId ON Fuel.WorkflowLog
GO
DROP INDEX IX_WorkflowLog_InvoiceId ON Fuel.WorkflowLog
GO
DROP INDEX IX_WorkflowLog_CurrentWorkflowStepId ON Fuel.WorkflowLog
GO
DROP INDEX IX_WorkflowLog_ActorUserId ON Fuel.WorkflowLog
GO

ALTER TABLE Fuel.WorkflowLog DROP CONSTRAINT PK_WorkflowLog
GO
ALTER TABLE Fuel.WorkflowLog SET (LOCK_ESCALATION = TABLE)
GO

COMMIT
");
        }

        private void setPartiesIdColumnAsIndentity()
        {
            //Following statement didn't work and the change script generated by SQL.
            //Alter.Table("Parties").InSchema(Migration_Initial.SECURITY_SCHEMA).AlterColumn("Id").AsInt64().Identity().PrimaryKey();

            this.Execute.Sql(@"
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Parties
	(
	Id bigint NOT NULL IDENTITY (1, 1),
	PartyName nvarchar(100) NOT NULL,
	RowVersion timestamp NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Parties SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Parties ON
GO
IF EXISTS(SELECT * FROM dbo.Parties)
	 EXEC('INSERT INTO dbo.Tmp_Parties (Id, PartyName)
		SELECT Id, PartyName FROM dbo.Parties WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Parties OFF
GO
ALTER TABLE dbo.Users
	DROP CONSTRAINT FK_Users_Id_Parties_Id
GO
ALTER TABLE dbo.Groups
	DROP CONSTRAINT FK_Groups_Id_Parties_Id
GO
ALTER TABLE dbo.Parties_CustomActions
	DROP CONSTRAINT FK_Parties_CustomActions_PartyId_Parties_Id
GO
DROP TABLE dbo.Parties
GO
EXECUTE sp_rename N'dbo.Tmp_Parties', N'Parties', 'OBJECT' 
GO
ALTER TABLE dbo.Parties ADD CONSTRAINT
	PK_Parties PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE UNIQUE NONCLUSTERED INDEX IX_Parties_PartyName ON dbo.Parties
	(
	PartyName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Parties_CustomActions ADD CONSTRAINT
	FK_Parties_CustomActions_PartyId_Parties_Id FOREIGN KEY
	(
	PartyId
	) REFERENCES dbo.Parties
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Parties_CustomActions SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Groups ADD CONSTRAINT
	FK_Groups_Id_Parties_Id FOREIGN KEY
	(
	Id
	) REFERENCES dbo.Parties
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Groups SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	FK_Users_Id_Parties_Id FOREIGN KEY
	(
	Id
	) REFERENCES dbo.Parties
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
");
        }
    }
}
